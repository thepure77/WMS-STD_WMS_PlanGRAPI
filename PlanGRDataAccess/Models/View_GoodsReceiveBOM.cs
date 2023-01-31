using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRDataAccess.Models
{
    public partial class View_GoodsReceiveBOM
    {
        [Key]
        public Guid PlanGoodsReceiveItem_Index { get; set; }
        public Guid PlanGoodsReceive_Index { get; set; }
        public Guid Product_Index { get; set; }

        public string PlanGoodsReceive_No { get; set; }

        public string PO_Type { get; set; }
        public string ITEM_CAT { get; set; }
        public string LineNum { get; set; }
        public string HIGH_LV_ITEM { get; set; }
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public string Product_Lot { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Ratio { get; set; }
        public decimal? TotalQty { get; set; }
        public decimal? PlanTotalQty { get; set; }
        public decimal? BOM_Ratio { get; set; }
        public decimal? BomTotalQTY { get; set; }
        public decimal? ModBomTotalQty { get; set; }
        public decimal? BomRemianTotalQty { get; set; }
        public decimal? BomDiffTotalQty { get; set; }
        public string CheckBOM { get; set; }

        
    }
}
