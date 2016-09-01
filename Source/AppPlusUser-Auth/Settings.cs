/*
 * Partner Center API - App Only Authentication Sample
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

using System.Configuration;

namespace PartnerCenter.Samples.AppPlusUserAuth
{
    /// <summary>
    /// Helper object that provides quick access to application level settings.
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Gets the application identifier for the Azure AD application to used to interact with the Partner Center API.
        /// </summary>
        /// <remarks>
        /// This value is a GUID that must be configured within the app.config prior to executing this sample project.
        /// </remarks>
        public static string ApplicationId => ConfigurationManager.AppSettings["ApplicationId"];

        /// <summary>
        /// Gets the application secret for the Azure AD application to used to interact with the Partner Center API.
        /// </summary>
        /// <remarks>
        /// This is the key (aka application secret or client secret) obtained from the Azure AD application configuration
        /// or the Partner Center portal. This value must be populated in the app.config prior to executing this sample project.
        /// </remarks>
        public static string ApplicationSecret => ConfigurationManager.AppSettings["ApplicationSecret"];

        /// <summary>
        /// Gets the authority to be used when obtaining a security token.
        /// </summary>
        /// <remarks>
        /// This value must be configured in the app.config prior to executing this sample project.
        /// </remarks>
        public static string Authority => ConfigurationManager.AppSettings["Authority"];

        /// <summary>
        /// Gets the Uri for the Partner Center API.
        /// </summary>
        /// <remarks>
        /// In a real world application this value should not be hardcoded.
        /// </remarks>
        public static string PartnerCenterApiUri => "https://api.partnercenter.microsoft.com";

        /// <summary>
        /// Gets the tenant identifier for the CSP reseller account.
        /// </summary>
        /// <remarks>
        /// This setting can be configured to either the tenant identifier (account identifier) or
        /// the domain (e.g. cspreseller.onmicrosoft.com) associated with the CSP reseller Azure AD
        /// tenant. It can be either the production or integration sandbox Azure AD tenant. This setting
        /// must be configured in the app.config prior to executing this sample project.
        /// </remarks>
        public static string TenantId => ConfigurationManager.AppSettings["TenantId"];
    }
}