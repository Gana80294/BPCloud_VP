using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BPCloud_VP.SupportDeskService.Models;
using BPCloud_VP.SupportDeskService.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BPCloud_VP.SupportDeskService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SupportDeskController : ControllerBase
    {
        private readonly ISupportDeskRepository _supportDeskRepository;

        public SupportDeskController(ISupportDeskRepository supportDeskRepository)
        {
            _supportDeskRepository = supportDeskRepository;
        }

        [HttpGet]
        public List<SupportMaster> GetSupportMasters()
        {
            try
            {
                var SupportDesk = _supportDeskRepository.GetSupportMasters();
                return SupportDesk;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/GetSupportMasters", ex);
                return null;
            }
        }

        [HttpGet]
        public List<SupportMasterViews> GetSupportMasterViews()
        {
            try
            {
                var SupportDesk = _supportDeskRepository.GetSupportMasterViews();
                return SupportDesk;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/GetSupportMasterViews", ex);
                return null;
            }
        }

        [HttpGet]
        public List<string> GetReasonCodes()
        {
            try
            {
                var reasons = _supportDeskRepository.GetReasonCodes();
                return reasons;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/GetReasonCodes", ex);
                return null;
            }
        }

        [HttpGet]
        public List<SupportAppMaster> GetSupportAppMasters()
        {
            try
            {
                var SupportDesk = _supportDeskRepository.GetSupportAppMasters();
                return SupportDesk;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/GetSupportAppMasters", ex);
                return null;
            }
        }

        [HttpGet]
        public List<SupportMaster> GetSupportMastersByPartnerID(string PartnerID)
        {
            try
            {
                var SupportDesk = _supportDeskRepository.GetSupportMastersByPartnerID(PartnerID);
                return SupportDesk;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/GetSupportMastersByPartnerID", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSupportMaster(SupportMaster supportMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _supportDeskRepository.CreateSupportMaster(supportMaster);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/CreateSupportMaster", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSupportMaster(SupportMaster supportMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _supportDeskRepository.UpdateSupportMaster(supportMaster);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/UpdateSupportMaster", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CloseSupportTicket(SupportHeader header)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _supportDeskRepository.CloseSupportTicket(header);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/CloseSupportTicket", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSupportMaster(SupportMaster supportMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _supportDeskRepository.DeleteSupportMaster(supportMaster);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/DeleteSupportMaster", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public SupportDetails GetSupportDetailsByPartnerAndSupportID(string SupportID, string PartnerID)
        {
            try
            {
                var supportDetails = _supportDeskRepository.GetSupportDetailsByPartnerAndSupportID(SupportID, PartnerID);
                return supportDetails;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/GetSupportDetailsByPartnerAndSupportID", ex);
                return null;
            }
        }

        [HttpGet]
        public SupportDetails GetSupportDetailsByPartnerIDAndDocRefNo(string PartnerID, string DocRefNo)
        {
            try
            {
                var supportDetails = _supportDeskRepository.GetSupportDetailsByPartnerIDAndDocRefNo(PartnerID, DocRefNo);
                return supportDetails;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/GetSupportDetailsByPartnerIDAndDocRefNo", ex);
                return null;
            }
        }

        [HttpGet]
        public List<SupportHeaderView> GetSupportTicketsByPartnerID(string PartnerID)
        {
            try
            {
                var SupportDesk = _supportDeskRepository.GetSupportTicketsByPartnerID(PartnerID);
                return SupportDesk;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/GetSupportTicketsByPartnerID", ex);
                return null;
            }
        }
        [HttpGet]
        public List<SupportHeaderView> GetSupportTicketsByPlants(string GetPlantByUser)
        {
            try
            {
                var SupportDesk = _supportDeskRepository.GetSupportTicketsByPlants(GetPlantByUser);
                return SupportDesk;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/GetSupportTicketsByPartnerID", ex);
                return null;
            }
        }

        [HttpPost]
        public List<SupportHeaderView> GetTicketSearch(Ticketsearch Ticketsearch)
        {
            try
            {
                var SupportDesk = _supportDeskRepository.GetTicketSearch(Ticketsearch);
                return SupportDesk;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/GetSupportTicketsByPartnerID", ex);
                return null;
            }
        }
        [HttpGet]
        public SupportDetails GetSupportDetailsByPartnerAndSupportIDAndType(string SupportID, string PartnerID, string Type)
        {
            try
            {
                var supportDetails = _supportDeskRepository.GetSupportDetailsByPartnerAndSupportIDAndType(SupportID, PartnerID, Type);
                return supportDetails;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/GetSupportDetailsByPartnerAndSupportIDAndType", ex);
                return null;
            }
        }

        [HttpGet]
        public SupportDetails GetSupportDetailsBySupportID(string SupportID)
        {
            try
            {
                var supportDetails = _supportDeskRepository.GetSupportDetailsBySupportID(SupportID);
                return supportDetails;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/GetSupportDetailsBySupportID", ex);
                return null;
            }
        }

        [HttpGet]
        public SupportDetails GetSupportDetailsByPartnerIDAndDocRefNoAndType(string PartnerID, string DocRefNo, string Type)
        {
            try
            {
                var supportDetails = _supportDeskRepository.GetSupportDetailsByPartnerIDAndDocRefNoAndType(PartnerID, DocRefNo, Type);
                return supportDetails;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/GetSupportDetailsByPartnerIDAndDocRefNoAndType", ex);
                return null;
            }
        }

        [HttpGet]
        public List<SupportHeaderView> GetSupportTicketsByPartnerIDAndType(string PartnerID, string Type)
        {
            try
            {
                var SupportDesk = _supportDeskRepository.GetSupportTicketsByPartnerIDAndType(PartnerID, Type);
                return SupportDesk;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/GetSupportTicketsByPartnerIDAndType", ex);
                return null;
            }
        }

        [HttpGet]
        public List<SupportHeaderView> GetBuyerSupportTickets(string PartnerID, string Plant)
        {
            try
            {
                var SupportDesk = _supportDeskRepository.GetBuyerSupportTickets(PartnerID, Plant);
                return SupportDesk;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/GetBuyerSupportTickets", ex);
                return null;
            }
        }

        [HttpPost]
        public List<SupportHeaderView> GetHelpDeskAdminSupportTickets(HelpDeskAdminDetails helpDeskAdminDetails)
        {
            try
            {
                var SupportDesk = _supportDeskRepository.GetHelpDeskAdminSupportTickets(helpDeskAdminDetails);
                return SupportDesk;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/GetHelpDeskAdminSupportTickets", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSupportTicket(SupportHeaderView supportHeader)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _supportDeskRepository.CreateSupportTicket(supportHeader);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/CreateSupportTicket", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public List<SupportLog> GetSupportLogsByPartnerAndSupportID(string SupportID, string PartnerID)
        {
            try
            {
                var SupportDesk = _supportDeskRepository.GetSupportLogsByPartnerAndSupportID(SupportID, PartnerID);
                return SupportDesk;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/GetSupportLogsByPartnerAndSupportID", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSupportLog(SupportLog supportLog)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _supportDeskRepository.CreateSupportLog(supportLog);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/CreateSupportLog", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSupportLog(SupportLogView supportLog)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _supportDeskRepository.UpdateSupportLog(supportLog);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/UpdateSupportLog", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ReOpenSupportTicket(SupportLogView supportLog)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _supportDeskRepository.ReOpenSupportTicket(supportLog);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/ReOpenSupportTicket", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddSupportAttachment()
        {
            try
            {
                var request = Request;
                var SupportID = request.Form["SupportID"].ToString();
                var CreatedBy = request.Form["CreatedBy"].ToString();
                IFormFileCollection postedfiles = request.Form.Files;
                List<BPCSupportAttachment> BPAttachments = new List<BPCSupportAttachment>();
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
                                    BPCSupportAttachment BPAttachment = new BPCSupportAttachment();
                                    BPAttachment.AttachmentName = FileName;
                                    BPAttachment.ContentType = ContentType;
                                    BPAttachment.ContentLength = ContentLength;
                                    BPAttachment.AttachmentFile = fileBytes;
                                    BPAttachments.Add(BPAttachment);
                                }
                            }

                        }

                    }
                    if (BPAttachments.Count > 0)
                    {
                        await _supportDeskRepository.AddSupportAttachment(BPAttachments, SupportID);
                    }
                }

            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/AddSupportAttachment", ex);
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddSupportLogAttachment()
        {
            try
            {
                var request = Request;
                var SupportID = request.Form["SupportID"].ToString();
                var SupportLogID = request.Form["SupportLogID"].ToString();
                var CreatedBy = request.Form["CreatedBy"].ToString();
                IFormFileCollection postedfiles = request.Form.Files;
                List<BPCSupportAttachment> BPAttachments = new List<BPCSupportAttachment>();
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
                                    BPCSupportAttachment BPAttachment = new BPCSupportAttachment();
                                    BPAttachment.AttachmentName = FileName;
                                    BPAttachment.ContentType = ContentType;
                                    BPAttachment.ContentLength = ContentLength;
                                    BPAttachment.AttachmentFile = fileBytes;
                                    BPAttachments.Add(BPAttachment);
                                }
                            }

                        }
                    }
                    if (BPAttachments.Count > 0)
                    {
                        await _supportDeskRepository.AddSupportLogAttachment(BPAttachments, SupportID, SupportLogID);
                    }
                }

            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/AddSupportLogAttachment", ex);
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SaveFAQAttachment()
        {
            try
            {
                var request = Request;
                var CreatedBy = request.Form["CreatedBy"].ToString();
                IFormFileCollection postedfiles = request.Form.Files;
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
                                    BPCFAQAttachment BPAttachment = new BPCFAQAttachment();
                                    BPAttachment.AttachmentName = FileName;
                                    BPAttachment.ContentType = ContentType;
                                    BPAttachment.ContentLength = ContentLength;
                                    BPAttachment.AttachmentFile = fileBytes;
                                    await _supportDeskRepository.SaveFAQAttachment(BPAttachment);
                                }
                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/SaveFAQAttachment", ex);
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpGet]
        public IActionResult GetFAQAttachment()
        {
            try
            {
                BPCFAQAttachment BPAttachment = _supportDeskRepository.GetFAQAttachment();
                if (BPAttachment != null && BPAttachment.AttachmentFile.Length > 0)
                {
                    Stream stream = new MemoryStream(BPAttachment.AttachmentFile);
                    return File(BPAttachment.AttachmentFile, "application/octet-stream", BPAttachment.AttachmentName);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/GetRFQAttachment", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult DownloadSupportLogAttachment(string AttachmentName, string SupportLogID)
        {
            try
            {
                BPCSupportAttachment BPAttachment = _supportDeskRepository.GetAttachmentByName(AttachmentName, SupportLogID);
                if (BPAttachment != null && BPAttachment.AttachmentFile.Length > 0)
                {
                    Stream stream = new MemoryStream(BPAttachment.AttachmentFile);
                    return File(BPAttachment.AttachmentFile, "application/octet-stream", BPAttachment.AttachmentName);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/DownloadSupportLogAttachment", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult DownloadSupportAttachment(string AttachmentName, string SupportID)
        {
            try
            {
                BPCSupportAttachment BPAttachment = _supportDeskRepository.GetAttachmentByName(AttachmentName, SupportID);
                if (BPAttachment != null && BPAttachment.AttachmentFile.Length > 0)
                {
                    Stream stream = new MemoryStream(BPAttachment.AttachmentFile);
                    return File(BPAttachment.AttachmentFile, "application/octet-stream", BPAttachment.AttachmentName);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/DownloadSupportAttachment", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public List<BPCSupportAttachment> GetSupportAttachments()
        {
            try
            {
                var SupportDesk = _supportDeskRepository.GetSupportAttachments();
                return SupportDesk;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("SupportDesk/GetSupportAttachments", ex);
                return null;
            }
        }



    }
}