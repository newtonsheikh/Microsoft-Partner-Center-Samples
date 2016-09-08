/*
 * Microsoft Partner Center - Admin on Behalf of (AOBO) Sample
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

using Microsoft.Exchange.WebServices.Data;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.IO;
using System.Reflection;

namespace PartnerCenter.Samples.AOBO.Context
{
    /// <summary>
    /// Helper class for perform Excahnge Web Service related operations.
    /// </summary>
    public class ExchangeContext
    {
        private readonly ExchangeService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeContext"/> class.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="impersonationAddress">Email address of the user to impersonate.</param>
        /// <exception cref="ArgumentNullException">
        /// customerId
        /// or
        /// impersonationAddress
        /// </exception>
        public ExchangeContext(string customerId, string impersonationAddress)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }
            if (string.IsNullOrEmpty(impersonationAddress))
            {
                throw new ArgumentNullException(nameof(impersonationAddress));
            }

            _service = new ExchangeService(ExchangeVersion.Exchange2013_SP1)
            {
                Credentials = GetOAuthCredential(customerId),
                ImpersonatedUserId = new ImpersonatedUserId(ConnectingIdType.SmtpAddress, impersonationAddress),
                TraceEnabled = true,
                TraceFlags = TraceFlags.All,
                Url = new Uri($"{Settings.EWSUri}/ews/Exchange.asmx")
            };
        }

        /// <summary>
        /// Installs the application described by the manifest file.
        /// </summary>
        /// <param name="manifest">Path for the manifest XML file.</param>
        /// <exception cref="ArgumentNullException">manifest</exception>
        /// <exception cref="FileNotFoundException">Unable to locate the manifest file</exception>
        public void InstallApp(string manifest)
        {
            Assembly assembly;

            if (string.IsNullOrEmpty(manifest))
            {
                throw new ArgumentNullException(nameof(manifest));
            }

            try
            {
                assembly = Assembly.GetExecutingAssembly();

                using (Stream manifestStream = assembly.GetManifestResourceStream(manifest))
                {
                    _service.InstallApp(manifestStream);
                }
            }
            finally
            {
                assembly = null;
            }
        }

        private OAuthCredentials GetOAuthCredential(string customerId)
        {
            AuthenticationResult authResult;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }

            try
            {
                authResult = new TokenContext().GetAppOnlyToken(
                    $"{Settings.Authority}/{customerId}",
                    Settings.EWSUri,
                    Settings.Thumbprint);
                return new OAuthCredentials(authResult.AccessToken);
            }
            finally
            {
                authResult = null;
            }
        }
    }
}