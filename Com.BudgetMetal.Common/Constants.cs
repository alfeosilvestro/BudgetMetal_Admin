using System;

namespace Com.BudgetMetal.Common
{
    public class Constants
    {
        public static readonly int app_totalRecords = 12;
        public static readonly int app_firstPage = 1;

        public static readonly string RFQDefaultRole = "RFQ Creator";
        public static readonly string RFQDefaultRoleId = "3";

        public static readonly int RFQCreatorRoleId =3;
        public static readonly int RFQApproverRoleId = 7;

        public static readonly string QuotationDefaultRole = "Quotation Creator";
        public static readonly string QuotationDefaultRoleId = "8";
        public static readonly int C_Admin_Role = 2;

        public static readonly string[] arrExchanges = { "USD", "GBP", "SGD", "EUR", "CNY", "THB" , "VND", "AUD" };

    }
}
