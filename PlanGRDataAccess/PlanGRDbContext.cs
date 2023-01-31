using GRDataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PlanGRDataAccess.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataAccess
{
    public class PlanGRDbContext : DbContext
    {
        //public PlanGRDbContext()
        //{
        //}

        public PlanGRDbContext(DbContextOptions<PlanGRDbContext> options) : base(options)
        {
        }

        public DbSet<IM_PlanGoodsReceive> IM_PlanGoodsReceive { get; set; }
        public DbSet<IM_PlanGoodsReceiveItem> IM_PlanGoodsReceiveItem { get; set; }
        public DbSet<IM_GoodsReceive> IM_GoodsReceives { get; set; }
        public DbSet<IM_GoodsReceiveItem> IM_GoodsReceiveItem { get; set; }
        public DbSet<View_ProductDetail> View_ProductDetail { get; set; }
        public DbSet<View_RPT_DN> View_RPT_DN { get; set; }
        public virtual DbSet<GetValueByColumn> GetValueByColumn { get; set; }
        public virtual DbSet<View_CheckCloseDocument> View_CheckCloseDocuments { get; set; }
        public DbSet<View_PlanGoodsReceiveItem> View_PlanGoodsReceiveItem { get; set; }
        public DbSet<View_PlanGrProcessStatus> View_PlanGrProcessStatus { get; set; }

        public DbSet<View_CheckDocumentStatus> View_CheckDocumentStatus { get; set; }

        public DbSet<View_GetScanProductDetail> View_GetScanProductDetail { get; set; }
        public DbSet<Get_PlanGoodsIssueItemPopup> Get_PlanGoodsIssueItemPopup { get; set; }

        public DbSet<View_ReturnReceive> View_ReturnReceive { get; set; }
        public DbSet<View_GetPlanGoodsReceiveItem> View_GetPlanGoodsReceiveItem { get; set; }
        public virtual DbSet<View_GoodsReceivePending> View_GoodsReceivePending { get; set; }

        public DbSet<View_GetPlanGoodsReceive_Popup> View_GetPlanGoodsReceive_Popup { get; set; }

        public virtual DbSet<sy_Config> sy_Config { get; set; }

        public DbSet<View_GetScanPlanGRI> View_GetScanPlanGRI { get; set; }

        public DbSet<im_DocumentFile> im_DocumentFile { get; set; }
        public DbSet<View_CheckPlanPO> View_CheckPlanPO { get; set; }
        public DbSet<View_GoodsReceiveBOM> View_GoodsReceiveBOM { get; set; }

        public DbSet<im_PurchaseOrder_Ref> im_PurchaseOrder_Ref { get; set; }
        public DbSet<im_PurchaseOrderItem_Ref> im_PurchaseOrderItem_Ref { get; set; }
        public DbSet<im_PlanGoodsReceive_Ref> im_PlanGoodsReceive_Ref { get; set; }
        public DbSet<im_PlanGoodsReceiveItem_Ref> im_PlanGoodsReceiveItem_Ref { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    var builder = new ConfigurationBuilder();
            //    builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: false);

            //    var configuration = builder.Build();

            //    var connectionString = configuration.GetConnectionString("DefaultConnection").ToString();

            //    optionsBuilder.UseSqlServer(connectionString);
            //}

            //optionsBuilder.UseSqlServer(@"Server=192.168.1.11\MSSQL2017;Database=WMSDB;Trusted_Connection=True;Integrated Security=False;user id=sa;password=K@sc0db12345;");
            //optionsBuilder.UseSqlServer(@"Server=10.0.177.33\SQLEXPRESS;Database=WMSDB;Trusted_Connection=True;Integrated Security=False;user id=cfrffmusr;password=ffmusr@cfr;");
            //optionsBuilder.UseSqlServer(@"Server=kascoit.ddns.net,22017;Database=WMSDB_QA;Trusted_Connection=True;Integrated Security=False;user id=sa;password=K@sc0db12345;");
        }

        
    }


}
