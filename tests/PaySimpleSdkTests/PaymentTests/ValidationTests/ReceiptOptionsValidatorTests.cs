﻿#region License
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

using PaySimpleSdk.Payments;
using PaySimpleSdk.Payments.Validation;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

namespace PaySimpleSdkTests.PaymentTests.ValidationTests
{
    [ExcludeFromCodeCoverage]
    public class ReceiptOptionsValidatorTests
    {
        [Fact]
        public void SendToOtherAddresses_Address_Is_Not_Valid_Generates_Error()
        {
            // Arrange
            var validator = new ReceiptOptionsValidator();
            var receiptOptions = new ReceiptOptions { SendToOtherAddresses = new List<string> { "scooper@caltech.edu", "hwolowitz@caltech", "lhofstadter@caltech.edu" } };

            // Act
            var result = validator.Validate(receiptOptions);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "SendToOtherAddresses is invalid"));
        }

        [Fact]
        public void SendToOtherAddresses_Empty_Is_Valid()
        {
            // Arrange
            var validator = new ReceiptOptionsValidator();
            var receiptOptions = new ReceiptOptions { SendToOtherAddresses = new List<string>() };

            // Act
            var result = validator.Validate(receiptOptions);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "SendToOtherAddresses is invalid"));
        }

        [Fact]
        public void SendToOtherAddresses_One_Email_Address_Is_Valid()
        {
            // Arrange
            var validator = new ReceiptOptionsValidator();
            var receiptOptions = new ReceiptOptions { SendToOtherAddresses = new List<string> { "scooper@caltech.edu" } };

            // Act
            var result = validator.Validate(receiptOptions);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "SendToOtherAddresses is invalid.  Using brackets, enter valid email addresses seperated by commas"));
        }

        [Fact]
        public void SendToOtherAddresses_Multiple_Email_Addresses_Is_Valid()
        {
            // Arrange
            var validator = new ReceiptOptionsValidator();
            var receiptOptions = new ReceiptOptions { SendToOtherAddresses = new List<string> { "scooper@caltech.edu", "hwolowitz@caltech.edu", "lhofstadter@caltech.edu" } };

            // Act
            var result = validator.Validate(receiptOptions);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "SendToOtherAddresses is invalid"));
        }
    }
}