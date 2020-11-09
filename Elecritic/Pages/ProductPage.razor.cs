using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elecritic.Models;
using Microsoft.AspNetCore.Components;

namespace Elecritic.Pages {
    public partial class ProductPage {

        [Parameter]
        public string ProductId { get; set; }
    }
}
