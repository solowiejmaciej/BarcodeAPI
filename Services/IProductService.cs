using BarcodeAPI.Models;

namespace BarcodeAPI.Services
{
    public interface IProductService
    {
        //Task<OpenFoodApiProductRequestModel> GetProductFromAPI(string productEAN);

        Task<ProductDto> GetProduct(string productEAN);

        //Task<Product> GetProductFromDb(string productEAN);
    }
}