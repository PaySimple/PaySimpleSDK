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

using PaySimpleSdk.Payments;
using PaySimpleSdk.Payments.Validation;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Xunit;

namespace PaySimpleSdkTests.PaymentTests.ValidationTests
{
    [ExcludeFromCodeCoverage]
    public class PaymentValidatorTests
    {
        [Fact]
        public void AccountId_Is_Zero_Generates_Error()
        {
            // Arrange
            var validator = new PaymentValidator();
            var payment = new Payment { AccountId = 0 };

            // Act
            var result = validator.Validate(payment);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "AccountId must be a interger greater than 0"));
        }

        [Fact]
        public void AccountId_Is_Greater_Than_Zero_Is_Valid()
        {
            // Arrange
            var validator = new PaymentValidator();
            var payment = new Payment { AccountId = 1 };

            // Act
            var result = validator.Validate(payment);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "AccountId must be a interger greater than 0"));
        }

        // *************************************************************************************************

        [Fact]
        public void Amount_Is_Zero_Generates_Error()
        {
            // Arrange
            var validator = new PaymentValidator();
            var payment = new Payment { Amount = 0.00M };

            // Act
            var result = validator.Validate(payment);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "Amount must be greater than 0.00"));
        }

        [Fact]
        public void Amount_Is_Greater_Than_Zero_Is_Valid()
        {
            // Arrange
            var validator = new PaymentValidator();
            var payment = new Payment { Amount = 0.01M };

            // Act
            var result = validator.Validate(payment);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "Amount must be greater than 0.00"));
        }

        // *************************************************************************************************

        [Fact]
        public void Cvv_Has_Alpha_Generates_Error()
        {
            // Arrange
            var validator = new PaymentValidator();
            var payment = new Payment { Cvv = "A1234" };

            // Act
            var result = validator.Validate(payment);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "CVV is invalid"));
        }

        [Fact]
        public void Cvv_Has_2_Digits_Generates_Error()
        {
            // Arrange
            var validator = new PaymentValidator();
            var payment = new Payment { Cvv = "12" };

            // Act
            var result = validator.Validate(payment);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "CVV is invalid"));
        }

        [Fact]
        public void Cvv_Has_5_Digits_Generates_Error()
        {
            // Arrange
            var validator = new PaymentValidator();
            var payment = new Payment { Cvv = "12345" };

            // Act
            var result = validator.Validate(payment);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "CVV is invalid"));
        }

        [Fact]
        public void Cvv_Is_Blank_Is_Valid()
        {
            // Arrange
            var validator = new PaymentValidator();
            var payment = new Payment { Cvv = "" };

            // Act
            var result = validator.Validate(payment);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "CVV is invalid"));
        }

        [Fact]
        public void Cvv_Is_3_Digits_Is_Valid()
        {
            // Arrange
            var validator = new PaymentValidator();
            var payment = new Payment { Cvv = "123" };

            // Act
            var result = validator.Validate(payment);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "CVV is invalid"));
        }

        [Fact]
        public void Cvv_Is_4_Digits_Is_Valid()
        {
            // Arrange
            var validator = new PaymentValidator();
            var payment = new Payment { Cvv = "1234" };

            // Act
            var result = validator.Validate(payment);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "CVV is invalid"));
        }

        // *************************************************************************************************

        [Fact]
        public void InvoiceNumber_Exceeds_50_Characters_Generates_Error()
        {
            // Arrange
            var validator = new PaymentValidator();
            var payment = new Payment { InvoiceNumber = "kkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk" };

            // Act
            var result = validator.Validate(payment);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "InvoiceNumber cannot exceed 50 characters"));
        }

        // *************************************************************************************************

        [Fact]
        public void PurchaseOrderNumber_Exceeds_50_Characters_Generates_Error()
        {
            // Arrange
            var validator = new PaymentValidator();
            var payment = new Payment { PurchaseOrderNumber = "kkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk" };

            // Act
            var result = validator.Validate(payment);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "PurchaseOrderNumber cannot exceed 50 characters"));
        }

        // *************************************************************************************************

        [Fact]
        public void OrderId_Exceeds_50_Characters_Generates_Error()
        {
            // Arrange
            var validator = new PaymentValidator();
            var payment = new Payment { OrderId = "kkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk" };

            // Act
            var result = validator.Validate(payment);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "OrderId cannot exceed 50 characters"));
        }

        // *************************************************************************************************

        [Fact]
        public void Description_Exceeds_2048_Characters_Generates_Error()
        {
            // Arrange
            var description = new StringBuilder();

            for (int i = 0; i < 2049; i++)
                description.Append("k");

            var validator = new PaymentValidator();
            var payment = new Payment { Description = description.ToString() };

            // Act
            var result = validator.Validate(payment);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "Description cannot exceed 2048 characters"));
        }
    }
}