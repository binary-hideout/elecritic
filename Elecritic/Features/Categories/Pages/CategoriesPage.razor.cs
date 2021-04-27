using System.Collections.Generic;
using System.Threading.Tasks;

using Elecritic.Features.Categories.Queries;

using MediatR;

using Microsoft.AspNetCore.Components;

namespace Elecritic.Features.Categories.Pages {
    public partial class CategoriesPage {
        [Inject]
        private IMediator Mediator { get; set; }
        private List<Lists.CategoryDto> Categories { get; set; }
        private bool IsLoading { get; set; }

        public CategoriesPage() {
            Categories = new List<Lists.CategoryDto>();
            IsLoading = false;
        }

        protected override async Task OnInitializedAsync() {
            IsLoading = true;

            Categories = (await Mediator.Send(new Lists.Query()))
                .Categories;

            IsLoading = false;
        }
    }
}
