namespace BarcodeAPI.Models
{
    public class ProductDto
    {
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public string? BrandName { get; set; }
        public string Ean { get; set; }
    }
}