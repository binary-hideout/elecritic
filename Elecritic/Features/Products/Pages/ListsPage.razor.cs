using System.Collections.Generic;
using System.Threading.Tasks;

using Elecritic.Features.Products.Queries;

using MediatR;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace Elecritic.Features.Products.Pages {
    public partial class ListsPage {
        public int CategoryId { get; set; }
        public int SkipNumber { get; set; }
        public int TakeNumber { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        private IMediator Mediator { get; set; }

        /// <summary>
        /// Determines if the passed parameter <see cref="CategoryId"/> exists.
        /// It's initialized to <c>true</c> so when the page is loading, the error message isn't immediately showed.
        /// </summary>
        private bool IsValidCategoryId { get; set; }
        private string InvalidMessage { get; set; }

        private List<Lists.ProductDto> Products { get; set; }

        private bool IsLoading { get; set; }

        public ListsPage() {
            CategoryId = 0;
            SkipNumber = 0;
            TakeNumber = 20;

            IsValidCategoryId = true;
            InvalidMessage = "";
            IsLoading = false;
        }

        /// <summary>
        /// Based on the Category received as parameter this method will get the corresponding products.
        /// </summary>
        protected override async Task OnInitializedAsync() {
            IsLoading = true;
            await Task.Delay(1);

            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue(nameof(CategoryId), out var categoryId)) {
                CategoryId = int.Parse(categoryId);
            }
            else {
                IsValidCategoryId = false;
                InvalidMessage = "Especifica un número de categoría para filtrar los productos.";
            }

            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("skip", out var skipNumber)) {
                SkipNumber = int.Parse(skipNumber);
            }

            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("take", out var takeNumber)) {
                TakeNumber = int.Parse(takeNumber);
            }

            Products = (await Mediator.Send(
                    new Lists.Query {
                        CategoryId = CategoryId,
                        SkipNumber = SkipNumber,
                        TakeNumber = TakeNumber
                    }))
                .Products;
            IsValidCategoryId = Products.Count > 0;

            IsLoading = false;
        }
    }
}