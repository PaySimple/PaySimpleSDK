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
using PaySimpleSdk.PaymentSchedules;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaySimpleSdk.Customers
{
    public interface ICustomerService
    {
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(int customerId);
        Task<IEnumerable<Ach>> GetAchAccountsAsync(int customerId);
		Task<AccountList> GetAllAccountsAsync(int customerId);
		Task<IEnumerable<CreditCard>> GetCreditCardAccountsAsync(int customerId);
        Task<Customer> GetCustomerAsync(int customerId);
        Task<PagedResult<IEnumerable<Customer>>> GetCustomersAsync(CustomerSort sortBy = CustomerSort.LastName, SortDirection direction = SortDirection.ASC, int page = 1, int pageSize = 200, bool lite = false);
		Task<Ach> GetDefaultAchAccountAsync(int customerId);
		Task<CreditCard> GetDefaultCreditCardAccountAsync(int customerId);
        Task<PagedResult<IEnumerable<PaymentPlan>>> GetPaymentPlansAsync(int customerId, DateTime? startDate = null, DateTime? endDate = null, ScheduleStatus? status = null, ScheduleSort sortBy = ScheduleSort.Id, SortDirection direction = SortDirection.ASC, int page = 1, int pageSize = 200, bool lite = false);
        Task<PagedResult<IEnumerable<Payment>>> GetPaymentsAsync(int customerId, DateTime? startDate = null, DateTime? endDate = null, IEnumerable<PaymentStatus> status = null, PaymentSort sortBy = PaymentSort.PaymentId, SortDirection direction = SortDirection.ASC, int page = 1, int pageSize = 200, bool lite = false);
        Task<PagedResult<PaymentScheduleList>> GetPaymentSchedulesAsync(int customerId, DateTime? startDate = null, DateTime? endDate = null, ScheduleStatus? status = null, ScheduleSort sortBy = ScheduleSort.Id, SortDirection direction = SortDirection.ASC, int page = 1, int pageSize = 200, bool lite = false);
        Task<PagedResult<IEnumerable<RecurringPayment>>> GetRecurringPaymentSchedulesAsync(int customerId, DateTime? startDate = null, DateTime? endDate = null, ScheduleStatus? status = null, ScheduleSort sortBy = ScheduleSort.Id, SortDirection direction = SortDirection.ASC, int page = 1, int pageSize = 200, bool lite = false);
        Task SetDefaultAccountAsync(int customerId, int accountId);
        Task<Customer> UpdateCustomerAsync(Customer customer);
		/// <summary>
		/// Will match or create a new customer and credit card account.
		/// NOTE: PCI Compliance is the responsibility of the user of this SDK
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		Task<PaymentToken> MatchOrCreateCustomerAndCreditCardAccountAsync(CustomerAndAccountRequest request);
		/// <summary>
		/// Will match or create a new customer and ach account.
		/// NOTE: PCI Compliance is the responsibility of the user of this SDK
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		Task<PaymentToken> MatchOrCreateCustomerAndAchAccountAsync(CustomerAndAccountRequest request);

        Task<TokenResponse> GetCustomerTokenAsync(int customerId);

    }
}