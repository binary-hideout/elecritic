using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Elecritic.Models;
using Elecritic.Services;

namespace Elecritic.Helpers {
    public static class FuzzyLogic {

        public static List<Product> GetRecommended(List<Product> userFavorites, List<Product> products, int totalRecommendedItems) {
            // Get favorites
            // Count % of favorite's categories
            // Fetch DB items with 4+ stars and within the same % of categories in favorites
            // PROFIT $$$
            List<Product> recommendedProducts = new List<Product>();
            
            float spPercentages = userFavorites.Count(p => p.Category.Id == 3) / userFavorites.Count;
            float tvPercentages = userFavorites.Count(p => p.Category.Id == 2) / userFavorites.Count;
            float lpPercentages = userFavorites.Count(p => p.Category.Id == 1) / userFavorites.Count;

            int recommendedSmartphones = (int)spPercentages * totalRecommendedItems;
            int recommendedTv = (int)tvPercentages * totalRecommendedItems;
            int recommendedLaptops = (int)lpPercentages * totalRecommendedItems;

            products = ProductService.GetRandomProductNotAsync().ToList();

            return recommendedProducts;
        }

    }
}
