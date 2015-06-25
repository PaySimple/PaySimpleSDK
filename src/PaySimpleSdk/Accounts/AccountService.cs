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

using PaySimpleSdk.Helpers;
using PaySimpleSdk.Models;
using PaySimpleSdk.Validation;
using System;
using System.Threading.Tasks;

namespace PaySimpleSdk.Accounts
{
    public class AccountService : ServiceBase, IAccountService
    {
        public AccountService(IPaySimpleSettings settings)
            : base(settings)
        { }

        internal AccountService(IPaySimpleSettings settings, IValidationService validationService, IWebServiceRequest webServiceRequest, IServiceFactory serviceFactory)
            : base(settings, validationService, webServiceRequest, serviceFactory)
        { }

        public async Task<Result<T>> CreateAccountAsync<T>(T account)
            where T : Account
        {
            if (account.GetType() == typeof(Ach))
                return await CreateAchAccountAsync(account as Ach) as Result<T>;                
            else
                return await CreateCreditCardAccountAsync(account as CreditCard) as Result<T>;
        }

        public async Task<Result<Ach>> CreateAchAccountAsync(Ach account)
        {
            validationService.Validate(account);
            var endpoint = string.Format("{0}{1}", settings.BaseUrl, Endpoints.AchAccount);
            return await webServiceRequest.PostDeserializedAsync<Ach, Result<Ach>>(new Uri(endpoint), account);
        }

        public async Task<Result<CreditCard>> CreateCreditCardAccountAsync(CreditCard account)
        {
            validationService.Validate(account);
            var endpoint = string.Format("{0}{1}", settings.BaseUrl, Endpoints.CreditCardAccount);
            return await webServiceRequest.PostDeserializedAsync<CreditCard, Result<CreditCard>>(new Uri(endpoint), account);
        }

        public async Task DeleteAchAccountAsync(int accountId)
        {
            var endpoint = string.Format("{0}{1}/{2}", settings.BaseUrl, Endpoints.AchAccount, accountId);
            await webServiceRequest.DeleteAsync(new Uri(endpoint));
        }

        public async Task DeleteCreditCardAccountAsync(int accountId)
        {
            var endpoint = string.Format("{0}{1}/{2}", settings.BaseUrl, Endpoints.CreditCardAccount, accountId);
            await webServiceRequest.DeleteAsync(new Uri(endpoint));
        }

        public async Task<Result<Ach>> GetAchAccountAsync(int accountId)
        {
            var endpoint = string.Format("{0}{1}/{2}", settings.BaseUrl, Endpoints.AchAccount, accountId);
            return await webServiceRequest.GetDeserializedAsync<Result<Ach>>(new Uri(endpoint));
        }

        public async Task<Result<CreditCard>> GetCreditCardAccountAsync(int accountId)
        {
            var endpoint = string.Format("{0}{1}/{2}", settings.BaseUrl, Endpoints.CreditCardAccount, accountId);
            return await webServiceRequest.GetDeserializedAsync<Result<CreditCard>>(new Uri(endpoint));
        }

        public async Task<Result<Ach>> UpdateAchAccountAsync(Ach account)
        {
            validationService.Validate(account);
            var endpoint = string.Format("{0}{1}", settings.BaseUrl, Endpoints.AchAccount);
            return await webServiceRequest.PutDeserializedAsync<Ach, Result<Ach>>(new Uri(endpoint), account);
        }

        public async Task<Result<CreditCard>> UpdateCreditCardAccountAsync(CreditCard account)
        {
            validationService.Validate(account);
            var endpoint = string.Format("{0}{1}", settings.BaseUrl, Endpoints.CreditCardAccount);
            return await webServiceRequest.PutDeserializedAsync<CreditCard, Result<CreditCard>>(new Uri(endpoint), account);
        }
    }
}