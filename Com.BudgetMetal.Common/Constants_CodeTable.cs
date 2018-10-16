using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.Common
{
    public class Constants_CodeTable
    {
        #region Code Category

        public static readonly int CodeCat_User = 100010;
        public static readonly int CodeCat_Doc = 100020;
        public static readonly int CodeCat_RFQStatus = 100030;
        public static readonly int CodeCat_QuotationStatus = 100040;
        public static readonly int CodeCat_ToleranceLevels = 10100000;
        public static readonly int CodeCat_Company = 100050;
        #endregion

        #region Code Table

        public static readonly int Code_Buyer = 100011;
        public static readonly int Code_Supplier = 100012;
        public static readonly int Code_System = 100013;

        public static readonly int Code_RFQ = 100021;
        public static readonly int Code_Quotation = 100022;



        public static readonly int Code_RFQ_Draft = 100031;
        public static readonly int Code_RFQ_Submitted = 100032;
        public static readonly int Code_RFQ_Closed = 100033;


        public static readonly int Code_Quotation_Draft = 100041;
        public static readonly int Code_Quotation_Submitted = 100042;
        public static readonly int Code_Quotation_Accepted = 100043;
        public static readonly int Code_Quotation_Rejected = 100044;
        public static readonly int Code_Quotation_Cancelled = 100045;


        public static readonly int Code_MaxDefaultRFQPerWeek = 10100001;
        public static readonly int Code_MaxDefaultQuotePerWeek = 10100002;


        public static readonly int Code_C_Buyer = 100051;
        public static readonly int Code_C_Supplier = 100052;

        
        #endregion
    }
}
