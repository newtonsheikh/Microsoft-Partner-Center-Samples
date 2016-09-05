/*
 * Microsoft Partner Center - Token Caching Sample
 *
 * MIT License
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this
 * software and associated documentation files (the "Software"), to deal in the Software
 * without restriction, including without limitation the rights to use, copy, modify, merge,
 * publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
 * to whom the Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
 * FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
 * OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 */

using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Store.PartnerCenter;
using Microsoft.Store.PartnerCenter.Extensions;
using Newtonsoft.Json;
using PartnerCenter.Samples.TokenCaching.Cache;
using PartnerCenter.Samples.TokenCaching.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PartnerCenter.Samples.TokenCaching.Context
{
    /// <summary>
    /// Helper class for retrieving access tokens.
    /// </summary>
    public class TokenContext
    {
        /// <summary>
        /// Gets an access token from the authority.
        /// </summary>
        /// <param name="authority">Address of the authority to issue the token.</param>
        /// <param name="resource">Identifier of the target resource that is the recipent of the requested token.</param>
        /// <returns>An instnace of <see cref="AuthenticationResult"/> that represented the access token.</returns>
        /// <exception cref="ArgumentNullException">
        /// authority
        /// or
        /// resource
        /// </exception>
        public async Task<AuthenticationResult> GetAADTokenAsync(string authority, string resource)
        {
            AuthenticationContext authContext;
            DistributedTokenCache tokenCache;

            if (string.IsNullOrEmpty(authority))
            {
                throw new ArgumentNullException(nameof(authority));
            }
            if (string.IsNullOrEmpty(resource))
            {
                throw new ArgumentNullException(nameof(resource));
            }

            try
            {
                // If the Redis Cache connection string is not populated then utilize the constructor
                // that only requires the authority. That constructor will utilize a in-memory caching
                // feature that is built-in into ADAL.
                if (string.IsNullOrEmpty(Settings.RedisConnection))
                {
                    authContext = new AuthenticationContext(authority);
                }
                else
                {
                    tokenCache = new DistributedTokenCache(resource);
                    authContext = new AuthenticationContext(authority, tokenCache);
                }

                return await authContext.AcquireTokenAsync(
                    resource,
                    new ClientCredential(
                        Settings.ApplicationId,
                        Settings.ApplicationSecret),
                    new UserAssertion(UserAssertionToken, "urn:ietf:params:oauth:grant-type:jwt-bearer"));
            }
            finally
            {
                authContext = null;
                tokenCache = null;
            }
        }

        /// <summary>
        /// Get an access token for the Partner Center API.
        /// </summary>
        /// <param name="authority">Address of the authority to issue the token.</param>
        /// <returns>
        /// An instance of <see cref="IPartnerCredentials" /> that represents the access token.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// authority
        /// </exception>
        public async Task<IPartnerCredentials> GetPartnerCenterTokenAsync(string authority)
        {
            AuthenticationResult authResult;
            IPartnerCredentials credentials;

            if (string.IsNullOrEmpty(authority))
            {
                throw new ArgumentNullException(nameof(authority));
            }

            try
            {
                if (CacheManager.Instance.Exists(Key))
                {
                    credentials = JsonConvert.DeserializeObject<PartnerCenterTokenModel>(
                        CacheManager.Instance.Read(Key));

                    if (!credentials.IsExpired())
                    {
                        return credentials;
                    }
                }

                authResult = await GetAADTokenAsync(
                    authority,
                    "https://api.partnercenter.microsoft.com");

                credentials = await PartnerCredentials.Instance.GenerateByUserCredentialsAsync(
                    Settings.ApplicationId,
                    new AuthenticationToken(authResult.AccessToken, authResult.ExpiresOn));

                CacheManager.Instance.Write(Key, JsonConvert.SerializeObject(credentials));

                return credentials;
            }
            finally
            {
                authResult = null;
            }
        }

        /// <summary>
        /// Gets the user assertion token.
        /// </summary>
        /// <value>
        /// The user assertion token for the authentication user.
        /// </value>
        /// <remarks>
        /// This token was obtained when the user logged into the site.
        /// </remarks>
        private static string UserAssertionToken
        {
            get
            {
                System.IdentityModel.Tokens.BootstrapContext bootstrapContext;

                try
                {
                    bootstrapContext = ClaimsPrincipal.Current.Identities.First().BootstrapContext as System.IdentityModel.Tokens.BootstrapContext;

                    return bootstrapContext?.Token;
                }
                finally
                {
                    bootstrapContext = null;
                }
            }
        }

        private static string Key =>
           $"Resource:PartnerCenterAPI::UserId:{ClaimsPrincipal.Current.Identities.First().FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value}";
    }
}