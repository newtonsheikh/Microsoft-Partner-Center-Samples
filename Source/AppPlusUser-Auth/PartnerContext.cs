/*
 * Microsoft Partner Center - App Plus User Authentication Sample
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

using System;
using System.Threading.Tasks;
using Microsoft.Store.PartnerCenter;
using Microsoft.Store.PartnerCenter.Extensions;

namespace PartnerCenter.Samples.AppPlusUserAuth
{
    public class PartnerContext
    {
        private IAggregatePartner _partner;

        /// <summary>
        /// Gets the an instance of <see cref="IAggregatePartner" /> used to interact with Partner Center.
        /// </summary>
        /// <returns>
        /// An instance of <see cref="IAggregatePartner" /> connected to the CSP reseller in the app.config.
        /// </returns>
        /// <remarks>
        /// Various operations with the Partner Center SDK require App + User authorization. More details
        /// regarding Partner Center authentication can be found at
        /// https://msdn.microsoft.com/en-us/library/partnercenter/mt634709.aspx.
        /// </remarks>
        public IAggregatePartner GetPartnerOperations(string username, string password)
        {
            AuthorizationToken token;
            IPartnerCredentials credentials;

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            try
            {
                if (_partner != null)
                {
                    return _partner;
                }

                token = TokenContext.GetAADToken(
                    $"{Settings.Authority}/{Settings.TenantId}/oauth2/token",
                    "https://api.partnercenter.microsoft.com",
                    username,
                    password);

                credentials = PartnerCredentials.Instance.GenerateByUserCredentials(
                    Settings.ApplicationId,
                    new AuthenticationToken(token.AccessToken, token.ExpiresOn), authenticationToken =>
                    {
                        token = TokenContext.GetAADToken(
                            $"{Settings.Authority}/{Settings.TenantId}/oauth2/token",
                            "https://api.partnercenter.microsoft.com",
                            username,
                            password);

                        return Task.FromResult(new AuthenticationToken(token.AccessToken, token.ExpiresOn));
                    });


                _partner = PartnerService.Instance.CreatePartnerOperations(credentials);

                return _partner;
            }
            finally
            {
                credentials = null;
                token = null;
            }
        }
    }
}