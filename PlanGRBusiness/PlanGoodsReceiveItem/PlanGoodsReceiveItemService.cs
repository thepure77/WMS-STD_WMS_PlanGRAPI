using Business.Library;
using Comone.Utils;
using DataAccess;
using GRBusiness;
using GRBusiness.PlanGoodsReceive;
using Microsoft.EntityFrameworkCore;
using PlanGRBusiness.PlanGoodsReceive;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace PlanGRBusiness.PlanGoodsReceiveItem
{
    public class PlanGoodsReceiveItemService
    {
        //private PlanGRDbContext db = new PlanGRDbContext();
        private PlanGRDbContext db ;

        //public PlanGoodsReceiveItemService()
        //{
        //    db = new PlanGRDbContext();
        //}

        public PlanGoodsReceiveItemService(PlanGRDbContext db)
        {
            this.db = db;
        }

        public PlanGoodsReceiveItemService()
        {
        }

        public List<PlanGoodsReceiveItemDocViewModel> GetByPlanGoodReceiveId(Guid id)
        {
            try
            {
                var result = new List<PlanGoodsReceiveItemDocViewModel>();

                var queryResult = db.IM_PlanGoodsReceiveItem.Where(c => c.PlanGoodsReceive_Index == id && c.Document_Status != -1).ToList();

                foreach (var data in queryResult)
                {
                    var item = new PlanGoodsReceiveItemDocViewModel();

                    item.planGoodsReceive_Index = data.PlanGoodsReceive_Index;
                    item.planGoodsReceiveItem_Index = data.PlanGoodsReceiveItem_Index;
                    item.product_Index = data.Product_Index;
                    item.lineNum = data.LineNum;
                    item.product_Id = data.Product_Id;
                    item.product_Name = data.Product_Name;
                    item.product_SecondName = data.Product_SecondName;
                    item.product_ThirdName = data.Product_ThirdName;
                    item.product_Lot = data.Product_Lot;
                    item.itemStatus_Index = data.ItemStatus_Index;
                    item.itemStatus_Id = data.ItemStatus_Id;
                    item.itemStatus_Name = data.ItemStatus_Name;
                    //item.qty = string.Format(String.Format("{0:N3}", data.Qty));
                    item.qty = data.Qty;
                    item.ratio = data.Ratio;
                    item.totalQty = data.TotalQty;
                    item.productConversion_Index = data.ProductConversion_Index;
                    item.productConversion_Id = data.ProductConversion_Id;
                    item.productConversion_Name = data.ProductConversion_Name;
                    item.mFG_Date = data.MFG_Date.toString();
                    item.eXP_Date = data.EXP_Date.toString();

                    item.unitWeight = data.UnitWeight;
                    item.weight = data.Weight;
                    item.weight_Index = data.Weight_Index;
                    item.weight_Id = data.Weight_Id;
                    item.weight_Name = data.Weight_Name;
                    item.netWeight = data.NetWeight;

                    item.unitGrsWeight = data.UnitGrsWeight;
                    item.grsWeight = data.GrsWeight;
                    item.grsWeight_Index = data.GrsWeight_Index;
                    item.grsWeight_Id = data.GrsWeight_Id;
                    item.grsWeight_Name = data.GrsWeight_Name;

                    item.unitWidth = data.UnitWidth;
                    item.width = data.Width;
                    item.width_Index = data.Width_Index;
                    item.width_Id = data.Width_Id;
                    item.width_Name = data.Width_Name;

                    item.unitLength = data.UnitLength;
                    item.length = data.Length;
                    item.length_Index = data.Length_Index;
                    item.length_Id = data.Length_Id;
                    item.length_Name = data.Length_Name;

                    item.unitHeight = data.UnitHeight;
                    item.height = data.Height;
                    item.height_Index = data.Height_Index;
                    item.height_Id = data.Height_Id;
                    item.height_Name = data.Height_Name;

                    item.unitVolume = data.UnitVolume;
                    item.volume = data.Volume;


                    item.unitPrice = data.UnitPrice;
                    item.price = data.Price;
                    item.totalPrice = data.TotalQty;

                    item.currency_Index = data.Currency_Index;
                    item.currency_Id = data.Currency_Id;
                    item.currency_Name = data.Currency_Name;

                    item.ref_Code1 = data.Ref_Code1;
                    item.ref_Code2 = data.Ref_Code2;
                    item.ref_Code3 = data.Ref_Code3;
                    item.ref_Code4 = data.Ref_Code4;
                    item.ref_Code5 = data.Ref_Code5;

                    item.documentRef_No1 = data.DocumentRef_No1;
                    item.documentRef_No2 = data.DocumentRef_No2;
                    item.documentRef_No3 = data.DocumentRef_No3;
                    item.documentRef_No4 = data.DocumentRef_No4;
                    item.documentRef_No5 = data.DocumentRef_No5;
                    item.document_Status = data.Document_Status;
                    item.documentItem_Remark = data.DocumentItem_Remark;
                    item.uDF_1 = data.UDF_1;
                    item.uDF_2 = data.UDF_2;
                    item.uDF_3 = data.UDF_3;
                    item.uDF_4 = data.UDF_4;
                    item.uDF_5 = data.UDF_5;

                    item.ref_Document_No = data.Ref_Document_No;

                    var sku = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("autoSkufilter"), new { key = data.Product_Id}.sJson()).FirstOrDefault();
                    item.productId_Ref1 = sku.value1;
                    item.productId_Ref2 = sku.value2;

                    result.Add(item);
                }
                return result;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PlanGoodsReceiveItemDocViewModel> returnmatdoc(string id)
        {
            try
            {
                var result = new List<PlanGoodsReceiveItemDocViewModel>();

                var matdoc_ = new { matdoc = id };
                var itemmatdoc = utils.SendDataApi<List<PlanGoodIssueDocViewModelItem>>(new AppSettingConfig().GetUrl("returnmatdoc"), matdoc_.sJson());

                foreach (var data in itemmatdoc)
                {
                    var item = new PlanGoodsReceiveItemDocViewModel();

                    item.product_Index = data.product_Index;
                    item.lineNum = data.lineNum;
                    item.product_Id = data.product_Id;
                    item.product_Name = data.product_Name;
                    item.product_SecondName = data.product_SecondName;
                    item.product_ThirdName = data.product_ThirdName;
                    item.product_Lot = data.product_Lot;
                    item.itemStatus_Index = data.itemStatus_Index;
                    item.itemStatus_Id = data.itemStatus_Id;
                    item.itemStatus_Name = data.itemStatus_Name;
                    item.qty = 0;
                    item.defult_qty = decimal.Parse(data.qty);
                    item.ratio = data.ratio;
                    item.totalQty = data.totalQty;
                    item.productConversion_Index = data.productConversion_Index;
                    item.productConversion_Id = data.productConversion_Id;
                    item.productConversion_Name = data.productConversion_Name;
                    item.mFG_Date = data.mFG_Date == null ? null : data.mFG_Date.toString();
                    item.eXP_Date = data.eXP_Date == null ? null : data.eXP_Date.toString();

                    item.unitWeight = data.unitWeight;
                    item.weight = data.weight;
                    item.weight_Index = data.weight_Index;
                    item.weight_Id = data.weight_Id;
                    item.weight_Name = data.weight_Name;
                    item.netWeight = data.netWeight;

                    item.unitGrsWeight = data.unitGrsWeight;
                    item.grsWeight = data.grsWeight;
                    item.grsWeight_Index = data.grsWeight_Index;
                    item.grsWeight_Id = data.grsWeight_Id;
                    item.grsWeight_Name = data.grsWeight_Name;

                    item.unitWidth = data.unitWidth;
                    item.width = data.width;
                    item.width_Index = data.width_Index;
                    item.width_Id = data.width_Id;
                    item.width_Name = data.width_Name;

                    item.unitLength = data.unitLength;
                    item.length = data.length;
                    item.length_Index = data.length_Index;
                    item.length_Id = data.length_Id;
                    item.length_Name = data.length_Name;

                    item.unitHeight = data.unitHeight;
                    item.height = data.height;
                    item.height_Index = data.height_Index;
                    item.height_Id = data.height_Id;
                    item.height_Name = data.height_Name;

                    item.unitVolume = data.unitVolume;
                    item.volume = data.volume;


                    item.unitPrice = data.unitPrice;
                    item.price = data.price;

                    item.documentRef_No1 = data.documentRef_No1;
                    item.documentRef_No2 = data.documentRef_No2;
                    item.documentRef_No3 = data.documentRef_No3;
                    item.documentRef_No4 = data.documentRef_No4;
                    item.documentRef_No5 = data.documentRef_No5;
                    item.document_Status = data.document_Status;
                    item.documentItem_Remark = data.documentItem_Remark;
                    item.uDF_1 = data.uDF_1;
                    item.uDF_2 = data.uDF_2;
                    item.uDF_3 = data.uDF_3;
                    item.uDF_4 = data.uDF_4;
                    item.uDF_5 = data.uDF_5;
                    
                    result.Add(item);
                }
                return result;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PlanGoodsReceiveItemViewModel find(Guid id)
        {
            if (id == Guid.Empty) { throw new NullReferenceException(); }

            try
            {

                var queryResult = db.IM_PlanGoodsReceiveItem.Where(c => c.PlanGoodsReceiveItem_Index == id).FirstOrDefault();

                var item = new PlanGoodsReceiveItemViewModel();

                item.planGoodsReceive_Index = queryResult.PlanGoodsReceive_Index;
                item.planGoodsReceiveItem_Index = queryResult.PlanGoodsReceiveItem_Index;
                item.product_Index = queryResult.Product_Index;
                item.product_Id = queryResult.Product_Id;
                item.product_Name = queryResult.Product_Name;
                item.product_SecondName = queryResult.Product_SecondName;
                item.product_ThirdName = queryResult.Product_ThirdName;
                item.product_Lot = queryResult.Product_Lot;
                item.itemStatus_Index = queryResult.ItemStatus_Index;
                item.itemStatus_Id = queryResult.ItemStatus_Id;
                item.itemStatus_Name = queryResult.ItemStatus_Name;
                item.qty = queryResult.Qty;
                item.ratio = queryResult.Ratio;
                item.totalQty = queryResult.TotalQty;
                item.productConversion_Index = queryResult.ProductConversion_Index;
                item.productConversion_Id = queryResult.ProductConversion_Id;
                item.productConversion_Name = queryResult.ProductConversion_Name;
                item.mFG_Date = queryResult.MFG_Date;
                item.eXP_Date = queryResult.EXP_Date;
                item.weight = queryResult.Weight;
                item.unitWeight = queryResult.UnitWeight;
                item.unitWidth = queryResult.UnitWidth;
                item.unitLength = queryResult.UnitLength;
                item.unitHeight = queryResult.UnitHeight;
                item.unitVolume = queryResult.UnitVolume;
                item.volume = queryResult.Volume;
                item.unitPrice = queryResult.UnitPrice;
                item.price = queryResult.Price;
                item.documentRef_No1 = queryResult.DocumentRef_No1;
                item.documentRef_No2 = queryResult.DocumentRef_No2;
                item.documentRef_No3 = queryResult.DocumentRef_No3;
                item.documentRef_No4 = queryResult.DocumentRef_No4;
                item.documentRef_No5 = queryResult.DocumentRef_No5;
                item.document_Status = queryResult.Document_Status;
                item.documentItem_Remark = queryResult.DocumentItem_Remark;
                item.uDF_1 = queryResult.UDF_1;
                item.uDF_2 = queryResult.UDF_2;
                item.uDF_3 = queryResult.UDF_3;
                item.uDF_4 = queryResult.UDF_4;
                item.uDF_5 = queryResult.UDF_5;

                item.ref_Process_Index = queryResult.Ref_Process_Index;
                item.ref_Document_LineNum = queryResult.Ref_Document_LineNum;
                item.ref_Document_No = queryResult.Ref_Document_No;
                item.ref_Document_Index = queryResult.Ref_Document_Index;
                item.ref_DocumentItem_Index = queryResult.Ref_DocumentItem_Index;

                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<GetGoodsReceiveViewModel> GetGoodsReceiveItem(Guid id)
        {
            try
            {
                string pstring = " and Ref_Document_Index = '" + id + "'";
                pstring += " and Document_Status != -1 ";

                var strwhere = new SqlParameter("@strwhere", pstring);
                var queryResult = db.IM_GoodsReceiveItem.FromSql("sp_GetGoodsReceiveItem @strwhere", strwhere).ToList();

                var result = new List<GetGoodsReceiveViewModel>();

                foreach (var item in queryResult)
                {
                    var resultItem = new GetGoodsReceiveViewModel();

                    resultItem.GoodsReceiveIndex = item.GoodsReceive_Index;
                    resultItem.RefDocumentNo = item.Ref_Document_No;
                    resultItem.GoodsReceiveItemIndex = item.GoodsReceiveItem_Index;
                    var strwhere1 = new SqlParameter("@strwhere", " and GoodsReceive_Index = '" + item.GoodsReceive_Index + "'");
                    var itemList = db.IM_GoodsReceives.FromSql("sp_GetGoodsReceive @strwhere", strwhere1).FirstOrDefault();
                    resultItem.GoodsReceiveNo = itemList.GoodsReceive_No;
                    resultItem.ProductName = item.Product_Name;
                    resultItem.ProductSecondName = item.Product_SecondName;
                    resultItem.ProductConversionName = item.ProductConversion_Name;
                    resultItem.TotalQty = item.TotalQty;
                    resultItem.qty = item.Qty;
                    resultItem.Weight = item.Weight;
                    resultItem.Volume = item.Volume;
                    resultItem.ProductId = item.Product_Id;
                    resultItem.ProductName = item.Product_Name;
                    resultItem.ProductConversionName = item.ProductConversion_Name;
                    resultItem.ItemStatusIndex = item.ItemStatus_Index;
                    resultItem.ItemStatusName = item.ItemStatus_Name;
                    resultItem.ItemStatusId = item.ItemStatus_Id;
                    resultItem.DocumentStatus = item.Document_Status;
                    resultItem.ProductConversionName = item.ProductConversion_Name;
                    resultItem.GoodsReceiveDate = itemList.GoodsReceive_Date.toString();
                    resultItem.Create_Date = item.Create_Date.GetValueOrDefault();
                    resultItem.Create_By = item.Create_By;
                    resultItem.Update_Date = item.Update_Date.GetValueOrDefault();
                    resultItem.Update_By = item.Update_By;
                    resultItem.Cancel_Date = item.Cancel_Date.GetValueOrDefault();
                    resultItem.Cancel_By = item.Cancel_By;
                    result.Add(resultItem);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RemainQtyViewModel> GetRemainQty(Guid id)
        {
            try
            {
                string pstring = " and PlanGoodsReceive_Index = '" + id + "'";
                pstring += "  and Total > 0 ";
                var strwhere = new SqlParameter("@strwhere", pstring);

                var queryResult = db.View_GoodsReceivePending.FromSql("sp_GetGoodsReceivePending @strwhere", strwhere).ToList();

                var result = new List<RemainQtyViewModel>();

                foreach (var item in queryResult)
                {
                    var resultItem = new RemainQtyViewModel();

                    resultItem.PlanGoodsReceive_Index = item.PlanGoodsReceive_Index;
                    resultItem.PlanGoodsReceiveItem_Index = item.PlanGoodsReceiveItem_Index;
                    resultItem.ProductConversionId = item.ProductConversion_Id;
                    resultItem.ProductConversionName = item.ProductConversion_Name;
                    resultItem.ProductId = item.Product_Id;
                    resultItem.ProductName = item.Product_Name;
                    resultItem.ProductSecondName = item.Product_SecondName;
                    resultItem.Total = item.Total;
                    resultItem.Qty = item.Qty;
                    resultItem.Ratio = item.Ratio;
                    resultItem.GRTotalQty = item.GRTotalQty;
                    //resultItem.GoodsReceiveDate = item.GoodsReceive_Date.toString();



                    result.Add(resultItem);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PlanGoodsIssueItemPopViewModel> GetPlanGoodsIssueItemPopup(PlanGoodsIssueItemPopViewModel model)
        {
            try
            {
                var result = new List<PlanGoodsIssueItemPopViewModel>();

                var pstring = "";

                pstring = " and PlanGoodsIssue_Index = '" + model.PlanGoodsIssueIndex + "' and Document_Status != -1 ";

                var strwhere1 = new SqlParameter("@strwhere", pstring);
                var queryResult = db.Get_PlanGoodsIssueItemPopup.FromSql("sp_GetPlanGoodsIssueReturnReceive @strwhere", strwhere1).ToList();


                foreach (var data in queryResult)
                {
                    var item = new PlanGoodsIssueItemPopViewModel();

                    var strwhere = new SqlParameter("@strwhere", " and Ref_Document_Index = '" + model.PlanGoodsIssueIndex + "' and Ref_DocumentItem_Index ='" + data.PlanGoodsIssueItem_Index + "'");
                    //var chkGIL = context.IM_GoodsIssueItemLocation.FromSql("sp_GetGoodsIssueItemLocation @strwhere", strwhere).FirstOrDefault();
                    item.PlanGoodsIssueIndex = data.PlanGoodsIssue_Index;
                    item.PlanGoodsIssueItemIndex = data.PlanGoodsIssueItem_Index;
                    item.PlanGoodsIssueNo = data.PlanGoodsIssue_No;
                    item.ProductIndex = data.Product_Index;
                    item.ProductId = data.Product_Id;
                    item.ProductName = data.Product_Name;
                    item.ProductSecondName = data.Product_SecondName;
                    item.ProductThirdName = data.Product_ThirdName;
                    item.ProductLot = data.Product_Lot;
                    item.ItemStatusIndex = data.ItemStatus_Index;
                    item.ItemStatusId = data.ItemStatus_Id;
                    item.ItemStatusName = data.ItemStatus_Name;
                    item.Qty = data.Qty;
                    item.Ratio = data.Ratio;
                    item.TotalQty = data.TotalQty;
                    item.ProductConversionIndex = data.ProductConversion_Index;
                    item.ProductConversionId = data.ProductConversion_Id;
                    item.ProductConversionName = data.ProductConversion_Name;
                    //if (data.EXP_Date != null || data.MFG_Date != null)
                    //{
                    //    item.EXPDate = data.EXP_Date;
                    //    item.MFGDate = data.MFG_Date;
                    //}
                    //else
                    //{
                    //    item.EXPDate = data.EXP_Date;
                    //    item.MFGDate = data.MFG_Date;
                    //}
                    item.EXPDate = data.EXP_Date;
                    item.MFGDate = data.MFG_Date;
                    item.Weight = data.Weight;
                    item.UnitWeight = data.UnitWeight;
                    item.UnitWidth = data.UnitWidth;
                    item.UnitLength = data.UnitLength;
                    item.UnitHeight = data.UnitHeight;
                    item.UnitVolume = data.UnitVolume;
                    item.Volume = data.Volume;
                    item.UnitPrice = data.UnitPrice;
                    item.Price = data.Price;
                    item.DocumentRefNo1 = data.DocumentRef_No1;
                    item.DocumentRefNo2 = data.DocumentRef_No2;
                    item.DocumentRefNo3 = data.DocumentRef_No3;
                    item.DocumentRefNo4 = data.DocumentRef_No4;
                    item.DocumentRefNo5 = data.DocumentRef_No5;
                    item.DocumentStatus = data.Document_Status;
                    item.DocumentRemark = data.Document_Remark;
                    item.UDF1 = data.UDF_1;
                    item.UDF2 = data.UDF_2;
                    item.UDF3 = data.UDF_3;
                    item.UDF4 = data.UDF_4;
                    item.UDF5 = data.UDF_5;

                    result.Add(item);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PlanGoodsReceiveItemViewModel> getPlanGoodsReceiveItem(PlanGoodsReceiveItemViewModel data)
        {
            try
            {
                var query = db.IM_PlanGoodsReceiveItem.Where(c => c.PlanGoodsReceive_Index == data.planGoodsReceive_Index 
                                                               && c.PlanGoodsReceiveItem_Index == data.planGoodsReceiveItem_Index)
                                                               .ToList();

                



                var items = new List<PlanGoodsReceiveItemViewModel>();

                //var result = query.Select(c => new { c.TotalQty}).ToList();


                foreach (var item in query)
                {
                    var resultItem = new PlanGoodsReceiveItemViewModel
                    {
                        planGoodsReceiveItem_Index = item.PlanGoodsReceiveItem_Index,
                        planGoodsReceive_Index = item.PlanGoodsReceive_Index,
                        product_Index = item.Product_Index,
                        productConversion_Index = item.ProductConversion_Index,
                        itemStatus_Index = item.ItemStatus_Index,
                        qty = item.Qty,
                        ratio = item.Ratio,
                        totalQty = item.TotalQty
                    };
                    items.Add(resultItem);

                }



                return items;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region im_PlanGoodsReceiveItem
        public List<PlanGoodsReceiveItemViewModel> im_PlanGoodsReceiveItem(DocumentViewModel model)
        {
            try
            {
                var query = db.IM_PlanGoodsReceiveItem.AsQueryable();


                if (model.listDocumentViewModel.FirstOrDefault().document_Index != null)
                {
                    query = query.Where(c => model.listDocumentViewModel.Select(s => s.document_Index).Contains(c.PlanGoodsReceive_Index));
                }

                else if (model.listDocumentViewModel.FirstOrDefault().documentItem_Index != null)
                {
                    query = query.Where(c => model.listDocumentViewModel.Select(s => s.documentItem_Index).Contains(c.PlanGoodsReceiveItem_Index));
                }

                else if (model.listDocumentViewModel.FirstOrDefault().document_Status != null)
                {
                    query = query.Where(c => model.listDocumentViewModel.Select(s => s.document_Status).Contains(c.Document_Status));
                }




                var queryresult = query.ToList();

                var result = new List<PlanGoodsReceiveItemViewModel>();

                foreach (var item in queryresult)
                {
                    var resultItem = new PlanGoodsReceiveItemViewModel();
                    resultItem.planGoodsReceiveItem_Index = item.PlanGoodsReceiveItem_Index;
                    resultItem.planGoodsReceive_Index = item.PlanGoodsReceive_Index;
                    resultItem.lineNum = item.LineNum;
                    resultItem.product_Index = item.Product_Index;
                    resultItem.product_Id = item.Product_Id;
                    resultItem.product_Name = item.Product_Name;
                    resultItem.product_SecondName = item.Product_SecondName;
                    resultItem.product_ThirdName = item.Product_ThirdName;
                    resultItem.product_Lot = item.Product_Lot;
                    resultItem.itemStatus_Index = item.ItemStatus_Index;
                    resultItem.itemStatus_Id = item.ItemStatus_Id;
                    resultItem.itemStatus_Name = item.ItemStatus_Name;
                    resultItem.qty = item.Qty;
                    resultItem.ratio = item.Ratio;
                    resultItem.totalQty = item.TotalQty;
                    resultItem.productConversion_Index = item.ProductConversion_Index;
                    resultItem.productConversion_Id = item.ProductConversion_Id;
                    resultItem.productConversion_Name = item.ProductConversion_Name;
                    resultItem.mFG_Date = item.MFG_Date;
                    resultItem.eXP_Date = item.EXP_Date;
                    resultItem.unitWeight = item.UnitWeight;
                    resultItem.weight = item.Weight;
                    resultItem.unitWidth = item.UnitWidth;
                    resultItem.unitLength = item.UnitLength;
                    resultItem.unitHeight = item.UnitHeight;
                    resultItem.unitVolume = item.UnitVolume;
                    resultItem.volume = item.Volume;
                    resultItem.unitPrice = item.UnitPrice;
                    resultItem.price = item.Price;
                    resultItem.documentRef_No1 = item.DocumentRef_No1;
                    resultItem.documentRef_No2 = item.DocumentRef_No2;
                    resultItem.documentRef_No3 = item.DocumentRef_No3;
                    resultItem.documentRef_No4 = item.DocumentRef_No4;
                    resultItem.documentRef_No5 = item.DocumentRef_No5;
                    resultItem.document_Status = item.Document_Status;
                    resultItem.documentItem_Remark = item.DocumentItem_Remark;
                    resultItem.uDF_1 = item.UDF_1;
                    resultItem.uDF_2 = item.UDF_2;
                    resultItem.uDF_3 = item.UDF_3;
                    resultItem.uDF_4 = item.UDF_4;
                    resultItem.uDF_5 = item.UDF_5;
                    resultItem.create_Date = item.Create_Date;
                    resultItem.create_By = item.Create_By;
                    resultItem.update_Date = item.Update_Date;
                    resultItem.update_By = item.Update_By;
                    resultItem.cancel_Date = item.Cancel_Date;
                    resultItem.cancel_By = item.Cancel_By;


                    result.Add(resultItem);
                }


                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        public Boolean Delete(PlanGoodsReceiveDocViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {
                var PlanGoodsReceiveItem = db.IM_PlanGoodsReceiveItem.Where(c => c.PlanGoodsReceive_Index == data.planGoodsReceive_Index).ToList();

                if (PlanGoodsReceiveItem.Count() > 0)
                {
                    foreach (var p in PlanGoodsReceiveItem)
                    {
                        p.Document_Status = -1;
                    }

                    var transaction = db.Database.BeginTransaction();
                    try
                    {
                        db.SaveChanges();
                        transaction.Commit();
                        return true;
                    }

                    catch (Exception exy)
                    {
                        msglog = State + " ex Rollback " + exy.Message.ToString();
                        olog.logging("DeletePlanGR", msglog);
                        transaction.Rollback();
                        throw exy;
                    }
                }


                return false;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
