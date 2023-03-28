using AutoMapper;
using BarcodeAPI.Entities;
using BarcodeAPI.Exceptions;
using BarcodeAPI.Models;
using Flurl;
using Flurl.Http;
using Microsoft.EntityFrameworkCore;

namespace BarcodeAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductsDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(ProductsDbContext dbContext, IMapper mapper, ILogger<ProductService> logger)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _logger = logger;
        }

        private async Task<OpenFoodApiProductRequestModel> GetProductFromAPI(string productEAN)
        {
            try
            {
                var result = await $"https://openfoodfacts.org/api/v3/product/{productEAN}"
                    .SetQueryParams(new
                    {
                        fields = "product_name,brands"
                    })
                    .GetAsync()
                    .ReceiveJson<OpenFoodApiProductRequestModel>();
                return result;
            }
            catch (FlurlHttpException ex)
            {
                return null;
            }
        }

        private Product ProductFromDto(ProductDto dto)
        {
            var isBrandInDb = _dbContext.Brands.Any(b => b.Name == dto.BrandName);

            if (!isBrandInDb)
            {
                return new Product()
                {
                    Name = dto.Name,
                    Price = null,
                    Ean = dto.Ean,
                    Brand = new Brand()
                    {
                        Name = dto.BrandName,
                        Description = null
                    }
                };
            }
            else
            {
                var brand = _dbContext.Brands.FirstOrDefault(b => b.Name == dto.BrandName);
                return new Product()
                {
                    Name = dto.Name,
                    Price = null,
                    Ean = dto.Ean,
                    Brand = brand
                };
            }
        }

        private async Task AddMissingProductToDb(ProductDto dto)
        {
            var product = ProductFromDto(dto);
            _dbContext.AddRangeAsync(product);
            _dbContext.SaveChanges();
        }

        private async Task<Product> GetProductFromDb(string productEAN)
        {
            var product = _dbContext.Products
                .Include(p => p.Brand)
                .FirstOrDefault(p => p.Ean.Equals(productEAN));
            return product;
        }

        public async Task<ProductDto> GetProduct(string productEAN)
        {
            var resultFromProductDb = await GetProductFromDb(productEAN);
            if (resultFromProductDb == null)
            {
                _logger.LogInformation($"Ean {productEAN} missing from DB");
                var resultFromOpenFoodApi = await GetProductFromAPI(productEAN);
                if (resultFromOpenFoodApi == null)
                {
                    throw new NotFoundException($"Product {productEAN} not found in DB and OpenFoodAPI");
                }
                _logger.LogInformation($"Ean {productEAN} found in API name: {resultFromOpenFoodApi.product.product_name}");
                var resultFromApi = _mapper.Map<ProductDto>(resultFromOpenFoodApi);

                //Product nie jest nullem a nie ma go w db wiec wykonaj taska ktory doda produkt do DB
                AddMissingProductToDb(resultFromApi);
                _logger.LogInformation($"Product {resultFromApi.Name} was added to DB");

                return resultFromApi;
            }
            var productDto = _mapper.Map<ProductDto>(resultFromProductDb);
            _logger.LogInformation($"Product {productDto.Name} found in DB");

            return productDto;
        }
    }
}