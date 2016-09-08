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

using Microsoft.Owin.Security.DataProtection;
using System.Web.Security;

namespace PartnerCenter.Samples.AOBO.Cache
{
    /// <summary>
    /// Protects data using <se cref="MachineKey"/> encryption..
    /// </summary>
    /// <seealso cref="Microsoft.Owin.Security.DataProtection.IDataProtector" />
    public class MachineKeyDataProtector : IDataProtector
    {
        private readonly string[] _purposes;

        /// <summary>
        /// Initializes a new instance of the <see cref="MachineKeyDataProtector"/> class.
        /// </summary>
        /// <param name="purposes">THe purpose of the data being protected.</param>
        public MachineKeyDataProtector(string[] purposes)
        {
            _purposes = purposes;
        }

        /// <summary>
        /// Protects the specified data.
        /// </summary>
        /// <param name="userData">The data to be protected.</param>
        /// <returns></returns>
        public byte[] Protect(byte[] userData)
        {
            return MachineKey.Protect(userData, _purposes);
        }

        /// <summary>
        /// Unprotects the specified data.
        /// </summary>
        /// <param name="protectedData">The data to be unprotected.</param>
        /// <returns></returns>
        public byte[] Unprotect(byte[] protectedData)
        {
            return MachineKey.Unprotect(protectedData, _purposes);
        }
    }
}