using System;
using System.IO;
using System.Net;
using DataAccess;
using GRBusiness;
using GRBusiness.PlanGoodsReceive;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlanGRBusiness.ModelConfig;
using PlanGRBusiness.PlanGoodsReceive;
using PlanGRBusiness.Reports;
using PTTPL.OMS.Business.Documents;
using PTTPL.TMS.Business.Common;
using PTTPL.TMS.Business.ViewModels;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;
using MasterDataBusiness.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PlanGRAPI.Controllers
{
    [Route("api/PlanGoodsReceive")]
    public class PlanGoodsReceiveController : Controller
    {
        private PlanGRDbContext context;

        private readonly IHostingEnvironment _hostingEnvironment;
        public PlanGoodsReceiveController(PlanGRDbContext context, IHostingEnvironment hostingEnvironment)
        {
            this.context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        //private readonly IHostingEnvironment _hostingEnvironment;

        //public PlanGoodsReceiveController(IHostingEnvironment hostingEnvironment)
        //{
        //    _hostingEnvironment = hostingEnvironment;
        //}

        #region filter
        [HttpPost("filter")]
        public IActionResult filter([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new SearchDetailModel();
                Models = JsonConvert.DeserializeObject<SearchDetailModel>(body.ToString());
                var result = service.filter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        #region filter_Exprot
        [HttpPost("filter_Exprot")]
        public IActionResult filter_Exprot([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new SearchDetailModel();
                Models = JsonConvert.DeserializeObject<SearchDetailModel>(body.ToString());
                var result = service.filter_Exprot(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        [HttpPost("filter_in")]
        public IActionResult FilterInClause([FromBody]JObject body)
        {
            try
            {
                PlanGoodsReceiveService service = new PlanGoodsReceiveService(context);
                SearchPlanGoodsReceiveInClauseViewModel Models = JsonConvert.DeserializeObject<SearchPlanGoodsReceiveInClauseViewModel>(body is null ? string.Empty : body.ToString());
                var result = service.FilterInClause(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region CreateOrUpdate
        [HttpPost("createOrUpdate")]
        public IActionResult CreateOrUpdate([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new PlanGoodsReceiveDocViewModel();
                Models = JsonConvert.DeserializeObject<PlanGoodsReceiveDocViewModel>(body.ToString());
                var result = service.CreateOrUpdate(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region find
        [HttpGet("find/{id}")]
        public IActionResult find(Guid id)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var result = service.find(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region Delete
        [HttpPost("delete")]
        public IActionResult Delete([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
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

        #region ConfirmStatus
        [HttpPost("confirmStatus")]
        public IActionResult ConfirmStatus([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new PlanGoodsReceiveDocViewModel();
                Models = JsonConvert.DeserializeObject<PlanGoodsReceiveDocViewModel>(body.ToString());
                var result = service.confirmStatus(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region CloseDocument
        [HttpPost("closeDocument/")]
        public IActionResult CloseDocument([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new PlanGoodsReceiveDocViewModel();
                Models = JsonConvert.DeserializeObject<PlanGoodsReceiveDocViewModel>(body.ToString());
                var result = service.closeDocument(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region UpdatePlanGRStatus
        [HttpPost("UpdatePlanGRStatus/")]
        public IActionResult UpdatePlanGRStatus([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new PlanGoodsReceiveDocViewModel();
                Models = JsonConvert.DeserializeObject<PlanGoodsReceiveDocViewModel>(body.ToString());
                var result = service.updatePlanGRStatus(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region updateUserAssign
        [HttpPost("updateUserAssign")]
        public IActionResult updateUserAssign([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new PlanGoodsReceiveDocViewModel();
                Models = JsonConvert.DeserializeObject<PlanGoodsReceiveDocViewModel>(body.ToString());
                var result = service.updateUserAssign(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region deleteUserAssign
        [HttpPost("deleteUserAssign")]
        public IActionResult deleteUserAssign([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new PlanGoodsReceiveDocViewModel();
                Models = JsonConvert.DeserializeObject<PlanGoodsReceiveDocViewModel>(body.ToString());
                var result = service.deleteUserAssign(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region CheckDocumentStatus
        [HttpPost("checkDocumentStatus/")]
        public IActionResult CheckDocumentStatus([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new CheckDocumentStatusViewModel();
                Models = JsonConvert.DeserializeObject<CheckDocumentStatusViewModel>(body.ToString());
                var result = service.CheckDocumentStatus(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region getScanPlanGRI
        [HttpPost("getScanPlanGRI")]
        public IActionResult getScanPlanGRI([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new PlanGoodsReceiveItemViewModel();
                Models = JsonConvert.DeserializeObject<PlanGoodsReceiveItemViewModel>(body.ToString());
                var result = service.getScanPlanGRI(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region ScanPlanGR
        [HttpPost("ScanPlanGR")]
        public IActionResult ScanPlanGR([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new ScanPlanGoodsReceiveViewModel();
                Models = JsonConvert.DeserializeObject<ScanPlanGoodsReceiveViewModel>(body.ToString());
                var result = service.ScanPlanGR(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        [HttpPost("PrintPlanGoodsReceive")]
        public IActionResult PrintPlanGoodsReceive([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new ReportPlanGoodsReceiveViewModel();
                Models = JsonConvert.DeserializeObject<ReportPlanGoodsReceiveViewModel>(body.ToString());
                localFilePath = service.PrintPlanGoodsReceive(Models, _hostingEnvironment.ContentRootPath);
                if (!System.IO.File.Exists(localFilePath))
                {
                    return NotFound();
                }
                return File(System.IO.File.ReadAllBytes(localFilePath), "application/octet-stream");
                //return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally
            {
                System.IO.File.Delete(localFilePath);
            }
        }


        [HttpPost("PrintReportDN")]
        public IActionResult PrintReportDN([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new ReportDNViewModel();
                Models = JsonConvert.DeserializeObject<ReportDNViewModel>(body.ToString());
                localFilePath = service.PrintReportDN(Models, _hostingEnvironment.ContentRootPath);
                if (!System.IO.File.Exists(localFilePath))
                {
                    return NotFound();
                }
                return File(System.IO.File.ReadAllBytes(localFilePath), "application/octet-stream");
                //return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally
            {
                System.IO.File.Delete(localFilePath);
            }
        }
        #region Cancel
        [HttpPost("Cancel")]
        public IActionResult Cancel([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new PlanGoodsReceiveDocViewModel();
                Models = JsonConvert.DeserializeObject<PlanGoodsReceiveDocViewModel>(body.ToString());
                var result = service.Cancel(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region genDocumentNo
        [HttpPost("genDocumentNo")]
        public IActionResult genDocumentNo([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new PlanGoodsReceiveDocViewModel();
                Models = JsonConvert.DeserializeObject<PlanGoodsReceiveDocViewModel>(body.ToString());
                var result = service.genDocumentNo(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        //[HttpPost("importFileInbound")]
        //public async Task<IActionResult> importFileInboundAsync([FromBody]JObject body)
        //{
        //    var result = new HttpResponseMessage(HttpStatusCode.OK);
        //    // Check if the request contains multipart/form-data.
        //    string path = "";

        //    if (!AppsInfo.MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
        //    {
        //        ModelState.AddModelError("File",
        //            $"The request couldn't be processed (Error 1).");
        //        // Log error

        //        return BadRequest(ModelState);
        //    }

        //    resultViewModel items = new resultViewModel();
        //    try
        //    {
        //        // Read the form data.
        //        string memoryPath = AppsInfo.upload;
        //        string memoryDocument = AppsInfo.document_upload;
        //        string virtualDocument = AppsInfo.document_path;

        //        string root = _hostingEnvironment.ContentRootPath + memoryPath;

        //        DirectoryInfo dir = new DirectoryInfo(root);
        //        if (!dir.Exists)
        //        {
        //            dir.Create();
        //        }

        //        // path = root;
        //        var provider = new MultipartFormDataStreamProvider(root);
        //        //var task = await Request.Content.ReadAsMultipartAsync(provider);

        //        // string uploadToType = task.FormData["uploadToType"] != null ? task.FormData["uploadToType"].ToString() : "";

        //        path = _hostingEnvironment.ContentRootPath + memoryDocument;

        //        if (provider.FileData.Count == 0)
        //        {
        //            return this.BadRequest("file is null ?");
        //        }
        //        else
        //        {
        //            foreach (MultipartFileData fileData in provider.FileData)
        //            {
        //                string guidName = "";
        //                if (!string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
        //                {
        //                    var item = new fileViewModel();
        //                    guidName = Guid.NewGuid().ToString().ToUpper().Replace("-", "_");
        //                    string extension = fileAppService.getExtension(fileData.Headers.ContentType);

        //                    string fileName = fileData.Headers.ContentDisposition.FileName;

        //                    if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
        //                    {
        //                        fileName = fileName.Trim('"');
        //                    }
        //                    if (fileName.Contains(@"/") || fileName.Contains(@"\"))
        //                    {
        //                        fileName = Path.GetFileName(fileName);
        //                    }

        //                    string newPath = path + guidName + "\\" + fileName;
        //                    string thumbPath = path + guidName + "\\" + "thumb\\" + fileName;

        //                    if (!Directory.Exists(path + guidName + "\\"))
        //                    {
        //                        DirectoryInfo di = Directory.CreateDirectory(path + guidName + "\\");
        //                    }

        //                    System.IO.File.Move(fileData.LocalFileName, newPath);
        //                    if (fileAppService.getTypeImage(fileData.Headers.ContentType))
        //                        fileAppService.setThumbnail(newPath, thumbPath);
        //                    else
        //                        thumbPath = "";

        //                    item.name = fileName;
        //                    item.extension = extension;
        //                    item.virtualPath = virtualDocument;
        //                    item.path = virtualDocument + guidName + "/" + fileName;
        //                    item.orginal = fileName;
        //                    item.thumb = virtualDocument + guidName + "thumb/" + fileName;
        //                    if (thumbPath == "")
        //                        item.fileType = "document";
        //                    else
        //                        item.fileType = "image";
        //                    path = item.path;
        //                    if (path.StartsWith("~"))
        //                    {
        //                        path = path.Substring(1);
        //                    }

        //                }
        //            }
        //            provider.FileData.Clear();
        //        }
        //        items.result = true;
        //        items.value = path;
        //        string baseUrl = AppsInfo.upload_host;
        //        items.url = baseUrl + path;
        //        return this.Ok(items);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return this.BadRequest(ex.Message);
        //    }
        //}

        [HttpPost("importFileInboundV2")]
        public async Task<IActionResult> Index(EngineerVM engineerVM)
        {
            try
            {
                resultViewModel items = new resultViewModel();
                if (engineerVM.File != null)
                {
                    //upload files to wwwroot
                    var fileName = Path.GetFileName(engineerVM.File.FileName);
                    var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Uploads", fileName);

                    string memoryPath = AppsInfo.upload;
                    string memoryDocument = AppsInfo.document_upload;
                    string virtualDocument = AppsInfo.document_path;

                    string root = _hostingEnvironment.ContentRootPath + memoryPath;

                    DirectoryInfo dir = new DirectoryInfo(root);
                    if (!dir.Exists)
                    {
                        dir.Create();
                    }

                    // path = root;
                    var provider = new MultipartFormDataStreamProvider(root);

                    var path = _hostingEnvironment.ContentRootPath + memoryDocument;

                    string guidName = "";
                    using (var fileSteam = new FileStream(filePath, FileMode.Create))
                    {
                        await engineerVM.File.CopyToAsync(fileSteam);
                        var item = new fileViewModel();
                        guidName = Guid.NewGuid().ToString().ToUpper().Replace("-", "_");
                        string extension = fileAppService.getExtension(engineerVM.File.ContentType);

                        if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                        {
                            fileName = fileName.Trim('"');
                        }
                        if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                        {
                            fileName = Path.GetFileName(fileName);
                        }

                        string newPath = path + guidName + "\\" + fileName;
                        string thumbPath = path + guidName + "\\" + "thumb\\" + fileName;

                        if (fileAppService.getTypeImage(engineerVM.File.ContentType))
                            fileAppService.setThumbnail(newPath, thumbPath);
                        else
                            thumbPath = "";

                        item.name = fileName;
                        item.extension = extension;
                        item.virtualPath = virtualDocument;
                        item.path = virtualDocument  + "/" + fileName;
                        item.orginal = fileName;
                        item.thumb = virtualDocument + guidName + "thumb/" + fileName;
                        if (thumbPath == "")
                            item.fileType = "document";
                        else
                            item.fileType = "image";
                        path = item.path;
                        if (path.StartsWith("~"))
                        {
                            path = path.Substring(1);
                        }
                    }
                    //your logic to save filePath to database, for example

                    items.result = true;
                    items.value = filePath;
                    string baseUrl = AppsInfo.upload_host;
                    items.url = baseUrl + path;
                    return this.Ok(items);
                }
                else
                {

                }
                return View();
            }
            catch (System.Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        #region checkProduct
        [HttpPost("checkProduct")]
        public IActionResult checkProduct([FromBody]JObject body)
        {
            try
            {
                var service = new PlanGoodsReceiveService(context);
                var Models = new ProductViewModel();
                Models = JsonConvert.DeserializeObject<ProductViewModel>(body.ToString());
                var result = service.checkProduct(Models);
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
