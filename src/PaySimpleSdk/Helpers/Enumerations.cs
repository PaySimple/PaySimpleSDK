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
    public enum CountryCode
    {
        AD,
        AE,
        AF,
        AG,
        AI,
        AL,
        AM,
        AO,
        AQ,
        AR,
        AS,
        AT,
        AU,
        AW,
        AX,
        AZ,
        BA,
        BB,
        BD,
        BE,
        BF,
        BG,
        BH,
        BI,
        BJ,
        BL,
        BM,
        BN,
        BO,
        BQ,
        BR,
        BS,
        BT,
        BV,
        BW,
        BY,
        BZ,
        CA,
        CC,
        CD,
        CF,
        CG,
        CH,
        CI,
        CK,
        CL,
        CM,
        CN,
        CO,
        CR,
        CU,
        CV,
        CW,
        CX,
        CY,
        CZ,
        DE,
        DJ,
        DK,
        DM,
        DO,
        DZ,
        EC,
        EE,
        EG,
        EH,
        ER,
        ES,
        ET,
        FI,
        FJ,
        FK,
        FM,
        FO,
        FR,
        GA,
        GB,
        GD,
        GE,
        GF,
        GG,
        GH,
        GI,
        GL,
        GM,
        GN,
        GP,
        GQ,
        GR,
        GS,
        GT,
        GU,
        GW,
        GY,
        HK,
        HM,
        HN,
        HR,
        HT,
        HU,
        ID,
        IE,
        IL,
        IM,
        IN,
        IO,
        IQ,
        IR,
        IS,
        IT,
        JE,
        JM,
        JO,
        JP,
        KE,
        KG,
        KH,
        KI,
        KM,
        KN,
        KP,
        KR,
        KW,
        KY,
        KZ,
        LA,
        LB,
        LC,
        LI,
        LK,
        LR,
        LS,
        LT,
        LU,
        LV,
        LY,
        MA,
        MC,
        MD,
        ME,
        MF,
        MG,
        MH,
        MK,
        ML,
        MM,
        MN,
        MO,
        MP,
        MQ,
        MR,
        MS,
        MT,
        MU,
        MV,
        MW,
        MX,
        MY,
        MZ,
        NA,
        NC,
        NE,
        NF,
        NG,
        NI,
        NL,
        NO,
        NP,
        NR,
        NU,
        NZ,
        OM,
        PA,
        PE,
        PF,
        PG,
        PH,
        PK,
        PL,
        PM,
        PN,
        PR,
        PS,
        PT,
        PW,
        PY,
        QA,
        RE,
        RO,
        RS,
        RU,
        RW,
        SA,
        SB,
        SC,
        SD,
        SE,
        SG,
        SH,
        SI,
        SJ,
        SK,
        SL,
        SM,
        SN,
        SO,
        SR,
        SS,
        ST,
        SV,
        SX,
        SY,
        SZ,
        TC,
        TD,
        TF,
        TG,
        TH,
        TJ,
        TK,
        TL,
        TM,
        TN,
        TO,
        TR,
        TT,
        TV,
        TW,
        TZ,
        UA,
        UG,
        UM,
        US,
        UY,
        UZ,
        VA,
        VC,
        VE,
        VG,
        VI,
        VN,
        VU,
        WF,
        WS,
        YE,
        YT,
        ZA,
        ZM,
        ZW
    }

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

    public enum ExecutionFrequencyType
    {
        Annually = 9,
        BiWeekly = 3,
        Daily = 1,
        FirstOfMonth = 4,
        LastOfMonth = 6,
        Quarterly = 7,
        SemiAnnually = 8,
        SpecificDayOfMonth = 5,
        Weekly = 2
    }

    public enum Issuer
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

    public enum ScheduleStatus
    {
        None = 0,
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

    public enum Status
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

    public enum PaymentScheduleType
    {
        /// <summary>
        /// The schedule is a payment plan.
        /// </summary>
        PaymentPlan = 1,

        /// <summary>
        /// The schedule is a Recurring Payment Schedule.
        /// </summary>
        RecurringPayment = 2
    }

    [ExcludeFromCodeCoverage]
    public static class EnumStrings
    {
        public static BiLookup<CountryCode, string> CountryCodeStrings = new BiLookup<CountryCode, string>
        {
            {CountryCode.AD, "AD"},
            {CountryCode.AE, "AE"},
            {CountryCode.AF, "AF"},
            {CountryCode.AG, "AG"},
            {CountryCode.AI, "AI"},
            {CountryCode.AL, "AL"},
            {CountryCode.AM, "AM"},
            {CountryCode.AO, "AO"},
            {CountryCode.AQ, "AQ"},
            {CountryCode.AR, "AR"},
            {CountryCode.AS, "AS"},
            {CountryCode.AT, "AT"},
            {CountryCode.AU, "AU"},
            {CountryCode.AW, "AW"},
            {CountryCode.AX, "AX"},
            {CountryCode.AZ, "AZ"},
            {CountryCode.BA, "BA"},
            {CountryCode.BB, "BB"},
            {CountryCode.BD, "BD"},
            {CountryCode.BE, "BE"},
            {CountryCode.BF, "BF"},
            {CountryCode.BG, "BG"},
            {CountryCode.BH, "BH"},
            {CountryCode.BI, "BI"},
            {CountryCode.BJ, "BJ"},
            {CountryCode.BL, "BL"},
            {CountryCode.BM, "BM"},
            {CountryCode.BN, "BN"},
            {CountryCode.BO, "BO"},
            {CountryCode.BQ, "BQ"},
            {CountryCode.BR, "BR"},
            {CountryCode.BS, "BS"},
            {CountryCode.BT, "BT"},
            {CountryCode.BV, "BV"},
            {CountryCode.BW, "BW"},
            {CountryCode.BY, "BY"},
            {CountryCode.BZ, "BZ"},
            {CountryCode.CA, "CA"},
            {CountryCode.CC, "CC"},
            {CountryCode.CD, "CD"},
            {CountryCode.CF, "CF"},
            {CountryCode.CG, "CG"},
            {CountryCode.CH, "CH"},
            {CountryCode.CI, "CI"},
            {CountryCode.CK, "CK"},
            {CountryCode.CL, "CL"},
            {CountryCode.CM, "CM"},
            {CountryCode.CN, "CN"},
            {CountryCode.CO, "CO"},
            {CountryCode.CR, "CR"},
            {CountryCode.CU, "CU"},
            {CountryCode.CV, "CV"},
            {CountryCode.CW, "CW"},
            {CountryCode.CX, "CX"},
            {CountryCode.CY, "CY"},
            {CountryCode.CZ, "CZ"},
            {CountryCode.DE, "DE"},
            {CountryCode.DJ, "DJ"},
            {CountryCode.DK, "DK"},
            {CountryCode.DM, "DM"},
            {CountryCode.DO, "DO"},
            {CountryCode.DZ, "DZ"},
            {CountryCode.EC, "EC"},
            {CountryCode.EE, "EE"},
            {CountryCode.EG, "EG"},
            {CountryCode.EH, "EH"},
            {CountryCode.ER, "ER"},
            {CountryCode.ES, "ES"},
            {CountryCode.ET, "ET"},
            {CountryCode.FI, "FI"},
            {CountryCode.FJ, "FJ"},
            {CountryCode.FK, "FK"},
            {CountryCode.FM, "FM"},
            {CountryCode.FO, "FO"},
            {CountryCode.FR, "FR"},
            {CountryCode.GA, "GA"},
            {CountryCode.GB, "GB"},
            {CountryCode.GD, "GD"},
            {CountryCode.GE, "GE"},
            {CountryCode.GF, "GF"},
            {CountryCode.GG, "GG"},
            {CountryCode.GH, "GH"},
            {CountryCode.GI, "GI"},
            {CountryCode.GL, "GL"},
            {CountryCode.GM, "GM"},
            {CountryCode.GN, "GN"},
            {CountryCode.GP, "GP"},
            {CountryCode.GQ, "GQ"},
            {CountryCode.GR, "GR"},
            {CountryCode.GS, "GS"},
            {CountryCode.GT, "GT"},
            {CountryCode.GU, "GU"},
            {CountryCode.GW, "GW"},
            {CountryCode.GY, "GY"},
            {CountryCode.HK, "HK"},
            {CountryCode.HM, "HM"},
            {CountryCode.HN, "HN"},
            {CountryCode.HR, "HR"},
            {CountryCode.HT, "HT"},
            {CountryCode.HU, "HU"},
            {CountryCode.ID, "ID"},
            {CountryCode.IE, "IE"},
            {CountryCode.IL, "IL"},
            {CountryCode.IM, "IM"},
            {CountryCode.IN, "IN"},
            {CountryCode.IO, "IO"},
            {CountryCode.IQ, "IQ"},
            {CountryCode.IR, "IR"},
            {CountryCode.IS, "IS"},
            {CountryCode.IT, "IT"},
            {CountryCode.JE, "JE"},
            {CountryCode.JM, "JM"},
            {CountryCode.JO, "JO"},
            {CountryCode.JP, "JP"},
            {CountryCode.KE, "KE"},
            {CountryCode.KG, "KG"},
            {CountryCode.KH, "KH"},
            {CountryCode.KI, "KI"},
            {CountryCode.KM, "KM"},
            {CountryCode.KN, "KN"},
            {CountryCode.KP, "KP"},
            {CountryCode.KR, "KR"},
            {CountryCode.KW, "KW"},
            {CountryCode.KY, "KY"},
            {CountryCode.KZ, "KZ"},
            {CountryCode.LA, "LA"},
            {CountryCode.LB, "LB"},
            {CountryCode.LC, "LC"},
            {CountryCode.LI, "LI"},
            {CountryCode.LK, "LK"},
            {CountryCode.LR, "LR"},
            {CountryCode.LS, "LS"},
            {CountryCode.LT, "LT"},
            {CountryCode.LU, "LU"},
            {CountryCode.LV, "LV"},
            {CountryCode.LY, "LY"},
            {CountryCode.MA, "MA"},
            {CountryCode.MC, "MC"},
            {CountryCode.MD, "MD"},
            {CountryCode.ME, "ME"},
            {CountryCode.MF, "MF"},
            {CountryCode.MG, "MG"},
            {CountryCode.MH, "MH"},
            {CountryCode.MK, "MK"},
            {CountryCode.ML, "ML"},
            {CountryCode.MM, "MM"},
            {CountryCode.MN, "MN"},
            {CountryCode.MO, "MO"},
            {CountryCode.MP, "MP"},
            {CountryCode.MQ, "MQ"},
            {CountryCode.MR, "MR"},
            {CountryCode.MS, "MS"},
            {CountryCode.MT, "MT"},
            {CountryCode.MU, "MU"},
            {CountryCode.MV, "MV"},
            {CountryCode.MW, "MW"},
            {CountryCode.MX, "MX"},
            {CountryCode.MY, "MY"},
            {CountryCode.MZ, "MZ"},
            {CountryCode.NA, "NA"},
            {CountryCode.NC, "NC"},
            {CountryCode.NE, "NE"},
            {CountryCode.NF, "NF"},
            {CountryCode.NG, "NG"},
            {CountryCode.NI, "NI"},
            {CountryCode.NL, "NL"},
            {CountryCode.NO, "NO"},
            {CountryCode.NP, "NP"},
            {CountryCode.NR, "NR"},
            {CountryCode.NU, "NU"},
            {CountryCode.NZ, "NZ"},
            {CountryCode.OM, "OM"},
            {CountryCode.PA, "PA"},
            {CountryCode.PE, "PE"},
            {CountryCode.PF, "PF"},
            {CountryCode.PG, "PG"},
            {CountryCode.PH, "PH"},
            {CountryCode.PK, "PK"},
            {CountryCode.PL, "PL"},
            {CountryCode.PM, "PM"},
            {CountryCode.PN, "PN"},
            {CountryCode.PR, "PR"},
            {CountryCode.PS, "PS"},
            {CountryCode.PT, "PT"},
            {CountryCode.PW, "PW"},
            {CountryCode.PY, "PY"},
            {CountryCode.QA, "QA"},
            {CountryCode.RE, "RE"},
            {CountryCode.RO, "RO"},
            {CountryCode.RS, "RS"},
            {CountryCode.RU, "RU"},
            {CountryCode.RW, "RW"},
            {CountryCode.SA, "SA"},
            {CountryCode.SB, "SB"},
            {CountryCode.SC, "SC"},
            {CountryCode.SD, "SD"},
            {CountryCode.SE, "SE"},
            {CountryCode.SG, "SG"},
            {CountryCode.SH, "SH"},
            {CountryCode.SI, "SI"},
            {CountryCode.SJ, "SJ"},
            {CountryCode.SK, "SK"},
            {CountryCode.SL, "SL"},
            {CountryCode.SM, "SM"},
            {CountryCode.SN, "SN"},
            {CountryCode.SO, "SO"},
            {CountryCode.SR, "SR"},
            {CountryCode.SS, "SS"},
            {CountryCode.ST, "ST"},
            {CountryCode.SV, "SV"},
            {CountryCode.SX, "SX"},
            {CountryCode.SY, "SY"},
            {CountryCode.SZ, "SZ"},
            {CountryCode.TC, "TC"},
            {CountryCode.TD, "TD"},
            {CountryCode.TF, "TF"},
            {CountryCode.TG, "TG"},
            {CountryCode.TH, "TH"},
            {CountryCode.TJ, "TJ"},
            {CountryCode.TK, "TK"},
            {CountryCode.TL, "TL"},
            {CountryCode.TM, "TM"},
            {CountryCode.TN, "TN"},
            {CountryCode.TO, "TO"},
            {CountryCode.TR, "TR"},
            {CountryCode.TT, "TT"},
            {CountryCode.TV, "TV"},
            {CountryCode.TW, "TW"},
            {CountryCode.TZ, "TZ"},
            {CountryCode.UA, "UA"},
            {CountryCode.UG, "UG"},
            {CountryCode.UM, "UM"},
            {CountryCode.US, "US"},
            {CountryCode.UY, "UY"},
            {CountryCode.UZ, "UZ"},
            {CountryCode.VA, "VA"},
            {CountryCode.VC, "VC"},
            {CountryCode.VE, "VE"},
            {CountryCode.VG, "VG"},
            {CountryCode.VI, "VI"},
            {CountryCode.VN, "VN"},
            {CountryCode.VU, "VU"},
            {CountryCode.WF, "WF"},
            {CountryCode.WS, "WS"},
            {CountryCode.YE, "YE"},
            {CountryCode.YT, "YT"},
            {CountryCode.ZA, "ZA"},
            {CountryCode.ZM, "ZM"}
        };

        public static BiLookup<CustomerSort, string> CustomerSortStrings = new BiLookup<CustomerSort, string>
        {
            {CustomerSort.FirstName, "FirstName"},
            {CustomerSort.MiddleName, "MiddleName"},
            {CustomerSort.LastName, "LastName"},
            {CustomerSort.Company, "Company"},
            {CustomerSort.BillingAddressCity, "BillingAddress.City"},
            {CustomerSort.BillingAddressState, "BillingAddress.State"},
            {CustomerSort.BillingAddressZip, "BillingAddress.Zip"},
            {CustomerSort.BillingAddressCountry, "BillingAddress.Country"},
            {CustomerSort.ShippingAddressCity, "ShippingAddress.City"},
            {CustomerSort.ShippingAddressState, "ShippingAddress.State"},
            {CustomerSort.ShippingAddressZip, "ShippingAddress.Zip"},
            {CustomerSort.ShippingAddressCountry, "ShippingAddress.Country"}
        };

        public static BiLookup<PaymentSort, string> PaymentSortStrings = new BiLookup<PaymentSort, string>
        {
            {PaymentSort.ActualSettledDate, "actualsettleddate"},
            {PaymentSort.Amount, "amount"},
            {PaymentSort.EstimatedSettleDate, "estimatedsettledate"},
            {PaymentSort.PaymentId, "paymentid"},
            {PaymentSort.PaymentDate, "paymentdate"},
            {PaymentSort.PaymentSubType, "paymentsubtype"},
            {PaymentSort.PaymentType, "paymenttype"},
            {PaymentSort.ReturnDate, "returndate"}
        };

        public static BiLookup<PaymentStatus, string> PaymentStatusStrings = new BiLookup<PaymentStatus, string>
        {
            {PaymentStatus.Authorized, "authorized"},
            {PaymentStatus.Chargeback, "chargeback"},
            {PaymentStatus.Failed, "failed"},
            {PaymentStatus.Pending, "pending"},
            {PaymentStatus.Posted, "posted"},
            {PaymentStatus.RefundSettled, "refundsettled"},
            {PaymentStatus.Returned, "returned"},
            {PaymentStatus.Reversed, "reversed"},
            {PaymentStatus.ReverseNSF, "reversensf"},
            {PaymentStatus.ReversePosted, "reverseposted"},
            {PaymentStatus.Settled, "settled"},
            {PaymentStatus.Voided, "voided"}
        };

        public static BiLookup<PaymentType, string> PaymentTypeStrings = new BiLookup<PaymentType, string>
        {
            {PaymentType.ACH, "ACH"},
            {PaymentType.CC, "CC"}
        };

        public static BiLookup<PaymentSubType, string> PaymentSubTypeStrings = new BiLookup<PaymentSubType, string>
        {
            {PaymentSubType.Ccd, "Ccd"},
            {PaymentSubType.Moto, "Moto"},
            {PaymentSubType.Ppd, "Ppd"},
            {PaymentSubType.Swipe, "Swipe"},
            {PaymentSubType.Tel, "Tel"},
            {PaymentSubType.Web, "Web"}
        };

        public static BiLookup<ScheduleSort, string> ScheduleSortStrings = new BiLookup<ScheduleSort, string>
        {
            {ScheduleSort.EndDate, "enddate"},
            {ScheduleSort.ExecutionFrequencyType, "executionfrequencytype"},
            {ScheduleSort.Id, "id"},
            {ScheduleSort.NextPaymentDate, "nextpaymentdate"},
            {ScheduleSort.PaymentAmount, "paymentamount"},
            {ScheduleSort.PaymentScheduleType, "paymentscheduletype"},
            {ScheduleSort.ScheduleStatus, "schedulestatus"},
            {ScheduleSort.StartDate, "startdate"}
        };

        public static BiLookup<SortDirection, string> SortDirectionStrings = new BiLookup<SortDirection, string>
        {
            {SortDirection.ASC, "ASC"},
            {SortDirection.DESC, "DESC"}
        };

        public static BiLookup<StateCode, string> StateCodeStrings = new BiLookup<StateCode, string>
        {
            {StateCode.AB, "AB"},
            {StateCode.AK, "AK"},
            {StateCode.AL, "AL"},
            {StateCode.AR, "AR"},
            {StateCode.AZ, "AZ"},
            {StateCode.BC, "BC"},
            {StateCode.CA, "CA"},
            {StateCode.CO, "CO"},
            {StateCode.CT, "CT"},
            {StateCode.DC, "DC"},
            {StateCode.DE, "DE"},
            {StateCode.FL, "FL"},
            {StateCode.GA, "GA"},
            {StateCode.HI, "HI"},
            {StateCode.IA, "IA"},
            {StateCode.ID, "ID"},
            {StateCode.IL, "IL"},
            {StateCode.IN, "IN"},
            {StateCode.KS, "KS"},
            {StateCode.KY, "KY"},
            {StateCode.LA, "LA"},
            {StateCode.MA, "MA"},
            {StateCode.MB, "MB"},
            {StateCode.MD, "MD"},
            {StateCode.ME, "ME"},
            {StateCode.MI, "MI"},
            {StateCode.MN, "MN"},
            {StateCode.MO, "MO"},
            {StateCode.MS, "MS"},
            {StateCode.MT, "MT"},
            {StateCode.NB, "NB"},
            {StateCode.NC, "NC"},
            {StateCode.ND, "ND"},
            {StateCode.NE, "NE"},
            {StateCode.NH, "NH"},
            {StateCode.NJ, "NJ"},
            {StateCode.NL, "NL"},
            {StateCode.NV, "NV"},
            {StateCode.NM, "NM"},
            {StateCode.NS, "NS"},
            {StateCode.NT, "NT"},
            {StateCode.NU, "NU"},
            {StateCode.NY, "NY"},
            {StateCode.OH, "OH"},
            {StateCode.OK, "OK"},
            {StateCode.ON, "ON"},
            {StateCode.OR, "OR"},
            {StateCode.PA, "PA"},
            {StateCode.PE, "PE"},
            {StateCode.PR, "PR"},
            {StateCode.QC, "QC"},
            {StateCode.RI, "RI"},
            {StateCode.SC, "SC"},
            {StateCode.SD, "SD"},
            {StateCode.SK, "SK"},
            {StateCode.TN, "TN"},
            {StateCode.TX, "TX"},
            {StateCode.UT, "UT"},
            {StateCode.VA, "VA"},
            {StateCode.VT, "VT"},
            {StateCode.WA, "WA"},
            {StateCode.WI, "WI"},
            {StateCode.WV, "WV"},
            {StateCode.WY, "WY"},
            {StateCode.YT, "YT"}
        };

        public static BiLookup<Status, string> StatusStrings = new BiLookup<Status, string>
        {
            {Status.Authorized, "Authorized"},
            {Status.Chargeback, "Chargeback"},
            {Status.Failed, "Failed"},
            {Status.Pending, "Pending"},
            {Status.Posted, "Posted"},
            {Status.RefundSettled, "RefundSettled"},
            {Status.Returned, "Returned"},
            {Status.Reversed, "Reversed"},
            {Status.ReverseNSF, "ReverseNSF"},
            {Status.ReversePosted, "ReversePosted"},
            {Status.Settled, "Settled"},
            {Status.Voided, "Voided"}
        };

        public static BiLookup<PaymentScheduleType, string> PaymentScheduleTypeStrings =
            new BiLookup<PaymentScheduleType, string>
            {
                { PaymentScheduleType.PaymentPlan, "1" },
                { PaymentScheduleType.RecurringPayment, "2" }
            };

        public static object GetEnumMappings<T>()
        {
            switch (typeof(T).Name)
            {
                case "CountryCode":
                    return CountryCodeStrings;
                case "CustomerSort":
                    return CustomerSortStrings;
                case "PaymentSort":
                    return PaymentSortStrings;
                case "PaymentScheduleType":
                    return PaymentScheduleTypeStrings;
                case "PaymentStatus":
                    return PaymentStatusStrings;
                case "PaymentSubType":
                    return PaymentSubTypeStrings;
                case "PaymentType":
                    return PaymentTypeStrings;
                case "ScheduleSort":
                    return ScheduleSortStrings;
                case "SortDirection":
                    return SortDirectionStrings;
                case "StateCode":
                    return StateCodeStrings;
                case "Status":
                    return StatusStrings;
                default:
                    return null;
            }
        }
    }
}