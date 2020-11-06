using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elecritic.Models;
using Microsoft.AspNetCore.Components;

namespace Elecritic.Pages {
    public partial class Index {
        [Inject]
        public ProductService productService { get; set; }

        private Product[] Products { get; set; }

        protected override async Task OnInitializedAsync() {
            Products = await productService.GetForecastAsync(DateTime.Now);
        }
    }
}
