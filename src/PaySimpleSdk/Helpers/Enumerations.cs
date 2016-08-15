#region License
// The MIT License (MIT)
//
// Copyright (c) 2015 Scott Lance, Ethan Tipton
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

using System.Diagnostics.CodeAnalysis;
namespace PaySimpleSdk.Helpers
{
    public enum CustomerSort
    {
        FirstName,
        MiddleName,
        LastName,
        Company,
        BillingAddressCity,
        BillingAddressState,
        BillingAddressZip,
        BillingAddressCountry,
        ShippingAddressCity,
        ShippingAddressState,
        ShippingAddressZip,
        ShippingAddressCountry
    }

    public enum ExecutionFrequencyType : int
    {
        Annually = 9,
        BiWeekly = 3,
        Daily = 1,
        FirstOfMonth = 4,
        LastOfMonth = 6,
        Quarterly = 7,
        SemiAnnually = 8,
        SpecificDayOfMonth = 5,
        Weekly = 2,
    }

    public enum Issuer : int
    {
        Amex = 14,
        Discover = 15,
        Master = 13,
        Visa = 12
    }

    public enum PaymentSort
    {
        ActualSettledDate,
        Amount,
        EstimatedSettleDate,
        PaymentDate,
        PaymentId,
        PaymentSubType,
        PaymentType,
        ReturnDate
    }

    public enum PaymentStatus
    {
        Authorized,
        Chargeback,
        Failed,
        Pending,
        Posted,
        RefundSettled,
        Returned,
        Reversed,
        ReverseNSF,
        ReversePosted,
        Settled,
        Voided
    }

    public enum PaymentSubType
    {
        Ccd,
        Moto,
        Ppd,
        Swipe,
        Tel,
        Web
    }

    public enum PaymentType
    {
        CC = 1,
        ACH = 2
    }

    public enum ScheduleSort
    {
        EndDate,
        ExecutionFrequencyType,
        Id,
        NextPaymentDate,
        PaymentAmount,
        PaymentScheduleType,
        ScheduleStatus,
        StartDate
    }

    public enum ScheduleStatus : int
    {
        Active = 1,
        Expired = 3,
        PauseUntil = 2,
        Suspended = 4
    }

    public enum SortDirection
    {
        ASC,
        DESC
    }

    public enum StateCode
    {
        AB,
        AK,
        AL,
        AR,
        AZ,
        BC,
        CA,
        CO,
        CT,
        DC,
        DE,
        FL,
        GA,
        HI,
        IA,
        ID,
        IL,
        IN,
        KS,
        KY,
        LA,
        MA,
        MB,
        MD,
        ME,
        MI,
        MN,
        MO,
        MS,
        MT,
        NB,
        NC,
        ND,
        NE,
        NH,
        NJ,
        NL,
        NV,
        NM,
        NS,
        NT,
        NU,
        NY,
        OH,
        OK,
        ON,
        OR,
        PA,
        PE,
        PR,
        QC,
        RI,
        SC,
        SD,
        SK,
        TN,
        TX,
        UT,
        VA,
        VT,
        WA,
        WI,
        WV,
        WY,
        YT
    }

    public enum Status : int
    {
        Authorized,
        Chargeback,
        Failed,
        Pending,
        Posted,
        RefundSettled,
        Returned,
        Reversed,
        ReverseNSF,
        ReversePosted,
        Settled,
        Voided
    }

    [ExcludeFromCodeCoverage]
    public static class EnumStrings
    {
        public static object GetEnumMappings<T>()
        {
            switch (typeof(T).Name)
            {
                case "CustomerSort": return CustomerSortStrings;
                case "PaymentSort": return PaymentSortStrings;
                case "PaymentStatus": return PaymentStatusStrings;
                case "PaymentSubType": return PaymentSubTypeStrings;
                case "PaymentType": return PaymentTypeStrings;
                case "ScheduleSort": return ScheduleSortStrings;
                case "SortDirection": return SortDirectionStrings;
                case "StateCode": return StateCodeStrings;
                case "Status": return StatusStrings;
                default: return null;
            }
        }

        public static BiLookup<CustomerSort, string> CustomerSortStrings = new BiLookup<CustomerSort, string>
        {
            { CustomerSort.FirstName, "FirstName" },
            { CustomerSort.MiddleName, "MiddleName" },
            { CustomerSort.LastName, "LastName" },
            { CustomerSort.Company, "Company" },
            { CustomerSort.BillingAddressCity, "BillingAddress.City" },
            { CustomerSort.BillingAddressState, "BillingAddress.State" },
            { CustomerSort.BillingAddressZip, "BillingAddress.Zip" },
            { CustomerSort.BillingAddressCountry, "BillingAddress.Country" },
            { CustomerSort.ShippingAddressCity, "ShippingAddress.City" },
            { CustomerSort.ShippingAddressState, "ShippingAddress.State" },
            { CustomerSort.ShippingAddressZip, "ShippingAddress.Zip" },
            { CustomerSort.ShippingAddressCountry, "ShippingAddress.Country" }
        };

        public static BiLookup<PaymentSort, string> PaymentSortStrings = new BiLookup<PaymentSort, string>
        {          
            { PaymentSort.ActualSettledDate, "actualsettleddate" },
            { PaymentSort.Amount, "amount" },
            { PaymentSort.EstimatedSettleDate, "estimatedsettledate" },
            { PaymentSort.PaymentId, "paymentid" },
            { PaymentSort.PaymentDate, "paymentdate" },
            { PaymentSort.PaymentSubType, "paymentsubtype" },
            { PaymentSort.PaymentType, "paymenttype" },
            { PaymentSort.ReturnDate, "returndate" }
        };

        public static BiLookup<PaymentStatus, string> PaymentStatusStrings = new BiLookup<PaymentStatus, string>
        {         
            { PaymentStatus.Authorized, "authorized" },
            { PaymentStatus.Chargeback, "chargeback" },
            { PaymentStatus.Failed, "failed" },
            { PaymentStatus.Pending, "pending" },
            { PaymentStatus.Posted, "posted" },
            { PaymentStatus.RefundSettled, "refundsettled" },
            { PaymentStatus.Returned, "returned" },
            { PaymentStatus.Reversed, "reversed" },
            { PaymentStatus.ReverseNSF, "reversensf" },
            { PaymentStatus.ReversePosted, "reverseposted" },
            { PaymentStatus.Settled, "settled" },
            { PaymentStatus.Voided, "voided" }
        };

        public static BiLookup<PaymentType, string> PaymentTypeStrings = new BiLookup<PaymentType, string>
        {            
            { PaymentType.ACH, "ACH" },
            { PaymentType.CC, "CC" }
        };

        public static BiLookup<PaymentSubType, string> PaymentSubTypeStrings = new BiLookup<PaymentSubType, string>
        {            
            { PaymentSubType.Ccd, "Ccd" },
            { PaymentSubType.Moto, "Moto" },
            { PaymentSubType.Ppd, "Ppd" },
            { PaymentSubType.Swipe, "Swipe" },
            { PaymentSubType.Tel, "Tel" },
            { PaymentSubType.Web, "Web" } 
        };

        public static BiLookup<ScheduleSort, string> ScheduleSortStrings = new BiLookup<ScheduleSort, string>
        {
            { ScheduleSort.EndDate, "enddate" },
            { ScheduleSort.ExecutionFrequencyType, "executionfrequencytype" },
            { ScheduleSort.Id, "id" },
            { ScheduleSort.NextPaymentDate, "nextpaymentdate" },
            { ScheduleSort.PaymentAmount, "paymentamount" },
            { ScheduleSort.PaymentScheduleType, "paymentscheduletype" },
            { ScheduleSort.ScheduleStatus, "schedulestatus" },
            { ScheduleSort.StartDate, "startdate" }
        };

        public static BiLookup<SortDirection, string> SortDirectionStrings = new BiLookup<SortDirection, string>
        {
            { SortDirection.ASC, "ASC" },
            { SortDirection.DESC, "DESC" }
        };

        public static BiLookup<StateCode, string> StateCodeStrings = new BiLookup<StateCode, string>
        {
            { StateCode.AB, "AB" },
            { StateCode.AK, "AK" },
            { StateCode.AL, "AL" },
            { StateCode.AR, "AR" },
            { StateCode.AZ, "AZ" },
            { StateCode.BC, "BC" },
            { StateCode.CA, "CA" },
            { StateCode.CO, "CO" },
            { StateCode.CT, "CT" },
            { StateCode.DC, "DC" },
            { StateCode.DE, "DE" },
            { StateCode.FL, "FL" },
            { StateCode.GA, "GA" },
            { StateCode.HI, "HI" },
            { StateCode.IA, "IA" },
            { StateCode.ID, "ID" },
            { StateCode.IL, "IL" },
            { StateCode.IN, "IN" },
            { StateCode.KS, "KS" },
            { StateCode.KY, "KY" },
            { StateCode.LA, "LA" },
            { StateCode.MA, "MA" },
            { StateCode.MB, "MB" },
            { StateCode.MD, "MD" },
            { StateCode.ME, "ME" },
            { StateCode.MI, "MI" },
            { StateCode.MN, "MN" },
            { StateCode.MO, "MO" },
            { StateCode.MS, "MS" },
            { StateCode.MT, "MT" },
            { StateCode.NB, "NB" },
            { StateCode.NC, "NC" },
            { StateCode.ND, "ND" },
            { StateCode.NE, "NE" },
            { StateCode.NH, "NH" },
            { StateCode.NJ, "NJ" },
            { StateCode.NL, "NL" },
            { StateCode.NV, "NV" },
            { StateCode.NM, "NM" },
            { StateCode.NS, "NS" },
            { StateCode.NT, "NT" },
            { StateCode.NU, "NU" },
            { StateCode.NY, "NY" },
            { StateCode.OH, "OH" },
            { StateCode.OK, "OK" },
            { StateCode.ON, "ON" },
            { StateCode.OR, "OR" },
            { StateCode.PA, "PA" },
            { StateCode.PE, "PE" },
            { StateCode.PR, "PR" },
            { StateCode.QC, "QC" },
            { StateCode.RI, "RI" },
            { StateCode.SC, "SC" },
            { StateCode.SD, "SD" },
            { StateCode.SK, "SK" },
            { StateCode.TN, "TN" },
            { StateCode.TX, "TX" },
            { StateCode.UT, "UT" },
            { StateCode.VA, "VA" },
            { StateCode.VT, "VT" },
            { StateCode.WA, "WA" },
            { StateCode.WI, "WI" },
            { StateCode.WV, "WV" },
            { StateCode.WY, "WY" },
            { StateCode.YT, "YT" }
        };

        public static BiLookup<Status, string> StatusStrings = new BiLookup<Status, string>
        {
            { Status.Authorized, "Authorized" },
            { Status.Chargeback, "Chargeback" },
            { Status.Failed, "Failed" },
            { Status.Pending, "Pending" },
            { Status.Posted, "Posted" },
            { Status.RefundSettled, "RefundSettled" },
            { Status.Returned, "Returned" },
            { Status.Reversed, "Reversed" },
            { Status.ReverseNSF, "ReverseNSF" },
            { Status.ReversePosted, "ReversePosted" },
            { Status.Settled, "Settled" },
            { Status.Voided, "Voided" },
        };
    }
}