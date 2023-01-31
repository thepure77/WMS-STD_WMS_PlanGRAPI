using System;
using DataAccess;
using GRBusiness.PlanGoodsReceive;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlanGRBusiness.PlanGoodsReceive;
using PlanGRBusiness.PlanGoodsReceiveItem;

namespace PlanGRAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanGoodsReceiveItemController : ControllerBase
    {
        private PlanGRDbContext context;

        public PlanGoodsReceiveItemController(PlanGRDbContext context)
        {
            this.context = context;
        }

        #region getByPlanGoodReceiveId
        [HttpGet("GetByPlanGoodReceiveId/{id}")]
        public IActionResult GetByPlanGoodReceiveId(Guid id)
        {
            try
            {
                PlanGoodsReceiveItemService service = new PlanGoodsReceiveItemService(context);
                var result = service.GetByPlanGoodReceiveId(id);
                return this.Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }
        #endregion

        #region find
        [HttpGet("find/{id}")]
        public IActionResult find(Guid id)
        {
            try
            {
                var service = new PlanGoodsReceiveItemService(context);

                var result = service.find(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region GetGoodsReceiveItem
        [HttpGet("getGoodsReceiveItem/{id}")]
        public IActionResult GetGoodsReceiveItem(Guid id)
        {
            try
            {
                var service = new PlanGoodsReceiveItemService(context);

                var result = service.GetGoodsReceiveItem(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region GetRemainQty
        [HttpGet("getRemainQty/{id}")]
        public IActionResult GetRemainQty(Guid id)
        {
            try
            {
                var service = new PlanGoodsReceiveItemService(context);

                var result = service.GetRemainQty(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region GetPlanGoodsIssueItemPopup
        [HttpPost("getPlanGoodsIssueItemPopup")]
        public IActionResult GetPlanGoodsIssueItemPopup([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveItemService(context);
                var Models = new PlanGoodsIssueItemPopViewModel();
                Models = JsonConvert.DeserializeObject<PlanGoodsIssueItemPopViewModel>(body.ToString());
                var result = service.GetPlanGoodsIssueItemPopup(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        [HttpPost("getPlanGoodsReceiveIem")]
        public IActionResult getPlanGoodsReceiveIem([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveItemService(context);
                //var Models = new planGoodsReceiveitemview();
                var Models = JsonConvert.DeserializeObject<PlanGoodsReceiveItemViewModel>(body.ToString());
                var result = service.getPlanGoodsReceiveItem(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #region Delete
        [HttpPost("delete")]
        public IActionResult Delete([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveItemService(context);
                var Models = new PlanGoodsReceiveDocViewModel();
                Models = JsonConvert.DeserializeObject<PlanGoodsReceiveDocViewModel>(body.ToString());
                var result = service.Delete(Models);
                return Ok(result);
                //PlanGoodsReceiveService _appService = new PlanGoodsReceiveService(context);
                //var _model = _appService.getDelete(model);
                //return this.Ok(_model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region returnmatdoc
        [HttpGet("returnmatdoc/{id}")]
        public IActionResult returnmatdoc(string id)
        {
            try
            {
                PlanGoodsReceiveItemService service = new PlanGoodsReceiveItemService(context);
                var result = service.returnmatdoc(id);
                return this.Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }
        #endregion
    }
}