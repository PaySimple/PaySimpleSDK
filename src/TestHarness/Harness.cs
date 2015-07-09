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
using PaySimpleSdk.Customers;
using PaySimpleSdk.Exceptions;
using PaySimpleSdk.Helpers;
using PaySimpleSdk.Models;
using PaySimpleSdk.Payments;
using PaySimpleSdk.PaymentSchedules;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace TestHarness
{
    public class Harness
    {
        IPaySimpleSettings settings;
        IAccountService accountService;
        ICustomerService customerService;
        IPaymentService paymentService;
        IPaymentScheduleService paymentScheduleService;

        public Harness()
        {
            var apiKey = ConfigurationManager.AppSettings["apiKey"];
            var username = ConfigurationManager.AppSettings["username"];
            settings = new PaySimpleSettings(apiKey, username, "https://sandbox-api.paysimple.com");
            accountService = new AccountService(settings);
            customerService = new CustomerService(settings);
            paymentService = new PaymentService(settings);
            paymentScheduleService = new PaymentScheduleService(settings);
        }

        public async Task RunMethods()
        {
            try
            {
                await GetPaymentsAsync(pageSize: 29, page: 4);
                //await GetAllAccountsAsync(301606);
                // Run this for PaySimple Certification
                //await Certification();
            }
            catch (PaySimpleException ex)
            {
                DumpObject("PaySimpleException", ex.ValidationErrors);
            }
            catch (PaySimpleEndpointException ex)
            {
                DumpObject("PaySimpleEndpointException", ex.EndpointErrors);
            }
        }

        #region Certification

        public async Task Certification()
        {
            var customerId = await CustomerCertification();
            var accounts = await AccountCertification(customerId);
            await PaymentCertification(accounts.AchAccounts.First().Id, accounts.CreditCardAccounts.First().Id);
        }

        #endregion

        #region Account Service Methods

        public async Task<AccountList> AccountCertification(int customerId)
        {
            var accounts = new AccountList();

            try
            {
                // Create an ACH account
                var ach = new Ach
                {
                    CustomerId = customerId,
                    RoutingNumber = "307075259",
                    AccountNumber = "751111111",
                    BankName = "PaySimple Bank"
                };

                var createAchResult = await CreateAchAccountAsync(ach);

                // Add for payments certification
                accounts.AchAccounts = new List<Ach> { createAchResult };

                // Sample Error: Attempt to create the same ACH account
                var recreateAchResult = await CreateAchAccountAsync(ach);
            }
            catch (PaySimpleException ex)
            {
                DumpObject("PaySimpleException", ex.ValidationErrors);
            }
            catch (PaySimpleEndpointException ex)
            {
                // 400 Bad Request - Account Already Exists
                DumpObject("PaySimpleEndpointException", ex.EndpointErrors);
            }

            try
            {
                // Create an CreditCard account
                var creditCard = new CreditCard
                {
                    CustomerId = customerId,
                    CreditCardNumber = "371449635398456",
                    ExpirationDate = "12/2021",
                    Issuer = Issuer.Amex
                };

                var createCCResult = await CreateCreditCardAccountAsync(creditCard);

                // Add for payments certification
                accounts.CreditCardAccounts = new List<CreditCard> { createCCResult };

                var invalidCreditCard = new CreditCard
                {
                    CustomerId = customerId,
                    CreditCardNumber = "1111111111111111",
                    ExpirationDate = "12/2021",
                    Issuer = Issuer.Amex
                };

                // Sample Error: Attempt to create the same CreditCard account
                var invalidCCResult = await CreateCreditCardAccountAsync(invalidCreditCard);
            }
            catch (PaySimpleException ex)
            {
                DumpObject("PaySimpleException", ex.ValidationErrors);
            }
            catch (PaySimpleEndpointException ex)
            {
                // 400 Bad Request - Account Already Exists
                DumpObject("PaySimpleEndpointException", ex.EndpointErrors);
            }

            return accounts;
        }

        public async Task<Ach> CreateAchAccountAsync(Ach account)
        {
            var result = await accountService.CreateAchAccountAsync(account);

            if (result != null)
                DumpObject("CreateAchAccountAsync", result);

            return result;
        }

        public async Task<CreditCard> CreateCreditCardAccountAsync(CreditCard account)
        {
            var result = await accountService.CreateCreditCardAccountAsync(account);

            if (result != null)
                DumpObject("CreateCreditCardAccountAsync", result);

            return result;
        }

        public async Task DeleteAchAccountAsync(int accountId)
        {
            await accountService.DeleteAchAccountAsync(accountId);
        }

        public async Task DeleteCreditCardAccountAsync(int accountId)
        {
            await accountService.DeleteCreditCardAccountAsync(accountId);
        }

        public async Task<Ach> GetAchAccountAsync(int accountId)
        {
            var result = await accountService.GetAchAccountAsync(accountId);

            if (result != null)
                DumpObject("GetAchAccountAsync", result);

            return result;
        }

        public async Task<CreditCard> GetCreditCardAccountAsync(int accountId)
        {
            var result = await accountService.GetCreditCardAccountAsync(accountId);

            if (result != null)
                DumpObject("GetCreditCardAccountAsync", result);

            return result;
        }

        public async Task<Ach> UpdateAchAccountAsync(Ach account)
        {
            var result = await accountService.UpdateAchAccountAsync(account);

            if (result != null)
                DumpObject("UpdateAchAccountAsync", result);

            return result;
        }

        public async Task<CreditCard> UpdateCreditCardAccountAsync(CreditCard account)
        {
            var result = await accountService.UpdateCreditCardAccountAsync(account);

            if (result != null)
                DumpObject("UpdateCreditCardAccountAsync", result);

            return result;
        }

        #endregion

        #region Customer Service Methods

        public async Task<int> CustomerCertification()
        {
            var customerId = 0;

            try
            {
                // Create Customer
                var customer = new Customer
                {
                    FirstName = "Scott",
                    LastName = "Lance",
                    BillingAddress = new Address
                    {
                        StreetAddress1 = "205 W 700 S",
                        City = "Salt Lake City",
                        StateCode = StateCode.UT,
                        ZipCode = "84101",
                        Country = CountryCode.US
                    }
                };

                var results = await CreateCustomerAsync(customer);
                customerId = results.Id;

                // Create Invalid Customer
                var invalidCustomer = new Customer
                {
                    FirstName = "Scott",
                    LastName = "Lance",
                    BillingAddress = new Address
                    {
                        City = "Salt Lake City",
                        StateCode = StateCode.UT,
                        ZipCode = "84101",
                        Country = CountryCode.US
                    }
                };

                results = await CreateCustomerAsync(invalidCustomer);
            }
            catch (PaySimpleException ex)
            {
                DumpObject("PaySimpleException", ex.ValidationErrors);
            }
            catch (PaySimpleEndpointException ex)
            {
                DumpObject("PaySimpleEndpointException", ex.EndpointErrors);
            }

            return customerId;
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            var result = await customerService.CreateCustomerAsync(customer);

            if (result != null)
                DumpObject("CreateCustomerAsync", result);

            return result;
        }

        public async Task DeleteCustomerAsync(int customerId)
        {
            await customerService.DeleteCustomerAsync(customerId);
        }

        public async Task<IEnumerable<CustomerSearchResult>> FindCustomer(string query)
        {
            var result = await customerService.FindCustomerAsync(query);

            if (result != null)
                DumpObject("FindCustomer", result);

            return result;
        }

        public async Task<IEnumerable<Ach>> GetAchAccountsAsync(int customerId)
        {
            var result = await customerService.GetAchAccountsAsync(customerId);

            if (result != null)
                DumpObject("GetAchAccountsAsync", result);

            return result;
        }

        public async Task<AccountList> GetAllAccountsAsync(int customerId)
        {
            var result = await customerService.GetAllAccountsAsync(customerId);

            if (result != null)
                DumpObject("GetAllAccountsAsync", result);

            return result;
        }

        public async Task<IEnumerable<CreditCard>> GetCreditCardAccountsAsync(int customerId)
        {
            var result = await customerService.GetCreditCardAccountsAsync(customerId);

            if (result != null)
                DumpObject("GetCreditCardAccountsAsync", result);

            return result;
        }

        public async Task<Customer> GetCustomerAsync(int customerId)
        {
            var result = await customerService.GetCustomerAsync(customerId);

            if (result != null)
                DumpObject("GetCustomerAsync", result);

            return result;
        }

        public async Task<PagedResult<IEnumerable<Customer>>> GetCustomersAsync(CustomerSort sortBy = CustomerSort.LastName, SortDirection direction = SortDirection.ASC, int page = 1, int pageSize = 200, bool lite = false)
        {
            var result = await customerService.GetCustomersAsync(sortBy, direction, page, pageSize, lite);

            if (result != null)
                DumpObject("GetCustomersAsync", result);

            return result;
        }

        public async Task<Ach> GetDefaultAchAccountAsync(int customerId)
        {
            var result = await customerService.GetDefaultAchAccountAsync(customerId);

            if (result != null)
                DumpObject("GetDefaultAchAccountAsync", result);

            return result;
        }

        public async Task<CreditCard> GetDefaultCreditCardAccountAsync(int customerId)
        {
            var result = await customerService.GetDefaultCreditCardAccountAsync(customerId);

            if (result != null)
                DumpObject("GetDefaultCreditCardAccountAsync", result);

            return result;
        }

        public async Task<PagedResult<IEnumerable<PaymentPlan>>> GetPaymentPlansAsync(int customerId, DateTime? startDate = null, DateTime? endDate = null, ScheduleStatus status = ScheduleStatus.None, ScheduleSort sortBy = ScheduleSort.Id, SortDirection direction = SortDirection.ASC, int page = 1, int pageSize = 200, bool lite = false)
        {
            var result = await customerService.GetPaymentPlansAsync(customerId, startDate, endDate, status, sortBy, direction, page, pageSize, lite);

            if (result != null)
                DumpObject("GetPaymentPlansAsync", result);

            return result;
        }

        public async Task<PagedResult<IEnumerable<Payment>>> GetPaymentsAsync(int customerId, DateTime? startDate = null, DateTime? endDate = null, IEnumerable<PaymentStatus> status = null, PaymentSort sortBy = PaymentSort.PaymentId, SortDirection direction = SortDirection.ASC, int page = 1, int pageSize = 200, bool lite = false)
        {
            var result = await customerService.GetPaymentsAsync(customerId, startDate, endDate, status, sortBy, direction, page, pageSize, lite);

            if (result != null)
                DumpObject("GetPaymentsAsync", result);

            return result;
        }

        public async Task<PagedResult<PaymentScheduleList>> GetPaymentSchedulesAsync(int customerId, DateTime? startDate = null, DateTime? endDate = null, ScheduleStatus status = ScheduleStatus.None, ScheduleSort sortBy = ScheduleSort.Id, SortDirection direction = SortDirection.ASC, int page = 1, int pageSize = 200, bool lite = false)
        {
            var result = await customerService.GetPaymentSchedulesAsync(customerId, startDate, endDate, status, sortBy, direction, page, pageSize, lite);

            if (result != null)
                DumpObject("GetPaymentSchedulesAsync", result);

            return result;
        }

        public async Task<PagedResult<IEnumerable<RecurringPayment>>> GetRecurringPaymentSchedulesAsync(int customerId, DateTime? startDate = null, DateTime? endDate = null, ScheduleStatus status = ScheduleStatus.Active, ScheduleSort sortBy = ScheduleSort.Id, SortDirection direction = SortDirection.ASC, int page = 1, int pageSize = 200, bool lite = false)
        {
            var result = await customerService.GetRecurringPaymentSchedulesAsync(customerId, startDate, endDate, status, sortBy, direction, page, pageSize, lite);

            if (result != null)
                DumpObject("GetRecurringPaymentSchedulesAsync", result);

            return result;
        }

        public async Task SetDefaultAccountAsync(int customerId, int accountId)
        {
            await customerService.SetDefaultAccountAsync(customerId, accountId);
        }

        public async Task<Customer> UpdateCustomerAsync(Customer customer)
        {
            var result = await customerService.UpdateCustomerAsync(customer);

            if (result != null)
                DumpObject("UpdateCustomerAsync", result);

            return result;
        }

        #endregion

        #region Payment Service Methods

        public async Task PaymentCertification(int achAccountId, int creditCardAccountId)
        {
            try
            {
                var achPayment = new Payment
                {
                    AccountId = achAccountId,
                    Amount = 15.00M,
                    PaymentSubType = PaymentSubType.Web
                };

                // Make a payment
                await CreatePaymentAsync(achPayment);

                // Make the same payment again within 5 minutes
                await CreatePaymentAsync(achPayment);
            }
            catch (PaySimpleException ex)
            {
                DumpObject("PaySimpleException", ex.ValidationErrors);
            }
            catch (PaySimpleEndpointException ex)
            {
                DumpObject("PaySimpleEndpointException", ex.EndpointErrors);
            }

            try
            {
                // Make a valid payment
                var creditCardPayment = new Payment
                {
                    AccountId = creditCardAccountId,
                    Amount = 15.00M,
                    Cvv = "996"
                };

                var result = await CreatePaymentAsync(creditCardPayment);

                // Void the payment
                await VoidPaymentAsync(result.Id.Value);

                // Wrong CVV
                var failCreditCardPayment = new Payment
                {
                    AccountId = creditCardAccountId,
                    Amount = 10.00M,
                    PaymentSubType = PaymentSubType.Moto,
                    Cvv = "042"
                };

                // Make a payment with the wrong CVV
                await CreatePaymentAsync(failCreditCardPayment);

                // Zero Amount Payment
                var zeroAmountCreditCardPayment = new Payment
                {
                    AccountId = creditCardAccountId,
                    Amount = 0M,
                    PaymentSubType = PaymentSubType.Moto
                };

                // Make a payment with the wrong CVV
                await CreatePaymentAsync(zeroAmountCreditCardPayment);
            }
            catch (PaySimpleException ex)
            {
                DumpObject("PaySimpleException", ex.ValidationErrors);
            }
            catch (PaySimpleEndpointException ex)
            {
                DumpObject("PaySimpleEndpointException", ex.EndpointErrors);
            }
        }
        /*
                public async Task<Result<NewAccountPayment<T>>> CreateNewAccountPaymentAsync<T>(NewAccountPayment<T> accountPayment)
                    where T : Account, new()
                {
                    try
                    {
                        var result = await paymentService.CreateNewAccountPaymentAsync<T>(accountPayment);

                        if (result != null)
                            DumpObject("CreateNewAccountPaymentAsync", result);

                        return result;
                    }
                    catch (PaySimpleException ex)
                    {
                        DumpObject("PaySimpleException", ex.ValidationErrors);
                    }
                    catch (PaySimpleEndpointException ex)
                    {
                        DumpObject("PaySimpleEndpointException", ex.EndpointErrors);
                    }

                    return null;
                }

                public async Task<Result<NewCustomerPayment<T>>> CreateNewCustomerPaymentAsync<T>(NewCustomerPayment<T> customerPayment)
                    where T : Account, new()
                {
                    try
                    {
                        var result = await paymentService.CreateNewCustomerPaymentAsync<T>(customerPayment);

                        if (result != null)
                            DumpObject("CreateNewCustomerPaymentAsync", result);

                        return result;
                    }
                    catch (PaySimpleException ex)
                    {
                        DumpObject("PaySimpleException", ex.ValidationErrors);
                    }
                    catch (PaySimpleEndpointException ex)
                    {
                        DumpObject("PaySimpleEndpointException", ex.EndpointErrors);
                    }

                    return null;
                }
        */
        public async Task<Payment> CreatePaymentAsync(Payment payment)
        {
            var result = await paymentService.CreatePaymentAsync(payment);

            if (result != null)
                DumpObject("CreatePaymentAsync", result);

            return result;
        }

        public async Task<Payment> GetPaymentAsync(int paymentId)
        {
            var result = await paymentService.GetPaymentAsync(paymentId);

            if (result != null)
                DumpObject("GetPaymentAsync", result);

            return result;
        }

        public async Task<PagedResult<IEnumerable<Payment>>> GetPaymentsAsync(DateTime? startDate = null, DateTime? endDate = null, IEnumerable<PaymentStatus> status = null, PaymentSort sortBy = PaymentSort.PaymentId, SortDirection direction = SortDirection.DESC, int page = 1, int pageSize = 200, bool lite = false)
        {
            var result = await paymentService.GetPaymentsAsync(startDate, endDate, status, sortBy, direction, page, pageSize, lite);

            if (result != null)
                DumpObject("GetPaymentsAsync", result);

            return result;
        }

        public async Task<Payment> ReversePaymentAsync(int paymentId)
        {
            var result = await paymentService.ReversePaymentAsync(paymentId);

            if (result != null)
                DumpObject("ReversePaymentAsync", result);

            return result;
        }

        public async Task<Payment> VoidPaymentAsync(int paymentId)
        {
            var result = await paymentService.VoidPaymentAsync(paymentId);

            if (result != null)
                DumpObject("VoidPaymentAsync", result);

            return result;
        }

        #endregion

        #region Payment Schedule Service Methods
        /*
        public async Task<Result<NewAccountPaymentPlan<T>>> CreateNewAccountPaymentPlanAsync<T>(NewAccountPaymentPlan<T> accountPaymentPlan)
            where T : Account, new()
        {
            try
            {
                var result = await paymentScheduleService.CreateNewAccountPaymentPlanAsync<T>(accountPaymentPlan);

                if (result != null)
                    DumpObject("CreateNewAccountPaymentPlanAsync", result);

                return result;
            }
            catch (PaySimpleException ex)
            {
                DumpObject("PaySimpleException", ex.ValidationErrors);
            }
            catch (PaySimpleEndpointException ex)
            {
                DumpObject("PaySimpleEndpointException", ex.EndpointErrors);
            }

            return null;
        }

        public async Task<Result<NewCustomerPaymentPlan<T>>> CreateNewCustomerPaymentPlanAsync<T>(NewCustomerPaymentPlan<T> customerPaymentPlan)
            where T : Account, new()
        {
            try
            {
                var result = await paymentScheduleService.CreateNewCustomerPaymentPlanAsync<T>(customerPaymentPlan);

                if (result != null)
                    DumpObject("CreateNewCustomerPaymentPlanAsync", result);

                return result;
            }
            catch (PaySimpleException ex)
            {
                DumpObject("PaySimpleException", ex.ValidationErrors);
            }
            catch (PaySimpleEndpointException ex)
            {
                DumpObject("PaySimpleEndpointException", ex.EndpointErrors);
            }

            return null;
        }
*/
        public async Task<Result<PaymentPlan>> CreatePaymentPlanAsync(PaymentPlan paymentPlan)
        {
            var result = await paymentScheduleService.CreatePaymentPlanAsync(paymentPlan);

            if (result != null)
                DumpObject("CreatePaymentPlanAsync", result);

            return result;
        }
        /*
                public async Task<Result<NewAccountRecurringPayment<T>>> CreateNewAccountRecurringPaymentAsync<T>(NewAccountRecurringPayment<T> accountRecurringPayment)
                    where T : Account, new()
                {
                    try
                    {
                        var result = await paymentScheduleService.CreateNewAccountRecurringPaymentAsync<T>(accountRecurringPayment);

                        if (result != null)
                            DumpObject("CreateNewAccountRecurringPaymentAsync", result);

                        return result;
                    }
                    catch (PaySimpleException ex)
                    {
                        DumpObject("PaySimpleException", ex.ValidationErrors);
                    }
                    catch (PaySimpleEndpointException ex)
                    {
                        DumpObject("PaySimpleEndpointException", ex.EndpointErrors);
                    }

                    return null;
                }

                public async Task<Result<NewCustomerRecurringPayment<T>>> CreateNewCustomerRecurringPaymentAsync<T>(NewCustomerRecurringPayment<T> customerRecurringPayment)
                    where T : Account, new()
                {
                    try
                    {
                        var result = await paymentScheduleService.CreateNewCustomerRecurringPaymentAsync<T>(customerRecurringPayment);

                        if (result != null)
                            DumpObject("CreateNewCustomerRecurringPaymentAsync", result);

                        return result;
                    }
                    catch (PaySimpleException ex)
                    {
                        DumpObject("PaySimpleException", ex.ValidationErrors);
                    }
                    catch (PaySimpleEndpointException ex)
                    {
                        DumpObject("PaySimpleEndpointException", ex.EndpointErrors);
                    }

                    return null;
                }
        */
        public async Task<Result<RecurringPayment>> CreateRecurringPaymentAsync(RecurringPayment recurringPayment)
        {
            var result = await paymentScheduleService.CreateRecurringPaymentAsync(recurringPayment);

            if (result != null)
                DumpObject("CreateRecurringPaymentAsync", result);

            return result;
        }

        public async Task DeletePaymentPlanAsync(int paymentPlanId)
        {
            await paymentScheduleService.DeletePaymentPlanAsync(paymentPlanId);
        }

        public async Task DeleteRecurringPaymentAsync(int recurringPaymentId)
        {
            await paymentScheduleService.DeleteRecurringPaymentAsync(recurringPaymentId);
        }

        public async Task<Result<IEnumerable<RecurringPayment>>> GetAllPaymentSchedulesAsync()
        {
            var result = await paymentScheduleService.GetAllPaymentSchedulesAsync();

            if (result != null)
                DumpObject("GetAllPaymentSchedulesAsync", result);

            return result;
        }

        public async Task<Result<IEnumerable<Payment>>> GetPaymentPlanPaymentsAsync(int paymentPlanId)
        {
            var result = await paymentScheduleService.GetPaymentPlanPaymentsAsync(paymentPlanId);

            if (result != null)
                DumpObject("GetPaymentPlanPaymentsAsync", result);

            return result;
        }

        public async Task<Result<PaymentPlan>> GetPaymentPlanScheduleAsync(int paymentPlanId)
        {
            var result = await paymentScheduleService.GetPaymentPlanScheduleAsync(paymentPlanId);

            if (result != null)
                DumpObject("GetPaymentPlanScheduleAsync", result);

            return result;
        }

        public async Task<Result<IEnumerable<Payment>>> GetRecurringPaymentsAsync(int recurringPaymentId)
        {
            var result = await paymentScheduleService.GetRecurringPaymentsAsync(recurringPaymentId);

            if (result != null)
                DumpObject("GetRecurringPaymentsAsync", result);

            return result;
        }

        public async Task<Result<RecurringPayment>> GetRecurringScheduleAsync(int recurringPaymentId)
        {
            var result = await paymentScheduleService.GetRecurringScheduleAsync(recurringPaymentId);

            if (result != null)
                DumpObject("GetRecurringScheduleAsync", result);

            return result;
        }

        public async Task PausePaymentPlanAsync(int paymentPlanId, DateTime endDate)
        {
            await paymentScheduleService.PausePaymentPlanAsync(paymentPlanId, endDate);
        }

        public async Task PauseRecurringPaymentAsync(int recurringPaymentId, DateTime endDate)
        {
            await paymentScheduleService.PauseRecurringPaymentAsync(recurringPaymentId, endDate);
        }

        public async Task ResumePaymentPlanAsync(int paymentPlanId)
        {
            await paymentScheduleService.ResumePaymentPlanAsync(paymentPlanId);
        }

        public async Task ResumeRecurringPaymentAsync(int recurringPaymentId)
        {
            await paymentScheduleService.ResumeRecurringPaymentAsync(recurringPaymentId);
        }

        public async Task SuspendPaymentPlanAsync(int paymentPlanId)
        {
            await paymentScheduleService.SuspendPaymentPlanAsync(paymentPlanId);
        }

        public async Task SuspendRecurringPaymentAsync(int recurringPaymentId)
        {
            await paymentScheduleService.SuspendRecurringPaymentAsync(recurringPaymentId);
        }

        public async Task<Result<RecurringPayment>> UpdateRecurringPaymentAsync(RecurringPayment recurringPayment)
        {
            var result = await paymentScheduleService.UpdateRecurringPaymentAsync(recurringPayment);

            if (result != null)
                DumpObject("UpdateRecurringPaymentAsync", result);

            return result;
        }

        #endregion

        #region DumpObject
        private void DumpObject(string functionName, object obj)
        {
            var sep = "********************************************************************";

            Console.WriteLine(sep);
            Console.WriteLine((functionName + " ").PadRight(sep.Length, '*'));
            Console.WriteLine(sep);

            if (obj != null)
                obj.PrintDump();

            Console.WriteLine("");
        }
        #endregion
    }
}