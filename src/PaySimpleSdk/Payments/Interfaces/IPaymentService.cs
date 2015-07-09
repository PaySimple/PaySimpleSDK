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
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaySimpleSdk.Payments
{
    public interface IPaymentService
    {
        //Task<Result<NewAccountPayment<T>>> CreateNewAccountPaymentAsync<T>(NewAccountPayment<T> accountPayment) where T : Account, new();
        //Task<Result<NewCustomerPayment<T>>> CreateNewCustomerPaymentAsync<T>(NewCustomerPayment<T> customerPayment) where T : Account, new();
        Task<Payment> CreatePaymentAsync(Payment payment);
        Task<Payment> GetPaymentAsync(int paymentId);
        Task<PagedResult<IEnumerable<Payment>>> GetPaymentsAsync(DateTime? startDate = null, DateTime? endDate = null, IEnumerable<PaymentStatus> status = null, PaymentSort sortBy = PaymentSort.PaymentId, SortDirection direction = SortDirection.DESC, int page = 1, int pageSize = 200, bool lite = false);
        Task<Payment> ReversePaymentAsync(int paymentId);
        Task<Payment> VoidPaymentAsync(int paymentId);
    }
}