using System;
using System.Collections.Generic;
using System.Linq;

using Elecritic.Models;
using Elecritic.Features.Products.Queries;

namespace Elecritic.Features.Products.Modules {

    /// <summary>
    /// Static helper with a method that implements a fuzzy logic algorithm.
    /// </summary>
    public static class FuzzyLogic {

        /// <summary>
        /// Runs a fuzzy logic algorithm to recommend products to a certain user, according to its <paramref name="userFavorites"/>.
        /// </summary>
        /// <param name="userFavorites">Favorite products of a user.</param>
        /// <param name="products">Products from which the recommended ones will be filtered.</param>
        /// <param name="totalRecommendedItems">Size of the recommended products' list.</param>
        /// <returns>A list that contains <paramref name="totalRecommendedItems"/> <see cref="Product"/>s.</returns>
        public static List<List.ProductDto> RecommendProducts(
            List<List.ProductDto> userFavorites,
            List<List.ProductDto> products,
            int totalRecommendedItems) {
            // select IDs of products others than user favorites
            var noFavoritesIds = products
                .Select(p => p.Id)
                .Except(userFavorites
                    .Select(p => p.Id));

            // filter products by above IDs
            products = products
                .Where(p => noFavoritesIds.Contains(p.Id))
                .ToList();

            // TODO: also remove products that user has reviewed

            var recommendedProducts = new List<List.ProductDto>();

            // TODO: fetch categories IDs from database
            // for each category ID (1 - 3)
            for (int i = 1; i < 4; i++) {
                // number of favorite products of same category
                int number = userFavorites.Count(p => p.CategoryId == i);
                float percentage = (float)number / (float)userFavorites.Count;
                // number of products of current category to be recommended
                int total = (int)Math.Round(percentage * totalRecommendedItems);

                var tempProducts = products
                    // filter products of current category
                    .Where(p => p.CategoryId == i)
                    // sort from highest to lowest rating
                    .OrderByDescending(p => p.AverageRating)
                    .Take(total);

                // add filtered products of current category to recommended ones
                recommendedProducts.AddRange(tempProducts);
            }

            return recommendedProducts
                // sort from highest to lowest rating
                .OrderByDescending(p => p.AverageRating)
                .ToList();
        }
    }
}