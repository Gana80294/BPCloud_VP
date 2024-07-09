using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Tesseract;
using System.Threading.Tasks;
using BPCloud_VP_POService.Models;
using BPCloud_VP_POService.Repositories;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using iTextSharp.text.pdf.parser;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto.Engines;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using static QRCoder.PayloadGenerator.SwissQrCode;
using System.Text.RegularExpressions;
using System.Collections;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Logging;
using PdfSharp.Pdf.Content.Objects;
using System.Text;
using Microsoft.EntityFrameworkCore.Internal;
using ZXing;
using PdfSharp.Charting;
using System.Net.Mail;
using Org.BouncyCastle.Ocsp;
using static QRCoder.PayloadGenerator.BezahlCode;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Threading;
using System.IdentityModel.Tokens.Jwt;
using static System.Net.WebRequestMethods;

namespace BPCloud_VP_POService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    
    public class ASNController : ControllerBase
    {
        private readonly IASNRepository _ASNRepository;
        string writerFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Outbox");
        public ASNController(IASNRepository ASNRepository)
        {
            _ASNRepository = ASNRepository;
        }

        #region BPCASNHeader

        [HttpGet]
        public List<BPCASNHeader> GetAllASNs()
        {
            try
            {
                var ASNs = _ASNRepository.GetAllASNs();
                return ASNs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetAllASNs", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCASNHeader1> GetAllASN1ByPartnerID(string PartnerID)
        {
            try
            {
                var ASNs = _ASNRepository.GetAllASN1ByPartnerID(PartnerID);
                return ASNs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetAllASN1ByPartnerID", ex);
                return null;
            }
        }


        [HttpGet]
        public List<BPCASNHeader1> GetOfSuperUserASN1Details(string GetPlantByUser)
        {
            try
            {
                var ASNs = _ASNRepository.GetOfSuperUserASN1Details(GetPlantByUser);
                return ASNs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetAllASN1ByPartnerID", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCASNHeader> GetAllASNByPartnerID(string PartnerID)
        {
            try
            {
                var ASNs = _ASNRepository.GetAllASNByPartnerID(PartnerID);
                return ASNs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetAllASNByPartnerID", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCASNHeader> GetAllASNBySuperUser(string GetPlantByUser)
        {
            try
            {
                var ASNs = _ASNRepository.GetAllASNBySuperUser(GetPlantByUser);
                return ASNs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetAllASNByPartnerID", ex);
                return null;
            }
        }
        [HttpGet]
        public List<ASNListView> GetAllASNList()
        {
            try
            {
                var ASNs = _ASNRepository.GetAllASNList();
                return ASNs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetAllASNListByPartnerID", ex);
                return null;
            }
        }
        [HttpGet]
        public List<ASNListView> GetAllASNListByPartnerID(string PartnerID)
        {
            try
            {
                var ASNs = _ASNRepository.GetAllASNListByPartnerID(PartnerID);
                return ASNs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetAllASNListByPartnerID", ex);
                return null;
            }
        }
        [HttpGet]
        public List<ASNListView> FilterASNList(string VendorCode = null, string ASNNumber = null, string DocNumber = null, string Material = null, string Status = null, DateTime? ASNFromDate = null, DateTime? ASNToDate = null)
        {
            try
            {
                var data = _ASNRepository.FilterASNList(VendorCode, ASNNumber, DocNumber, Material, Status, ASNFromDate, ASNToDate);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/FilterASNList", ex);
                return null;
            }
        }
        [HttpPost]
        public List<ASNListView> FilterASNListByPlants(ASNListFilter filter)
        {
            try
            {
                var data = _ASNRepository.FilterASNListByPlants(filter);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/FilterASNList", ex);
                return null;
            }
        }

        [HttpPost]
        public List<ASNListView> FilterASNList1ByPlants(ASNListFilter filter)
        {
            try
            {
                var data = _ASNRepository.FilterASNList1ByPlants(filter);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/FilterASNList", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCASNAttachment> GetASNAttachmentsASNNumber(string ASNNumber)
        {
            try
            {
                var data = _ASNRepository.GetASNAttachmentsASNNumber(ASNNumber);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetASNAttachmentsASNNumber", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCASNAttachment> GetASNAttachment1ASNNumber(string ASNNumber)
        {
            try
            {
                var data = _ASNRepository.GetASNAttachment1ASNNumber(ASNNumber);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetASNAttachment1ASNNumber", ex);
                return null;
            }
        }

        [HttpGet]
        public List<ASNListView> FilterASNListByPartnerID(string PartnerID, string ASNNumber = null, string DocNumber = null, string Material = null, string Status = null, DateTime? ASNFromDate = null, DateTime? ASNToDate = null)
        {
            try
            {
                var data = _ASNRepository.FilterASNListByPartnerID(PartnerID, ASNNumber, DocNumber, Material, Status, ASNFromDate, ASNToDate);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/FilterASNListByPartnerID", ex);
                return null;
            }
        }
        [HttpGet]
        public List<ASNListView> FilterASNListByPlant(string GetPlantByUser, string ASNNumber = null, string DocNumber = null, string Material = null, string Status = null, DateTime? ASNFromDate = null, DateTime? ASNToDate = null)
        {
            try
            {
                var data = _ASNRepository.FilterASNListByPlant(GetPlantByUser, ASNNumber, DocNumber, Material, Status, ASNFromDate, ASNToDate);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/FilterASNListByPartnerID", ex);
                return null;
            }
        }

        [HttpGet]
        public List<ASNListView> FilterASNListBySuperUser(string GetPlantByUser, string ASNNumber = null, string DocNumber = null, string Material = null, string Status = null, DateTime? ASNFromDate = null, DateTime? ASNToDate = null)
        {
            try
            {
                var data = _ASNRepository.FilterASNListBySuperUser(GetPlantByUser, ASNNumber, DocNumber, Material, Status, ASNFromDate, ASNToDate);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/FilterASNListByPartnerID", ex);
                return null;
            }
        }
        [HttpGet]
        public List<ASNListView> FilterASNList1ByPartnerID(string PartnerID, string ASNNumber = null, string DocNumber = null, string Material = null, string Status = null, DateTime? ASNFromDate = null, DateTime? ASNToDate = null)
        {
            try
            {
                var data = _ASNRepository.FilterASNList1ByPartnerID(PartnerID, ASNNumber, DocNumber, Material, Status, ASNFromDate, ASNToDate);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/FilterASNList1ByPartnerID", ex);
                return null;
            }
        }
        [HttpGet]
        public List<ASNListView> FilterASNList1BySuperUser(string GetPlantByUser, string ASNNumber = null, string DocNumber = null, string Material = null, string Status = null, DateTime? ASNFromDate = null, DateTime? ASNToDate = null)
        {
            try
            {
                var data = _ASNRepository.FilterASNList1BySuperUser(GetPlantByUser, ASNNumber, DocNumber, Material, Status, ASNFromDate, ASNToDate);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/FilterASNList1ByPartnerID", ex);
                return null;
            }
        }

        [HttpGet]
        public List<ASNListView> FilterASNListBySER(string PartnerID, string ASNNumber = null, string DocNumber = null, string Material = null, string Status = null, DateTime? ASNFromDate = null, DateTime? ASNToDate = null)
        {
            try
            {
                var data = _ASNRepository.FilterASNListBySER(PartnerID, ASNNumber, DocNumber, Material, Status, ASNFromDate, ASNToDate);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/FilterASNListBySER", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCASNHeader> GetASNsByDoc(string DocNumber)
        {
            try
            {
                var ASNs = _ASNRepository.GetASNsByDoc(DocNumber);
                return ASNs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetASNsByDoc", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCASNHeader> GetASNByDocAndPartnerID(string DocNumber, string PartnerID)
        {
            try
            {
                var ASNs = _ASNRepository.GetASNByDocAndPartnerID(DocNumber, PartnerID);
                return ASNs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetASNByDocAndPartnerID", ex);
                return null;
            }
        }

        [HttpPost]
        public List<BPCASNHeader1> GetASNByOFAndPartnerID(BPCOFItemViewFilter filter)
        {
            try
            {
                var ASNs = _ASNRepository.GetASNByOFAndPartnerID(filter);
                return ASNs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetASNByOFAndPartnerID", ex);
                return null;
            }
        }

        [HttpGet]
        public BPCASNHeader GetASNByPartnerID(string PartnerID)
        {
            try
            {
                var ASNs = _ASNRepository.GetASNByPartnerID(PartnerID);
                return ASNs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetASNByPartnerID", ex);
                return null;
            }
        }

        [HttpGet]
        public BPCASNHeader GetASNByDocAndASN(string DocNumber, string ASNNumber)
        {
            try
            {
                var ASNs = _ASNRepository.GetASNByDocAndASN(DocNumber, ASNNumber);
                return ASNs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetASNByASN", ex);
                return null;
            }
        }

        [HttpGet]
        public BPCASNHeader GetASNByASN(string ASNNumber)
        {
            try
            {
                var ASNs = _ASNRepository.GetASNByASN(ASNNumber);
                return ASNs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetASNByASN", ex);
                return null;
            }
        }

        [HttpGet]
        public BPCASNHeader1 GetASN1ByASN(string ASNNumber)
        {
            try
            {
                var ASNs = _ASNRepository.GetASN1ByASN(ASNNumber);
                return ASNs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetASN1ByASN", ex);
                return null;
            }
        }

        [HttpGet]
        public BPCASNView2 GetASNViewByASN(string ASNNumber)
        {
            try
            {
                var ASNs = _ASNRepository.GetASNViewByASN(ASNNumber);
                return ASNs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetASNViewByASN", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateGateEntry(GateEntry gateEntry)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _ASNRepository.CreateGateEntry(gateEntry);
                return Ok("Gate entry details updated successfully");
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/CreateGateEntry", ex);
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateASN(BPCASNView ASN,bool OCRvalidate,string InvoiceAmountOCR, bool shipAndInvAmount,bool ShipInvOcrAmount,bool InvoiceNumber, string InvoiceNumberOCR,string GSTValue,bool GSTcheck,string SupplierGSTValue,bool SupplierGSTCheck)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _ASNRepository.CreateASN(ASN, OCRvalidate, InvoiceAmountOCR, shipAndInvAmount, ShipInvOcrAmount, InvoiceNumber, InvoiceNumberOCR,GSTValue,GSTcheck, SupplierGSTValue, SupplierGSTCheck);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/CreateASN", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateASN1(BPCASNView1 ASN, bool OCRvalidate, string InvoiceAmountOCR, bool shipAndInvAmount, bool ShipInvOcrAmount, bool InvoiceNumber, string InvoiceNumberOCR, string GSTValue, bool GSTcheck, string SupplierGSTValue, bool SupplierGSTCheck)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _ASNRepository.CreateASN1(ASN, OCRvalidate, InvoiceAmountOCR, shipAndInvAmount,ShipInvOcrAmount,InvoiceNumber,InvoiceNumberOCR,GSTValue,GSTcheck,SupplierGSTValue,SupplierGSTCheck);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/CreateASN1", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateASN(BPCASNView ASN,bool OCRvalidate, string InvoiceAmountOCR, bool shipAndInvAmount, bool ShipInvOcrAmount, bool InvoiceNumber, string InvoiceNumberOCR,string GSTValue,bool GSTcheck,string SupplierGSTValue, bool SupplierGSTCheck)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _ASNRepository.UpdateASN(ASN, OCRvalidate, InvoiceAmountOCR, shipAndInvAmount,ShipInvOcrAmount,InvoiceNumber, InvoiceNumberOCR,GSTValue,GSTcheck,SupplierGSTValue,SupplierGSTCheck);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/UpdateASN", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateASN1(BPCASNView1 ASN, bool OCRvalidate, string InvoiceAmountOCR, bool shipAndInvAmount, bool ShipInvOcrAmount, bool InvoiceNumber, string InvoiceNumberOCR, string GSTValue, bool GSTcheck,string SupplierGSTValue, bool SupplierGSTCheck)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _ASNRepository.UpdateASN1(ASN,OCRvalidate, InvoiceAmountOCR, shipAndInvAmount,ShipInvOcrAmount,InvoiceNumber,InvoiceNumberOCR,GSTValue, GSTcheck,SupplierGSTValue,SupplierGSTCheck);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/UpdateASN1", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateASNApprovalStatus(BPCASNViewApproval viewApproval)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _ASNRepository.UpdateASNApprovalStatus(viewApproval);
                return Ok();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/UpdateASNApprovalStatus", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateASN1ApprovalStatus(BPCASNViewApproval viewApproval)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _ASNRepository.UpdateASN1ApprovalStatus(viewApproval);
                return Ok();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/UpdateASN1ApprovalStatus", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteASN(BPCASNHeader ASN)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _ASNRepository.DeleteASN(ASN);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/DeleteASN", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteASN1(BPCASNHeader1 ASN)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _ASNRepository.DeleteASN1(ASN);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/DeleteASN", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CancelASN([FromBody] List<CancelASNView> ASN)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _ASNRepository.CancelASN(ASN);
                return Ok("ASN Cancelled successfully");
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/CancelASN", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CancelGate([FromBody] List<CancelASNView> ASN)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _ASNRepository.CancelGate(ASN);
                return Ok("Gate entry Cancelled successfully");
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/CancelGate", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public List<BPCASNFieldMaster> GetAllASNFieldMaster()
        {
            try
            {
                var ASNs = _ASNRepository.GetAllASNFieldMaster();
                return ASNs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetAllASNFieldMaster", ex);
                return null;
            }
        }

        [HttpGet]
        public BPCASNPreShipmentMaster GetASNPreShipmentMaster()
        {
            try
            {
                var ASNs = _ASNRepository.GetASNPreShipmentMaster();
                return ASNs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetASNPreShipmentMaster", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCASNPreShipmentMaster> GetAllASNPreShipmentMasters()
        {
            try
            {
                var result = _ASNRepository.GetAllASNPreShipmentMasters();
                return result;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/GetAllASNPreShipmentMasters", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateASNPreShipmentMaster(BPCASNPreShipmentMaster ASNPreShipmentMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var result = await _ASNRepository.CreateASNPreShipmentMaster(ASNPreShipmentMaster);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                WriteLog.WriteToFile("Master/CreateASNPreShipmentMaster", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateASNPreShipmentMaster(BPCASNPreShipmentMaster ASNPreShipmentMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var result = await _ASNRepository.UpdateASNPreShipmentMaster(ASNPreShipmentMaster);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                WriteLog.WriteToFile("Master/UpdateASNPreShipmentMaster", ex);
                return BadRequest(ex.Message);
            }
            //return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteASNPreShipmentMaster(BPCASNPreShipmentMaster ASNPreShipmentMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _ASNRepository.DeleteASNPreShipmentMaster(ASNPreShipmentMaster);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/DeleteASNPreShipmentMaster", ex);
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        public List<BPCASNFieldMaster> GetASNFieldMasterByType(string DocType)
        {
            try
            {
                var ASNs = _ASNRepository.GetASNFieldMasterByType(DocType);
                return ASNs;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetASNFieldMasterByType", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateASNFieldMaster(BPCASNFieldMaster ASNFieldMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _ASNRepository.UpdateASNFieldMaster(ASNFieldMaster);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/UpdateASNFieldMaster", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public int GetArrivalDateIntervalByPO(string DocNumber)
        {
            try
            {
                var interval = _ASNRepository.GetArrivalDateIntervalByPO(DocNumber);
                return interval;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetArrivalDateIntervalByPO", ex);
                return 1;
            }
        }

        [HttpGet]
        public int GetArrivalDateIntervalByPOAndPartnerID(string DocNumber)
        {
            try
            {
                var interval = _ASNRepository.GetArrivalDateIntervalByPOAndPartnerID(DocNumber);
                return interval;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetArrivalDateIntervalByPOAndPartnerID", ex);
                return 1;
            }
        }


        #endregion

        [HttpGet]
        public List<string> GetASNOFsByASN(string ASNNumber)
        {
            try
            {
                var ASNItems = _ASNRepository.GetASNOFsByASN(ASNNumber);
                return ASNItems;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetASNOFsByASN", ex);
                return null;
            }
        }

        #region BPCASNItems

        [HttpGet]
        public List<BPCASNItem> GetASNItemsByASN(string ASNNumber)
        {
            try
            {
                var ASNItems = _ASNRepository.GetASNItemsByASN(ASNNumber);
                return ASNItems;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetASNItemsByASN", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCASNItemView> GetASNItemsWithBatchesByASN(string ASNNumber)
        {
            try
            {
                var ASNItems = _ASNRepository.GetASNItemsWithBatchesByASN(ASNNumber);
                return ASNItems;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetASNItemsWithBatchesByASN", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCASNItemView1> GetASNItem1WithBatchesByASN(string ASNNumber)
        {
            try
            {
                var ASNItems = _ASNRepository.GetASNItem1WithBatchesByASN(ASNNumber);
                return ASNItems;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetASNItem1WithBatchesByASN", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCASNItemBatch> GetASNItemBatchesByASN(string ASNNumber)
        {
            try
            {
                var ASNItems = _ASNRepository.GetASNItemBatchesByASN(ASNNumber);
                return ASNItems;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetASNItemBatchesByASN", ex);
                return null;
            }
        }

        #endregion

        #region BPCASNPacks

        [HttpGet]
        public List<BPCASNPack> GetASNPacksByASN(string ASNNumber)
        {
            try
            {
                var ASNPacks = _ASNRepository.GetASNPacksByASN(ASNNumber);
                return ASNPacks;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetASNPacksByASN", ex);
                return null;
            }
        }

        #endregion


        [HttpGet]
        public List<BPCDocumentCenter> GetDocumentCentersByASN(string ASNNumber)
        {
            try
            {
                var ASNItems = _ASNRepository.GetDocumentCentersByASN(ASNNumber);
                return ASNItems;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetASNItemsByASN", ex);
                return null;
            }
        }

        #region Attachment

        [HttpPost]
        public async Task<IActionResult> AddInvoiceAttachment()
        {
            try
            {
                var request = Request;
                var Client = request.Form["Client"].ToString();
                var Company = request.Form["Company"].ToString();
                var Type = request.Form["Type"].ToString();
                var PatnerID = request.Form["PatnerID"].ToString();
                var ReferenceNo = request.Form["ReferenceNo"].ToString();
                // var ReferenceNo = "";
                var CreatedBy = request.Form["CreatedBy"].ToString();
                IFormFileCollection postedfiles = request.Form.Files;
                BPCAttachment BPAttachment = new BPCAttachment();
                BPAttachment.Client = Client;
                BPAttachment.Company = Company;
                BPAttachment.Type = Type;
                BPAttachment.PatnerID = PatnerID;
                BPAttachment.ReferenceNo = ReferenceNo;
                BPAttachment.CreatedBy = CreatedBy;
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
                        await _ASNRepository.AddInvoiceAttachment(BPAttachment);
                    }
                }

            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/AddInvoiceAttachment", ex);
                return BadRequest(ex.Message);
            }
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> ValidateInvoiceAttachment()
        {
            OCRValue ocrtext = new OCRValue();
            string filePath = "";
            try
            {
                var request = Request;
                var Client = request.Form["Client"].ToString();
                var Company = request.Form["Company"].ToString();
                var Type = request.Form["Type"].ToString();
                var PatnerID = request.Form["PatnerID"].ToString();
                var ReferenceNo = request.Form["ReferenceNo"].ToString();
                // var ReferenceNo = "";
                var CreatedBy = request.Form["CreatedBy"].ToString();
                var Plant = request.Form["plant"].ToString();
                IFormFileCollection postedfiles = request.Form.Files;
                
                //BPAttachment.Client = Client;
                //BPAttachment.Company = Company;
                //BPAttachment.Type = Type;
                //BPAttachment.PatnerID = PatnerID;
                //BPAttachment.ReferenceNo = ReferenceNo;
                //BPAttachment.CreatedBy = CreatedBy;
                if (postedfiles.Count > 0)
                {
                    BPCAttachment BPAttachment = new BPCAttachment();
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
                    string Signed_path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UploadDocument");
                    string uploadfilename = BPAttachment.AttachmentName;
                    filePath = System.IO.Path.Combine(Signed_path, uploadfilename);
                    System.IO.File.WriteAllBytes(filePath, BPAttachment.AttachmentFile);
                    ocrtext = GetOCR(filePath, Plant, PatnerID);
                    string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InvoiceDocument");
                    string copyPath = System.IO.Path.Combine(path, uploadfilename);
                    System.IO.File.WriteAllBytes(copyPath, BPAttachment.AttachmentFile);
                    // string path = @"C:\BackupPath\";
                    //path = System.IO.Path.Combine(path, uploadfilename);
                    //if (!Directory.Exists(path))
                    //    Directory.CreateDirectory(path);
                    //WriteLog.WriteToFile( filePath);
                    //WriteLog.WriteToFile( path);
                    //System.IO.File.Copy(filePath, path, true);
                    //if (!string.IsNullOrEmpty(BPAttachment.AttachmentName))
                    //{
                    //    await _ASNRepository.AddInvoiceAttachment(BPAttachment);
                    //}
                }

            }

            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/ValidateInvoiceAttachment", ex);
                return BadRequest(ex.Message);
            }
            System.IO.File.Delete(filePath);
            return Ok(ocrtext);
        }

        public OCRValue GetOCR(string invoicepath,string Plant,string PatnerID)
        {
            string invoiceamount = "";
            //Boolean gstvalue = false;
            WriteLog.WriteToFile("GetOCR");
            OCRValue getocr = new OCRValue();
            PythonOCRValue pythonocrvalue = new PythonOCRValue();
            List<int> PageNumberGRN = new List<int>();
            string Extension = System.IO.Path.GetExtension(invoicepath);
            string OnlyFileName = System.IO.Path.GetFileNameWithoutExtension(invoicepath);
            string OCRText = "";
            bool PDFISSearchable = true;
            string NewPath = "";
            bool iSVendorPortal = false;
            Boolean gstvalidation = false;
            bool Suppliergstvalidate = false;
            Dictionary<int, string> CurrentPDFTexts = new Dictionary<int, string>();
            try
            {
                Byte[] bytes = System.IO.File.ReadAllBytes(invoicepath);
                //String file = Convert.ToBase64String(bytes);
                MyPayload pdffile = new MyPayload();
                pdffile.encode = Convert.ToBase64String(bytes);
                HttpClient httpClient = new HttpClient();
                WriteLog.WriteToFile("Coonect Python API");
                using (HttpClient client1 = httpClient)
                {
                    client1.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
                    string projectDispatchJson = JsonConvert.SerializeObject(pdffile);
                    //ErrorLog.WriteErrorLog("jsonformat : " + projectDispatchJson);
                    string OCRURL = "http://172.17.2.27:81/ocr/home";
                    var content = new StringContent(projectDispatchJson, Encoding.UTF8, "application/json");
                    var response = client1.PostAsync(@OCRURL, content).Result;
                    var result = response.Content.ReadAsStringAsync().Result;
                    WriteLog.WriteToFile("Python API Connected");
                    if (response.IsSuccessStatusCode)
                    {
                        WriteLog.WriteToFile("Python API successfull");
                        WriteLog.WriteToFile("Python API successfull"+ pythonocrvalue.buyer_gst);
                        pythonocrvalue = JsonConvert.DeserializeObject<PythonOCRValue>(result);
                        gstvalidation = _ASNRepository.GSTValidate(pythonocrvalue.buyer_gst, Plant);
                        getocr.InvoiceNumber = pythonocrvalue.invoice_number;
                        getocr.Amount = pythonocrvalue.amount;
                        getocr.GstValue = pythonocrvalue.buyer_gst;
                        getocr.vendor_gst = pythonocrvalue.vendor_gst;
                        getocr.GST = gstvalidation;
                        WriteLog.WriteToFile("Python ocr value gteocr " + pythonocrvalue);

                    }
                }
                    return getocr;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("GetOCR", ex);
                //WriteLog.WriteToFile(getocr);
                return getocr;
            }
        }
        private  OCRValue GhostscriptPDFToImage(string Plant,string inputFile, string outputFileName, int PageNumber,string amount,Boolean gstvalidation,string InvoiceNumber,string gstvalue)
        {
            OCRValue ocr = new OCRValue();
            try
            {
                var xDpi = 300; //set the x DPI
                var yDpi = 300; //set the y DPI
                string outputPNGPath = "";
                
                {
                    WriteLog.WriteToFile("GhostscriptPDFToImage running");
                    string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TempFolder");
                    outputPNGPath = System.IO.Path.Combine(path, string.Format("{0}.png", Guid.NewGuid() + outputFileName));

                    Ghostscript.NET.GhostscriptPngDevice img = new Ghostscript.NET.GhostscriptPngDevice(Ghostscript.NET.GhostscriptPngDeviceType.PngGray);
                    img.GraphicsAlphaBits = Ghostscript.NET.GhostscriptImageDeviceAlphaBits.V_4;
                    img.TextAlphaBits = Ghostscript.NET.GhostscriptImageDeviceAlphaBits.V_4;
                    img.ResolutionXY = new Ghostscript.NET.GhostscriptImageDeviceResolution(xDpi, yDpi);
                    img.InputFiles.Add(inputFile);
                    img.Pdf.FirstPage = PageNumber;
                    img.Pdf.LastPage = PageNumber;
                    img.PostScript = string.Empty;
                    img.CustomSwitches.Add("-dDOINTERPOLATE");
                    img.OutputPath = outputPNGPath;
                    img.Process();
                }
                WriteLog.WriteToFile("GetTextFromPNGFile calling");
                return ocr = GetTextFromPNGFile(Plant,outputPNGPath, PageNumber, outputFileName, amount, gstvalidation, InvoiceNumber, gstvalue);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("GhostscriptPDFToImage", ex);
                return ocr;
            }
        }
        public OCRValue GetTextFromPNGFile(string Plant,string outputPNGPath, int PageNumber, string outputFileName,string amount,Boolean gstvalidation,string InvoiceNumber,string gstvalue)
        {
            var ocrtext = string.Empty;
            OCRValue ocrvalue = new OCRValue();
            try
            {
                WriteLog.WriteToFile("GetTextFromPNGFile running");
                string[] configs = { System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"tessdata\configs\config.txt") };
                using (var engine = new TesseractEngine(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata"), "eng", EngineMode.TesseractAndLstm, configs))
                {
                    using (var img = Pix.LoadFromFile(outputPNGPath))
                    {
                        using (var page = engine.Process(img))
                        {
                            ocrtext = page.GetText();
                           string  OCRText = ocrtext;
                            string[] ar = null ;

                            string gst = "";
                            string[] amountsplit = null;
                            string[] invoicenumber = null;
                            string[] lines = OCRText.Split(new string[] { "\n", }, StringSplitOptions.RemoveEmptyEntries);
                            string NewOcrText = string.Empty;
                            
                            
                            foreach (var line in lines)
                            {
                                if (!string.IsNullOrEmpty(line.Trim()))
                                {
                                    NewOcrText = NewOcrText + line + Environment.NewLine;
                                    if(line.Length == 15)
                                    {
                                        gst = line;
                                        StringBuilder tempString = new StringBuilder(gst);
                                        tempString[2] = 'A';
                                        tempString[3] = 'A';
                                        tempString[4] = 'A';
                                        tempString[5] = 'C';
                                        tempString[6] = 'H';
                                        tempString[7] = '7';
                                        tempString[8] = '4';
                                        tempString[9] = '1';
                                        tempString[10] = '2';
                                        tempString[11] = 'G';
                                        tempString[13] = 'Z';
                                        string modifiedString = tempString.ToString();
                                        WriteLog.WriteToFile(modifiedString);
                                        if (modifiedString != string.Empty)
                                        {
                                            if (!gstvalidation)
                                            {
                                                gstvalidation = _ASNRepository.GSTValidate(modifiedString, Plant);
                                            }
                                            
                                            
                                        }
                                    }
                                    if ((line.StartsWith("GSTIN Number")) || (line.StartsWith("GSTIN/Unique ID")|| (line.StartsWith("GSTIN/UniqueID") || (line.Contains("GSTIN:")) || (line.Contains("GSTIN")) || line.Contains("GSTIN Reg.No:")) || (line.Contains("GSTIN :")))){
                                         ar = line.Split(' ');

                                       
                                        for(int i = 0; i < ar.Length; i++)
                                        {
                                           if(ar[i].Length == 15 && !gstvalidation) {

                                                 gst = ar[i];
                                                if(gst  != "GSTIN/UniqueID:" && gst != "GSTIN Number" && gst != "GSTIN/Unique ID" && gst != "GSTIN:" && gst != "GSTIN")
                                                {
                                                    StringBuilder tempString = new StringBuilder(gst);
                                                    tempString[2] = 'A';
                                                    tempString[3] = 'A';
                                                    tempString[4] = 'A';
                                                    tempString[5] = 'C';
                                                    tempString[6] = 'H';
                                                    tempString[7] = '7';
                                                    tempString[8] = '4';
                                                    tempString[9] = '1';
                                                    tempString[10] = '2';
                                                    tempString[11] = 'G';
                                                    tempString[13] = 'Z';
                                                    string modifiedString = tempString.ToString();
                                                    if ((modifiedString.StartsWith("18")) || (modifiedString.StartsWith("37")))
                                                    {
                                                        tempString[14] = 'S';
                                                        modifiedString = tempString.ToString();
                                                    }
                                                    WriteLog.WriteToFile(modifiedString);
                                                    if (modifiedString != string.Empty)
                                                    {
                                                        gstvalidation = _ASNRepository.GSTValidate(modifiedString, Plant);
                                                        if (gstvalidation)
                                                        {
                                                            gstvalue = modifiedString;
                                                            break;
                                                        }
                                                        
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if ((line.StartsWith("Total:")) || (line.Contains("Total:")) )
                                    {
                                        amountsplit = line.Split(':');
                                        for (int i = 0; i < amountsplit.Length; i++)
                                        {
                                            if (amountsplit[i] != null && amountsplit[i] != "Total" && amount == "")
                                            {
                                                amount = amountsplit[i].ToString();
                                            }
                                        }
                                    }
                                    else if (((line.Contains("Total Current Charges")) || line.StartsWith("Total") || line.StartsWith("TOTAL")) && !line.StartsWith("Total In Words"))
                                    {
                                        amountsplit = line.Split(' ');
                                        if ((line.Contains("Total Current Charges")))
                                        {
                                            int taxindex = amountsplit.IndexOf("Charges");
                                            if (taxindex != -1)
                                            {
                                                for (int i = taxindex + 1; i < amountsplit.Length; i++)
                                                {
                                                    if (amountsplit[i] != "" && amount=="")
                                                    {
                                                        amount = amountsplit[i];
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            for (int i = 0; i < amountsplit.Length; i++)
                                            {
                                                if (amountsplit[i] != "" && (amountsplit[i] != "TOTAL" && amountsplit[i] != "Total") && amount == "")
                                                {
                                                    amount = amountsplit[i].ToString();
                                                }
                                            }
                                        }
                                       
                                       
                                    }
                                    if((line.StartsWith("Total Amount after Tax")) || (line.Contains("Total Amount after Tax") && amount!=""))
                                    {
                                        ar = line.Split(' ');
                                        int taxindex = ar.IndexOf("Tax");
                                        if (taxindex != -1)
                                        {
                                            for (int i = taxindex + 1; i < ar.Length; i++)
                                            {
                                                if (ar[i] != "")
                                                {
                                                    amount = ar[i];
                                                }
                                            }
                                        }
                                    }
                                    if(line.StartsWith("Serial No of Invoice: ") )
                                    {
                                         invoicenumber = line.Split(' ');
                                                int Invoiceindex = invoicenumber.IndexOf("Invoice:");
                                                if (Invoiceindex != -1)
                                                {
                                                    for (int i = Invoiceindex + 1; i < invoicenumber.Length; i++)
                                                    {
                                                        if (invoicenumber[i] != "" && InvoiceNumber.Length == 0)
                                                        {
                                                            InvoiceNumber = invoicenumber[i];
                                                        }
                                                    }
                                                }
                                           // }
                                        //}
                                    }
                                   else if (line.StartsWith("TAX INVOICE"))
                                    {
                                        invoicenumber = line.Split(' ');
                                        int Invoiceindex = invoicenumber.IndexOf("INVOICE");
                                        if (Invoiceindex != -1)
                                        {
                                            for (int i = Invoiceindex + 1; i < invoicenumber.Length; i++)
                                            {
                                                if (invoicenumber[i] != "" && InvoiceNumber.Length == 0 && InvoiceNumber=="")
                                                {
                                                    InvoiceNumber = invoicenumber[i];
                                                }
                                            }
                                        }
                                    }
                                    else if (line.StartsWith("Bill No."))
                                    {
                                        invoicenumber = line.Split(' ');
                                        int Invoiceindex = invoicenumber.IndexOf("No.");
                                        if (Invoiceindex != -1)
                                        {
                                            for (int i = Invoiceindex+1; i < invoicenumber.Length; i++)
                                            {
                                                if (invoicenumber[i] != "" && InvoiceNumber.Length == 0 && InvoiceNumber == "")
                                                {
                                                    InvoiceNumber = invoicenumber[i];
                                                }
                                            }
                                        }
                                    }
                                    else if(line.StartsWith("Invoice No. :"))
                                    {
                                        invoicenumber = line.Split(':');
                                        for (int i = 0; i < invoicenumber.Length; i++)
                                        {
                                            string[] splitinvoicenumber = line.Split(' ');
                                            for(int j = 0 ; j< splitinvoicenumber.Length; j++)
                                            {
                                                int Invoiceindex = splitinvoicenumber[j].IndexOf(":");
                                                if (Invoiceindex != -1)
                                                {

                                                    for (int k = Invoiceindex; k < splitinvoicenumber.Length; k++)
                                                    {
                                                        if (splitinvoicenumber[k] != "" && InvoiceNumber.Length == 0)
                                                        {
                                                            InvoiceNumber = splitinvoicenumber[k].ToString();
                                                        }
                                                    }
                                                    
                                                }
                                                
                                            }
                                            
                                        }
                                    }
                                    else if (line.StartsWith("Invoice Number"))
                                    {
                                        string[] splitinvoicenumber = line.Split(' ');
                                        int Invoiceindex = splitinvoicenumber.IndexOf("Number");
                                        if (Invoiceindex != -1)
                                        {
                                            for (int i = Invoiceindex + 1; i < splitinvoicenumber.Length; i++)
                                            {
                                                if (splitinvoicenumber[i] != "" && InvoiceNumber.Length == 0 && InvoiceNumber == "")
                                                {
                                                    InvoiceNumber = splitinvoicenumber[i];
                                                    if (InvoiceNumber.StartsWith(":")){
                                                        InvoiceNumber = InvoiceNumber.Remove(0, 1);
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    else if (line.StartsWith("Invoice N0.:") || line.StartsWith("Invoice No.:"))
                                    {
                                        invoicenumber = line.Split(':');
                                        for (int i = 0; i < invoicenumber.Length; i++)
                                        {
                                            string[] splitinvoicenumber = line.Split(' ');
                                            for (int j = 0; j < splitinvoicenumber.Length; j++)
                                            {
                                                int Invoiceindex = splitinvoicenumber[j].IndexOf(":");
                                                if (Invoiceindex != -1)
                                                {
                                                    if(InvoiceNumber=="")
                                                    
                                                            InvoiceNumber = splitinvoicenumber[Invoiceindex-1].ToString();
                                                       // }
                                                   

                                                }

                                            }

                                        }
                                    }
                                }
                            }
                            
                            if(gstvalidation || amount != "" || InvoiceNumber != "")
                            {
                                WriteLog.WriteToFile(amount);
                                WriteLog.WriteToFile(gstvalidation.ToString());
                                ocrvalue.Amount = amount;
                                ocrvalue.GST = gstvalidation;
                                ocrvalue.GstValue = gstvalue;
                                ocrvalue.InvoiceNumber = InvoiceNumber;
                            }
                            
                        }
                    }
                }
                return ocrvalue;
            }
            catch (Exception ex)    
            {
                WriteLog.WriteToFile("GetTextFromPNGFile", ex);
                return ocrvalue;
            }
        }


        //public static long ConvertToNumbers(string numberString)
        //{
        //    var numbers = Regex.Matches(numberString, @"\w+").Cast<Match>()
        //            .Select(m => m.Value.ToLowerInvariant())
        //            .Where(v => numberTable.ContainsKey(v))
        //            .Select(v => numberTable[v]);
        //    long acc = 0, total = 0L;
        //    foreach (var n in numbers)
        //    {
        //        if (n >= 1000)
        //        {
        //            total += acc * n;
        //            acc = 0;
        //        }
        //        else if (n >= 100)
        //        {
        //            acc *= n;
        //        }
        //        else acc += n;
        //    }
        //    return (total + acc) * (numberString.StartsWith("minus",
        //            StringComparison.InvariantCultureIgnoreCase) ? -1 : 1);
        //}



        //End


        [HttpPost]
        public async Task<IActionResult> AddDocumentCenterAttachment()
        {
            try
            {
                string filePath = "";
                var request = Request;
                var Client = request.Form["Client"].ToString();
                var Company = request.Form["Company"].ToString();
                var Type = request.Form["Type"].ToString();
                var PatnerID = request.Form["PatnerID"].ToString();
                var ASNNumber = request.Form["ASNNumber"].ToString();
                var CreatedBy = request.Form["CreatedBy"].ToString();
                var IsSubmitted = request.Form["IsSubmitted"].ToString();
                IFormFileCollection postedfiles = request.Form.Files;
                List<BPCAttachment> BPAttachments = new List<BPCAttachment>();
                
                if (postedfiles.Count > 0)
                {
                    for (int i = 0; i < postedfiles.Count; i++)
                    {
                        int Filecount = i + 1;
                        var FileName = postedfiles[i].FileName;
                        var ContentType = postedfiles[i].ContentType;
                        var ContentLength = postedfiles[i].Length;
                        BPCAttachment BPAttachment = new BPCAttachment();
                        using (Stream st = postedfiles[i].OpenReadStream())
                        {
                            using (BinaryReader br = new BinaryReader(st))
                            {
                                byte[] fileBytes = br.ReadBytes((Int32)st.Length);
                                if (fileBytes.Length > 0)
                                {
                                    
                                    BPAttachment.Client = Client;
                                    BPAttachment.Company = Company;
                                    BPAttachment.Type = Type;
                                    BPAttachment.PatnerID = PatnerID;
                                    BPAttachment.CreatedBy = CreatedBy;

                                    BPAttachment.AttachmentName = FileName;
                                    BPAttachment.ContentType = ContentType;
                                    BPAttachment.ContentLength = ContentLength;
                                    BPAttachment.AttachmentFile = fileBytes;
                                    BPAttachments.Add(BPAttachment);

                                    //string Signed_path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DocumentCenterAttachment");
                                    // string uploadfilename = BPAttachment.AttachmentName;
                                    // filePath = System.IO.Path.Combine(Signed_path, uploadfilename);
                                    // System.IO.File.WriteAllBytes(filePath, BPAttachment.AttachmentFile);
                                    // WriteLog.WriteToFile("Document files upload" + Signed_path);

                                    // string Documentcenterfolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DocumentCenterAttachment");
                                    // DirectoryInfo directory = new DirectoryInfo(Documentcenterfolder);
                                    //FileInfo[] documentfiles = directory.GetFiles();
                                    //if (documentfiles.Length > 0)
                                    //{
                                    //    int Filecount = 0;
                                    //   foreach (var sourceFilePath in Directory.GetFiles(Documentcenterfolder))
                                    //   {
                                    //      WriteLog.WriteToFile("Document center " + Filecount);
                                    //      i++;
                                    //        //string fileName = Path.GetFileName(sourceFilePath);


                                    //var BaseFileName = Company + DateTime.Now.ToString("ddMMyyyy") + "_" + ASNNumber;
                                    //writerFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Outbox");
                                    //string destinationFilePath = System.IO.Path.Combine(writerFolder, BaseFileName + '_' + Filecount + ".pdf");
                                    //string writerPath = System.IO.Path.Combine(writerFolder, FileName);
                                    //System.IO.File.WriteAllBytes(destinationFilePath, BPAttachment.AttachmentFile);


                                    //System.IO.File.Copy(sourceFilePath, destinationFilePath, true);
                                    //WriteLog.WriteToFile("sourceFilePath " + sourceFilePath);
                                    //WriteLog.WriteToFile("destinationFilePath" + destinationFilePath);
                                    //if (System.IO.File.Exists(sourceFilePath))
                                    //{
                                    //    System.IO.File.Delete(sourceFilePath);
                                    //}

                                    // }
                                    //}
                                }
                            }

                        }

                    }
                    //await _ASNRepository.AddDocumentCenterAttachment(BPAttachments, ASNNumber);
                    //if (BPAttachment != null)
                    //{
                    //    await _ASNRepository.AddDocumentCenterAttachment(BPAttachments, ASNNumber);
                    //}
                    if (BPAttachments.Count > 0)
                    {
                        await _ASNRepository.AddDocumentCenterAttachment(BPAttachments, ASNNumber,IsSubmitted, Company);
                    }
                }

            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/AddDocumentCenterAttachment", ex);
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpGet]
        public IActionResult DowloandInvoiceAttachment(string AttachmentName, string ASNNumber)
        {
            try
            {
                BPCAttachment BPAttachment = _ASNRepository.GetAttachmentByName(AttachmentName, ASNNumber);
                if (BPAttachment != null && BPAttachment.AttachmentFile.Length > 0)
                {
                    Stream stream = new MemoryStream(BPAttachment.AttachmentFile);
                    return File(BPAttachment.AttachmentFile, "application/octet-stream", BPAttachment.AttachmentName);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Attachment/DowloandInvoiceAttachment", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult DowloandDocumentCenterAttachment(string AttachmentName, string ASNNumber)
        {
            try
            {
                BPCAttachment BPAttachment = _ASNRepository.GetAttachmentByName(AttachmentName, ASNNumber);
                if (BPAttachment != null && BPAttachment.AttachmentFile.Length > 0)
                {
                    Stream stream = new MemoryStream(BPAttachment.AttachmentFile);
                    return File(BPAttachment.AttachmentFile, "application/octet-stream", BPAttachment.AttachmentName);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Attachment/DowloandDocumentCenterAttachment", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult DowloandAttachmentByID(int AttachmentID)
        {
            try
            {
                BPCAttachment BPAttachment = _ASNRepository.DowloandAttachmentByID(AttachmentID);
                if (BPAttachment != null && BPAttachment.AttachmentFile.Length > 0)
                {
                    Stream stream = new MemoryStream(BPAttachment.AttachmentFile);
                    return File(BPAttachment.AttachmentFile, "application/octet-stream", BPAttachment.AttachmentName);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Attachment/DowloandAttachmentByID", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetInvoiceAttachmentByASN(string ASNNumber, string InvDocReferenceNo)
        {
            try
            {
                BPCInvoiceAttachment BPAttachment = _ASNRepository.GetInvoiceAttachmentByASN(ASNNumber, InvDocReferenceNo);
                return Ok(BPAttachment);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Attachment/GetInvoiceAttachmentByASN", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetInvoiceAttachment1ByASN(string ASNNumber, string InvDocReferenceNo)
        {
            try
            {
                BPCInvoiceAttachment BPAttachment = _ASNRepository.GetInvoiceAttachment1ByASN(ASNNumber, InvDocReferenceNo);
                return Ok(BPAttachment);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Attachment/GetInvoiceAttachment1ByASN", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult CreateASNPdf(string ASNNumber, bool FTPFlag)
        {
            try
            {
                byte[] bytes = _ASNRepository.CreateASNPdf(ASNNumber, FTPFlag);
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
                    WriteLog.WriteToFile("FileName : "+ FileName);
                    return File(bytes, "application/octet-stream", FileName);
                };
                return NotFound();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Attachment/CreateASNPdf", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult CreateASNPdf1(string ASNNumber, bool FTPFlag)
        {
            try
            {
                byte[] bytes = _ASNRepository.CreateASNPdf1(ASNNumber, FTPFlag);
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
                WriteLog.WriteToFile("Attachment/CreateASNPdf1", ex);
                return BadRequest(ex.Message);
            }
        }


        #endregion
       

    }
}