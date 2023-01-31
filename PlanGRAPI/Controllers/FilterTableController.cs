using System;
using DataAccess;
using GRBusiness;
using GRBusiness.PlanGoodsReceive;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlanGRBusiness.PlanGoodsReceive;
using PlanGRBusiness.PlanGoodsReceiveItem;

namespace PlanGRAPI.Controllers
{
    [Route("api/FilterTable")]
    [ApiController]
    public class FilterTableController : ControllerBase
    {
        private PlanGRDbContext context;

        public FilterTableController(PlanGRDbContext context)
        {
            this.context = context;
        }


        #region im_PlanGoodsReceive
        [HttpPost("im_PlanGoodsReceive")]
        public IActionResult im_PlanGoodsReceive([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new DocumentViewModel();
                Models = JsonConvert.DeserializeObject<DocumentViewModel>(body.ToString());
                var result = service.im_PlanGoodsReceive(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region im_PlanGoodsReceiveItem
        [HttpPost("im_PlanGoodsReceiveItem")]
        public IActionResult im_PlanGoodsReceiveItem([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveItemService(context);
                var Models = new DocumentViewModel();
                Models = JsonConvert.DeserializeObject<DocumentViewModel>(body.ToString());
                var result = service.im_PlanGoodsReceiveItem(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion



    }
}