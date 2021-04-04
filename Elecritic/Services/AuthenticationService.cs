using System.Security.Claims;
using System.Threading.Tasks;

using Blazored.LocalStorage;

using Elecritic.Models;

using Microsoft.AspNetCore.Components.Authorization;

namespace Elecritic.Services {

    /// <summary>
    /// Custom authentication state provider to authorize users.
    /// </summary>
    public class AuthenticationService : AuthenticationStateProvider {

        /// <summary>
        /// Current scoped logged-in user.
        /// </summary>
        public User LoggedUser { get; set; }
        
        /// <summary>
        /// Current scoped authentication state.
        /// </summary>
        private AuthenticationState AuthState { get; set; }

        /// <summary>
        /// Name of the key that holds the token string in browser's local storage.
        /// </summary>
        private const string USER_TOKEN_KEY = "userToken";

        /// <summary>
        /// Injected service to read and write browser's local storage.
        /// </summary>
        private readonly ILocalStorageService _localStorage;

        /// <summary>
        /// Injected service to manage JWT tokens.
        /// </summary>
        private readonly TokenService _tokenService;

        public AuthenticationService(ILocalStorageService localStorage, TokenService tokenService) {
            _localStorage = localStorage;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Gets the current authentication state when it's needed to authorize views or actions.
        /// </summary>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync() {
            if (AuthState is null || LoggedUser is null) {
                string token = await _localStorage.GetItemAsync<string>(USER_TOKEN_KEY);
                bool isTokenStored = !string.IsNullOrEmpty(token);
                var identity = isTokenStored && _tokenService.IsValid(token) ?
                    new ClaimsIdentity(_tokenService.GetClaims(token), "login") :
                    new ClaimsIdentity();

                var claimsPrincipal = new ClaimsPrincipal(identity);
                LoggedUser = isTokenStored ? new User(claimsPrincipal) : new User { Id = 0 };
                AuthState = new AuthenticationState(claimsPrincipal);
            }

            return AuthState;
        }

        /// <summary>
        /// Logs in a user and writes its token to browser's local storage.
        /// </summary>
        /// <param name="user">User logging in.</param>
        public async Task LogIn(User user) {
            string token = _tokenService.CreateToken(user);
            await _localStorage.SetItemAsync(USER_TOKEN_KEY, token);
            LoggedUser = user;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        /// <summary>
        /// Logs out a user and deletes its token from browser's local storage.
        /// </summary>
        /// <returns></returns>
        public async Task LogOut() {
            await _localStorage.RemoveItemAsync(USER_TOKEN_KEY);
            LoggedUser = null;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
