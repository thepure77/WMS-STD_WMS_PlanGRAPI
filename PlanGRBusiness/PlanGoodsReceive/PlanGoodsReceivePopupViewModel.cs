﻿using PlanGRBusiness.PlanGoodsReceive;
using System;
using System.Collections.Generic;
using System.Text;

namespace GRBusiness.PlanGoodsReceive
{
    public partial class PlanGoodsReceivePopupViewModel : Pagination
    {
        
        public Guid PlanGoodsReceiveIndex { get; set; }

        public Guid PlanGoodsReceiveItemIndex { get; set; }

        public Guid OwnerIndex { get; set; }


        
        public string OwnerId { get; set; }


        
        public string OwnerName { get; set; }

        public Guid VendorIndex { get; set; }


        
        public string VendorId { get; set; }


        
        public string VendorName { get; set; }

        public Guid DocumentTypeIndex { get; set; }


        
        public string DocumentTypeId { get; set; }


        
        public string DocumentTypeName { get; set; }


        
        public string PlanGoodsReceiveNo { get; set; }


        public string PlanGoodsReceiveDate { get; set; }
        public string PlanGoodsReceiveDueDate { get; set; }


        
        public string DocumentRefNo1 { get; set; }

        
        public string DocumentRefNo2 { get; set; }

        
        public string DocumentRefNo3 { get; set; }

        
        public string DocumentRefNo4 { get; set; }

        
        public string DocumentRefNo5 { get; set; }

        public int? DocumentStatus { get; set; }

        
        public string UDF1 { get; set; }

        
        public string UDF2 { get; set; }

        
        public string UDF3 { get; set; }

        
        public string UDF4 { get; set; }

        
        public string UDF5 { get; set; }

        public int? DocumentPriorityStatus { get; set; }

        
        public string DocumentRemark { get; set; }

        
        public string Create_By { get; set; }


        public string Create_Date { get; set; }

        
        public string Update_By { get; set; }


        public string Update_Date { get; set; }

        
        public string Cancel_By { get; set; }


        public string Cancel_Date { get; set; }

        public Guid? WarehouseIndex { get; set; }

        
        public string WarehouseId { get; set; }

        
        public string WarehouseName { get; set; }

        public Guid? WarehouseIndexTo { get; set; }

        
        public string WarehouseIdTo { get; set; }

        
        public string WarehouseNameTo { get; set; }

        public string chk { get; set; }
        public int count { get; set; }

        public Guid GrDocumentTypeIndex { get; set; }



        public string GrDocumentTypeId { get; set; }



        public string GrDocumentTypeName { get; set; }



        public class actionResultPlanGRPopupViewModel
        {
            public IList<PlanGoodsReceivePopupViewModel> itemsPlanGR { get; set; }
            public Pagination pagination { get; set; }
        }

        public List<PlanGoodsReceiveItemViewModel> listPlanGoodsReceiveItemViewModel { get; set; }

    }
}
