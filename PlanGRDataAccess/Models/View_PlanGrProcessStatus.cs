using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRDataAccess.Models
{
    public class View_PlanGrProcessStatus
    {
        [Key]
        public Guid PlanGoodsReceive_Index { get; set; }

        public Guid Owner_Index { get; set; }



        public string Owner_Id { get; set; }



        public string Owner_Name { get; set; }

        

        public Guid DocumentType_Index { get; set; }



        public string DocumentType_Id { get; set; }


        [StringLength(200)]
        public string DocumentType_Name { get; set; }



        public string PlanGoodsReceive_No { get; set; }

        public DateTime? PlanGoodsReceive_Date { get; set; }

        public DateTime? PlanGoodsReceive_Due_Date { get; set; }

        public string DocumentRef_No1 { get; set; }

       

        public int? Document_Status { get; set; }

      
        public string Create_By { get; set; }
        public string Update_By { get; set; }
        public string Cancel_By { get; set; }

        public decimal Qty { get; set; }
        public string Our_Reference { get; set; }

        

    }
}
