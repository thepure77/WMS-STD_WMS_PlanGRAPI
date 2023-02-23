using AspNetCore.Reporting;
using Business.Library;
using Common.Utils;
//using Comone.Utils;
using DataAccess;
using DataBusiness.AutoNumber;
using GRBusiness.GoodsReceive;
using GRBusiness.PlanGoodsReceive;
using GRDataAccess.Models;
using MasterDataBusiness.CargoType;
using MasterDataBusiness.ContainerType;
using MasterDataBusiness.CostCenter;
using MasterDataBusiness.Currency;
using MasterDataBusiness.DockDoor;
using MasterDataBusiness.DocumentPriority;
using MasterDataBusiness.ShipmentType;
using MasterDataBusiness.VehicleType;
using MasterDataBusiness.ViewModels;
using MasterDataBusiness.Volume;
using MasterDataBusiness.Weight;
using Microsoft.EntityFrameworkCore;
//using Microsoft.VisualBasic.CompilerServices;
using PlanGRBusiness;
using PlanGRBusiness.ModelConfig;
using PlanGRBusiness.PlanGoodsReceive;
using PlanGRBusiness.Reports;
using PlanGRDataAccess.Models;
using POBusiness.PopupPurchaseOrderBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using static GRBusiness.PlanGoodsReceive.ReturnReceiveViewModel;
using static GRBusiness.PlanGoodsReceive.SearchDetailModel;
using static PlanGRBusiness.PlanGoodsReceive.PlanGoodsReceiveDocViewModel;
using Utils = PlanGRBusiness.Libs.Utils;

namespace GRBusiness
{
    public class PlanGoodsReceiveService
    {
        private DbContextOptions<PlanGRDbContext> options;

        private PlanGRDbContext db;

        public PlanGoodsReceiveService(PlanGRDbContext db)
        {
            this.db = db;
        }

        public PlanGoodsReceiveService()
        {

        }

        #region CreateDataTable
        public static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));

            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        #endregion


        #region filter
        public actionResultPlanGRViewModel filter(SearchDetailModel model)
        {
            try
            {
                var query = db.IM_PlanGoodsReceive.AsQueryable();


                #region advanceSearch
                if (model.advanceSearch == true)
                {
                    if (!string.IsNullOrEmpty(model.planGoodsReceive_No))
                    {
                        query = query.Where(c => c.PlanGoodsReceive_No.Contains(model.planGoodsReceive_No));
                    }

                    if (!string.IsNullOrEmpty(model.owner_Name))
                    {
                        query = query.Where(c => c.Owner_Name.Contains(model.owner_Name));
                    }

                    if (!string.IsNullOrEmpty(model.vendor_Name))
                    {
                        query = query.Where(c => c.Vendor_Name.Contains(model.vendor_Name));
                    }

                    //if (!string.IsNullOrEmpty(model.warehouse_Name))
                    //{
                    //    query = query.Where(c => c.Warehouse_Name.Contains(model.warehouse_Name));
                    //}

                    //if (!string.IsNullOrEmpty(model.warehouse_Name_To))
                    //{
                    //    query = query.Where(c => c.Warehouse_Name_To.Contains(model.warehouse_Name_To));
                    //}

                    if (!string.IsNullOrEmpty(model.document_Status.ToString()))
                    {
                        query = query.Where(c => c.Document_Status == (model.document_Status));
                    }

                    //if (!string.IsNullOrEmpty(model.processStatus_Name))
                    //{
                    //    query = query.Where(c => c.ProcessStatus_Name.Contains(model.processStatus_Name));
                    //}

                    if ((model.processStatus_Id ?? -99) != -99)
                    {
                        query = query.Where(c => c.Document_Status == model.processStatus_Id);
                    }

                    if (!string.IsNullOrEmpty(model.documentType_Index.ToString()) && model.documentType_Index.ToString() != "00000000-0000-0000-0000-000000000000")
                    {
                        query = query.Where(c => c.DocumentType_Index == (model.documentType_Index));
                    }

                    if (!string.IsNullOrEmpty(model.planGoodsReceive_date) && !string.IsNullOrEmpty(model.planGoodsReceive_date_To))
                    {
                        var dateStart = model.planGoodsReceive_date.toBetweenDate();
                        var dateEnd = model.planGoodsReceive_date_To.toBetweenDate();
                        query = query.Where(c => c.PlanGoodsReceive_Date >= dateStart.start && c.PlanGoodsReceive_Date <= dateEnd.end);
                    }
                    else if (!string.IsNullOrEmpty(model.planGoodsReceive_date))
                    {
                        var planGoodsReceive_date_From = model.planGoodsReceive_date.toBetweenDate();
                        query = query.Where(c => c.PlanGoodsReceive_Date >= planGoodsReceive_date_From.start);
                    }
                    else if (!string.IsNullOrEmpty(model.planGoodsReceive_date_To))
                    {
                        var planGoodsReceive_date_To = model.planGoodsReceive_date_To.toBetweenDate();
                        query = query.Where(c => c.PlanGoodsReceive_Date <= planGoodsReceive_date_To.start);
                    }

                    if (!string.IsNullOrEmpty(model.planGoodsReceive_due_date) && !string.IsNullOrEmpty(model.planGoodsReceive_due_date_To))
                    {
                        var dateStart = model.planGoodsReceive_due_date.toBetweenDate();
                        var dateEnd = model.planGoodsReceive_due_date_To.toBetweenDate();
                        query = query.Where(c => c.PlanGoodsReceive_Due_Date >= dateStart.start && c.PlanGoodsReceive_Date <= dateEnd.end);
                    }
                    else if (!string.IsNullOrEmpty(model.planGoodsReceive_due_date))
                    {
                        var planGoodsReceive_due_date_From = model.planGoodsReceive_date.toBetweenDate();
                        query = query.Where(c => c.PlanGoodsReceive_Due_Date >= planGoodsReceive_due_date_From.start);
                    }
                    else if (!string.IsNullOrEmpty(model.planGoodsReceive_due_date_To))
                    {
                        var planGoodsReceive_due_date_To = model.planGoodsReceive_date_To.toBetweenDate();
                        query = query.Where(c => c.PlanGoodsReceive_Due_Date <= planGoodsReceive_due_date_To.start);
                    }
                    if (!string.IsNullOrEmpty(model.create_By))
                    {
                        query = query.Where(c => c.Create_By == (model.create_By));
                    }
                    if (!string.IsNullOrEmpty(model.documentRef_No1))
                    {
                        query = query.Where(c => c.DocumentRef_No1 == model.documentRef_No1);
                    }
                    if (!string.IsNullOrEmpty(model.matdoc))
                    {
                        query = query.Where(c => c.Matdoc == model.matdoc);
                    }
                }
                
                #endregion

                #region Basic
                else
                {
                    if (!string.IsNullOrEmpty(model.po_no))
                    {
                        var findPGRItem = db.IM_PlanGoodsReceiveItem.Where(c => c.Ref_Document_No == model.po_no && c.Document_Status != -1).GroupBy(c => c.PlanGoodsReceive_Index).Select(c => c.Key).ToList();
                        query = query.Where(c => findPGRItem.Contains(c.PlanGoodsReceive_Index));
                    }
                    if (!string.IsNullOrEmpty(model.key))
                    {
                        query = query.Where(c => c.PlanGoodsReceive_No.Contains(model.key)
                                            //|| c.Owner_Name.Contains(model.key)
                                            //|| c.Create_By.Contains(model.key)
                                            //|| c.DocumentRef_No1.Contains(model.key)
                                            //|| c.DocumentType_Name.Contains(model.key)
                                            );
                    }

                    if (!string.IsNullOrEmpty(model.owner_Name))
                    {
                        query = query.Where(c => c.Owner_Name.Contains(model.owner_Name)
                                            //|| c.Owner_Name.Contains(model.key)
                                            //|| c.Create_By.Contains(model.key)
                                            //|| c.DocumentRef_No1.Contains(model.key)
                                            //|| c.DocumentType_Name.Contains(model.key)
                                            );
                    }

                    if (!string.IsNullOrEmpty(model.planGoodsReceive_date) && !string.IsNullOrEmpty(model.planGoodsReceive_date_To))
                    {
                        var dateStart = model.planGoodsReceive_date.toBetweenDate();
                        var dateEnd = model.planGoodsReceive_date_To.toBetweenDate();
                        query = query.Where(c => c.PlanGoodsReceive_Date >= dateStart.start && c.PlanGoodsReceive_Date <= dateEnd.end);
                    }
                    else if (!string.IsNullOrEmpty(model.planGoodsReceive_date))
                    {
                        var planGoodsReceive_date_From = model.planGoodsReceive_date.toBetweenDate();
                        query = query.Where(c => c.PlanGoodsReceive_Date >= planGoodsReceive_date_From.start);
                    }
                    else if (!string.IsNullOrEmpty(model.planGoodsReceive_date_To))
                    {
                        var planGoodsReceive_date_To = model.planGoodsReceive_date_To.toBetweenDate();
                        query = query.Where(c => c.PlanGoodsReceive_Date <= planGoodsReceive_date_To.start);
                    }

                    var statusModels = new List<int?>();
                    var sortModels = new List<SortModel>();

                    if (model.status.Count > 0)
                    {
                        foreach (var item in model.status)
                        {

                            if (item.value == 0)
                            {
                                statusModels.Add(0);
                            }
                            if (item.value == 1)
                            {
                                statusModels.Add(1);
                            }
                            if (item.value == 2)
                            {
                                statusModels.Add(2);
                            }
                            if (item.value == 3)
                            {
                                statusModels.Add(3);
                            }
                            if (item.value == 4)
                            {
                                statusModels.Add(4);
                            }
                            if (item.value == -1)
                            {
                                statusModels.Add(-1);
                            }
                            if (item.value == -2)
                            {
                                statusModels.Add(-2);
                            }
                        }

                        query = query.Where(c => statusModels.Contains(c.Document_Status));
                    }

                    if (model.sort.Count > 0)
                    {
                        foreach (var item in model.sort)
                        {

                            if (item.value == "PlanGoodsReceive_No")
                            {
                                sortModels.Add(new SortModel
                                {
                                    ColId = "PlanGoodsReceive_No",
                                    Sort = "desc"
                                });
                            }
                            if (item.value == "PlanGoodsReceive_Date")
                            {
                                sortModels.Add(new SortModel
                                {
                                    ColId = "PlanGoodsReceive_Date",
                                    Sort = "desc"
                                });
                            }
                            if (item.value == "DocumentType_Name")
                            {
                                sortModels.Add(new SortModel
                                {
                                    ColId = "DocumentType_Name",
                                    Sort = "desc"
                                });
                            }
                            if (item.value == "Qty")
                            {
                                sortModels.Add(new SortModel
                                {
                                    ColId = "Qty",
                                    Sort = "desc"
                                });
                            }
                            if (item.value == "Weight")
                            {
                                sortModels.Add(new SortModel
                                {
                                    ColId = "Weight",
                                    Sort = "desc"
                                });
                            }
                            if (item.value == "ProcessStatus_Name")
                            {
                                sortModels.Add(new SortModel
                                {
                                    ColId = "Document_Status",
                                    Sort = "desc"
                                });
                            }
                            if (item.value == "Vendor_name")
                            {
                                sortModels.Add(new SortModel
                                {
                                    ColId = "Vendor_name",
                                    Sort = "desc"
                                });

                            }
                        }
                        query = query.KWOrderBy(sortModels);

                    }

                }

                #endregion

                var Item = new List<IM_PlanGoodsReceive>();
                var TotalRow = new List<IM_PlanGoodsReceive>();


                TotalRow = query.OrderByDescending(o => o.Create_Date).ThenByDescending(o => o.Create_Date).ToList();


                if (model.CurrentPage != 0 && model.PerPage != 0)
                {
                    query = query.Skip(((model.CurrentPage - 1) * model.PerPage));
                }

                if (model.PerPage != 0)
                {
                    query = query.Take(model.PerPage);

                }

                if (model.sort.Count > 0)
                {
                    Item = query.ToList();
                }
                else
                {
                    Item = query.OrderByDescending(c => c.Create_Date).ToList();
                }

                //Item = query.ToList();

                var ProcessStatus = new List<ProcessStatusViewModel>();

                var filterModel = new ProcessStatusViewModel();

                filterModel.process_Index = new Guid("C2A3F847-BAA6-46FE-B502-44F2D5826A1C");

                //GetConfig
                ProcessStatus = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("processStatus"), filterModel.sJson());

                String Statue = "";
                var result = new List<SearchDetailModel>();

                foreach (var item in Item)
                {
                    var resultItem = new SearchDetailModel();

                    var findPGRItem = db.IM_PlanGoodsReceiveItem.Where(c =>  c.PlanGoodsReceive_Index == item.PlanGoodsReceive_Index && c.Document_Status != -1).ToList();
           

                    if (findPGRItem.Count > 0)
                    {
                        var findPO = findPGRItem.Select(s => s.Ref_Document_No).Distinct().ToList();

                        resultItem.documentRef_No1 = string.Join(",", findPO.Take(3)) + (findPO.Count > 3 ? "..." : "");
                    }

                    resultItem.planGoodsReceive_Index = item.PlanGoodsReceive_Index;
                    resultItem.planGoodsReceive_No = item.PlanGoodsReceive_No;
                    resultItem.planGoodsReceive_date = item.PlanGoodsReceive_Date.toString();
                    resultItem.planGoodsReceive_due_date = item.PlanGoodsReceive_Due_Date.toString();
                    resultItem.documentType_Index = item.DocumentType_Index;
                    resultItem.documentType_Id = item.DocumentType_Id;
                    resultItem.documentType_Name = item.DocumentType_Name;
                    resultItem.document_Status = item.Document_Status;

                    resultItem.vendor_Index = item.Vendor_Index;
                    resultItem.vendor_Id = item.Vendor_Id;
                    resultItem.vendor_Name = item.Vendor_Name;

                    resultItem.owner_Index = item.Owner_Index;
                    resultItem.owner_Id = item.Owner_Id;
                    resultItem.owner_Name = item.Owner_Name;

                    //resultItem.documentRef_No1 = item.DocumentRef_No1;

                    Statue = item.Document_Status.ToString();
                    var ProcessStatusName = ProcessStatus.Where(c => c.processStatus_Id == Statue).FirstOrDefault();
                    resultItem.processStatus_Name = ProcessStatusName.processStatus_Name;

                    //resultItem.qty = item.Qty;
                    resultItem.create_By = item.Create_By;
                    //resultItem.our_Reference = item.Our_Reference;
                    resultItem.update_By = item.Update_By;
                    resultItem.cancel_By = item.Cancel_By;
                    resultItem.status_SAP = item.Status_SAP;
                    resultItem.matdoc = item.Matdoc;
                    resultItem.message = item.Message;
                    result.Add(resultItem);
                }
                var count = TotalRow.Count;

                var actionResultPlanGR = new actionResultPlanGRViewModel();
                actionResultPlanGR.itemsPlanGR = result.OrderByDescending(o => o.create_date).ToList();
                actionResultPlanGR.pagination = new Pagination() { TotalRow = count, CurrentPage = model.CurrentPage, PerPage = model.PerPage, };

                return actionResultPlanGR;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region filter_Exprot
        public actionResultPlanGRViewModel filter_Exprot(SearchDetailModel model)
        {
            try
            {
                var query = db.View_RPT_ASN.AsQueryable();
            
                    if (!string.IsNullOrEmpty(model.key))
                    {
                        query = query.Where(c => c.PlanGoodsReceive_No.Contains(model.key));
                    }

                    if (!string.IsNullOrEmpty(model.owner_Name))
                    {
                        query = query.Where(c => c.Owner_Name.Contains(model.owner_Name));
                    }

                    if (!string.IsNullOrEmpty(model.planGoodsReceive_date) && !string.IsNullOrEmpty(model.planGoodsReceive_date_To))
                    {
                        var dateStart = model.planGoodsReceive_date.toBetweenDate();
                        var dateEnd = model.planGoodsReceive_date_To.toBetweenDate();
                        query = query.Where(c => c.PlanGoodsReceive_Date >= dateStart.start && c.PlanGoodsReceive_Date <= dateEnd.end);
                    }
                    else if (!string.IsNullOrEmpty(model.planGoodsReceive_date))
                    {
                        var planGoodsReceive_date_From = model.planGoodsReceive_date.toBetweenDate();
                        query = query.Where(c => c.PlanGoodsReceive_Date >= planGoodsReceive_date_From.start);
                    }
                    else if (!string.IsNullOrEmpty(model.planGoodsReceive_date_To))
                    {
                        var planGoodsReceive_date_To = model.planGoodsReceive_date_To.toBetweenDate();
                        query = query.Where(c => c.PlanGoodsReceive_Date <= planGoodsReceive_date_To.start);
                    }

                    var statusModels = new List<int?>();
                    var sortModels = new List<SortModel>();

                    if (model.status.Count > 0)
                    {
                        foreach (var item in model.status)
                        {

                            if (item.value == 0)
                            {
                                statusModels.Add(0);
                            }
                            if (item.value == 1)
                            {
                                statusModels.Add(1);
                            }
                            if (item.value == 2)
                            {
                                statusModels.Add(2);
                            }
                            if (item.value == 3)
                            {
                                statusModels.Add(3);
                            }
                            if (item.value == 4)
                            {
                                statusModels.Add(4);
                            }
                            if (item.value == -1)
                            {
                                statusModels.Add(-1);
                            }
                            if (item.value == -2)
                            {
                                statusModels.Add(-2);
                            }
                        }

                        query = query.Where(c => statusModels.Contains(c.Document_Status));
                    }

                    
                var result = new List<SearchDetailModel>();

                foreach (var item in query)
                {
                    var resultItem = new SearchDetailModel();

                    resultItem.row_Index = item.Row_Index;
                    resultItem.planGoodsReceive_Index = item.PlanGoodsReceive_Index.GetValueOrDefault();
                    resultItem.planGoodsReceive_date = item.PlanGoodsReceive_Date.GetValueOrDefault().ToString(/*"yyyy-mm-dd HH:mm:ss"*/);
                    resultItem.planGoodsReceive_No = item.PlanGoodsReceive_No;
                    resultItem.owner_Id = item.Owner_Id;
                    resultItem.owner_Name = item.Owner_Name;
                    resultItem.vendor_Id = item.Vendor_Id;
                    resultItem.vendor_Name = item.Vendor_Name;
                    resultItem.documentType_Name = item.DocumentType_Name;
                    resultItem.product_Id = item.Product_Id;
                    resultItem.product_Name = item.Product_Name;
                    resultItem.qty = item.Qty.GetValueOrDefault();
                    resultItem.productConversion_Name = item.ProductConversion_Name;
                    resultItem.document_Status = item.Document_Status;
                    
                    result.Add(resultItem);
                }

                var actionResultPlanGR = new actionResultPlanGRViewModel();
                actionResultPlanGR.itemsPlanGR = result.OrderBy(o => o.row_Index).ToList();

                return actionResultPlanGR;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public actionResultPlanGRViewModel FilterInClause(SearchPlanGoodsReceiveInClauseViewModel model)
        {
            try
            {
                var query = db.IM_PlanGoodsReceive.AsQueryable();

                if ((model?.List_PlanGoodsReceive_Index?.Count ?? 0) > 0)
                {
                    query = query.Where(w => model.List_PlanGoodsReceive_Index.Contains(w.PlanGoodsReceive_Index));
                }

                if ((model?.List_PlanGoodsReceive_No?.Count ?? 0) > 0)
                {
                    query = query.Where(w => model.List_PlanGoodsReceive_No.Contains(w.PlanGoodsReceive_No));
                }

                if ((model?.List_DocumentRef_No1?.Count ?? 0) > 0)
                {
                    query = query.Where(w => model.List_DocumentRef_No1.Contains(w.DocumentRef_No1));
                }

                var Item = new List<IM_PlanGoodsReceive>();
                var TotalRow = new List<IM_PlanGoodsReceive>();


                TotalRow = query.ToList();


                if (model.CurrentPage != 0 && model.PerPage != 0)
                {
                    query = query.Skip(((model.CurrentPage - 1) * model.PerPage));
                }

                if (model.PerPage != 0)
                {
                    query = query.Take(model.PerPage);

                }

                Item = query.ToList();

                var ProcessStatus = new List<ProcessStatusViewModel>();

                var filterModel = new ProcessStatusViewModel()
                {
                    process_Index = new Guid("C2A3F847-BAA6-46FE-B502-44F2D5826A1C")
                };


                //GetConfig
                ProcessStatus = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("processStatus"), filterModel.sJson());


                string Statue = "";
                var result = new List<SearchDetailModel>();

                foreach (var item in Item)
                {
                    var resultItem = new SearchDetailModel();
                    resultItem.planGoodsReceive_Index = item.PlanGoodsReceive_Index;
                    resultItem.planGoodsReceive_No = item.PlanGoodsReceive_No;
                    resultItem.planGoodsReceive_date = item.PlanGoodsReceive_Date.toString();
                    resultItem.planGoodsReceive_due_date = item.PlanGoodsReceive_Due_Date.toString();
                    resultItem.documentType_Index = item.DocumentType_Index;
                    resultItem.documentType_Id = item.DocumentType_Id;
                    resultItem.documentType_Name = item.DocumentType_Name;
                    resultItem.document_Status = item.Document_Status;
                    resultItem.documentRef_No1 = item.DocumentRef_No1;
                    resultItem.documentRef_No2 = item.DocumentRef_No2;
                    resultItem.documentRef_No3 = item.DocumentRef_No3;
                    resultItem.documentRef_No4 = item.DocumentRef_No4;
                    resultItem.documentRef_No5 = item.DocumentRef_No5;
                    resultItem.vendor_Index = item.Vendor_Index;
                    resultItem.vendor_Id = item.Vendor_Id;
                    resultItem.vendor_Name = item.Vendor_Name;

                    Statue = item.Document_Status.ToString();
                    var ProcessStatusName = ProcessStatus.Where(c => c.processStatus_Id == Statue).FirstOrDefault();
                    resultItem.processStatus_Name = ProcessStatusName.processStatus_Name;

                    //resultItem.qty = item.Qty;
                    resultItem.create_By = item.Create_By;
                    //resultItem.our_Reference = item.Our_Reference;
                    resultItem.update_By = item.Update_By;
                    resultItem.cancel_By = item.Cancel_By;
                    result.Add(resultItem);
                }
                var count = TotalRow.Count;

                var actionResultPlanGR = new actionResultPlanGRViewModel();
                actionResultPlanGR.itemsPlanGR = result.OrderByDescending(o => o.create_date).ToList();
                actionResultPlanGR.pagination = new Pagination() { TotalRow = count, CurrentPage = model.CurrentPage, PerPage = model.PerPage, };

                return actionResultPlanGR;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region CreateOrUpdate
        public actionResult CreateOrUpdate(PlanGoodsReceiveDocViewModel data)
        {
            Guid PlanGoodsReceiveIndex = new Guid();
            String PlanGoodsReceiveNo = "";

            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            Boolean IsNew = false;

            String userName = "";

            var actionResult = new actionResult();
            try
            {
                var itemDetail = new List<IM_PlanGoodsReceiveItem>();
                var _purchaseOrderNo = "";
                var _purchaseOrderRefIndex = Guid.Empty;

                var PlanGoodsReceiveOld = db.IM_PlanGoodsReceive.Find(data.planGoodsReceive_Index);

                if (data.create_By != null)
                {
                    userName = data.create_By;
                }
                if (data.update_By != null)
                {
                    userName = data.update_By;
                }

                if (PlanGoodsReceiveOld == null)
                {
                    IsNew = true;
                    PlanGoodsReceiveIndex = Guid.NewGuid();
                    var _guidPlanGoodsReceiveRef = Guid.NewGuid();

                    var result = new List<GenDocumentTypeViewModel>();

                    var filterModel = new GenDocumentTypeViewModel();


                    filterModel.process_Index = new Guid("C2A3F847-BAA6-46FE-B502-44F2D5826A1C");
                    filterModel.documentType_Index = data.documentType_Index;
                    //GetConfig
                    result = utils.SendDataApi<List<GenDocumentTypeViewModel>>(new AppSettingConfig().GetUrl("dropDownDocumentType"), filterModel.sJson());

                    var genDoc = new AutoNumberService(db);
                    string DocNo = "";

                    DateTime DocumentDate = (DateTime)data.planGoodsReceive_Date.toDate();
                    DocNo = genDoc.genAutoDocmentNumber(result, DocumentDate);

                    IM_PlanGoodsReceive itemHeader = new IM_PlanGoodsReceive();
                    var document_status = 0;

                    PlanGoodsReceiveNo = DocNo;
                    itemHeader.PlanGoodsReceive_Index = PlanGoodsReceiveIndex;
                    itemHeader.PlanGoodsReceive_No = DocNo;
                    itemHeader.Owner_Index = data.owner_Index;
                    itemHeader.Owner_Id = data.owner_Id;
                    itemHeader.Owner_Name = data.owner_Name;
                    itemHeader.Vendor_Index = data.vendor_Index;
                    itemHeader.Vendor_Id = data.vendor_Id;
                    itemHeader.Vendor_Name = data.vendor_Name;
                    itemHeader.DocumentType_Index = data.documentType_Index;
                    itemHeader.DocumentType_Id = data.documentType_Id;
                    itemHeader.DocumentType_Name = data.documentType_Name;
                    itemHeader.PlanGoodsReceive_Date = data.planGoodsReceive_Date.toDate();
                    itemHeader.PlanGoodsReceive_Time = data.planGoodsReceive_Time;
                    itemHeader.PlanGoodsReceive_Due_Date = data.planGoodsReceive_Due_Date.toDate();
                    itemHeader.PlanGoodsReceive_Due_DateTime = data.planGoodsReceive_Due_DateTime;
                    itemHeader.DocumentRef_No1 = data.documentRef_No1;
                    itemHeader.DocumentRef_No2 = data.documentRef_No2;
                    itemHeader.DocumentRef_No3 = data.documentRef_No3;
                    itemHeader.DocumentRef_No4 = data.documentRef_No4;
                    itemHeader.DocumentRef_No5 = data.documentRef_No5;
                    itemHeader.Document_Status = document_status;
                    itemHeader.UDF_1 = data.uDF_1;
                    itemHeader.UDF_2 = data.uDF_2;
                    itemHeader.UDF_3 = data.uDF_3;
                    itemHeader.UDF_4 = data.uDF_4;
                    itemHeader.UDF_5 = data.uDF_5;
                    itemHeader.DocumentPriority_Status = data.documentpriority_Status;
                    itemHeader.Document_Remark = data.document_Remark;
                    itemHeader.Warehouse_Index = data.warehouse_Index;
                    itemHeader.Warehouse_Id = data.warehouse_Id;
                    itemHeader.Warehouse_Name = data.warehouse_Name;
                    itemHeader.Warehouse_Index_To = data.warehouse_Index;
                    itemHeader.Warehouse_Id_To = data.warehouse_Id;
                    itemHeader.Warehouse_Name_To = data.warehouse_Name;

                    itemHeader.Dock_Index = data.dock_Index;
                    itemHeader.Dock_Id = data.dock_Id;
                    itemHeader.Dock_Name = data.dock_Name;
                    itemHeader.VehicleType_Index = data.vehicleType_Index;
                    itemHeader.VehicleType_Id = data.vehicleType_Id;
                    itemHeader.VehicleType_Name = data.vehicleType_Name;
                    itemHeader.Transport_Index = data.transport_Index;
                    itemHeader.Transport_Id = data.transport_Id;
                    itemHeader.Transport_Name = data.transport_Name;
                    itemHeader.Driver_Name = data.driver_Name;
                    itemHeader.Round_Index = data.round_Index;
                    itemHeader.Round_Id = data.round_Id;
                    itemHeader.Round_Name = data.round_Name;
                    itemHeader.License_Name = data.license_Name;
                    itemHeader.Forwarder_Index = data.forwarder_Index;
                    itemHeader.Forwarder_Id = data.forwarder_Id;
                    itemHeader.Forwarder_Name = data.forwarder_Name;
                    itemHeader.ShipmentType_Index = data.shipmentType_Index;
                    itemHeader.ShipmentType_Id = data.shipmentType_Id;
                    itemHeader.ShipmentType_Name = data.shipmentType_Name;
                    itemHeader.CargoType_Index = data.cargoType_Index;
                    itemHeader.CargoType_Id = data.cargoType_Id;
                    itemHeader.CargoType_Name = data.cargoType_Name;
                    itemHeader.UnloadingType_Index = data.unloadingType_Index;
                    itemHeader.UnloadingType_Id = data.unloadingType_Id;
                    itemHeader.UnloadingType_Name = data.unloadingType_Name;
                    itemHeader.ContainerType_Index = data.containerType_Index;
                    itemHeader.ContainerType_Id = data.containerType_Id;
                    itemHeader.ContainerType_Name = data.containerType_Name;
                    itemHeader.Container_No1 = data.container_No1;
                    itemHeader.Container_No2 = data.container_No2;
                    itemHeader.Labur = data.labur;
                    itemHeader.Create_By = userName;
                    itemHeader.Create_Date = DateTime.Now;
                    itemHeader.Import_Index = data.import_Index;
                    itemHeader.CostCenter_Index = data.costCenter_Index;
                    itemHeader.CostCenter_Id = data.costCenter_Id;
                    itemHeader.CostCenter_Name = data.costCenter_Name;
                    itemHeader.DocumentRef_No5 = data.Billing_No;

                    db.IM_PlanGoodsReceive.Add(itemHeader);

                    //if(data.listPlanGoodsReceiveItemViewModel != null)
                    //{
                    //    _purchaseOrderNo = data.listPlanGoodsReceiveItemViewModel[0].ref_Document_No;
                    //}

                    //if (_purchaseOrderNo != "")
                    //{
                    //    var resPORef = db.im_PurchaseOrder_Ref.Where(c => c.PurchaseOrder_No == _purchaseOrderNo).FirstOrDefault();
                    //    var resPORef = db.im_PurchaseOrder_Ref.Where(c => c.PurchaseOrder_Ref_Index == resultItem.Ref_DocumentItem_Index).FirstOrDefault()
                        
                    //    if (resPORef != null)
                    //    {
                    //        _purchaseOrderRefIndex = resPORef.PurchaseOrder_Ref_Index;

                    //        im_PlanGoodsReceive_Ref resultPGR_Ref = new im_PlanGoodsReceive_Ref();
                    //        resultPGR_Ref.PlanGoodsReceive_Ref_Index = _guidPlanGoodsReceiveRef;
                    //        resultPGR_Ref.PlanGoodsReceive_Index = PlanGoodsReceiveIndex;
                    //        resultPGR_Ref.PlanGoodsReceive_No = DocNo;
                    //        resultPGR_Ref.SO_No = resPORef.SO_No;
                    //        resultPGR_Ref.SO_Type = resPORef.SO_Type;
                    //        resultPGR_Ref.SHIP_CON = resPORef.SHIP_CON;
                    //        resultPGR_Ref.DO_Type = resPORef.DO_Type;
                    //        resultPGR_Ref.RTN_Flag = resPORef.RTN_Flag;
                    //        resultPGR_Ref.CREDIT_STATUS = resPORef.CREDIT_STATUS;
                    //        resultPGR_Ref.EXPORT_FLAG = resPORef.EXPORT_FLAG;
                    //        resultPGR_Ref.FOC_FLAG = resPORef.FOC_FLAG;
                    //        resultPGR_Ref.CLAIM_FLAG = resPORef.CLAIM_FLAG;
                    //        resultPGR_Ref.Ref_Type = resPORef.Ref_Type;
                    //        resultPGR_Ref.IO = resPORef.IO;
                    //        resultPGR_Ref.TAR_VAL = resPORef.TAR_VAL;
                    //        resultPGR_Ref.YEAR = resPORef.YEAR;
                    //        resultPGR_Ref.ITEM_TEXT = resPORef.ITEM_TEXT;

                    //        db.im_PlanGoodsReceive_Ref.Add(resultPGR_Ref);
                    //    }
                    //}

                    if (data.documents != null)
                    {
                        foreach (var d in data.documents.Where(c => !c.isDelete))
                        {
                            im_DocumentFile documents = new im_DocumentFile();

                            documents.DocumentFile_Index = Guid.NewGuid(); ;
                            documents.DocumentFile_Name = d.filename;
                            documents.DocumentFile_Path = d.path;
                            documents.DocumentFile_Url = d.urlAttachFile;
                            documents.DocumentFile_Type = d.type;
                            documents.DocumentFile_Status = 0;
                            documents.Create_By = userName;
                            documents.Create_Date = DateTime.Now;
                            documents.Ref_Index = itemHeader.PlanGoodsReceive_Index;
                            documents.Ref_No = itemHeader.PlanGoodsReceive_No;
                            db.im_DocumentFile.Add(documents);
                        }
                    }

                    var groupRefIndex = data.listPlanGoodsReceiveItemViewModel.GroupBy(item => item.ref_Document_Index)
                     .Select(group => new { Ref_Document_Index = group.Key })
                     .ToList();
                    
                    if(groupRefIndex.Count > 0)
                    {
                        foreach(var item in groupRefIndex)
                        {
                            var resPORef = db.im_PurchaseOrder_Ref.Where(c => c.PurchaseOrder_Index == item.Ref_Document_Index).FirstOrDefault();
                            if (resPORef != null)
                            {
                                //_purchaseOrderRefIndex = resPORef.PurchaseOrder_Ref_Index;

                                im_PlanGoodsReceive_Ref resultPGR_Ref = new im_PlanGoodsReceive_Ref();
                                resultPGR_Ref.PlanGoodsReceive_Ref_Index = Guid.NewGuid();
                                resultPGR_Ref.PlanGoodsReceive_Index = PlanGoodsReceiveIndex;
                                resultPGR_Ref.PlanGoodsReceive_No = DocNo;
                                resultPGR_Ref.SO_No = resPORef.SO_No;
                                resultPGR_Ref.SO_Type = resPORef.SO_Type;
                                resultPGR_Ref.SHIP_CON = resPORef.SHIP_CON;
                                resultPGR_Ref.DO_Type = resPORef.DO_Type;
                                resultPGR_Ref.RTN_Flag = resPORef.RTN_Flag;
                                resultPGR_Ref.CREDIT_STATUS = resPORef.CREDIT_STATUS;
                                resultPGR_Ref.EXPORT_FLAG = resPORef.EXPORT_FLAG;
                                resultPGR_Ref.FOC_FLAG = resPORef.FOC_FLAG;
                                resultPGR_Ref.CLAIM_FLAG = resPORef.CLAIM_FLAG;
                                resultPGR_Ref.Ref_Type = resPORef.Ref_Type;
                                resultPGR_Ref.IO = resPORef.IO;
                                resultPGR_Ref.TAR_VAL = resPORef.TAR_VAL;
                                resultPGR_Ref.YEAR = resPORef.YEAR;
                                resultPGR_Ref.ITEM_TEXT = resPORef.ITEM_TEXT;
                                resultPGR_Ref.Ref_Document_Index = resPORef.PurchaseOrder_Index;
                                resultPGR_Ref.Ref_Document_No = resPORef.PurchaseOrder_No;

                                db.im_PlanGoodsReceive_Ref.Add(resultPGR_Ref);
                            }
                        }
                    }

                    int addNumber = 0;
                    foreach (var item in data.listPlanGoodsReceiveItemViewModel)
                    {

                        var Productresult = new List<ProductViewModel>();

                        var ProductfilterModel = new ProductViewModel();
                        ProductfilterModel.product_Index = item.product_Index;

                        //GetConfig
                        Productresult = utils.SendDataApi<List<ProductViewModel>>(new AppSettingConfig().GetUrl("product"), ProductfilterModel.sJson());

                        IM_PlanGoodsReceiveItem resultItem = new IM_PlanGoodsReceiveItem();

                        addNumber++;
                        // Gen Index for line item

                        item.planGoodsReceiveItem_Index = Guid.NewGuid();
                        resultItem.PlanGoodsReceive_Index = PlanGoodsReceiveIndex;
                        resultItem.LineNum = item.lineNum != null ? item.lineNum : addNumber.ToString();
                        resultItem.ItemStatus_Index = item.itemStatus_Index;

                        resultItem.ItemStatus_Id = item.itemStatus_Id;

                        resultItem.ItemStatus_Name = item.itemStatus_Name;
                        resultItem.Product_Index = item.product_Index;
                        resultItem.Product_Id = item.product_Id;
                        resultItem.Product_Name = item.product_Name;
                        if (Productresult.Count > 0)
                        {
                            resultItem.Product_SecondName = Productresult.FirstOrDefault().product_SecondName;
                            resultItem.Product_ThirdName = Productresult.FirstOrDefault().product_ThirdName;
                        }

                        if (item.product_Lot != null)
                        {
                            resultItem.Product_Lot = item.product_Lot;
                        }
                        else
                        {
                            resultItem.Product_Lot = "";
                        }

                        resultItem.Qty = item.qty;
                        resultItem.Ratio = item.ratio;
                        if (item.ratio != 0)
                        {
                            var totalqty = item.qty * item.ratio;
                            item.totalQty = totalqty;
                        }
                        resultItem.TotalQty = item.totalQty;
                        resultItem.ProductConversion_Index = item.productConversion_Index;
                        resultItem.ProductConversion_Id = item.productConversion_Id;
                        resultItem.ProductConversion_Name = item.productConversion_Name;
                        resultItem.MFG_Date = item.mFG_Date.toDate();
                        resultItem.EXP_Date = item.eXP_Date.toDate();

                        resultItem.WeightRatio = item.weightRatio;
                        resultItem.UnitWeight = item.unitWeight;
                        resultItem.Weight = item.qty * (item.unitWeight * item.weightRatio);
                        resultItem.Weight_Index = item.weight_Index;
                        resultItem.Weight_Id = item.weight_Id;
                        resultItem.Weight_Name = item.weight_Name;
                        resultItem.NetWeight = item.netWeight;

                        resultItem.GrsWeightRatio = item.grsWeightRatio;
                        resultItem.GrsWeight = item.grsWeight;
                        resultItem.UnitGrsWeight = (item.grsWeight != null ? item.grsWeight : 1 / item.grsWeightRatio != null ? item.grsWeightRatio : 1);
                        resultItem.GrsWeight_Index = item.grsWeight_Index;
                        resultItem.GrsWeight_Id = item.grsWeight_Id;
                        resultItem.GrsWeight_Name = item.grsWeight_Name;

                        resultItem.WidthRatio = item.widthRatio;
                        resultItem.UnitWidth = item.unitWidth;
                        resultItem.Width = item.unitWidth * item.qty;
                        resultItem.Width_Index = item.width_Index;
                        resultItem.Width_Id = item.width_Id;
                        resultItem.Width_Name = item.width_Name;

                        resultItem.LengthRatio = item.lengthRatio;
                        resultItem.UnitLength = item.unitLength;
                        resultItem.Length = item.unitLength * item.qty;
                        resultItem.Length_Index = item.length_Index;
                        resultItem.Length_Id = item.length_Id;
                        resultItem.Length_Name = item.length_Name;

                        resultItem.HeightRatio = item.heightRatio;
                        resultItem.UnitHeight = item.unitHeight;
                        resultItem.Height = item.unitHeight * item.qty;
                        resultItem.Height_Index = item.height_Index;
                        resultItem.Height_Id = item.height_Id;
                        resultItem.Height_Name = item.height_Name;

                        resultItem.UnitVolume = (resultItem.UnitWidth * resultItem.UnitLength * resultItem.UnitHeight) / item.volume_Ratio != null ? item.volume_Ratio : 1;
                        resultItem.Volume = resultItem.Qty * resultItem.UnitVolume;


                        resultItem.UnitPrice = item.unitPrice;
                        resultItem.Price = item.unitPrice * item.qty;
                        resultItem.TotalPrice = resultItem.Price * resultItem.Qty;

                        resultItem.Currency_Index = item.currency_Index;
                        resultItem.Currency_Id = item.currency_Id;
                        resultItem.Currency_Name = item.currency_Name;

                        resultItem.Ref_Code1 = item.ref_Code1;
                        resultItem.Ref_Code2 = item.ref_Code2;
                        resultItem.Ref_Code3 = item.ref_Code3;
                        resultItem.Ref_Code4 = item.ref_Code4;
                        resultItem.Ref_Code5 = item.ref_Code5;


                        resultItem.DocumentRef_No1 = item.documentRef_No1;
                        resultItem.DocumentRef_No2 = item.documentRef_No2;
                        resultItem.DocumentRef_No3 = item.documentRef_No3;
                        resultItem.DocumentRef_No4 = item.documentRef_No4;
                        resultItem.DocumentRef_No5 = item.documentRef_No5;
                        resultItem.Document_Status = 0;
                        resultItem.DocumentItem_Remark = item.documentItem_Remark;
                        resultItem.UDF_1 = item.uDF_1;
                        resultItem.UDF_2 = item.uDF_2;
                        resultItem.UDF_3 = item.uDF_3;
                        resultItem.UDF_4 = item.uDF_4;
                        resultItem.UDF_5 = item.uDF_5;
                        resultItem.Create_By = userName;
                        resultItem.Create_Date = DateTime.Now;

                        if (data.Yard == "1")
                        {
                            resultItem.Ref_Document_LineNum = item.lineNum;
                            resultItem.Ref_Document_No = data.purchaseOrder_No;
                            resultItem.Ref_Document_Index = item.purchaseOrder_Index;
                            resultItem.Ref_DocumentItem_Index = item.purchaseOrderItem_Index;
                        }
                        else
                        {
                            resultItem.Ref_Process_Index = item.ref_Process_Index;
                            resultItem.Ref_Document_LineNum = item.ref_Document_LineNum;
                            resultItem.Ref_Document_No = item.ref_Document_No;
                            resultItem.Ref_Document_Index = item.ref_Document_Index;
                            resultItem.Ref_DocumentItem_Index = item.ref_DocumentItem_Index;

                            resultItem.ERP_Location = item.erp_Location;
                        }

                        db.IM_PlanGoodsReceiveItem.Add(resultItem);

                        #region Add PO Ref
                        var resPOItemRef = db.im_PurchaseOrderItem_Ref.Where(c => c.PurchaseOrderItem_Ref_Index == resultItem.Ref_DocumentItem_Index).FirstOrDefault();
                        if (resPOItemRef != null)
                        {
                            im_PlanGoodsReceiveItem_Ref resultPGRItem_Ref = new im_PlanGoodsReceiveItem_Ref();
                            resultPGRItem_Ref.PlanGoodsReceiveItem_Ref_Index = resultItem.PlanGoodsReceiveItem_Index;
                            resultPGRItem_Ref.PlanGoodsReceive_Ref_Index = _guidPlanGoodsReceiveRef;
                            resultPGRItem_Ref.SAP_Id = resPOItemRef.SAP_Id;
                            resultPGRItem_Ref.ITEM_CAT = resPOItemRef.ITEM_CAT;
                            resultPGRItem_Ref.HIGH_LV_ITEM = resPOItemRef.HIGH_LV_ITEM;
                            resultPGRItem_Ref.Plant_Id = resPOItemRef.Plant_Id;
                            resultPGRItem_Ref.Sloc_Id = resPOItemRef.Sloc_Id;
                            resultPGRItem_Ref.Plan_QTY = resPOItemRef.Plan_QTY;
                            resultPGRItem_Ref.WMS_GET_FLAG = resPOItemRef.WMS_GET_FLAG;
                            resultPGRItem_Ref.SALE_UNIT = resPOItemRef.SALE_UNIT;
                            resultPGRItem_Ref.MSG_TYPE = resPOItemRef.MSG_TYPE;
                            resultPGRItem_Ref.MSG_TEXT = resPOItemRef.MSG_TEXT;
                            resultPGRItem_Ref.Document_Remark = resPOItemRef.Document_Remark;
                            resultPGRItem_Ref.CRE_DATE = resPOItemRef.CRE_DATE;
                            resultPGRItem_Ref.CRE_TIME = resPOItemRef.CRE_TIME;
                            resultPGRItem_Ref.CRE_BY = resPOItemRef.CRE_BY;
                            resultPGRItem_Ref.PROC_DATE = resPOItemRef.PROC_DATE;
                            resultPGRItem_Ref.PROC_TIME = resPOItemRef.PROC_TIME;
                            resultPGRItem_Ref.CHG_DATE = resPOItemRef.CHG_DATE;
                            resultPGRItem_Ref.CHG_TIME = resPOItemRef.CHG_TIME;
                            resultPGRItem_Ref.CHG_BY = resPOItemRef.CHG_BY;
                            resultPGRItem_Ref.Document_Status = resPOItemRef.Document_Status;
                            resultPGRItem_Ref.MovementType_Id = resPOItemRef.MovementType_Id;
                            resultPGRItem_Ref.soldTo_Id = resPOItemRef.soldTo_Id;
                            resultPGRItem_Ref.WareHouse_Id = resPOItemRef.WareHouse_Id;
                            resultPGRItem_Ref.RECEI_SLOC = resPOItemRef.RECEI_SLOC;
                            resultPGRItem_Ref.MAT_GRP = resPOItemRef.MAT_GRP;
                            resultPGRItem_Ref.MAT_GPNM = resPOItemRef.MAT_GPNM;
                            resultPGRItem_Ref.IO = resPOItemRef.IO;
                            resultPGRItem_Ref.COMP_NO = resPOItemRef.COMP_NO;
                            resultPGRItem_Ref.COMP_MAT = resPOItemRef.COMP_MAT;
                            resultPGRItem_Ref.COMP_NAME = resPOItemRef.COMP_NAME;
                            resultPGRItem_Ref.COMP_PLANT = resPOItemRef.COMP_PLANT;
                            resultPGRItem_Ref.COMP_SLOC = resPOItemRef.COMP_SLOC;
                            resultPGRItem_Ref.COMP_QTY_BASE = resPOItemRef.COMP_QTY_BASE;
                            resultPGRItem_Ref.COMP_UOM_BASE = resPOItemRef.COMP_UOM_BASE;
                            resultPGRItem_Ref.COMP_QTY = resPOItemRef.COMP_QTY;
                            resultPGRItem_Ref.COMP_UOM = resPOItemRef.COMP_UOM;
                            resultPGRItem_Ref.COLLECT_NO = resPOItemRef.COLLECT_NO;
                            resultPGRItem_Ref.CostCenter_Id = resPOItemRef.CostCenter_Id;
                            resultPGRItem_Ref.CostCenter_Name = resPOItemRef.CostCenter_Name;
                            resultPGRItem_Ref.TARGET_QTY = resPOItemRef.TARGET_QTY;
                            resultPGRItem_Ref.PROF_NO = resPOItemRef.PROF_NO;
                            resultPGRItem_Ref.PROF_NAME = resPOItemRef.PROF_NAME;
                            resultPGRItem_Ref.ORDER_NAME = resPOItemRef.ORDER_NAME;
                            resultPGRItem_Ref.GR_RCPT = resPOItemRef.GR_RCPT;
                            resultPGRItem_Ref.UNLOAD_PT = resPOItemRef.UNLOAD_PT;
                            resultPGRItem_Ref.STORE_PBL = resPOItemRef.STORE_PBL;
                            resultPGRItem_Ref.CREATE_ASSET = resPOItemRef.CREATE_ASSET;
                            resultPGRItem_Ref.ASSET_NO = resPOItemRef.ASSET_NO;
                            resultPGRItem_Ref.ORDERID = resPOItemRef.ORDERID;
                            resultPGRItem_Ref.STORE_COSTCENTER = resPOItemRef.STORE_COSTCENTER;
                            resultPGRItem_Ref.ITEM_TEXT = resPOItemRef.ITEM_TEXT;
                            resultPGRItem_Ref.ZLAST_FLAG = resPOItemRef.ZLAST_FLAG;

                            db.im_PlanGoodsReceiveItem_Ref.Add(resultPGRItem_Ref);
                        }
                        #endregion
                    }
                }
                else
                {
                    var _guidOldPlanGoodsReceiveRef = Guid.NewGuid();

                    PlanGoodsReceiveNo = PlanGoodsReceiveOld.PlanGoodsReceive_No;
                    PlanGoodsReceiveOld.PlanGoodsReceive_Index = data.planGoodsReceive_Index;
                    PlanGoodsReceiveOld.PlanGoodsReceive_No = data.planGoodsReceive_No;
                    PlanGoodsReceiveOld.Owner_Index = data.owner_Index;
                    PlanGoodsReceiveOld.Owner_Id = data.owner_Id;
                    PlanGoodsReceiveOld.Owner_Name = data.owner_Name;
                    PlanGoodsReceiveOld.Vendor_Index = data.vendor_Index;
                    PlanGoodsReceiveOld.Vendor_Id = data.vendor_Id;
                    PlanGoodsReceiveOld.Vendor_Name = data.vendor_Name;
                    PlanGoodsReceiveOld.DocumentType_Index = data.documentType_Index;
                    PlanGoodsReceiveOld.DocumentType_Id = data.documentType_Id;
                    PlanGoodsReceiveOld.DocumentType_Name = data.documentType_Name;
                    PlanGoodsReceiveOld.PlanGoodsReceive_Date = data.planGoodsReceive_Date.toDate();
                    PlanGoodsReceiveOld.PlanGoodsReceive_Due_Date = data.planGoodsReceive_Due_Date.toDate();
                    PlanGoodsReceiveOld.PlanGoodsReceive_Due_DateTime = data.planGoodsReceive_Due_DateTime;
                    PlanGoodsReceiveOld.PlanGoodsReceive_Time = data.planGoodsReceive_Time;

                    PlanGoodsReceiveOld.DocumentRef_No1 = data.documentRef_No1;
                    PlanGoodsReceiveOld.DocumentRef_No2 = data.documentRef_No2;
                    PlanGoodsReceiveOld.DocumentRef_No3 = data.documentRef_No3;
                    PlanGoodsReceiveOld.DocumentRef_No4 = data.documentRef_No4;
                    PlanGoodsReceiveOld.DocumentRef_No5 = data.documentRef_No5;
                    PlanGoodsReceiveOld.Document_Status = data.document_Status;
                    PlanGoodsReceiveOld.UDF_1 = data.uDF_1;
                    PlanGoodsReceiveOld.UDF_2 = data.uDF_2;
                    PlanGoodsReceiveOld.UDF_3 = data.uDF_3;
                    PlanGoodsReceiveOld.UDF_4 = data.uDF_4;
                    PlanGoodsReceiveOld.UDF_5 = data.uDF_5;
                    PlanGoodsReceiveOld.DocumentPriority_Status = data.documentpriority_Status;
                    PlanGoodsReceiveOld.Document_Remark = data.document_Remark;
                    PlanGoodsReceiveOld.Warehouse_Index = data.warehouse_Index;
                    PlanGoodsReceiveOld.Warehouse_Id = data.warehouse_Id;
                    PlanGoodsReceiveOld.Warehouse_Name = data.warehouse_Name;
                    PlanGoodsReceiveOld.Warehouse_Index_To = data.warehouse_Index_To;
                    PlanGoodsReceiveOld.Warehouse_Id_To = data.warehouse_Id_To;
                    PlanGoodsReceiveOld.Warehouse_Name_To = data.warehouse_Name_To;
                    PlanGoodsReceiveOld.Dock_Index = data.dock_Index;
                    PlanGoodsReceiveOld.Dock_Id = data.dock_Id;
                    PlanGoodsReceiveOld.Dock_Name = data.dock_Name;
                    PlanGoodsReceiveOld.VehicleType_Index = data.vehicleType_Index;
                    PlanGoodsReceiveOld.VehicleType_Id = data.vehicleType_Id;
                    PlanGoodsReceiveOld.VehicleType_Name = data.vehicleType_Name;
                    PlanGoodsReceiveOld.Transport_Index = data.transport_Index;
                    PlanGoodsReceiveOld.Transport_Id = data.transport_Id;
                    PlanGoodsReceiveOld.Transport_Name = data.transport_Name;
                    PlanGoodsReceiveOld.Driver_Name = data.driver_Name;
                    PlanGoodsReceiveOld.Round_Index = data.round_Index;
                    PlanGoodsReceiveOld.Round_Id = data.round_Id;
                    PlanGoodsReceiveOld.Round_Name = data.round_Name;
                    PlanGoodsReceiveOld.License_Name = data.license_Name;
                    PlanGoodsReceiveOld.UserAssign = "";
                    PlanGoodsReceiveOld.Forwarder_Index = data.forwarder_Index;
                    PlanGoodsReceiveOld.Forwarder_Id = data.forwarder_Id;
                    PlanGoodsReceiveOld.Forwarder_Name = data.forwarder_Name;
                    PlanGoodsReceiveOld.ShipmentType_Index = data.shipmentType_Index;
                    PlanGoodsReceiveOld.ShipmentType_Id = data.shipmentType_Id;
                    PlanGoodsReceiveOld.ShipmentType_Name = data.shipmentType_Name;
                    PlanGoodsReceiveOld.CargoType_Index = data.cargoType_Index;
                    PlanGoodsReceiveOld.CargoType_Id = data.cargoType_Id;
                    PlanGoodsReceiveOld.CargoType_Name = data.cargoType_Name;
                    PlanGoodsReceiveOld.UnloadingType_Index = data.unloadingType_Index;
                    PlanGoodsReceiveOld.UnloadingType_Id = data.unloadingType_Id;
                    PlanGoodsReceiveOld.UnloadingType_Name = data.unloadingType_Name;
                    PlanGoodsReceiveOld.ContainerType_Index = data.containerType_Index;
                    PlanGoodsReceiveOld.ContainerType_Id = data.containerType_Id;
                    PlanGoodsReceiveOld.ContainerType_Name = data.containerType_Name;
                    PlanGoodsReceiveOld.Container_No1 = data.container_No1;
                    PlanGoodsReceiveOld.Container_No2 = data.container_No2;
                    PlanGoodsReceiveOld.Labur = data.labur;

                    PlanGoodsReceiveOld.CostCenter_Index = data.costCenter_Index;
                    PlanGoodsReceiveOld.CostCenter_Id = data.costCenter_Id;
                    PlanGoodsReceiveOld.CostCenter_Name = data.costCenter_Name;

                    if (IsNew != true)
                    {
                        PlanGoodsReceiveOld.Update_By = data.update_By;
                        PlanGoodsReceiveOld.Update_Date = DateTime.Now;
                    }

                    //if (data.listPlanGoodsReceiveItemViewModel != null)
                    //{
                    //    _purchaseOrderNo = data.listPlanGoodsReceiveItemViewModel[0].ref_Document_No;
                    //}

                    //if (_purchaseOrderNo != "")
                    //{
                    //    var resPORef = db.im_PurchaseOrder_Ref.Where(c => c.PurchaseOrder_No == _purchaseOrderNo).FirstOrDefault();
                    //    if (resPORef != null)
                    //    {
                    //        _purchaseOrderRefIndex = resPORef.PurchaseOrder_Ref_Index;

                    //        im_PlanGoodsReceive_Ref resultPGR_Ref = new im_PlanGoodsReceive_Ref();
                    //        resultPGR_Ref.PlanGoodsReceive_Ref_Index = _guidOldPlanGoodsReceiveRef;
                    //        resultPGR_Ref.PlanGoodsReceive_Index = PlanGoodsReceiveOld.PlanGoodsReceive_Index;
                    //        resultPGR_Ref.PlanGoodsReceive_No = PlanGoodsReceiveOld.PlanGoodsReceive_No;
                    //        resultPGR_Ref.SO_No = resPORef.SO_No;
                    //        resultPGR_Ref.SO_Type = resPORef.SO_Type;
                    //        resultPGR_Ref.SHIP_CON = resPORef.SHIP_CON;
                    //        resultPGR_Ref.DO_Type = resPORef.DO_Type;
                    //        resultPGR_Ref.RTN_Flag = resPORef.RTN_Flag;
                    //        resultPGR_Ref.CREDIT_STATUS = resPORef.CREDIT_STATUS;
                    //        resultPGR_Ref.EXPORT_FLAG = resPORef.EXPORT_FLAG;
                    //        resultPGR_Ref.FOC_FLAG = resPORef.FOC_FLAG;
                    //        resultPGR_Ref.CLAIM_FLAG = resPORef.CLAIM_FLAG;
                    //        resultPGR_Ref.Ref_Type = resPORef.Ref_Type;
                    //        resultPGR_Ref.IO = resPORef.IO;
                    //        resultPGR_Ref.TAR_VAL = resPORef.TAR_VAL;
                    //        resultPGR_Ref.YEAR = resPORef.YEAR;
                    //        resultPGR_Ref.ITEM_TEXT = resPORef.ITEM_TEXT;

                    //        db.im_PlanGoodsReceive_Ref.Add(resultPGR_Ref);
                    //    }
                    //}

                    if (data.documents != null)
                    {
                        foreach (var d in data.documents)
                        {
                            if (d.index == null || d.index == Guid.Empty)
                            {
                                im_DocumentFile documents = new im_DocumentFile();

                                documents.DocumentFile_Index = Guid.NewGuid(); ;
                                documents.DocumentFile_Name = d.filename;
                                documents.DocumentFile_Path = d.path;
                                documents.DocumentFile_Url = d.urlAttachFile;
                                documents.DocumentFile_Type = d.type;
                                documents.DocumentFile_Status = 0;
                                documents.Create_By = userName;
                                documents.Create_Date = DateTime.Now;
                                documents.Ref_Index = PlanGoodsReceiveOld.PlanGoodsReceive_Index;
                                documents.Ref_No = PlanGoodsReceiveOld.PlanGoodsReceive_No;
                                db.im_DocumentFile.Add(documents);
                            }
                            else if ((d.index != null || d.index != Guid.Empty) && d.isDelete)
                            {
                                var Documents = db.im_DocumentFile.FirstOrDefault(c => c.DocumentFile_Index == d.index && c.Ref_Index == PlanGoodsReceiveOld.PlanGoodsReceive_Index && c.DocumentFile_Status == 0);
                                Documents.DocumentFile_Status = -1;
                                Documents.Update_By = data.update_By;
                                Documents.Update_Date = DateTime.Now;
                            }
                        }
                    }

                    var groupRefNo = data.listPlanGoodsReceiveItemViewModel.GroupBy(item => item.ref_Document_No)
                     .Select(group => new { Ref_Document_No = group.Key })
                     .ToList();

                    if (groupRefNo.Count > 0)
                    {
                        foreach (var item in groupRefNo)
                        {
                            var refOld = db.im_PlanGoodsReceive_Ref.Where(c => c.Ref_Document_No == item.Ref_Document_No && c.PlanGoodsReceive_Index == PlanGoodsReceiveOld.PlanGoodsReceive_Index).FirstOrDefault();

                            if (refOld != null)
                            {
                                continue;
                            }

                            var resPORef = db.im_PurchaseOrder_Ref.Where(c => c.PurchaseOrder_No == item.Ref_Document_No).FirstOrDefault();
                            if (resPORef != null)
                            {
                                //_purchaseOrderRefIndex = resPORef.PurchaseOrder_Ref_Index;

                                im_PlanGoodsReceive_Ref resultPGR_Ref = new im_PlanGoodsReceive_Ref();
                                resultPGR_Ref.PlanGoodsReceive_Ref_Index = Guid.NewGuid();
                                resultPGR_Ref.PlanGoodsReceive_Index = PlanGoodsReceiveOld.PlanGoodsReceive_Index;
                                resultPGR_Ref.PlanGoodsReceive_No = PlanGoodsReceiveOld.PlanGoodsReceive_No;
                                resultPGR_Ref.SO_No = resPORef.SO_No;
                                resultPGR_Ref.SO_Type = resPORef.SO_Type;
                                resultPGR_Ref.SHIP_CON = resPORef.SHIP_CON;
                                resultPGR_Ref.DO_Type = resPORef.DO_Type;
                                resultPGR_Ref.RTN_Flag = resPORef.RTN_Flag;
                                resultPGR_Ref.CREDIT_STATUS = resPORef.CREDIT_STATUS;
                                resultPGR_Ref.EXPORT_FLAG = resPORef.EXPORT_FLAG;
                                resultPGR_Ref.FOC_FLAG = resPORef.FOC_FLAG;
                                resultPGR_Ref.CLAIM_FLAG = resPORef.CLAIM_FLAG;
                                resultPGR_Ref.Ref_Type = resPORef.Ref_Type;
                                resultPGR_Ref.IO = resPORef.IO;
                                resultPGR_Ref.TAR_VAL = resPORef.TAR_VAL;
                                resultPGR_Ref.YEAR = resPORef.YEAR;
                                resultPGR_Ref.ITEM_TEXT = resPORef.ITEM_TEXT;
                                resultPGR_Ref.Ref_Document_Index = resPORef.PurchaseOrder_Index;
                                resultPGR_Ref.Ref_Document_No = resPORef.PurchaseOrder_No;

                                db.im_PlanGoodsReceive_Ref.Add(resultPGR_Ref);
                            }
                        }
                    }

                    foreach (var item in data.listPlanGoodsReceiveItemViewModel)
                    {

                        var PlanGoodsReceiveItemOld = db.IM_PlanGoodsReceiveItem.Find(item.planGoodsReceiveItem_Index);

                        if (PlanGoodsReceiveItemOld != null)
                        {

                            var Productresult = new List<ProductViewModel>();

                            var ProductfilterModel = new ProductViewModel();
                            ProductfilterModel.product_Index = item.product_Index;

                            //GetConfig
                            Productresult = utils.SendDataApi<List<ProductViewModel>>(new AppSettingConfig().GetUrl("product"), ProductfilterModel.sJson());


                            int addNumber = 0;

                            IM_PlanGoodsReceiveItem resultItem = new IM_PlanGoodsReceiveItem();

                            //Get ItemStatus

                            addNumber++;

                            PlanGoodsReceiveItemOld.PlanGoodsReceiveItem_Index = item.planGoodsReceiveItem_Index.GetValueOrDefault();
                            PlanGoodsReceiveItemOld.PlanGoodsReceive_Index = item.planGoodsReceive_Index;

                            if (item.lineNum == null)
                            {
                                PlanGoodsReceiveItemOld.LineNum = addNumber.ToString();
                            }
                            else
                            {
                                PlanGoodsReceiveItemOld.LineNum = item.lineNum;
                            }

                            PlanGoodsReceiveItemOld.Product_Index = item.product_Index;
                            PlanGoodsReceiveItemOld.Product_Id = item.product_Id;
                            PlanGoodsReceiveItemOld.Product_Name = item.product_Name;
                            if (Productresult.Count > 0)
                            {
                                PlanGoodsReceiveItemOld.Product_SecondName = Productresult.FirstOrDefault().product_SecondName;
                                PlanGoodsReceiveItemOld.Product_ThirdName = Productresult.FirstOrDefault().product_ThirdName;
                            }

                            if (item.product_Lot != null)
                            {
                                PlanGoodsReceiveItemOld.Product_Lot = item.product_Lot;
                            }
                            else
                            {
                                PlanGoodsReceiveItemOld.Product_Lot = "";
                            }
                            PlanGoodsReceiveItemOld.ItemStatus_Index = item.itemStatus_Index;
                            PlanGoodsReceiveItemOld.ItemStatus_Id = item.itemStatus_Id;
                            PlanGoodsReceiveItemOld.ItemStatus_Name = item.itemStatus_Name;

                            PlanGoodsReceiveItemOld.Qty = item.qty;
                            PlanGoodsReceiveItemOld.Ratio = item.ratio;
                            if (item.ratio != 0)
                            {
                                var totalqty = item.qty * item.ratio;
                                item.totalQty = totalqty;
                            }
                            PlanGoodsReceiveItemOld.TotalQty = item.totalQty;
                            PlanGoodsReceiveItemOld.ProductConversion_Index = item.productConversion_Index;
                            PlanGoodsReceiveItemOld.ProductConversion_Id = item.productConversion_Id;
                            PlanGoodsReceiveItemOld.ProductConversion_Name = item.productConversion_Name;
                            PlanGoodsReceiveItemOld.MFG_Date = item.mFG_Date.toDate();
                            PlanGoodsReceiveItemOld.EXP_Date = item.eXP_Date.toDate();

                            PlanGoodsReceiveItemOld.WeightRatio = item.weightRatio;
                            PlanGoodsReceiveItemOld.UnitWeight = item.unitWeight;
                            PlanGoodsReceiveItemOld.Weight = item.qty * (item.unitWeight * item.weightRatio);
                            PlanGoodsReceiveItemOld.Weight_Index = item.weight_Index;
                            PlanGoodsReceiveItemOld.Weight_Id = item.weight_Id;
                            PlanGoodsReceiveItemOld.Weight_Name = item.weight_Name;
                            PlanGoodsReceiveItemOld.NetWeight = PlanGoodsReceiveItemOld.Weight * item.qty;

                            PlanGoodsReceiveItemOld.GrsWeightRatio = item.grsWeightRatio;
                            PlanGoodsReceiveItemOld.UnitGrsWeight = item.unitGrsWeight;
                            PlanGoodsReceiveItemOld.GrsWeight = item.qty * (item.unitGrsWeight * item.grsWeightRatio);
                            PlanGoodsReceiveItemOld.GrsWeight_Index = item.grsWeight_Index;
                            PlanGoodsReceiveItemOld.GrsWeight_Id = item.grsWeight_Id;
                            PlanGoodsReceiveItemOld.GrsWeight_Name = item.grsWeight_Name;

                            PlanGoodsReceiveItemOld.WidthRatio = item.widthRatio;
                            PlanGoodsReceiveItemOld.UnitWidth = item.unitWidth;
                            PlanGoodsReceiveItemOld.Width = item.unitWidth * item.qty;
                            PlanGoodsReceiveItemOld.Width_Index = item.width_Index;
                            PlanGoodsReceiveItemOld.Width_Id = item.width_Id;
                            PlanGoodsReceiveItemOld.Width_Name = item.width_Name;

                            PlanGoodsReceiveItemOld.LengthRatio = item.lengthRatio;
                            PlanGoodsReceiveItemOld.UnitLength = item.unitLength;
                            PlanGoodsReceiveItemOld.Length = item.unitLength * item.qty;
                            PlanGoodsReceiveItemOld.Length_Index = item.length_Index;
                            PlanGoodsReceiveItemOld.Length_Id = item.length_Id;
                            PlanGoodsReceiveItemOld.Length_Name = item.length_Name;

                            PlanGoodsReceiveItemOld.HeightRatio = item.heightRatio;
                            PlanGoodsReceiveItemOld.UnitHeight = item.unitHeight;
                            PlanGoodsReceiveItemOld.Height = item.unitHeight * item.qty;
                            PlanGoodsReceiveItemOld.Height_Index = item.height_Index;
                            PlanGoodsReceiveItemOld.Height_Id = item.height_Id;
                            PlanGoodsReceiveItemOld.Height_Name = item.height_Name;


                            PlanGoodsReceiveItemOld.UnitVolume = (PlanGoodsReceiveItemOld.UnitWidth * PlanGoodsReceiveItemOld.UnitLength * PlanGoodsReceiveItemOld.UnitHeight) / item.volume_Ratio;
                            PlanGoodsReceiveItemOld.Volume = resultItem.Qty * PlanGoodsReceiveItemOld.UnitVolume;


                            PlanGoodsReceiveItemOld.UnitPrice = item.unitPrice;
                            PlanGoodsReceiveItemOld.Price = item.unitPrice * item.qty;
                            PlanGoodsReceiveItemOld.TotalPrice = PlanGoodsReceiveItemOld.Price * PlanGoodsReceiveItemOld.Qty;

                            PlanGoodsReceiveItemOld.Currency_Index = item.currency_Index;
                            PlanGoodsReceiveItemOld.Currency_Id = item.currency_Id;
                            PlanGoodsReceiveItemOld.Currency_Name = item.currency_Name;

                            PlanGoodsReceiveItemOld.Ref_Code1 = item.ref_Code1;
                            PlanGoodsReceiveItemOld.Ref_Code2 = item.ref_Code2;
                            PlanGoodsReceiveItemOld.Ref_Code3 = item.ref_Code3;
                            PlanGoodsReceiveItemOld.Ref_Code4 = item.ref_Code4;
                            PlanGoodsReceiveItemOld.Ref_Code5 = item.ref_Code5;

                            PlanGoodsReceiveItemOld.DocumentRef_No1 = item.documentRef_No1;
                            PlanGoodsReceiveItemOld.DocumentRef_No2 = item.documentRef_No2;
                            PlanGoodsReceiveItemOld.DocumentRef_No3 = item.documentRef_No3;
                            PlanGoodsReceiveItemOld.DocumentRef_No4 = item.documentRef_No4;
                            PlanGoodsReceiveItemOld.DocumentRef_No5 = item.documentRef_No5;
                            PlanGoodsReceiveItemOld.Document_Status = 0;
                            PlanGoodsReceiveItemOld.DocumentItem_Remark = item.documentItem_Remark;
                            PlanGoodsReceiveItemOld.UDF_1 = item.uDF_1;
                            PlanGoodsReceiveItemOld.UDF_2 = item.uDF_2;
                            PlanGoodsReceiveItemOld.UDF_3 = item.uDF_3;
                            PlanGoodsReceiveItemOld.UDF_4 = item.uDF_4;
                            PlanGoodsReceiveItemOld.UDF_5 = item.uDF_5;
                            PlanGoodsReceiveItemOld.Update_By = userName;
                            PlanGoodsReceiveItemOld.Update_Date = DateTime.Now;

                            //PlanGoodsReceiveItemOld.Ref_Process_Index = item.ref_Process_Index;
                            //PlanGoodsReceiveItemOld.Ref_Document_LineNum = item.ref_Document_LineNum;
                            //PlanGoodsReceiveItemOld.Ref_Document_No = item.ref_Document_No;
                            //PlanGoodsReceiveItemOld.Ref_Document_Index = item.ref_Document_Index;
                            //PlanGoodsReceiveItemOld.Ref_DocumentItem_Index = item.ref_DocumentItem_Index;
                            //PlanGoodsReceiveItemOld.ERP_Location = item.erp_Location;

                            _purchaseOrderNo = PlanGoodsReceiveItemOld.Ref_Document_No;
                        }

                        else
                        {
                            int addNumber = 0;

                            IM_PlanGoodsReceiveItem resultItem = new IM_PlanGoodsReceiveItem();

                            var Productresult = new List<ProductViewModel>();

                            var ProductfilterModel = new ProductViewModel();
                            ProductfilterModel.product_Index = item.product_Index;

                            //GetConfig
                            Productresult = utils.SendDataApi<List<ProductViewModel>>(new AppSettingConfig().GetUrl("product"), ProductfilterModel.sJson());


                            addNumber++;
                            // Gen Index for line item

                            item.planGoodsReceiveItem_Index = Guid.NewGuid();
                            //resultItem.PlanGoodsReceive_Index = item.planGoodsReceive_Index;

                            resultItem.PlanGoodsReceiveItem_Index = Guid.NewGuid();

                            resultItem.PlanGoodsReceive_Index = data.planGoodsReceive_Index;

                            // Index From Header

                            if (item.lineNum == null)
                            {
                                resultItem.LineNum = addNumber.ToString();
                            }
                            else
                            {
                                resultItem.LineNum = item.lineNum;
                            }
                            resultItem.ItemStatus_Index = item.itemStatus_Index;

                            resultItem.ItemStatus_Id = item.itemStatus_Id;

                            resultItem.ItemStatus_Name = item.itemStatus_Name;
                            resultItem.Product_Index = item.product_Index;
                            resultItem.Product_Id = item.product_Id;
                            resultItem.Product_Name = item.product_Name;
                            if (Productresult.Count > 0)
                            {
                                resultItem.Product_SecondName = Productresult.FirstOrDefault().product_SecondName;
                                resultItem.Product_ThirdName = Productresult.FirstOrDefault().product_ThirdName;
                            }
                            if (item.product_Lot != null)
                            {
                                resultItem.Product_Lot = item.product_Lot;
                            }
                            else
                            {
                                resultItem.Product_Lot = "";
                            }
                            resultItem.Qty = item.qty;
                            resultItem.Ratio = item.ratio;
                            if (item.ratio != 0)
                            {
                                var totalqty = item.qty * item.ratio;
                                item.totalQty = totalqty;
                            }
                            resultItem.TotalQty = item.totalQty;
                            resultItem.ProductConversion_Index = item.productConversion_Index;
                            resultItem.ProductConversion_Id = item.productConversion_Id;
                            resultItem.ProductConversion_Name = item.productConversion_Name;
                            resultItem.MFG_Date = item.mFG_Date.toDate();
                            resultItem.EXP_Date = item.eXP_Date.toDate();

                            resultItem.WeightRatio = item.weightRatio;
                            resultItem.UnitWeight = item.unitWeight;
                            resultItem.Weight = item.qty * (item.unitWeight * item.weightRatio);
                            resultItem.Weight_Index = item.weight_Index;
                            resultItem.Weight_Id = item.weight_Id;
                            resultItem.Weight_Name = item.weight_Name;
                            resultItem.NetWeight = resultItem.Weight * item.qty;

                            resultItem.GrsWeightRatio = item.grsWeightRatio;
                            resultItem.UnitGrsWeight = item.unitGrsWeight;
                            resultItem.GrsWeight = item.qty * (item.unitGrsWeight * item.grsWeightRatio);
                            resultItem.GrsWeight_Index = item.grsWeight_Index;
                            resultItem.GrsWeight_Id = item.grsWeight_Id;
                            resultItem.GrsWeight_Name = item.grsWeight_Name;

                            resultItem.WidthRatio = item.widthRatio;
                            resultItem.UnitWidth = item.unitWidth;
                            resultItem.Width = item.unitWidth * item.qty;
                            resultItem.Width_Index = item.width_Index;
                            resultItem.Width_Id = item.width_Id;
                            resultItem.Width_Name = item.width_Name;

                            resultItem.LengthRatio = item.lengthRatio;
                            resultItem.UnitLength = item.unitLength;
                            resultItem.Length = item.unitLength * item.qty;
                            resultItem.Length_Index = item.length_Index;
                            resultItem.Length_Id = item.length_Id;
                            resultItem.Length_Name = item.length_Name;

                            resultItem.HeightRatio = item.heightRatio;
                            resultItem.UnitHeight = item.unitHeight;
                            resultItem.Height = item.unitHeight * item.qty;
                            resultItem.Height_Index = item.height_Index;
                            resultItem.Height_Id = item.height_Id;
                            resultItem.Height_Name = item.height_Name;

                            //resultItem.UnitVolume = item.unitVolume;
                            //resultItem.Volume = item.volume;

                            resultItem.UnitVolume = (resultItem.UnitWidth * resultItem.UnitLength * resultItem.UnitHeight) / item.volume_Ratio;
                            resultItem.Volume = resultItem.Qty * resultItem.UnitVolume;

                            resultItem.UnitPrice = item.unitPrice;
                            resultItem.Price = item.unitPrice * item.qty;
                            resultItem.TotalPrice = resultItem.Price * resultItem.Qty;

                            resultItem.Currency_Index = item.currency_Index;
                            resultItem.Currency_Id = item.currency_Id;
                            resultItem.Currency_Name = item.currency_Name;

                            resultItem.Ref_Code1 = item.ref_Code1;
                            resultItem.Ref_Code2 = item.ref_Code2;
                            resultItem.Ref_Code3 = item.ref_Code3;
                            resultItem.Ref_Code4 = item.ref_Code4;
                            resultItem.Ref_Code5 = item.ref_Code5;

                            resultItem.DocumentRef_No1 = item.documentRef_No1;
                            resultItem.DocumentRef_No2 = item.documentRef_No2;
                            resultItem.DocumentRef_No3 = item.documentRef_No3;
                            resultItem.DocumentRef_No4 = item.documentRef_No4;
                            resultItem.DocumentRef_No5 = item.documentRef_No5;
                            resultItem.Document_Status = 0;
                            resultItem.DocumentItem_Remark = item.documentItem_Remark;
                            resultItem.UDF_1 = item.uDF_1;
                            resultItem.UDF_2 = item.uDF_2;
                            resultItem.UDF_3 = item.uDF_3;
                            resultItem.UDF_4 = item.uDF_4;
                            resultItem.UDF_5 = item.uDF_5;
                            resultItem.Update_By = userName;
                            resultItem.Update_Date = DateTime.Now;

                            if (data.Yard == "1")
                            {
                                resultItem.Ref_Document_LineNum = item.lineNum;
                                resultItem.Ref_Document_No = data.purchaseOrder_No;
                                resultItem.Ref_Document_Index = item.purchaseOrder_Index;
                                resultItem.Ref_DocumentItem_Index = item.purchaseOrderItem_Index;
                            }
                            else
                            {
                                resultItem.Ref_Process_Index = item.ref_Process_Index;
                                resultItem.Ref_Document_LineNum = item.ref_Document_LineNum;
                                resultItem.Ref_Document_No = item.ref_Document_No;
                                resultItem.Ref_Document_Index = item.ref_Document_Index;
                                resultItem.Ref_DocumentItem_Index = item.ref_DocumentItem_Index;

                                resultItem.ERP_Location = item.erp_Location;
                            }


                            var resPOItemRef = db.im_PurchaseOrderItem_Ref.Where(c => c.PurchaseOrderItem_Ref_Index == resultItem.Ref_DocumentItem_Index).FirstOrDefault();
                            if (resPOItemRef != null)
                            {
                                im_PlanGoodsReceiveItem_Ref resultPGRItem_Ref = new im_PlanGoodsReceiveItem_Ref();
                                resultPGRItem_Ref.PlanGoodsReceiveItem_Ref_Index = resultItem.PlanGoodsReceiveItem_Index;
                                resultPGRItem_Ref.PlanGoodsReceive_Ref_Index = _guidOldPlanGoodsReceiveRef;
                                resultPGRItem_Ref.SAP_Id = resPOItemRef.SAP_Id;
                                resultPGRItem_Ref.ITEM_CAT = resPOItemRef.ITEM_CAT;
                                resultPGRItem_Ref.HIGH_LV_ITEM = resPOItemRef.HIGH_LV_ITEM;
                                resultPGRItem_Ref.Plant_Id = resPOItemRef.Plant_Id;
                                resultPGRItem_Ref.Sloc_Id = resPOItemRef.Sloc_Id;
                                resultPGRItem_Ref.Plan_QTY = resPOItemRef.Plan_QTY;
                                resultPGRItem_Ref.WMS_GET_FLAG = resPOItemRef.WMS_GET_FLAG;
                                resultPGRItem_Ref.SALE_UNIT = resPOItemRef.SALE_UNIT;
                                resultPGRItem_Ref.MSG_TYPE = resPOItemRef.MSG_TYPE;
                                resultPGRItem_Ref.MSG_TEXT = resPOItemRef.MSG_TEXT;
                                resultPGRItem_Ref.Document_Remark = resPOItemRef.Document_Remark;
                                resultPGRItem_Ref.CRE_DATE = resPOItemRef.CRE_DATE;
                                resultPGRItem_Ref.CRE_TIME = resPOItemRef.CRE_TIME;
                                resultPGRItem_Ref.CRE_BY = resPOItemRef.CRE_BY;
                                resultPGRItem_Ref.PROC_DATE = resPOItemRef.PROC_DATE;
                                resultPGRItem_Ref.PROC_TIME = resPOItemRef.PROC_TIME;
                                resultPGRItem_Ref.CHG_DATE = resPOItemRef.CHG_DATE;
                                resultPGRItem_Ref.CHG_TIME = resPOItemRef.CHG_TIME;
                                resultPGRItem_Ref.CHG_BY = resPOItemRef.CHG_BY;
                                resultPGRItem_Ref.Document_Status = resPOItemRef.Document_Status;
                                resultPGRItem_Ref.MovementType_Id = resPOItemRef.MovementType_Id;
                                resultPGRItem_Ref.soldTo_Id = resPOItemRef.soldTo_Id;
                                resultPGRItem_Ref.WareHouse_Id = resPOItemRef.WareHouse_Id;
                                resultPGRItem_Ref.RECEI_SLOC = resPOItemRef.RECEI_SLOC;
                                resultPGRItem_Ref.MAT_GRP = resPOItemRef.MAT_GRP;
                                resultPGRItem_Ref.MAT_GPNM = resPOItemRef.MAT_GPNM;
                                resultPGRItem_Ref.IO = resPOItemRef.IO;
                                resultPGRItem_Ref.COMP_NO = resPOItemRef.COMP_NO;
                                resultPGRItem_Ref.COMP_MAT = resPOItemRef.COMP_MAT;
                                resultPGRItem_Ref.COMP_NAME = resPOItemRef.COMP_NAME;
                                resultPGRItem_Ref.COMP_PLANT = resPOItemRef.COMP_PLANT;
                                resultPGRItem_Ref.COMP_SLOC = resPOItemRef.COMP_SLOC;
                                resultPGRItem_Ref.COMP_QTY_BASE = resPOItemRef.COMP_QTY_BASE;
                                resultPGRItem_Ref.COMP_UOM_BASE = resPOItemRef.COMP_UOM_BASE;
                                resultPGRItem_Ref.COMP_QTY = resPOItemRef.COMP_QTY;
                                resultPGRItem_Ref.COMP_UOM = resPOItemRef.COMP_UOM;
                                resultPGRItem_Ref.COLLECT_NO = resPOItemRef.COLLECT_NO;
                                resultPGRItem_Ref.CostCenter_Id = resPOItemRef.CostCenter_Id;
                                resultPGRItem_Ref.CostCenter_Name = resPOItemRef.CostCenter_Name;
                                resultPGRItem_Ref.TARGET_QTY = resPOItemRef.TARGET_QTY;
                                resultPGRItem_Ref.PROF_NO = resPOItemRef.PROF_NO;
                                resultPGRItem_Ref.PROF_NAME = resPOItemRef.PROF_NAME;
                                resultPGRItem_Ref.ORDER_NAME = resPOItemRef.ORDER_NAME;
                                resultPGRItem_Ref.GR_RCPT = resPOItemRef.GR_RCPT;
                                resultPGRItem_Ref.UNLOAD_PT = resPOItemRef.UNLOAD_PT;
                                resultPGRItem_Ref.STORE_PBL = resPOItemRef.STORE_PBL;
                                resultPGRItem_Ref.CREATE_ASSET = resPOItemRef.CREATE_ASSET;
                                resultPGRItem_Ref.ASSET_NO = resPOItemRef.ASSET_NO;
                                resultPGRItem_Ref.ORDERID = resPOItemRef.ORDERID;
                                resultPGRItem_Ref.STORE_COSTCENTER = resPOItemRef.STORE_COSTCENTER;
                                resultPGRItem_Ref.ITEM_TEXT = resPOItemRef.ITEM_TEXT;
                                resultPGRItem_Ref.ZLAST_FLAG = resPOItemRef.ZLAST_FLAG;

                                db.im_PlanGoodsReceiveItem_Ref.Add(resultPGRItem_Ref);
                                
                            }

                            db.IM_PlanGoodsReceiveItem.Add(resultItem);
                        }


                    }

                    var deleteItem = db.IM_PlanGoodsReceiveItem.Where(c => !data.listPlanGoodsReceiveItemViewModel.Select(s => s.planGoodsReceiveItem_Index).Contains(c.PlanGoodsReceiveItem_Index)
                                        && c.PlanGoodsReceive_Index == PlanGoodsReceiveOld.PlanGoodsReceive_Index).ToList();

                    foreach (var c in deleteItem)
                    {
                        var deletePlanGoodsReceiveItem = db.IM_PlanGoodsReceiveItem.Find(c.PlanGoodsReceiveItem_Index);

                        deletePlanGoodsReceiveItem.Document_Status = -1;
                        deletePlanGoodsReceiveItem.Update_By = userName;
                        deletePlanGoodsReceiveItem.Update_Date = DateTime.Now;

                    }
                }

                var transactionx = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transactionx.Commit();
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("SavePlanGR", msglog);
                    transactionx.Rollback();

                    throw exy;

                }

                try
                {
                    var groupline = data.listPlanGoodsReceiveItemViewModel.GroupBy(c => c.uDF_4).Select(c => c.Key).ToList();
                    foreach (var item in groupline)
                    {
                        if (item == null)
                        {
                            continue;
                        }
                        var sPlanGoodsIssue_Index = new SqlParameter("@PlanGoodsReceive_Index", PlanGoodsReceiveIndex);
                        var sHIGH_LV_ITEM = new SqlParameter("@HIGH_LV_ITEM", item);

                        var ViewBOM = db.View_GoodsReceiveBOM.FromSql(" sp_GetCheckBOMAfterGR  @PlanGoodsReceive_Index , @HIGH_LV_ITEM ", sPlanGoodsIssue_Index, sHIGH_LV_ITEM).ToList();
                        foreach (var itemx in ViewBOM)
                        {
                            if (itemx.CheckBOM == "NOTPASS")
                            {
                                actionResult.Message = false;
                                return actionResult;
                            }
                        }
                        
                        olog.logging("updateCheckBOM", "sp_GetCheckBOMAfterTask : " + PlanGoodsReceiveIndex + " , " + item);
                    }
                    

                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("SavePlanGR", msglog);
                    transactionx.Rollback();

                    throw exy;

                }

                actionResult.document_No = PlanGoodsReceiveNo;
                actionResult.planGoodsReceive_Index = PlanGoodsReceiveIndex;
                actionResult.Message = true;

                return actionResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region find
        public PlanGoodsReceiveDocViewModel find(Guid id)
        {

            try
            {
                var queryResult = db.IM_PlanGoodsReceive.Where(c => c.PlanGoodsReceive_Index == id).FirstOrDefault();

                var resultItem = new PlanGoodsReceiveDocViewModel();


                var ProcessStatus = new List<ProcessStatusViewModel>();

                var filterModel = new ProcessStatusViewModel();

                filterModel.process_Index = new Guid("C2A3F847-BAA6-46FE-B502-44F2D5826A1C");

                //GetConfig
                ProcessStatus = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("processStatus"), filterModel.sJson());


                resultItem.planGoodsReceive_Index = queryResult.PlanGoodsReceive_Index;
                resultItem.planGoodsReceive_No = queryResult.PlanGoodsReceive_No;
                resultItem.documentType_Index = queryResult.DocumentType_Index;
                resultItem.documentType_Name = queryResult.DocumentType_Name;
                resultItem.documentType_Id = queryResult.DocumentType_Id;
                resultItem.vendor_Index = queryResult.Vendor_Index;
                resultItem.vendor_Id = queryResult.Vendor_Id;
                resultItem.vendor_Name = queryResult.Vendor_Name;
                resultItem.owner_Index = queryResult.Owner_Index;
                resultItem.owner_Id = queryResult.Owner_Id;
                resultItem.owner_Name = queryResult.Owner_Name;
                resultItem.uDF_1 = queryResult.UDF_1;
                resultItem.documentRef_No1 = queryResult.DocumentRef_No1;
                resultItem.documentRef_No2 = queryResult.DocumentRef_No2;
                resultItem.documentRef_No3 = queryResult.DocumentRef_No3;
                resultItem.documentRef_No4 = queryResult.DocumentRef_No4;
                resultItem.documentRef_No5 = queryResult.DocumentRef_No5;
                resultItem.document_Status = queryResult.Document_Status;
                resultItem.warehouse_Index = queryResult.Warehouse_Index;
                resultItem.warehouse_Index_To = queryResult.Warehouse_Index_To;
                resultItem.warehouse_Id = queryResult.Warehouse_Id;
                resultItem.warehouse_Id_To = queryResult.Warehouse_Id_To;
                resultItem.warehouse_Name = queryResult.Warehouse_Name;
                resultItem.warehouse_Name_To = queryResult.Warehouse_Name_To;
                resultItem.document_Remark = queryResult.Document_Remark;
                resultItem.planGoodsReceive_Date = queryResult.PlanGoodsReceive_Date.toString();
                resultItem.planGoodsReceive_Due_Date = queryResult.PlanGoodsReceive_Due_Date.toString();
                resultItem.userAssign = queryResult.UserAssign;

                resultItem.planGoodsReceive_Time = queryResult.PlanGoodsReceive_Time;
                resultItem.dock_Index = queryResult.Dock_Index;
                resultItem.dock_Id = queryResult.Dock_Id;
                resultItem.dock_Name = queryResult.Dock_Name;
                resultItem.vehicleType_Index = queryResult.VehicleType_Index;
                resultItem.vehicleType_Id = queryResult.VehicleType_Id;
                resultItem.vehicleType_Name = queryResult.VehicleType_Name;
                resultItem.transport_Index = queryResult.Transport_Index;
                resultItem.transport_Id = queryResult.Transport_Id;
                resultItem.transport_Name = queryResult.Transport_Name;
                resultItem.driver_Name = queryResult.Driver_Name;
                resultItem.round_Index = queryResult.Round_Index;
                resultItem.round_Id = queryResult.Round_Id;
                resultItem.round_Name = queryResult.Round_Name;
                resultItem.license_Name = queryResult.License_Name;

                resultItem.dock_Index = queryResult.Dock_Index;
                resultItem.dock_Id = queryResult.Dock_Id;
                resultItem.dock_Name = queryResult.Dock_Name;
                resultItem.vehicleType_Index = queryResult.VehicleType_Index;
                resultItem.vehicleType_Id = queryResult.VehicleType_Id;
                resultItem.vehicleType_Name = queryResult.VehicleType_Name;
                resultItem.transport_Index = queryResult.Transport_Index;
                resultItem.transport_Id = queryResult.Transport_Id;
                resultItem.transport_Name = queryResult.Transport_Name;
                resultItem.driver_Name = queryResult.Driver_Name;
                resultItem.round_Index = queryResult.Round_Index;
                resultItem.round_Id = queryResult.Round_Id;
                resultItem.round_Name = queryResult.Round_Name;
                resultItem.license_Name = queryResult.License_Name;
                resultItem.forwarder_Index = queryResult.Forwarder_Index;
                resultItem.forwarder_Id = queryResult.Forwarder_Id;
                resultItem.forwarder_Name = queryResult.Forwarder_Name;
                resultItem.shipmentType_Index = queryResult.ShipmentType_Index;
                resultItem.shipmentType_Id = queryResult.ShipmentType_Id;
                resultItem.shipmentType_Name = queryResult.ShipmentType_Name;
                resultItem.cargoType_Index = queryResult.CargoType_Index;
                resultItem.cargoType_Id = queryResult.CargoType_Id;
                resultItem.cargoType_Name = queryResult.CargoType_Name;
                resultItem.unloadingType_Index = queryResult.UnloadingType_Index;
                resultItem.unloadingType_Id = queryResult.UnloadingType_Id;
                resultItem.unloadingType_Name = queryResult.UnloadingType_Name;
                resultItem.containerType_Index = queryResult.ContainerType_Index;
                resultItem.containerType_Id = queryResult.ContainerType_Id;
                resultItem.containerType_Name = queryResult.ContainerType_Name;
                resultItem.container_No1 = queryResult.Container_No1;
                resultItem.container_No2 = queryResult.Container_No2;
                resultItem.labur = queryResult.Labur;
                resultItem.costCenter_Index = queryResult.CostCenter_Index;
                resultItem.costCenter_Id = queryResult.CostCenter_Id;
                resultItem.costCenter_Name = queryResult.CostCenter_Name;

                String Statue = "";

                Statue = queryResult.Document_Status.ToString();
                var ProcessStatusName = ProcessStatus.Where(c => c.processStatus_Id == Statue).FirstOrDefault();
                resultItem.processStatus_Name = ProcessStatusName.processStatus_Name;


                //var owner = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("autoOwnerFilter"), new { key = queryResult.Owner_Id }.sJson()).FirstOrDefault();
                //resultItem.ownerDocumentRef_No1 = owner.value1;
                //resultItem.ownerDocumentRef_No2 = owner.value7;
                //resultItem.ownerDocumentRef_No3 = owner.value3;

                //var Listdocuments = new List<document>();
                //var DocumentFile = db.im_DocumentFile.Where(c => c.Ref_Index == queryResult.PlanGoodsReceive_Index && c.DocumentFile_Status == 0).ToList();
                //foreach (var d in DocumentFile)
                //{
                //    var documents = new document();
                //    documents.index = d.DocumentFile_Index;
                //    documents.filename = d.DocumentFile_Name;
                //    documents.path = d.DocumentFile_Path;
                //    documents.urlAttachFile = d.DocumentFile_Url;
                //    Listdocuments.Add(documents);
                //}
                //resultItem.documents = Listdocuments;

                return resultItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Delete
        public Boolean Delete(PlanGoodsReceiveDocViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {
                var PlanGoodsReceive = db.IM_PlanGoodsReceive.Find(data.planGoodsReceive_Index);

                if (PlanGoodsReceive != null)
                {
                    PlanGoodsReceive.Document_Status = -1;
                    PlanGoodsReceive.Update_By = data.update_By;
                    PlanGoodsReceive.Update_Date = DateTime.Now;

                    var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
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

        #endregion

        #region ConfirmStatus
        public Boolean confirmStatus(PlanGoodsReceiveDocViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {

                var PlanGoodsReceive = db.IM_PlanGoodsReceive.Find(data.planGoodsReceive_Index);

                if (PlanGoodsReceive != null)
                {
                    PlanGoodsReceive.Document_Status = 1;
                    PlanGoodsReceive.Update_By = data.update_By;
                    PlanGoodsReceive.Update_Date = DateTime.Now;

                    //craete shipment TMS
                    var service = new PlanGRBusiness.Demo.DemoService(db);
                    var tmsresponse = service.Callback_TMS(data.planGoodsReceive_Index);
                    if (tmsresponse != "Success")
                    {
                        return false;
                    }
                    //craete shipment TMS

                    var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {
                        db.SaveChanges();
                        transaction.Commit();
                    }

                    catch (Exception exy)
                    {
                        msglog = State + " ex Rollback " + exy.Message.ToString();
                        olog.logging("confirmStatus", msglog);
                        transaction.Rollback();
                        throw exy;
                    }
                }

                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region closeDocument
        public Boolean closeDocument(PlanGoodsReceiveDocViewModel model)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {
                var PlanGoodsReceive = db.IM_PlanGoodsReceive.Find(model.planGoodsReceive_Index);

                if (PlanGoodsReceive != null)
                {
                    PlanGoodsReceive.Document_Status = 4;
                    PlanGoodsReceive.Update_By = model.update_By;
                    PlanGoodsReceive.Update_Date = DateTime.Now;

                    var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {
                        db.SaveChanges();
                        transaction.Commit();
                        return true;
                    }

                    catch (Exception exy)
                    {
                        msglog = State + " ex Rollback " + exy.Message.ToString();
                        olog.logging("closeDocumentPlanGR", msglog);
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

        #endregion

        #region updatePlanGRStatus
        public Boolean updatePlanGRStatus(PlanGoodsReceiveDocViewModel model)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {
                var PlanGoodsReceive = db.IM_PlanGoodsReceive.Find(model.planGoodsReceive_Index);

                if (PlanGoodsReceive != null)
                {
                    PlanGoodsReceive.Document_Status = model.document_Status;
                    PlanGoodsReceive.Update_By = model.update_By;
                    PlanGoodsReceive.Update_Date = DateTime.Now;

                    var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {
                        db.SaveChanges();
                        transaction.Commit();
                        return true;
                    }

                    catch (Exception exy)
                    {
                        msglog = State + " ex Rollback " + exy.Message.ToString();
                        olog.logging("closeDocumentPlanGR", msglog);
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

        #endregion

        #region CheckDocumentStatus
        public List<string> CheckDocumentStatus(CheckDocumentStatusViewModel model)
        {
            try
            {
                var res = new List<string>();

                var queryResult = db.View_CheckDocumentStatus.Where(c => c.PlanGoodsReceive_No == model.planGoodsReceive_No)
                                    .GroupBy(g => new { g.GRDocument_Status, g.PlanGRDocument_Status, g.PlanGoodsReceive_No })
                                    .Select(c => new { c.Key.GRDocument_Status, c.Key.PlanGRDocument_Status, c.Key.PlanGoodsReceive_No }).ToList();


                //ดูว่า Plan GR มี GR หรือยัง
                if (queryResult.Count > 0)
                {


                    var isValidateStatus = queryResult.Any(c => c.GRDocument_Status != 3 && c.GRDocument_Status != -99);

                    //ถ้าไม่มี GR Status เป็น 3 1ใบ และไม่ใช่ 3 1ใบ ปิดไม่ได้
                    if (isValidateStatus)
                    {

                        var data = queryResult.Where(c => c.GRDocument_Status != 3 && c.GRDocument_Status != -1).ToList();
                        if (data.Count > 0)
                        {
                            //var strwhere1 = new SqlParameter("@strwhere", " and Ref_Document_No in (" + docNo + ")");
                            //var chkGRItem = db.IM_GoodsReceiveItems.FromSql("sp_GetGoodsReceiveItem @strwhere ", strwhere1).ToList();
                            var chkGRItem = db.IM_GoodsReceiveItem.Where(c => c.Ref_Document_No == model.planGoodsReceive_No).ToList();

                            var Gritem = chkGRItem.Any(c => c.Document_Status == -1);
                            if (Gritem)
                            {
                                return res;
                            }
                            else
                            {
                                foreach (var d in data)
                                {

                                    if (d.PlanGRDocument_Status == 3)
                                    {
                                        res.Add(d.PlanGoodsReceive_No + " ทำการรับครบถ้วนแล้ว ไม่สามารถ Close เอกสารได้ ");
                                    }
                                    else
                                    {
                                        res.Add(d.PlanGoodsReceive_No + " มีการผูกเอกสารไปแล้ว และยังทำการรับสินค้าไม่เสร็จสิ้น ");
                                    }
                                }
                            }


                        }
                        else if (data.Count == 0)
                        {
                            var data1 = queryResult.Where(c => c.PlanGRDocument_Status == 2)
                                .GroupBy(g => new { g.PlanGRDocument_Status, g.PlanGoodsReceive_No })
                                .Select(c => new { c.Key.PlanGRDocument_Status, c.Key.PlanGoodsReceive_No }).ToList();
                            foreach (var d in data1)
                            {
                                res.Add(d.PlanGoodsReceive_No + " ทำการ Close เอกสารเรียบร้อยแล้ว ไม่สามารถ Close เอกสารได้อีก ");
                            }

                        }

                        return res;
                    }
                    //ถ้ามีแล้ว Status ต้องเป็น 3 ถึงจะปิดได้
                    else
                    {
                        isValidateStatus = queryResult.Any(c => c.PlanGRDocument_Status != 0);

                        if (isValidateStatus)
                        {
                            var data = queryResult.ToList();
                            foreach (var d in data)
                            {
                                switch (d.PlanGRDocument_Status)
                                {
                                    case 2:
                                        res.Add(d.PlanGoodsReceive_No + " ทำการ Close เอกสารเรียบร้อยแล้ว ไม่สามารถ Close เอกสารได้อีก ");
                                        break;
                                    case 3:
                                        res.Add(d.PlanGoodsReceive_No + " ทำการรับครบถ้วนแล้ว ไม่สามารถ Close เอกสารได้ ");
                                        break;
                                    case -1:
                                        res.Add(d.PlanGoodsReceive_No + " ทำการ Delete ไปแล้ว ไม่สามารถ Close เอกสารได้ ");
                                        break;
                                    default:
                                        break;
                                }
                            }

                            return res;
                        }

                        return new List<string>();
                    }
                }

                else if (queryResult.Count == 0)
                {
                    //var queryResult1 = db.IM_PlanGoodsReceive.FromSql("sp_GetPlanGoodsReceive @strwhere ", strwhere).ToList();
                    var queryResult1 = db.IM_PlanGoodsReceive.Where(c => c.PlanGoodsReceive_No == model.planGoodsReceive_No).ToList();

                    var data = queryResult1.ToList();
                    foreach (var d in data)
                    {
                        switch (d.Document_Status)
                        {
                            //case 1:
                            //    res.Add(d.PlanGoodsReceive_No + " เอกสารมีการ Confirm แล้ว ไม่สามารถ Close เอกสารได้ ");
                            //    break;
                            case 2:
                                res.Add(d.PlanGoodsReceive_No + " ทำการ Close เอกสารเรียบร้อยแล้ว ไม่สามารถ Close เอกสารได้อีก ");
                                break;
                            default:
                                break;
                        }
                    }


                    return res;
                }
                //}

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region updateUserAssign
        public String updateUserAssign(PlanGoodsReceiveDocViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {

                var PlanGoodsReceive = db.IM_PlanGoodsReceive.Find(data.planGoodsReceive_Index);

                if (PlanGoodsReceive != null)
                {
                    PlanGoodsReceive.UserAssign = data.userAssign;

                    var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {
                        db.SaveChanges();
                        transaction.Commit();
                    }

                    catch (Exception exy)
                    {
                        msglog = State + " ex Rollback " + exy.Message.ToString();
                        olog.logging("UpdateUserAssign", msglog);
                        transaction.Rollback();
                        throw exy;
                    }
                }

                var FindUser = db.IM_PlanGoodsReceive.Find(data.planGoodsReceive_Index);

                return FindUser.UserAssign.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region deleteUserAssign
        public String deleteUserAssign(PlanGoodsReceiveDocViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {
                if (!string.IsNullOrEmpty(data.planGoodsReceive_Index.ToString().Replace("00000000-0000-0000-0000-000000000000", "")))
                {
                    var PlanGoodsReceive = db.IM_PlanGoodsReceive.Find(data.planGoodsReceive_Index);

                    if (PlanGoodsReceive != null)
                    {
                        PlanGoodsReceive.UserAssign = "";

                        var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                        try
                        {
                            db.SaveChanges();
                            transaction.Commit();
                        }

                        catch (Exception exy)
                        {
                            msglog = State + " ex Rollback " + exy.Message.ToString();
                            olog.logging("deleteUserAssign", msglog);
                            transaction.Rollback();
                            throw exy;
                        }
                    }

                    var FindUser = db.IM_PlanGoodsReceive.Find(data.planGoodsReceive_Index);

                    return FindUser.UserAssign.ToString();
                }
                else
                {
                    return "";
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region PopupPlanGoodsIssuefilter
        public actionResultPlanGIPopupViewModel popupPlanGoodsIssuefilter(ReturnReceiveViewModel data)
        {
            try
            {

                string pwhereFilter = "";
                var actionResultPlanGIPopup = new actionResultPlanGIPopupViewModel();

                var result = new List<ReturnReceiveViewModel>();



                if (data.PlanGoodsIssueNo != "" && data.PlanGoodsIssueNo != null)
                {
                    pwhereFilter = " And PlanGoodsIssue_No like N'%" + data.PlanGoodsIssueNo + "%'";
                }
                else
                {
                    //pwhereFilter = " and CAST(PlanGoodsIssue_Due_Date as Date) = '" + DateTime.Today.AddDays(-10).ToString("yyyy-MM-dd") + "'";
                    //pwhereFilter += " and CAST(PlanGoodsIssue_Due_Date as Date) >= '" + DateTime.Today.AddDays(-10).ToString("yyyy-MM-dd") + "'" + " and CAST(PlanGoodsIssue_Due_Date as Date) <= '" + DateTime.Today.ToString("yyyy-MM-dd") + "'";
                    pwhereFilter += "";
                }

                if (data.OwnerName != "" && data.OwnerName != null)
                {
                    pwhereFilter += " And Owner_Name like N'%" + data.OwnerName + "%'";
                }
                else
                {
                    pwhereFilter += "";
                }

                //if (data.DocumentTypeName == "Return from customer")
                //{
                //    pwhereFilter += " AND PlanGoodsIssue_Index in (select a.Ref_Document_Index from im_TruckLoadItem a inner join im_TruckLoad b on a.TruckLoad_Index = b.TruckLoad_Index where b.Document_Status = 2)";
                //}
                var strwhere = new SqlParameter("@strwhere", pwhereFilter);
                var query = db.View_ReturnReceive.FromSql("sp_GetPlanGoodsIssueByPaginationPopup @strwhere", strwhere).ToList();

                var perpages = data.PerPage == 0 ? query.ToList() : query.Skip((data.CurrentPage - 1) * data.PerPage).Take(data.PerPage).ToList();


                //var strwhere = new SqlParameter("@strwhere", pwhereFilter);
                //var PageNumber = new SqlParameter("@PageNumber", 1);
                //var RowspPage = new SqlParameter("@RowspPage", 1000);

                //var queryResultTotal = context.View_ReturnReceive.FromSql("sp_GetPlanGoodsIssueByPaginationPopup @strwhere , @PageNumber , @RowspPage ", strwhere, PageNumber, RowspPage).ToList();

                //var strwhere1 = new SqlParameter("@strwhere", pwhereFilter);
                //var PageNumber1 = new SqlParameter("@PageNumber", data.CurrentPage);
                //var RowspPage1 = new SqlParameter("@RowspPage", data.PerPage);
                //var query = context.View_ReturnReceive.FromSql("sp_GetPlanGoodsIssueByPaginationPopup @strwhere , @PageNumber , @RowspPage ", strwhere1, PageNumber1, RowspPage1).ToList();

                foreach (var item in perpages)
                {
                    var resultItem = new ReturnReceiveViewModel();

                    resultItem.PlanGoodsIssueIndex = item.PlanGoodsIssue_Index;
                    resultItem.PlanGoodsIssueNo = item.PlanGoodsIssue_No;
                    resultItem.PlanGoodsIssueDate = item.PlanGoodsIssue_Date.toString();
                    resultItem.PlanGoodsIssueDueDate = item.PlanGoodsIssue_Due_Date.toString();
                    resultItem.OwnerIndex = item.Owner_Index;
                    resultItem.OwnerId = item.Owner_Id;
                    resultItem.OwnerName = item.Owner_Name;
                    resultItem.DocumentRefNo1 = item.DocumentRef_No1;
                    resultItem.DocumentStatus = item.Document_Status;
                    resultItem.WarehouseIndex = item.Warehouse_Index;
                    resultItem.WarehouseIndexTo = item.Warehouse_Index_To;
                    resultItem.WarehouseId = item.Warehouse_Id;
                    resultItem.WarehouseIdTo = item.Warehouse_Id_To;
                    resultItem.WarehouseName = item.Warehouse_Name;
                    resultItem.WarehouseNameTo = item.Warehouse_Name_To;
                    resultItem.CreateDate = item.Create_Date.toString();
                    resultItem.CreateBy = item.Create_By;
                    resultItem.UpdateDate = item.Update_Date.toString();
                    resultItem.UpdateBy = item.Update_By;
                    resultItem.CancelDate = item.Cancel_Date.toString();
                    resultItem.CancelBy = item.Cancel_By;
                    resultItem.RefPlanGoodsIssueNo = item.Ref_PlanGoodsIssue_No;
                    result.Add(resultItem);
                }
                var count = query.Count;
                actionResultPlanGIPopup = new actionResultPlanGIPopupViewModel();
                actionResultPlanGIPopup.itemsPlanGI = result.ToList();
                actionResultPlanGIPopup.pagination = new Pagination() { TotalRow = count, CurrentPage = data.CurrentPage, PerPage = data.PerPage };




                return actionResultPlanGIPopup;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region PopupPlanGRfilter
        public List<PlanGoodsReceiveAutoViewModel> popupPlanGRfilter(PlanGoodsReceiveAutoViewModel data)
        {
            try
            {

                var items = new List<PlanGoodsReceiveAutoViewModel>();

                //PlanGR popup GRCreate Page
                if (data.chk == "2")
                {

                    var query = db.View_GetPlanGoodsReceive_Popup.AsQueryable();

                    if (!string.IsNullOrEmpty(data.planGoodsReceive_No))
                    {
                        query = query.Where(c => c.PlanGoodsReceive_No.Contains(data.planGoodsReceive_No));
                    }
                    else if (!string.IsNullOrEmpty(data.vendor_Name))
                    {
                        query = query.Where(c => c.Vendor_Name.Contains(data.vendor_Name));
                    }
                    else if (!string.IsNullOrEmpty(data.planGoodsReceive_Date))
                    {
                        query = query.Where(c => c.PlanGoodsReceive_Date.toString().Contains(data.planGoodsReceive_Date));
                    }
                    else if (!string.IsNullOrEmpty(data.planGoodsReceive_Due_Date))
                    {
                        query = query.Where(c => c.PlanGoodsReceive_Due_Date.toString().Contains(data.planGoodsReceive_Due_Date));
                    }
                    else if (!string.IsNullOrEmpty(data.owner_Name))
                    {
                        query = query.Where(c => c.Owner_Name.Contains(data.owner_Name));
                    }
                    else if (!string.IsNullOrEmpty(data.owner_Index.ToString()))
                    {
                        query = query.Where(c => c.Owner_Index == data.owner_Index);
                    }
                    if (data.documentType_Index.ToString() == "713a4e87-30fb-4750-bf9d-6995e28e71eb")
                    {
                        query = query.Where(c => c.DocumentType_Index.ToString() == "774cf194-b35b-45e7-b873-3e8453e257fd");
                    }
                    else
                    {
                        query = query.Where(c => c.DocumentType_Index.ToString() != "774cf194-b35b-45e7-b873-3e8453e257fd");
                    }
                    query = query.Where(c => c.Document_Status == 1);


                    var result = query.ToList();

                    foreach (var item in result)
                    {
                        var ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),DocumentType_Index_To)");
                        var ColumnName2 = new SqlParameter("@ColumnName2", "DocumentType_Id_To");
                        var ColumnName3 = new SqlParameter("@ColumnName3", "DocumentType_Name_To");
                        var ColumnName4 = new SqlParameter("@ColumnName4", "''");
                        var ColumnName5 = new SqlParameter("@ColumnName5", "''");
                        var TableName = new SqlParameter("@TableName", "sy_DocumentTypeRef");
                        var Where = new SqlParameter("@Where", "Where DocumentType_Index = '" + item.DocumentType_Index + "'");
                        var DataDocumentTypeRef = db.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).FirstOrDefault();
                        var resultItem = new PlanGoodsReceiveAutoViewModel
                        {
                            planGoodsReceive_Index = item.PlanGoodsReceive_Index,
                            planGoodsReceive_No = item.PlanGoodsReceive_No,
                            planGoodsReceive_Date = item.PlanGoodsReceive_Date.toString(),
                            planGoodsReceive_Due_Date = item.PlanGoodsReceive_Due_Date.toString(),
                            vendor_Index = item.Vendor_Index,
                            vendor_Id = item.Vendor_Id,
                            vendor_Name = item.Vendor_Name,
                            owner_Index = item.Owner_Index,
                            owner_Id = item.Owner_Id,
                            owner_Name = item.Owner_Name,
                            documentRef_No1 = item.DocumentRef_No1,
                            document_Status = item.Document_Status,
                            warehouse_Index = item.Warehouse_Index,
                            warehouse_Index_To = item.Warehouse_Index_To,
                            warehouse_Id = item.Warehouse_Id,
                            warehouse_Id_To = item.Warehouse_Id_To,
                            warehouse_Name = item.Warehouse_Name,
                            warehouse_Name_To = item.Warehouse_Name_To,
                            documentType_Index = item.DocumentType_Index,
                            documentType_Id = item.DocumentType_Id,
                            documentType_Name = item.DocumentType_Name,
                            document_Remark = item.Document_Remark,
                            //grDocumentType_Index = new Guid(DataDocumentTypeRef.dataincolumn1),
                            //grDocumentType_Id = DataDocumentTypeRef.dataincolumn2,
                            //grDocumentType_Name = DataDocumentTypeRef.dataincolumn3
                        };
                        items.Add(resultItem);
                    }

                }
                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #region PopupProductConversionfilter
        public List<ProductConversionViewModelDoc> popupProductConversionfilter(ProductConversionViewModelDoc data)
        {
            try
            {
                var result = new List<ProductConversionViewModelDoc>();

                var filterModel = new ProductConversionViewModelDoc();

                if (!string.IsNullOrEmpty(data.productConversion_Id))
                {
                    filterModel.productConversion_Id = data.productConversion_Id;
                }

                if (!string.IsNullOrEmpty(data.productConversion_Name))
                {
                    filterModel.productConversion_Name = data.productConversion_Name;
                }

                if (data.product_Index != null && data.product_Index.ToString() != "00000000-0000-0000-0000-000000000000")
                {
                    filterModel.product_Index = data.product_Index;
                }

                if (data.productConversion_Index != null && data.productConversion_Index.ToString() != "00000000-0000-0000-0000-000000000000")
                {
                    filterModel.productConversion_Index = data.productConversion_Index;
                }

                //GetConfig
                result = utils.SendDataApi<List<ProductConversionViewModelDoc>>(new AppSettingConfig().GetUrl("ProductConversionfilter"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AutobasicSuggestion
        public List<ItemListViewModel> autobasicSuggestion(ItemListViewModel data)
        {
            var items = new List<ItemListViewModel>();
            try
            {
                if (!string.IsNullOrEmpty(data.key))
                {
                    var query1 = db.View_PlanGrProcessStatus.Where(c => c.PlanGoodsReceive_No.Contains(data.key)).Select(s => new ItemListViewModel
                    {
                        name = s.PlanGoodsReceive_No,
                        key = s.PlanGoodsReceive_No
                    }).Distinct();

                    var query2 = db.View_PlanGrProcessStatus.Where(c => c.Owner_Name.Contains(data.key)).Select(s => new ItemListViewModel
                    {
                        name = s.Owner_Name,
                        key = s.Owner_Name
                    }).Distinct();

                    //var query3 = db.View_PlanGrProcessStatus.Where(c => c.Vendor_Name.Contains(data.key)).Select(s => new ItemListViewModel
                    //{
                    //    name = s.Vendor_Name,
                    //    key = s.Vendor_Name

                    //}).Distinct();

                    var query = query1.Union(query2).Union(query2);

                    items = query.OrderBy(c => c.name).Take(10).ToList();
                }

            }
            catch (Exception ex)
            {

            }

            return items;
        }

        #endregion


        #region AutobasicSuggestion
        public List<ItemListViewModel> autobasicSuggestionPO(ItemListViewModel data)
        {
            var query = new List<ItemListViewModel>();
            try
            {
                if (!string.IsNullOrEmpty(data.key))
                {
                    var query1 = db.IM_PlanGoodsReceive.Where(c => c.PlanGoodsReceive_No.Contains(data.key) && !new List<int?> { -1, -2 }.Contains(c.Document_Status)).OrderBy(o => o.PlanGoodsReceive_No).Select(s => new ItemListViewModel
                    {
                        name = s.PlanGoodsReceive_No,
                        key = s.PlanGoodsReceive_No
                    }).Distinct();


                    query = query1.Take(10).ToList();

                }

            }
            catch (Exception ex)
            {

            }

            return query;
        }

        #endregion


        #region AutobasicSuggestion
        public List<ItemListViewModel> autobasicSuggestionVender(ItemListViewModel data)
        {
            var items = new List<ItemListViewModel>();
            try
            {
                if (!string.IsNullOrEmpty(data.key))
                {
                    List<int?> status = new List<int?> { -1, -2 };
                    var query1 = db.IM_PlanGoodsReceive.Where(c => c.Vendor_Name.Contains(data.key) || c.Vendor_Id.Contains(data.key) && !new List<int?> { -1, -2 }.Contains(c.Document_Status)).Select(s => new ItemListViewModel
                    {
                        name = s.Vendor_Name,
                        id = s.Vendor_Id,
                        index = s.Vendor_Index
                    }).Distinct();


                    //var query3 = db.View_PlanGrProcessStatus.Where(c => c.Vendor_Name.Contains(data.key)).Select(s => new ItemListViewModel
                    //{
                    //    name = s.Vendor_Name,
                    //    key = s.Vendor_Name

                    //}).Distinct();

                    var query = query1;

                    items = query.OrderBy(c => c.name).Take(10).ToList();
                }

            }
            catch (Exception ex)
            {

            }

            return items;
        }

        #endregion

        #region AutoOwnerfilter
        public List<ItemListViewModel> autoOwnerfilter(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();

                var filterModel = new ItemListViewModel();
                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key = data.key;
                }

                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("autoOwnerFilter"), filterModel.sJson());
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AutoVenderfilter
        public List<ItemListViewModel> autoVenderfilter(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();

                var filterModel = new ItemListViewModel();

                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key = data.key;
                }

                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("autoVendorFilter"), filterModel.sJson());

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AutoStatusfilter
        public List<ItemListViewModel> autoStatusfilter(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();


                var filterModel = new ItemListViewModel();

                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key = data.key;
                }
                if (data.chk != null)
                {
                    filterModel.chk = data.chk;
                }

                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("autoStatusFilter"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AutoWarehousefilter
        public List<ItemListViewModel> autoWarehousefilter(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();

                var filterModel = new ItemListViewModel();
                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key = data.key;
                }

                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("autoWarehousefilter"), filterModel.sJson());
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AutoDocumentTypefilter
        public List<ItemListViewModel> autoDocumentTypefilter(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();

                var filterModel = new ItemListViewModel();

                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key = data.key;
                }

                filterModel.index = new Guid("C2A3F847-BAA6-46FE-B502-44F2D5826A1C");


                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("autoDocumentTypefilter"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AutoPlanGoodsReceiveNo
        public List<ItemListViewModel> autoPlanGoodsReceiveNo(ItemListViewModel data)
        {
            try
            {
                var query = db.View_PlanGrProcessStatus.AsQueryable();

                if (data.key == "-")
                {


                }
                else if (!string.IsNullOrEmpty(data.key))
                {
                    query = query.Where(c => c.PlanGoodsReceive_No.Contains(data.key));

                }

                //if (!string.IsNullOrEmpty(data.key))
                //{
                //    query = query.Where(c => c.PlanGoodsReceive_No.Contains(data.key));

                //}

                var items = new List<ItemListViewModel>();

                var result = query.Select(c => new { c.PlanGoodsReceive_Index, c.PlanGoodsReceive_No }).Distinct().Take(10).ToList();


                foreach (var item in result)
                {
                    var resultItem = new ItemListViewModel
                    {
                        index = item.PlanGoodsReceive_Index,
                        name = item.PlanGoodsReceive_No
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

        #endregion

        #region AutoPlanGRFilter
        public List<PlanGoodsReceiveAutoViewModel> autoPlanGRFilter(PlanGoodsReceiveAutoViewModel data)
        {
            try
            {
                var query = db.View_PlanGrProcessStatus.AsQueryable();

                if (!string.IsNullOrEmpty(data.planGoodsReceive_No))
                {
                    query = query.Where(c => c.PlanGoodsReceive_No.Contains(data.planGoodsReceive_No));

                }

                var items = new List<PlanGoodsReceiveAutoViewModel>();

                var result = query.Select(c => new { c.PlanGoodsReceive_Index, c.PlanGoodsReceive_No }).Distinct().Take(10).ToList();


                foreach (var item in result)
                {
                    var resultItem = new PlanGoodsReceiveAutoViewModel
                    {
                        planGoodsReceive_Index = item.PlanGoodsReceive_Index,
                        planGoodsReceive_No = item.PlanGoodsReceive_No
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

        #endregion

        #region AutoUser
        public List<ItemListViewModel> autoUser(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();


                var filterModel = new ItemListViewModel();

                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key = data.key;
                }


                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("autoUserfilter"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AutoSku
        public List<ItemListViewModel> autoSkufilter(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();


                var filterModel = new ItemListViewModel();

                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key = data.key;
                }
                if (!string.IsNullOrEmpty(data.key2))
                {
                    filterModel.key2 = data.key2;
                }
                else
                {
                    filterModel.key2 = "00000000-0000-0000-0000-000000000000";
                }

                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("autoSkufilter"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AutoProduct
        public List<ItemListViewModel> autoProductfilter(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();


                var filterModel = new ItemListViewModel();

                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key = data.key;
                }
                if (!string.IsNullOrEmpty(data.key2))
                {
                    filterModel.key2 = data.key2;
                }
                else
                {
                    filterModel.key2 = "00000000-0000-0000-0000-000000000000";
                }

                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("autoProductfilter"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #region DropdownDocumentType
        public List<DocumentTypeViewModel> DropdownDocumentType(DocumentTypeViewModel data)
        {
            try
            {
                var result = new List<DocumentTypeViewModel>();

                var filterModel = new DocumentTypeViewModel();


                filterModel.process_Index = new Guid("C2A3F847-BAA6-46FE-B502-44F2D5826A1C");

                //GetConfig
                result = utils.SendDataApi<List<DocumentTypeViewModel>>(new AppSettingConfig().GetUrl("dropDownDocumentType"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region DropdownStatus
        public List<ProcessStatusViewModel> dropdownStatus(ProcessStatusViewModel data)
        {
            try
            {
                var result = new List<ProcessStatusViewModel>();

                var filterModel = new ProcessStatusViewModel();


                filterModel.process_Index = new Guid("C2A3F847-BAA6-46FE-B502-44F2D5826A1C");

                //GetConfig
                result = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("dropdownStatus"), filterModel.sJson());


                //var resultStatus = result.Where(c => c.processStatus_Id.Contains("0") || c.processStatus_Id.Contains("1") || c.processStatus_Id.Contains("-1") || c.processStatus_Id.Contains("-2")).ToList();
                var resultStatus = result.Where(c => c.processStatus_Id == "0" || c.processStatus_Id == "1" || c.processStatus_Id == "-1" || c.processStatus_Id == "-2").ToList();

                return resultStatus;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region DropdownWarehouse
        public List<warehouseDocViewModel> dropdownWarehouse(warehouseDocViewModel data)
        {
            try
            {
                var result = new List<warehouseDocViewModel>();

                var filterModel = new warehouseDocViewModel();

                //GetConfig
                result = utils.SendDataApi<List<warehouseDocViewModel>>(new AppSettingConfig().GetUrl("dropdownWarehouse"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region DropdownRound
        public List<roundDocViewModel> dropdownRound(roundDocViewModel data)
        {
            try
            {
                var result = new List<roundDocViewModel>();

                var filterModel = new roundDocViewModel();

                //GetConfig
                result = utils.SendDataApi<List<roundDocViewModel>>(new AppSettingConfig().GetUrl("dropdownRound"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region DropdownProductconversion
        public List<ProductConversionViewModelDoc> dropdownProductconversion(ProductConversionViewModelDoc data)
        {
            try
            {
                var result = new List<ProductConversionViewModelDoc>();

                var filterModel = new ProductConversionViewModelDoc();

                if (!string.IsNullOrEmpty(data.product_Index.ToString()))
                {
                    filterModel.product_Index = data.product_Index;
                }
                //GetConfig
                result = utils.SendDataApi<List<ProductConversionViewModelDoc>>(new AppSettingConfig().GetUrl("dropdownProductconversion"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region dropdownItemStatus
        public List<ItemStatusDocViewModel> dropdownItemStatus(ItemStatusDocViewModel data)
        {
            try
            {
                var result = new List<ItemStatusDocViewModel>();

                var filterModel = new ItemStatusDocViewModel();

                //GetConfig
                result = utils.SendDataApi<List<ItemStatusDocViewModel>>(new AppSettingConfig().GetUrl("dropdownItemStatus"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region dropdownVehicle
        public List<VehicleViewModel> dropdownVehicle(VehicleViewModel data)
        {
            try
            {
                var result = new List<VehicleViewModel>();

                var filterModel = new VehicleViewModel();

                //GetConfig
                result = utils.SendDataApi<List<VehicleViewModel>>(new AppSettingConfig().GetUrl("dropdownVehicle"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region dropdownTransport
        public List<TransportViewModel> dropdownTransport(TransportViewModel data)
        {
            try
            {
                var result = new List<TransportViewModel>();

                var filterModel = new TransportViewModel();

                //GetConfig
                result = utils.SendDataApi<List<TransportViewModel>>(new AppSettingConfig().GetUrl("dropdownTransport"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region ScanPlanGR
        public List<ScanPlanGoodsReceiveViewModel> ScanPlanGR(ScanPlanGoodsReceiveViewModel data)
        {
            try
            {
                var query = db.IM_PlanGoodsReceive.AsQueryable();

                if (!string.IsNullOrEmpty(data.planGoodsReceive_No))
                {
                    //query = query.Where(c => c.PlanGoodsReceive_No.Contains(data.planGoodsReceive_No));
                    query = query.Where(c => c.PlanGoodsReceive_No == data.planGoodsReceive_No && c.Document_Status != -1);

                }

                var items = new List<ScanPlanGoodsReceiveViewModel>();

                var result = query.ToList();


                foreach (var item in result)
                {
                    var resultItem = new ScanPlanGoodsReceiveViewModel
                    {
                        planGoodsReceive_Index = item.PlanGoodsReceive_Index,
                        planGoodsReceive_No = item.PlanGoodsReceive_No,
                        planGoodsReceive_Date = item.PlanGoodsReceive_Date.toString(),
                        planGoodsReceive_Due_Date = item.PlanGoodsReceive_Due_Date.toString(),
                        document_Status = item.Document_Status,
                        documentType_Index = item.DocumentType_Index,
                        documentType_Name = item.DocumentType_Name,
                        documentType_Id = item.DocumentType_Id,
                        owner_Index = item.Owner_Index,
                        owner_Id = item.Owner_Id,
                        owner_Name = item.Owner_Name,
                        vendor_Index = item.Vendor_Index,
                        vendor_Id = item.Vendor_Id,
                        vendor_Name = item.Vendor_Name,
                        documentRef_No1 = item.DocumentRef_No1,
                        documentRef_No2 = item.DocumentRef_No2,
                        documentRef_No3 = item.DocumentRef_No3,
                        documentRef_No4 = item.DocumentRef_No4,
                        documentRef_No5 = item.DocumentRef_No5,
                        warehouse_Index = item.Warehouse_Index,
                        warehouse_Id = item.Warehouse_Id,
                        warehouse_Name = item.Warehouse_Name,
                        warehouse_Index_To = item.Warehouse_Index_To,
                        warehouse_Id_To = item.Warehouse_Id_To,
                        warehouse_Name_To = item.Warehouse_Name_To,
                        create_Date = item.Create_Date.toString(),
                        create_By = item.Create_By,
                        update_Date = item.Update_Date.toString(),
                        update_By = item.Update_By,
                        cancel_Date = item.Cancel_Date.toString(),
                        cancel_By = item.Cancel_By
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

        #endregion

        #region getScanPlanGRI
        public List<PlanGoodsReceiveItemViewModel> getScanPlanGRI(PlanGoodsReceiveItemViewModel data)
        {
            try
            {
                var query = db.View_GetScanPlanGRI.AsQueryable();

                if (!string.IsNullOrEmpty(data.planGoodsReceive_Index.ToString()) && data.planGoodsReceive_Index.ToString() != "00000000-0000-0000-0000-000000000000")
                {
                    query = query.Where(c => c.PlanGoodsReceive_Index == data.planGoodsReceive_Index);

                }
                if (!string.IsNullOrEmpty(data.product_Index.ToString()) && data.product_Index.ToString() != "00000000-0000-0000-0000-000000000000")
                {
                    query = query.Where(c => c.Product_Index == data.product_Index);

                }

                var items = new List<PlanGoodsReceiveItemViewModel>();

                var result = query.ToList();


                foreach (var item in result)
                {
                    var resultItem = new PlanGoodsReceiveItemViewModel
                    {
                        planGoodsReceive_Index = item.PlanGoodsReceive_Index,
                        planGoodsReceiveItem_Index = item.PlanGoodsReceiveItem_Index,
                        lineNum = item.LineNum,
                        product_Index = item.Product_Index,
                        product_Id = item.Product_Id,
                        product_Name = item.Product_Name,
                        product_SecondName = item.Product_SecondName,
                        product_ThirdName = item.Product_ThirdName,
                        product_Lot = item.Product_Lot,
                        itemStatus_Index = item.ItemStatus_Index,
                        itemStatus_Id = item.ItemStatus_Id,
                        itemStatus_Name = item.ItemStatus_Name,
                        qty = item.Qty,
                        ratio = item.Ratio,
                        totalQty = item.TotalQty,
                        productConversion_Index = item.ProductConversion_Index,
                        productConversion_Id = item.ProductConversion_Id,
                        productConversion_Name = item.ProductConversion_Name,
                        mFG_Date = item.MFG_Date,
                        eXP_Date = item.EXP_Date,
                        unitWeight = item.UnitWeight,
                        weight = item.Weight,
                        unitWidth = item.UnitWidth,
                        unitLength = item.UnitLength,
                        unitHeight = item.UnitHeight,
                        unitVolume = item.UnitVolume,
                        volume = item.Volume,
                        unitPrice = item.UnitPrice,
                        price = item.Price,
                        documentRef_No1 = item.DocumentRef_No1,
                        documentRef_No2 = item.DocumentRef_No2,
                        documentRef_No3 = item.DocumentRef_No3,
                        documentRef_No4 = item.DocumentRef_No4,
                        documentRef_No5 = item.DocumentRef_No5,
                        document_Status = item.Document_Status,
                        documentItem_Remark = item.DocumentItem_Remark,
                        uDF_1 = item.UDF_1,
                        uDF_2 = item.UDF_2,
                        uDF_3 = item.UDF_3,
                        uDF_4 = item.UDF_4,
                        uDF_5 = item.UDF_5,
                        create_By = item.Create_By,
                        create_Date = item.Create_Date,
                        update_By = item.Update_By,
                        update_Date = item.Update_Date,
                        cancel_By = item.Cancel_By,
                        cancel_Date = item.Cancel_Date

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

        #endregion

        #region popupPlanGRIfilter
        public List<PlanGoodsReceiveItemViewModel> popupPlanGRIfilter(PlanGoodsReceiveItemViewModel data)
        {
            try
            {

                var items = new List<PlanGoodsReceiveItemViewModel>();



                var query = db.IM_PlanGoodsReceiveItem.AsQueryable();

                if (!string.IsNullOrEmpty(data.planGoodsReceive_Index.ToString()))
                {
                    query = query.Where(c => c.PlanGoodsReceive_Index == data.planGoodsReceive_Index);
                }
                if (!string.IsNullOrEmpty(data.product_Index.ToString()))
                {
                    query = query.Where(c => c.Product_Index == data.product_Index);
                }



                var result = query.ToList();

                foreach (var item in result)
                {

                    var resultItem = new PlanGoodsReceiveItemViewModel
                    {
                        planGoodsReceive_Index = item.PlanGoodsReceive_Index,
                        planGoodsReceiveItem_Index = item.PlanGoodsReceiveItem_Index,
                        product_Index = item.Product_Index,
                        product_Id = item.Product_Id,
                        product_Name = item.Product_Name,
                        product_SecondName = item.Product_SecondName,
                        product_ThirdName = item.Product_ThirdName,
                        product_Lot = item.Product_Lot,
                        productConversion_Id = item.ProductConversion_Id,
                        productConversion_Index = item.ProductConversion_Index,
                        productConversion_Name = item.ProductConversion_Name,
                        ratio = item.Ratio,
                        qty = item.Qty,
                        weight = item.Weight,
                        volume = item.Volume,
                        totalQty = item.TotalQty,
                        itemStatus_Index = item.ItemStatus_Index,
                        itemStatus_Name = item.ItemStatus_Name,
                        itemStatus_Id = item.ItemStatus_Id,
                        create_Date = item.Create_Date.GetValueOrDefault(),
                        create_By = item.Create_By,
                        update_Date = item.Update_Date.GetValueOrDefault(),
                        update_By = item.Update_By,
                        cancel_Date = item.Cancel_Date.GetValueOrDefault(),
                        cancel_By = item.Cancel_By,
                        lineNum = item.LineNum,
                        document_Status = item.Document_Status
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

        #endregion

        #region GetPlanGRIfilter
        public List<PopupPlanGoodsReceiveViewModel> GetPlanGRIfilter(PopupPlanGoodsReceiveViewModel data)
        {
            try
            {

                var items = new List<PopupPlanGoodsReceiveViewModel>();



                var query = db.View_GetPlanGoodsReceiveItem.AsQueryable();


                if (!string.IsNullOrEmpty(data.owner_Index.ToString()))
                {
                    query = query.Where(c => c.Owner_Index == data.owner_Index);
                }

                if (data.id.Count > 1)
                {
                    var G = new List<Guid>();

                    data.id.ForEach(c => G.Add(new Guid(c.Replace("'", ""))));



                    query = query.Where(c => G.Contains(c.PlanGoodsReceive_Index));
                }
                else if (!string.IsNullOrEmpty(data.planGoodsReceive_Index.ToString()))
                {
                    query = query.Where(c => c.PlanGoodsReceive_Index == data.planGoodsReceive_Index);
                }




                var result = query.ToList();

                foreach (var item in result)
                {
                    var resultProductconversion = new List<ProductConversionViewModelDoc>();

                    var resultProductDetail = new List<ProductViewModel>();
                    var filterModel = new ProductConversionViewModelDoc();

                    if (!string.IsNullOrEmpty(item.Product_Index.ToString()))
                    {
                        filterModel.product_Index = item.Product_Index.GetValueOrDefault();
                    }

                    if (!string.IsNullOrEmpty(item.ProductConversion_Index.ToString()))
                    {
                        filterModel.productConversion_Index = item.ProductConversion_Index.GetValueOrDefault();
                    }
                    //GetConfig
                    //resultProductconversion = utils.SendDataApi<List<ProductConversionViewModelDoc>>(new AppSettingConfig().GetUrl("dropdownProductconversion"), filterModel.sJson());
                    resultProductDetail = utils.SendDataApi<List<ProductViewModel>>(new AppSettingConfig().GetUrl("product"), filterModel.sJson());

                    var resultItem = new PopupPlanGoodsReceiveViewModel();

                    resultItem.planGoodsReceive_Index = item.PlanGoodsReceive_Index;
                    resultItem.planGoodsReceiveItem_Index = item.PlanGoodsReceiveItem_Index;
                    resultItem.planGoodsReceive_No = item.PlanGoodsReceive_No;
                    resultItem.product_Index = item.Product_Index;
                    resultItem.lineNum = item.LineNum;
                    resultItem.product_Id = item.Product_Id;
                    resultItem.product_Name = item.Product_Name;
                    resultItem.product_SecondName = item.Product_SecondName;
                    resultItem.product_ThirdName = item.Product_ThirdName;
                    resultItem.product_Lot = (resultProductDetail.FirstOrDefault().isLot == 1) ? item.Product_Lot : "";
                    resultItem.owner_Index = item.Owner_Index;
                    resultItem.qty = item.Qty.GetValueOrDefault();
                    resultItem.ratio = item.Ratio.GetValueOrDefault();
                    resultItem.totalQty = item.TotalQty;
                    resultItem.productConversion_Index = item.ProductConversion_Index;
                    resultItem.productConversion_Id = item.ProductConversion_Id;
                    resultItem.productConversion_Name = item.ProductConversion_Name;
                    resultItem.mfg_Date = (resultProductDetail.FirstOrDefault().isMfgDate == 1) ? item.MFG_Date.toString() : "";
                    resultItem.exp_Date = (resultProductDetail.FirstOrDefault().isExpDate == 1) ? item.EXP_Date.toString() : "";

                    resultItem.unitWeight = item.UnitWeight;
                    resultItem.weight = item.Weight;
                    resultItem.weight_Index = item.Weight_Index;
                    resultItem.weight_Id = item.Weight_Id;
                    resultItem.weight_Name = item.Weight_Name;
                    resultItem.weightRatio = item.WeightRatio;
                    resultItem.netWeight = item.NetWeight;
                    resultItem.unitGrsWeight = item.UnitGrsWeight;
                    resultItem.grsWeightRatio = item.GrsWeightRatio;
                    resultItem.grsWeight = item.GrsWeight;
                    resultItem.grsWeight_Index = item.GrsWeight_Index;
                    resultItem.grsWeight_Id = item.GrsWeight_Id;
                    resultItem.grsWeight_Name = item.GrsWeight_Name;
                    resultItem.unitWidth = item.UnitWidth;
                    resultItem.widthRatio = item.WidthRatio;
                    resultItem.width = item.Width;
                    resultItem.width_Index = item.Width_Index;
                    resultItem.width_Id = item.Width_Id;
                    resultItem.width_Name = item.Width_Name;
                    resultItem.unitLength = item.UnitLength;
                    resultItem.lengthRatio = item.LengthRatio;
                    resultItem.length = item.Length;
                    resultItem.length_Index = item.Length_Index;
                    resultItem.length_Id = item.Length_Id;
                    resultItem.length_Name = item.Length_Name;
                    resultItem.unitHeight = item.UnitHeight;
                    resultItem.heightRatio = item.HeightRatio;
                    resultItem.height = item.Height;
                    resultItem.height_Index = item.Height_Index;
                    resultItem.height_Id = item.Height_Id;
                    resultItem.height_Name = item.Height_Name;
                    if (item.UnitVolume == 0 || item.UnitVolume == null)
                    {
                        resultItem.unitVolume = 0;
                    }
                    else
                    {
                        resultItem.unitVolume = item.UnitVolume;

                    }

                    if (item.Volume == 0 || item.Volume == null)
                    {
                        resultItem.volume = 0;
                    }
                    else
                    {
                        resultItem.volume = item.Volume;
                    }

                    resultItem.unitPrice = item.UnitPrice;
                    resultItem.price = item.Price;
                    resultItem.totalPrice = item.TotalQty;

                    resultItem.currency_Index = item.Currency_Index;
                    resultItem.currency_Id = item.Currency_Id;
                    resultItem.currency_Name = item.Currency_Name;

                    resultItem.ref_Code1 = item.Ref_Code1;
                    resultItem.ref_Code2 = item.Ref_Code2;
                    resultItem.ref_Code3 = item.Ref_Code3;
                    resultItem.ref_Code4 = item.Ref_Code4;
                    resultItem.ref_Code5 = item.Ref_Code5;


                    resultItem.documentRef_No1 = item.DocumentRef_No1;
                    resultItem.documentRef_No2 = item.DocumentRef_No2;
                    resultItem.documentRef_No3 = item.DocumentRef_No3;
                    resultItem.documentRef_No4 = item.DocumentRef_No4;
                    resultItem.documentRef_No5 = item.DocumentRef_No5;
                    resultItem.document_Status = item.Document_Status;
                    resultItem.documentItem_Remark = item.DocumentItem_Remark;
                    resultItem.udf_1 = item.UDF_1;
                    resultItem.udf_2 = item.UDF_2;
                    resultItem.udf_3 = item.UDF_3;
                    resultItem.udf_4 = item.UDF_4;
                    resultItem.udf_5 = item.UDF_5;
                    resultItem.erp_Location = item.ERP_Location;
                    resultItem.isLot = resultProductDetail.FirstOrDefault().isLot;
                    resultItem.isMfgDate = resultProductDetail.FirstOrDefault().isMfgDate;
                    resultItem.isExpDate = resultProductDetail.FirstOrDefault().isExpDate;
                    resultItem.mfgDate_Default = DateTime.Now.toString();
                    resultItem.expDate_Default = (resultProductDetail.FirstOrDefault().productItemLife_D > 0) ? DateTime.Now.AddDays(Convert.ToDouble(resultProductDetail.FirstOrDefault().productItemLife_D)).toString() : "";
                    items.Add(resultItem);

                }


                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetPlanGRIPendingfilter
        public List<PopupPlanGoodsReceiveViewModel> GetPlanGRIPendingfilter(PopupPlanGoodsReceiveViewModel data)
        {
            try
            {

                var items = new List<PopupPlanGoodsReceiveViewModel>();



                var query = db.View_PlanGoodsReceiveItem.AsQueryable();


                if (!string.IsNullOrEmpty(data.owner_Index.ToString()))
                {
                    query = query.Where(c => c.Owner_Index == data.owner_Index);
                }
                if (data.id.Count > 1)
                {
                    var G = new List<Guid>();

                    data.id.ForEach(c => G.Add(new Guid(c.Replace("'", ""))));



                    query = query.Where(c => G.Contains(c.PlanGoodsReceive_Index));
                }
                else if (!string.IsNullOrEmpty(data.planGoodsReceive_Index.ToString()))
                {
                    query = query.Where(c => c.PlanGoodsReceive_Index == data.planGoodsReceive_Index);
                }



                var result = query.ToList();

                foreach (var item in result)
                {
                    var resultProductconversion = new List<ProductConversionViewModelDoc>();

                    var filterModel = new ProductConversionViewModelDoc();

                    if (!string.IsNullOrEmpty(item.Product_Index.ToString()))
                    {
                        filterModel.product_Index = item.Product_Index;
                    }

                    if (!string.IsNullOrEmpty(item.ProductConversion_Index.ToString()))
                    {
                        filterModel.productConversion_Index = item.ProductConversion_Index;
                    }
                    resultProductconversion = utils.SendDataApi<List<ProductConversionViewModelDoc>>(new AppSettingConfig().GetUrl("dropdownProductconversion"), filterModel.sJson());

                    var resultItem = new PopupPlanGoodsReceiveViewModel();

                    resultItem.planGoodsReceive_Index = item.PlanGoodsReceive_Index;
                    resultItem.planGoodsReceiveItem_Index = item.PlanGoodsReceiveItem_Index;
                    resultItem.planGoodsReceive_No = item.PlanGoodsReceive_No;
                    resultItem.product_Index = item.Product_Index;
                    resultItem.lineNum = item.LineNum;
                    resultItem.product_Id = item.Product_Id;
                    resultItem.product_Name = item.Product_Name;
                    resultItem.product_SecondName = item.Product_SecondName;
                    resultItem.product_ThirdName = item.Product_ThirdName;
                    resultItem.product_Lot = item.Product_Lot;
                    resultItem.owner_Index = item.Owner_Index;
                    resultItem.qty = item.Total;
                    resultItem.ratio = item.Ratio;
                    resultItem.totalQty = (item.Total * item.Ratio);
                    resultItem.productConversion_Index = item.ProductConversion_Index;
                    resultItem.productConversion_Id = item.ProductConversion_Id;
                    resultItem.productConversion_Name = item.ProductConversion_Name;
                    resultItem.mfg_Date = item.MFG_Date.toString();
                    resultItem.exp_Date = item.EXP_Date.toString();
                    resultItem.weight = item.Weight;
                    resultItem.unitWeight = resultProductconversion.FirstOrDefault().productConversion_Weight;
                    resultItem.unitWidth = item.UnitWidth;
                    resultItem.unitLength = item.UnitLength;
                    resultItem.unitHeight = item.UnitHeight;
                    //resultItem.unitVolume = resultProductconversion.FirstOrDefault().productConversion_Volume;
                    //resultItem.volume = item.Volume;

                    if (resultProductconversion.FirstOrDefault().productConversion_Volume == 0 || resultProductconversion.FirstOrDefault().productConversion_Volume == null)
                    {
                        resultItem.unitVolume = 0;
                    }
                    else
                    {
                        resultItem.unitVolume = resultProductconversion.FirstOrDefault().productConversion_Volume;
                    }

                    if (item.Volume == 0 || item.Volume == null)
                    {
                        resultItem.volume = 0;
                    }
                    else
                    {
                        resultItem.volume = item.Volume;
                    }
                    resultItem.unitPrice = item.UnitPrice;
                    resultItem.price = item.Price;
                    resultItem.documentRef_No1 = item.DocumentRef_No1;
                    resultItem.documentRef_No2 = item.DocumentRef_No2;
                    resultItem.documentRef_No3 = item.DocumentRef_No3;
                    resultItem.documentRef_No4 = item.DocumentRef_No4;
                    resultItem.documentRef_No5 = item.DocumentRef_No5;
                    resultItem.document_Status = item.Document_Status;
                    resultItem.documentItem_Remark = item.DocumentItem_Remark;
                    resultItem.udf_1 = item.UDF_1;
                    resultItem.udf_2 = item.UDF_2;
                    resultItem.udf_3 = item.UDF_3;
                    resultItem.udf_4 = item.UDF_4;
                    resultItem.udf_5 = item.UDF_5;
                    resultItem.erp_Location = item.ERP_Location;
                    items.Add(resultItem);
                }


                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region im_PlanGoodsReceive
        public List<PlanGoodsReceiveViewModel> im_PlanGoodsReceive(DocumentViewModel model)
        {
            try
            {



                var query = db.IM_PlanGoodsReceive.AsQueryable();

                var result = new List<PlanGoodsReceiveViewModel>();

                if (model.listDocumentViewModel.FirstOrDefault().document_Index != null)
                {
                    query = query.Where(c => model.listDocumentViewModel.Select(s => s.document_Index).Contains(c.PlanGoodsReceive_Index));
                }

                else if (model.listDocumentViewModel.FirstOrDefault().document_Status != null)
                {
                    query = query.Where(c => model.listDocumentViewModel.Select(s => s.document_Status).Contains(c.Document_Status));
                }




                var queryresult = query.ToList();


                foreach (var item in queryresult)
                {
                    var resultItem = new PlanGoodsReceiveViewModel();
                    resultItem.planGoodsReceive_Index = item.PlanGoodsReceive_Index;
                    resultItem.owner_Index = item.Owner_Index;
                    resultItem.owner_Id = item.Owner_Id;
                    resultItem.owner_Name = item.Owner_Name;
                    resultItem.vendor_Index = item.Vendor_Index;
                    resultItem.vendor_Id = item.Vendor_Id;
                    resultItem.vendor_Name = item.Vendor_Name;
                    resultItem.documentType_Index = item.DocumentType_Index;
                    resultItem.documentType_Id = item.DocumentType_Id;
                    resultItem.documentType_Name = item.DocumentType_Name;
                    resultItem.planGoodsReceive_No = item.PlanGoodsReceive_No;
                    resultItem.planGoodsReceive_Date = item.PlanGoodsReceive_Date;
                    resultItem.planGoodsReceive_Time = item.PlanGoodsReceive_Time;
                    resultItem.planGoodsReceive_Due_Date = item.PlanGoodsReceive_Due_Date;
                    resultItem.documentRef_No1 = item.DocumentRef_No1;
                    resultItem.documentRef_No2 = item.DocumentRef_No2;
                    resultItem.documentRef_No3 = item.DocumentRef_No3;
                    resultItem.documentRef_No4 = item.DocumentRef_No4;
                    resultItem.documentRef_No5 = item.DocumentRef_No5;
                    resultItem.document_Status = item.Document_Status;
                    resultItem.uDF_1 = item.UDF_1;
                    resultItem.uDF_2 = item.UDF_2;
                    resultItem.uDF_3 = item.UDF_3;
                    resultItem.uDF_4 = item.UDF_4;
                    resultItem.uDF_5 = item.UDF_5;
                    resultItem.documentPriority_Status = item.DocumentPriority_Status;
                    resultItem.document_Remark = item.Document_Remark;
                    resultItem.create_Date = item.Create_Date;
                    resultItem.create_By = item.Create_By;
                    resultItem.update_Date = item.Update_Date;
                    resultItem.update_By = item.Update_By;
                    resultItem.cancel_Date = item.Cancel_Date;
                    resultItem.cancel_By = item.Cancel_By;
                    resultItem.warehouse_Index = item.Warehouse_Index;
                    resultItem.warehouse_Id = item.Warehouse_Id;
                    resultItem.warehouse_Name = item.Warehouse_Name;
                    resultItem.warehouse_Index_To = item.Warehouse_Index_To;
                    resultItem.warehouse_Id_To = item.Warehouse_Id_To;
                    resultItem.warehouse_Name_To = item.Warehouse_Name_To;
                    resultItem.userAssign = item.UserAssign;
                    resultItem.userAssignKey = item.UserAssignKey;
                    resultItem.dock_Index = item.Dock_Index;
                    resultItem.dock_Id = item.Dock_Id;
                    resultItem.dock_Name = item.Dock_Name;
                    resultItem.transport_Index = item.Transport_Index;
                    resultItem.transport_Id = item.Transport_Id;
                    resultItem.transport_Name = item.Transport_Name;
                    resultItem.round_Index = item.Round_Index;
                    resultItem.round_Id = item.Round_Id;
                    resultItem.round_Name = item.Round_Name;
                    resultItem.license_Name = item.License_Name;

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

        #region autoDocumentRef
        public List<ItemListViewModel> autoDocumentRef(ItemListViewModel data)
        {
            try
            {
                var query = db.IM_PlanGoodsReceive.AsQueryable();

                if (data.key == "-")
                {


                }
                else if (!string.IsNullOrEmpty(data.key))
                {
                    query = query.Where(c => c.DocumentRef_No2.Contains(data.key));

                }

                var items = new List<ItemListViewModel>();

                var result = query.Select(c => new { c.DocumentRef_No2 }).Distinct().Take(10).ToList();


                foreach (var item in result)
                {
                    var resultItem = new ItemListViewModel
                    {
                        name = item.DocumentRef_No2
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
        #endregion

        #region PrintPlanGoodsReceive
        public string PrintPlanGoodsReceive(ReportPlanGoodsReceiveViewModel data, string rootPath = "")
        {
            var culture = new System.Globalization.CultureInfo("en-US");
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            Guid? planGRItemIndex = new Guid();

            try
            {
                var queryHead = db.IM_PlanGoodsReceive.FirstOrDefault(c => c.PlanGoodsReceive_Index == data.planGoodsReceive_Index);
                var query = db.IM_PlanGoodsReceiveItem.Where(c => c.PlanGoodsReceive_Index == data.planGoodsReceive_Index);
                var result = new List<ReportPlanGoodsReceiveViewModel>();

                string date = queryHead.PlanGoodsReceive_Date.toString();
                string planGRDate = DateTime.ParseExact(date.Substring(0, 8), "yyyyMMdd",
                System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy", culture);

                string dueDate = queryHead.PlanGoodsReceive_Due_Date.toString();
                string planGR_DueDate = DateTime.ParseExact(dueDate.Substring(0, 8), "yyyyMMdd",
                System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy", culture);

                int i = 1;
                foreach (var item in query)
                {
                    var resultItem = new ReportPlanGoodsReceiveViewModel();
                    resultItem.table_No = i;
                    resultItem.planGoodsReceive_No = queryHead.PlanGoodsReceive_No;
                    resultItem.planGoodsReceive_Date = planGRDate;
                    resultItem.planGoodsReceive_Time = queryHead.PlanGoodsReceive_Time;
                    resultItem.warehouse_Name = queryHead.Warehouse_Name;
                    resultItem.documentType_Name = queryHead.DocumentType_Name;
                    resultItem.planGoodsReceive_Due_Date = planGR_DueDate;
                    resultItem.license_Name = queryHead.License_Name;
                    resultItem.dock_Name = queryHead.Dock_Name;
                    resultItem.owner_Name = queryHead.Owner_Name;
                    resultItem.vendor_Name = queryHead.Vendor_Name;
                    resultItem.driver_Name = queryHead.Driver_Name;
                    resultItem.document_Remark = queryHead.Document_Remark;
                    resultItem.transport_Name = queryHead.Transport_Name;
                    resultItem.round_Name = queryHead.Round_Name;
                    resultItem.documentRef_No1 = queryHead.DocumentRef_No1;
                    resultItem.planGoodsReceiveNo_Barcode = new NetBarcode.Barcode(queryHead.PlanGoodsReceive_No, NetBarcode.Type.Code128B).GetBase64Image();
                    resultItem.product_Index = item.Product_Index;
                    resultItem.product_Id = item.Product_Id;
                    resultItem.product_Name = item.Product_Name;
                    resultItem.itemStatus_Name = item.ItemStatus_Name;
                    resultItem.product_Lot = item.Product_Lot;
                    resultItem.qty = Convert.ToInt32(item.Qty);
                    resultItem.productConversion_Name = item.ProductConversion_Name;
                    resultItem.weight = Convert.ToDecimal(string.Format("{0:0.00}", item.Weight));
                    resultItem.documentItem_Remark = item.DocumentItem_Remark;

                    result.Add(resultItem);
                    i++;
                }
                result.ToList();

                rootPath = rootPath.Replace("\\PlanGRAPI", "");
                //var reportPath = rootPath + "\\PlanGRBusiness\\Reports\\PlanGR\\ReportPlanGoodsReceive.rdlc";
                var reportPath = rootPath + "\\Reports\\PlanGR\\ReportPlanGoodsReceive.rdlc";
                LocalReport report = new LocalReport(reportPath);
                report.AddDataSource("DataSet1", result);

                System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                string fileName = "";
                string fullPath = "";
                fileName = "tmpReport" + DateTime.Now.ToString("yyyyMMddHHmmss");

                var renderedBytes = report.Execute(RenderType.Pdf);

                Utils objReport = new Utils();
                fullPath = objReport.saveReport(renderedBytes.MainStream, fileName + ".pdf", rootPath);
                var saveLocation = objReport.PhysicalPath(fileName + ".pdf", rootPath);
                return saveLocation;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Cancel
        public Boolean Cancel(PlanGoodsReceiveDocViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {

                #region CheckGR

                var resultGRItem = new List<GoodsReceiveItemViewModel>();
                var list = new List<DocumentViewModel> { new DocumentViewModel { ref_document_Index = data.planGoodsReceive_Index } };
                var grItem = new DocumentViewModel();
                grItem.listDocumentViewModel = list;

                resultGRItem = utils.SendDataApi<List<GoodsReceiveItemViewModel>>(new AppSettingConfig().GetUrl("FindGR"), grItem.sJson());
                var check_Document = resultGRItem.Where(c => c.document_Status != -1).ToList();
                if (check_Document.Count > 0)
                {
                    return false;
                }
                #endregion



                var PlanGoodsReceive = db.IM_PlanGoodsReceive.Find(data.planGoodsReceive_Index);

                if (PlanGoodsReceive != null)
                {
                    PlanGoodsReceive.Document_Status = -1;
                    PlanGoodsReceive.Cancel_By = data.cancel_By;
                    PlanGoodsReceive.Cancel_Date = DateTime.Now;

                    var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {
                        db.SaveChanges();
                        transaction.Commit();
                        return true;
                    }

                    catch (Exception exy)
                    {
                        msglog = State + " ex Rollback " + exy.Message.ToString();
                        olog.logging("CancelPlanGR", msglog);
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

        #endregion

        #region genDocumentNo
        public String genDocumentNo(PlanGoodsReceiveDocViewModel data)
        {
            try
            {
                var result = new List<GenDocumentTypeViewModel>();

                var filterModel = new GenDocumentTypeViewModel();

                filterModel.process_Index = new Guid("C2A3F847-BAA6-46FE-B502-44F2D5826A1C");
                filterModel.documentType_Index = data.documentType_Index;
                //GetConfig
                result = utils.SendDataApi<List<GenDocumentTypeViewModel>>(new AppSettingConfig().GetUrl("dropDownDocumentType"), filterModel.sJson());

                var genDoc = new AutoNumberService(db);
                string DocNo = "";
                DateTime DocumentDate = (DateTime)data.planGoodsReceive_Date.toDate();
                DocNo = genDoc.genAutoDocmentNumber(result, DocumentDate);

                return DocNo;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region dropdownCostCenter
        public List<CostCenterViewModel> dropdownCostCenter(CostCenterViewModel data)
        {
            try
            {
                var result = new List<CostCenterViewModel>();

                var filterModel = new CostCenterViewModel();

                //GetConfig
                result = utils.SendDataApi<List<CostCenterViewModel>>(new AppSettingConfig().GetUrl("dropdownCostCenter"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #region dropdownWeight
        public List<WeightViewModel> dropdownWeight(WeightViewModel data)
        {
            try
            {
                var result = new List<WeightViewModel>();

                var filterModel = new WeightViewModel();

                //GetConfig
                result = utils.SendDataApi<List<WeightViewModel>>(new AppSettingConfig().GetUrl("dropdownWeight"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #region dropdownCurrency
        public List<CurrencyViewModel> dropdownCurrency(CurrencyViewModel data)
        {
            try
            {
                var result = new List<CurrencyViewModel>();

                var filterModel = new CurrencyViewModel();

                //GetConfig
                result = utils.SendDataApi<List<CurrencyViewModel>>(new AppSettingConfig().GetUrl("dropdownCurrency"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region dropdownVolume
        public List<VolumeViewModel> dropdownVolume(VolumeViewModel data)
        {
            try
            {
                var result = new List<VolumeViewModel>();

                var filterModel = new VolumeViewModel();

                //GetConfig
                result = utils.SendDataApi<List<VolumeViewModel>>(new AppSettingConfig().GetUrl("dropdownVolume"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region dropdownShipmentType
        public List<ShipmentTypeViewModel> dropdownShipmentType(ShipmentTypeViewModel data)
        {
            try
            {
                var result = new List<ShipmentTypeViewModel>();

                var filterModel = new ShipmentTypeViewModel();

                //GetConfig
                result = utils.SendDataApi<List<ShipmentTypeViewModel>>(new AppSettingConfig().GetUrl("dropdownShipmentType"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region dropdownContainerType
        public List<ContainerTypeViewModelV2> dropdownContainerType(ContainerTypeViewModelV2 data)
        {
            try
            {
                var result = new List<ContainerTypeViewModelV2>();

                var filterModel = new ContainerTypeViewModelV2();

                //GetConfig
                result = utils.SendDataApi<List<ContainerTypeViewModelV2>>(new AppSettingConfig().GetUrl("dropdownContainerType"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region dropdownDockDoor
        public List<DockDoorViewModelV2> dropdownDockDoor(DockDoorViewModelV2 data)
        {
            try
            {
                var result = new List<DockDoorViewModelV2>();

                var filterModel = new DockDoorViewModelV2();

                //GetConfig
                result = utils.SendDataApi<List<DockDoorViewModelV2>>(new AppSettingConfig().GetUrl("dropdownDockDoor"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region dropdownVehicleType
        public List<VehicleTypeViewModel> dropdownVehicleType(VehicleTypeViewModel data)
        {
            try
            {
                var result = new List<VehicleTypeViewModel>();

                var filterModel = new VehicleTypeViewModel();

                //GetConfig
                result = utils.SendDataApi<List<VehicleTypeViewModel>>(new AppSettingConfig().GetUrl("dropdownVehicleType"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region dropdownUnloadingType
        public List<UnloadingTypeViewModel> dropdownUnloadingType(UnloadingTypeViewModel data)
        {
            try
            {
                var result = new List<UnloadingTypeViewModel>();

                var filterModel = new UnloadingTypeViewModel();

                //GetConfig
                result = utils.SendDataApi<List<UnloadingTypeViewModel>>(new AppSettingConfig().GetUrl("dropdownUnloadingType"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region dropdownCargoType
        public List<CargoTypeViewModel> dropdownCargoType(CargoTypeViewModel data)
        {
            try
            {
                var result = new List<CargoTypeViewModel>();

                var filterModel = new CargoTypeViewModel();

                //GetConfig
                result = utils.SendDataApi<List<CargoTypeViewModel>>(new AppSettingConfig().GetUrl("dropdownCargoType"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region dropdownDocumentPriority
        public List<DocumentPriorityViewModel> dropdownDocumentPriority(DocumentPriorityViewModel data)
        {
            try
            {
                var result = new List<DocumentPriorityViewModel>();

                var filterModel = new DocumentPriorityViewModel();

                //GetConfig
                result = utils.SendDataApi<List<DocumentPriorityViewModel>>(new AppSettingConfig().GetUrl("dropdownDocumentPriority"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region ReportDNService
        public string PrintReportDN(ReportDNViewModel data, string rootPath = "")
        {
            var culture = new System.Globalization.CultureInfo("en-US");
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            //var PlanGR_DB_1 = new PlanGRDbContext();
            try
            {
                var queryPlanGR = db.View_RPT_DN.AsQueryable();
                var result = new List<ReportDNViewModel>();
                var query = queryPlanGR.ToList();

                query = query.Where(q => q.PlanGoodsReceive_Index == data.planGoodsReceive_Index).ToList();

                string Date = DateTime.ParseExact(data.planGoodsReceive_Date.Substring(0, 8), "yyyyMMdd",
                System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MMM/yyyy", culture);

                string Due_Date = DateTime.ParseExact(data.planGoodsReceive_Due_Date.Substring(0, 8), "yyyyMMdd",
                System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MMM/yyyy", culture);

                int i = 0;

                foreach (var item in query)
                {
                    i++;
                    var resultItem = new ReportDNViewModel();

                    resultItem.owner_Id = item.Owner_Id;
                    resultItem.owner_Name = item.Owner_Name;
                    resultItem.planGoodsReceive_Due_Date = item.PlanGoodsReceive_Due_Date.toString();
                    resultItem.planGoodsReceive_Date = item.PlanGoodsReceive_Date.toString();
                    resultItem.lineNum = item.LineNum;
                    resultItem.product_Id = item.Product_Id;
                    resultItem.product_Name = item.Product_Name;
                    resultItem.qty = item.Qty;
                    resultItem.productConversion_Name = item.ProductConversion_Name;
                    resultItem.planGoodsReceive_No_Barcode = new NetBarcode.Barcode(item.PlanGoodsReceive_No, NetBarcode.Type.Code128B).GetBase64Image();
                    resultItem.unitHeight = item.UnitHeight;
                    resultItem.unitLength = item.UnitLength;
                    resultItem.unitWidth = item.UnitWidth;
                    resultItem.volume_Name = item.Volume_Name;
                    resultItem.planGoodsReceive_No = item.PlanGoodsReceive_No;
                    resultItem.size = (item.UnitWidth ?? 0).ToString("#.###") + " x " + (item.UnitLength ?? 0).ToString("#.###") + " x " + (item.UnitHeight ?? 0).ToString("#.###");
                    resultItem.date = Date;
                    resultItem.due_Date = Due_Date;
                    resultItem.shelfLifeGR = item.ShelfLifeGR;
                    resultItem.ti = string.IsNullOrEmpty(item.TI) ? "0" : item.TI;
                    resultItem.hi = string.IsNullOrEmpty(item.HI) ? "0" : item.HI;
                    resultItem.ref_PO = item.Ref_PO;
                    var calTiHi = (item.TI == "" && item.HI == "") ? 0 : Convert.ToInt16(string.IsNullOrEmpty(item.TI) ? "0" : item.TI) * Convert.ToInt16(item.HI);
                    resultItem.tixhi = (item.TI == "" && item.HI == "") ? "" : item.TI + " x " + item.HI + " (" + calTiHi + ")";
                    resultItem.sale_qty = item.Sale_Qty;
                    resultItem.sale_unit = item.Sale_Unit;
                    resultItem.in_qty = item.In_Qty;
                    resultItem.in_unit = item.In_Unit;
                    resultItem.estPallet = item.EstPallet;
                    resultItem.productConversion_Ratio = item.ProductConversion_Ratio;
                    resultItem.documentType_Name = item.DocumentType_Name;
                    resultItem.document_Remark = item.Document_Remark;
                    resultItem.isLot = item.IsLot;

                    result.Add(resultItem);
                    if (query.Count == i)
                    {
                        resultItem.count = query.Count;
                    }
                }
                result.ToList();

                rootPath = rootPath.Replace("\\PlanGRAPI", "");
                var reportPath = rootPath + new AppSettingConfig().GetUrl("ReportDN");
                //var reportPath = rootPath + "\\PlanGRBusiness\\Reports\\ReportDN\\ReportDN.rdlc";
                //var reportPath = rootPath + "\\Reports\\ReportDN\\ReportDN.rdlc";
                LocalReport report = new LocalReport(reportPath);
                report.AddDataSource("DataSet1", result);

                System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                string fileName = "";
                string fullPath = "";
                fileName = "tmpReport" + DateTime.Now.ToString("yyyyMMddHHmmss");

                var renderedBytes = report.Execute(RenderType.Pdf);

                Utils objReport = new Utils();
                fullPath = objReport.saveReport(renderedBytes.MainStream, fileName + ".pdf", rootPath);
                var saveLocation = objReport.PhysicalPath(fileName + ".pdf", rootPath);
                return saveLocation;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion


        #region AutobasicSuggestionOwner
        public List<ItemListViewModel> AutobasicSuggestionOwner(ItemListViewModel data)
        {
            var items = new List<ItemListViewModel>();
            try
            {
                if (!string.IsNullOrEmpty(data.key))
                {
                    List<int?> status = new List<int?> { -1, -2 };
                    var query1 = db.IM_PlanGoodsReceive.Where(c => c.Owner_Name.Contains(data.key) || c.Owner_Id.Contains(data.key) && !new List<int?> { -1, -2 }.Contains(c.Document_Status)).Select(s => new ItemListViewModel
                    {
                        name = s.Owner_Name,
                        id = s.Owner_Id,
                        index = s.Owner_Index
                    }).Distinct();


                    var query = query1;

                    items = query.OrderBy(c => c.name).Take(10).ToList();
                }

            }
            catch (Exception ex)
            {

            }

            return items;
        }

        #endregion


        #region autoOwnerfilterName
        public List<ItemListViewModel> autoOwnerfilterName(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();

                var filterModel = new ItemListViewModel();
                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key = data.key;
                }

                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("autoOwnerfilterName"), filterModel.sJson());
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AutoPlanGoodsReceiveNoAndOwner
        public List<ItemListViewModel> autoPlanGoodsReceiveNoAndOwner(ItemListViewModel data)
        {
            try
            {
                var query = db.IM_PlanGoodsReceive.AsQueryable();

                if (data.key == "-")
                {


                }
                else if (!string.IsNullOrEmpty(data.key))
                {
                    query = query.Where(c => c.PlanGoodsReceive_No.Contains(data.key));

                }

                var items = new List<ItemListViewModel>();

                var result = query.Select(c => new { c.PlanGoodsReceive_No, c.Owner_Index, c.Owner_Id, c.Owner_Name }).Distinct().Take(10).ToList();


                foreach (var item in result)
                {
                    var resultItem = new ItemListViewModel
                    {
                        index = item.Owner_Index,
                        name = item.PlanGoodsReceive_No,
                        value1 = item.Owner_Id,
                        value2 = item.Owner_Name

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

        #endregion

        #region checkProduct
        public bool checkProduct(ProductViewModel data)
        {
            try
            {

                var Productresult = new List<ProductViewModel>();

                var ProductfilterModel = new ProductViewModel();
                ProductfilterModel.product_Index = data.product_Index;

                //GetConfig
                Productresult = utils.SendDataApi<List<ProductViewModel>>(new AppSettingConfig().GetUrl("product"), ProductfilterModel.sJson());

                if (Productresult.Count > 0)
                {
                    return true;
                }

                return false;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        public actionResultPlanPOViewModels PlanPOfilterPopup(PopupPurchaseOrderDocViewModel data)
        {
            try
            {
                var result = new List<PopupPurchaseOrderDocViewModel>();
                //GetConfig
                result = utils.SendDataApi<List<PopupPurchaseOrderDocViewModel>>(new AppSettingConfig().GetUrl("PopupPlanPO"), data.sJson()).Where(c => c.document_Status != -1 && c.document_Status != -2 && c.document_Status != 0).ToList();

                var items = data.PerPage == 0 ? result.ToList() : result.OrderByDescending(o => o.purchaseOrder_Due_Date).Skip((data.CurrentPage - 1) * data.PerPage).Take(data.PerPage).ToList();
                //var listPGR_Index = db.IM_PlanGoodsReceiveItem.Where(c => c.Document_Status != -1).GroupBy(g => g.Ref_Document_Index).ToList();

                //if (listPGR_Index.Count > 0)
                //{
                //    items.RemoveAll(c => listPGR_Index.Select(s => s.Key).Contains(c.purchaseOrder_Index));
                //}

                //var checkudf1 = db.IM_GoodsReceiveItem.Where(c => items.Select(s => s.planGoodsReceive_Index).Contains(c.Ref_Document_Index.sParse<Guid>()) && !(c.UDF_1 == "" || c.UDF_1 == null) && c.Document_Status != -1).GroupBy(g => g.Ref_Document_Index).ToList();

                //if (checkudf1.Count() > 0)
                //{
                //    items.RemoveAll(c => checkudf1.Select(s => s.Key).Contains(c.planGoodsReceive_Index));
                //}

                var count = result.Count;
                var actionResultPlanGRPopup = new actionResultPlanPOViewModels();
                actionResultPlanGRPopup.itemsPlanPO = items;
                actionResultPlanGRPopup.pagination = new Pagination() { TotalRow = count, CurrentPage = data.CurrentPage, PerPage = data.PerPage };

                return actionResultPlanGRPopup;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<View_GetPurchaseOrderItemViewModel> GetPlanPOPopup(View_GetPurchaseOrderItemViewModel model)
        {
            //if (String.IsNullOrEmpty(id)) throw new NullReferenceException();

            try
            {
                var result = new List<View_GetPurchaseOrderItemViewModel>();

                var checkGR = db.View_CheckPlanPO.Find(model.purchaseOrder_Index);

                if (checkGR == null)
                {


                    var filterModel = new List<View_GetPurchaseOrderItemViewModel>();

                    result = utils.SendDataApi<List<View_GetPurchaseOrderItemViewModel>>(new AppSettingConfig().GetUrl("getPlanPOIfilter"), model.sJson());


                }
                else
                {
                    //var query = db.View_PlanGoodsReceiveItem.AsQueryable();

                    var filterModel = new List<View_GetPurchaseOrderItemViewModel>();
                    result = utils.SendDataApi<List<View_GetPurchaseOrderItemViewModel>>(new AppSettingConfig().GetUrl("getPlanPOIPendingfilter"), model.sJson());



                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
