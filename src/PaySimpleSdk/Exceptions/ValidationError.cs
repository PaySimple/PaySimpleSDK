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

using FluentValidation.Results;
using System.Diagnostics.CodeAnalysis;

namespace PaySimpleSdk.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class ValidationError
    {
        public object AttemptedValue { get; set; }
        public object CustomState { get; set; }
        public string ErrorMessage { get; set; }
        public string PropertyName { get; set; }
        public ValidationError(ValidationFailure error)
        {
            this.AttemptedValue = error.AttemptedValue;
            this.CustomState = error.CustomState;
            this.ErrorMessage = error.ErrorMessage;
            this.PropertyName = error.PropertyName;
        }

        public ValidationError(string propertyName, string error)
        {
            this.PropertyName = propertyName;
            this.ErrorMessage = error;
        }

        public ValidationError(string propertyName, string error, object attemptedValue)
        {
            this.PropertyName = propertyName;
            this.ErrorMessage = error;
            this.AttemptedValue = attemptedValue;
        }

        public ValidationError(string propertyName, string error, object attemptedValue, object customState)
        {
            this.PropertyName = propertyName;
            this.ErrorMessage = error;
            this.AttemptedValue = attemptedValue;
            this.CustomState = customState;
        }
    }
}