using CloudinaryDotNet.Actions;
using crud_application.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace crud_application.service
{
    public class ItemService
    {
       

        private readonly IMongoCollection<ItemModel> _Item;

        private readonly IMongoCollection<CategoryModel> _categoryCollection;

        private readonly CloudinaryService _cloudinaryService;

        private readonly ILogger<ItemService> _logger;
        public ItemService(CloudinaryService cloudinaryService, ILogger<ItemService> logger)
        {
            

            var client = new MongoClient("mongodb://localhost:27017/");
            var database = client.GetDatabase("CarRent");

            _Item = database.GetCollection<ItemModel>("ItemCollection");
            _categoryCollection = database.GetCollection<CategoryModel>("CategoryCollection");

            _cloudinaryService = cloudinaryService;

            _logger = logger;
        }

        public async Task<List<ItemWithCategory>> GetItems()
        {
            var pipeline = new[]
        {
            new BsonDocument("$lookup",
                new BsonDocument
                {
                    { "from", "CategoryCollection" },
                    { "localField", "Category" },
                    { "foreignField", "_id" },
                    { "as", "CategoryDetails" }
                }),
            new BsonDocument("$unwind",
                new BsonDocument
                {
                    { "path", "$CategoryDetails" },
                    { "preserveNullAndEmptyArrays", true }
                }),
            new BsonDocument("$project",
                new BsonDocument
                {
                    { "Id", 1 },
                    { "Name", 1 },
                    { "SubName", 1 },
                    { "Category", 1 },
                    { "CategoryName", "$CategoryDetails.Name" },
                    { "Description", 1 },
                    { "Quantity", 1 },
                    { "UnitPrice", 1 },
                    { "BestSelling", 1 },
                    { "NewReleased", 1 },
                    { "ImageUrl", 1 }
                })
        };
            return await _Item.Aggregate<ItemWithCategory>(pipeline).ToListAsync();
           // return await _Item.Find(_ => true).ToListAsync();
        }

        public async Task<ItemModel> GetItem(string id)
        {
            return await _Item.Find(p => p.Id == id).FirstOrDefaultAsync();

        }

        public async Task<List<ItemWithCategory>> GetCategoryWiseItems(string id)
        {

            var objectId = new ObjectId(id);
            var pipeline = new[]
   {
        new BsonDocument("$match",
            new BsonDocument("Category", objectId)),
        new BsonDocument("$lookup",
            new BsonDocument
            {
                { "from", "CategoryCollection" },
                { "localField", "Category" },
                { "foreignField", "_id" },
                { "as", "CategoryDetails" }
            }),
        new BsonDocument("$unwind",
            new BsonDocument
            {
                { "path", "$CategoryDetails" },
                { "preserveNullAndEmptyArrays", true }
            }),
        new BsonDocument("$project",
            new BsonDocument
            {
                { "Id", 1 },
                { "Name", 1 },
                { "SubName", 1 },
                { "Category", 1 },
                { "CategoryName", "$CategoryDetails.Name" },
                { "Description", 1 },
                { "Quantity", 1 },
                { "UnitPrice", 1 },
                { "BestSelling", 1 },
                { "NewReleased", 1 },
                { "ImageUrl", 1 }
            })
    };

            return await _Item.Aggregate<ItemWithCategory>(pipeline).ToListAsync();

        }
        public async Task<List<ItemWithCategory>> GetNewReleaseItems()
        {
            var pipeline = new[]
   {
        new BsonDocument("$match",
            new BsonDocument("NewReleased", "true")),
        new BsonDocument("$lookup",
            new BsonDocument
            {
                { "from", "CategoryCollection" },
                { "localField", "Category" },
                { "foreignField", "_id" },
                { "as", "CategoryDetails" }
            }),
        new BsonDocument("$unwind",
            new BsonDocument
            {
                { "path", "$CategoryDetails" },
                { "preserveNullAndEmptyArrays", true }
            }),
        new BsonDocument("$project",
            new BsonDocument
            {
                { "Id", 1 },
                { "Name", 1 },
                { "SubName", 1 },
                { "Category", 1 },
                { "CategoryName", "$CategoryDetails.Name" },
                { "Description", 1 },
                { "Quantity", 1 },
                { "UnitPrice", 1 },
                { "BestSelling", 1 },
                { "NewReleased", 1 },
                { "ImageUrl", 1 }
            })
    };

            return await _Item.Aggregate<ItemWithCategory>(pipeline).ToListAsync();

        }

        public async Task<List<ItemWithCategory>> GetBestSellingItems()
        {
            var pipeline = new[]
   {
        new BsonDocument("$match",
            new BsonDocument("BestSelling", "true")),
        new BsonDocument("$lookup",
            new BsonDocument
            {
                { "from", "CategoryCollection" },
                { "localField", "Category" },
                { "foreignField", "_id" },
                { "as", "CategoryDetails" }
            }),
        new BsonDocument("$unwind",
            new BsonDocument
            {
                { "path", "$CategoryDetails" },
                { "preserveNullAndEmptyArrays", true }
            }),
        new BsonDocument("$project",
            new BsonDocument
            {
                { "Id", 1 },
                { "Name", 1 },
                { "SubName", 1 },
                { "Category", 1 },
                { "CategoryName", "$CategoryDetails.Name" },
                { "Description", 1 },
                { "Quantity", 1 },
                { "UnitPrice", 1 },
                { "BestSelling", 1 },
                { "NewReleased", 1 },
                { "ImageUrl", 1 }
            })
    };

            return await _Item.Aggregate<ItemWithCategory>(pipeline).ToListAsync();

        }

        public async Task<ItemModel> CreateItem(ItemRequest item)
        {
            
                ImageUploadResult uploadResult = null;
                List<string> imageUrls = new List<string>();
                foreach (var file in item.ImageFile)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        uploadResult = await _cloudinaryService.UploadImageAsync(stream, file.FileName);
                        imageUrls.Add(uploadResult?.SecureUrl?.ToString());
                    }
                }

                ItemModel savingItem = new ItemModel
                {
                    Name = item.Name,
                    Description = item.Description,
                    SubName = item.SubName,
                    Category = item.Category,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity,
                    BestSelling =item.BestSelling,
                    NewReleased=item.NewReleased,
                    ImageUrl = imageUrls
                };

                await _Item.InsertOneAsync(savingItem);

                return savingItem;
            
        }

        public async Task UpdateItem(string id, ItemRequest item)
        {
            var existingItem = await _Item.Find(p => p.Id == id).FirstOrDefaultAsync();
            if (existingItem == null)
            {
                return;
            }


            foreach (var existimage in existingItem.ImageUrl)
            {
                if (!item.ImageUrl.Contains(existimage))
                {
                    var publicId = ExtractPublicIdFromUrl(existimage);
                    if (!string.IsNullOrEmpty(publicId))
                    {
                        // Delete the image from Cloudinary
                        var result = await _cloudinaryService.DestroyAsync(publicId); // Changed line
                        if (result.Result != "ok" ||  result.Result != "not found")
                        {
                          //  throw new Exception("Failed to delete image from Cloudinary");
                        }
                    }
                }

            }

            List<string> imageUrls = new List<string>();
            imageUrls = item.ImageUrl;

            ImageUploadResult uploadResult = null;
            if (item.ImageFile?.Count > 0)
            {
                foreach (var file in item.ImageFile)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        uploadResult = await _cloudinaryService.UploadImageAsync(stream, file.FileName);
                        imageUrls.Add(uploadResult?.SecureUrl?.ToString());
                    }
                }
            }


            existingItem.Name = item.Name;
            existingItem.SubName = item.SubName;
            existingItem.Description = item.Description;
            existingItem.UnitPrice = item.UnitPrice;
            existingItem.Category = item.Category;
            existingItem.Quantity = item.Quantity;
            existingItem.NewReleased = item.NewReleased;
            existingItem.BestSelling = item.BestSelling;
            existingItem.ImageUrl = imageUrls;
            await _Item.ReplaceOneAsync(p => p.Id == id, existingItem);
        }

        public async Task DeleteItem(string id)
        {
            var existingItem = await _Item.Find(p => p.Id == id).FirstOrDefaultAsync();
            if (existingItem == null)
            {
                _logger.LogInformation("Item not found");
                return;
            }
            _logger.LogInformation("Item  found");
            if (existingItem.ImageUrl != null && existingItem.ImageUrl.Any())
            {
                foreach (var existingImageUrl in existingItem.ImageUrl)
                {
                    var publicId = ExtractPublicIdFromUrl(existingImageUrl);
                    if (!string.IsNullOrEmpty(publicId))
                    {
                        _logger.LogInformation($"Attempting to delete image with public ID: {publicId}");
                       
                        try
                        {
                            if (!string.IsNullOrEmpty(publicId))
                            {
                                // Delete the image from Cloudinary
                                var result = await _cloudinaryService.DestroyAsync(publicId);
                                _logger.LogInformation($"Attempting to delete image with public ID: {result.Result}"); // Changed line
                                if (result.Result != "ok" || result.Result != "not found")
                                {
                                   throw new Exception("Failed to delete image from Cloudinary");
                                }
                            }
                           
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Exception occurred while deleting image from Cloudinary: {ex.Message}");
                            throw;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Extracted public ID is null or empty");
                    }
                }
            }

            await _Item.DeleteOneAsync(p => p.Id == id);
            Console.WriteLine("Item deleted successfully");
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
}
