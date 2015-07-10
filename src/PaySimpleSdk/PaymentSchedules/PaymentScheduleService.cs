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
        /*
        public async Task<Result<NewAccountPaymentPlan<T>>> CreateNewAccountPaymentPlanAsync<T>(NewAccountPaymentPlan<T> accountPaymentPlan)
            where T : Account, new()
        {
            // Validate objects
            validationService.Validate(accountPaymentPlan);

            var newAccountPaymentPlan = new NewAccountPaymentPlan<T>();

            // Create the account
            var accountService = serviceFactory.GetAccountService();
            var newAccountResult = await accountService.CreateAccountAsync<T>(accountPaymentPlan.Account);
            newAccountPaymentPlan.Account = newAccountResult.Response;

            // Make the Payment            
            accountPaymentPlan.PaymentPlan.AccountId = newAccountPaymentPlan.Account.Id;
            var newPayment = await CreatePaymentPlanAsync(accountPaymentPlan.PaymentPlan);
            newAccountPaymentPlan.PaymentPlan = newPayment.Response;

            var result = new Result<NewAccountPaymentPlan<T>>()
            {
                ResultData = newPayment.ResultData,
                Response = newAccountPaymentPlan
            };

            return result;
        }

        public async Task<Result<NewCustomerPaymentPlan<T>>> CreateNewCustomerPaymentPlanAsync<T>(NewCustomerPaymentPlan<T> customerPaymentPlan)
            where T : Account, new()
        {
            // Validate objects
            validationService.Validate(customerPaymentPlan);

            var newCustomerPaymentPlan = new NewCustomerPaymentPlan<T>();

            // Create the new Customer
            var customerService = serviceFactory.GetCustomerService();
            var createdCustomer = await customerService.CreateCustomerAsync(customerPaymentPlan.Customer);
            newCustomerPaymentPlan.Customer = createdCustomer.Response;

            // Create the new Account, and make the payment
            customerPaymentPlan.Account.CustomerId = createdCustomer.Response.Id;
            var paymentResult = await CreateNewAccountPaymentPlanAsync<T>(customerPaymentPlan);

            newCustomerPaymentPlan.Account = paymentResult.Response.Account;
            newCustomerPaymentPlan.PaymentPlan = paymentResult.Response.PaymentPlan;

            var result = new Result<NewCustomerPaymentPlan<T>>()
            {
                ResultData = paymentResult.ResultData,
                Response = newCustomerPaymentPlan
            };

            return result;
        }
        */
        public async Task<PaymentPlan> CreatePaymentPlanAsync(PaymentPlan paymentPlan)
        {
            validationService.Validate(paymentPlan);
            var endpoint = string.Format("{0}{1}", settings.BaseUrl, Endpoints.PaymentPlan);
            var result = await webServiceRequest.PostDeserializedAsync<PaymentPlan, Result<PaymentPlan>>(new Uri(endpoint), paymentPlan);
            return result.Response;
        }
        /*
        public async Task<Result<NewAccountRecurringPayment<T>>> CreateNewAccountRecurringPaymentAsync<T>(NewAccountRecurringPayment<T> accountRecurringPayment)
            where T : Account, new()
        {
            // Validate objects
            validationService.Validate(accountRecurringPayment);

            var newAccountRecurringPayment = new NewAccountRecurringPayment<T>();

            // Create the account
            var accountService = serviceFactory.GetAccountService();
            var newAccountResult = await accountService.CreateAccountAsync<T>(accountRecurringPayment.Account);
            newAccountRecurringPayment.Account = newAccountResult.Response;

            // Make the Payment            
            accountRecurringPayment.RecurringPayment.AccountId = newAccountRecurringPayment.Account.Id;
            var newPayment = await CreateRecurringPaymentAsync(accountRecurringPayment.RecurringPayment);
            newAccountRecurringPayment.RecurringPayment = newPayment.Response;

            var result = new Result<NewAccountRecurringPayment<T>>()
            {
                ResultData = newPayment.ResultData,
                Response = newAccountRecurringPayment
            };

            return result;
        }

        public async Task<Result<NewCustomerRecurringPayment<T>>> CreateNewCustomerRecurringPaymentAsync<T>(NewCustomerRecurringPayment<T> customerRecurringPayment)
            where T : Account, new()
        {
            // Validate objects
            validationService.Validate(customerRecurringPayment);

            var newCustomerRecurringPayment = new NewCustomerRecurringPayment<T>();

            // Create the new Customer
            var customerService = serviceFactory.GetCustomerService();
            var createdCustomer = await customerService.CreateCustomerAsync(customerRecurringPayment.Customer);
            newCustomerRecurringPayment.Customer = createdCustomer.Response;

            // Create the new Account, and make the payment
            customerRecurringPayment.Account.CustomerId = createdCustomer.Response.Id;
            var paymentResult = await CreateNewAccountRecurringPaymentAsync<T>(customerRecurringPayment);

            newCustomerRecurringPayment.Account = paymentResult.Response.Account;
            newCustomerRecurringPayment.RecurringPayment = paymentResult.Response.RecurringPayment;

            var result = new Result<NewCustomerRecurringPayment<T>>()
            {
                ResultData = paymentResult.ResultData,
                Response = newCustomerRecurringPayment
            };

            return result;
        }
        */
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

        public async Task<PagedResult<IEnumerable<RecurringPayment>>> GetAllPaymentSchedulesAsync()
        {
            var endpoint = string.Format("{0}{1}", settings.BaseUrl, Endpoints.PaymentSchedule);
            var result = await webServiceRequest.GetDeserializedAsync<Result<IEnumerable<RecurringPayment>>>(new Uri(endpoint));
            return PagedResult.ConvertToPagedResult<IEnumerable<RecurringPayment>>(result);
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

        public async Task<PagedResult<IEnumerable<Payment>>> GetRecurringPaymentsAsync(int recurringPaymentId)
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