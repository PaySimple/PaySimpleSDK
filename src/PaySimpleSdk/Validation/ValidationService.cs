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
using PaySimpleSdk.Exceptions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace PaySimpleSdk.Validation
{
    [ExcludeFromCodeCoverage]
    internal class ValidationService : IValidationService
    {
        public void Validate(IValidatable model)
        {
            var result = model.Validate();

            if (result.Count() > 0)
                throw new PaySimpleException(result);
        }
    }

    [ExcludeFromCodeCoverage]
    internal static class Validator
    {
        internal static IEnumerable<ValidationError> Validate<T, V>(T obj)
            where T : class
            where V : AbstractValidator<T>, new()
        {
            var oav = new V();
            var validationResult = oav.Validate(obj);
            var result = validationResult.Errors.Select(e => new ValidationError(e.PropertyName, e.ErrorMessage, e.AttemptedValue, e.CustomState));
            return result;
        }
    }
}