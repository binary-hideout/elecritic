using System.Threading.Tasks;

using Elecritic.Models;

using MediatR;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Elecritic.Features.Users.Pages {
    public partial class Profile {
        [Inject]
        private IMediator Mediator { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthStateTask { get; set; }

        private Queries.Details.UserDto LoggedInUser { get; set; }

        private bool IsLoading { get; set; }

        protected override async Task OnInitializedAsync() {
            IsLoading = true;

            var authState = await AuthStateTask;
            if (authState.User.Identity.IsAuthenticated) {
                var user = new User(authState.User);
                LoggedInUser = await Mediator.Send(new Queries.Details.Query { UserId = user.Id });
            }
            else {
                NavigationManager.NavigateTo("/login");
            }

            IsLoading = false;
        }
    }
}