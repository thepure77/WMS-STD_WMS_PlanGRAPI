using System;
using System.Collections.Generic;
using System.Text;

namespace PlanGRBusiness.Demo
{
    public class DemoASNRequestViewModel
    {
        public string WmsTrans_Id                { get; set; }
        public string PlanGoodsReceive_No        { get; set; }
        public string Vendor_Id                  { get; set; }
        //public string Sloc_Id                    { get; set; }
        //public string start_Date                 { get; set; }
        //public string end_Date                   { get; set; }
        public string Warehouse { get; set; }
        public string Document_Status            { get; set; }
        //public string DOC_TYP { get; set; }
        public string Creat_By { get; set; }
        public List<DemoASNItem_RequestViewModel> items { get; set; }
    }

    public class DemoASNItem_RequestViewModel
    {
        public string Line_Num { get; set; }
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public decimal? QTY { get; set; }
        public string ProductConversion_Name { get; set; }
    }

    public class DemoASNResponseViewModel
    {
        public string order_no { get; set; }
        public int? status { get; set; }
        public string message { get; set; }
    }

}
