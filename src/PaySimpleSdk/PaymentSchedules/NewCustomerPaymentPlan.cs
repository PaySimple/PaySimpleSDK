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
using PaySimpleSdk.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySimpleSdk.PaymentSchedules
{
    public class NewCustomerPaymentPlan<T> : NewAccountPaymentPlan<T>, IValidatable
        where T : Account, new()
    {
        public Customer Customer { get; set; }

        public NewCustomerPaymentPlan()
        {
            this.Customer = new Customer();
            this.Account = new T();
            this.PaymentPlan = new PaymentPlan();
        }

        public virtual new IEnumerable<ValidationError> Validate()
        {
            var errors = new List<ValidationError>();
            errors.AddRange(Customer.Validate());
            errors.AddRange(Account.Validate());
            errors.AddRange(PaymentPlan.Validate());

            // Since Customer or Account Ids have not been created yet we need to remove 
            // those errors from the validation
            errors.RemoveAll(e => e.PropertyName == "CustomerId" || e.PropertyName == "AccountId");
            
            return errors;
        }
    }
}