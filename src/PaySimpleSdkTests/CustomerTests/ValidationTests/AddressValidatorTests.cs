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

using PaySimpleSdk.Customers;
using PaySimpleSdk.Customers.Validation;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

namespace PaySimpleSdkTests.CustomerTests.ValidationTests
{
    [ExcludeFromCodeCoverage]
    public class AddressValidatorTests
    {
        [Fact]
        public void StreetAddress1_Is_Empty_Generates_Error()
        {
            // Arrange
            var address = new Address { StreetAddress1 = "" };
            var validator = new AddressValidator();

            // Act
            var result = validator.Validate(address);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "StreetAddress1 is required"));
        }

        [Fact]
        public void StreetAddress1_Exceeds_250_Characters_Generates_Error()
        {
            // Arrange
            var address = new Address
            {
                StreetAddress1 = "kkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk"
            };

            var validator = new AddressValidator();

            // Act
            var result = validator.Validate(address);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "StreetAddress1 cannot exceed 250 characters"));
        }

        // *************************************************************************************************

        [Fact]
        public void StreetAddress2_Exceeds_250_Characters_Generates_Error()
        {
            // Arrange
            var address = new Address
            {
                StreetAddress2 = "kkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk"
            };

            var validator = new AddressValidator();

            // Act
            var result = validator.Validate(address);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "StreetAddress2 cannot exceed 250 characters"));
        }

        // *************************************************************************************************

        [Fact]
        public void City_Is_Empty_Generates_Error()
        {
            // Arrange
            var address = new Address { City = "" };
            var validator = new AddressValidator();

            // Act
            var result = validator.Validate(address);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "City is required"));
        }

        [Fact]
        public void City_Exceeds_100_Characters_Generates_Error()
        {
            // Arrange
            var address = new Address
            {
                City = "kkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk"
            };

            var validator = new AddressValidator();

            // Act
            var result = validator.Validate(address);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "City cannot exceed 100 characters"));
        }

        // *************************************************************************************************

        [Fact]
        public void ZipCode_Is_Empty_Generates_Error()
        {
            // Arrange
            var address = new Address { ZipCode = "" };
            var validator = new AddressValidator();

            // Act
            var result = validator.Validate(address);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "ZipCode is required"));
        }

        [Fact]
        public void ZipCode_Is_Not_Valid_Generates_Error()
        {
            // Arrange
            var validatior = new AddressValidator();
            var address = new Address { ZipCode = "789456" };

            // Act
            var result = validatior.Validate(address);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "ZipCode must be a valid US or CA postal code, acceptable formats are 11111, 11111-1111, A1A1A1, or A1A 1A1"));
        }

        [Fact]
        public void ZipCode_Is_5_Digits_Is_Valid()
        {
            // Arrange
            var validatior = new AddressValidator();
            var address = new Address { ZipCode = "84101" };

            // Act
            var result = validatior.Validate(address);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "ZipCode must be a valid US or CA postal code, acceptable formats are 11111, 11111-1111, A1A1A1, or A1A 1A1"));
        }

        [Fact]
        public void ZipCode_Is_5_Plus_4_Digits_Is_Valid()
        {
            // Arrange
            var validatior = new AddressValidator();
            var address = new Address { ZipCode = "84101-7331" };

            // Act
            var result = validatior.Validate(address);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "ZipCode must be a valid US or CA postal code, acceptable formats are 11111, 11111-1111, A1A1A1, or A1A 1A1"));
        }

        [Fact]
        public void ZipCode_Is_Canadian_Postal_Code_With_Space_Is_Valid()
        {
            // Arrange
            var validatior = new AddressValidator();
            var address = new Address { ZipCode = "L4L 9C8" };

            // Act
            var result = validatior.Validate(address);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "ZipCode must be a valid US or CA postal code, acceptable formats are 11111, 11111-1111, A1A1A1, or A1A 1A1"));
        }

        [Fact]
        public void ZipCode_Is_Canadian_Postal_Code_Without_Space_Is_Valid()
        {
            // Arrange
            var validatior = new AddressValidator();
            var address = new Address { ZipCode = "L4L9C8" };

            // Act
            var result = validatior.Validate(address);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "ZipCode must be a valid US or CA postal code, acceptable formats are 11111, 11111-1111, A1A1A1, or A1A 1A1"));
        }

        // *************************************************************************************************
    }
}