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

using FluentValidation;

namespace PaySimpleSdk.Payments.Validation
{
    internal class PaymentValidator : AbstractValidator<Payment>
    {
        public PaymentValidator()
        {
	        RuleFor(m => m.AccountId).GreaterThan(0).WithMessage("AccountId must be a interger greater than 0");
            RuleFor(m => m.Amount).GreaterThan(0).WithMessage("Amount must be greater than 0.00");
			// would be nice to validate this based on card type and is 3 or 4 digits not both
			RuleFor(m => m.Cvv).Matches(@"^(|\d{3,4})$").WithMessage("CVV is invalid").When(m => string.IsNullOrEmpty(m.PaymentToken));
            RuleFor(m => m.InvoiceNumber).Length(0, 50).WithMessage("InvoiceNumber cannot exceed 50 characters");
            RuleFor(m => m.PurchaseOrderNumber).Length(0, 50).WithMessage("PurchaseOrderNumber cannot exceed 50 characters");
            RuleFor(m => m.OrderId).Length(0, 50).WithMessage("OrderId cannot exceed 50 characters");
            RuleFor(m => m.Description).Length(0, 2048).WithMessage("Description cannot exceed 2048 characters");
        }
    }
}