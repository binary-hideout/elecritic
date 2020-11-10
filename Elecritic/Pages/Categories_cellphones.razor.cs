using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elecritic.Models;
using Microsoft.AspNetCore.Components;

namespace Elecritic.Pages {
    public partial class Categories_cellphones {
        [Inject]
        public ProductService ProductService { get; set; }

        private Product[] Products { get; set; }

        protected override async Task OnInitializedAsync() {
            Products = await ProductService.GetRandomProductsAsync(DateTime.Now);
        }
    }
}
