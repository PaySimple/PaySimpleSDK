#region License
// The MIT License (MIT)
//
// Copyright (c) 2015 Scott Lance, Ethan Tipton
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

namespace PaySimpleSdk.Helpers
{
    public static class Endpoints
    {
        public const string AchAccount = "/v4/account/ach";
        public const string Customer = "/v4/customer";
        public const string CreditCardAccount = "/v4/account/creditcard";
        public const string GlobalSearch = "/v4/globalSearch";
        public const string Payment = "/v4/payment";
        public const string Credit = "/v4/payment/credit";
        public const string PaymentPlan = "/v4/paymentplan";
        public const string PaymentSchedule = "/v4/paymentschedule";
        public const string RecurringPayment = "/v4/recurringpayment";
        public const string PaymentToken = "/v4/paymenttoken";
        public const string CheckoutToken = "/v4/checkouttoken";
        public const string MatchOrCreateCustomerAndCreditCardAccount = "/v4/customer/matchcreditcard";
        public const string MatchOrCreateCustomerAndAchAccount = "/v4/customer/matchach";
        public const string CustomerToken = "/v4/customer/{0}/token";
    }
}