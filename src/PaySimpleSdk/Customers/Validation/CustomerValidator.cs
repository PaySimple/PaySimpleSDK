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

namespace PaySimpleSdk.Customers.Validation
{
    internal class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(m => m.FirstName).NotEmpty().WithMessage("FirstName is required").Length(0, 150).WithMessage("FirstName cannot exceed 150 characters");
            RuleFor(m => m.LastName).NotEmpty().WithMessage("LastName is required").Length(0, 150).WithMessage("LastName cannot exceed 150 characters");
            RuleFor(m => m.MiddleName).Length(0, 150).WithMessage("MiddleName cannot exceed 150 characters");
            RuleFor(m => m.ShippingAddress).NotNull().When(c => !c.ShippingSameAsBilling).WithMessage("ShippingAddress is required if ShippingSameAsBilling is set to false");
            RuleFor(m => m.Company).Length(0, 50).WithMessage("Company cannot exceed 50 characters");
            RuleFor(m => m.CustomerAccount).Length(0, 28).WithMessage("CustomerAccount cannot exceed 28 characters");
            RuleFor(m => m.Phone).Phone().WithMessage("Phone is a maximum of 10 characters. Do not include paranthesis, spaces or dashes");
            RuleFor(m => m.AltPhone).Phone().WithMessage("AltPhone is a maximum of 10 characters. Do not include paranthesis, spaces or dashes");
            RuleFor(m => m.MobilePhone).Phone().WithMessage("MobilePhone is a maximum of 10 characters. Do not include paranthesis, spaces or dashes");
            RuleFor(m => m.Fax).Phone().WithMessage("Fax is a maximum of 10 characters. Do not include paranthesis, spaces or dashes");
            RuleFor(m => m.Email).EmailAddress().WithMessage("Email is invalid").Length(0, 100).WithMessage("Email cannot exceed 100 characters");
            RuleFor(m => m.AltEmail).EmailAddress().WithMessage("AltEmail is invalid").Length(0, 100).WithMessage("AltEmail cannot exceed 100 characters");
            RuleFor(m => m.Notes).Length(0, 2048).WithMessage("Notes cannot exceed 2048 characters");
        }
    }
}