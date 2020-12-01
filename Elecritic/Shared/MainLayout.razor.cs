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

        private bool UserLogged = false;
        protected override void OnInitialized() {
            var user = UserService.LoggedUser.Id;
            if (user != 0) {
                UserLogged = true;
            }
        }
    }
}
