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
    internal class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
            RuleFor(m => m.StreetAddress1).NotEmpty().WithMessage("StreetAddress1 is required").Length(0, 250).WithMessage("StreetAddress1 cannot exceed 250 characters");
            RuleFor(m => m.StreetAddress2).Length(0, 250).WithMessage("StreetAddress2 cannot exceed 250 characters");
            RuleFor(m => m.City).NotEmpty().WithMessage("City is required").Length(0, 100).WithMessage("City cannot exceed 100 characters");
            RuleFor(m => m.ZipCode).NotEmpty().WithMessage("ZipCode is required").PostalCode().WithMessage("ZipCode cannot exceed 10 characters");
        }
    }
}