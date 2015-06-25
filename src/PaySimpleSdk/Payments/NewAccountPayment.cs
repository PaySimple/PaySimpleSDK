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
using PaySimpleSdk.Exceptions;
using PaySimpleSdk.Validation;
using System.Collections.Generic;

namespace PaySimpleSdk.Payments
{
    public class NewAccountPayment<T> : IValidatable
        where T : Account, new()
    {
        public T Account { get; set; }
        public Payment Payment { get; set; }

        public NewAccountPayment()
        {
            this.Account = new T();
            this.Payment = new Payment();
        }

        public virtual IEnumerable<ValidationError> Validate()
        {
            var errors = new List<ValidationError>();
            errors.AddRange(Account.Validate());
            errors.AddRange(Payment.Validate());

            // Since Account Ids have not been created yet we need to remove 
            // those errors from the validation
            errors.RemoveAll(e => e.PropertyName == "AccountId");

            return errors;
        }
    }
}