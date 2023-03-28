using BarcodeAPI.Entities;
using System.Net;

using BarcodeAPI.Entities;

namespace BarcodeAPI
{
    public class Seeder
    {
        private readonly ProductsDbContext _dbContext;

        public Seeder(ProductsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }

                if (!_dbContext.Products.Any())
                {
                    var products = getProducts();
                    _dbContext.Products.AddRange(products);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Product> getProducts()
        {
            var lays = new Brand()
            {
                Name = "Lays",
                Description = "Snacks for everybody!"
            };
            var products = new List<Product>()
            {
                new Product()
            {
                Name = "Coca-cola - 330 mL",
                Price = 5.3M,
                Ean = "5449000214911",
                Brand = new Brand()
                {
                    Name = "Coca cola",
                    Description = "We used to add cocaine to our drinks! and everybody loved it."
                }
            },
                new Product()
                {
                    Name = "Lays zielona cebulka - Lay's - 165g",
                    Price = 7.99M,
                    Ean = "5900259099921",
                    Brand = lays
                },
                new Product()
                {
                    Name = "Fromage - Lay's - 140g",
                    Price = 5.99M,
                    Ean = "5900259099396",
                    Brand = lays
                }
            };

            return products;
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new ()
                {
                    Name = "User"
                },
                new ()
                {
                    Name = "Admin"
                }
            };

            return roles;
        }
    }
}