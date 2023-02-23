using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PlanGRBusiness.Demo;

namespace PlanGRAPI.Controllers
{
    [Route("api/InterfaceInbound")]
    [ApiController]
    public class DemoController : Controller
    {
        private PlanGRDbContext context;

        private readonly IHostingEnvironment _hostingEnvironment;
        public DemoController(PlanGRDbContext context, IHostingEnvironment hostingEnvironment)
        {
            this.context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        #region ASN
        [HttpPost("ASN")]
        public IActionResult CreateASN(DemoASNRequestViewModel model)
        {
            try
            {
                var service = new DemoService(context);
                var Result = service.CreateASN(model);
                return Ok(Result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        [HttpGet("Callback_TMS")]
        public IActionResult Callback_TMS(Guid id)
        {
            try
            {
                var service = new DemoService(context);
                var Result = service.Callback_TMS(id);
                return Ok(Result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}