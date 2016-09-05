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

using System.Configuration;

namespace PartnerCenter.Samples.TokenCaching
{
    public static class Settings
    {
        public static string ApplicationId => ConfigurationManager.AppSettings["ApplicationId"];

        public static string ApplicationSecret => ConfigurationManager.AppSettings["ApplicationSecret"];

        /// <summary>
        /// Gets the caching strategy used for caching access tokens.
        /// </summary>
        /// <value>
        /// This caching strategy used for caching access token.
        /// </value>
        /// <remarks>
        /// The only valid values for this property are Empty, InMemory, or Redis. If any other value is 
        /// specified the program will default to the empty strategy.
        /// </remarks>
        public static string CachingStrategy => ConfigurationManager.AppSettings["CachingStrategy"];

        /// <summary>
        /// Gets the Redis connection string from the web.config file.
        /// </summary>
        /// <value>
        /// The Redis Cache connection string
        /// </value>
        public static string RedisConnection => ConfigurationManager.AppSettings["RedisConnection"];
    }
}