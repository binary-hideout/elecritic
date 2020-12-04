using System;
using System.Collections.Generic;
using System.Linq;

using Elecritic.Models;

namespace Elecritic.Helpers {

    public static class FuzzyLogic {

        public static List<Product> GetRecommended(List<Product> userFavorites, List<Product> products, int totalRecommendedItems) {
            // filter  IDs of products others than user favorites
            var noFavoritesIds = products
                .Select(p => p.Id)
                .Except(userFavorites
                    .Select(p => p.Id));
            // select products from above IDs
            products = products
                .Where(p => noFavoritesIds.Contains(p.Id))
                .ToList();

            var recommendedProducts = new List<Product>();
            for (int i = 1; i < 4; i++) {
                int number = userFavorites.Count(p => p.Category.Id == i);
                float percentage = (float)number / (float)userFavorites.Count;
                int total = (int)(percentage * totalRecommendedItems);

                var tempProducts = products
                    .Where(p => p.Category.Id == i)
                    .Take(total);

                recommendedProducts.AddRange(tempProducts);
            }

            int lacks = 10 - recommendedProducts.Count;
            if (lacks > 0) {
                // filter IDs of not yet recommended products
                var noRecommendedIds = products
                    .Select(p => p.Id)
                    .Except(recommendedProducts
                        .Select(p => p.Id));
                // select remaining products from above IDs
                var remaining = products
                    .Where(p => noRecommendedIds.Contains(p.Id))
                    .ToList();

                var random = new Random();
                int[] randomIndexes = Enumerable
                    .Range(0, lacks)
                    .Select(_ => random.Next(remaining.Count))
                    .ToArray();

                var randomProducts = randomIndexes
                    .Select(i => remaining[i]);

                recommendedProducts.AddRange(randomProducts);
            }

            return recommendedProducts
                .OrderByDescending(p => p.GetAverageRating())
                .ToList();
        }

    }
}
