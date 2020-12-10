﻿using System.Threading.Tasks;

using Elecritic.Database;
using Elecritic.Models;

using Microsoft.AspNetCore.Components;

namespace Elecritic.Pages {

    public partial class CategoriesCategory {

        [Parameter]
        public int CategoryId { get; set; }

        /// <summary>
        /// Determines if the passed parameter <see cref="CategoryId"/> exists.
        /// It's initialized to <c>true</c> so when the page is loading, the error message isn't immediately showed.
        /// </summary>
        private bool IsValidCategoryId { get; set; } = true;

        //Service
        [Inject]
        private CategoryProductsContext CategoryProductsContext { get; set; }

        private Category Category { get; set; }

        /// <summary>
        /// Display of the category name in plural.
        /// </summary>
        private string CategoryNameDisplay {
            get {
                switch (Category.Name) {
                    case "Laptop": {
                        return "Laptops";
                    }
                    case "TV": {
                        return "Televisiones";
                    }
                    case "Celular": {
                        return "Celulares";
                    }
                    default: {
                        return "";
                    }
                }
            }
        }

        /// <summary>
        /// Based on the Category received as parameter this method will get the corresponding products.
        /// </summary>
        protected override async Task OnInitializedAsync() {
            Category = await CategoryProductsContext.GetCategoryWithProductsAsync(CategoryId, 20);
            IsValidCategoryId = Category.Id != 0;
        }
    }
}