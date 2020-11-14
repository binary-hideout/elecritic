using System;
using System.Threading.Tasks;

using Elecritic.Models;
using Elecritic.Services;
using Microsoft.AspNetCore.Components;

namespace Elecritic.Pages {
    public partial class CategoriesCategory {
        [Parameter]
        public string Category { get; set; }

        //Service
        [Inject]
        public ProductService ProductService { get; set; }

        private Product[] Products { get; set; }

        /// <summary>
        /// Based on the Category received as parameter this method will get the corresponding products
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync() {
            Products = await ProductService.GetRandomProductsAsync(DateTime.Now);
        }
    }
}
