using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

using Elecritic.Models;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Elecritic.Services {

    /// <summary>
    /// Service class to manage JWT security tokens that helps in users authentication.
    /// </summary>
    public class TokenService {

        /// <summary>
        /// Base URI of the website.
        /// </summary>
        private string BaseUri { get; }

        /// <summary>
        /// Injected configuration to read application settings and secrets.
        /// </summary>
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration) {
            _configuration = configuration;

            BaseUri = _configuration.GetValue<string>(nameof(BaseUri));
        }

        /// <summary>
        /// Creates a new JWT security token containing <see cref="Claim"/>s based on <paramref name="user"/>.
        /// </summary>
        /// <param name="user">User who is logging in.</param>
        /// <returns>The encoded token as <c>string</c>.</returns>
        public string CreateToken(User user) {
            // create claims based on user's basic data
            var claims = new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.RoleId.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(claims),
                Issuer = BaseUri,
                Audience = BaseUri,
                Expires = DateTime.UtcNow.AddMonths(1),
                SigningCredentials = new SigningCredentials(GetSecurityKey(), SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Determines if the encoded <paramref name="token"/> is still valid.
        /// </summary>
        /// <param name="token">Encoded <c>string</c> of a JWT security token.</param>
        /// <returns><c>true</c> if the token is still valid, otherwise <c>false</c>.</returns>
        public bool IsValid(string token) {
            try {
                new JwtSecurityTokenHandler().ValidateToken(
                    token,
                    new TokenValidationParameters {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = GetSecurityKey(),
                        ValidIssuer = BaseUri,
                        ValidateIssuer = true,
                        ValidAudience = BaseUri,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        RequireExpirationTime = true
                    },
                    out var _);

                return true;
            }
            catch (SecurityTokenException) {
                return false;
            }
        }

        /// <summary>
        /// Retrieves the security key from app's secrets.
        /// </summary>
        /// <returns>A <see cref="SymmetricSecurityKey"/> object created from the key's value.</returns>
        private SymmetricSecurityKey GetSecurityKey() {
            var keyBytes = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Key"));
            return new SymmetricSecurityKey(keyBytes);
        }

        /// <summary>
        /// Gets the <see cref="Claim"/>s from <paramref name="jwtEncodedString"/>.
        /// </summary>
        /// <param name="jwtEncodedString">The JWT security token <c>string</c> to get the claims from.</param>
        /// <returns>An array of <paramref name="jwtEncodedString"/>'s claims.</returns>
        public Claim[] GetClaims(string jwtEncodedString) {
            return GetJwtSecurityToken(jwtEncodedString).Claims.ToArray();
        }

        /// <summary>
        /// Gets the actual token object from <paramref name="jwtEncodedString"/>.
        /// </summary>
        /// <param name="jwtEncodedString">Encoded string of a token.</param>
        /// <returns>An instance of <see cref="JwtSecurityToken"/>.</returns>
        public JwtSecurityToken GetJwtSecurityToken(string jwtEncodedString) {
            return new JwtSecurityToken(jwtEncodedString);
        }
    }
}
