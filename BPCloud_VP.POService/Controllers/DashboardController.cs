using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BPCloud_VP_POService.Models;
using BPCloud_VP_POService.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BPCloud_VP_POService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardRepositorie _dashboardRepositorie;
        private readonly IAIACTRepository _AIACTRepository;


        public DashboardController(IDashboardRepositorie dashboardRepositorie, IAIACTRepository aIACTRepository)
        {
            _dashboardRepositorie = dashboardRepositorie;
            _AIACTRepository = aIACTRepository;
        }

        #region Order Fulfilment(OFHeader)

        [HttpGet]
        public List<BPCOFHeaderView> GetOfsByPartnerID(string PartnerID)
        {
            try
            {
                var ofHeaders = _dashboardRepositorie.GetOfsByPartnerID(PartnerID);
                return ofHeaders;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/GetOfsByPartnerID", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCOFHeaderView> GetOfPODetails(string GetPlantByUser)
        {
            try
            {
                var ofHeaders = _dashboardRepositorie.GetOfPODetails(GetPlantByUser);
                return ofHeaders;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/GetOfsByPartnerID", ex);
                return null;
            }
        }

        [HttpPost]
        public List<BPCOFHeaderView> GetOfsByOption(OfOption ofOption)
        {
            try
            {
                var OfHeaders = _dashboardRepositorie.GetOfsByOption(ofOption);
                return OfHeaders;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/GetOfsByOption", ex);
                return null;
            }
        }
        [HttpGet]
        public List<FulfilmentDetails> GetVendorDoughnutChartData(string PartnerID)
        {
            try
            {
                var fulfilmentDetails = _dashboardRepositorie.GetVendorDoughnutChartData(PartnerID);
                return fulfilmentDetails;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/GetVendorDoughnutChartData", ex);
                return null;
            }
        }
        //[HttpGet]
        //public List<FulfilmentDetails> GetVendorDoughnutChartDataByUser(string Plant)
        //{
        //    try
        //    {
        //        var fulfilmentDetails = _dashboardRepositorie.GetVendorDoughnutChartDataByUser(Plant);
        //        return fulfilmentDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog.WriteToFile("Dashboard/GetVendorDoughnutChartData", ex);
        //        return null;
        //    }
        //}
        [HttpGet]
        public List<FulfilmentDetails> GetVendorDoughnutChartDataByPlant(string GetPlantByUser)
        {
            try
            {
                var fulfilmentDetails = _dashboardRepositorie.GetVendorDoughnutChartDataByPlant(GetPlantByUser);
                return fulfilmentDetails;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/GetVendorDoughnutChartData", ex);
                return null;
            }
        }

        [HttpGet]
        public FulfilmentStatus GetOfStatusByPartnerID(string PartnerID)
        {
            try
            {
                var fulfilmentStatus = _dashboardRepositorie.GetOfStatusByPartnerID(PartnerID);
                return fulfilmentStatus;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/GetOfStatusByPartnerID", ex);
                return null;
            }
        }

        [HttpGet]
        public List<ItemDetails> GetOfItemsByPartnerIDAndDocNumber(string PartnerID, string DocNumber)
        {
            try
            {
                var itemDetails = _dashboardRepositorie.GetOfItemsByPartnerIDAndDocNumber(PartnerID, DocNumber);
                return itemDetails;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/GetOfItemsByPartnerIDAndDocNumber", ex);
                return null;
            }
        }

        [HttpGet]
        public List<ASNDetails> GetOfASNsByPartnerIDAndDocNumber(string PartnerID, string DocNumber)
        {
            try
            {
                var aSNDetails = _dashboardRepositorie.GetOfASNsByPartnerIDAndDocNumber(PartnerID, DocNumber);
                return aSNDetails;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/GetOfASNsByPartnerIDAndDocNumber", ex);
                return null;
            }
        }

        [HttpGet]
        public List<GRNDetails> GetOfGRGIsByPartnerIDAndDocNumber(string PartnerID, string DocNumber)
        {
            try
            {
                var gRNDetails = _dashboardRepositorie.GetOfGRGIsByPartnerIDAndDocNumber(PartnerID, DocNumber);
                return gRNDetails;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/GetOfGRGIsByPartnerIDAndDocNumber", ex);
                return null;
            }
        }
        [HttpGet]
        public List<GRNDetails> GetGRGIsByPartnerIDAndDocNumber(string PartnerID, string DocNumber)
        {
            try
            {
                var gRNDetails = _dashboardRepositorie.GetGRGIsByPartnerIDAndDocNumber(PartnerID, DocNumber);
                return gRNDetails;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/GetOfGRGIsByPartnerIDAndDocNumber", ex);
                return null;
            }
        }
        [HttpGet]
        public List<GRNDetails> GetCancelGRGIsByPartnerIDAndDocNumber(string PartnerID, string DocNumber)
        {
            try
            {
                var gRNDetails = _dashboardRepositorie.GetCancelGRGIsByPartnerIDAndDocNumber(PartnerID, DocNumber);
                return gRNDetails;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/GetOfGRGIsByPartnerIDAndDocNumber", ex);
                return null;
            }
        }
        [HttpGet]
        public List<QADetails> GetOfQMsByPartnerIDAndDocNumber(string PartnerID, string DocNumber)
        {
            try
            {
                var qADetails = _dashboardRepositorie.GetOfQMsByPartnerIDAndDocNumber(PartnerID, DocNumber);
                return qADetails;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/GetOfQMsByPartnerIDAndDocNumber", ex);
                return null;
            }
        }

        [HttpGet]
        public List<SLDetails> GetOfSLsByPartnerIDAndDocNumber(string PartnerID, string DocNumber)
        {
            try
            {
                var scheduleLineDetails = _dashboardRepositorie.GetOfSLsByPartnerIDAndDocNumber(PartnerID, DocNumber);
                return scheduleLineDetails;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/GetOfSLsByPartnerIDAndDocNumber", ex);
                return null;
            }
        }

        [HttpGet]
        public List<DocumentDetails> GetOfDocumentsByPartnerIDAndDocNumber(string PartnerID, string DocNumber)
        {
            try
            {
                var documentDetails = _dashboardRepositorie.GetOfDocumentsByPartnerIDAndDocNumber(PartnerID, DocNumber);
                return documentDetails;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/GetOfDocumentsByPartnerIDAndDocNumber", ex);
                return null;
            }
        }

        [HttpGet]
        public List<FlipDetails> GetOfFlipsByPartnerIDAndDocNumber(string PartnerID, string DocNumber)
        {
            try
            {
                var flipDetails = _dashboardRepositorie.GetOfFlipsByPartnerIDAndDocNumber(PartnerID, DocNumber);
                return flipDetails;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/GetOfFlipsByPartnerIDAndDocNumber", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadOfAttachment()
        {
            BPCAttachment bPCAttachment = new BPCAttachment();
            try
            {
                var request = Request;
                var DocNumber = request.Form["DocNumber"].ToString();
                var PartnerID = request.Form["PartnerID"].ToString();
                var CreatedBy = request.Form["CreatedBy"].ToString();
                //IFormFileCollection file =
                IFormFileCollection postedfiles = request.Form.Files;
                BPCAttachment BPAttachment = new BPCAttachment();
                if (postedfiles.Count > 0)
                {
                    for (int i = 0; i < postedfiles.Count; i++)
                    {
                        var FileName = postedfiles[i].FileName;
                        var ContentType = postedfiles[i].ContentType;
                        var ContentLength = postedfiles[i].Length;
                        using (Stream st = postedfiles[i].OpenReadStream())
                        {
                            using (BinaryReader br = new BinaryReader(st))
                            {
                                byte[] fileBytes = br.ReadBytes((Int32)st.Length);
                                if (fileBytes.Length > 0)
                                {
                                    BPAttachment.AttachmentName = FileName;
                                    BPAttachment.ContentType = ContentType;
                                    BPAttachment.ContentLength = ContentLength;
                                    BPAttachment.AttachmentFile = fileBytes;
                                }
                            }

                        }

                    }
                    if (!string.IsNullOrEmpty(BPAttachment.AttachmentName))
                    {
                        bPCAttachment = await _dashboardRepositorie.UploadOfAttachment(PartnerID, BPAttachment, DocNumber);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/UploadOfAttachment", ex);
                return BadRequest(ex.Message);
            }
            return Ok(bPCAttachment);
        }

        //[HttpPost]
        //public async Task<IActionResult> UploadOfAttachmentByDocNumber()
        //{
        //    BPCAttachment bPCAttachment = new BPCAttachment();
        //    try
        //    {
        //        var request = Request;
        //        var DocNumber = request.Form["DocNumber"].ToString();
        //       // var PartnerID = request.Form["PartnerID"].ToString();
        //        var CreatedBy = request.Form["CreatedBy"].ToString();
        //        //IFormFileCollection file =
        //        IFormFileCollection postedfiles = request.Form.Files;
        //        BPCAttachment BPAttachment = new BPCAttachment();
        //        if (postedfiles.Count > 0)
        //        {
        //            for (int i = 0; i < postedfiles.Count; i++)
        //            {
        //                var FileName = postedfiles[i].FileName;
        //                var ContentType = postedfiles[i].ContentType;
        //                var ContentLength = postedfiles[i].Length;
        //                using (Stream st = postedfiles[i].OpenReadStream())
        //                {
        //                    using (BinaryReader br = new BinaryReader(st))
        //                    {
        //                        byte[] fileBytes = br.ReadBytes((Int32)st.Length);
        //                        if (fileBytes.Length > 0)
        //                        {
        //                            BPAttachment.AttachmentName = FileName;
        //                            BPAttachment.ContentType = ContentType;
        //                            BPAttachment.ContentLength = ContentLength;
        //                            BPAttachment.AttachmentFile = fileBytes;
        //                        }
        //                    }

        //                }

        //            }
        //            if (!string.IsNullOrEmpty(BPAttachment.AttachmentName))
        //            {
        //                bPCAttachment = await _dashboardRepositorie.UploadOfAttachmentByDocNumber(BPAttachment, DocNumber);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog.WriteToFile("Dashboard/UploadOfAttachment", ex);
        //        return BadRequest(ex.Message);
        //    }
        //    return Ok(bPCAttachment);
        //}

        [HttpGet]
        public IActionResult DownloadOfAttachment(string PartnerID, string AttachmentName, string DocNumber)
        {
            try
            {
                BPCAttachment BPAttachment = _dashboardRepositorie.GetAttachmentByName(PartnerID, AttachmentName, DocNumber);
                if (BPAttachment != null && BPAttachment.AttachmentFile.Length > 0)
                {
                    Stream stream = new MemoryStream(BPAttachment.AttachmentFile);
                    return File(BPAttachment.AttachmentFile, "application/octet-stream", BPAttachment.AttachmentName);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/DownloadOfAttachment", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult DownloadOfAttachmentByDocNumber(string AttachmentName, string DocNumber)
        {
            try
            {
                BPCAttachment BPAttachment = _dashboardRepositorie.DownloadOfAttachmentByDocNumber(AttachmentName, DocNumber);
                if (BPAttachment != null && BPAttachment.AttachmentFile.Length > 0)
                {
                    Stream stream = new MemoryStream(BPAttachment.AttachmentFile);
                    return File(BPAttachment.AttachmentFile, "application/octet-stream", BPAttachment.AttachmentName);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/DownloadOfAttachment", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetOfAttachmentByPartnerIDAndDocNumber(string PartnerID, string DocNumber)
        {
            try
            {
                BPCInvoiceAttachment BPAttachment = _dashboardRepositorie.GetOfAttachmentByPartnerIDAndDocNumber(PartnerID, DocNumber);
                return Ok(BPAttachment);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/GetOfAttachmentByPartnerIDAndDocNumber", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetOfAttachmentsByPartnerIDAndDocNumber(string PartnerID, string DocNumber)
        {
            try
            {
                List<BPCInvoiceAttachment> BPAttachments = _dashboardRepositorie.GetOfAttachmentsByPartnerIDAndDocNumber(PartnerID, DocNumber);
                return Ok(BPAttachments);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/GetOfAttachmentsByPartnerIDAndDocNumber", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult GetOfAttachmentsByDocNumber(string DocNumber)
        {
            try
            {
                List<BPCInvoiceAttachment> BPAttachments = _dashboardRepositorie.GetOfAttachmentsByDocNumber(DocNumber);
                return Ok(BPAttachments);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/GetOfAttachmentsByPartnerIDAndDocNumber", ex);
                return BadRequest(ex.Message);
            }
        }


        #endregion

        #region AIACT

        [HttpGet]
        public List<BPCOFAIACT> GetAllAIACTs()
        {
            try
            {
                var AIACTs = _AIACTRepository.GetAllAIACTs();
                return AIACTs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/GetAllAIACTs", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCOFAIACT> GetAIACTsByPartnerID(string PartnerID)
        {
            try
            {
                var AIACTs = _AIACTRepository.GetAIACTsByPartnerID(PartnerID);
                return AIACTs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/GetAIACTsByPartnerID", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCOFAIACT> GetActionsByPartnerID(string PartnerID)
        {
            try
            {
                var AIACTs = _AIACTRepository.GetActionsByPartnerID(PartnerID);
                return AIACTs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/GetActionsByPartnerID", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCOFAIACT> GetNotificationsByPartnerID(string PartnerID)
        {
            try
            {
                var AIACTs = _AIACTRepository.GetNotificationsByPartnerID(PartnerID);
                return AIACTs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/GetNotificationsByPartnerID", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateNotification(BPCOFAIACT AIACT)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _AIACTRepository.UpdateNotification(AIACT);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/UpdateNotification", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAIACT(BPCOFAIACT AIACT)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _AIACTRepository.CreateAIACT(AIACT);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/CreateAIACT", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAIACT(BPCOFAIACT AIACT)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _AIACTRepository.UpdateAIACT(AIACT);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/UpdateAIACT", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAIACT(BPCOFAIACT AIACT)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _AIACTRepository.DeleteAIACT(AIACT);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/DeleteAIACT", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AcceptAIACT(BPCOFAIACT AIACT)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _AIACTRepository.AcceptAIACT(AIACT);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/AcceptAIACT", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AcceptAIACTs([FromBody] List<BPCOFAIACT> AIACTs)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _AIACTRepository.AcceptAIACTs(AIACTs);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/AcceptAIACTs", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RejectAIACT(BPCOFAIACT AIACT)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _AIACTRepository.RejectAIACT(AIACT);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/RejectAIACT", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Ramanji Methods

        [HttpGet]
        public List<PODetails> GetPODetails(string PatnerID)
        {
            try
            {
                var podetails = _dashboardRepositorie.GetPODetails(PatnerID);
                return podetails;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/GetPODetails", ex);
                return null;
            }
        }

        [HttpPost]
        public List<PODetails> GetAllPOBasedOnDate(POSearch pOSearch)
        {
            try
            {
                var podetails = _dashboardRepositorie.GetAllPOBasedOnDate(pOSearch);
                return podetails;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/GetPODetails", ex);
                return null;
            }
        }

        [HttpGet]
        public OrderFulfilmentDetails GetOrderFulfilmentDetails(string PO, string PatnerID)
        {
            try
            {
                var orderFulfilmentDetails = _dashboardRepositorie.GetOrderFulfilmentDetails(PO, PatnerID);
                return orderFulfilmentDetails;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/GetOrderFulfilmentDetails", ex);
                return null;
            }
        }
        [HttpGet]
        public OrderFulfilmentDetails GetOrderFulfilmentDetailsByDocNumber(string PO)
        {
            try
            {
                var orderFulfilmentDetails = _dashboardRepositorie.GetOrderFulfilmentDetailsByDocNumber(PO);
                return orderFulfilmentDetails;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/GetOrderFulfilmentDetails", ex);
                return null;
            }
        }
        
        [HttpGet]
        public BPCPlantMaster GetItemPlantDetails(string PlantCode)
        {
            try
            {
                var PlantDetails = _dashboardRepositorie.GetItemPlantDetails(PlantCode);
                return PlantDetails;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/GetItemPlantDetails", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAcknowledgement(Acknowledgement acknowledgement)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _dashboardRepositorie.CreateAcknowledgement(acknowledgement);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/CreateAcknowledgement", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePOItems(Acknowledgement acknowledgement)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _dashboardRepositorie.UpdatePOItems(acknowledgement);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/UpdatePOItems", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        [HttpGet]
        public List<SODetails> GetSODetails(string Client, string Company, string Type, string PatnerID)
        {
            try
            {
                var sodetails = _dashboardRepositorie.GetSODetails(Client, Company,Type, PatnerID);
                return sodetails;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/SODetails", ex);
                return null;
            }
        }
        #region document SOLookUp
      /*  public List<BPCAttachment> GetAttachmentByPatnerIdAndDocNum(string PatnerID, string DocNumber)
        {
            try
            {
                var doc_result = _dashboardRepositorie.GetAttachmentByPatnerIdAndDocNum(PatnerID, DocNumber);
                return doc_result;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/GetAttachmentByPatnerIdAndDocNum", ex);
                return null;
            }
        }*/
        #endregion
        [HttpGet]
        public BPCOFHeader GetBPCOFHeader(string partnerId, string ReferenceNo)
        {
            try
            {
                var Header = _dashboardRepositorie.GetBPCOFHeader(partnerId, ReferenceNo);
                return Header;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/GetBPCOFHeader", ex);
                return null;
            }
        }
        [HttpGet]
        public BPCOFHeader GetBPCOFHeaderByDocNumber(string ReferenceNo)
        {
            try
            {
                var Header = _dashboardRepositorie.GetBPCOFHeaderByDocNumber(ReferenceNo);
                return Header;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/GetBPCOFHeader", ex);
                return null;
            }
        }
        [HttpGet]
        public List<SODetails> GetFilteredSODetailsByPartnerID(string Type, string PartnerID, DateTime? FromDate = null, DateTime? ToDate = null, string Status = null)
        {
            try
            {
                var sodetails = _dashboardRepositorie.GetFilteredSODetailsByPartnerID(Type, PartnerID, FromDate, ToDate, Status);
                return sodetails;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/GetFilteredSODetailsByPartnerID", ex);
                return null;
            }
        }
    }
}