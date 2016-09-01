/*
 * Microsoft Partner Center - App Only Authentication Sample
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

using Microsoft.Store.PartnerCenter;
using Microsoft.Store.PartnerCenter.Extensions;

namespace PartnerCenter.Samples.AppOnlyAuth
{
    public class PartnerContext
    {
        private IAggregatePartner _partner;

        /// <summary>
        /// Gets the an instance of <see cref="IAggregatePartner"/> used to interact with Partner Center.
        /// </summary>
        /// <returns>
        /// An instance of <see cref="IAggregatePartner"/> connected to the CSP reseller in the app.config.
        /// </returns>
        /// <remarks>
        /// Various operations with the Partner Center SDK require App + User authorization. More details
        /// regarding Partner Center authentication can be found at
        /// https://msdn.microsoft.com/en-us/library/partnercenter/mt634709.aspx.
        /// </remarks>
        /// <example>
        /// public void ListCustomers()
        ///{
        ///    PartnerContext context;
        ///    SeekBasedResourceCollection&lt;Customer&gt; customers;
        ///
        ///    try
        ///    {
        ///        context = new PartnerContext();
        ///        customers = context.GetPartnerOperations().Customers.Get();
        ///
        ///        foreach (Customer c in customers.Items)
        ///        {
        ///            Console.WriteLine(c.CompanyProfile.CompanyName);
        ///        }
        ///    }
        ///    finally
        ///    {
        ///        context = null;
        ///        customers = null;
        ///    }
        ///}
        /// </example>
        public IAggregatePartner GetPartnerOperations()
        {
            IPartnerCredentials credentials;

            try
            {
                if (_partner != null)
                {
                    return _partner;
                }

                credentials = PartnerCredentials.Instance.GenerateByApplicationCredentials(
                    Settings.ApplicationId,
                    Settings.ApplicationSecret,
                    Settings.TenantId);
                _partner = PartnerService.Instance.CreatePartnerOperations(credentials);

                return _partner;
            }
            finally
            {
                credentials = null;
            }
        }
    }
}