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
using System.Text;
using Xunit;

namespace PaySimpleSdkTests.CustomerTests.ValidationTests
{
    [ExcludeFromCodeCoverage]
    public class CustomerValidatorTests
    {
        [Fact]
        public void FirstName_Is_Empty_Generates_Error()
        {
            // Arrange            
            var customer = new Customer { FirstName = "" };
            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "FirstName is required"));
        }

        [Fact]
        public void FirstName_Exceeds_150_Characters_Generates_Error()
        {
            // Arrange            
            var customer = new Customer
            {
                FirstName = "kkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk"
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "FirstName cannot exceed 150 characters"));
        }

        // *************************************************************************************************

        [Fact]
        public void LastName_Is_Empty_Generates_Error()
        {
            // Arrange            
            var customer = new Customer { LastName = "" };
            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "LastName is required"));
        }

        [Fact]
        public void LastName_Exceeds_150_Characters_Generates_Error()
        {
            // Arrange            
            var customer = new Customer
            {
                LastName = "kkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk"
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "LastName cannot exceed 150 characters"));
        }

        // *************************************************************************************************

        [Fact]
        public void MiddleName_Exceeds_150_Characters_Generates_Error()
        {
            // Arrange            
            var customer = new Customer
            {
                MiddleName = "kkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk"
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "MiddleName cannot exceed 150 characters"));
        }

        // *************************************************************************************************      

        [Fact]
        public void ShippingSameAsBilling_Is_True_Shipping_Address_Is_Null_Generates_Error()
        {
            // Arrange            
            var customer = new Customer
            {
                ShippingAddress = null,
                ShippingSameAsBilling = false
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "ShippingAddress is required if ShippingSameAsBilling is set to false"));
        }

        // *************************************************************************************************

        [Fact]
        public void Company_Exceeds_50_Characters_Generates_Error()
        {
            // Arrange            
            var customer = new Customer
            {
                Company = "kkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk"
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "Company cannot exceed 50 characters"));
        }

        // *************************************************************************************************

        [Fact]
        public void CustomerAccount_Exceeds_28_Characters_Generates_Error()
        {
            // Arrange            
            var customer = new Customer
            {
                CustomerAccount = "kkkkkkkkkkkkkkkkkkkkkkkkkkkkkk"
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "CustomerAccount cannot exceed 28 characters"));
        }

        // *************************************************************************************************

        [Fact]
        public void Phone_Has_Spaces_Generates_Error()
        {
            // Arrange            
            var customer = new Customer
            {
                Phone = "714 524 1234"
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "Phone is a maximum of 10 characters. Do not include paranthesis, spaces or dashes"));
        }

        [Fact]
        public void Phone_Has_Paranthesis_Generates_Error()
        {
            // Arrange            
            var customer = new Customer
            {
                Phone = "(714)5241234"
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "Phone is a maximum of 10 characters. Do not include paranthesis, spaces or dashes"));
        }

        [Fact]
        public void Phone_Has_Dashes_Generates_Error()
        {
            // Arrange            
            var customer = new Customer
            {
                Phone = "714-524-1234"
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "Phone is a maximum of 10 characters. Do not include paranthesis, spaces or dashes"));
        }


        [Fact]
        public void Phone_Has_Spaces_Paranthesis_And_Dashes_Generates_Error()
        {
            // Arrange            
            var customer = new Customer
            {
                Phone = "(714) 524-1234"
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "Phone is a maximum of 10 characters. Do not include paranthesis, spaces or dashes"));
        }

        [Fact]
        public void Phone_Is_Numeric_10_Digit_String_Is_Valid()
        {
            // Arrange            
            var customer = new Customer
            {
                Phone = "7145241234"
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "Phone is a maximum of 10 characters. Do not include paranthesis, spaces or dashes"));
        }

        // *************************************************************************************************

        [Fact]
        public void AltPhone_Has_Spaces_Generates_Error()
        {
            // Arrange            
            var customer = new Customer
            {
                AltPhone = "714 524 1234"
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "AltPhone is a maximum of 10 characters. Do not include paranthesis, spaces or dashes"));
        }

        [Fact]
        public void AltPhone_Has_Paranthesis_Generates_Error()
        {
            // Arrange            
            var customer = new Customer
            {
                AltPhone = "(714)5241234"
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "AltPhone is a maximum of 10 characters. Do not include paranthesis, spaces or dashes"));
        }

        [Fact]
        public void AltPhone_Has_Dashes_Generates_Error()
        {
            // Arrange            
            var customer = new Customer
            {
                AltPhone = "714-524-1234"
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "AltPhone is a maximum of 10 characters. Do not include paranthesis, spaces or dashes"));
        }


        [Fact]
        public void AltPhone_Has_Spaces_Paranthesis_And_Dashes_Generates_Error()
        {
            // Arrange            
            var customer = new Customer
            {
                AltPhone = "(714) 524-1234"
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "AltPhone is a maximum of 10 characters. Do not include paranthesis, spaces or dashes"));
        }

        [Fact]
        public void AltPhone_Is_Numeric_10_Digit_String_Is_Valid()
        {
            // Arrange            
            var customer = new Customer
            {
                AltPhone = "7145241234"
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "AltPhone is a maximum of 10 characters. Do not include paranthesis, spaces or dashes"));
        }

        // *************************************************************************************************

        [Fact]
        public void MobilePhone_Has_Spaces_Generates_Error()
        {
            // Arrange            
            var customer = new Customer
            {
                MobilePhone = "714 524 1234"
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "MobilePhone is a maximum of 10 characters. Do not include paranthesis, spaces or dashes"));
        }

        [Fact]
        public void MobilePhone_Has_Paranthesis_Generates_Error()
        {
            // Arrange            
            var customer = new Customer
            {
                MobilePhone = "(714)5241234"
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "MobilePhone is a maximum of 10 characters. Do not include paranthesis, spaces or dashes"));
        }

        [Fact]
        public void MobilePhone_Has_Dashes_Generates_Error()
        {
            // Arrange            
            var customer = new Customer
            {
                MobilePhone = "714-524-1234"
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "MobilePhone is a maximum of 10 characters. Do not include paranthesis, spaces or dashes"));
        }


        [Fact]
        public void MobilePhone_Has_Spaces_Paranthesis_And_Dashes_Generates_Error()
        {
            // Arrange            
            var customer = new Customer
            {
                MobilePhone = "(714) 524-1234"
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "MobilePhone is a maximum of 10 characters. Do not include paranthesis, spaces or dashes"));
        }

        [Fact]
        public void MobilePhone_Is_Numeric_10_Digit_String_Is_Valid()
        {
            // Arrange            
            var customer = new Customer
            {
                MobilePhone = "7145241234"
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "MobilePhone is a maximum of 10 characters. Do not include paranthesis, spaces or dashes"));
        }

        // *************************************************************************************************

        [Fact]
        public void Fax_Has_Spaces_Generates_Error()
        {
            // Arrange            
            var customer = new Customer
            {
                Fax = "714 524 1234"
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "Fax is a maximum of 10 characters. Do not include paranthesis, spaces or dashes"));
        }

        [Fact]
        public void Fax_Has_Paranthesis_Generates_Error()
        {
            // Arrange            
            var customer = new Customer
            {
                Fax = "(714)5241234"
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "Fax is a maximum of 10 characters. Do not include paranthesis, spaces or dashes"));
        }

        [Fact]
        public void Fax_Has_Dashes_Generates_Error()
        {
            // Arrange            
            var customer = new Customer
            {
                Fax = "714-524-1234"
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "Fax is a maximum of 10 characters. Do not include paranthesis, spaces or dashes"));
        }


        [Fact]
        public void Fax_Has_Spaces_Paranthesis_And_Dashes_Generates_Error()
        {
            // Arrange            
            var customer = new Customer
            {
                Fax = "(714) 524-1234"
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "Fax is a maximum of 10 characters. Do not include paranthesis, spaces or dashes"));
        }

        [Fact]
        public void Fax_Is_Numeric_10_Digit_String_Is_Valid()
        {
            // Arrange            
            var customer = new Customer
            {
                Fax = "7145241234"
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "Fax is a maximum of 10 characters. Do not include paranthesis, spaces or dashes"));
        }

        // *************************************************************************************************

        [Fact]
        public void Email_Is_Invalid_Generates_Error()
        {
            // Arrange            
            var customer = new Customer
            {
                Email = "scooper@"
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "Email is invalid"));
        }

        [Fact]
        public void Email_Exceeds_100_Characters_Generates_Error()
        {
            // Arrange            
            var customer = new Customer
            {
                Email = "scooper@caltech.eduuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuu"
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "Email cannot exceed 100 characters"));
        }

        [Fact]
        public void Email_Is_In_Valid_Format_Is_Valid()
        {
            // Arrange            
            var customer = new Customer
            {
                Email = "scooper@caltech.edu"
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "Email is invalid"));
        }

        // *************************************************************************************************

        [Fact]
        public void AltEmail_Is_Invalid_Generates_Error()
        {
            // Arrange            
            var customer = new Customer
            {
                AltEmail = "scooper@"
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "AltEmail is invalid"));
        }

        [Fact]
        public void AltEmail_Exceeds_100_Characters_Generates_Error()
        {
            // Arrange            
            var customer = new Customer
            {
                AltEmail = "scooper@caltech.eduuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuu"
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "AltEmail cannot exceed 100 characters"));
        }

        [Fact]
        public void AltEmail_Is_In_Valid_Format_Is_Valid()
        {
            // Arrange            
            var customer = new Customer
            {
                AltEmail = "scooper@caltech.edu"
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.False(result.Errors.Any(e => e.ErrorMessage == "AltEmail is invalid"));
        }

        // *************************************************************************************************

        [Fact]
        public void Notes_Exceed_2048_Characters_Generates_Error()
        {
            // Arrange            
            var notes = new StringBuilder();

            for (int i = 0; i < 2049; i++)
                notes.Append("k");

            var customer = new Customer
            {
                Notes = notes.ToString()
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assert
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "Notes cannot exceed 2048 characters"));
        }

        // *************************************************************************************************

        [Fact]
        public void BillingAddress_Is_Null_Is_Valid()
        {
            // Arrange
            var customer = new Customer
            {
                FirstName = "Sheldon",
                LastName = "Cooper",
                BillingAddress = null
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assertt
            Assert.False(result.Errors.Any());
        }

        // *************************************************************************************************

        [Fact]
        public void ShippingAddress_Is_Null_Is_Valid_When_ShippingSameAsBilling_Is_True()
        {
            // Arrange
            var customer = new Customer
            {
                FirstName = "Sheldon",
                LastName = "Cooper",
                ShippingSameAsBilling = true,
                ShippingAddress = null
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assertt
            Assert.False(result.Errors.Any());
        }

        [Fact]
        public void ShippingAddress_Is_Null_Is_Not_Valid_When_ShippingSameAsBilling_Is_False()
        {
            // Arrange
            var customer = new Customer
            {
                FirstName = "Sheldon",
                LastName = "Cooper",
                ShippingSameAsBilling = false,
                ShippingAddress = null
            };

            var validator = new CustomerValidator();

            // Act
            var result = validator.Validate(customer);

            // Assertt
            Assert.True(result.Errors.Any(e => e.ErrorMessage == "ShippingAddress is required if ShippingSameAsBilling is set to false"));
        }
        // *************************************************************************************************
    }
}