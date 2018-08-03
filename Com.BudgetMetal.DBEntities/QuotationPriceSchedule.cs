﻿using System;
using System.Collections.Generic;

namespace Com.BudgetMetal.DBEntities
{
    public class QuotationPriceSchedule
    {
        public int Id { get; set; }
        public uint DocumentId { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public string InternalRefrenceCode { get; set; }
        public string QuantityRequired { get; set; }
        public decimal ItemAmount { get; set; }
        public string Version { get; set; }
        public uint QuotationId { get; set; }

        public Quotation Quotation { get; set; }
    }
}