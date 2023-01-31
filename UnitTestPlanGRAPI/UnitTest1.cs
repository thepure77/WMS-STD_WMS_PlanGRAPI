//using DataAccess;
//using GRBusiness.PlanGoodsReceive;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using PlanGRAPI.Controllers;
//using PlanGRDataAccess.Models;
//using System;
//using System.Collections.Generic;

//namespace UnitTestPlanGRAPI
//{
//    [TestClass]
//    public class UnitTest1
//    {
//        private DbContextOptions<PlanGRDbContext> options;

//        [TestMethod]
//        public void SavePlanGR()
//        {
//            options = new DbContextOptionsBuilder<PlanGRDbContext>()
//                .UseInMemoryDatabase(databaseName: "PlanGR")
//                .Options;

//            using (var context = new PlanGRDbContext(options))
//            {
//                context.IM_PlanGoodsReceive.Add(new PlanGRDataAccess.Models.IM_PlanGoodsReceive
//                {
//                    PlanGoodsReceive_Index = new Guid("28572122-4209-42E7-BA56-22EE7E7937A6"),
//                    PlanGoodsReceive_No = "MN1910000001",
//                    Owner_Index = new Guid("8B8B6203-A634-4769-A247-C0346350A963"),
//                    Owner_Id = "CFR FC",
//                    Owner_Name = "Top Online",
//                    Vendor_Index = new Guid("9311AF2B-CD3E-46C5-99B3-00036B09FDBE"),
//                    Vendor_Id = "1",
//                    Vendor_Name = "ไม่ระบุ",
//                    DocumentType_Index = new Guid("45A7790A-8477-44D4-959E-A4B845870507"),
//                    DocumentType_Id = "FN-010",
//                    DocumentType_Name = "Manual create order",
//                    PlanGoodsReceive_Date = DateTime.Today,
//                    PlanGoodsReceive_Due_Date = DateTime.Today,
//                    Document_Status = 0,
//                    Create_By = "adminkasco",
//                    Create_Date = DateTime.Now,
//                    Warehouse_Index = new Guid("72885519-D256-4AAD-9C37-A783B90E1DF6"),
//                    Warehouse_Id = "432",
//                    Warehouse_Name = "CFR FC WH",
//                    Warehouse_Index_To = new Guid("8A7B5FBB-CF80-401E-AAEA-25E2C2E9297A"),
//                    Warehouse_Id_To = "1",
//                    Warehouse_Name_To = "ไม่ระบุ",
//                });

//                context.IM_PlanGoodsReceiveItem.Add(new PlanGRDataAccess.Models.IM_PlanGoodsReceiveItem
//                {
//                    PlanGoodsReceiveItem_Index = Guid.NewGuid(),
//                    PlanGoodsReceive_Index = Guid.NewGuid(),
//                    Product_Index = new Guid("41E2DADA-C753-4EA3-9218-00331B94D70F"),
//                    Product_Id = "0000048439633",
//                    Product_Name = "มายช้อยส์แกงเลียงออแกนิค 230ก",
//                    Product_SecondName  = "มายช้อยส์แกงเลียงออแกนิค 230ก",
//                    Product_ThirdName = "มายช้อยส์แกงเลียงออแกนิค 230ก",
//                    ItemStatus_Index = new Guid("C043169D-1D73-421B-9E33-69C770DCC3B4"),
//                    ItemStatus_Id = "A",
//                    ItemStatus_Name = "Grade A",
//                    Qty = 2,
//                    Ratio = 1,
//                    TotalQty = 2,
//                    ProductConversion_Index = new Guid("CF072538-5F92-446B-B259-5F57BEE7CDA0"),
//                    ProductConversion_Id = "1409095",
//                    ProductConversion_Name = "SPAC",
//                    Document_Status = 0,
//                    Create_By = "adminkasco",
//                    Create_Date = DateTime.Now

//                });

//                context.SaveChanges();
//            }

//            using (var context = new PlanGRDbContext(options))
//            {
//                var controller = new PlanGoodsReceiveController(context);

//                var item = new PlanGRBusiness.PlanGoodsReceive.PlanGoodsReceiveDocViewModel
//                {
//                    planGoodsReceive_Index = Guid.NewGuid(),
//                    planGoodsReceive_No = "MN1910000001",
//                    owner_Index = new Guid("8B8B6203-A634-4769-A247-C0346350A963"),
//                    owner_Id = "CFR FC",
//                    owner_Name = "Top Online",
//                    vendor_Index = new Guid("9311AF2B-CD3E-46C5-99B3-00036B09FDBE"),
//                    vendor_Id = "1",
//                    vendor_Name = "ไม่ระบุ",
//                    documentType_Index = new Guid("45A7790A-8477-44D4-959E-A4B845870507"),
//                    documentType_Id = "FN-010",
//                    documentType_Name = "Manual create order",
//                    planGoodsReceive_Date = "20191017",
//                    planGoodsReceive_Due_Date = "20191017",
//                    document_Status = 0,
//                    create_By = "adminkasco",
//                    create_date = "20191017",
//                    warehouse_Index = new Guid("72885519-D256-4AAD-9C37-A783B90E1DF6"),
//                    warehouse_Id = "432",
//                    warehouse_Name = "CFR FC WH",
//                    warehouse_Index_To = new Guid("8A7B5FBB-CF80-401E-AAEA-25E2C2E9297A"),
//                    warehouse_Id_To = "1",
//                    warehouse_Name_To = "ไม่ระบุ",
//                };

//                item.listPlanGoodsReceiveItemViewModel = new List<PlanGRBusiness.PlanGoodsReceive.PlanGoodsReceiveItemViewModel>();

//                item.listPlanGoodsReceiveItemViewModel.Add(new PlanGRBusiness.PlanGoodsReceive.PlanGoodsReceiveItemViewModel
//                {
//                    planGoodsReceiveItem_Index = Guid.NewGuid(),
//                    planGoodsReceive_Index = Guid.NewGuid(),
//                    product_Index = new Guid("41E2DADA-C753-4EA3-9218-00331B94D70F"),
//                    product_Id = "0000048439633",
//                    product_Name = "มายช้อยส์แกงเลียงออแกนิค 230ก",
//                    product_SecondName = "มายช้อยส์แกงเลียงออแกนิค 230ก",
//                    product_ThirdName = "มายช้อยส์แกงเลียงออแกนิค 230ก",
//                    itemStatus_Index = new Guid("C043169D-1D73-421B-9E33-69C770DCC3B4"),
//                    itemStatus_Id = "A",
//                    itemStatus_Name = "Grade A",
//                    qty = 2,
//                    ratio = 1,
//                    totalQty = 2,
//                    productConversion_Index = new Guid("CF072538-5F92-446B-B259-5F57BEE7CDA0"),
//                    productConversion_Id = "1409095",
//                    productConversion_Name = "SPAC",
//                    document_Status = 0,
//                    create_By = "adminkasco",
//                    create_date = DateTime.Now
//                });

//                IActionResult actionResult = controller.Post(item);

                
//                var result = actionResult as ObjectResult;

//                Assert.IsNotNull(result);
//                Assert.IsTrue(result is OkObjectResult);
//                Assert.IsInstanceOfType(result.Value, typeof(Boolean));
//                Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
//            }
//        }
//    }
//}
