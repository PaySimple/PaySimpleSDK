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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaySimpleSdk.Accounts;
using PaySimpleSdk.Customers.Validation;
using PaySimpleSdk.Exceptions;
using PaySimpleSdk.Payments;
using PaySimpleSdk.Payments.Validation;
using PaySimpleSdk.Validation;

namespace PaySimpleSdk.Customers
{
	public class CustomerAndAccountRequest : IValidatable
	{
		public Customer Customer { get; set; }
		public Ach AchAccount { get; set; }
		public CreditCard CreditCardAccount { get; set; }
		public ProtectedCardData ProtectedCardData { get; set; }
		public IEnumerable<ValidationError> Validate()
		{
			return Validator.Validate<CustomerAndAccountRequest, CustomerAndAccountRequestValidator>(this);
		}
	}
}
