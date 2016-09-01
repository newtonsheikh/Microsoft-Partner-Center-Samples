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

using Microsoft.Store.PartnerCenter.Models;
using Microsoft.Store.PartnerCenter.Models.Customers;
using System;

namespace PartnerCenter.Samples.AppOnlyAuth
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (ValidateSettings() == false)
            {
                Console.WriteLine(
                    "Please make the necessary corrections in the app.config before trying to execute this code.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();
                Environment.Exit(0);
            }

            PartnerContext context;
            SeekBasedResourceCollection<Customer> customers;

            try
            {
                context = new PartnerContext();
                customers = context.GetPartnerOperations().Customers.Get();

                foreach (Customer c in customers.Items)
                {
                    Console.WriteLine(c.CompanyProfile.CompanyName);
                }

                Console.WriteLine("fin");
                Console.ReadLine();
            }
            finally
            {
                context = null;
                customers = null;
            }
        }

        private static bool ValidateSettings()
        {
            if (string.IsNullOrEmpty(Settings.ApplicationId))
            {
                Console.WriteLine("The ApplicationId setting has not been configured in the app.config");
                return false;
            }
            if (string.IsNullOrEmpty(Settings.ApplicationSecret))
            {
                Console.WriteLine("The ApplicationSecret setting has not been configured in the app.config");
                return false;
            }
            if (string.IsNullOrEmpty(Settings.Authority))
            {
                Console.WriteLine("The Authority setting has not been configured in the app.config");
                return false;
            }
            if (string.IsNullOrEmpty(Settings.TenantId))
            {
                Console.WriteLine("The TenantId setting has not been configured in the app.config");
                return false;
            }

            return true;
        }
    }
}