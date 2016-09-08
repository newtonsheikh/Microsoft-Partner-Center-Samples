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

using System;
using System.Collections.Generic;
using System.Text;

namespace PartnerCenter.Samples.AOBO.Cache
{
    /// <summary>
    /// Provides a mechanism for caching access tokens uisng an in-memory strategy.
    /// </summary>
    /// <seealso cref="PartnerCenter.Samples.AOBO.Cache.ICacheManager" />
    public class InMemoryCacheManager : ICacheManager
    {
        private static readonly Lazy<Dictionary<string, string>> Cache =
           new Lazy<Dictionary<string, string>>(() => new Dictionary<string, string>());

        private readonly MachineKeyDataProtector _protector;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryCacheManager"/> class.
        /// </summary>
        public InMemoryCacheManager()
        {
            _protector = new MachineKeyDataProtector(new[] { typeof(InMemoryCacheManager).FullName });
        }

        /// <summary>
        /// Determines whether the specified key exists or not.
        /// </summary>
        /// <param name="key">The key value to be checked.</param>
        /// <returns>
        ///   <c>true</c> if the key exists; otherwise <c>false</c>.
        /// </returns>
        public bool Exists(string key)
        {
            return CacheInstance.ContainsKey(key);
        }

        /// <summary>
        /// Reads the value for the specified key.
        /// </summary>
        /// <param name="key">The key of the value.</param>
        /// <returns>The value assigned to the specified key.</returns>
        public string Read(string key)
        {
            byte[] data = _protector.Unprotect(
                Convert.FromBase64String(CacheInstance[key]));

            return Encoding.ASCII.GetString(data);
        }

        /// <summary>
        /// Clears the cache by deleting all the items.
        /// </summary>
        public void Clear()
        {
            CacheInstance.Clear();
        }

        /// <summary>
        /// Deletes the specified key.
        /// </summary>
        /// <param name="key">The key value to be deleted.</param>
        public void Delete(string key)
        {
            CacheInstance.Remove(key);
        }

        /// <summary>
        /// Writes the specified key value pair.
        /// </summary>
        /// <param name="key">The key value to be written.</param>
        /// <param name="value">The value to be written.</param>
        public void Write(string key, string value)
        {
            string data = Convert.ToBase64String(
                _protector.Protect(Encoding.ASCII.GetBytes(value)));
            CacheInstance[key] = data;
        }

        /// <summary>
        /// Writes the specified key value pair.
        /// </summary>
        /// <param name="key">The key of the element to be written.</param>
        /// <param name="value">The value of the element to be written.</param>
        /// <param name="expires">When the value should expire.</param>
        /// <remarks>
        /// The expiration functionality is not implemented for this cache manager implementation.
        /// </remarks>
        public void Write(string key, string value, TimeSpan expires)
        {
            string data = Convert.ToBase64String(
                _protector.Protect(Encoding.ASCII.GetBytes(value)));

            CacheInstance[key] = data;
        }

        private static Dictionary<string, string> CacheInstance => Cache.Value;
    }
}