using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using crud_application.Models;
using crud_application.service;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;


public class CategoryService
    {
        private readonly IMongoCollection<CategoryModel> _Category;
        private readonly CloudinaryService _cloudinaryService;

    public CategoryService(CloudinaryService cloudinaryService)
        {
            var client = new MongoClient("mongodb://localhost:27017/");
            var database = client.GetDatabase("CarRent");
            _Category = database.GetCollection<CategoryModel>("CategoryCollection");
            _cloudinaryService = cloudinaryService;



    }
   

   



    public async Task<List<CategoryModel>> GetCategories()
        {
            return await _Category.Find(_ => true).ToListAsync();
    }

        public async Task<CategoryModel> GetCategory(string id)
        {
        return await _Category.Find(p => p.Id == id).FirstOrDefaultAsync();

    }

    public async Task<CategoryModel> CreateCategory(CategoryRequest category)

        {
        ImageUploadResult uploadResult = null;
        if (category.ImageFile != null)
        {
            using (var stream = category.ImageFile.OpenReadStream())
            {
                uploadResult = await _cloudinaryService.UploadImageAsync(stream, category.ImageFile.FileName);
            }
        }

        CategoryModel savingCategory = new CategoryModel
        {
            Name = category.Name,
            Description = category.Description,
            ImageUrl = uploadResult?.SecureUrl.ToString()
        };

        await _Category.InsertOneAsync(savingCategory);

        return savingCategory;
    }

    public async Task UpdateCategory(string id, CategoryRequest category)
    {
        var existingCategory = await _Category.Find(p => p.Id == id).FirstOrDefaultAsync();
        if (existingCategory == null)
        {
            return; 
        }

        existingCategory.Name = category.Name;
        existingCategory.Description = category.Description;

        ImageUploadResult uploadResult = null;
        if (category.ImageFile != null)
        {
            using (var stream = category.ImageFile.OpenReadStream())
            {
                uploadResult = await _cloudinaryService.UploadImageAsync(stream, category.ImageFile.FileName);
            }
            var ImageUrl = uploadResult?.SecureUrl.ToString();

            if (!string.IsNullOrEmpty(existingCategory.ImageUrl))
            {
                // Extract the public ID from the ImageUrl
                var publicId = ExtractPublicIdFromUrl(existingCategory.ImageUrl);
                if (!string.IsNullOrEmpty(publicId))
                {
                    // Delete the image from Cloudinary
                    var result = await _cloudinaryService.DestroyAsync(publicId); // Changed line
                    if (result.Result != "ok")
                    {
                        throw new Exception("Failed to delete image from Cloudinary");
                    }
                }
            }
            existingCategory.ImageUrl = ImageUrl;
        }
        else
        {
            existingCategory.ImageUrl = existingCategory.ImageUrl;

        }

        await _Category.ReplaceOneAsync(p => p.Id == id, existingCategory);
        //await _Category.ReplaceOneAsync(p => p.Id == id, category);
    }

    public async Task DeleteCategory(string id)
    {
        var category = await _Category.Find(p => p.Id == id).FirstOrDefaultAsync();
        if (category == null)
        {
            throw new Exception("Category not found");
        }

        if (!string.IsNullOrEmpty(category.ImageUrl))
        {
            // Extract the public ID from the ImageUrl
            var publicId = ExtractPublicIdFromUrl(category.ImageUrl);
            if (!string.IsNullOrEmpty(publicId))
            {
                // Delete the image from Cloudinary
                var result = await _cloudinaryService.DestroyAsync(publicId); // Changed line
                if (result.Result != "ok")
                {
                    //throw new Exception("Failed to delete image from Cloudinary");
                }
            }
        }

        await _Category.DeleteOneAsync(p => p.Id == id);
    }
    private string ExtractPublicIdFromUrl(string imageUrl)
    {
        var uri = new Uri(imageUrl);
        var segments = uri.Segments;
        // The public ID is the last segment without the file extension
        var publicIdWithExtension = segments.Last();
        var publicId = Path.GetFileNameWithoutExtension(publicIdWithExtension);
        return publicId;
    }
}

