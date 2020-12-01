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

        
        public void CheckLogged() {
            int userId = UserService.LoggedUser.Id;
            if(userId != 0) {
                UserLogged = true;
                this.StateHasChanged();
            }
        }
        
        protected override void OnAfterRender(bool firstRender) {
            CheckLogged();
            if (firstRender) {
                // Do work to load page data and set properties
                CheckLogged();
            }
            
        }
        

    }
}
