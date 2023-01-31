using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace GRBusiness.PlanGoodsReceive
{
    [DataContract]
    [Serializable]
    public class CloseDocumentViewModel
    {
        //[DataMember]
        //public string[] id { get; set; }

        //[DataMember]
        //public string[] username { get; set; }
        public Guid planGoodsReceive_Index { get; set; }
        public string update_By { get; set; }

    }
}
