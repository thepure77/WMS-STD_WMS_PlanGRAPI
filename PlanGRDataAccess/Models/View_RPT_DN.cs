using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRDataAccess.Models
{
    public partial class View_RPT_DN
    {
        [Key]
        public long? Row_Index { get; set; }
        public Guid? PlanGoodsReceive_Index { get; set; }
        public Guid? PlanGoodsReceiveItem_Index { get; set; }
        public Guid? Owner_Index { get; set; }
        public string Owner_Id { get; set; }
        public string Owner_Name { get; set; }
        public DateTime? PlanGoodsReceive_Due_Date { get; set; }
        public DateTime? PlanGoodsReceive_Date { get; set; }
        public string LineNum { get; set; }
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public decimal Qty { get; set; }
        public Guid? ProductConversion_Index { get; set; }
        public string ProductConversion_Id { get; set; }
        public string ProductConversion_Name { get; set; }
        public decimal? ProductConversion_Ratio { get; set; }
        public decimal? UnitWidth { get; set; }
        public decimal? UnitLength { get; set; }
        public decimal? UnitHeight { get; set; }
        public Guid? Volume_Index { get; set; }
        public string Volume_Id { get; set; }
        public string Volume_Name { get; set; }
        public string PlanGoodsReceive_No { get; set; }
        public string ShelfLifeGR { get; set; }
        public string TI { get; set; }
        public string HI { get; set; }
        public string Ref_PO { get; set; }
        public decimal? Sale_Qty { get; set; }
        public string Sale_Unit { get; set; }
        public decimal? In_Qty { get; set; }
        public string In_Unit { get; set; }
        public decimal? EstPallet { get; set; }
        public string DocumentType_Name { get; set; }
        public string Document_Remark { get; set; }
        public int? IsLot { get; set; }


    }
}
