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

using System;

namespace PartnerCenter.Samples.TokenCaching.Cache
{
    public class EmptyCacheManager : ICacheManager
    {
        /// <summary>
        /// Determines whether the specified key exists or not.
        /// </summary>
        /// <param name="key">The key value to be checked.</param>
        /// <returns>
        ///   <c>true</c> if the key exists; otherwise <c>false</c>.
        public bool Exists(string key)
        {
            return false;
        }

        /// <summary>
        /// Reads the value for the specified key.
        /// </summary>
        /// <param name="key">The key of the value.</param>
        /// <returns>Value assigned to the specified key.</returns>
        public string Read(string key)
        {
            return string.Empty;
        }

        /// <summary>
        /// Clears the cache by deleting all the items.
        /// </summary>
        public void Clear()
        { }

        /// <summary>
        /// Deletes the specified key.
        /// </summary>
        /// <param name="key">The key value to be deleted.</param>
        public void Delete(string key)
        { }

        /// <summary>
        /// Writes the specified key value pair.
        /// </summary>
        /// <param name="key">The key of the element to be written.</param>
        /// <param name="value">The value of the element to be written.</param>
        public void Write(string key, string value)
        { }

        /// <summary>
        /// Writes the specified key value pair.
        /// </summary>
        /// <param name="key">The key of the element to be written.</param>
        /// <param name="value">The value of the element to be written.</param>
        /// <param name="expires">When the value should expire.</param>
        public void Write(string key, string value, TimeSpan expires)
        { }
    }
}