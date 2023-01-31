using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PlanGRDataAccess.Models
{
    public partial class im_PurchaseOrder_Ref
    {
        [Key]
        public Guid PurchaseOrder_Ref_Index { get; set; }

        public Guid PurchaseOrder_Index { get; set; }

        //public virtual ICollection<im_PurchaseOrderItem_Ref> im_PurchaseOrderItem_Ref { get; set; }

        public string PurchaseOrder_No { get; set; }
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

    }
}
