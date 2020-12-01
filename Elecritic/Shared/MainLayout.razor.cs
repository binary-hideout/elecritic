using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Elecritic.Database;
using Elecritic.Models;
using Elecritic.Services;

using Microsoft.AspNetCore.Components;

namespace Elecritic.Shared {
    public partial class MainLayout {

        [Inject]
        private UserService UserService { get; set; }

        public bool UserLogged { get; set; }
        protected override async Task OnInitializedAsync() {
            var userId = UserService.LoggedUser.Id;
            if (userId != 0) {
                
                UserLogged = true;
                this.StateHasChanged();
            }
        }

    }
}
