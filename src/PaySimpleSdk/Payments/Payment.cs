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
using PaySimpleSdk.Payments.Validation;
using PaySimpleSdk.Validation;
using System;
using System.Collections.Generic;

namespace PaySimpleSdk.Payments
{
    public class Payment : IValidatable
    {
        [JsonProperty("Id")]
        public int? Id { get; set; }
        [JsonProperty("AccountId")]
        public int? AccountId { get; set; }
        [JsonProperty("Amount")]
        public decimal Amount { get; set; }
        [JsonProperty("IsDebit")]
        public bool IsDebit { get; set; }
        [JsonProperty("CVV")]
        public string Cvv { get; set; }
        [JsonProperty("PaymentSubType"), JsonConverter(typeof(TypeEnumConverter<PaymentSubType, BiLookup<PaymentSubType, string>>))]
        public PaymentSubType? PaymentSubType { get; set; }
        [JsonProperty("InvoiceId")]
        public int? InvoiceId { get; set; }
        [JsonProperty("InvoiceNumber")]
        public string InvoiceNumber { get; set; }
        [JsonProperty("PurchaseOrderNumber")]
        public string PurchaseOrderNumber { get; set; }
        [JsonProperty("OrderId")]
        public string OrderId { get; set; }
        [JsonProperty("Description")]
        public string Description { get; set; }
        [JsonProperty("Latitude")]
        public float? Latitude { get; set; }
        [JsonProperty("Longitude")]
        public float? Longitude { get; set; }
        [JsonProperty("SuccessReceiptOptions")]
        public ReceiptOptions SuccessReceiptOptions { get; set; }
        [JsonProperty("FaiureReceiptOptions")]
        public ReceiptOptions FailureReceiptOptions { get; set; }
        [JsonProperty("CustomerId")]
        public int CustomerId { get; set; }
        [JsonProperty("CustomerFirstName")]
        public string CustomerFirstName { get; set; }
        [JsonProperty("CustomerLastName")]
        public string CustomerLastName { get; set; }
        [JsonProperty("CustomerCompany")]
        public string CustomerCompany { get; set; }
        [JsonProperty("ReferenceId")]
        public int ReferenceId { get; set; }
        [JsonProperty("Status"), JsonConverter(typeof(TypeEnumConverter<Status, BiLookup<Status, string>>))]
        public Status? Status { get; set; }
        [JsonProperty("RecurringScheduleId")]
        public int RecurringScheduleId { get; set; }
        [JsonProperty("PaymentType"), JsonConverter(typeof(TypeEnumConverter<PaymentType, BiLookup<PaymentType, string>>))]
        public PaymentType PaymentType { get; set; }
        [JsonProperty("ProviderAuthCode")]
        public string ProviderAuthCode { get; set; }
        [JsonProperty("TraceNumber")]
        public string TraceNumber { get; set; }
        [JsonProperty("PaymentDate"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime? PaymentDate { get; set; }
        [JsonProperty("ReturnDate"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime? ReturnDate { get; set; }
        [JsonProperty("EstimatedSettleDate"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime? EstimatedSettleDate { get; set; }
        [JsonProperty("ActualSettledDate"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime? ActualSettleDate { get; set; }
        [JsonProperty("CanVoidUntil"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime? CanVoidUntil { get; set; }
        [JsonProperty("FailureData")]
        public FailureData FailureData { get; set; }
        [JsonProperty("IsDecline")]
        public bool IsDecline { get; set; }
        [JsonProperty("CreatedOn"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime? CreatedOn { get; set; }
        [JsonProperty("LastModified"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime? LastModified { get; set; }
        [JsonProperty("RequiresReceipt")]
        public bool RequiresReceipt { get; set; }
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string PaymentToken { get; set; }

		public IEnumerable<ValidationError> Validate()
        {
            var errors = new List<ValidationError>();
            errors.AddRange(Validator.Validate<Payment, PaymentValidator>(this));

            if (SuccessReceiptOptions != null)
                errors.AddRange(this.SuccessReceiptOptions.Validate());

            if (FailureReceiptOptions != null)
                errors.AddRange(this.FailureReceiptOptions.Validate());

            return errors;
        }
    }
}