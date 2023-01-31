using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRBusiness.PlanGoodsReceive
{
    public class PlanGoodsReceiveViewModel
    {
        [Key]
        public Guid planGoodsReceive_Index { get; set; }

        public Guid owner_Index { get; set; }



        public string owner_Id { get; set; }



        public string owner_Name { get; set; }

        public Guid vendor_Index { get; set; }



        public string vendor_Id { get; set; }



        public string vendor_Name { get; set; }

        public Guid documentType_Index { get; set; }



        public string documentType_Id { get; set; }



        public string documentType_Name { get; set; }



        public string planGoodsReceive_No { get; set; }


        public DateTime? planGoodsReceive_Date { get; set; }


        public string planGoodsReceive_Time { get; set; }

        public DateTime? planGoodsReceive_Due_Date { get; set; }


        public string documentRef_No1 { get; set; }


        public string documentRef_No2 { get; set; }


        public string documentRef_No3 { get; set; }


        public string documentRef_No4 { get; set; }


        public string documentRef_No5 { get; set; }

        public int? document_Status { get; set; }


        public string  uDF_1 { get; set; }


        public string  uDF_2 { get; set; }


        public string  uDF_3 { get; set; }


        public string  uDF_4 { get; set; }


        public string  uDF_5 { get; set; }

        public int? documentPriority_Status { get; set; }


        public string document_Remark { get; set; }


        public string create_By { get; set; }


        public DateTime? create_Date { get; set; }


        public string update_By { get; set; }


        public DateTime? update_Date { get; set; }


        public string cancel_By { get; set; }


        public DateTime? cancel_Date { get; set; }

        public Guid? warehouse_Index { get; set; }


        public string warehouse_Id { get; set; }


        public string warehouse_Name { get; set; }

        public Guid? warehouse_Index_To { get; set; }


        public string warehouse_Id_To { get; set; }


        public string warehouse_Name_To { get; set; }

        public string userAssign { get; set; }

        public string userAssignKey { get; set; }

        public Guid? dock_Index { get; set; }

        public string dock_Id { get; set; }

        public string dock_Name { get; set; }

        public Guid? Vehicle_Index { get; set; }

        public string Vehicle_Id { get; set; }

        public string Vehicle_Name { get; set; }

        public Guid? driver_Index { get; set; }

        public string driver_Id { get; set; }

        public string driver_Name { get; set; }

        public Guid? transport_Index { get; set; }

        public string transport_Id { get; set; }

        public string transport_Name { get; set; }

        public Guid? round_Index { get; set; }

        public string round_Id { get; set; }

        public string round_Name { get; set; }

        public string license_Name { get; set; }

    }
}
