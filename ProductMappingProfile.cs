using AutoMapper;
using BarcodeAPI.Entities;
using BarcodeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BarcodeAPI
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<OpenFoodApiProductRequestModel, ProductDto>()
                .ForMember(m => m.Name, c => c.MapFrom(s => s.product.product_name))
                .ForMember(m => m.Ean, c => c.MapFrom(s => s.code))
                .ForMember(m => m.BrandName, c => c.MapFrom(s => s.product.brands));
            //.ForMember(m => m.Price, c => c.MapFrom(s => 0.0));

            CreateMap<Product, ProductDto>()
                .ForMember(m => m.BrandName, c => c.MapFrom(s => s.Brand.Name));
        }
    }
}