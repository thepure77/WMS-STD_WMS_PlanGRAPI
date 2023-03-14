using Business.Library;
using Common.Utils;
using DataAccess;
using GRBusiness;
using MasterDataBusiness.ViewModels;
using Newtonsoft.Json;
using PlanGRBusiness.ViewModels;
using PlanGRDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanGRBusiness.Demo
{
    public class DemoService
    {
        private PlanGRDbContext db;

        public DemoService(PlanGRDbContext db)
        {
            this.db = db;
        }

        public DemoASNResponseViewModel CreateASN(DemoASNRequestViewModel param)
        {
            var result = new DemoASNResponseViewModel();
            var callback_req = new DemoCallbackViewModel();
            string message = "";
            var productList = new List<ProductViewModel>();
            var conversionList = new List<ProductConversionViewModelDoc>();
            var a = param.sJson();
            string state = "0";
            var logindex = Guid.NewGuid();
            try
            {
                SaveLogRequest(param.PlanGoodsReceive_No, param.sJson(), "Create ASN", 1, "", logindex);

                result.order_no = param.PlanGoodsReceive_No;
                var chkreq = CheckReq_ASN(param);
                if (chkreq != "")
                {
                    result.status = -1;
                    result.message = chkreq;
                    return result;
                }
                else { }

                var vendorfilterModel = new VendorViewModel();
                var vendorResult = new VendorViewModel();
                vendorfilterModel.vendor_Id = param.Vendor_Id;

                state = "0-1";
                var vendorMasterResult = utils.SendDataApi<actionResultVendorViewModel>(new AppSettingConfig().GetUrl("Vendor"), vendorfilterModel.sJson()).itemsVendor;
                if (vendorMasterResult.Count == 0)
                {
                    if (!string.IsNullOrEmpty(message))
                    {
                        message += " | ";
                    }
                    message += "Vendor_Id " + param.Vendor_Id + " not found";
                }
                else
                {
                    state = "0-2";
                    vendorResult = vendorMasterResult.Where(c => c.vendor_Id == param.Vendor_Id).FirstOrDefault();
                    if(vendorResult == null)
                    {
                        if (!string.IsNullOrEmpty(message))
                        {
                            message += " | ";
                        }
                        message += "Vendor_Id " + param.Vendor_Id + " not found";
                    }
                }

                state = "1";
                foreach (var i in param.items)
                {
                    i.Product_Id = i.Product_Id.TrimStart(new Char[] { '0' });
                    i.Product_Id = i.Product_Id.Trim(new Char[] { ' ', ' ' });

                    var productfilterModel = new ProductViewModel();
                    productfilterModel.product_Id = i.Product_Id;
                    //GetConfig
                    var productMasterResult = utils.SendDataApi<List<ProductViewModel>>(new AppSettingConfig().GetUrl("product"), productfilterModel.sJson());
                    
                    if (productMasterResult.Count == 0)
                    {
                        if (!string.IsNullOrEmpty(message))
                        {
                            message += " | ";
                        }
                        message += "Product " + i.Product_Id + " not found";
                        continue;
                    }
                    else
                    {
                        productList.Add(productMasterResult.FirstOrDefault());
                    }
                    

                    var conversionModel = new ProductConversionViewModelDoc();
                    conversionModel.productConversion_Name = i.ProductConversion_Name;
                    conversionModel.product_Index = productMasterResult[0].product_Index ?? Guid.Parse("00000000-0000-0000-0000-000000000000");
                    var conversionMasterResult = utils.SendDataApi<List<ProductConversionViewModelDoc>>(new AppSettingConfig().GetUrl("dropdownProductconversion"), conversionModel.sJson());
                    
                    if (conversionMasterResult.Count == 0)
                    {
                        if (!string.IsNullOrEmpty(message))
                        {
                            message += " | ";
                        }
                        message += "Product " + i.Product_Id + " Conversion "+ i.ProductConversion_Name+ " not found";
                        continue;
                    }
                    else
                    {
                        conversionList.Add(conversionMasterResult.FirstOrDefault());
                    }
                }

                state = "2";
                if (message != "")
                {
                    result.status = -1;
                    result.message = message;
                    return result;
                }
                else
                {
                    var owm = new GRBusiness.PlanGoodsReceive.ItemListViewModel();
                    var ownerResult = new GRBusiness.PlanGoodsReceive.ItemListViewModel();
                    //ownerResult = PlanGoodsReceiveService.autoOwnerfilter(owm).Where(c => c.index == Guid.Parse("02b31868-9d3d-448e-b023-05c121a424f4")).FirstOrDefault();
                    ownerResult = (utils.SendDataApi<List<GRBusiness.PlanGoodsReceive.ItemListViewModel>>(new AppSettingConfig().GetUrl("autoOwnerFilter"), owm.sJson())).Where(c => c.index == Guid.Parse("02b31868-9d3d-448e-b023-05c121a424f4")).FirstOrDefault();
                    var wh = new warehouseDocViewModel();
                    var warehouseResult = new warehouseDocViewModel();
                    //warehouseResult = PlanGoodsReceiveService.dropdownWarehouse(wh).Where(c => c.warehouse_Index == Guid.Parse("b0ad4e8f-a7b1-4952-bac7-1a9482baba79")).FirstOrDefault();
                    warehouseResult = (utils.SendDataApi<List<warehouseDocViewModel>>(new AppSettingConfig().GetUrl("dropdownWarehouse"), wh.sJson())).Where(c => c.warehouse_Index == Guid.Parse("b0ad4e8f-a7b1-4952-bac7-1a9482baba79")).FirstOrDefault();

                    var url = new AppSettingConfig().GetUrl("dropDownDocumentType");
                    var docrequest = new GenDocumentTypeViewModel();
                    docrequest.documentType_Index = Guid.Parse("7522BB74-8E9A-4819-AF18-12B13709C32C");
                    //docrequest.documentType_Id = "PK10";
                    docrequest.process_Index = Guid.Parse("C2A3F847-BAA6-46FE-B502-44F2D5826A1C");
                    var doctype = utils.SendDataApi<List<GenDocumentTypeViewModel>>(new AppSettingConfig().GetUrl("dropDownDocumentType"), docrequest.sJson());

                    var plan = db.IM_PlanGoodsReceive.Where(c => c.PlanGoodsReceive_No == param.PlanGoodsReceive_No && c.Document_Status != -1).FirstOrDefault();
                    if (param.Document_Status == "")
                    {
                        state = "3";
                        if (plan != null)
                        {
                            result.status = -1;
                            result.message = "Order Duplicate";
                            return result;
                        }

                        DateTime DocumentDate = Convert.ToDateTime(DateTime.Now);
                        IM_PlanGoodsReceive head = new IM_PlanGoodsReceive();
                        head.PlanGoodsReceive_Index = Guid.NewGuid();
                        head.PlanGoodsReceive_No = param.PlanGoodsReceive_No;
                        //head.PlanGoodsReceive_Date = Convert.ToDateTime(param.start_Date);
                        //head.PlanGoodsReceive_Due_Date = Convert.ToDateTime(param.end_Date);
                        head.PlanGoodsReceive_Date = DateTime.Now;
                        //head.PlanGoodsReceive_Due_Date = DateTime.Now;

                        head.Transaction_Id = param.WmsTrans_Id;
                        head.Owner_Index = ownerResult.index ?? Guid.Parse("02b31868-9d3d-448e-b023-05c121a424f4");
                        head.Owner_Id = ownerResult.name;
                        head.Owner_Name = ownerResult.id;
                        head.DocumentType_Index = doctype[0].documentType_Index;
                        head.DocumentType_Id = doctype[0].documentType_Id;
                        head.DocumentType_Name = doctype[0].documentType_Name;
                        head.Warehouse_Index = warehouseResult.warehouse_Index;
                        head.Warehouse_Id = warehouseResult.warehouse_Id;
                        head.Warehouse_Name = warehouseResult.warehouse_Name;
                        head.Create_By = param.Creat_By;
                        head.Create_Date = DateTime.Now;
                        head.Document_Status = 0;
                        head.Vendor_Index = vendorResult.vendor_Index;
                        head.Vendor_Id = vendorResult.vendor_Id;
                        head.Vendor_Name = vendorResult.vendor_Name;
                        db.IM_PlanGoodsReceive.Add(head);

                        int i = 0;
                        foreach (var item in param.items)
                        {
                            state = "4";
                            var planItem = new IM_PlanGoodsReceiveItem();
                            planItem.PlanGoodsReceiveItem_Index = Guid.NewGuid();
                            planItem.PlanGoodsReceive_Index = head.PlanGoodsReceive_Index;
                            planItem.LineNum = item.Line_Num;
                            //planItem.DocumentRef_No1 = item.Key.PLANT;
                            //planItem.DocumentRef_No2 = item.Key.SLOC;
                            planItem.Product_Index = productList[i].product_Index;
                            planItem.Product_Id = productList[i].product_Id;
                            planItem.Product_Name = productList[i].product_Name;
                            planItem.Product_SecondName = productList[i].product_SecondName;
                            planItem.Product_ThirdName = productList[i].product_ThirdName;
                            planItem.Qty = item.QTY;
                            planItem.Ratio = Convert.ToDecimal(conversionList[i].productconversion_Ratio);
                            planItem.TotalQty = planItem.Qty * planItem.Ratio;
                            planItem.ProductConversion_Index = conversionList[i].productConversion_Index;
                            planItem.ProductConversion_Id = conversionList[i].productConversion_Id;
                            planItem.ProductConversion_Name = conversionList[i].productConversion_Name;
                            //planItem.DocumentItem_Remark = item.Key.MESS;
                            //planItem.Price = decimal.Parse(item.Key.NETPR);
                            planItem.UnitWeight = conversionList[i].productConversion_Weight;
                            planItem.Weight = conversionList[i].productConversion_Weight * planItem.Qty;
                            planItem.NetWeight = conversionList[i].productConversion_Weight * planItem.Qty;
                            planItem.UnitGrsWeight = conversionList[i].productConversion_GrsWeight;
                            planItem.GrsWeight = conversionList[i].productConversion_GrsWeight * planItem.Qty;
                            planItem.UnitWidth = conversionList[i].productConversion_Width;
                            planItem.Width = conversionList[i].productConversion_Width * planItem.Qty;
                            planItem.UnitLength = conversionList[i].productConversion_Length;
                            planItem.Length = conversionList[i].productConversion_Length * planItem.Qty;
                            planItem.UnitHeight = conversionList[i].productConversion_Height;
                            planItem.Height = conversionList[i].productConversion_Height * planItem.Qty;
                            //planItem.UDF_1 = item.Key.ITEM_CAT;
                            //planItem.UDF_2 = item.Key.ACC_CAT;
                            //planItem.DocumentRef_No3 = model.item[i].GL_ACC;
                            //planItem.DocumentRef_No2 = model.item[i].GL_TXT;
                            var width = (planItem.UnitWidth ?? 0);
                            var Length = (planItem.UnitLength ?? 0);
                            var Height = (planItem.UnitHeight ?? 0);
                            var unitVolume = (width * Length * Height);
                            planItem.UnitVolume = unitVolume;
                            //(numQty * ((UnitWeight * UnitLength * UnitHeight) / productconversion.volume_Ratio)/ 1000000)
                            planItem.Volume = (planItem.Qty * (planItem.UnitWeight * planItem.UnitLength * planItem.UnitHeight));
                            planItem.UnitVolume = conversionList[i].productConversion_Volume;
                            planItem.ItemStatus_Index = Guid.Parse("525BCFF1-2AD9-4ACB-819D-0DEA4E84EA12");
                            planItem.ItemStatus_Id = "10";
                            planItem.ItemStatus_Name = "Goods-UR";
                            planItem.Weight_Index = Guid.Parse("080AEF7B-E9C5-4B84-969A-2D033F0C1E2A");
                            planItem.Weight_Id = "1";
                            planItem.Weight_Name = "KG";
                            planItem.WeightRatio = 1;
                            planItem.GrsWeight_Index = Guid.Parse("080AEF7B-E9C5-4B84-969A-2D033F0C1E2A");
                            planItem.GrsWeight_Id = "1";
                            planItem.GrsWeight_Name = "KG";
                            planItem.GrsWeightRatio = 1;
                            planItem.Width_Index = Guid.Parse("3778CD6E-45ED-499A-8ACC-9EB1F3AB1A6A");
                            planItem.Width_Id = "2";
                            planItem.Width_Name = "CM";
                            planItem.WidthRatio = 1;
                            planItem.Height_Index = Guid.Parse("3778CD6E-45ED-499A-8ACC-9EB1F3AB1A6A");
                            planItem.Height_Id = "2";
                            planItem.Height_Name = "CM";
                            planItem.HeightRatio = 1;
                            planItem.Length_Index = Guid.Parse("3778CD6E-45ED-499A-8ACC-9EB1F3AB1A6A");
                            planItem.Length_Id = "2";
                            planItem.Length_Name = "CM";
                            planItem.LengthRatio = 1;
                            planItem.Document_Status = 0;
                            planItem.Create_By = param.Creat_By;
                            planItem.Create_Date = DateTime.Now;
                            db.IM_PlanGoodsReceiveItem.Add(planItem);
                            state = "5";
                            i++;
                        }
                    }
                    else if (param.Document_Status == "U")
                    {
                        if (plan == null)
                        {
                            result.status = -1;
                            result.message = "Order not found";
                            return result;
                        }
                        else
                        {
                            if (plan.Document_Status != 0)
                            {
                                result.status = -1;
                                result.message = "Please check Order status";
                                return result;
                            }
                            else
                            {
                                plan.Transaction_Id = param.WmsTrans_Id;
                                //plan.PlanGoodsReceive_Date = Convert.ToDateTime(param.start_Date);
                                //plan.PlanGoodsReceive_Due_Date = Convert.ToDateTime(param.end_Date);
                                plan.PlanGoodsReceive_Date = DateTime.Now;
                                //plan.PlanGoodsReceive_Due_Date = DateTime.Now;

                                plan.Vendor_Index = vendorResult.vendor_Index;
                                plan.Vendor_Id = vendorResult.vendor_Id;
                                plan.Vendor_Name = vendorResult.vendor_Name;
                                plan.Update_By = param.Creat_By;
                                plan.Update_Date = DateTime.Now;

                                int i = 0;
                                foreach (var item in param.items)
                                {
                                    var planItem = db.IM_PlanGoodsReceiveItem.Where(c => c.PlanGoodsReceive_Index == plan.PlanGoodsReceive_Index && c.LineNum == item.Line_Num && c.Document_Status != -1).FirstOrDefault();
                                    if (planItem == null)
                                    {
                                        result.status = -1;
                                        result.message = "Line num " + item.Line_Num + " not found";
                                        return result;
                                    }
                                    else
                                    {
                                      
                                        //planItem.PlanGoodsReceiveItem_Index = Guid.NewGuid();
                                        //planItem.PlanGoodsReceive_Index = head.PlanGoodsReceive_Index;
                                        //planItem.LineNum = item.Line_Num;
                                        //planItem.DocumentRef_No1 = item.Key.PLANT;
                                        //planItem.DocumentRef_No2 = item.Key.SLOC;
                                        planItem.Product_Index = productList[i].product_Index;
                                        planItem.Product_Id = productList[i].product_Id;
                                        planItem.Product_Name = productList[i].product_Name;
                                        planItem.Qty = item.QTY;
                                        planItem.Ratio = Convert.ToDecimal(conversionList[i].productconversion_Ratio);
                                        planItem.TotalQty = planItem.Qty * planItem.Ratio;
                                        planItem.ProductConversion_Index = conversionList[i].productConversion_Index;
                                        planItem.ProductConversion_Id = conversionList[i].productConversion_Id;
                                        planItem.ProductConversion_Name = conversionList[i].productConversion_Name;
                                        //planItem.DocumentItem_Remark = item.Key.MESS;
                                        //planItem.Price = decimal.Parse(item.Key.NETPR);
                                        planItem.UnitWeight = conversionList[i].productConversion_Weight;
                                        planItem.Weight = conversionList[i].productConversion_Weight * planItem.Qty;
                                        planItem.NetWeight = conversionList[i].productConversion_Weight * planItem.Qty;
                                        planItem.UnitGrsWeight = conversionList[i].productConversion_GrsWeight;
                                        planItem.GrsWeight = conversionList[i].productConversion_GrsWeight * planItem.Qty;
                                        planItem.UnitWidth = conversionList[i].productConversion_Width;
                                        planItem.Width = conversionList[i].productConversion_Width * planItem.Qty;
                                        planItem.UnitLength = conversionList[i].productConversion_Length;
                                        planItem.Length = conversionList[i].productConversion_Length * planItem.Qty;
                                        planItem.UnitHeight = conversionList[i].productConversion_Height;
                                        planItem.Height = conversionList[i].productConversion_Height * planItem.Qty;
                                        //planItem.UDF_1 = item.Key.ITEM_CAT;
                                        //planItem.UDF_2 = item.Key.ACC_CAT;
                                        //planItem.DocumentRef_No3 = model.item[i].GL_ACC;
                                        //planItem.DocumentRef_No2 = model.item[i].GL_TXT;
                                        var width = (planItem.UnitWidth ?? 0);
                                        var Length = (planItem.UnitLength ?? 0);
                                        var Height = (planItem.UnitHeight ?? 0);
                                        var unitVolume = (width * Length * Height);
                                        planItem.UnitVolume = unitVolume;
                                        //(numQty * ((UnitWeight * UnitLength * UnitHeight) / productconversion.volume_Ratio)/ 1000000)
                                        planItem.Volume = (planItem.Qty * (planItem.UnitWeight * planItem.UnitLength * planItem.UnitHeight));
                                        planItem.UnitVolume = conversionList[i].productConversion_Volume;
                                        planItem.ItemStatus_Index = Guid.Parse("525BCFF1-2AD9-4ACB-819D-0DEA4E84EA12");
                                        planItem.ItemStatus_Id = "10";
                                        planItem.ItemStatus_Name = "Goods-UR";
                                        planItem.Weight_Index = Guid.Parse("080AEF7B-E9C5-4B84-969A-2D033F0C1E2A");
                                        planItem.Weight_Id = "1";
                                        planItem.Weight_Name = "KG";
                                        planItem.WeightRatio = 1;
                                        planItem.GrsWeight_Index = Guid.Parse("080AEF7B-E9C5-4B84-969A-2D033F0C1E2A");
                                        planItem.GrsWeight_Id = "1";
                                        planItem.GrsWeight_Name = "KG";
                                        planItem.GrsWeightRatio = 1;
                                        planItem.Width_Index = Guid.Parse("3778CD6E-45ED-499A-8ACC-9EB1F3AB1A6A");
                                        planItem.Width_Id = "2";
                                        planItem.Width_Name = "CM";
                                        planItem.WidthRatio = 1;
                                        planItem.Height_Index = Guid.Parse("3778CD6E-45ED-499A-8ACC-9EB1F3AB1A6A");
                                        planItem.Height_Id = "2";
                                        planItem.Height_Name = "CM";
                                        planItem.HeightRatio = 1;
                                        planItem.Length_Index = Guid.Parse("3778CD6E-45ED-499A-8ACC-9EB1F3AB1A6A");
                                        planItem.Length_Id = "2";
                                        planItem.Length_Name = "CM";
                                        planItem.LengthRatio = 1;
                                        planItem.Document_Status = 0;
                                        planItem.Create_By = param.Creat_By;
                                        planItem.Create_Date = DateTime.Now;
                                        db.IM_PlanGoodsReceiveItem.Add(planItem);
                                    }
                                    i++;
                                }
                            }
                            
                        }
                    }
                    else if (param.Document_Status == "D")
                    {
                        if(plan == null)
                        {
                            result.status = -1;
                            result.message = "Order not found";
                            return result;
                        }
                        else
                        {
                            if(plan.Document_Status != 0)
                            {
                                result.status = -1;
                                result.message = "Please check Order status";
                                return result;
                            }
                            else
                            {
                                plan.Document_Status = -1;
                                plan.Update_By = param.Creat_By;
                                plan.Update_Date = DateTime.Now;
                                plan.Cancel_By = param.Creat_By;
                                plan.Cancel_Date = DateTime.Now;
                            }
                            
                        }
                    }
                    else
                    {
                        result.status = -1;
                        result.message = "Document_Status not map";
                        return result;
                    }
                }
                state = "6";
                db.SaveChanges();
                result.status = 1;
                result.message = "Success";

                SaveLogResponse(param.PlanGoodsReceive_No, param.sJson(), "Create ASN", 1, "", logindex);

                return result;
            }
            catch(Exception ex)
            {
                result.status = -1;
                result.message = ex.Message;

                SaveLogResponse(param.PlanGoodsReceive_No, result.sJson(), "Create SO", -1, result.message, logindex);

                return result;
            } 
        }

        public string CheckReq_ASN (DemoASNRequestViewModel param)
        {
            var result = "";

            if (string.IsNullOrEmpty(param.WmsTrans_Id))
            {
                if (!string.IsNullOrEmpty(result))
                {
                    result += "|";
                }
                else
                {
                    result += " WmsTrans_Id is empty";
                    return result;
                }
            }
            else { }

            if (string.IsNullOrEmpty(param.PlanGoodsReceive_No))
            {
                if (!string.IsNullOrEmpty(result))
                {
                    result += "|";
                }
                else
                {
                    result += " PlanGoodsReceive_No is empty";
                    return result;
                }
            }
            else { }

            if (string.IsNullOrEmpty(param.Vendor_Id))
            {
                if (!string.IsNullOrEmpty(result))
                {
                    result += "|";
                }
                else
                {
                    result += " Vendor_Id is empty";
                    return result;
                }
            }
            else { }

            //if (string.IsNullOrEmpty(param.Sloc_Id))
            //{
            //    if (!string.IsNullOrEmpty(result))
            //    {
            //        result += "|";
            //    }
            //    else
            //    {
            //        result += " Sloc_Id is empty";
            //        return result;
            //    }
            //}
            //else { }

            //if (param.DOC_TYP == "N5WK" && string.IsNullOrEmpty(param.start_Date))
            //{
            //    if (!string.IsNullOrEmpty(result))
            //    {
            //        result += "|";
            //    }
            //    else
            //    {
            //        result += " start_Date is empty";
            //        return result;
            //    }
            //}
            //else { }

            //if (param.DOC_TYP == "N5WK" && string.IsNullOrEmpty(param.end_Date))
            //{
            //    if (!string.IsNullOrEmpty(result))
            //    {
            //        result += "|";
            //    }
            //    else
            //    {
            //        result += " end_Date is empty";
            //        return result;
            //    }
            //}
            //else { }


            if (param.Document_Status != "" && param.Document_Status != "U" && param.Document_Status != "D")
            {
                if (!string.IsNullOrEmpty(result))
                {
                    result += "|";
                }
                else
                {
                    result += " Document_Status not map";
                    return result;
                }
            }
            else { }

            if (param.items.Count == 0)
            {
                if (!string.IsNullOrEmpty(result))
                {
                    result += "|";
                }
                else
                {
                    result += " items is empty";
                    return result;
                }
            }
            else { }

            foreach (var item in param.items)
            {
                if (string.IsNullOrEmpty(item.Line_Num))
                {
                    if (!string.IsNullOrEmpty(result))
                    {
                        result += "|";
                    }
                    else
                    {
                        result += " Line_Num is empty";
                        return result;
                    }
                }
                else { }

                if (string.IsNullOrEmpty(item.Product_Id))
                {
                    if (!string.IsNullOrEmpty(result))
                    {
                        result += "|";
                    }
                    else
                    {
                        result += " Product_Id is empty";
                        return result;
                    }
                }
                else { }

                if (string.IsNullOrEmpty(item.Product_Name))
                {
                    if (!string.IsNullOrEmpty(result))
                    {
                        result += "|";
                    }
                    else
                    {
                        result += " Product_Name is empty";
                        return result;
                    }
                }
                else { }

                if (item.QTY == null || item.QTY == 0)
                {
                    if (!string.IsNullOrEmpty(result))
                    {
                        result += "|";
                    }
                    else
                    {
                        result += " Plan_QTY is empty";
                        return result;
                    }
                }
                else { }

                if (string.IsNullOrEmpty(item.ProductConversion_Name))
                {
                    if (!string.IsNullOrEmpty(result))
                    {
                        result += "|";
                    }
                    else
                    {
                        result += " Sale_Unit is empty";
                        return result;
                    }
                }
                else { }
            }


            return result;
        }

        public string SaveLogRequest(string orderno, string json, string interfacename, int status, string txt, Guid logindex)
        {
            try
            {
                log_api_request l = new log_api_request();
                l.log_id = logindex;
                l.log_date = DateTime.Now;
                l.log_requestbody = json;
                l.log_absoluteuri = "";
                l.status = status;
                l.Interface_Name = interfacename;
                l.Status_Text = txt;
                l.File_Name = orderno;
                db.log_api_request.Add(l);
                db.SaveChanges();
                return "";
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public string SaveLogResponse(string orderno, string json, string interfacename, int status, string txt, Guid logindex)
        {
            try
            {
                bool IsNew = false;
                log_api_reponse l = new log_api_reponse();
                l.log_id = logindex;
                l.log_date = DateTime.Now;
                l.log_reponsebody = json;
                l.log_absoluteuri = "";
                l.status = status;
                l.Interface_Name = interfacename;
                l.Status_Text = txt;
                l.File_Name = orderno;
                db.log_api_reponse.Add(l);

                var d = db.log_api_request.Find(logindex);
                if (d == null)
                {
                    IsNew = true;
                    d = new log_api_request();
                    d.log_id = logindex;
                    d.log_date = DateTime.Now;
                    d.log_requestbody = "";
                    d.log_absoluteuri = "";
                    d.status = status;
                    d.Interface_Name = interfacename;
                    d.Status_Text = txt;
                    d.File_Name = orderno;
                }
                d.status = status;
                d.Status_Text = txt;

                if (IsNew)
                {
                    db.log_api_request.Add(d);
                }
                else
                {
                }

                db.SaveChanges();
                return "";
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public string Callback_TMS(Guid id)
        {
            var logindex = Guid.NewGuid();
            try
            {
                //var conversionMasterResult = utils.SendDataApi<List<ProductConversionViewModelDoc>>(new AppSettingConfig().GetUrl("callback_OMS"), param.sJson());

                var data = db.IM_PlanGoodsReceive.Where(c => c.PlanGoodsReceive_Index == id).FirstOrDefault();
                var dataItem = db.IM_PlanGoodsReceiveItem.Where(c => c.PlanGoodsReceive_Index == id && c.Document_Status != -1).ToList();
                

                var result = new DemoCallbackViewModel();
                result.packageCode          = "BIGTHN202209160001";
                result.packageOriginType    = "Domestic";
                result.packageType          = "Sales_order";
                result.firstmileTplSlug     = "thai-parcel";
                result.trackingNumber       = data.PlanGoodsReceive_No;
                result.tplSlug              ="thai-parcel";
                result.tplName              = "thai-parcel";
                result.deliveryServiceType  = "standard";
                result.deliveryPriority     = false;
                result.pickupType           = "Pickup";
                result.api = "TPS_PACKAGE_CREATE";

                PlatformInfo pinfo = new PlatformInfo();
                pinfo.platformName = "BigThailand";
                pinfo.platformOrderNumber = "BT-117D2C8E66AA";
                pinfo.platformOrderId = "19833";
                result.platformInfo = pinfo;

                Payment pay = new Payment();
                pay.paymentType = "NONE_COD";
                pay.shippingType = "warehouse";
                pay.currency= "THB";
                result.payment = pay;

                Dimweight dmweight = new Dimweight();
                dmweight.weight = dataItem.Sum(s => s.UnitWeight);
                dmweight.volume = dataItem.Sum(s => s.UnitVolume);
                dmweight.height = dataItem.Sum(s => s.UnitHeight);
                dmweight.width = dataItem.Sum(s => s.UnitWidth);
                dmweight.length = dataItem.Sum(s => s.UnitLength);
                result.dimweight = dmweight;

                Shipper shipper = new Shipper();
                shipper.sellerId = "20618";
                result.shipper = shipper;

                Origin origin = new Origin();
                origin.name = "Lazada Express Limited";
                origin.phone = "0925549954";
                origin.email = "doppio.pois@gmail.com";
                Address origin_detail = new Address();
                origin_detail.country = "TH";
                origin_detail.province = "กรุงเทพมหานคร";
                origin_detail.city = "วัฒนา";
                origin_detail.district = "คลองเตยเหนือ";
                origin_detail.zipCode = "10110";
                origin_detail.details = "Unit No.2904 – 2906, 29th Floor, Bhiraj Tower, 689 Sukhumvit Road, KlongtonNua, Wattana,";
                origin.address = origin_detail;

                result.origin = origin;

                Destination destination = new Destination();
                destination.name = "Lazada Express Limited (Collect)";
                destination.phone = "0928767649";
                destination.email = "ppppppp@gmail.com";
                Address destination_detail = new Address();
                destination_detail.country = "TH";
                destination_detail.province = "กรุงเทพมหานคร/ Bangkok";
                destination_detail.city = "วัฒนา";
                destination_detail.district = "คลองเตยเหนือ";
                destination_detail.zipCode = "10110";
                destination_detail.details = "Lazada Express Limited (Collect)";
                destination.address = destination_detail;

                result.destination = destination;

                ReturnInfo returnInfo = new ReturnInfo();
                returnInfo.name = "Lazada Express Limited";
                returnInfo.phone = "0925549954";
                returnInfo.email = "doppio.pois@gmail.com";
                Address returnInfo_detail = new Address();
                returnInfo_detail.country = "TH";
                returnInfo_detail.province = "กรุงเทพมหานคร";
                returnInfo_detail.city = "วัฒนา";
                returnInfo_detail.district = "คลองเตยเหนือ";
                returnInfo_detail.zipCode = "10110";
                returnInfo_detail.details = "Unit No.2904 – 2906, 29th Floor, Bhiraj Tower, 689 Sukhumvit Road, KlongtonNua, Wattana,";
                returnInfo.address = returnInfo_detail;

                result.returnInfo = returnInfo;

                List<CallBackItem> cbitems = new List<CallBackItem>();
                foreach(var i in dataItem)
                {
                    CallBackItem cbItem = new CallBackItem();
                    cbItem.name = i.Product_Name;
                    cbItem.sku = i.Product_Id;
                    cbItem.quantity = Convert.ToInt32(i.Qty);

                    ItemDW iDW = new ItemDW();
                    iDW.weight = i.UnitWeight * 1000;
                    if (i.UnitVolume == null)
                    {
                        iDW.volume = (i.UnitWidth * i.UnitLength * i.UnitHeight);
                    }
                    else
                    {
                        iDW.volume = i.UnitVolume;
                    }
                    iDW.height = i.UnitHeight;
                    iDW.width = i.UnitWidth;
                    iDW.length = i.UnitLength;
                    cbItem.itemDW = iDW;

                    cbitems.Add(cbItem);
                }
                result.items = cbitems;

                var token = new AppSettingConfig().GetUrl("callback_Bearer");
                var sURL = new AppSettingConfig().GetUrl("callback_TMS");
                var JsonData = result.sJson();

                SaveLogRequest(id.ToString(), JsonData, "Call TMS", 1, "", logindex);

                //var conversionMasterResult = utils.SendDataApi<List<ProductConversionViewModelDoc>>(new AppSettingConfig().GetUrl("callback_OMS"), param.sJson());

                using (var client = new System.Net.Http.HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.Timeout = TimeSpan.FromMinutes(30);

                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);


                    var content = new System.Net.Http.StringContent(JsonData, Encoding.UTF8, "application/json");
                    var callresult = client.PostAsync(sURL, content).Result;
                    var contentResult = callresult.Content.ReadAsStringAsync().Result;
                    //return contentResult.ToString();

                    var resModel = JsonConvert.DeserializeObject<dynamic>(contentResult.ToString());

                    SaveLogResponse(id.ToString(), contentResult, "Call TMS", 1, "", logindex);

                    if (resModel.success != true)
                    {
                        return resModel.errors[0].message;
                    }
                    else
                    {
                        return "Success";
                    }
                }

                //return "Success";
            }
            catch(Exception ex)
            {
                SaveLogResponse(id.ToString(), ex.Message, "Call TMS", -1, ex.Message, logindex);
                return "Send TMS Fail";
            }
        }
    }
}
