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
using System.Collections.Generic;

namespace PaySimpleSdk.Customers
{
    public class Address : IValidatable
    {
        [JsonProperty("StreetAddress1")]
        public string StreetAddress1 { get; set; }
        [JsonProperty("StreetAddress2")]
        public string StreetAddress2 { get; set; }
        [JsonProperty("City")]
        public string City { get; set; }
        [JsonProperty("StateCode"), JsonConverter(typeof(TypeEnumConverter<StateCode, BiLookup<StateCode, string>>))]
        public StateCode? StateCode { get; set; }
        [JsonProperty("ZipCode")]
        public string ZipCode { get; set; }
        [JsonProperty("Country"), JsonConverter(typeof(TypeEnumConverter<CountryCode, BiLookup<CountryCode, string>>))]
        public CountryCode? Country { get; set; }

        public IEnumerable<ValidationError> Validate()
        {
            return Validator.Validate<Address, AddressValidator>(this);
        }
    }
}