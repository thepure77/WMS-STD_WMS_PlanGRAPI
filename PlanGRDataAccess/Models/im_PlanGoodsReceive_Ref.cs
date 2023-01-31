using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PlanGRDataAccess.Models
{
    public partial class im_PlanGoodsReceive_Ref
    {
        [Key]
        public Guid PlanGoodsReceive_Ref_Index { get; set; }
        public Guid PlanGoodsReceive_Index { get; set; }
        public string PlanGoodsReceive_No { get; set; }
        public string SO_No { get; set; }
        public string SO_Type { get; set; }
        public string SHIP_CON { get; set; }
        public string DO_Type { get; set; }
        public string RTN_Flag { get; set; }
        public string CREDIT_STATUS { get; set; }
        public string EXPORT_FLAG { get; set; }
        public string FOC_FLAG { get; set; }
        public string CLAIM_FLAG { get; set; }
        public string Ref_Type { get; set; }
        public string IO { get; set; }
        public string TAR_VAL { get; set; }
        public string YEAR { get; set; }
        public string ITEM_TEXT { get; set; }
        public Guid? Ref_Document_Index { get; set; }
        public string Ref_Document_No { get; set; }
    }
}
