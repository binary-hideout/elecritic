using System.Collections.Generic;
using System.Threading.Tasks;

using Elecritic.Features.Products.Queries;

using MediatR;

using Microsoft.AspNetCore.Components;

namespace Elecritic.Features.Products.Pages {
    public partial class Categories {
        [Inject]
        private IMediator Mediator { get; set; }
        private List<ListCategories.CategoryDto> CategoriesList { get; set; }
        private bool IsLoading { get; set; }

        public Categories() {
            CategoriesList = new List<ListCategories.CategoryDto>();
            IsLoading = false;
        }

        protected override async Task OnInitializedAsync() {
            IsLoading = true;

            CategoriesList = (await Mediator.Send(
                    new ListCategories.Query()))
                .Categories;

            IsLoading = false;
        }
    }
}
