namespace BarcodeAPI.Models
{
    public class TokenResponseModel
    {
        public string Token { get; set; }
        public int StatusCode { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime IssuedAt { get; set; } = DateTime.Now;
    }
}