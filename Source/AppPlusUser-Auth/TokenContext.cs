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
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PartnerCenter.Samples.AppPlusUserAuth
{
    public static class TokenContext
    {
        /// <summary>
        /// Obtain a security token.
        /// </summary>
        /// <param name="authority">Authority used to obtain the token.</param>
        /// <param name="resource">The resource for which the token should be obtained.</param>
        /// <param name="username">The username to be used to obtain the token.</param>
        /// <param name="password">The password to be used to obtain the token.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// authority
        /// or
        /// resource
        /// or
        /// username
        /// or
        /// password
        /// </exception>
        public static AuthorizationToken GetAADToken(string authority, string resource, string username, string password)
        {
            if (string.IsNullOrEmpty(authority))
            {
                throw new ArgumentNullException(nameof(authority));
            }
            if (string.IsNullOrEmpty(resource))
            {
                throw new ArgumentNullException(nameof(resource));
            }
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            return SynchronousExecute(() => GetAADTokenAsync(authority, resource, username, password));
        }

        /// <summary>
        /// Obtain a security token.
        /// </summary>
        /// <param name="authority">Authority used to obtain the token.</param>
        /// <param name="resource">The resource for which the token should be obtained.</param>
        /// <param name="username">The username to be used to obtain the token.</param>
        /// <param name="password">The password to be used to obtain the token.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// authority
        /// or
        /// resource
        /// or
        /// username
        /// or
        /// password
        /// </exception>
        public static async Task<AuthorizationToken> GetAADTokenAsync(string authority, string resource, string username, string password)
        {
            Communication comm;
            HttpContent content;
            List<KeyValuePair<string, string>> values;

            if (string.IsNullOrEmpty(authority))
            {
                throw new ArgumentNullException(nameof(authority));
            }
            if (string.IsNullOrEmpty(resource))
            {
                throw new ArgumentNullException(nameof(resource));
            }
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            try
            {
                comm = new Communication();
                values = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("resource", resource),
                    new KeyValuePair<string, string>("client_id", Settings.ApplicationId),
                    new KeyValuePair<string, string>("client_secret", Settings.ApplicationSecret),
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("password", password),
                    new KeyValuePair<string, string>("username", username)
                };

                content = new FormUrlEncodedContent(values);

                return await comm.PostAsync<AuthorizationToken>(authority, content);
            }
            finally
            {
                content = null;
                values = null;
            }
        }

        /// <summary>
        /// Synchronously executes an asynchronous function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="operation">The operation.</param>
        /// <returns></returns>
        private static T SynchronousExecute<T>(Func<Task<T>> operation)
        {
            try
            {
                return Task.Run(async () => await operation()).Result;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }
    }
}