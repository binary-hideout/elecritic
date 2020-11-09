﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elecritic.Models {
    public class ReviewService {
        private static readonly string[] FakeReviews = new[] {
            "carousel-images/huawei.jpg", "carousel-images/iphone.png", "carousel-images/lg.jpg",
            "carousel-images/motorola.jpg", "carousel-images/samsung.jpg", "carousel-images/xiaomi.jpg",
            "carousel-images/alienware.jpg","carousel-images/laptop_hp.png", "carousel-images/lenovo.jpg"
        };
        public static int id = 0;
        public Task<Product[]> GetRandomProductsAsync(DateTime startDate) {
            id++;
            var rng = new Random();
            return Task.FromResult(Enumerable.Range(1, 20).Select(index => new Product {
                Id = id,
                Category = "1",
                Company = "Apple",
                Name = "Celular",
                Description = "Buen estado",
                ImagePath = ImagesPath[rng.Next(ImagesPath.Length)],
                ReleaseDate = startDate.AddDays(index),
            }).ToArray());
        }
    }
}