namespace BarcodeAPI.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public int? BrandId { get; set; }
        public string Ean { get; set; }
        public virtual Brand Brand { get; set; }
    }
}