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

using PaySimpleSdk.Accounts;
using PaySimpleSdk.Helpers;
using PaySimpleSdk.Models;
using PaySimpleSdk.Payments;
using PaySimpleSdk.Validation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaySimpleSdk.PaymentSchedules
{
    public class PaymentScheduleService : ServiceBase, IPaymentScheduleService
    {
        public PaymentScheduleService(IPaySimpleSettings settings)
            : base(settings)
        { }

        internal PaymentScheduleService(IPaySimpleSettings settings, IValidationService validationService, IWebServiceRequest webServiceRequest, IServiceFactory serviceFactory)
            : base(settings, validationService, webServiceRequest, serviceFactory)
        { }
        
        public async Task<NewAccountPaymentPlan<T>> CreateNewAccountPaymentPlanAsync<T>(NewAccountPaymentPlan<T> accountPaymentPlan)
            where T : Account, new()
        {
            // Validate objects
            validationService.Validate(accountPaymentPlan);

            var newAccountPaymentPlan = new NewAccountPaymentPlan<T>();

            // Create the account
            var accountService = serviceFactory.GetAccountService();
            newAccountPaymentPlan.Account = await accountService.CreateAccountAsync<T>(accountPaymentPlan.Account);             

            // Make the Payment            
            accountPaymentPlan.PaymentPlan.AccountId = newAccountPaymentPlan.Account.Id;
            newAccountPaymentPlan.PaymentPlan = await CreatePaymentPlanAsync(accountPaymentPlan.PaymentPlan);

            return newAccountPaymentPlan;
        }

        public async Task<NewCustomerPaymentPlan<T>> CreateNewCustomerPaymentPlanAsync<T>(NewCustomerPaymentPlan<T> customerPaymentPlan)
            where T : Account, new()
        {
            // Validate objects
            validationService.Validate(customerPaymentPlan);

            var newCustomerPaymentPlan = new NewCustomerPaymentPlan<T>();

            // Create the new Customer
            var customerService = serviceFactory.GetCustomerService();
            newCustomerPaymentPlan.Customer = await customerService.CreateCustomerAsync(customerPaymentPlan.Customer);            

            // Create the new Account, and make the payment
            customerPaymentPlan.Account.CustomerId = newCustomerPaymentPlan.Customer.Id;
            var paymentResult = await CreateNewAccountPaymentPlanAsync<T>(customerPaymentPlan);

            newCustomerPaymentPlan.Account = paymentResult.Account;
            newCustomerPaymentPlan.PaymentPlan = paymentResult.PaymentPlan;

            return newCustomerPaymentPlan;
        }
        
        public async Task<PaymentPlan> CreatePaymentPlanAsync(PaymentPlan paymentPlan)
        {
            validationService.Validate(paymentPlan);
            var endpoint = string.Format("{0}{1}", settings.BaseUrl, Endpoints.PaymentPlan);
            var result = await webServiceRequest.PostDeserializedAsync<PaymentPlan, Result<PaymentPlan>>(new Uri(endpoint), paymentPlan);
            return result.Response;
        }
        
        public async Task<NewAccountRecurringPayment<T>> CreateNewAccountRecurringPaymentAsync<T>(NewAccountRecurringPayment<T> accountRecurringPayment)
            where T : Account, new()
        {
            // Validate objects
            validationService.Validate(accountRecurringPayment);

            var newAccountRecurringPayment = new NewAccountRecurringPayment<T>();

            // Create the account
            var accountService = serviceFactory.GetAccountService();
            newAccountRecurringPayment.Account = await accountService.CreateAccountAsync<T>(accountRecurringPayment.Account);             

            // Make the Payment            
            accountRecurringPayment.RecurringPayment.AccountId = newAccountRecurringPayment.Account.Id;
            newAccountRecurringPayment.RecurringPayment = await CreateRecurringPaymentAsync(accountRecurringPayment.RecurringPayment);
            
            return newAccountRecurringPayment;
        }

        public async Task<NewCustomerRecurringPayment<T>> CreateNewCustomerRecurringPaymentAsync<T>(NewCustomerRecurringPayment<T> customerRecurringPayment)
            where T : Account, new()
        {
            // Validate objects
            validationService.Validate(customerRecurringPayment);

            var newCustomerRecurringPayment = new NewCustomerRecurringPayment<T>();

            // Create the new Customer
            var customerService = serviceFactory.GetCustomerService();
            newCustomerRecurringPayment.Customer  = await customerService.CreateCustomerAsync(customerRecurringPayment.Customer);

            // Create the new Account, and make the payment
            customerRecurringPayment.Account.CustomerId = newCustomerRecurringPayment.Customer.Id;
            var paymentResult = await CreateNewAccountRecurringPaymentAsync<T>(customerRecurringPayment);

            newCustomerRecurringPayment.Account = paymentResult.Account;
            newCustomerRecurringPayment.RecurringPayment = paymentResult.RecurringPayment;
            
            return newCustomerRecurringPayment;
        }
        
        public async Task<RecurringPayment> CreateRecurringPaymentAsync(RecurringPayment recurringPayment)
        {
            validationService.Validate(recurringPayment);
            var endpoint = string.Format("{0}{1}", settings.BaseUrl, Endpoints.RecurringPayment);
            var result = await webServiceRequest.PostDeserializedAsync<RecurringPayment, Result<RecurringPayment>>(new Uri(endpoint), recurringPayment);
            return result.Response;
        }

        public async Task DeletePaymentPlanAsync(int paymentPlanId)
        {
            var endpoint = string.Format("{0}{1}/{2}", settings.BaseUrl, Endpoints.PaymentPlan, paymentPlanId);
            await webServiceRequest.DeleteAsync(new Uri(endpoint));
        }

        public async Task DeleteRecurringPaymentAsync(int recurringPaymentId)
        {
            var endpoint = string.Format("{0}{1}/{2}", settings.BaseUrl, Endpoints.RecurringPayment, recurringPaymentId);
            await webServiceRequest.DeleteAsync(new Uri(endpoint));
        }

        public async Task<PagedResult<IEnumerable<PaymentSchedule>>> GetAllPaymentSchedulesAsync(int page = 1, int pageSize = 200)
        {
            var endpoint = $"{settings.BaseUrl}{Endpoints.PaymentSchedule}?page={page}&pagesize={pageSize}";
            var result = await webServiceRequest.GetDeserializedAsync<Result<IEnumerable<PaymentSchedule>>>(new Uri(endpoint));
            return PagedResult.ConvertToPagedResult<IEnumerable<PaymentSchedule>>(result);
        }

        public async Task<PagedResult<IEnumerable<Payment>>> GetPaymentPlanPaymentsAsync(int paymentPlanId)
        {
            var endpoint = string.Format("{0}{1}/{2}/payments", settings.BaseUrl, Endpoints.PaymentPlan, paymentPlanId);
            var result = await webServiceRequest.GetDeserializedAsync<Result<IEnumerable<Payment>>>(new Uri(endpoint));
            return PagedResult.ConvertToPagedResult<IEnumerable<Payment>>(result);
        }

        public async Task<PaymentPlan> GetPaymentPlanScheduleAsync(int paymentPlanId)
        {
            var endpoint = string.Format("{0}{1}/{2}", settings.BaseUrl, Endpoints.PaymentPlan, paymentPlanId);
            var result = await webServiceRequest.GetDeserializedAsync<Result<PaymentPlan>>(new Uri(endpoint));
            return result.Response;
        }

        public async Task<PagedResult<IEnumerable<Payment>>> GetRecurringSchedulePaymentsAsync(int recurringPaymentId)
        {
            var endpoint = string.Format("{0}{1}/{2}/payments", settings.BaseUrl, Endpoints.RecurringPayment, recurringPaymentId);
            var result = await webServiceRequest.GetDeserializedAsync<Result<IEnumerable<Payment>>>(new Uri(endpoint));
            return PagedResult.ConvertToPagedResult<IEnumerable<Payment>>(result);
        }

        public async Task<RecurringPayment> GetRecurringScheduleAsync(int recurringPaymentId)
        {
            var endpoint = string.Format("{0}{1}/{2}", settings.BaseUrl, Endpoints.RecurringPayment, recurringPaymentId);
            var result = await webServiceRequest.GetDeserializedAsync<Result<RecurringPayment>>(new Uri(endpoint));
            return result.Response;
        }

        public async Task PausePaymentPlanAsync(int paymentPlanId, DateTime endDate)
        {
            var endpoint = string.Format("{0}{1}/{2}/pause?enddate={3}", settings.BaseUrl, Endpoints.PaymentPlan, paymentPlanId, endDate.ToString("yyyy-MM-dd"));
            await webServiceRequest.PutAsync(new Uri(endpoint));
        }

        public async Task PauseRecurringPaymentAsync(int recurringPaymentId, DateTime endDate)
        {
            var endpoint = string.Format("{0}{1}/{2}/pause?enddate={3}", settings.BaseUrl, Endpoints.RecurringPayment, recurringPaymentId, endDate.ToString("yyyy-MM-dd"));
            await webServiceRequest.PutAsync(new Uri(endpoint));
        }

        public async Task ResumePaymentPlanAsync(int paymentPlanId)
        {
            var endpoint = string.Format("{0}{1}/{2}/resume", settings.BaseUrl, Endpoints.PaymentPlan, paymentPlanId);
            await webServiceRequest.PutAsync(new Uri(endpoint));
        }

        public async Task ResumeRecurringPaymentAsync(int recurringPaymentId)
        {
            var endpoint = string.Format("{0}{1}/{2}/resume", settings.BaseUrl, Endpoints.RecurringPayment, recurringPaymentId);
            await webServiceRequest.PutAsync(new Uri(endpoint));
        }

        public async Task SuspendPaymentPlanAsync(int paymentPlanId)
        {
            var endpoint = string.Format("{0}{1}/{2}/suspend", settings.BaseUrl, Endpoints.PaymentPlan, paymentPlanId);
            await webServiceRequest.PutAsync(new Uri(endpoint));
        }

        public async Task SuspendRecurringPaymentAsync(int recurringPaymentId)
        {
            var endpoint = string.Format("{0}{1}/{2}/suspend", settings.BaseUrl, Endpoints.RecurringPayment, recurringPaymentId);
            await webServiceRequest.PutAsync(new Uri(endpoint));
        }

        public async Task<RecurringPayment> UpdateRecurringPaymentAsync(RecurringPayment recurringPayment)
        {
            validationService.Validate(recurringPayment);
            var endpoint = string.Format("{0}{1}", settings.BaseUrl, Endpoints.RecurringPayment);
            var result = await webServiceRequest.PutDeserializedAsync<RecurringPayment, Result<RecurringPayment>>(new Uri(endpoint), recurringPayment);
            return result.Response;
        }
    }
}