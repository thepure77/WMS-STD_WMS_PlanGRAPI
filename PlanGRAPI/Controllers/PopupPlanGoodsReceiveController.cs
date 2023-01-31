using System;
using System.Collections.Generic;
using DataAccess;
using GRBusiness;
using GRBusiness.PlanGoodsReceive;
using MasterDataBusiness.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlanGRBusiness.PlanGoodsReceive;
using PlanGRBusiness.ViewModels;
using POBusiness.PopupPurchaseOrderBusiness;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PlanGRAPI.Controllers
{
    [Route("api/PopupPlanGoodsReceive")]
    public class PopupPlanGoodsReceiveController : Controller
    {
        private PlanGRDbContext context;

        public PopupPlanGoodsReceiveController(PlanGRDbContext context)
        {
            this.context = context;
        }


        #region PopupPlanGoodsIssuefilter
        [HttpPost("popupPlanGoodsIssuefilter")]
        public IActionResult popupPlanGoodsIssuefilter([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new ReturnReceiveViewModel();
                Models = JsonConvert.DeserializeObject<ReturnReceiveViewModel>(body.ToString());
                var result = service.popupPlanGoodsIssuefilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region PopupPlanGRfilter
        [HttpPost("popupPlanGRfilter")]
        public IActionResult popupPlanGRfilter([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new PlanGoodsReceiveAutoViewModel();
                Models = JsonConvert.DeserializeObject<PlanGoodsReceiveAutoViewModel>(body.ToString());
                var result = service.popupPlanGRfilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        //#region PopupProductfilter
        //[HttpPost("popupProductfilter")]
        //public IActionResult popupProductfilter([FromBody]JObject body)
        //{
        //    try
        //    {
        //        var service = new PlanGoodsReceiveService(context);
        //        var Models = new ProductViewModel();
        //        Models = JsonConvert.DeserializeObject<ProductViewModel>(body.ToString());
        //        var result = service.popupProductfilter(Models);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}
        //#endregion

        #region PopupProductConversionfilter
        [HttpPost("popupProductConversionfilter")]
        public IActionResult popupProductConversionfilter([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new ProductConversionViewModelDoc();
                Models = JsonConvert.DeserializeObject<ProductConversionViewModelDoc>(body.ToString());
                var result = service.popupProductConversionfilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        [HttpPost("popupPlanGRIfilter")]
        public IActionResult popupPlanGRIfilter([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new PlanGoodsReceiveItemViewModel();
                Models = JsonConvert.DeserializeObject<PlanGoodsReceiveItemViewModel>(body.ToString());
                var result = service.popupPlanGRIfilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("GetPlanGRIfilter")]
        public IActionResult GetPlanGRIfilter([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new PopupPlanGoodsReceiveViewModel();
                Models = JsonConvert.DeserializeObject<PopupPlanGoodsReceiveViewModel>(body.ToString());
                var result = service.GetPlanGRIfilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("GetPlanGRIPendingfilter")]
        public IActionResult GetPlanGRIPendingfilter([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new PopupPlanGoodsReceiveViewModel();
                Models = JsonConvert.DeserializeObject<PopupPlanGoodsReceiveViewModel>(body.ToString());
                var result = service.GetPlanGRIPendingfilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost("PlanPOfilterPopup")]
        public IActionResult PlanGRfilterPopup([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new PopupPurchaseOrderDocViewModel();
                Models = JsonConvert.DeserializeObject<PopupPurchaseOrderDocViewModel>(body.ToString());
                var result = service.PlanPOfilterPopup(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost("GetPlanPOPopup")]
        public IActionResult GetPlanPOPopup([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new View_GetPurchaseOrderItemViewModel();
                Models = JsonConvert.DeserializeObject<View_GetPurchaseOrderItemViewModel>(body.ToString());
                var result = service.GetPlanPOPopup(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
