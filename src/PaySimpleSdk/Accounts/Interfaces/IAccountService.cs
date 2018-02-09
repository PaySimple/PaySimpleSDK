#region License
// The MIT License (MIT)
//
// Copyright (c) 2015 Scott Lance
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
// The most recent version of this license can be found at: http://opensource.org/licenses/MIT
#endregion

using System.Threading.Tasks;

namespace PaySimpleSdk.Accounts
{
    public interface IAccountService
    {
        /// <summary>
        /// NOTE: PCI Compliance is the responsibility of the user of this SDK
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="account"></param>
        /// <returns></returns>
        Task<T> CreateAccountAsync<T>(T account) where T : Account;
        /// <summary>
        /// NOTE: PCI Compliance is the responsibility of the user of this SDK
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Task<Ach> CreateAchAccountAsync(Ach account);
        /// <summary>
        /// NOTE: PCI Compliance is the responsibility of the user of this SDK
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Task<CreditCard> CreateCreditCardAccountAsync(CreditCard account);
        Task DeleteAchAccountAsync(int accountId);
        Task DeleteCreditCardAccountAsync(int accountId);
        Task<Ach> GetAchAccountAsync(int accountId);
        Task<CreditCard> GetCreditCardAccountAsync(int accountId);
        /// <summary>
        /// NOTE: PCI Compliance is the responsibility of the user of this SDK
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Task<Ach> UpdateAchAccountAsync(Ach account);
        /// <summary>
        /// NOTE: PCI Compliance is the responsibility of the user of this SDK
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Task<CreditCard> UpdateCreditCardAccountAsync(CreditCard account);
    }
}