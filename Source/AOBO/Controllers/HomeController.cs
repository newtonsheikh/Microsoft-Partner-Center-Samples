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

using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.Azure.ActiveDirectory.GraphClient.Extensions;
using Microsoft.Store.PartnerCenter;
using Microsoft.Store.PartnerCenter.Models;
using Microsoft.Store.PartnerCenter.Models.Customers;
using Microsoft.Store.PartnerCenter.Models.Subscriptions;
using PartnerCenter.Samples.AOBO.Context;
using PartnerCenter.Samples.AOBO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PartnerCenter.Samples.AOBO.Controllers
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

        [Route("InstallApp/{customerId}/{userId}")]
        public async Task<HttpResponseMessage> InstallAppAsync(string customerId, string userId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            GraphContext graph;
            ExchangeContext context;
            IUser user;

            try
            {
                graph = new GraphContext(customerId);
                user = await graph.Client.Users.GetByObjectId(userId).ExecuteAsync();
                context = new ExchangeContext(customerId, user.Mail);
                context.InstallApp("PartnerCenter.Samples.AOBO.Manifests.HelloWorld.HelloWorld.xml");
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
            finally
            {
                context = null;
                graph = null;
                user = null;
            }
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

        [Route("ListSubscriptions/{customerId}")]
        public async Task<PartialViewResult> ListSubscriptionsAsync(string customerId)
        {
            IAggregatePartner operations;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }

            try
            {
                operations = await new SdkContext().GetOperationsAsync();

                SubscriptionsModel subscriptionsModel = new SubscriptionsModel()
                {
                    CustomerId = customerId,
                    Subscriptions = await GetSubscriptionsAsync(customerId)
                };

                return PartialView("ListSubscriptions", subscriptionsModel);
            }
            finally
            {
                operations = null;
            }
        }

        [Route("ListUsers/{customerId}")]
        public async Task<PartialViewResult> ListUsersAsync(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }


            UsersModel usersModel = new UsersModel()
            {
                CustomerId = customerId,
                Users = await GetUsersAsync(customerId),
            };

            return PartialView("ListUsers", usersModel);
        }

        [Route("Subscriptions/{customerId}")]
        public ViewResult Subscriptions(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(customerId);
            }

            SubscriptionsModel subscriptionsModel = new SubscriptionsModel()
            {
                CustomerId = customerId
            };

            return View(subscriptionsModel);
        }

        [Route("Users/{customerId}")]
        public ViewResult Users(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(customerId);
            }

            UsersModel usersModel = new UsersModel()
            {
                CustomerId = customerId,
            };

            return View(usersModel);
        }

        private async Task<List<SubscriptionModel>> GetSubscriptionsAsync(string customerId)
        {
            IAggregatePartner operations;
            ResourceCollection<Subscription> subscriptions;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(customerId);
            }

            try
            {
                operations = await new SdkContext().GetOperationsAsync();
                subscriptions = await operations.Customers.ById(customerId).Subscriptions.GetAsync();

                return subscriptions.Items.Select(s => new SubscriptionModel()
                {
                    CustomerId = customerId,
                    FriendlyName = s.FriendlyName,
                    Id = s.Id,
                    Status = s.Status.ToString()
                }).ToList();
            }
            finally
            {
                operations = null;
            }
        }

        private async Task<List<UserModel>> GetUsersAsync(string customerId)
        {
            GraphContext context;
            IPagedCollection<IUser> users;

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }

            try
            {
                context = new GraphContext(customerId);
                users = await context.Client.Users.ExecuteAsync();

                return users.CurrentPage.ToList().Select(u => new UserModel()
                {
                    CustomerId = customerId,
                    Mail = u.Mail,
                    ObjectId = u.ObjectId,
                    UserPrincipalName = u.UserPrincipalName
                }).ToList();
            }
            finally
            {
                users = null;
            }
        }
    }
}