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

using Microsoft.Store.PartnerCenter;
using Microsoft.Store.PartnerCenter.Models;
using Microsoft.Store.PartnerCenter.Models.Customers;
using Microsoft.Store.PartnerCenter.Models.Subscriptions;
using PartnerCenter.Samples.TokenCaching.Context;
using PartnerCenter.Samples.TokenCaching.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PartnerCenter.Samples.TokenCaching.Controllers
{
    [Authorize]
    [RoutePrefix("")]
    public class HomeController : Controller
    {
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("ListCustomers")]
        public async Task<PartialViewResult> ListCustomersAsync()
        {
            IAggregatePartner operations;
            SeekBasedResourceCollection<Customer> customers;

            try
            {
                operations = await new SdkContext().GetOperationsAsync();
                customers = await operations.Customers.GetAsync();

                CustomersModel customersModel = new CustomersModel()
                {
                    Customers = customers.Items.ToList()
                };

                return PartialView("ListCustomers", customersModel);
            }
            finally
            {
                operations = null;
            }
        }
        [Route("Subscriptions/{customerId}")]
        public ViewResult Subscriptions(string customerId)
        {
            SubscriptionsModel subscriptionsModel = new SubscriptionsModel()
            {
                CustomerId = customerId
            };

            return View(subscriptionsModel);
        }

        [Route("ListSubscriptions/{customerId}")]
        public async Task<PartialViewResult> ListSubscriptions(string customerId)
        {
            IAggregatePartner operations;
            ResourceCollection<Subscription> subscriptions;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }

            try
            {
                operations = await new SdkContext().GetOperationsAsync();
                subscriptions = await operations.Customers.ById(customerId).Subscriptions.GetAsync();

                SubscriptionsModel subscriptionsModel = new SubscriptionsModel()
                {
                    CustomerId = customerId,
                    Subscriptions = subscriptions.Items.ToList()
                };

                return PartialView(subscriptionsModel);
            }
            finally
            {
                operations = null;
            }
        }
    }
}