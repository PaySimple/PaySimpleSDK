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

using Newtonsoft.Json;
using PaySimpleSdk.Customers.Validation;
using PaySimpleSdk.Exceptions;
using PaySimpleSdk.Helpers;
using PaySimpleSdk.Validation;
using System;
using System.Collections.Generic;

namespace PaySimpleSdk.Customers
{
    public class Customer : IValidatable
    {
        public Customer()
        {
            this.ShippingSameAsBilling = true;
        }

        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("CustomerAccount")]
        public string CustomerAccount { get; set; }
        [JsonProperty("FirstName")]
        public string FirstName { get; set; }
        [JsonProperty("MiddleName")]
        public string MiddleName { get; set; }
        [JsonProperty("LastName")]
        public string LastName { get; set; }
        [JsonProperty("BillingAddress")]
        public Address BillingAddress { get; set; }
        [JsonProperty("ShippingSameAsBilling")]
        public bool ShippingSameAsBilling { get; set; }
        [JsonProperty("ShippingAddress")]
        public Address ShippingAddress { get; set; }
        [JsonProperty("Company")]
        public string Company { get; set; }
        [JsonProperty("Phone")]
        public string Phone { get; set; }
        [JsonProperty("AltPhone")]
        public string AltPhone { get; set; }
        [JsonProperty("MobilePhone")]
        public string MobilePhone { get; set; }
        [JsonProperty("Fax")]
        public string Fax { get; set; }
        [JsonProperty("Email")]
        public string Email { get; set; }
        [JsonProperty("AltEmail")]
        public string AltEmail { get; set; }
        [JsonProperty("Website")]
        public string Website { get; set; }
        [JsonProperty("Notes")]
        public string Notes { get; set; }
        [JsonProperty("CreatedOn"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime? CreatedOn { get; set; }
        [JsonProperty("LastModified"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime? LastModified { get; set; }

        public IEnumerable<ValidationError> Validate()
        {
            var errors = new List<ValidationError>();
            errors.AddRange(Validator.Validate<Customer, CustomerValidator>(this));

            if (BillingAddress != null)
                errors.AddRange(this.BillingAddress.Validate());

            if (ShippingAddress != null)
                errors.AddRange(this.ShippingAddress.Validate());

            return errors;
        }
    }
}