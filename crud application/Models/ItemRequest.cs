namespace crud_application.Models
{
    public class ItemRequest
    {
        public string? Id { get; set; }

        public string Name { get; set; }

        public string SubName { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public float UnitPrice { get; set; }

        public string BestSelling { get; set; }

        public string NewReleased { get; set; }

        public List<string>? ImageUrl { get; set; }

        public List<IFormFile>? ImageFile { get; set; }
    }
}
