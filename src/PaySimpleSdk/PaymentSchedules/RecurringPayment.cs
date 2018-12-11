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
using PaySimpleSdk.Exceptions;
using PaySimpleSdk.Helpers;
using PaySimpleSdk.PaymentSchedules.Validation;
using PaySimpleSdk.Validation;
using System;
using System.Collections.Generic;

namespace PaySimpleSdk.PaymentSchedules
{
    public class RecurringPayment : IValidatable
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("AccountId")]
        public int AccountId { get; set; }
        [JsonProperty("CustomerId")]
        public string CustomerId { get; set; }
        [JsonProperty("PaymentAmount")]
        public decimal PaymentAmount { get; set; }
        [JsonProperty("CustomerFirstName")]
        public string CustomerFirstName { get; set; }
        [JsonProperty("CustomerLastName")]
        public string CustomerLastName { get; set; }
        [JsonProperty("CustomerCompany")]
        public string CustomerCompany { get; set; }
        [JsonProperty("PaymentSubType"), JsonConverter(typeof(TypeEnumConverter<PaymentSubType, BiLookup<PaymentSubType, string>>))]
        public PaymentSubType? PaymentSubType { get; set; }
        [JsonProperty("OrderId")]
        public string OrderId { get; set; }
        [JsonProperty("InvoiceNumber")]
        public string InvoiceNumber { get; set; }
        [JsonProperty("ExecutionFrequencyType")]
        public ExecutionFrequencyType ExecutionFrequencyType { get; set; }
        [JsonProperty("ExecutionFrequencyParameter")]
        public int ExecutionFrequencyParameter { get; set; }
        [JsonProperty("StartDate")]
        public DateTime? StartDate { get; set; }
        [JsonProperty("EndDate")]
        public DateTime? EndDate { get; set; }
        [JsonProperty("DateOfLastPaymentMade")]
        public DateTime? DateOfLastPaymentMade { get; set; }
        [JsonProperty("NextScheduleDate")]
        public DateTime? NextScheduleDate { get; set; }
        [JsonProperty("PauseUntilDate")]
        public DateTime? PauseUntilDate { get; set; }
        [JsonProperty("TotalAmountPaid")]
        public decimal TotalAmountPaid { get; set; }
        [JsonProperty("FirstPaymentAmount")]
        public decimal? FirstPaymentAmount { get; set; }
        [JsonProperty("FirstPaymentDate")]
        public DateTime? FirstPaymentDate { get; set; }
        [JsonProperty("FirstPaymentDone")]
        public bool FirstPaymentDone { get; set; }
        [JsonProperty("ScheduleStatus")]
        public ScheduleStatus ScheduleStatus { get; set; }
        [JsonProperty("Description")]
        public string Description { get; set; }
        [JsonProperty("CreatedOn")]
        public DateTime? CreatedOn { get; set; }
        [JsonProperty("LastModified")]
        public DateTime? LastModified { get; set; }

        public virtual IEnumerable<ValidationError> Validate()
        {
            return Validator.Validate<RecurringPayment, RecurringPaymentValidator<RecurringPayment>>(this);
        }
    }
}