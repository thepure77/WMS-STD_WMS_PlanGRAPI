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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PlanGRAPI.Controllers
{
    [Route("api/AutoPlanGoodsReceive")]
    public class AutoPlanGoodsReceiveController : Controller
    {
        private PlanGRDbContext context;

        public AutoPlanGoodsReceiveController(PlanGRDbContext context)
        {
            this.context = context;
        }


        #region AutobasicSuggestion
        [HttpPost("autobasicSuggestion")]
        public IActionResult autobasicSuggestion([FromBody]JObject body)

        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autobasicSuggestion(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region AutobasicSuggestionPO
        [HttpPost("autobasicSuggestionPO")]
        public IActionResult autobasicSuggestionPO([FromBody]JObject body)

        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autobasicSuggestionPO(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region AutobasicSuggestionVender
        [HttpPost("autobasicSuggestionVender")]
        public IActionResult autobasicSuggestionVender([FromBody]JObject body)

        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autobasicSuggestionVender(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion


        #region AutoOwnerfilter
        [HttpPost("autoOwnerfilter")]
        public IActionResult autoOwnerfilter([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoOwnerfilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region AutoVenderfilter

        [HttpPost("autoVenderfilter")]
        public IActionResult autoVenderfilter([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoVenderfilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region AutoStatusfilter
        [HttpPost("autoStatusfilter")]
        public IActionResult autoStatusfilter([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoStatusfilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region AutoWarehousefilter
        [HttpPost("autoWarehousefilter")]
        public IActionResult autoWarehousefilter([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoWarehousefilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region AutoDocumentTypefilter
        [HttpPost("autoDocumentTypefilter")]
        public IActionResult autoDocumentTypefilter([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoDocumentTypefilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion


        #region AutoPlanGoodsReceiveNo
        [HttpPost("autoPlanGoodsReceiveNo")]
        public IActionResult autoPlanGoodsReceiveNo([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoPlanGoodsReceiveNo(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region AutoPlanGRFilter
        [HttpPost("autoPlanGRFilter")]
        public IActionResult autoPlanGRFilter([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new PlanGoodsReceiveAutoViewModel();
                Models = JsonConvert.DeserializeObject<PlanGoodsReceiveAutoViewModel>(body.ToString());
                var result = service.autoPlanGRFilter(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region AutoUser
        [HttpPost("autoUser")]
        public IActionResult autoUser([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoUser(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region AutoSkufilter
        [HttpPost("autoSkufilter")]
        public IActionResult autoSkufilter([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoSkufilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region AutoProductfilter
        [HttpPost("autoProductfilter")]
        public IActionResult autoProdutfilter([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoProductfilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region autoDocumentRef
        [HttpPost("autoDocumentRef")]
        public IActionResult autoDocumentRef([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoDocumentRef(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region AutobasicSuggestionOwner
        [HttpPost("autobasicSuggestionOwner")]
        public IActionResult AutobasicSuggestionOwner([FromBody]JObject body)

        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.AutobasicSuggestionOwner(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region autoOwnerfilterName
        [HttpPost("autoOwnerfilterName")]
        public IActionResult autoOwnerfilterName([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoOwnerfilterName(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region AutoPlanGoodsReceiveNoAndOwner
        [HttpPost("autoPlanGoodsReceiveNoAndOwner")]
        public IActionResult autoPlanGoodsReceiveNoAndOwner([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoPlanGoodsReceiveNoAndOwner(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

    }
}
