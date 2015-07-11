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
using PaySimpleSdk.Helpers;
using System;

namespace PaySimpleSdk.PaymentSchedules.Validation
{
    internal class RecurringPaymentValidator<T> : AbstractValidator<T>
        where T : RecurringPayment
    {
        public RecurringPaymentValidator()
        {
            RuleFor(m => m.AccountId).GreaterThan(0).WithMessage("AccountId is required");
            RuleFor(m => m.PaymentAmount).GreaterThan(0.00M).When(m => m.GetType() != typeof(PaymentPlan)).WithMessage("PaymentAmount must be greater than 0");
            RuleFor(m => m.StartDate).NotNull().WithMessage("StartDate is required").Must(m => m >= DateTime.Now).WithMessage("StartDate must be in the future");
            RuleFor(m => m.EndDate).Must((m, d) => !d.HasValue || d > m.StartDate).WithMessage("EndDate must be after StartDate");
            RuleFor(m => m.InvoiceNumber).Length(0, 50).WithMessage("InvoiceNumber cannot exceed 50 characters");
            RuleFor(m => m.OrderId).Length(0, 50).WithMessage("OrderId cannot exceed 50 characters");
            RuleFor(m => m.FirstPaymentDate).Must((m, d) => m.StartDate > d && d >= DateTime.Now.Date).When(m => m.FirstPaymentDate.HasValue).WithMessage("FirstPaymentDate must be a current or future date and must be a date before StartDate");
            RuleFor(m => m.ExecutionFrequencyParameter).InclusiveBetween(1, 7).When(m => m.ExecutionFrequencyType == ExecutionFrequencyType.Weekly || m.ExecutionFrequencyType == ExecutionFrequencyType.BiWeekly).WithMessage("ExecutionFrequencyParameter must be a value 1-7 (Sunday - Saturday), when ExecutionFrequencyType is Weekly or BiWeekly");
            RuleFor(m => m.ExecutionFrequencyParameter).InclusiveBetween(1, 31).When(m => m.ExecutionFrequencyType == ExecutionFrequencyType.SpecificDayOfMonth).WithMessage("ExecutionFrequencyParameter must be a value 1-7 (Sunday - Saturday), when ExecutionFrequencyType is SpecificDayOfMonth.  Note: If you want to bill on the 30th or 31st, use the LastDayOfMonth ExecutionFrequencyType");
            RuleFor(m => m.Description).Length(0, 2048).WithMessage("Description can not exceed 2048 characters");
        }
    }

    internal class PaymentPlanValidator : RecurringPaymentValidator<PaymentPlan>
    {
        public PaymentPlanValidator()
        {
            RuleFor(m => m.TotalDueAmount).GreaterThan(0.00M).WithMessage("TotalDueAmount must be greater than 0");
            RuleFor(m => m.TotalNumberOfPayments).InclusiveBetween(1, 99).WithMessage("TotalNumberOfPayments must be a integer between 1 and 99");
        }
    }
}