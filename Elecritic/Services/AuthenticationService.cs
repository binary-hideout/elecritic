using System.Security.Claims;
using System.Threading.Tasks;

using Blazored.LocalStorage;

using Elecritic.Models;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

namespace Elecritic.Services {

    /// <summary>
    /// Custom authentication state provider to authorize users.
    /// </summary>
    public class AuthenticationService : AuthenticationStateProvider {

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

        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(ILocalStorageService localStorage, TokenService tokenService, ILogger<AuthenticationService> logger) {
            _localStorage = localStorage;
            _tokenService = tokenService;
            _logger = logger;
        }

        /// <summary>
        /// Gets the current authentication state when it's needed to authorize views or actions.
        /// </summary>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync() {
            _logger.LogInformation("Getting authentication state.");

            string token = await _localStorage.GetItemAsync<string>(USER_TOKEN_KEY);

            _logger.LogInformation($"Token retrieved from local storage: {token}");

            var identity = !string.IsNullOrEmpty(token) && _tokenService.IsValid(token) ?
                new ClaimsIdentity(_tokenService.GetClaims(token), "login") :
                new ClaimsIdentity();

            var claimsPrincipal = new ClaimsPrincipal(identity);
            var authState = new AuthenticationState(claimsPrincipal);

            _logger.LogInformation("Auth state created: {@authState}", authState);

            return authState;
        }

        /// <summary>
        /// Logs in a user and writes its token to browser's local storage.
        /// </summary>
        /// <param name="user">User logging in.</param>
        public async Task LogIn(User user) {
            _logger.LogInformation($"Logging in {user.Id}, {user.Username}.");

            string token = _tokenService.CreateToken(user);
            await _localStorage.SetItemAsync(USER_TOKEN_KEY, token);

            _logger.LogInformation($"Token created and saved in local storage: {token}.");

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        /// <summary>
        /// Logs out a user and deletes its token from browser's local storage.
        /// </summary>
        /// <returns></returns>
        public async Task LogOut() {
            _logger.LogInformation("Logging out current user.");

            await _localStorage.RemoveItemAsync(USER_TOKEN_KEY);

            _logger.LogInformation("Token deleted from local storage.");

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
