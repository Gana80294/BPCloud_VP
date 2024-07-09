using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BPCloud_VP_POService.Models;
using BPCloud_VP_POService.Repositories;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace BPCloud_VP_POService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class POController : ControllerBase
    {
        private readonly IPORepository _PORepository;
        private readonly IAttachmentRepository _AttachmentRepository;
        private readonly IFLIPCostRepository _IFLIPCostRepository;

        public POController(IPORepository PORepository, IAttachmentRepository AttachmentRepository, IFLIPCostRepository IFLIPCostRepository)
        {
            _PORepository = PORepository;
            _AttachmentRepository = AttachmentRepository;
            _IFLIPCostRepository = IFLIPCostRepository;
        }

        #region BPCPOHeader

        [HttpGet]
        public List<BPCOFHeader> GetAllPOList()
        {
            try
            {
                var POs = _PORepository.GetAllPOList();
                return POs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetAllPOList", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCOFHeader> FilterPOList(string VendorCode, string PONumber = null, string Status = null, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            try
            {
                var POs = _PORepository.FilterPOList(VendorCode, PONumber, Status, FromDate, ToDate);
                return POs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/FilterPOList", ex);
                return null;
            }
        }

        [HttpGet]
        public BPCOFHeader GetPOByDoc(string DocNumber)
        {
            try
            {
                var POs = _PORepository.GetPOByDoc(DocNumber);
                return POs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetPOByDoc", ex);
                return null;
            }
        }

        [HttpGet]
        public BPCOFHeader GetPOByDocAndPartnerID(string DocNumber, string PartnerID)
        {
            try
            {
                var POs = _PORepository.GetPOByDocAndPartnerID(DocNumber, PartnerID);
                return POs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetPOByDocAndPartnerID", ex);
                return null;
            }
        }

        [HttpPost]
        public List<BPCOFHeader> GetOFByDocAndPartnerID(BPCOFItemViewFilter filter)
        {
            try
            {
                var POs = _PORepository.GetOFByDocAndPartnerID(filter);
                return POs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetOFByDocAndPartnerID", ex);
                return null;
            }
        }

        [HttpGet]
        public BPCOFHeader GetPOPartnerID(string PartnerID)
        {
            try
            {
                var POs = _PORepository.GetPOPartnerID(PartnerID);
                return POs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetPOByDocAndPartnerID", ex);
                return null;
            }
        }

        [HttpGet]
        public IActionResult CreatePOPdf(string DocNumber)
        {
            try
            {
                byte[] bytes = _PORepository.CreatePOPdf(DocNumber);
                if (bytes != null && bytes.Length > 0)
                {
                    DateTime dt = DateTime.Now;
                    string FileName = dt.ToString("yyyyMMddHHmmss") + ".pdf";
                    //string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PdfFiles");
                    //if (!Directory.Exists(path))
                    //{
                    //    Directory.CreateDirectory(path);
                    //}
                    //string FilePath = Path.Combine(path, FileName);
                    //System.IO.File.WriteAllBytes(FilePath, bytes);
                    Stream stream = new MemoryStream(bytes);
                    return File(bytes, "application/octet-stream", FileName);
                };
                return NotFound();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/CreatePOPdf", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult PrintPO(string DocNumber)
        {
            try
            {
                BPCAttachment attachment = _PORepository.PrintPO(DocNumber);
                if (attachment != null && attachment.AttachmentFile.Length > 0)
                {
                    DateTime dt = DateTime.Now;
                    string FileName = attachment.AttachmentName;
                    //string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PdfFiles");
                    //if (!Directory.Exists(path))
                    //{
                    //    Directory.CreateDirectory(path);
                    //}
                    //string FilePath = Path.Combine(path, FileName);
                    //System.IO.File.WriteAllBytes(FilePath, bytes);
                    Stream stream = new MemoryStream(attachment.AttachmentFile);
                    return File(attachment.AttachmentFile, "application/octet-stream", FileName);
                };
                return NotFound();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/PrintPO", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region BPCPOItems

        [HttpGet]
        public List<BPCOFItem> GetPOItemsByDoc(string DocNumber)
        {
            try
            {
                var POItems = _PORepository.GetPOItemsByDoc(DocNumber);
                return POItems;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetPOItemsByPO", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCOFItemSES> GetOFItemSES()
        {
            try
            {
                var POItems = _PORepository.GetOFItemSES();
                return POItems;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetOFItemSES", ex);
                return null;
            }
        }
        [HttpGet]
        public List<BPCOFItemSES> GetOFItemSESByItem(string item, string DocumentNo, string PartnerId)
        {
            try
            {
                var POItems = _PORepository.GetOFItemSESByItem(item, DocumentNo, PartnerId);
                return POItems;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetOFItemSESByItem", ex);
                return null;
            }
        }
        [HttpGet]
        public List<BPCOFItemSES> GetOFItemSESByDocNumber(string item, string DocumentNo)
        {
            try
            {
                var POItems = _PORepository.GetOFItemSESByDocNumber(item, DocumentNo);
                return POItems;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetOFItemSESByItem", ex);
                return null;
            }
        }
        [HttpGet]
        public List<BPCOFItem> GetSupportPOItemsByDoc(string DocNumber)
        {
            try
            {
                var POItems = _PORepository.GetSupportPOItemsByDoc(DocNumber);
                return POItems;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetPOItemsByPO", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCOFItem> GetPOItemsByDocAndPartnerID(string DocNumber, string PartnerID)
        {
            try
            {
                var POItems = _PORepository.GetPOItemsByDocAndPartnerID(DocNumber, PartnerID);
                return POItems;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetPOItemsByDocAndPartnerID", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCOFItemView> GetPOItemViewsByDocAndPartnerID(string DocNumber, string PartnerID)
        {
            try
            {
                var POItems = _PORepository.GetPOItemViewsByDocAndPartnerID(DocNumber, PartnerID);
                return POItems;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetPOItemViewsByDocAndPartnerID", ex);
                return null;
            }
        }
        [HttpGet]
        public List<BPCOFItemView> GetPOItemViewsByDoc(string DocNumber)
        {
            try
            {
                var POItems = _PORepository.GetPOItemViewsByDoc(DocNumber);
                return POItems;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetPOItemViewsByDocAndPartnerID", ex);
                return null;
            }
        }
        [HttpPost]
        public List<BPCOFItemView> GetOFItemViewsForASNByDocAndPartnerID(BPCOFItemViewFilter filter)
        {
            try
            {
                var POItems = _PORepository.GetOFItemViewsForASNByDocAndPartnerID(filter);
                return POItems;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetOFItemViewsForASNByDocAndPartnerID", ex);
                return null;
            }
        }

        [HttpGet]
        public BPCOFScheduleLine GetBPCSLByDocNumber(string docNumber)
        {
            try
            {
                if (docNumber == null)
                {
                    return null;
                }
                var result = _PORepository.GetBPCSLByDocNumber(docNumber);
                return result;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetBPCSLByDocNumber", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBPCH([FromBody] List<BPCOFHeaderWithAttachments> Items)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _PORepository.CreateBPCH(Items);
                return Ok("Data are inserted successfully");
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/CreateBPCH", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateBPCHGateStatus([FromBody] List<BPCOFHeader_Gate> Items)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _PORepository.UpdateBPCHGateStatus(Items);
                return Ok("Data are updated successfully");
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/CreateBPCH", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBPCItems([FromBody] List<BPCOFItem> Items)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _PORepository.CreateBPCItems(Items);
                return Ok("Data are inserted successfully");
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/CreateBPCItems", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOFItemSES([FromBody] List<BPCOFItemSES> Items)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _PORepository.CreateOFItemSES(Items);
                return Ok("Data are inserted successfully");
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/CreateOFItemSES", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBPCSL([FromBody] List<BPCOFScheduleLine> Items)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _PORepository.CreateBPCSL(Items);
                return Ok("Data are inserted successfully");
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/CreateBPCSL", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBPCH([FromBody] List<BPCOFHeader> Items)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _PORepository.UpdateBPCH(Items);
                return Ok("Data are inserted successfully");
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/UpdateBPCH", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBPCHReleaseStatus([FromBody] List<BPCOFReleaseView> data)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _PORepository.UpdateBPCHReleaseStatus(data);
                return Ok("Released status details are updated successfully");
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/UpdateBPCHReleaseStatus", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBPCItems([FromBody] List<BPCOFItem> Items)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _PORepository.UpdateBPCItems(Items);
                return Ok("Data are inserted successfully");
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/UpdateBPCItems", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBPCSL([FromBody] List<BPCOFScheduleLine> Items)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _PORepository.UpdateBPCSL(Items);
                return Ok("Data are inserted successfully");
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/UpdateBPCSL", ex);
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        public SOItemCount GetSOItemCountByDocAndPartnerID(string DocNumber, string PartnerID)
        {
            try
            {
                var Items = _PORepository.GetSOItemCountByDocAndPartnerID(DocNumber, PartnerID);
                return Items;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetSOItemCountByDocAndPartnerID", ex);
                return null;
            }
        }

        #endregion

        [HttpGet]
        public List<POScheduleLineView> GetPOSLByDocAndPartnerID(string DocNumber, string PartnerID)
        {
            try
            {
                var POSLs = _PORepository.GetPOSLByDocAndPartnerID(DocNumber, PartnerID);
                return POSLs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetPOSLByDocAndPartnerID", ex);
                return null;
            }
        }

        //GRR table


        [HttpGet]
        public List<BPCOFGRGI> GetAllGRR()
        {
            try
            {
                var POs = _PORepository.GetAllGRR();
                return POs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToGRGIFile("PO/GetAllGRR", ex);
                return null;
            }
        }
        [HttpGet]
        public List<BPCOFGRGI> GetGRRByPartnerId(string partnerId)
        {
            try
            {
                var POs = _PORepository.GetGRRByPartnerId(partnerId);
                return POs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToGRGIFile("PO/GetGRRByPartnerId", ex);
                return null;
            }
        }
        [HttpGet]
        public List<BPCOFGRGI> FilterGRRListByPartnerID(string PartnerID, string GRGIDoc = null, string DocNumber = null, string Material = null, DateTime? GRIDate = null)
        {
            try
            {
                var data = _PORepository.FilterGRRListByPartnerID(PartnerID, GRGIDoc, DocNumber, Material, GRIDate);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToGRGIFile("PO/FilterASNListByPartnerID", ex);
                return null;
            }
        }
        [HttpGet]
        public List<BPCOFGRGI> GetOfSuperUsergrnDetails(string GetPlantByUser, string GRGIDoc = null, string DocNumber = null, string Material = null, DateTime? GRIDate = null)
        {
            try
            {
                var data = _PORepository.GetOfSuperUsergrnDetails(GetPlantByUser, GRGIDoc, DocNumber, Material, GRIDate);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToGRGIFile("PO/FilterASNListByPartnerID", ex);
                return null;
            }
        }
        [HttpPost]
        public List<BPCOFGRGI> FilterGRGIListByPlants(GRNListFilter filter)
        {
            try
            {
                var data = _PORepository.FilterGRGIListByPlants(filter);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToGRGIFile("PO/FilterGRGIListByPlants", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCOFGRGI> FilterGRRListForBuyer(string GRGIDoc = null, string DocNumber = null, string Material = null, DateTime? ASNFromDate = null, DateTime? ASNToDate = null)
        {
            try
            {
                var data = _PORepository.FilterGRRListForBuyer(GRGIDoc, DocNumber, Material, ASNFromDate, ASNToDate);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToGRGIFile("PO/FilterGRRListForBuyer", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCOFGRGI> GetPOGRGIByDocAndPartnerID(string DocNumber, string PartnerID)
        {
            try
            {
                var Items = _PORepository.GetPOGRGIByDocAndPartnerID(DocNumber, PartnerID);
                return Items;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToGRGIFile("PO/GetPOGRGIByDocAndPartnerID", ex);
                return null;
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateBPCOFGRGI([FromBody] List<BPCOFGRGI> data)
        {
            try
            {
                if (data.Count == 0)
                {
                    return BadRequest(ModelState);
                }
                await _PORepository.CreateBPCOFGRGI(data);
                return Ok("Data are inserted successfully");
            }
            catch (Exception ex)
            {
                WriteLog.WriteToGRGIFile("PO/CreateBPCOFGRGI", ex);
                return BadRequest(ex.Message);

            }
        }
        [HttpPost]
        public async Task<IActionResult> CancelBPCOFGRGI([FromBody] List<BPCOFGRGI> data)
        {
            try
            {
                if (data.Count == 0)
                {
                    return BadRequest(ModelState);
                }
                await _PORepository.CancelBPCOFGRGI(data);
                return Ok("GRN Cancelled successfully");
            }
            catch (Exception ex)
            {
                WriteLog.WriteToGRGIFile("PO/CreateBPCOFGRGI", ex);
                return BadRequest(ex.Message);

            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateOFGRGIs([FromBody] List<BPCOFGRGIXLSX> OFGRGIs)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _PORepository.CreateOFGRGIs(OFGRGIs);
                return Ok();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToGRGIFile("PO/CreateOFGRGIs", ex);
                return BadRequest(ex.Message);
            }
        }


        #region PO FLIP
        [HttpGet]
        public List<BPCFLIPHeaderView> GetPOFLIPsByPartnerID(string PartnerID)
        {
            try
            {
                var POFLIPs = _PORepository.GetPOFLIPsByPartnerID(PartnerID);
                return POFLIPs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetPOFLIPsByPartnerID", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCFLIPHeaderView> GetPOFLIPsBySuperUser(string GetPlantByUser)
        {
            try
            {
                var POFLIPs = _PORepository.GetPOFLIPsBySuperUser(GetPlantByUser);
                return POFLIPs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetPOFLIPsByPartnerID", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCFLIPHeaderView> GetPOFLIPsByDocAndPartnerID(string DocNumber, string PartnerID)
        {
            try
            {
                var POFLIPs = _PORepository.GetPOFLIPsByDocAndPartnerID(DocNumber, PartnerID);
                return POFLIPs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetPOFLIPsByDocAndPartnerID", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePOFLIP(BPCFLIPHeaderView FLIPHeader)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _PORepository.CreatePOFLIP(FLIPHeader);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/CreatePOFLIP", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePOFLIP(BPCFLIPHeaderView FLIPHeader)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _PORepository.UpdatePOFLIP(FLIPHeader);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/UpdatePOFLIP", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeletePOFLIP(BPCFLIPHeaderView FLIPHeader)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _PORepository.DeletePOFLIP(FLIPHeader);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/DeletePOFLIP", ex);
                return BadRequest(ex.Message);
            }
        }

        //public async Task<FileStreamResult> GeneratePDF(BPCFLIPHeaderView FLIPHeader)
        //{
        //    try
        //    {
        //        //return (GeneratePDFBasedonModeL(FLIPHeader));

        //    }
        //    {

        //        WriteLog.WriteToFile("PO/GeneratePDF", ex);
        //        return null;
        //    }
        //}
        [HttpGet]
        public IActionResult GeneratePDF(BPCFLIPHeaderView FLIPHeader)
        {
            try
            {
                Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
                MemoryStream PDFData = new MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, PDFData);
                var titleFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
                var titleFontBlue = FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLUE);
                var boldTableFont = FontFactory.GetFont("Arial", 8, Font.BOLD);
                var bodyFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                var EmailFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLUE);
                BaseColor TabelHeaderBackGroundColor = WebColors.GetRGBColor("#EEEEEE");

                Rectangle pageSize = writer.PageSize;
                // Open the Document for writing
                pdfDoc.Open();
                //Add elements to the document here

                #region Top table
                // Create the header table 
                PdfPTable headertable = new PdfPTable(3);
                headertable.HorizontalAlignment = 0;
                headertable.WidthPercentage = 100;
                headertable.SetWidths(new float[] { 100f, 320f, 100f });  // then set the column's __relative__ widths
                headertable.DefaultCell.Border = Rectangle.NO_BORDER;
                {
                    PdfPCell middlecell = new PdfPCell();
                    middlecell.Border = Rectangle.NO_BORDER;
                    middlecell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                    middlecell.BorderWidthBottom = 1f;
                    headertable.AddCell(middlecell);
                }
                {
                    PdfPTable nested = new PdfPTable(1);
                    nested.DefaultCell.Border = Rectangle.NO_BORDER;
                    PdfPCell nextPostCell1 = new PdfPCell(new Phrase(FLIPHeader.Company, titleFont));
                    nextPostCell1.Border = Rectangle.NO_BORDER;
                    nested.AddCell(nextPostCell1);
                    PdfPCell nextPostCell2 = new PdfPCell(new Phrase("xxx xxx,xxx,xxx,", bodyFont));
                    nextPostCell2.Border = Rectangle.NO_BORDER;
                    nested.AddCell(nextPostCell2);
                    PdfPCell nextPostCell3 = new PdfPCell(new Phrase("(xxx) xxx-xxx", bodyFont));
                    nextPostCell3.Border = Rectangle.NO_BORDER;
                    nested.AddCell(nextPostCell3);
                    PdfPCell nextPostCell4 = new PdfPCell(new Phrase("company@example.com", EmailFont));
                    nextPostCell4.Border = Rectangle.NO_BORDER;
                    nested.AddCell(nextPostCell4);
                    nested.AddCell("");
                    PdfPCell nesthousing = new PdfPCell(nested);
                    nesthousing.Border = Rectangle.NO_BORDER;
                    nesthousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                    nesthousing.BorderWidthBottom = 1f;
                    nesthousing.Rowspan = 5;
                    nesthousing.PaddingBottom = 10f;
                    headertable.AddCell(nesthousing);
                }
                PdfPTable Invoicetable = new PdfPTable(3);
                Invoicetable.HorizontalAlignment = 0;
                Invoicetable.WidthPercentage = 100;
                Invoicetable.SetWidths(new float[] { 100f, 320f, 100f });  // then set the column's __relative__ widths
                Invoicetable.DefaultCell.Border = Rectangle.NO_BORDER;

                {
                    PdfPTable nested = new PdfPTable(1);
                    nested.DefaultCell.Border = Rectangle.NO_BORDER;
                    PdfPCell nextPostCell1 = new PdfPCell(new Phrase("INVOICE TO:", bodyFont));
                    nextPostCell1.Border = Rectangle.NO_BORDER;
                    nested.AddCell(nextPostCell1);
                    PdfPCell nextPostCell2 = new PdfPCell(new Phrase(FLIPHeader.Client, titleFont));
                    nextPostCell2.Border = Rectangle.NO_BORDER;
                    nested.AddCell(nextPostCell2);
                    PdfPCell nextPostCell3 = new PdfPCell(new Phrase("xxx xxxx xxx xx", bodyFont));
                    nextPostCell3.Border = Rectangle.NO_BORDER;
                    nested.AddCell(nextPostCell3);
                    PdfPCell nextPostCell4 = new PdfPCell(new Phrase("xxxx@example.com", EmailFont));
                    nextPostCell4.Border = Rectangle.NO_BORDER;
                    nested.AddCell(nextPostCell4);
                    nested.AddCell("");
                    PdfPCell nesthousing = new PdfPCell(nested);
                    nesthousing.Border = Rectangle.NO_BORDER;
                    //nesthousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                    //nesthousing.BorderWidthBottom = 1f;
                    nesthousing.Rowspan = 5;
                    nesthousing.PaddingBottom = 10f;
                    Invoicetable.AddCell(nesthousing);
                }
                {
                    PdfPCell middlecell = new PdfPCell();
                    middlecell.Border = Rectangle.NO_BORDER;
                    //middlecell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                    //middlecell.BorderWidthBottom = 1f;
                    Invoicetable.AddCell(middlecell);
                }


                {
                    PdfPTable nested = new PdfPTable(1);
                    nested.DefaultCell.Border = Rectangle.NO_BORDER;
                    PdfPCell nextPostCell1 = new PdfPCell(new Phrase("Invoice No:" + FLIPHeader.InvoiceNumber, titleFontBlue));
                    nextPostCell1.Border = Rectangle.NO_BORDER;
                    nested.AddCell(nextPostCell1);
                    PdfPCell nextPostCell2 = new PdfPCell(new Phrase("Date of Invoice: " + FLIPHeader.InvoiceDate, bodyFont));
                    nextPostCell2.Border = Rectangle.NO_BORDER;
                    nested.AddCell(nextPostCell2);
                    PdfPCell nextPostCell3 = new PdfPCell(new Phrase("Invoicd Type: " + FLIPHeader.InvoiceType, bodyFont));
                    nextPostCell3.Border = Rectangle.NO_BORDER;
                    nested.AddCell(nextPostCell3);
                    PdfPCell nextPostCell4 = new PdfPCell(new Phrase("PO Number: " + FLIPHeader.DocNumber, bodyFont));
                    nextPostCell3.Border = Rectangle.NO_BORDER;
                    nested.AddCell(nextPostCell4);
                    nested.AddCell("");
                    PdfPCell nesthousing = new PdfPCell(nested);
                    nesthousing.Border = Rectangle.NO_BORDER;
                    //nesthousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                    //nesthousing.BorderWidthBottom = 1f;
                    nesthousing.Rowspan = 5;
                    nesthousing.PaddingBottom = 10f;
                    Invoicetable.AddCell(nesthousing);
                }
                pdfDoc.Add(headertable);
                Invoicetable.PaddingTop = 10f;

                pdfDoc.Add(Invoicetable);
                #endregion

                #region Items Table
                //Create body table
                PdfPTable itemTable = new PdfPTable(3);

                itemTable.HorizontalAlignment = 0;
                itemTable.WidthPercentage = 100;
                itemTable.SetWidths(new float[] { 5, 40, 10, 20, 25 });  // then set the column's __relative__ widths
                itemTable.SpacingAfter = 40;
                itemTable.DefaultCell.Border = Rectangle.BOX;
                PdfPCell cell1 = new PdfPCell(new Phrase("Type", boldTableFont));
                cell1.BackgroundColor = TabelHeaderBackGroundColor;
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                itemTable.AddCell(cell1);
                PdfPCell cell2 = new PdfPCell(new Phrase("Amount", boldTableFont));
                cell2.BackgroundColor = TabelHeaderBackGroundColor;
                cell2.HorizontalAlignment = 1;
                itemTable.AddCell(cell2);
                PdfPCell cell3 = new PdfPCell(new Phrase("Remark", boldTableFont));
                cell3.BackgroundColor = TabelHeaderBackGroundColor;
                cell3.HorizontalAlignment = Element.ALIGN_CENTER;
                itemTable.AddCell(cell3);
                foreach (var expensevalue in FLIPHeader.FLIPCosts)
                {
                    PdfPCell Type = new PdfPCell(new Phrase(expensevalue.ExpenceType, bodyFont));
                    Type.HorizontalAlignment = 1;
                    Type.PaddingLeft = 10f;
                    Type.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    itemTable.AddCell(Type);

                    PdfPCell amountcell = new PdfPCell(new Phrase("" + expensevalue.Amount, bodyFont));
                    amountcell.HorizontalAlignment = 1;
                    amountcell.PaddingLeft = 10f;
                    amountcell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    itemTable.AddCell(amountcell);

                    PdfPCell remark = new PdfPCell(new Phrase(expensevalue.Remarks, bodyFont));
                    remark.HorizontalAlignment = 1;
                    remark.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    itemTable.AddCell(remark);
                }

                #endregion
                PdfPTable ItedData = new PdfPTable(3);
                {
                    ItedData.HorizontalAlignment = 0;
                    ItedData.WidthPercentage = 100;
                    ItedData.SetWidths(new float[] { 5, 40, 10, 20, 25 });  // then set the column's __relative__ widths
                    ItedData.SpacingAfter = 40;
                    ItedData.DefaultCell.Border = Rectangle.BOX;
                    PdfPCell itemcell1 = new PdfPCell(new Phrase("Item", boldTableFont));
                    itemcell1.BackgroundColor = TabelHeaderBackGroundColor;
                    itemcell1.HorizontalAlignment = Element.ALIGN_CENTER;
                    ItedData.AddCell(itemcell1);
                    PdfPCell Iteml2 = new PdfPCell(new Phrase("Material Text", boldTableFont));
                    Iteml2.BackgroundColor = TabelHeaderBackGroundColor;
                    Iteml2.HorizontalAlignment = 1;
                    ItedData.AddCell(Iteml2);
                    PdfPCell Iteml2cell3 = new PdfPCell(new Phrase("HSN", boldTableFont));
                    Iteml2cell3.BackgroundColor = TabelHeaderBackGroundColor;
                    Iteml2cell3.HorizontalAlignment = Element.ALIGN_CENTER;
                    ItedData.AddCell(Iteml2cell3);

                    PdfPCell itemcell4 = new PdfPCell(new Phrase("Order Qty", boldTableFont));
                    itemcell4.BackgroundColor = TabelHeaderBackGroundColor;
                    itemcell4.HorizontalAlignment = Element.ALIGN_CENTER;
                    ItedData.AddCell(itemcell4);
                    PdfPCell itemcell5 = new PdfPCell(new Phrase("Open Qty", boldTableFont));
                    itemcell5.BackgroundColor = TabelHeaderBackGroundColor;
                    itemcell5.HorizontalAlignment = 1;
                    ItedData.AddCell(itemcell5);
                    PdfPCell itemcell6 = new PdfPCell(new Phrase("Invoice Qty", boldTableFont));
                    itemcell6.BackgroundColor = TabelHeaderBackGroundColor;
                    itemcell6.HorizontalAlignment = Element.ALIGN_CENTER;
                    ItedData.AddCell(itemcell6);

                    PdfPCell itemcell7 = new PdfPCell(new Phrase("Price", boldTableFont));
                    itemcell7.BackgroundColor = TabelHeaderBackGroundColor;
                    itemcell7.HorizontalAlignment = Element.ALIGN_CENTER;
                    ItedData.AddCell(itemcell7);
                    PdfPCell itemcell8 = new PdfPCell(new Phrase("Tax", boldTableFont));
                    itemcell8.BackgroundColor = TabelHeaderBackGroundColor;
                    itemcell8.HorizontalAlignment = 1;
                    ItedData.AddCell(itemcell8);
                    PdfPCell itemcell9 = new PdfPCell(new Phrase("Amount", boldTableFont));
                    itemcell9.BackgroundColor = TabelHeaderBackGroundColor;
                    itemcell9.HorizontalAlignment = Element.ALIGN_CENTER;
                    ItedData.AddCell(itemcell9);
                    foreach (var expensevalue in FLIPHeader.FLIPItems)
                    {
                        PdfPCell Type = new PdfPCell(new Phrase(expensevalue.Item, bodyFont));
                        Type.HorizontalAlignment = 1;
                        Type.PaddingLeft = 10f;
                        Type.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                        ItedData.AddCell(Type);
                        PdfPCell amountcell = new PdfPCell(new Phrase("" + expensevalue.MaterialText, bodyFont));
                        amountcell.HorizontalAlignment = 1;
                        amountcell.PaddingLeft = 10f;
                        amountcell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                        ItedData.AddCell(amountcell);
                        PdfPCell remark = new PdfPCell(new Phrase(expensevalue.HSN, bodyFont));
                        remark.HorizontalAlignment = 1;
                        remark.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                        ItedData.AddCell(remark);

                        PdfPCell orderType = new PdfPCell(new Phrase("" + expensevalue.OrderedQty, bodyFont));
                        orderType.HorizontalAlignment = 1;
                        orderType.PaddingLeft = 10f;
                        orderType.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                        ItedData.AddCell(orderType);
                        PdfPCell Opencell = new PdfPCell(new Phrase("" + expensevalue.OpenQty, bodyFont));
                        Opencell.HorizontalAlignment = 1;
                        Opencell.PaddingLeft = 10f;
                        Opencell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                        ItedData.AddCell(Opencell);
                        PdfPCell Invoiceqtycell = new PdfPCell(new Phrase("" + expensevalue.InvoiceQty, bodyFont));
                        Invoiceqtycell.HorizontalAlignment = 1;
                        Invoiceqtycell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                        ItedData.AddCell(Invoiceqtycell);

                        PdfPCell pricecell = new PdfPCell(new Phrase("" + expensevalue.Price, bodyFont));
                        pricecell.HorizontalAlignment = 1;
                        pricecell.PaddingLeft = 10f;
                        pricecell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                        ItedData.AddCell(pricecell);
                        PdfPCell taxcell = new PdfPCell(new Phrase("" + expensevalue.Tax, bodyFont));
                        taxcell.HorizontalAlignment = 1;
                        taxcell.PaddingLeft = 10f;
                        taxcell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                        ItedData.AddCell(taxcell);
                        PdfPCell amounttcell = new PdfPCell(new Phrase("" + expensevalue.Amount, bodyFont));
                        amounttcell.HorizontalAlignment = 1;
                        amounttcell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                        ItedData.AddCell(amounttcell);
                    }

                }
                pdfDoc.Add(itemTable);
                ItedData.PaddingTop = 10f;
                pdfDoc.Add(ItedData);
                PdfContentByte cb = new PdfContentByte(writer);
                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
                cb = new PdfContentByte(writer);
                cb = writer.DirectContent;
                cb.BeginText();
                cb.SetFontAndSize(bf, 8);
                cb.SetTextMatrix(pageSize.GetLeft(120), 20);
                cb.ShowText("Invoice was created on a computer and is valid without the signature and seal. ");
                cb.EndText();

                //Move the pointer and draw line to separate footer section from rest of page
                cb.MoveTo(40, pdfDoc.PageSize.GetBottom(50));
                cb.LineTo(pdfDoc.PageSize.Width - 40, pdfDoc.PageSize.GetBottom(50));
                cb.Stroke();

                pdfDoc.Close();
                return File(PDFData, "Content/pdf");
                DownloadPDF(PDFData);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GeneratePDF", ex);
                return null;
            }
        }

        protected void DownloadPDF(System.IO.MemoryStream PDFData)
        {

        }

        [HttpPost]
        public async Task<IActionResult> AddPOFLIPAttachment()
        {
            try
            {
                var request = Request;
                var FLIPID = request.Form["FLIPID"].ToString();
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
                                    BPCFLIPAttachment BPCFLIPAttachment = new BPCFLIPAttachment();
                                    BPCFLIPAttachment.FLIPID = FLIPID;
                                    BPCFLIPAttachment.AttachmentName = FileName;
                                    BPCFLIPAttachment.ContentType = ContentType;
                                    BPCFLIPAttachment.ContentLength = ContentLength;
                                    BPCFLIPAttachment.AttachmentFile = fileBytes;
                                    BPCFLIPAttachment result = await _AttachmentRepository.UpdateAttachment(BPCFLIPAttachment);
                                }
                            }

                        }

                    }
                }

            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/AddPOFLIPAttachment", ex);
                return BadRequest(ex.Message);
            }
            return Ok();
        }
        [HttpGet]
        public IActionResult GetFlipAttachmentByID(string FlipID, string AttachmentName)
        {
            try
            {
                BPCFLIPAttachment BPAttachment = _AttachmentRepository.GetFlipAttachmentByID(FlipID, AttachmentName);
                if (BPAttachment != null && BPAttachment.AttachmentFile != null && BPAttachment.AttachmentFile.Length > 0)
                {
                    Stream stream = new MemoryStream(BPAttachment.AttachmentFile);
                    return File(BPAttachment.AttachmentFile, "application/octet-stream", BPAttachment.AttachmentName);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Attachment/GetFlipAttachmentByID", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public List<BPCFLIPItem> GetFLIPItemsByFLIPID(string FLIPID)
        {
            try
            {
                var POFLIPItems = _PORepository.GetFLIPItemsByFLIPID(FLIPID);
                return POFLIPItems;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetFLIPItemsByFLIPID", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCFLIPCost> GetFLIPCostsByFLIPID(string FLIPID)
        {
            try
            {
                var BPCFLIPCosts = _IFLIPCostRepository.GetFLIPCostsByFLIPID(FLIPID);
                return BPCFLIPCosts;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetFLIPCostsByFLIPID", ex);
                return null;
            }
        }

        #endregion

        //[HttpGet]
        //public List<BPCFLIPAttachment> FilterAttachments(string ProjectName, int AppID = 0, string AppNumber = null)
        //{
        //    try
        //    {
        //        var attachments = _AttachmentRepository.FilterAttachments(ProjectName, AppID, AppNumber);
        //        return attachments;
        //    }
        //    {
        //        WriteLog.WriteToFile("Attachment/FilterAttachments", ex);
        //        return null;
        //    }
        //}


        #region Data Migration

        [HttpPost]
        public async Task<IActionResult> CreateOFHeaders([FromBody] List<BPCOFHeaderXLSX> OFHeaders)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _PORepository.CreateOFHeaders(OFHeaders);
                return Ok();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/CreateOFHeaders", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOFItems([FromBody] List<BPCOFItemXLSX> OFItems)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _PORepository.CreateOFItems(OFItems);
                return Ok();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/CreateOFItems", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOFScheduleLines([FromBody] List<BPCOFScheduleLineXLSX> OFScheduleLines)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _PORepository.CreateOFScheduleLines(OFScheduleLines);
                return Ok();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/CreateOFScheduleLines", ex);
                return BadRequest(ex.Message);
            }
        }



        [HttpPost]
        public async Task<IActionResult> CreateOFQMs([FromBody] List<BPCOFQMXLSX> OFQMs)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _PORepository.CreateOFQMs(OFQMs);
                return Ok();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/CreateOFQMs", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBPCQM([FromBody] List<BPCOFQM> QM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _PORepository.CreateBPCQM(QM);
                return Ok();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/CreateBPCQM", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public List<BPCOFQM> GetBPCQMByDocNumber(string DocNumber)
        {
            try
            {
                if (DocNumber == null)
                {
                    return null;
                }
                return _PORepository.GetBPCQMByDocNumber(DocNumber);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetBPCQMByDocNumber", ex);
                return null;
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateBPCQM([FromBody] List<BPCOFQM> QM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _PORepository.UpdateBPCQM(QM);
                return Ok("Data are inserted successfully");
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/UpdateBPCQM", ex);
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        public List<BPCOFQM> GetBPCQMByPartnerID(string PartnerID)
        {
            try
            {
                var data = _PORepository.GetBPCQMByPartnerID(PartnerID);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetQMReports", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCOFQM> GetBPCQMByPartnerIDs(string PartnerID)
        {
            try
            {
                var data = _PORepository.GetBPCQMByPartnerIDs(PartnerID);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetQMReports", ex);
                return null;
            }
        }
        [HttpGet]
        public List<BPCOFQM> GetBPCQMByPartnerIDFilter(string PartnerID)
        {
            try
            {
                var data = _PORepository.GetBPCQMByPartnerIDFilter(PartnerID);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetBPCQMByPartnerIDFilter", ex);
                return null;
            }
        }
        [HttpGet]
        public List<BPCOFQM> GetBPCQMByPartnerIDsFilter(string PartnerID)
        {
            try
            {
                var data = _PORepository.GetBPCQMByPartnerIDsFilter(PartnerID);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetBPCQMByPartnerIDFilter", ex);
                return null;
            }
        }

        [HttpPost]
        public List<BPCOFQM> GetQMReportByDate(QMReportOption QMReportOption)
        {
            try
            {
                var data = _PORepository.GetQMReportByDate(QMReportOption);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetQMReportByDate", ex);
                return null;
            }
        }

        [HttpPost]
        public List<BPCOFQM> GetQMReportByOption(QMReportOption QMReportOption)
        {
            try
            {
                var data = _PORepository.GetQMReportByOption(QMReportOption);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetQMReportByOption", ex);
                return null;
            }
        }
        [HttpPost]
        public List<BPCOFQM> GetQMReportByPatnerID(QMReportOption QMReportOption)
        {
            try
            {
                var data = _PORepository.GetQMReportByPatnerID(QMReportOption);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetQMReportByOption", ex);
                return null;
            }
        }

        [HttpPost]
        public List<BPCOFQM> GetQMReportByStatus(QMReportOption QMReportOption)
        {
            try
            {
                var data = _PORepository.GetQMReportByStatus(QMReportOption);
                return data;
            }

            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetQMReportByStatus", ex);
                return null;
            }
        }



        [HttpPost]
        public async Task<IActionResult> CreatePAYASs([FromBody] List<BPCPAYASXLSX> PAYASs)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _PORepository.CreatePAYASs(PAYASs);
                return Ok();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/CreatePAYASs", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePAYPAYMENTs([FromBody] List<BPCPAYPAYMENTXLSX> PAYPAYMENTs)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _PORepository.CreatePAYPAYMENTs(PAYPAYMENTs);
                return Ok();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/CreatePAYPAYMENTs", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Plant

        [HttpGet]
        public IActionResult GetPlantByDocNmber(string DocNumber, string PartnerID)
        {
            try
            {
                if (DocNumber == null)
                {
                    return null;
                }
                var plant = _PORepository.GetPlantByDocNmber(DocNumber, PartnerID);
                return Ok(plant);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetPlantByDocNmber", ex);
                return null;
            }
        }

        [HttpGet]
        public string GetPlantByASNNmber(string DocNumber, string PartnerID)
        {
            try
            {
                if (DocNumber == null)
                {
                    return null;
                }
                return _PORepository.GetPlantByASNNmber(DocNumber, PartnerID);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetPlantByASNNmber", ex);
                return null;
            }
        }

        #endregion
        [HttpGet]
        public IActionResult DowloandAttachment(string AttachmentName)
        {
            try
            {
                BPCAttachment BPAttachment = _PORepository.GetAttachment(AttachmentName);
                if (BPAttachment != null && BPAttachment.AttachmentFile.Length > 0)
                {
                    Stream stream = new MemoryStream(BPAttachment.AttachmentFile);
                    return File(BPAttachment.AttachmentFile, "application/octet-stream", BPAttachment.AttachmentName);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/DowloandAttachment", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> CreateBPCLog(string APIMethod, int NoOfRecords)
        {
            try
            {
                var result = await _PORepository.CreateBPCLog(APIMethod, NoOfRecords);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> UpdateBPCFailureLog(int LogID, string ErrorMessage)
        {
            try
            {
                var result = await _PORepository.UpdateBPCFailureLog(LogID, ErrorMessage);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> UpdateBPCSucessLog(int LogID)
        {
            try
            {
                var result = await _PORepository.UpdateBPCSucessLog(LogID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        public async Task<IActionResult> SendDeliveryNotification()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _PORepository.SendDeliveryNotification();
                return Ok("Delivery Notification sent successfully");
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/SendDeliveryNotification", ex);
                return BadRequest(ex.Message);
            }
        }

    }
}