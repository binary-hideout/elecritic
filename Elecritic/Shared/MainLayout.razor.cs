﻿using Elecritic.Services;

using Microsoft.AspNetCore.Components;

namespace Elecritic.Shared {
    public partial class MainLayout : LayoutComponentBase {

        [Inject]
        private UserService UserService { get; set; }

        public bool UserIsLogged { get; set; }

        /// <summary>
        /// Method calls for UserService to know if the userId is different from 0 as that means user is logged
        /// </summary>
        public void CheckLogged() {
            UserIsLogged = UserService.LoggedUser.Id != 0;
            StateHasChanged();
        }
       
        /// <summary>
        /// Checks if the user has changed its state (logged or not) each time the page is refreshed
        /// </summary>
        /// <param name="firstRender"> This bool as the name implies is true when the page is first rendered</param>
        protected override void OnAfterRender(bool firstRender) {
            CheckLogged();
        }
    }
}