using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PlanGRDataAccess.Models
{
    public partial class View_RPT_ASN
    {
        [Key]
        public long? Row_Index { get; set; }

        public Guid? PlanGoodsReceive_Index { get; set; }

        public DateTime? PlanGoodsReceive_Date { get; set; }

        public string PlanGoodsReceive_No { get; set; }

        public string Owner_Id { get; set; }

        public string Owner_Name { get; set; }

        public string Vendor_Id { get; set; }

        public string Vendor_Name { get; set; }

        public string DocumentType_Name { get; set; }

        public string Product_Id { get; set; }

        public string Product_Name { get; set; }

        public decimal? Qty { get; set; }

        public string ProductConversion_Name { get; set; }

        public int? Document_Status { get; set; }
    }
}
