

namespace crud_application.Models
{
    public class CategoryRequest
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}
