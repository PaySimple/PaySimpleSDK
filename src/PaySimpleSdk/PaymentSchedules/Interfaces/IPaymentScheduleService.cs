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
using PaySimpleSdk.Models;
using PaySimpleSdk.Payments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaySimpleSdk.PaymentSchedules
{
    public interface IPaymentScheduleService
    {
        /// <summary>
        /// NOTE: PCI Compliance is the responsibility of the user of this SDK
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="accountPaymentPlan"></param>
        /// <returns></returns>
        Task<NewAccountPaymentPlan<T>> CreateNewAccountPaymentPlanAsync<T>(NewAccountPaymentPlan<T> accountPaymentPlan) where T : Account, new();
        /// <summary>
        /// NOTE: PCI Compliance is the responsibility of the user of this SDK
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="customerPaymentPlan"></param>
        /// <returns></returns>
        Task<NewCustomerPaymentPlan<T>> CreateNewCustomerPaymentPlanAsync<T>(NewCustomerPaymentPlan<T> customerPaymentPlan) where T : Account, new();
        /// <summary>
        /// NOTE: PCI Compliance is the responsibility of the user of this SDK
        /// </summary>
        /// <param name="paymentPlan"></param>
        /// <returns></returns>
        Task<PaymentPlan> CreatePaymentPlanAsync(PaymentPlan paymentPlan);
        /// <summary>
        /// NOTE: PCI Compliance is the responsibility of the user of this SDK
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="accountRecurringPayment"></param>
        /// <returns></returns>
        Task<NewAccountRecurringPayment<T>> CreateNewAccountRecurringPaymentAsync<T>(NewAccountRecurringPayment<T> accountRecurringPayment) where T : Account, new();
        /// <summary>
        /// NOTE: PCI Compliance is the responsibility of the user of this SDK
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="customerRecurringPayment"></param>
        /// <returns></returns>
        Task<NewCustomerRecurringPayment<T>> CreateNewCustomerRecurringPaymentAsync<T>(NewCustomerRecurringPayment<T> customerRecurringPayment) where T : Account, new();
        /// <summary>
        /// NOTE: PCI Compliance is the responsibility of the user of this SDK
        /// </summary>
        /// <param name="recurringPayment"></param>
        /// <returns></returns>
        Task<RecurringPayment> CreateRecurringPaymentAsync(RecurringPayment recurringPayment);
        Task DeletePaymentPlanAsync(int paymentPlanId);
        Task DeleteRecurringPaymentAsync(int recurringPaymentId);
        Task<PagedResult<IEnumerable<PaymentSchedule>>> GetAllPaymentSchedulesAsync(int page = 1, int pageSize = 200);
        Task<PagedResult<IEnumerable<Payment>>> GetPaymentPlanPaymentsAsync(int paymentPlanId);
        Task<PaymentPlan> GetPaymentPlanScheduleAsync(int paymentPlanId);
        Task<PagedResult<IEnumerable<Payment>>> GetRecurringSchedulePaymentsAsync(int recurringPaymentId);
        Task<RecurringPayment> GetRecurringScheduleAsync(int recurringPaymentId);
        Task PausePaymentPlanAsync(int paymentPlanId, DateTime endDate);
        Task PauseRecurringPaymentAsync(int recurringPaymentId, DateTime endDate);
        Task ResumePaymentPlanAsync(int paymentPlanId);
        Task ResumeRecurringPaymentAsync(int recurringPaymentId);
        Task SuspendPaymentPlanAsync(int paymentPlanId);
        Task SuspendRecurringPaymentAsync(int recurringPaymentId);
        Task<RecurringPayment> UpdateRecurringPaymentAsync(RecurringPayment recurringPayment);
    }
}