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

using StackExchange.Redis;
using System;
using System.Text;

namespace PartnerCenter.Samples.TokenCaching.Cache
{
    public class RedisCacheManager : ICacheManager
    {
        private readonly IDatabase _cache;
        private readonly MachineKeyDataProtector _protector;

        private static readonly Lazy<ConnectionMultiplexer> Connection =
            new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(Settings.RedisConnection));

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisCacheManager"/> class.
        /// </summary>
        public RedisCacheManager()
        {
            _cache = ConnectionInstance.GetDatabase();
            _protector = new MachineKeyDataProtector(new[] { typeof(RedisCacheManager).FullName });
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
            return _cache.KeyExists(key);
        }

        /// <summary>
        /// Reads the value for the specified key.
        /// </summary>
        /// <param name="key">The key of the value.</param>
        /// <returns></returns>
        public string Read(string key)
        {
            byte[] data = _protector.Unprotect(
                Convert.FromBase64String(_cache.StringGet(key)));

            return Encoding.ASCII.GetString(data);
        }

        /// <summary>
        /// Clears the cache by deleting all the items.
        /// </summary>
        public void Clear()
        {
            // _cache.
        }

        /// <summary>
        /// Deletes the specified key.
        /// </summary>
        /// <param name="key">The key value to be deleted.</param>
        public void Delete(string key)
        {
            _cache.KeyDelete(key);
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
            _cache.StringSet(key, data);
        }

        /// <summary>
        /// Writes the specified key value pair.
        /// </summary>
        /// <param name="key">The key of the element to be written.</param>
        /// <param name="value">The value of the element to be written.</param>
        /// <param name="expires">When the value should expire.</param>
        public void Write(string key, string value, TimeSpan expires)
        {
            string data = Convert.ToBase64String(
               _protector.Protect(Encoding.ASCII.GetBytes(value)));
            _cache.StringSet(key, data, expires);
        }

        private static ConnectionMultiplexer ConnectionInstance => Connection.Value;
    }
}