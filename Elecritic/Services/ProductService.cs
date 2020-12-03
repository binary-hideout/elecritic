using System;
using System.Linq;
using System.Threading.Tasks;

using Elecritic.Models;

namespace Elecritic.Services {
    /// <summary>
    /// This class is a service, declared as such in Startup.cs, which returns 20 objects of type Product,
    /// it has a random variable to pick a random image as well.
    /// </summary>
    public class ProductService {

        private static readonly string[] ImagesPath = new[] {
            "carousel-images/huawei.jpg", "carousel-images/iphone.png", "carousel-images/lg.jpg",
            "carousel-images/motorola.jpg", "carousel-images/samsung.jpg", "carousel-images/xiaomi.jpg",
            "carousel-images/alienware.jpg","carousel-images/laptop_hp.png", "carousel-images/lenovo.jpg"
        };

        public static Product[] GetRandomProductNotAsync() {
            var rng = new Random();
            return Enumerable.Range(1, 20).Select(index => new Product {
                Id = index,
                Category = new Category {
                    Id = rng.Next(1, 4)
                },
                Company = new Company {
                    Name = "Apple"
                },
                Name = "Celular",
                Description = "Buen estado",
                ImagePath = ImagesPath[rng.Next(ImagesPath.Length)],
                ReleaseDate = DateTime.Now.AddDays(index),
            }).ToArray();
        }

        public Task<Product[]> GetRandomProductsAsync(DateTime startDate) {
            var rng = new Random();
            return Task.FromResult(Enumerable.Range(1, 20).Select(index => new Product {
                Id = index,
                Category = new Category {
                    Id = rng.Next(1, 4)
                },
                Company = new Company {
                    Name = "Apple"
                },
                Name = "Celular",
                Description = "Buen estado",
                ImagePath = ImagesPath[rng.Next(ImagesPath.Length)],
                ReleaseDate = startDate.AddDays(index),
            }).ToArray());
        }
        
    }
}
