
using BPCloud_VP_POService.DBContexts;
using BPCloud_VP_POService.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
//using iTextSharp.text.pdf.qrcode;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Org.BouncyCastle.Bcpg;
using QRCoder;
//using SAP.Middleware.Connector;
//using SAP.Middleware.Connector;
//using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Tesseract;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;
using static ZXing.QrCode.Internal.Mode;
//using ZXing;
//using ZXing.QrCode;
//using ZXing.QrCode.Internal;

namespace BPCloud_VP_POService.Repositories
{
    public class ASNRepository : IASNRepository
    {
        private readonly POContext _dbContext;
        IConfiguration _configuration;
        private readonly IAIACTRepository _aIACTRepository;
        private readonly IGateRepository _gateRepository;
        private readonly IPORepository _poRepository;

        public ASNRepository(POContext dbContext, IConfiguration configuration, IAIACTRepository aIACTRepository, IGateRepository gateRepository, IPORepository poRepository)
        {
            _dbContext = dbContext;
           
            _configuration = configuration;
            _aIACTRepository = aIACTRepository;
            _gateRepository = gateRepository;
            _poRepository = poRepository;
        }

        public List<BPCASNHeader> GetAllASNs()
        {
            try
            {
                var result = (from tb in _dbContext.BPCASNHeaders
                              orderby tb.CreatedOn descending
                              select tb).ToList();
                return result;
                //return _dbContext.BPCASNHeaders.ToList();
            }
            catch (SqlException ex)
            {
                WriteLog.WriteToFile("ASNRepository/GetAllASNs", ex); throw new Exception("Something went wrong");
            }
            catch (InvalidOperationException ex)
            {
                WriteLog.WriteToFile("ASNRepository/GetAllASNs", ex); throw new Exception("Something went wrong");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCASNHeader> GetAllASNBySuperUser(string GetPlantByUser)
        {
            try
            {
                string[] Plants = GetPlantByUser.Split(',');
                for (int i = 0; i < Plants.Length; i++)
                {
                    Plants[i] = Plants[i].Trim();
                }
                List<BPCASNHeader> ASNHeader = new List<BPCASNHeader>();
                List<BPCASNHeader> ASNHeaders = new List<BPCASNHeader>();
                for (int i = 0; i < Plants.Length; i++)
                {
                    ASNHeader = (from tb in _dbContext.BPCASNHeaders
                                 where tb.Plant == Plants[i]
                                 orderby tb.CreatedOn descending
                                 select tb).ToList();
                    ASNHeaders.AddRange(ASNHeader);
                }
                return ASNHeaders;
                //return _dbContext.BPCASNHeaders.Where(x => x.PatnerID == PartnerID).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/GetAllASNByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/GetAllASNByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCASNHeader> GetAllASNByPartnerID(string PartnerID)
        {
            try
            {
                var result = (from tb in _dbContext.BPCASNHeaders
                              where tb.PatnerID == PartnerID
                              orderby tb.CreatedOn descending
                              select tb).ToList();
                return result;
                //return _dbContext.BPCASNHeaders.Where(x => x.PatnerID == PartnerID).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/GetAllASNByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/GetAllASNByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCASNHeader1> GetAllASN1ByPartnerID(string PartnerID)
        {
            try
            {
                var result = (from tb in _dbContext.BPCASNHeader1
                              join tb1 in _dbContext.BPCASNOFMap1 on tb.ASNNumber equals tb1.ASNNumber
                              where tb.PatnerID == PartnerID
                              orderby tb.CreatedOn descending
                              select tb).Distinct().ToList();
                return result;
                //return _dbContext.BPCASNHeaders.Where(x => x.PatnerID == PartnerID).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/GetAllASN1ByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/GetAllASN1ByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BPCASNHeader1> GetOfSuperUserASN1Details(string GetPlantByUser)
        {
            try
            {
                string[] Plants = GetPlantByUser.Split(',');
                for (int i = 0; i < Plants.Length; i++)
                {
                    Plants[i] = Plants[i].Trim();
                }
                List<BPCASNHeader1> bPCOFASNHeaders = new List<BPCASNHeader1>();
                List<BPCASNHeader1> bPCOFASNHeader = new List<BPCASNHeader1>();
                for (int i = 0; i < Plants.Length; i++)
                {
                    bPCOFASNHeader = (from tb in _dbContext.BPCASNHeader1
                                      join tb1 in _dbContext.BPCASNOFMap1 on tb.ASNNumber equals tb1.ASNNumber
                                      where tb.Plant == Plants[i]
                                      orderby tb.CreatedOn descending
                                      select tb).Distinct().ToList();
                    bPCOFASNHeaders.AddRange(bPCOFASNHeader);
                }
                return bPCOFASNHeaders;
                //return _dbContext.BPCASNHeaders.Where(x => x.PatnerID == PartnerID).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/GetAllASN1ByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/GetAllASN1ByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //
        public List<ASNListView> GetAllASNList()
        {
            try
            {
                var result = (from tb in _dbContext.BPCASNHeaders
                                  //join tb1 in _dbContext.BPCASNItems on tb.ASNNumber equals tb1.ASNNumber
                              orderby tb.CreatedOn descending
                              select new ASNListView()
                              {
                                  Client = tb.Client,
                                  Company = tb.Company,
                                  Type = tb.Type,
                                  PatnerID = tb.PatnerID,
                                  ASNNumber = tb.ASNNumber,
                                  ASNDate = tb.ASNDate,
                                  DocNumber = tb.DocNumber,
                                  AWBNumber = tb.AWBNumber,
                                  VessleNumber = tb.VessleNumber,
                                  Plant = tb.Plant,
                                  InvDocReferenceNo = tb.InvDocReferenceNo,
                                  InvoiceNumber = tb.InvoiceNumber,
                                  POBasicPrice = tb.POBasicPrice,
                                  InvoiceAmount = tb.InvoiceAmount,
                                  TaxAmount = tb.TaxAmount,
                                  ArrivalDate = tb.ArrivalDate,
                                  DepartureDate = tb.DepartureDate,
                                  //Material = tb1.Material,
                                  //MaterialText = tb1.MaterialText,
                                  //ASNQty = tb1.ASNQty,
                                  Status = tb.Status,
                                  IsBuyerApprovalRequired = tb.IsBuyerApprovalRequired,
                                  BuyerApprovalStatus = tb.BuyerApprovalStatus,
                                  BuyerApprovalOn = tb.BuyerApprovalOn
                              }).ToList();
                foreach (var res in result)
                {
                    //if (!string.IsNullOrEmpty(res.InvDocReferenceNo))
                    //{
                    //    res.InvoiceAttachmentName = (from tb in _dbContext.BPCAttachments
                    //                                 where tb.AttachmentID.ToString() == res.InvDocReferenceNo
                    //                                 select tb.AttachmentName).FirstOrDefault();
                    //}
                    res.DocCount = 0;
                    if (!string.IsNullOrEmpty(res.InvDocReferenceNo))
                    {
                        res.InvoiceAttachmentName = (from tb in _dbContext.BPCAttachments
                                                     where tb.AttachmentID.ToString() == res.InvDocReferenceNo
                                                     select tb.AttachmentName).FirstOrDefault();
                        if (!string.IsNullOrEmpty(res.InvoiceAttachmentName))
                        {
                            res.DocCount = res.DocCount + 1;
                        }
                    }
                    var docCenters = _dbContext.BPCDocumentCenters.Where(x => x.ASNNumber == res.ASNNumber).ToList();
                    foreach (var doc in docCenters)
                    {
                        if (!string.IsNullOrEmpty(doc.AttachmentReferenceNo))
                        {
                            res.DocCount = res.DocCount + 1;
                        }
                    }
                }
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/GetAllASNList", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/GetAllASNList", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ASNListView> GetAllASNListByPartnerID(string PartnerID)
        {
            try
            {
                var result = (from tb in _dbContext.BPCASNHeaders
                                  //join tb1 in _dbContext.BPCASNItems on tb.ASNNumber equals tb1.ASNNumber
                              where tb.PatnerID == PartnerID
                              orderby tb.CreatedOn descending
                              select new ASNListView()
                              {
                                  Client = tb.Client,
                                  Company = tb.Company,
                                  Type = tb.Type,
                                  PatnerID = tb.PatnerID,
                                  ASNNumber = tb.ASNNumber,
                                  ASNDate = tb.ASNDate,
                                  ArrivalDate = tb.ArrivalDate,
                                  CancelDuration = tb.CancelDuration,
                                  DepartureDate = tb.DepartureDate,
                                  DocNumber = tb.DocNumber,
                                  AWBNumber = tb.AWBNumber,
                                  VessleNumber = tb.VessleNumber,
                                  Plant = tb.Plant,
                                  InvDocReferenceNo = tb.InvDocReferenceNo,
                                  InvoiceNumber = tb.InvoiceNumber,
                                  POBasicPrice = tb.POBasicPrice,
                                  InvoiceAmount = tb.InvoiceAmount,
                                  TaxAmount = tb.TaxAmount,
                                  //Material = tb1.Material,
                                  //MaterialText = tb1.MaterialText,
                                  //ASNQty = tb1.ASNQty,
                                  Status = tb.Status,
                                  IsBuyerApprovalRequired = tb.IsBuyerApprovalRequired,
                                  BuyerApprovalStatus = tb.BuyerApprovalStatus,
                                  BuyerApprovalOn = tb.BuyerApprovalOn
                              }).ToList();
                foreach (var res in result)
                {
                    //if (!string.IsNullOrEmpty(res.InvDocReferenceNo))
                    //{
                    //    res.InvoiceAttachmentName = (from tb in _dbContext.BPCAttachments
                    //                                 where tb.AttachmentID.ToString() == res.InvDocReferenceNo
                    //                                 select tb.AttachmentName).FirstOrDefault();
                    //}
                    res.DocCount = 0;
                    if (!string.IsNullOrEmpty(res.InvDocReferenceNo))
                    {
                        res.InvoiceAttachmentName = (from tb in _dbContext.BPCAttachments
                                                     where tb.AttachmentID.ToString() == res.InvDocReferenceNo
                                                     select tb.AttachmentName).FirstOrDefault();
                        if (!string.IsNullOrEmpty(res.InvoiceAttachmentName))
                        {
                            res.DocCount = res.DocCount + 1;
                        }
                    }
                    var docCenters = _dbContext.BPCDocumentCenters.Where(x => x.ASNNumber == res.ASNNumber).ToList();
                    foreach (var doc in docCenters)
                    {
                        if (!string.IsNullOrEmpty(doc.AttachmentReferenceNo))
                        {
                            res.DocCount = res.DocCount + 1;
                        }
                    }
                }
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/GetAllASNListByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/GetAllASNListByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ASNListView> FilterASNList(string VendorCode = null, string ASNNumber = null, string DocNumber = null, string Material = null, string Status = null, DateTime? ASNFromDate = null, DateTime? ASNToDate = null)
        {
            try
            {
                bool IsVendorCode = !string.IsNullOrEmpty(VendorCode);
                bool IsASNNumber = !string.IsNullOrEmpty(ASNNumber);
                bool IsDocNumber = !string.IsNullOrEmpty(DocNumber);
                //bool IsMaterial = !string.IsNullOrEmpty(Material);
                bool IsStatus = !string.IsNullOrEmpty(Status);
                bool IsFromDate = ASNFromDate.HasValue;
                bool IsToDate = ASNToDate.HasValue;
                var result = (from tb in _dbContext.BPCASNHeaders
                                  //join tb1 in _dbContext.BPCASNItems on tb.ASNNumber equals tb1.ASNNumber
                              where (!IsVendorCode || tb.PatnerID == VendorCode) && (!IsASNNumber || tb.ASNNumber == ASNNumber) && (!IsDocNumber || tb.DocNumber == DocNumber)
                              //&& (!IsMaterial || tb1.Material.ToLower() == Material.ToLower()) 
                              && (!IsStatus || tb.Status.ToLower() == Status.ToLower())
                              && (!IsFromDate || (tb.ASNDate.HasValue && tb.ASNDate.Value.Date >= ASNFromDate.Value.Date))
                              && (!IsToDate || (tb.ASNDate.HasValue && tb.ASNDate.Value.Date <= ASNToDate.Value.Date))
                              orderby tb.CreatedOn descending
                              select new ASNListView()
                              {
                                  Client = tb.Client,
                                  Company = tb.Company,
                                  Type = tb.Type,
                                  PatnerID = tb.PatnerID,
                                  ASNNumber = tb.ASNNumber,
                                  ASNDate = tb.ASNDate,
                                  DocNumber = tb.DocNumber,
                                  AWBNumber = tb.AWBNumber,
                                  VessleNumber = tb.VessleNumber,
                                  Plant = tb.Plant,
                                  InvDocReferenceNo = tb.InvDocReferenceNo,
                                  InvoiceNumber = tb.InvoiceNumber,
                                  POBasicPrice = tb.POBasicPrice,
                                  InvoiceAmount = tb.InvoiceAmount,
                                  TaxAmount = tb.TaxAmount,
                                  //Material = tb1.Material,
                                  //MaterialText = tb1.MaterialText,
                                  //ASNQty = tb1.ASNQty,
                                  Status = tb.Status,
                                  IsBuyerApprovalRequired = tb.IsBuyerApprovalRequired,
                                  BuyerApprovalStatus = tb.BuyerApprovalStatus,
                                  BuyerApprovalOn = tb.BuyerApprovalOn
                              }).ToList();
                foreach (var res in result)
                {
                    //if (!string.IsNullOrEmpty(res.InvDocReferenceNo))
                    //{
                    //    res.InvoiceAttachmentName = (from tb in _dbContext.BPCAttachments
                    //                                 where tb.AttachmentID.ToString() == res.InvDocReferenceNo
                    //                                 select tb.AttachmentName).FirstOrDefault();
                    //}
                    res.DocCount = 0;
                    if (!string.IsNullOrEmpty(res.InvDocReferenceNo))
                    {
                        res.InvoiceAttachmentName = (from tb in _dbContext.BPCAttachments
                                                     where tb.AttachmentID.ToString() == res.InvDocReferenceNo
                                                     select tb.AttachmentName).FirstOrDefault();
                        if (!string.IsNullOrEmpty(res.InvoiceAttachmentName))
                        {
                            res.DocCount = res.DocCount + 1;
                        }
                    }
                    var docCenters = _dbContext.BPCDocumentCenters.Where(x => x.ASNNumber == res.ASNNumber).ToList();
                    foreach (var doc in docCenters)
                    {
                        if (!string.IsNullOrEmpty(doc.AttachmentReferenceNo))
                        {
                            res.DocCount = res.DocCount + 1;
                        }
                    }
                }
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/FilterASNList", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/FilterASNList", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ASNListView> FilterASNList1(string VendorCode = null, string ASNNumber = null, string DocNumber = null, string Material = null, string Status = null, DateTime? ASNFromDate = null, DateTime? ASNToDate = null)
        {
            try
            {
                bool IsVendorCode = !string.IsNullOrEmpty(VendorCode);
                bool IsASNNumber = !string.IsNullOrEmpty(ASNNumber);
                bool IsDocNumber = !string.IsNullOrEmpty(DocNumber);
                //bool IsMaterial = !string.IsNullOrEmpty(Material);
                bool IsStatus = !string.IsNullOrEmpty(Status);
                bool IsFromDate = ASNFromDate.HasValue;
                bool IsToDate = ASNToDate.HasValue;
                var result = (from tb in _dbContext.BPCASNHeader1
                              join tb1 in _dbContext.BPCASNOFMap1 on tb.ASNNumber equals tb1.ASNNumber
                              //join tb1 in _dbContext.BPCASNItems on tb.ASNNumber equals tb1.ASNNumber
                              where (!IsVendorCode || tb.PatnerID == VendorCode) && (!IsASNNumber || tb.ASNNumber == ASNNumber) && (!IsDocNumber || tb1.DocNumber == DocNumber)
                              //&& (!IsMaterial || tb1.Material.ToLower() == Material.ToLower()) 
                              && (!IsStatus || tb.Status.ToLower() == Status.ToLower())
                              && (!IsFromDate || (tb.ASNDate.HasValue && tb.ASNDate.Value.Date >= ASNFromDate.Value.Date))
                              && (!IsToDate || (tb.ASNDate.HasValue && tb.ASNDate.Value.Date <= ASNToDate.Value.Date))
                              orderby tb.CreatedOn descending
                              select new ASNListView()
                              {
                                  Client = tb.Client,
                                  Company = tb.Company,
                                  Type = tb.Type,
                                  PatnerID = tb.PatnerID,
                                  ASNNumber = tb.ASNNumber,
                                  ASNDate = tb.ASNDate,
                                  DocNumber = tb.DocNumber,
                                  AWBNumber = tb.AWBNumber,
                                  VessleNumber = tb.VessleNumber,
                                  Plant = tb.Plant,
                                  InvDocReferenceNo = tb.InvDocReferenceNo,
                                  InvoiceNumber = tb.InvoiceNumber,
                                  POBasicPrice = tb.POBasicPrice,
                                  InvoiceAmount = tb.InvoiceAmount,
                                  TaxAmount = tb.TaxAmount,
                                  ArrivalDate = tb.ArrivalDate,
                                  DepartureDate = tb.DepartureDate,
                                  //Material = tb1.Material,
                                  //MaterialText = tb1.MaterialText,
                                  //ASNQty = tb1.ASNQty,
                                  Status = tb.Status,
                                  IsBuyerApprovalRequired = tb.IsBuyerApprovalRequired,
                                  BuyerApprovalStatus = tb.BuyerApprovalStatus,
                                  BuyerApprovalOn = tb.BuyerApprovalOn
                              }).ToList();
                foreach (var res in result)
                {
                    //if (!string.IsNullOrEmpty(res.InvDocReferenceNo))
                    //{
                    //    res.InvoiceAttachmentName = (from tb in _dbContext.BPCAttachments
                    //                                 where tb.AttachmentID.ToString() == res.InvDocReferenceNo
                    //                                 select tb.AttachmentName).FirstOrDefault();
                    //}
                    res.DocCount = 0;
                    if (!string.IsNullOrEmpty(res.InvDocReferenceNo))
                    {
                        res.InvoiceAttachmentName = (from tb in _dbContext.BPCAttachments
                                                     where tb.AttachmentID.ToString() == res.InvDocReferenceNo
                                                     select tb.AttachmentName).FirstOrDefault();
                        if (!string.IsNullOrEmpty(res.InvoiceAttachmentName))
                        {
                            res.DocCount = res.DocCount + 1;
                        }
                    }
                    var docCenters = _dbContext.BPCDocumentCenters.Where(x => x.ASNNumber == res.ASNNumber).ToList();
                    foreach (var doc in docCenters)
                    {
                        if (!string.IsNullOrEmpty(doc.AttachmentReferenceNo))
                        {
                            res.DocCount = res.DocCount + 1;
                        }
                    }
                }
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/FilterASNList1", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/FilterASNList1", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<ASNListView> FilterASNListByPlants(ASNListFilter filter)
        {
            try
            {
                bool IsPatnerID = !string.IsNullOrEmpty(filter.VendorCode);
                bool IsASNNumber = !string.IsNullOrEmpty(filter.ASNNumber);
                bool IsDocNumber = !string.IsNullOrEmpty(filter.DocNumber);
                bool IsMaterial = !string.IsNullOrEmpty(filter.Material);
                bool IsStatus = !string.IsNullOrEmpty(filter.Status);
                bool IsFromDate = filter.ASNFromDate.HasValue;
                bool IsToDate = filter.ASNToDate.HasValue;
                var result = (from tb in _dbContext.BPCASNHeaders
                                  //join tb1 in _dbContext.BPCASNItems on tb.ASNNumber equals tb1.ASNNumber
                              where (filter.Plants.Any(x => x == tb.Plant)) && (!IsASNNumber || tb.ASNNumber == filter.ASNNumber) && (!IsDocNumber || tb.DocNumber == filter.DocNumber)
                              //&& (!IsMaterial || tb1.Material.ToLower() == filter.Material.ToLower()) 
                              && (!IsPatnerID || tb.PatnerID == filter.VendorCode)
                              && (!IsStatus || tb.Status.ToLower() == filter.Status.ToLower())
                              && (!IsFromDate || (tb.ASNDate.HasValue && tb.ASNDate.Value.Date >= filter.ASNFromDate.Value.Date))
                              && (!IsToDate || (tb.ASNDate.HasValue && tb.ASNDate.Value.Date <= filter.ASNToDate.Value.Date))
                              orderby tb.CreatedOn descending
                              select new ASNListView()
                              {
                                  Client = tb.Client,
                                  Company = tb.Company,
                                  Type = tb.Type,
                                  PatnerID = tb.PatnerID,
                                  ASNNumber = tb.ASNNumber,
                                  ASNDate = tb.ASNDate,
                                  DocNumber = tb.DocNumber,
                                  AWBNumber = tb.AWBNumber,
                                  VessleNumber = tb.VessleNumber,
                                  Plant = tb.Plant,
                                  InvDocReferenceNo = tb.InvDocReferenceNo,
                                  InvoiceNumber = tb.InvoiceNumber,
                                  POBasicPrice = tb.POBasicPrice,
                                  InvoiceAmount = tb.InvoiceAmount,
                                  TaxAmount = tb.TaxAmount,
                                  ArrivalDate = tb.ArrivalDate,
                                  DepartureDate = tb.DepartureDate,
                                  //Material = tb1.Material,
                                  //MaterialText = tb1.MaterialText,
                                  //ASNQty = tb1.ASNQty,
                                  Status = tb.Status,
                                  IsBuyerApprovalRequired = tb.IsBuyerApprovalRequired,
                                  BuyerApprovalStatus = tb.BuyerApprovalStatus,
                                  BuyerApprovalOn = tb.BuyerApprovalOn
                              }).ToList();
                if(result.Count > 0)
                {
                    foreach (var res in result)
                    {
                        res.DocCount = 0;
                        if (!string.IsNullOrEmpty(res.InvDocReferenceNo))
                        {
                            res.InvoiceAttachmentName = (from tb in _dbContext.BPCAttachments
                                                         where tb.AttachmentID.ToString() == res.InvDocReferenceNo
                                                         select tb.AttachmentName).FirstOrDefault();
                            if (!string.IsNullOrEmpty(res.InvoiceAttachmentName))
                            {
                                res.DocCount = res.DocCount + 1;
                            }
                        }
                        var docCenters = _dbContext.BPCDocumentCenters.Where(x => x.ASNNumber == res.ASNNumber).ToList();
                        foreach (var doc in docCenters)
                        {
                            if (!string.IsNullOrEmpty(doc.AttachmentReferenceNo))
                            {
                                res.DocCount = res.DocCount + 1;
                            }
                        }
                    }

                }

                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/FilterASNListByPlants", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/FilterASNListByPlants", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ASNListView> FilterASNList1ByPlants(ASNListFilter filter)
        {
            try
            {
                bool IsASNNumber = !string.IsNullOrEmpty(filter.ASNNumber);
                bool IsDocNumber = !string.IsNullOrEmpty(filter.DocNumber);
                bool IsMaterial = !string.IsNullOrEmpty(filter.Material);
                bool IsStatus = !string.IsNullOrEmpty(filter.Status);
                bool IsFromDate = filter.ASNFromDate.HasValue;
                bool IsToDate = filter.ASNToDate.HasValue;
                var result = (from tb in _dbContext.BPCASNHeader1
                              join tb1 in _dbContext.BPCASNOFMap1 on tb.ASNNumber equals tb1.ASNNumber
                              //join tb1 in _dbContext.BPCASNItems on tb.ASNNumber equals tb1.ASNNumber
                              where (filter.Plants.Any(x => x == tb.Plant)) && (!IsASNNumber || tb.ASNNumber == filter.ASNNumber) && (!IsDocNumber || tb1.DocNumber == filter.DocNumber)
                              //&& (!IsMaterial || tb1.Material.ToLower() == filter.Material.ToLower()) 
                              && (!IsStatus || tb.Status.ToLower() == filter.Status.ToLower())
                              && (!IsFromDate || (tb.ASNDate.HasValue && tb.ASNDate.Value.Date >= filter.ASNFromDate.Value.Date))
                              && (!IsToDate || (tb.ASNDate.HasValue && tb.ASNDate.Value.Date <= filter.ASNToDate.Value.Date))
                              orderby tb.CreatedOn descending
                              select new ASNListView()
                              {
                                  Client = tb.Client,
                                  Company = tb.Company,
                                  Type = tb.Type,
                                  PatnerID = tb.PatnerID,
                                  ASNNumber = tb.ASNNumber,
                                  ASNDate = tb.ASNDate,
                                  DocNumber = tb1.DocNumber,
                                  AWBNumber = tb.AWBNumber,
                                  VessleNumber = tb.VessleNumber,
                                  Plant = tb.Plant,
                                  InvDocReferenceNo = tb.InvDocReferenceNo,
                                  InvoiceNumber = tb.InvoiceNumber,
                                  POBasicPrice = tb.POBasicPrice,
                                  InvoiceAmount = tb.InvoiceAmount,
                                  TaxAmount = tb.TaxAmount,
                                  ArrivalDate = tb.ArrivalDate,
                                  DepartureDate = tb.DepartureDate,
                                  //Material = tb1.Material,
                                  //MaterialText = tb1.MaterialText,
                                  //ASNQty = tb1.ASNQty,
                                  Status = tb.Status,
                                  IsBuyerApprovalRequired = tb.IsBuyerApprovalRequired,
                                  BuyerApprovalStatus = tb.BuyerApprovalStatus,
                                  BuyerApprovalOn = tb.BuyerApprovalOn
                              }).ToList();
                if(result.Count >0)
                {
                    foreach (var res in result)
                    {
                        res.DocCount = 0;
                        if (!string.IsNullOrEmpty(res.InvDocReferenceNo))
                        {
                            res.InvoiceAttachmentName = (from tb in _dbContext.BPCAttachments
                                                         where tb.AttachmentID.ToString() == res.InvDocReferenceNo
                                                         select tb.AttachmentName).FirstOrDefault();
                            if (!string.IsNullOrEmpty(res.InvoiceAttachmentName))
                            {
                                res.DocCount = res.DocCount + 1;
                            }
                        }
                        var docCenters = _dbContext.BPCDocumentCenters.Where(x => x.ASNNumber == res.ASNNumber).ToList();
                        foreach (var doc in docCenters)
                        {
                            if (!string.IsNullOrEmpty(doc.AttachmentReferenceNo))
                            {
                                res.DocCount = res.DocCount + 1;
                            }
                        }
                    }

                }

                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/FilterASNList1ByPlants", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/FilterASNList1ByPlants", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<BPCASNAttachment> GetASNAttachmentsASNNumber(string ASNNumber)
        {
            try
            {
                List<BPCASNAttachment> bPCASNAttachments = new List<BPCASNAttachment>();
                var InvoiceAttachment = (from tb in _dbContext.BPCASNHeaders
                                         join tb1 in _dbContext.BPCAttachments on tb.InvDocReferenceNo equals tb1.AttachmentID.ToString()
                                         where tb.ASNNumber == ASNNumber
                                         select new
                                         {
                                             tb1.AttachmentID,
                                             tb1.AttachmentName,
                                             tb.InvoiceNumber
                                         }).FirstOrDefault();


                if (InvoiceAttachment != null && !string.IsNullOrEmpty(InvoiceAttachment.AttachmentName))
                {
                    BPCASNAttachment bPCASNAttachment = new BPCASNAttachment();
                    bPCASNAttachment.ASNNumber = ASNNumber;
                    bPCASNAttachment.AttachmentID = InvoiceAttachment.AttachmentID;
                    bPCASNAttachment.AttachmentName = InvoiceAttachment.AttachmentName;
                    bPCASNAttachment.Type = "Invoice";
                    bPCASNAttachment.Title = InvoiceAttachment.InvoiceNumber;
                    bPCASNAttachments.Add(bPCASNAttachment);
                }
                var docCenters = (from tb in _dbContext.BPCDocumentCenters
                                  join tb1 in _dbContext.BPCAttachments on tb.AttachmentReferenceNo equals tb1.AttachmentID.ToString()
                                  where tb.ASNNumber == ASNNumber
                                  select new
                                  {
                                      tb1.AttachmentID,
                                      tb1.AttachmentName,
                                      tb.DocumentTitle,
                                      tb.DocumentType,
                                  }).ToList();
                foreach (var doc in docCenters)
                {
                    if (!string.IsNullOrEmpty(doc.AttachmentName))
                    {
                        BPCASNAttachment bPCASNAttachment = new BPCASNAttachment();
                        bPCASNAttachment.ASNNumber = ASNNumber;
                        bPCASNAttachment.AttachmentID = doc.AttachmentID;
                        bPCASNAttachment.AttachmentName = doc.AttachmentName;
                        bPCASNAttachment.Type = "DocumentCenter";
                        bPCASNAttachment.DocumentType = doc.DocumentType;
                        bPCASNAttachment.Title = doc.DocumentTitle;
                        bPCASNAttachments.Add(bPCASNAttachment);
                    }
                }
                return bPCASNAttachments;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/GetASNAttachmentsASNNumber", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/GetASNAttachmentsASNNumber", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCASNAttachment> GetASNAttachment1ASNNumber(string ASNNumber)
        {
            try
            {
                List<BPCASNAttachment> bPCASNAttachments = new List<BPCASNAttachment>();
                var InvoiceAttachment = (from tb in _dbContext.BPCASNHeader1
                                         join tb1 in _dbContext.BPCAttachments on tb.InvDocReferenceNo equals tb1.AttachmentID.ToString()
                                         where tb.ASNNumber == ASNNumber
                                         select new
                                         {
                                             tb1.AttachmentID,
                                             tb1.AttachmentName,
                                             tb.InvoiceNumber
                                         }).FirstOrDefault();


                if (InvoiceAttachment != null && !string.IsNullOrEmpty(InvoiceAttachment.AttachmentName))
                {
                    BPCASNAttachment bPCASNAttachment = new BPCASNAttachment();
                    bPCASNAttachment.ASNNumber = ASNNumber;
                    bPCASNAttachment.AttachmentID = InvoiceAttachment.AttachmentID;
                    bPCASNAttachment.AttachmentName = InvoiceAttachment.AttachmentName;
                    bPCASNAttachment.Type = "Invoice";
                    bPCASNAttachment.Title = InvoiceAttachment.InvoiceNumber;
                    bPCASNAttachments.Add(bPCASNAttachment);
                }
                var docCenters = (from tb in _dbContext.BPCDocumentCenters
                                  join tb1 in _dbContext.BPCAttachments on tb.AttachmentReferenceNo equals tb1.AttachmentID.ToString()
                                  where tb.ASNNumber == ASNNumber
                                  select new
                                  {
                                      tb1.AttachmentID,
                                      tb1.AttachmentName,
                                      tb.DocumentTitle,
                                      tb.DocumentType,
                                  }).ToList();
                foreach (var doc in docCenters)
                {
                    if (!string.IsNullOrEmpty(doc.AttachmentName))
                    {
                        BPCASNAttachment bPCASNAttachment = new BPCASNAttachment();
                        bPCASNAttachment.ASNNumber = ASNNumber;
                        bPCASNAttachment.AttachmentID = doc.AttachmentID;
                        bPCASNAttachment.AttachmentName = doc.AttachmentName;
                        bPCASNAttachment.Type = "DocumentCenter";
                        bPCASNAttachment.DocumentType = doc.DocumentType;
                        bPCASNAttachment.Title = doc.DocumentTitle;
                        bPCASNAttachments.Add(bPCASNAttachment);
                    }
                }
                return bPCASNAttachments;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/GetASNAttachmentsASNNumber", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/GetASNAttachmentsASNNumber", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<ASNListView> FilterASNListByPartnerID(string PartnerID, string ASNNumber = null, string DocNumber = null, string Material = null, string Status = null, DateTime? ASNFromDate = null, DateTime? ASNToDate = null)
        {
            try
            {
                bool IsASNNumber = !string.IsNullOrEmpty(ASNNumber);
                bool IsDocNumber = !string.IsNullOrEmpty(DocNumber);
                //bool IsMaterial = !string.IsNullOrEmpty(Material);
                bool IsStatus = !string.IsNullOrEmpty(Status);
                bool IsFromDate = ASNFromDate.HasValue;
                bool IsToDate = ASNToDate.HasValue;
                var result = (from tb in _dbContext.BPCASNHeaders
                                  // join tb1 in _dbContext.BPCASNItems on tb.ASNNumber equals tb1.ASNNumber
                              where tb.PatnerID == PartnerID
                              && (!IsASNNumber || tb.ASNNumber == ASNNumber)
                              && (!IsDocNumber || tb.DocNumber == DocNumber)
                              //&& (!IsMaterial || tb1.Material.ToLower() == Material.ToLower())
                              && (!IsStatus || tb.Status.ToLower() == Status.ToLower())
                              && (!IsFromDate || (tb.ASNDate.HasValue && tb.ASNDate.Value.Date >= ASNFromDate.Value.Date))
                              && (!IsToDate || (tb.ASNDate.HasValue && tb.ASNDate.Value.Date <= ASNToDate.Value.Date))
                              orderby tb.CreatedOn descending
                              select new ASNListView()
                              {
                                  Client = tb.Client,
                                  Company = tb.Company,
                                  Type = tb.Type,
                                  PatnerID = tb.PatnerID,
                                  ASNNumber = tb.ASNNumber,
                                  ASNDate = tb.ASNDate,
                                  ArrivalDate = tb.ArrivalDate,
                                  CancelDuration = tb.CancelDuration,
                                  DepartureDate = tb.DepartureDate,
                                  DocNumber = tb.DocNumber,
                                  AWBNumber = tb.AWBNumber,
                                  VessleNumber = tb.VessleNumber,
                                  Plant = tb.Plant,
                                  InvDocReferenceNo = tb.InvDocReferenceNo,
                                  InvoiceNumber = tb.InvoiceNumber,
                                  POBasicPrice = tb.POBasicPrice,
                                  InvoiceAmount = tb.InvoiceAmount,
                                  TaxAmount = tb.TaxAmount,
                                  //Material = tb1.Material,
                                  //MaterialText = tb1.MaterialText,
                                  //ASNQty = tb1.ASNQty,
                                  Status = tb.Status

                              }).ToList();
                foreach (var res in result)
                {
                    //if (!string.IsNullOrEmpty(res.InvDocReferenceNo))
                    //{
                    //    res.InvoiceAttachmentName = (from tb in _dbContext.BPCAttachments
                    //                                 where tb.AttachmentID.ToString() == res.InvDocReferenceNo
                    //                                 select tb.AttachmentName).FirstOrDefault();
                    //}
                    res.DocCount = 0;
                    if (!string.IsNullOrEmpty(res.InvDocReferenceNo))
                    {
                        res.InvoiceAttachmentName = (from tb in _dbContext.BPCAttachments
                                                     where tb.AttachmentID.ToString() == res.InvDocReferenceNo
                                                     select tb.AttachmentName).FirstOrDefault();
                        if (!string.IsNullOrEmpty(res.InvoiceAttachmentName))
                        {
                            res.DocCount = res.DocCount + 1;
                        }
                    }
                    var docCenters = _dbContext.BPCDocumentCenters.Where(x => x.ASNNumber == res.ASNNumber).ToList();
                    foreach (var doc in docCenters)
                    {
                        if (!string.IsNullOrEmpty(doc.AttachmentReferenceNo))
                        {
                            res.DocCount = res.DocCount + 1;
                        }
                    }
                }
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/FilterASNListByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/FilterASNListByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<ASNListView> FilterASNListByPlant(string GetPlantByUser, string ASNNumber = null, string DocNumber = null, string Material = null, string Status = null, DateTime? ASNFromDate = null, DateTime? ASNToDate = null)
        {
            try
            {
                string[] Plants = GetPlantByUser.Split(',');
                for (int i = 0; i < Plants.Length; i++)
                {
                    Plants[i] = Plants[i].Trim();
                }
                bool IsASNNumber = !string.IsNullOrEmpty(ASNNumber);
                bool IsDocNumber = !string.IsNullOrEmpty(DocNumber);
                //bool IsMaterial = !string.IsNullOrEmpty(Material);
                bool IsStatus = !string.IsNullOrEmpty(Status);
                bool IsFromDate = ASNFromDate.HasValue;
                bool IsToDate = ASNToDate.HasValue;
                List<ASNListView> asnlist = new List<ASNListView>();
                List<ASNListView> asnlists = new List<ASNListView>();
                for (int i = 0; i < Plants.Length; i++)
                {
                    asnlist = (from tb in _dbContext.BPCASNHeaders
                                   // join tb1 in _dbContext.BPCASNItems on tb.ASNNumber equals tb1.ASNNumber
                               where tb.Plant == Plants[i]
                               && (!IsASNNumber || tb.ASNNumber == ASNNumber)
                               && (!IsDocNumber || tb.DocNumber == DocNumber)
                               //&& (!IsMaterial || tb1.Material.ToLower() == Material.ToLower())
                               && (!IsStatus || tb.Status.ToLower() == Status.ToLower())
                               && (!IsFromDate || (tb.ASNDate.HasValue && tb.ASNDate.Value.Date >= ASNFromDate.Value.Date))
                               && (!IsToDate || (tb.ASNDate.HasValue && tb.ASNDate.Value.Date <= ASNToDate.Value.Date))
                               orderby tb.CreatedOn descending
                               select new ASNListView()
                               {
                                   Client = tb.Client,
                                   Company = tb.Company,
                                   Type = tb.Type,
                                   PatnerID = tb.PatnerID,
                                   ASNNumber = tb.ASNNumber,
                                   ASNDate = tb.ASNDate,
                                   ArrivalDate = tb.ArrivalDate,
                                   CancelDuration = tb.CancelDuration,
                                   DepartureDate = tb.DepartureDate,
                                   DocNumber = tb.DocNumber,
                                   AWBNumber = tb.AWBNumber,
                                   VessleNumber = tb.VessleNumber,
                                   Plant = tb.Plant,
                                   InvDocReferenceNo = tb.InvDocReferenceNo,
                                   InvoiceNumber = tb.InvoiceNumber,
                                   POBasicPrice = tb.POBasicPrice,
                                   InvoiceAmount = tb.InvoiceAmount,
                                   TaxAmount = tb.TaxAmount,
                                   //Material = tb1.Material,
                                   //MaterialText = tb1.MaterialText,
                                   //ASNQty = tb1.ASNQty,
                                   Status = tb.Status

                               }).ToList();
                    asnlists.AddRange(asnlist);
                }
                foreach (var res in asnlists)
                {
                    //if (!string.IsNullOrEmpty(res.InvDocReferenceNo))
                    //{
                    //    res.InvoiceAttachmentName = (from tb in _dbContext.BPCAttachments
                    //                                 where tb.AttachmentID.ToString() == res.InvDocReferenceNo
                    //                                 select tb.AttachmentName).FirstOrDefault();
                    //}
                    res.DocCount = 0;
                    if (!string.IsNullOrEmpty(res.InvDocReferenceNo))
                    {
                        res.InvoiceAttachmentName = (from tb in _dbContext.BPCAttachments
                                                     where tb.AttachmentID.ToString() == res.InvDocReferenceNo
                                                     select tb.AttachmentName).FirstOrDefault();
                        if (!string.IsNullOrEmpty(res.InvoiceAttachmentName))
                        {
                            res.DocCount = res.DocCount + 1;
                        }
                    }
                    var docCenters = _dbContext.BPCDocumentCenters.Where(x => x.ASNNumber == res.ASNNumber).ToList();
                    foreach (var doc in docCenters)
                    {
                        if (!string.IsNullOrEmpty(doc.AttachmentReferenceNo))
                        {
                            res.DocCount = res.DocCount + 1;
                        }
                    }
                }
                return asnlists;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/FilterASNListByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/FilterASNListByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ASNListView> FilterASNListBySuperUser(string GetPlantByUser, string ASNNumber = null, string DocNumber = null, string Material = null, string Status = null, DateTime? ASNFromDate = null, DateTime? ASNToDate = null)
        {
            try
            {
                bool IsASNNumber = !string.IsNullOrEmpty(ASNNumber);
                bool IsDocNumber = !string.IsNullOrEmpty(DocNumber);
                //bool IsMaterial = !string.IsNullOrEmpty(Material);
                bool IsStatus = !string.IsNullOrEmpty(Status);
                bool IsFromDate = ASNFromDate.HasValue;
                bool IsToDate = ASNToDate.HasValue;
                string[] Plants = GetPlantByUser.Split(',');
                for (int i = 0; i < Plants.Length; i++)
                {
                    Plants[i] = Plants[i].Trim();
                }
                List<ASNListView> headers = new List<ASNListView>();
                List<ASNListView> header = new List<ASNListView>();
                for (int i = 0; i < Plants.Length; i++)
                {
                    headers = (from tb in _dbContext.BPCASNHeaders
                                   // join tb1 in _dbContext.BPCASNItems on tb.ASNNumber equals tb1.ASNNumber
                               where tb.Plant == Plants[i]
                               && (!IsASNNumber || tb.ASNNumber == ASNNumber)
                               && (!IsDocNumber || tb.DocNumber == DocNumber)
                               //&& (!IsMaterial || tb1.Material.ToLower() == Material.ToLower())
                               && (!IsStatus || tb.Status.ToLower() == Status.ToLower())
                               && (!IsFromDate || (tb.ASNDate.HasValue && tb.ASNDate.Value.Date >= ASNFromDate.Value.Date))
                               && (!IsToDate || (tb.ASNDate.HasValue && tb.ASNDate.Value.Date <= ASNToDate.Value.Date))
                               orderby tb.CreatedOn descending
                               select new ASNListView()
                               {
                                   Client = tb.Client,
                                   Company = tb.Company,
                                   Type = tb.Type,
                                   PatnerID = tb.PatnerID,
                                   ASNNumber = tb.ASNNumber,
                                   ASNDate = tb.ASNDate,
                                   ArrivalDate = tb.ArrivalDate,
                                   CancelDuration = tb.CancelDuration,
                                   DepartureDate = tb.DepartureDate,
                                   DocNumber = tb.DocNumber,
                                   AWBNumber = tb.AWBNumber,
                                   VessleNumber = tb.VessleNumber,
                                   Plant = tb.Plant,
                                   InvDocReferenceNo = tb.InvDocReferenceNo,
                                   InvoiceNumber = tb.InvoiceNumber,
                                   POBasicPrice = tb.POBasicPrice,
                                   InvoiceAmount = tb.InvoiceAmount,
                                   TaxAmount = tb.TaxAmount,
                                   //Material = tb1.Material,
                                   //MaterialText = tb1.MaterialText,
                                   //ASNQty = tb1.ASNQty,
                                   Status = tb.Status

                               }).ToList();
                    header.AddRange(headers);
                }
                foreach (var res in header)
                {
                    //if (!string.IsNullOrEmpty(res.InvDocReferenceNo))
                    //{
                    //    res.InvoiceAttachmentName = (from tb in _dbContext.BPCAttachments
                    //                                 where tb.AttachmentID.ToString() == res.InvDocReferenceNo
                    //                                 select tb.AttachmentName).FirstOrDefault();
                    //}
                    res.DocCount = 0;
                    if (!string.IsNullOrEmpty(res.InvDocReferenceNo))
                    {
                        res.InvoiceAttachmentName = (from tb in _dbContext.BPCAttachments
                                                     where tb.AttachmentID.ToString() == res.InvDocReferenceNo
                                                     select tb.AttachmentName).FirstOrDefault();
                        if (!string.IsNullOrEmpty(res.InvoiceAttachmentName))
                        {
                            res.DocCount = res.DocCount + 1;
                        }
                    }
                    var docCenters = _dbContext.BPCDocumentCenters.Where(x => x.ASNNumber == res.ASNNumber).ToList();
                    foreach (var doc in docCenters)
                    {
                        if (!string.IsNullOrEmpty(doc.AttachmentReferenceNo))
                        {
                            res.DocCount = res.DocCount + 1;
                        }
                    }
                }
                return headers;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/FilterASNListByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/FilterASNListByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ASNListView> FilterASNList1ByPartnerID(string PartnerID, string ASNNumber = null, string DocNumber = null, string Material = null, string Status = null, DateTime? ASNFromDate = null, DateTime? ASNToDate = null)
        {
            try
            {
                bool IsASNNumber = !string.IsNullOrEmpty(ASNNumber);
                bool IsDocNumber = !string.IsNullOrEmpty(DocNumber);
                //bool IsMaterial = !string.IsNullOrEmpty(Material);
                bool IsStatus = !string.IsNullOrEmpty(Status);
                bool IsFromDate = ASNFromDate.HasValue;
                bool IsToDate = ASNToDate.HasValue;
                var result = (from tb in _dbContext.BPCASNHeader1
                              join tb1 in _dbContext.BPCASNOFMap1 on tb.ASNNumber equals tb1.ASNNumber
                              // join tb1 in _dbContext.BPCASNItems on tb.ASNNumber equals tb1.ASNNumber
                              where tb.PatnerID == PartnerID
                              && (!IsASNNumber || tb.ASNNumber == ASNNumber)
                              && (!IsDocNumber || tb1.DocNumber == DocNumber)
                              //&& (!IsMaterial || tb1.Material.ToLower() == Material.ToLower())
                              && (!IsStatus || tb.Status.ToLower() == Status.ToLower())
                              && (!IsFromDate || (tb.ASNDate.HasValue && tb.ASNDate.Value.Date >= ASNFromDate.Value.Date))
                              && (!IsToDate || (tb.ASNDate.HasValue && tb.ASNDate.Value.Date <= ASNToDate.Value.Date))
                              orderby tb.CreatedOn descending
                              select new ASNListView()
                              {
                                  Client = tb.Client,
                                  Company = tb.Company,
                                  Type = tb.Type,
                                  PatnerID = tb.PatnerID,
                                  ASNNumber = tb.ASNNumber,
                                  ASNDate = tb.ASNDate,
                                  ArrivalDate = tb.ArrivalDate,
                                  CancelDuration = tb.CancelDuration,
                                  DepartureDate = tb.DepartureDate,
                                  DocNumber = tb1.DocNumber,
                                  AWBNumber = tb.AWBNumber,
                                  VessleNumber = tb.VessleNumber,
                                  Plant = tb.Plant,
                                  InvDocReferenceNo = tb.InvDocReferenceNo,
                                  InvoiceNumber = tb.InvoiceNumber,
                                  POBasicPrice = tb.POBasicPrice,
                                  InvoiceAmount = tb.InvoiceAmount,
                                  TaxAmount = tb.TaxAmount,
                                  //Material = tb1.Material,
                                  //MaterialText = tb1.MaterialText,
                                  //ASNQty = tb1.ASNQty,
                                  Status = tb.Status

                              }).ToList();
                foreach (var res in result)
                {
                    //if (!string.IsNullOrEmpty(res.InvDocReferenceNo))
                    //{
                    //    res.InvoiceAttachmentName = (from tb in _dbContext.BPCAttachments
                    //                                 where tb.AttachmentID.ToString() == res.InvDocReferenceNo
                    //                                 select tb.AttachmentName).FirstOrDefault();
                    //}
                    res.DocCount = 0;
                    if (!string.IsNullOrEmpty(res.InvDocReferenceNo))
                    {
                        res.InvoiceAttachmentName = (from tb in _dbContext.BPCAttachments
                                                     where tb.AttachmentID.ToString() == res.InvDocReferenceNo
                                                     select tb.AttachmentName).FirstOrDefault();
                        if (!string.IsNullOrEmpty(res.InvoiceAttachmentName))
                        {
                            res.DocCount = res.DocCount + 1;
                        }
                    }
                    var docCenters = _dbContext.BPCDocumentCenters.Where(x => x.ASNNumber == res.ASNNumber).ToList();
                    foreach (var doc in docCenters)
                    {
                        if (!string.IsNullOrEmpty(doc.AttachmentReferenceNo))
                        {
                            res.DocCount = res.DocCount + 1;
                        }
                    }
                }
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/FilterASNListByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/FilterASNListByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ASNListView> FilterASNList1BySuperUser(string GetPlantByUser, string ASNNumber = null, string DocNumber = null, string Material = null, string Status = null, DateTime? ASNFromDate = null, DateTime? ASNToDate = null)
        {
            try
            {
                string[] Plants = GetPlantByUser.Split(',');
                for (int i = 0; i < Plants.Length; i++)
                {
                    Plants[i] = Plants[i].Trim();
                }
                List<ASNListView> ASNListHeaders = new List<ASNListView>();
                List<ASNListView> ASNListHeader = new List<ASNListView>();
                for (int i = 0; i < Plants.Length; i++)
                {
                    bool IsASNNumber = !string.IsNullOrEmpty(ASNNumber);
                    bool IsDocNumber = !string.IsNullOrEmpty(DocNumber);
                    //bool IsMaterial = !string.IsNullOrEmpty(Material);
                    bool IsStatus = !string.IsNullOrEmpty(Status);
                    bool IsFromDate = ASNFromDate.HasValue;
                    bool IsToDate = ASNToDate.HasValue;
                    ASNListHeader = (from tb in _dbContext.BPCASNHeader1
                                     join tb1 in _dbContext.BPCASNOFMap1 on tb.ASNNumber equals tb1.ASNNumber
                                     // join tb1 in _dbContext.BPCASNItems on tb.ASNNumber equals tb1.ASNNumber
                                     where tb.Plant == Plants[i]
                                     && (!IsASNNumber || tb.ASNNumber == ASNNumber)
                                     && (!IsDocNumber || tb1.DocNumber == DocNumber)
                                     //&& (!IsMaterial || tb1.Material.ToLower() == Material.ToLower())
                                     && (!IsStatus || tb.Status.ToLower() == Status.ToLower())
                                     && (!IsFromDate || (tb.ASNDate.HasValue && tb.ASNDate.Value.Date >= ASNFromDate.Value.Date))
                                     && (!IsToDate || (tb.ASNDate.HasValue && tb.ASNDate.Value.Date <= ASNToDate.Value.Date))
                                     orderby tb.CreatedOn descending
                                     select new ASNListView()
                                     {
                                         Client = tb.Client,
                                         Company = tb.Company,
                                         Type = tb.Type,
                                         PatnerID = tb.PatnerID,
                                         ASNNumber = tb.ASNNumber,
                                         ASNDate = tb.ASNDate,
                                         ArrivalDate = tb.ArrivalDate,
                                         CancelDuration = tb.CancelDuration,
                                         DepartureDate = tb.DepartureDate,
                                         DocNumber = tb1.DocNumber,
                                         AWBNumber = tb.AWBNumber,
                                         VessleNumber = tb.VessleNumber,
                                         Plant = tb.Plant,
                                         InvDocReferenceNo = tb.InvDocReferenceNo,
                                         InvoiceNumber = tb.InvoiceNumber,
                                         POBasicPrice = tb.POBasicPrice,
                                         InvoiceAmount = tb.InvoiceAmount,
                                         TaxAmount = tb.TaxAmount,
                                         //Material = tb1.Material,
                                         //MaterialText = tb1.MaterialText,
                                         //ASNQty = tb1.ASNQty,
                                         Status = tb.Status

                                     }).ToList();
                    ASNListHeaders.AddRange(ASNListHeader);
                }
                foreach (var res in ASNListHeaders)
                {
                    //if (!string.IsNullOrEmpty(res.InvDocReferenceNo))
                    //{
                    //    res.InvoiceAttachmentName = (from tb in _dbContext.BPCAttachments
                    //                                 where tb.AttachmentID.ToString() == res.InvDocReferenceNo
                    //                                 select tb.AttachmentName).FirstOrDefault();
                    //}
                    res.DocCount = 0;
                    if (!string.IsNullOrEmpty(res.InvDocReferenceNo))
                    {
                        res.InvoiceAttachmentName = (from tb in _dbContext.BPCAttachments
                                                     where tb.AttachmentID.ToString() == res.InvDocReferenceNo
                                                     select tb.AttachmentName).FirstOrDefault();
                        if (!string.IsNullOrEmpty(res.InvoiceAttachmentName))
                        {
                            res.DocCount = res.DocCount + 1;
                        }
                    }
                    var docCenters = _dbContext.BPCDocumentCenters.Where(x => x.ASNNumber == res.ASNNumber).ToList();
                    foreach (var doc in docCenters)
                    {
                        if (!string.IsNullOrEmpty(doc.AttachmentReferenceNo))
                        {
                            res.DocCount = res.DocCount + 1;
                        }
                    }
                }
                return ASNListHeaders;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/FilterASNListByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/FilterASNListByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ASNListView> FilterASNListBySER(string PartnerID, string ASNNumber = null, string DocNumber = null, string Material = null, string Status = null, DateTime? ASNFromDate = null, DateTime? ASNToDate = null)
        {
            try
            {
                bool IsASNNumber = !string.IsNullOrEmpty(ASNNumber);
                bool IsDocNumber = !string.IsNullOrEmpty(DocNumber);
                //bool IsMaterial = !string.IsNullOrEmpty(Material);
                bool IsStatus = !string.IsNullOrEmpty(Status);
                bool IsFromDate = ASNFromDate.HasValue;
                bool IsToDate = ASNToDate.HasValue;
                var OfHFilter = (from tb in _dbContext.BPCASNHeaders
                                 join of_h in _dbContext.BPCOFHeaders on tb.DocNumber equals of_h.DocNumber
                                 where of_h.DocType == "SER"
                                 select tb).ToList();

                var result = (from tb in OfHFilter
                                  //join tb1 in _dbContext.BPCASNItems on tb.ASNNumber equals tb1.ASNNumber
                              where tb.PatnerID == PartnerID
                              && (!IsASNNumber || tb.ASNNumber == ASNNumber)
                              && (!IsDocNumber || tb.DocNumber == DocNumber)
                              //&& (!IsMaterial || tb1.Material.ToLower() == Material.ToLower())
                              && (!IsStatus || tb.Status.ToLower() == Status.ToLower())
                              && (!IsFromDate || (tb.ASNDate.HasValue && tb.ASNDate.Value.Date >= ASNFromDate.Value.Date))
                              && (!IsToDate || (tb.ASNDate.HasValue && tb.ASNDate.Value.Date <= ASNToDate.Value.Date))
                              orderby tb.CreatedOn descending
                              select new ASNListView()
                              {
                                  Client = tb.Client,
                                  Company = tb.Company,
                                  Type = tb.Type,
                                  PatnerID = tb.PatnerID,
                                  ASNNumber = tb.ASNNumber,
                                  ASNDate = tb.ASNDate,
                                  ArrivalDate = tb.ArrivalDate,
                                  DepartureDate = tb.DepartureDate,
                                  DocNumber = tb.DocNumber,
                                  AWBNumber = tb.AWBNumber,
                                  VessleNumber = tb.VessleNumber,
                                  Plant = tb.Plant,
                                  InvDocReferenceNo = tb.InvDocReferenceNo,
                                  InvoiceNumber = tb.InvoiceNumber,
                                  POBasicPrice = tb.POBasicPrice,
                                  InvoiceAmount = tb.InvoiceAmount,
                                  TaxAmount = tb.TaxAmount,
                                  //Material = tb1.Material,
                                  //MaterialText = tb1.MaterialText,
                                  //ASNQty = tb1.ASNQty,
                                  Status = tb.Status
                              }).ToList();
                foreach (var res in result)
                {
                    //if (!string.IsNullOrEmpty(res.InvDocReferenceNo))
                    //{
                    //    res.InvoiceAttachmentName = (from tb in _dbContext.BPCAttachments
                    //                                 where tb.AttachmentID.ToString() == res.InvDocReferenceNo
                    //                                 select tb.AttachmentName).FirstOrDefault();
                    //}
                    res.DocCount = 0;
                    if (!string.IsNullOrEmpty(res.InvDocReferenceNo))
                    {
                        res.InvoiceAttachmentName = (from tb in _dbContext.BPCAttachments
                                                     where tb.AttachmentID.ToString() == res.InvDocReferenceNo
                                                     select tb.AttachmentName).FirstOrDefault();
                        if (!string.IsNullOrEmpty(res.InvoiceAttachmentName))
                        {
                            res.DocCount = res.DocCount + 1;
                        }
                    }
                    var docCenters = _dbContext.BPCDocumentCenters.Where(x => x.ASNNumber == res.ASNNumber).ToList();
                    foreach (var doc in docCenters)
                    {
                        if (!string.IsNullOrEmpty(doc.AttachmentReferenceNo))
                        {
                            res.DocCount = res.DocCount + 1;
                        }
                    }
                }
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/FilterASNListBySER", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/FilterASNListBySER", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCASNHeader> GetASNsByDoc(string DocNumber)
        {
            try
            {
                return _dbContext.BPCASNHeaders.Where(x => x.DocNumber == DocNumber).ToList(); 
                //var result = (from tb in _dbContext.BPCASNHeaders
                //              where tb.DocNumber == DocNumber
                //              orderby tb.CreatedOn descending
                //              select tb).ToList();
                //return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/GetASNsByDoc", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/GetASNsByDoc", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCASNHeader> GetASNByDocAndPartnerID(string DocNumber, string PartnerID)
        {
            try
            {
                var result = (from tb in _dbContext.BPCASNHeaders
                              where tb.DocNumber == DocNumber && tb.PatnerID == PartnerID
                              orderby tb.CreatedOn descending
                              select tb).ToList();
                return result;
                //return _dbContext.BPCASNHeaders.Where(x => x.DocNumber == DocNumber && x.PatnerID == PartnerID).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/GetASNByDocAndPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/GetASNByDocAndPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCASNHeader1> GetASNByOFAndPartnerID(BPCOFItemViewFilter filter)
        {
            try
            {
                //var result = (from tb in _dbContext.BPCASNHeader1
                //              join tb1 in _dbContext.BPCASNOFMap1 on tb.ASNNumber equals tb1.ASNNumber
                //              where filter.DocNumbers.Any(x => x == tb1.DocNumber) && tb.PatnerID == filter.PatnerID
                //              orderby tb.CreatedOn descending
                //              select tb).Distinct().ToList();
                var result = (from tb in _dbContext.BPCASNHeader1
                              join tb1 in _dbContext.BPCASNOFMap1 on tb.ASNNumber equals tb1.ASNNumber
                              where filter.DocNumbers.Any(x => x == tb1.DocNumber) 
                              orderby tb.CreatedOn descending
                              select tb).Distinct().ToList();
                return result;
                //return _dbContext.BPCASNHeaders.Where(x => x.DocNumber == DocNumber && x.PatnerID == PartnerID).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/GetASNByOFAndPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/GetASNByOFAndPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public BPCASNHeader GetASNByPartnerID(string PartnerID)
        {
            try
            {
                //var result = (from tb in _dbContext.BPCASNHeaders
                //              where tb.PatnerID == PartnerID
                //              orderby tb.CreatedOn descending
                //              select tb).ToList();
                //return result;
                return _dbContext.BPCASNHeaders.FirstOrDefault(x => x.PatnerID == PartnerID);
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/GetASNByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/GetASNByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public BPCASNHeader GetASNByASN(string ASNNumber)
        {
            try
            {
                return _dbContext.BPCASNHeaders.FirstOrDefault(x => x.ASNNumber == ASNNumber);
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/GetASNByASN", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/GetASNByASN", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public BPCASNHeader1 GetASN1ByASN(string ASNNumber)
        {
            try
            {
                return _dbContext.BPCASNHeader1.FirstOrDefault(x => x.ASNNumber == ASNNumber);
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/GetASN1ByASN", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/GetASN1ByASN", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public BPCASNView2 GetASNViewByASN(string ASNNumber)
        {
            try
            {
                BPCASNView2 view2 = new BPCASNView2();
                view2.ASNItems = new List<BPCASNItemView2>();
                var result1 = (from tb in _dbContext.BPCASNHeaders
                               where tb.ASNNumber == ASNNumber
                               select tb).FirstOrDefault();
                if (result1 != null)
                {
                    view2.Vendor = result1.PatnerID;
                    view2.ASNNumber = ASNNumber;
                    view2.ASNDate = result1.ASNDate;
                    view2.DocNumber = result1.DocNumber;
                    view2.VehicleNumber = result1.VessleNumber;
                    view2.GrossWeight = result1.GrossWeight;
                    view2.NetWeight = result1.NetWeight;
                    view2.UOM = result1.NetWeightUOM;
                    view2.AWBNumber = result1.AWBNumber;
                    view2.AWBDate = result1.AWBDate;
                    view2.InvoiceNumber = result1.InvoiceNumber;
                    view2.InvoiceDate = result1.InvoiceDate;
                    view2.Plant = result1.Plant;
                    view2.GateEntryNo = result1.GateEntryNo;
                    view2.GateEntryDate = result1.GateEntryDate;
                    var result11 = (from tb in _dbContext.BPCASNItems
                                    where tb.ASNNumber == ASNNumber
                                    select tb).ToList();
                    foreach (var res in result11)
                    {
                        BPCASNItemView2 view21 = new BPCASNItemView2();
                        view21.ASNNumber = ASNNumber;
                        view21.DocNumber = view2.DocNumber;
                        view21.Item = res.Item;
                        view21.Material = res.Material;
                        view21.MaterialText = res.MaterialText;
                        view21.DeliveryDate = res.DeliveryDate;
                        view21.OrderedQty = res.OrderedQty;
                        view21.ASNQty = res.ASNQty;
                        view21.UOM = res.UOM;
                        view21.PlantCode = res.PlantCode;
                        view2.ASNItems.Add(view21);
                    }
                }
                else
                {
                    var result2 = (from tb in _dbContext.BPCASNHeader1
                                   where tb.ASNNumber == ASNNumber
                                   select tb).FirstOrDefault();
                    if (result2 != null)
                    {
                        view2.Vendor = result1.PatnerID;
                        view2.ASNNumber = ASNNumber;
                        view2.ASNDate = result2.ASNDate;
                        view2.DocNumber = result2.DocNumber;
                        view2.VehicleNumber = result2.VessleNumber;
                        view2.GrossWeight = result1.GrossWeight;
                        view2.NetWeight = result1.NetWeight;
                        view2.UOM = result1.NetWeightUOM;
                        view2.AWBNumber = result2.AWBNumber;
                        view2.AWBDate = result2.AWBDate;
                        view2.InvoiceNumber = result2.InvoiceNumber;
                        view2.InvoiceDate = result2.InvoiceDate;
                        view2.Plant = result2.Plant;
                        view2.GateEntryNo = result2.GateEntryNo;
                        view2.GateEntryDate = result2.GateEntryDate;
                        var result21 = (from tb in _dbContext.BPCASNItem1
                                        where tb.ASNNumber == ASNNumber
                                        select tb).ToList();
                        foreach (var res in result21)
                        {
                            BPCASNItemView2 view21 = new BPCASNItemView2();
                            view21.ASNNumber = ASNNumber;
                            view21.DocNumber = res.DocNumber;
                            view21.Item = res.Item;
                            view21.Material = res.Material;
                            view21.MaterialText = res.MaterialText;
                            view21.DeliveryDate = res.DeliveryDate;
                            view21.OrderedQty = res.OrderedQty;
                            view21.ASNQty = res.ASNQty;
                            view21.UOM = res.UOM;
                            view21.PlantCode = res.PlantCode;
                            view2.ASNItems.Add(view21);
                        }
                    }
                    else
                    {
                        throw new Exception("ASN not found");
                    }
                }
                return view2;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASNRepository/GetASNView1ByASN", ex);
                throw ex;
            }
        }

        public async Task CreateGateEntry(GateEntry gateEntry)
        {
            try
            {
                bool IsNonZeroOpenQty = false;
                BPCOFHeader header = new BPCOFHeader();
                List<BPCASNHeader> ASNLists = new List<BPCASNHeader>();
                List<BPCOFItem> items = new List<BPCOFItem>();
                List<BPCOFGRGI> GRNLists = new List<BPCOFGRGI>();
                bool isAllGateEntryCompleted = true;

                var ASNheader = _dbContext.BPCASNHeaders.Where(x => x.ASNNumber == gateEntry.ASNNumber).FirstOrDefault();
                if (ASNheader != null)
                {
                    ASNheader.GateEntryNo = gateEntry.GateEntryNo;
                    ASNheader.GateEntryDate = gateEntry.GateEntryDate;
                    ASNheader.GateEntryCreatedBy = gateEntry.GateEntryCreatedBy;
                    ASNheader.Status = "GateEntry Completed";

                    header = _dbContext.BPCOFHeaders.Where(x => x.DocNumber == ASNheader.DocNumber).FirstOrDefault();
                    items = _dbContext.BPCOFItems.Where(x => x.DocNumber == header.DocNumber).ToList();
                    ASNLists = _dbContext.BPCASNHeaders.Where(x => x.DocNumber == header.DocNumber).ToList();
                    GRNLists = _dbContext.BPCOFGRGIs.Where(x => x.DocNumber == header.DocNumber).ToList();

                    foreach (var item in items)
                    {
                        if (item.OpenQty.HasValue && item.OpenQty.Value > 0)
                        {
                            IsNonZeroOpenQty = true;
                        }
                    }
                    foreach (var asnl in ASNLists)
                    {
                        if (asnl.Status.ToLower() != "gateentry completed")
                        {
                            isAllGateEntryCompleted = false;
                        }
                    }
                    if (header != null)
                    {
                        if (IsNonZeroOpenQty)
                        {
                            header.Status = "PartialASN";
                        }
                        else
                        {
                            //header.Status = "PartialASN";
                            header.Status = isAllGateEntryCompleted ? GRNLists.Count > 0 ? "PartialGRN" : "DueForGRN" : "PartialGate";
                        }
                    }

                }
                else
                {
                    var ASNheader1 = _dbContext.BPCASNHeader1.Where(x => x.ASNNumber == gateEntry.ASNNumber).FirstOrDefault();
                    if (ASNheader1 != null)
                    {
                        ASNheader1.GateEntryNo = gateEntry.GateEntryNo;
                        ASNheader1.GateEntryDate = gateEntry.GateEntryDate;
                        ASNheader1.GateEntryCreatedBy = gateEntry.GateEntryCreatedBy;
                        ASNheader1.Status = "GateEntry Completed";

                        var headers = (from tb in _dbContext.BPCOFHeaders
                                       join tb1 in _dbContext.BPCASNOFMap1 on tb.DocNumber equals tb1.DocNumber
                                       where tb1.ASNNumber == gateEntry.ASNNumber
                                       select tb).ToList();

                        foreach (var head in headers)
                        {
                            header = head;
                            items = _dbContext.BPCOFItems.Where(x => x.DocNumber == header.DocNumber).ToList();
                            var ASNLists1 = (from tb in _dbContext.BPCASNHeader1
                                             join tb1 in _dbContext.BPCASNOFMap1 on tb.ASNNumber equals tb1.ASNNumber
                                             where tb1.DocNumber == head.DocNumber
                                             select tb).ToList();
                            GRNLists = _dbContext.BPCOFGRGIs.Where(x => x.DocNumber == header.DocNumber).ToList();

                            foreach (var item in items)
                            {
                                if (item.OpenQty.HasValue && item.OpenQty.Value > 0)
                                {
                                    IsNonZeroOpenQty = true;
                                }
                            }
                            foreach (var asnl in ASNLists1)
                            {
                                if (asnl.Status.ToLower() != "gateentry completed")
                                {
                                    isAllGateEntryCompleted = false;
                                }
                            }
                            if (header != null)
                            {
                                if (IsNonZeroOpenQty)
                                {
                                    header.Status = "PartialASN";
                                }
                                else
                                {
                                    //header.Status = "PartialASN";
                                    header.Status = isAllGateEntryCompleted ? GRNLists.Count > 0 ? "PartialGRN" : "DueForGRN" : "PartialGate";
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("ASN not found");
                    }
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/CreateGateEntry", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/CreateGateEntry", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BPCASNHeader GetASNByDocAndASN(string DocNumber, string ASNNumber)
        {
            try
            {
                return _dbContext.BPCASNHeaders.FirstOrDefault(x => x.DocNumber == DocNumber && x.ASNNumber == ASNNumber);
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/GetASNByDocAndASN", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/GetASNByDocAndASN", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public async Task<BPCASNHeader> CreateASN(BPCASNHeader ASN)
        //{
        //    try
        //    {
        //        ASN.IsActive = true;
        //        ASN.CreatedOn = DateTime.Now;
        //        var result = _dbContext.BPCASNHeaders.Add(ASN);
        //        await _dbContext.SaveChangesAsync();
        //        return ASN;
        //    }
        //    catch (SqlException ex){ WriteLog.WriteToFile("ASNRepository/", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile("ASNRepository/", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public async Task<BPCASNHeader> CreateASN(BPCASNView ASNView, Boolean OCRvalidate,string OCRAmount,bool shipAndInvAmount,bool ShipInvOcrAmount, bool InvoiceNumber, string InvoiceNumberOCR, string GSTValue, bool GSTcheck,string SupplierGSTValue,bool SupplierGSTCheck)
        {
            var FMResult = "Success";
            
            var ASNResult = new BPCASNHeader();
            var strategy = _dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        Random random = new Random();
                        int asnno = random.Next(100000000, 999999999);
                        BPCASNHeader ASN = new BPCASNHeader();
                        ASN.Client = ASNView.Client;
                        ASN.Company = ASNView.Company;
                        ASN.Type = ASNView.Type;
                        ASN.PatnerID = ASNView.PatnerID;
                        ASN.ASNNumber = asnno.ToString("D10");
                        ASNView.ASNNumber = ASN.ASNNumber;
                        ASN.DocNumber = ASNView.DocNumber;
                        ASN.TransportMode = ASNView.TransportMode;
                        ASN.VessleNumber = ASNView.VessleNumber;
                        ASN.CountryOfOrigin = ASNView.CountryOfOrigin;
                        ASN.AWBNumber = ASNView.AWBNumber;
                        ASN.AWBDate = ASNView.AWBDate;
                        ASN.DepartureDate = ASNView.DepartureDate;
                        ASN.ArrivalDate = ASNView.ArrivalDate;
                        ASN.ShippingAgency = ASNView.ShippingAgency;
                        ASN.GrossWeight = ASNView.GrossWeight;
                        ASN.GrossWeightUOM = ASNView.GrossWeightUOM;
                        ASN.NetWeight = ASNView.NetWeight;
                        ASN.NetWeightUOM = ASNView.NetWeightUOM;
                        ASN.VolumetricWeight = ASNView.VolumetricWeight;
                        ASN.VolumetricWeightUOM = ASNView.VolumetricWeightUOM;
                        ASN.NumberOfPacks = ASNView.NumberOfPacks;
                        ASN.InvoiceNumber = ASNView.InvoiceNumber;
                        ASN.InvoiceDate = ASNView.InvoiceDate;
                        ASN.POBasicPrice = ASNView.POBasicPrice;
                        ASN.TaxAmount = ASNView.TaxAmount;
                        ASN.InvoiceAmount = ASNView.InvoiceAmount;
                        ASN.InvoiceAmountUOM = ASNView.InvoiceAmountUOM;
                        ASN.Plant = ASNView.Plant;
                        ASN.CreatedBy = ASNView.CreatedBy;
                        ASN.ModifiedBy = ASNView.ModifiedBy;
                        ASN.IsSubmitted = ASNView.IsSubmitted;
                        ASN.ArrivalDateInterval = ASNView.ArrivalDateInterval;
                        ASN.BillOfLading = ASNView.BillOfLading;
                        ASN.TransporterName = ASNView.TransporterName;
                        ASN.AccessibleValue = ASNView.AccessibleValue;
                        ASN.ContactPerson = ASNView.ContactPerson;
                        ASN.ContactPersonNo = ASNView.ContactPersonNo;
                        ASN.Field1 = ASNView.Field1;
                        ASN.Field2 = ASNView.Field2;
                        ASN.Field3 = ASNView.Field3;
                        ASN.Field4 = ASNView.Field4;
                        ASN.Field5 = ASNView.Field5;
                        ASN.Field6 = ASNView.Field6;
                        ASN.Field7 = ASNView.Field7;
                        ASN.Field8 = ASNView.Field8;
                        ASN.Field9 = ASNView.Field9;
                        ASN.Field10 = ASNView.Field10;
                        ASN.IsActive = true;
                        ASN.CreatedOn = DateTime.Now;
                        ASN.ModifiedOn = DateTime.Now;
                        //ASN.Status = ASN.IsSubmitted ? "Submitted" : "Saved";
                        ASN.Status = ASNView.Status;
                        ASN.IsBuyerApprovalRequired = ASNView.IsBuyerApprovalRequired;
                        ASN.BuyerApprovalStatus = ASNView.BuyerApprovalStatus;
                        //ASN.ContactPersonNo = ASNView.ContactPersonNo;
                        string InvoiceRef="";
                        if (ASN.IsSubmitted)
                        {
                            ASN.ASNDate = ASNView.ASNDate;
                            if (ASN.Status == "ShipmentNotRelevant")
                            {
                                ASN.Status = "GateEntry";
                                ASN.Field2 = "ShipmentNotRelevant";
                                ASN.ASNDate = DateTime.Now;
                            }
                        }
                        else
                        {
                              InvoiceRef = UpdateASNInvAttachment(ASN, ASNView.InvAttachmentName);
                        }
                        ASN.InvDocReferenceNo = InvoiceRef;
                        var result = _dbContext.BPCASNHeaders.Add(ASN);
                        await _dbContext.SaveChangesAsync();

                        bool IsNonZeroOpenQty = await CreateASNItems(ASNView.ASNItems, ASN.ASNNumber, ASN.DocNumber, result.Entity.IsSubmitted);
                        //await CreateASNItemBatches(ASNView.ASNItemBatches, ASN.ASNNumber, ASN.DocNumber);
                        await CreateASNPacks(ASNView.ASNPacks, ASN.ASNNumber, ASN.DocNumber);
                        await CreateDocumentCenters(ASNView.DocumentCenters, ASN.ASNNumber);

                        if (ASN.IsSubmitted)
                        {
                            var BaseFileName = ASN.Company + DateTime.Now.ToString("ddMMyyyy") + "_" + ASN.ASNNumber;
                            bool ASNXmlDataResult = CreateASNXmlData(ASN, ASNView.ASNItems, BaseFileName, OCRvalidate);
                            bool InvoiceResult = UpdateAndSaveASNInvAttachment(result.Entity, ASNView.InvAttachmentName, BaseFileName);
                            
                           

                            if (ASNXmlDataResult && InvoiceResult)
                            {
                                ASN.Field1 = "";
                                var OFH = _dbContext.BPCOFHeaders.Where(x => x.DocNumber == ASN.DocNumber).FirstOrDefault();
                                if (OFH != null && OFH.DocType == "SER")
                                {
                                    WriteLog.WriteToFile($"SES creation started for ASN {ASN.ASNNumber}");
                                    FMResult = await CreateSES(ASNView, ASN.ASNNumber, ASN.DocNumber);
                                }
                                else
                                {
                                    WriteLog.WriteToFile($"IBD creation started for ASN {ASN.ASNNumber}");
                                    FMResult = await CreateIBD(ASNView, ASN.ASNNumber, ASN.DocNumber);
                                }

                                if (FMResult == "Success")
                                {
                                    ASN.Field1 = "";
                                }
                                else
                                {
                                    // ASN.ASNDate = ASNView.ASNDate;
                                    result.Entity.Status = "Saved";
                                    result.Entity.IsSubmitted = false;
                                    result.Entity.ASNDate = (DateTime?)null;
                                    result.Entity.Field1 = FMResult;
                                    await _dbContext.SaveChangesAsync();
                                    DeleteASNFiles(result.Entity.ASNNumber);
                                    //transaction.Commit();
                                    //transaction.Dispose();
                                    //throw new Exception(FMResult);
                                }
                            }
                            else
                            {
                                result.Entity.Status = "Saved";
                                result.Entity.IsSubmitted = false;
                                result.Entity.ASNDate = (DateTime?)null;
                                result.Entity.Field1 = ASNXmlDataResult ? InvoiceResult ? "Error occured for ASN FTP Files" : "Error occured while creating ASN Invoice document " : "Error occured while creating ASN Xml document";
                                await _dbContext.SaveChangesAsync();
                                DeleteASNFiles(result.Entity.ASNNumber);
                            }
                        }
                       


                        if (ASN.IsSubmitted && FMResult == "Success")
                        {
                            ASNView.ASNNumber = ASN.ASNNumber;
                            await FindPOCreatorAndSendASNNotification(ASNView);

                            if (IsNonZeroOpenQty)
                            {
                                await UpdatePOStatus(ASN, "PartialASN");
                            }
                            else
                            {
                                await UpdatePOStatus(ASN, "DueForGate");
                            }

                        }

                        if (ASN.IsBuyerApprovalRequired && string.IsNullOrEmpty(ASN.BuyerApprovalStatus))
                        {
                            await FindPOCreatorAndSendASNApprovalNotification(ASNView, OCRAmount, shipAndInvAmount, ShipInvOcrAmount,InvoiceNumber,InvoiceNumberOCR,GSTValue,GSTcheck, SupplierGSTValue, SupplierGSTCheck);
                        }

                        var aiact = _dbContext.BPCOFAIACTs.Where(x => x.PatnerID.ToLower() == ASN.PatnerID.ToLower()
                                                                   && x.DocNumber.ToLower() == ASN.DocNumber.ToLower() && x.Type.ToLower() == "action").FirstOrDefault();
                        if (aiact != null)
                        {
                            aiact.ModifiedOn = DateTime.Now;
                            aiact.Status = "DueForGate";
                            aiact.ActionText = "Gate";
                            aiact.Date = DateTime.Now;
                            aiact.Time = DateTime.Now.ToShortTimeString();
                            _dbContext.BPCOFAIACTs.Update(aiact);
                            await _dbContext.SaveChangesAsync();

                            BPCOFAIACT bPCOFAIACT = new BPCOFAIACT();
                            bPCOFAIACT.PatnerID = aiact.PatnerID;
                            bPCOFAIACT.Company = aiact.Company;
                            bPCOFAIACT.Client = aiact.Client;
                            bPCOFAIACT.DocNumber = aiact.DocNumber;
                            bPCOFAIACT.SeqNo = aiact.SeqNo;
                            bPCOFAIACT.AppID = aiact.AppID;
                            bPCOFAIACT.CreatedOn = DateTime.Now;
                            bPCOFAIACT.Type = "Notification";
                            bPCOFAIACT.Status = "ASN Done";
                            bPCOFAIACT.ActionText = "Gate";
                            bPCOFAIACT.Date = DateTime.Now;
                            bPCOFAIACT.Time = DateTime.Now.ToShortTimeString();
                            await _aIACTRepository.CreateAIACT(bPCOFAIACT);
                        }
                        transaction.Commit();
                        transaction.Dispose();
                        ASNResult = result.Entity;
                        return ASNResult;
                    }
                    catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/CreateASN", ex); throw new Exception("Something went wrong"); }
                    catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/CreateASN", ex); throw new Exception("Something went wrong"); }
                    catch (Exception ex)
                    {
                        //if (FMResult == "Success")
                        //{
                        //    transaction.Rollback();
                        //    transaction.Dispose();
                        //}
                        transaction.Rollback();
                        transaction.Dispose();
                        throw ex;
                    }

                }
            });
            return ASNResult;
        }

        public async Task<BPCASNHeader1> CreateASN1(BPCASNView1 ASNView, Boolean OCRvalidate, string InvoiceAmountOCR, bool shipAndInvAmount, bool ShipInvOcrAmount, bool InvoiceNumber, string InvoiceNumberOCR, string GSTValue, bool GSTcheck, string SupplierGSTValue, bool SupplierGSTCheck)
        {
            var FMResult = "Success";
            string InvRefNo = "";
            var ASNResult = new BPCASNHeader1();
            var strategy = _dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        Random random = new Random();
                        int asnno = random.Next(100000000, 999999999);
                        BPCASNHeader1 ASN = new BPCASNHeader1();
                        ASN.Client = ASNView.Client;
                        ASN.Company = ASNView.Company;
                        ASN.Type = ASNView.Type;
                        ASN.PatnerID = ASNView.PatnerID;
                        ASN.ASNNumber = asnno.ToString("D10");
                        ASNView.ASNNumber = ASN.ASNNumber;
                        ASN.DocNumber = ASNView.DocNumber;
                        ASN.TransportMode = ASNView.TransportMode;
                        ASN.VessleNumber = ASNView.VessleNumber;
                        ASN.CountryOfOrigin = ASNView.CountryOfOrigin;
                        ASN.AWBNumber = ASNView.AWBNumber;
                        ASN.AWBDate = ASNView.AWBDate;
                        ASN.DepartureDate = ASNView.DepartureDate;
                        ASN.ArrivalDate = ASNView.ArrivalDate;
                        ASN.ShippingAgency = ASNView.ShippingAgency;
                        ASN.GrossWeight = ASNView.GrossWeight;
                        ASN.GrossWeightUOM = ASNView.GrossWeightUOM;
                        ASN.NetWeight = ASNView.NetWeight;
                        ASN.NetWeightUOM = ASNView.NetWeightUOM;
                        ASN.VolumetricWeight = ASNView.VolumetricWeight;
                        ASN.VolumetricWeightUOM = ASNView.VolumetricWeightUOM;
                        ASN.NumberOfPacks = ASNView.NumberOfPacks;
                        ASN.InvoiceNumber = ASNView.InvoiceNumber;
                        ASN.InvoiceDate = ASNView.InvoiceDate;
                        ASN.POBasicPrice = ASNView.POBasicPrice;
                        ASN.TaxAmount = ASNView.TaxAmount;
                        ASN.InvoiceAmount = ASNView.InvoiceAmount;
                        ASN.InvoiceAmountUOM = ASNView.InvoiceAmountUOM;
                        ASN.Plant = ASNView.Plant;
                        ASN.CreatedBy = ASNView.CreatedBy;
                        ASN.ModifiedBy = ASNView.ModifiedBy;
                        ASN.IsSubmitted = ASNView.IsSubmitted;
                        ASN.ArrivalDateInterval = ASNView.ArrivalDateInterval;
                        ASN.BillOfLading = ASNView.BillOfLading;
                        ASN.TransporterName = ASNView.TransporterName;
                        ASN.AccessibleValue = ASNView.AccessibleValue;
                        ASN.ContactPerson = ASNView.ContactPerson;
                        ASN.ContactPersonNo = ASNView.ContactPersonNo;
                        ASN.Field1 = ASNView.Field1;
                        ASN.Field2 = ASNView.Field2;
                        ASN.Field3 = ASNView.Field3;
                        ASN.Field4 = ASNView.Field4;
                        ASN.Field5 = ASNView.Field5;
                        ASN.Field6 = ASNView.Field6;
                        ASN.Field7 = ASNView.Field7;
                        ASN.Field8 = ASNView.Field8;
                        ASN.Field9 = ASNView.Field9;
                        ASN.Field10 = ASNView.Field10;
                        ASN.IsActive = true;
                        ASN.CreatedOn = DateTime.Now;
                        ASN.ModifiedOn = DateTime.Now;
                        //ASN.Status = ASN.IsSubmitted ? "Submitted" : "Saved";
                        ASN.Status = ASNView.Status;
                        ASN.IsBuyerApprovalRequired = ASNView.IsBuyerApprovalRequired;
                        ASN.BuyerApprovalStatus = ASNView.BuyerApprovalStatus;

                        if (ASN.IsSubmitted)
                        {
                            ASN.ASNDate = ASNView.ASNDate;
                            if (ASN.Status == "ShipmentNotRelevant")
                            {
                                ASN.Status = "GateEntry";
                                ASN.Field2 = "ShipmentNotRelevant";
                                ASN.ASNDate = DateTime.Now;
                            }
                        }
                        else
                        {
                            InvRefNo = UpdateASNInvAttachment1(ASN, ASNView.InvAttachmentName);
                        }
                        ASN.InvDocReferenceNo = InvRefNo;
                        var result = _dbContext.BPCASNHeader1.Add(ASN);
                        await _dbContext.SaveChangesAsync();

                        if (ASNView.DocNumbers != null && ASNView.DocNumbers.Count > 0)
                        {
                            foreach (string doc in ASNView.DocNumbers)
                            {
                                BPCASNOFMap1 map = new BPCASNOFMap1();
                                map.Client = ASNView.Client;
                                map.Company = ASNView.Company;
                                map.Type = ASNView.Type;
                                map.PatnerID = ASNView.PatnerID;
                                map.ASNNumber = ASN.ASNNumber;
                                map.DocNumber = doc;
                                map.IsActive = true;
                                map.CreatedOn = DateTime.Now;
                                map.ModifiedOn = DateTime.Now;
                                var result1 = _dbContext.BPCASNOFMap1.Add(map);
                            }
                            await _dbContext.SaveChangesAsync();
                        }

                        var asnItemViews = await CreateASNItems1(ASNView.ASNItems, ASN.ASNNumber, ASN.DocNumber, result.Entity.IsSubmitted);
                        //await CreateASNItemBatches(ASNView.ASNItemBatches, ASN.ASNNumber, ASN.DocNumber);
                        await CreateASNPacks(ASNView.ASNPacks, ASN.ASNNumber, ASN.DocNumber);
                        await CreateDocumentCenters(ASNView.DocumentCenters, ASN.ASNNumber);

                        if (ASN.IsSubmitted)
                        {
                            var BaseFileName = ASN.Company + DateTime.Now.ToString("ddMMyyyy") + "_" + ASN.ASNNumber;
                            bool ASNXmlDataResult = CreateASNXmlData1(ASN, ASNView.DocNumbers, ASNView.ASNItems, BaseFileName, OCRvalidate);
                            bool InvoiceResult = UpdateAndSaveASNInvAttachment1(result.Entity, ASNView.InvAttachmentName, BaseFileName);
                            if (ASNXmlDataResult && InvoiceResult)
                            {
                                ASN.Field1 = "";
                                var OFH = _dbContext.BPCOFHeaders.Where(x => x.DocNumber == ASN.DocNumber).FirstOrDefault();
                                if (OFH != null && OFH.DocType == "SER")
                                {
                                    WriteLog.WriteToFile($"SES creation started for ASN {ASN.ASNNumber}");
                                    FMResult = await CreateSES1(ASNView, ASN.ASNNumber, ASN.DocNumber);
                                }
                                else
                                {
                                    WriteLog.WriteToFile($"IBD creation started for ASN {ASN.ASNNumber}");
                                    FMResult = await CreateIBD1(ASNView, ASN.ASNNumber, ASN.DocNumber);
                                }

                                if (FMResult == "Success")
                                {
                                    ASN.Field1 = "";
                                }
                                else
                                {
                                    // ASN.ASNDate = ASNView.ASNDate;
                                    result.Entity.Status = "Saved";
                                    result.Entity.IsSubmitted = false;
                                    result.Entity.ASNDate = (DateTime?)null;
                                    result.Entity.Field1 = FMResult;
                                    await _dbContext.SaveChangesAsync();
                                    DeleteASNFiles(result.Entity.ASNNumber);
                                    //transaction.Commit();
                                    //transaction.Dispose();
                                    //throw new Exception(FMResult);
                                }
                            }
                            else
                            {
                                result.Entity.Status = "Saved";
                                result.Entity.IsSubmitted = false;
                                result.Entity.ASNDate = (DateTime?)null;
                                result.Entity.Field1 = ASNXmlDataResult ? InvoiceResult ? "Error occured for ASN FTP Files" : "Error occured while creating ASN Invoice document " : "Error occured while creating ASN Xml document";
                                await _dbContext.SaveChangesAsync();
                                DeleteASNFiles(result.Entity.ASNNumber);
                            }
                        }
                       

                        if (ASN.IsSubmitted && FMResult == "Success")
                        {
                            ASNView.ASNNumber = ASN.ASNNumber;
                            await FindPOCreatorAndSendASNNotification1(ASNView, InvoiceAmountOCR, shipAndInvAmount,ShipInvOcrAmount,InvoiceNumber,InvoiceNumberOCR,GSTValue,GSTcheck);
                            var Groups = asnItemViews.GroupBy(x => new { x.Client, x.Company, x.Type, x.PatnerID, x.DocNumber });
                            var DocNumber = "";
                            foreach (var g in Groups)
                            {
                                bool IsNonZeroOpenQty1 = false;
                                foreach (var y in g)
                                {
                                    DocNumber = y.DocNumber;
                                    if (!IsNonZeroOpenQty1)
                                    {
                                        IsNonZeroOpenQty1 = y.ItemOpenQty.HasValue && y.ItemOpenQty.Value > 0;
                                    }
                                }
                                if (IsNonZeroOpenQty1)
                                {
                                    await UpdatePOStatus1(ASN, DocNumber, "PartialASN");
                                }
                                else
                                {
                                    await UpdatePOStatus1(ASN, DocNumber, "DueForGate");
                                }
                            }


                        }

                        if (ASN.IsBuyerApprovalRequired && string.IsNullOrEmpty(ASN.BuyerApprovalStatus))
                        {
                            await FindPOCreatorAndSendASNApprovalNotification1(ASNView, InvoiceAmountOCR, shipAndInvAmount, ShipInvOcrAmount, InvoiceNumber,  InvoiceNumberOCR, GSTValue,GSTcheck, SupplierGSTValue,  SupplierGSTCheck);
                        }

                        //var aiact = _dbContext.BPCOFAIACTs.Where(x => x.PatnerID.ToLower() == ASN.PatnerID.ToLower()
                        //                                           && x.DocNumber.ToLower() == ASN.DocNumber.ToLower() && x.Type.ToLower() == "action").FirstOrDefault();
                        //if (aiact != null)
                        //{
                        //    aiact.ModifiedOn = DateTime.Now;
                        //    aiact.Status = "DueForGate";
                        //    aiact.ActionText = "Gate";
                        //    aiact.Date = DateTime.Now;
                        //    aiact.Time = DateTime.Now.ToShortTimeString();
                        //    _dbContext.BPCOFAIACTs.Update(aiact);
                        //    await _dbContext.SaveChangesAsync();

                        //    BPCOFAIACT bPCOFAIACT = new BPCOFAIACT();
                        //    bPCOFAIACT.PatnerID = aiact.PatnerID;
                        //    bPCOFAIACT.Company = aiact.Company;
                        //    bPCOFAIACT.Client = aiact.Client;
                        //    bPCOFAIACT.DocNumber = aiact.DocNumber;
                        //    bPCOFAIACT.SeqNo = aiact.SeqNo;
                        //    bPCOFAIACT.AppID = aiact.AppID;
                        //    bPCOFAIACT.CreatedOn = DateTime.Now;
                        //    bPCOFAIACT.Type = "Notification";
                        //    bPCOFAIACT.Status = "ASN Done";
                        //    bPCOFAIACT.ActionText = "Gate";
                        //    bPCOFAIACT.Date = DateTime.Now;
                        //    bPCOFAIACT.Time = DateTime.Now.ToShortTimeString();
                        //    await _aIACTRepository.CreateAIACT(bPCOFAIACT);
                        //}
                        transaction.Commit();
                        transaction.Dispose();
                        ASNResult = result.Entity;
                        return ASNResult;
                    }
                    catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/CreateASN", ex); throw new Exception("Something went wrong"); }
                    catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/CreateASN", ex); throw new Exception("Something went wrong"); }
                    catch (Exception ex)
                    {
                        //if (FMResult == "Success")
                        //{
                        //    transaction.Rollback();
                        //    transaction.Dispose();
                        //}
                        transaction.Rollback();
                        transaction.Dispose();
                        throw ex;
                    }

                }
            });
            return ASNResult;
        }

        public bool CreateASNXmlData(BPCASNHeader header, List<BPCASNItemView> ASNitems, string BaseFileName, bool OCRvalidate)
        {
            try
            {
                string ocr = "";
                if (OCRvalidate)
                {
                    ocr = "YES";
                }
                else
                {
                    ocr = "NO";
                }

                //WriteLog.WriteErrorLog("Enter the GetPRWithVendorDetails method");
                CreateOutboxTempFolder();
                //Random r = new Random();
                //int num = r.Next(1, 9999999);
                string writerFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Outbox");
                //var FileName = header.ASNNumber + ".xml";
                //var FileName = header.Company + DateTime.Now.ToString("ddMMyyyy") + "_" + header.ASNNumber + ".xml";
                var FileName = BaseFileName + ".xml";
                string writerPath = Path.Combine(writerFolder, FileName);
                var OFHeader = _dbContext.BPCOFHeaders.FirstOrDefault(x => x.DocNumber == header.DocNumber);
                using (var sw = new StreamWriter(writerPath))
                {
                    XmlWriter writer = XmlWriter.Create(sw);

                    WriteLog.WriteToFile("ASN Xml creation started");
                    writer.WriteStartDocument();
                    writer.WriteStartElement("ASNDATA");
                    writer.WriteElementString("INVOICEQUALITY", ocr);
                    writer.WriteElementString("VID", header.PatnerID);
                    writer.WriteElementString("PO", header.DocNumber);
                    writer.WriteElementString("ASN", header.ASNNumber);
                    writer.WriteElementString("DOCTYPE", OFHeader != null ? OFHeader.DocType : "");
                    writer.WriteElementString("MOT", header.TransportMode);
                    writer.WriteElementString("DEPDATE", header.DepartureDate.HasValue ? header.DepartureDate.Value.ToString("dd/MM/yyyy") : "");
                    writer.WriteElementString("ARRDATE", header.ArrivalDate.HasValue ? header.ArrivalDate.Value.ToString("dd/MM/yyyy") : "");
                    writer.WriteElementString("SHIPPINGAGENCY", header.ShippingAgency);
                    writer.WriteElementString("TRUCKNNO", header.VessleNumber);
                    writer.WriteElementString("NETWEIGHT", header.NetWeight.HasValue ? header.NetWeight.Value.ToString() : "");
                    writer.WriteElementString("GROSSWEIGHT", header.GrossWeight.HasValue ? header.GrossWeight.Value.ToString() : "");
                    writer.WriteElementString("COUNTRYOFORGIN", header.CountryOfOrigin);
                    writer.WriteElementString("AWBNO", header.AWBNumber);
                    writer.WriteElementString("ARRDATE", header.ArrivalDate.HasValue ? header.ArrivalDate.Value.ToString("dd/MM/yyyy") : "");
                    writer.WriteElementString("BOL", header.BillOfLading);
                    writer.WriteElementString("TRANPORTER", header.TransporterName);
                    writer.WriteElementString("CONPERSON", header.ContactPerson);
                    writer.WriteElementString("CONNO", header.ContactPersonNo);

                    writer.WriteStartElement("SHIPITEMS");

                    foreach (var item in ASNitems)
                    {
                        List<BPCASNItemBatch> asnItemBatch = _dbContext.BPCASNItemBatches.Where(x => x.ASNNumber == header.ASNNumber && x.Item == item.Item).ToList();
                        if (asnItemBatch.Count > 0)
                        {
                            foreach (var patchItem in asnItemBatch)
                            {
                                writer.WriteStartElement("ITEM");
                                writer.WriteElementString("ITEMNUMBER", item.Item);
                                writer.WriteElementString("METERIAL", item.Material);
                                //writer.WriteElementString("HEADER_ID", item.HEADER_ID);
                                writer.WriteElementString("METERIALDESC", item.MaterialText);
                                writer.WriteElementString("DELDATE", item.DeliveryDate.HasValue ? item.DeliveryDate.Value.ToString("dd/MM/yyyy") : "");
                                writer.WriteElementString("ORDERQTY", item.OrderedQty.ToString());
                                writer.WriteElementString("GRQTY", item.CompletedQty.ToString());
                                writer.WriteElementString("PIPELINEQTY", patchItem.Qty.ToString());
                                writer.WriteElementString("OPENQTY", item.OpenQty.ToString());
                                writer.WriteElementString("UOM", item.UOM);
                                writer.WriteElementString("BATCH", patchItem.Batch);
                                writer.WriteElementString("MFDATE", patchItem.ManufactureDate.HasValue ? patchItem.ManufactureDate.Value.ToString("dd/MM/yyyy") : "");
                                writer.WriteElementString("EXPDATE", patchItem.ExpiryDate.HasValue ? patchItem.ExpiryDate.Value.ToString("dd/MM/yyyy") : "");
                                writer.WriteEndElement();
                            }
                        }
                        else
                        {
                            writer.WriteStartElement("ITEM");
                            writer.WriteElementString("ITEMNUMBER", item.Item);
                            writer.WriteElementString("METERIAL", item.Material);
                            //writer.WriteElementString("HEADER_ID", item.HEADER_ID);
                            writer.WriteElementString("METERIALDESC", item.MaterialText);
                            writer.WriteElementString("DELDATE", item.DeliveryDate.HasValue ? item.DeliveryDate.Value.ToString("dd/MM/yyyy") : "");
                            writer.WriteElementString("ORDERQTY", item.OrderedQty.ToString());
                            writer.WriteElementString("GRQTY", item.CompletedQty.ToString());
                            writer.WriteElementString("PIPELINEQTY", item.ASNQty.ToString());
                            writer.WriteElementString("OPENQTY", item.OpenQty.ToString());
                            writer.WriteElementString("UOM", item.UOM);
                            writer.WriteElementString("BATCH", "");
                            writer.WriteElementString("MFDATE", "");
                            writer.WriteElementString("EXPDATE", "");
                            writer.WriteEndElement();
                        }
                    }

                    writer.WriteEndElement();

                    writer.WriteStartElement("INVLIST");
                    writer.WriteStartElement("LIST");
                    writer.WriteElementString("INVNO", header.InvoiceNumber);
                    writer.WriteElementString("DATE", header.InvoiceDate.HasValue ? header.InvoiceDate.Value.ToString("dd/MM/yyyy") : "");
                    writer.WriteElementString("VALUE", header.InvoiceAmount.HasValue ? header.InvoiceAmount.Value.ToString() : "");
                    writer.WriteElementString("CUR", header.InvoiceAmountUOM);
                    writer.WriteEndElement();
                    writer.WriteEndElement();

                    writer.WriteEndElement();

                    writer.Flush();
                    writer.Close();
                    WriteLog.WriteToFile("ASN Xml creation ended");
                    return true;
                }
            }
            //catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/CreateASNXmlData", ex); throw new Exception("Something went wrong"); }
            //catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/CreateASNXmlData", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASNRepository/CreateASNXmlData/Exception : - " + ex.Message);
                return false;
            }

        }

        public bool CreateASNXmlData1(BPCASNHeader1 header, List<string> DocNumbers, List<BPCASNItemView1> ASNitems, string BaseFileName,bool OCRvalidate)
        {
            try
            {
                string ocr = "";
                if (OCRvalidate)
                {
                    ocr = "YES";
                }
                else
                {
                    ocr = "NO";
                }
                //WriteLog.WriteErrorLog("Enter the GetPRWithVendorDetails method");
                CreateOutboxTempFolder();
                //Random r = new Random();
                //int num = r.Next(1, 9999999);
                string writerFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Outbox");
                //var FileName = header.ASNNumber + ".xml";
                //var FileName = header.Company + DateTime.Now.ToString("ddMMyyyy") + "_" + header.ASNNumber + ".xml";
                var FileName = BaseFileName + ".xml";
                string writerPath = Path.Combine(writerFolder, FileName);
                var DocNumber = "";
                if (DocNumbers.Count > 0)
                {
                    DocNumber = DocNumbers[0];
                }
                var OFHeader = _dbContext.BPCOFHeaders.FirstOrDefault(x => x.DocNumber == (string.IsNullOrEmpty(header.DocNumber) ? DocNumber : header.DocNumber));
                using (var sw = new StreamWriter(writerPath))
                {
                    XmlWriter writer = XmlWriter.Create(sw);

                    WriteLog.WriteToFile("ASN Xml creation started");
                    writer.WriteStartDocument();
                    writer.WriteStartElement("ASNDATA");
                    writer.WriteElementString("INVOICEQUALITY", ocr);
                    writer.WriteElementString("VID", header.PatnerID);
                    writer.WriteElementString("PO", (DocNumbers != null && DocNumbers.Count == 1) ? DocNumber : header.DocNumber);
                    writer.WriteElementString("ASN", header.ASNNumber);
                    writer.WriteElementString("DOCTYPE", OFHeader != null ? OFHeader.DocType : "");
                    writer.WriteElementString("MOT", header.TransportMode);
                    writer.WriteElementString("DEPDATE", header.DepartureDate.HasValue ? header.DepartureDate.Value.ToString("dd/MM/yyyy") : "");
                    writer.WriteElementString("ARRDATE", header.ArrivalDate.HasValue ? header.ArrivalDate.Value.ToString("dd/MM/yyyy") : "");
                    writer.WriteElementString("SHIPPINGAGENCY", header.ShippingAgency);
                    writer.WriteElementString("TRUCKNNO", header.VessleNumber);
                    writer.WriteElementString("NETWEIGHT", header.NetWeight.HasValue ? header.NetWeight.Value.ToString() : "");
                    writer.WriteElementString("GROSSWEIGHT", header.GrossWeight.HasValue ? header.GrossWeight.Value.ToString() : "");
                    writer.WriteElementString("COUNTRYOFORGIN", header.CountryOfOrigin);
                    writer.WriteElementString("AWBNO", header.AWBNumber);
                    writer.WriteElementString("ARRDATE", header.ArrivalDate.HasValue ? header.ArrivalDate.Value.ToString("dd/MM/yyyy") : "");
                    writer.WriteElementString("BOL", header.BillOfLading);
                    writer.WriteElementString("TRANPORTER", header.TransporterName);
                    writer.WriteElementString("CONPERSON", header.ContactPerson);
                    writer.WriteElementString("CONNO", header.ContactPersonNo);

                    writer.WriteStartElement("SHIPITEMS");

                    foreach (var item in ASNitems)
                    {
                        List<BPCASNItemBatch1> asnItemBatch = _dbContext.BPCASNItemBatch1.Where(x => x.DocNumber == item.DocNumber && x.ASNNumber == header.ASNNumber && x.Item == item.Item).ToList();
                        if (asnItemBatch.Count > 0)
                        {
                            foreach (var patchItem in asnItemBatch)
                            {
                                writer.WriteStartElement("ITEM");
                                writer.WriteElementString("ITEMNUMBER", item.Item);
                                writer.WriteElementString("METERIAL", item.Material);
                                //writer.WriteElementString("HEADER_ID", item.HEADER_ID);
                                writer.WriteElementString("METERIALDESC", item.MaterialText);
                                writer.WriteElementString("DELDATE", item.DeliveryDate.HasValue ? item.DeliveryDate.Value.ToString("dd/MM/yyyy") : "");
                                writer.WriteElementString("ORDERQTY", item.OrderedQty.ToString());
                                writer.WriteElementString("GRQTY", item.CompletedQty.ToString());
                                writer.WriteElementString("PIPELINEQTY", patchItem.Qty.ToString());
                                writer.WriteElementString("OPENQTY", item.OpenQty.ToString());
                                writer.WriteElementString("UOM", item.UOM);
                                writer.WriteElementString("BATCH", patchItem.Batch);
                                writer.WriteElementString("MFDATE", patchItem.ManufactureDate.HasValue ? patchItem.ManufactureDate.Value.ToString("dd/MM/yyyy") : "");
                                writer.WriteElementString("EXPDATE", patchItem.ExpiryDate.HasValue ? patchItem.ExpiryDate.Value.ToString("dd/MM/yyyy") : "");
                                writer.WriteEndElement();
                            }
                        }
                        else
                        {
                            writer.WriteStartElement("ITEM");
                            writer.WriteElementString("ITEMNUMBER", item.Item);
                            writer.WriteElementString("METERIAL", item.Material);
                            //writer.WriteElementString("HEADER_ID", item.HEADER_ID);
                            writer.WriteElementString("METERIALDESC", item.MaterialText);
                            writer.WriteElementString("DELDATE", item.DeliveryDate.HasValue ? item.DeliveryDate.Value.ToString("dd/MM/yyyy") : "");
                            writer.WriteElementString("ORDERQTY", item.OrderedQty.ToString());
                            writer.WriteElementString("GRQTY", item.CompletedQty.ToString());
                            writer.WriteElementString("PIPELINEQTY", item.ASNQty.ToString());
                            writer.WriteElementString("OPENQTY", item.OpenQty.ToString());
                            writer.WriteElementString("UOM", item.UOM);
                            writer.WriteElementString("BATCH", "");
                            writer.WriteElementString("MFDATE", "");
                            writer.WriteElementString("EXPDATE", "");
                            writer.WriteEndElement();
                        }
                    }

                    writer.WriteEndElement();

                    writer.WriteStartElement("INVLIST");
                    writer.WriteStartElement("LIST");
                    writer.WriteElementString("INVNO", header.InvoiceNumber);
                    writer.WriteElementString("DATE", header.InvoiceDate.HasValue ? header.InvoiceDate.Value.ToString("dd/MM/yyyy") : "");
                    writer.WriteElementString("VALUE", header.InvoiceAmount.HasValue ? header.InvoiceAmount.Value.ToString() : "");
                    writer.WriteElementString("CUR", header.InvoiceAmountUOM);
                    writer.WriteEndElement();
                    writer.WriteEndElement();

                    writer.WriteEndElement();

                    writer.Flush();
                    writer.Close();
                    WriteLog.WriteToFile("ASN Xml creation ended");
                    return true;
                }
            }
            //catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/CreateASNXmlData", ex); throw new Exception("Something went wrong"); }
            //catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/CreateASNXmlData", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASNRepository/CreateASNXmlData/Exception : - " + ex.Message);
                return false;
            }

        }

        public string UpdateASNInvAttachment(BPCASNHeader header, string AttachmentName)
        {
            try
            {
                /* _dbContext.BPCAttachments.Where(x => x.AttachmentID.ToString() == header.InvDocReferenceNo).FirstOrDefault();*/
                BPCAttachment att = (from tb in _dbContext.BPCAttachments
                                     where tb.Client == header.Client && tb.Company == header.Company && tb.Type == header.Type &&
                                     tb.PatnerID == header.PatnerID && tb.ReferenceNo == header.DocNumber && tb.AttachmentName == AttachmentName
                                     select tb).FirstOrDefault();
                if (att != null)
                {
                    WriteLog.WriteToFile($"UpdateASNInvAttachment - Invoice attachment found for ASN {header.ASNNumber}");
                    header.InvDocReferenceNo = att.AttachmentID.ToString();
                }
                else
                {
                    WriteLog.WriteToFile($"UpdateASNInvAttachment - Invoice attachment not found for ASN {header.ASNNumber}");
                    return header.InvDocReferenceNo;
                }
                return header.InvDocReferenceNo;
            }
            //catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/SaveASNInvAttachment", ex); throw new Exception("Something went wrong"); }
            //catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/SaveASNInvAttachment", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASNRepository/UpdateASNInvAttachment/Exception : - " + ex.Message);
                return header.InvDocReferenceNo;
            }

        }
        public string UpdateASNInvAttachment1(BPCASNHeader1 header, string AttachmentName)
        {
            try
            {
                /* _dbContext.BPCAttachments.Where(x => x.AttachmentID.ToString() == header.InvDocReferenceNo).FirstOrDefault();*/
                BPCAttachment att = (from tb in _dbContext.BPCAttachments
                                     where tb.Client == header.Client && tb.Company == header.Company && tb.Type == header.Type &&
                                     tb.PatnerID == header.PatnerID && tb.ReferenceNo == AttachmentName && tb.AttachmentName == AttachmentName
                                     select tb).FirstOrDefault();
                if (att != null)
                {
                    WriteLog.WriteToFile($"UpdateASNInvAttachment - Invoice attachment found for ASN {header.ASNNumber}");
                    header.InvDocReferenceNo = att.AttachmentID.ToString();
                }
                else
                {
                    WriteLog.WriteToFile($"UpdateASNInvAttachment - Invoice attachment not found for ASN {header.ASNNumber}");
                    return header.InvDocReferenceNo;
                }
                return header.InvDocReferenceNo;
            }
            //catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/SaveASNInvAttachment", ex); throw new Exception("Something went wrong"); }
            //catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/SaveASNInvAttachment", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASNRepository/UpdateASNInvAttachment/Exception : - " + ex.Message);
                return header.InvDocReferenceNo;
            }

        }

        public bool SaveASNDocAttachment(BPCASNHeader header, string AttachmentName, string BaseFileName, int pagecount)
        {
            try
            {
                string writerFolder = "";
                /* _dbContext.BPCAttachments.Where(x => x.AttachmentID.ToString() == header.InvDocReferenceNo).FirstOrDefault();*/
                BPCAttachment att = (from tb in _dbContext.BPCAttachments
                                     where tb.Client == header.Client && tb.Company == header.Company && tb.Type == header.Type &&
                                     tb.PatnerID == header.PatnerID && tb.ReferenceNo == header.DocNumber && tb.AttachmentName == AttachmentName
                                     select tb).FirstOrDefault();
               // CreateOutboxTempFolder();

                if (att != null)
                {
                    WriteLog.WriteToFile($"Document attachment found for ASN {header.ASNNumber}");
                    header.InvDocReferenceNo = att.AttachmentID.ToString();
                   // CreateOutboxTempFolder();
                    writerFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Outbox");
                    //var FileName = $"{ASNNumber}_{InvoiceNumber}_{AttachmentName}";
                    //var FileName = header.Company + DateTime.Now.ToString("ddMMyyyyhh") + "_" + header.ASNNumber + Path.GetExtension(AttachmentName);
                    //var FileName = header.Company + DateTime.Now.ToString("ddMMyyyy") + "_" + header.ASNNumber + Path.GetExtension(AttachmentName);
                    var FileName = BaseFileName + "_" + pagecount  + Path.GetExtension(AttachmentName);
                    string writerPath = Path.Combine(writerFolder, FileName);
                    File.WriteAllBytes(writerPath, att.AttachmentFile);
                }
                else
                {
                    WriteLog.WriteToFile($"Document attachment not found for ASN {header.ASNNumber}");
                    return false;
                }
                return true;
            }
            //catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/SaveASNInvAttachment", ex); throw new Exception("Something went wrong"); }
            //catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/SaveASNInvAttachment", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASNRepository/SaveASNDocAttachment/Exception : - " + ex.Message);
                return false;
            }

        }


        public bool UpdateAndSaveASNInvAttachment(BPCASNHeader header, string AttachmentName, string BaseFileName)
        {
            try
            {
                string writerFolder = "";
                /* _dbContext.BPCAttachments.Where(x => x.AttachmentID.ToString() == header.InvDocReferenceNo).FirstOrDefault();*/
                BPCAttachment att = (from tb in _dbContext.BPCAttachments
                                     where tb.Client == header.Client && tb.Company == header.Company && tb.Type == header.Type &&
                                     tb.PatnerID == header.PatnerID && tb.ReferenceNo == header.DocNumber && tb.AttachmentName == AttachmentName
                                     select tb).FirstOrDefault();
                CreateOutboxTempFolder();

                if (att != null)
                {
                    WriteLog.WriteToFile($"Invoice attachment found for ASN {header.ASNNumber}");
                    header.InvDocReferenceNo = att.AttachmentID.ToString();
                    CreateOutboxTempFolder();
                    writerFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Outbox");
                    //var FileName = $"{ASNNumber}_{InvoiceNumber}_{AttachmentName}";
                    //var FileName = header.Company + DateTime.Now.ToString("ddMMyyyyhh") + "_" + header.ASNNumber + Path.GetExtension(AttachmentName);
                    //var FileName = header.Company + DateTime.Now.ToString("ddMMyyyy") + "_" + header.ASNNumber + Path.GetExtension(AttachmentName);
                    var FileName = BaseFileName + Path.GetExtension(AttachmentName);
                    string writerPath = Path.Combine(writerFolder, FileName);
                    File.WriteAllBytes(writerPath, att.AttachmentFile);
                }
                else
                {
                    WriteLog.WriteToFile($"Invoice attachment not found for ASN {header.ASNNumber}");
                    return false;
                }
                return true;
            }
            //catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/SaveASNInvAttachment", ex); throw new Exception("Something went wrong"); }
            //catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/SaveASNInvAttachment", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASNRepository/UpdateAndSaveASNInvAttachment/Exception : - " + ex.Message);
                return false;
            }

        }


        public bool UpdateAndSaveASNDocAttachment(string AttachmentName, string RefferenceNo, string BaseFileName, int filecount, string ASNNumber)
        {
            /* _dbContext.BPCAttachments.Where(x => x.AttachmentID.ToString() == header.InvDocReferenceNo).FirstOrDefault();*/
            BPCAttachment att = (from tb in _dbContext.BPCAttachments
                                 where tb.AttachmentID.ToString() == RefferenceNo
                                 select tb).FirstOrDefault();
            if (att != null)
            {

                WriteLog.WriteToFile($"Document Center attachment found for ASN {ASNNumber}");
                //header.InvDocReferenceNo = att.AttachmentID.ToString();
                //CreateOutboxTempFolder();
                string writerFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Outbox");
                //var FileName = $"{ASNNumber}_{InvoiceNumber}_{AttachmentName}";
                //var FileName = header.Company + DateTime.Now.ToString("ddMMyyyyhh") + "_" + header.ASNNumber + Path.GetExtension(AttachmentName);
                //var FileName = header.Company + DateTime.Now.ToString("ddMMyyyy") + "_" + header.ASNNumber + Path.GetExtension(AttachmentName);
                var FileName = BaseFileName + "_" + filecount + Path.GetExtension(AttachmentName);
                string writerPath = Path.Combine(writerFolder, FileName);
                File.WriteAllBytes(writerPath, att.AttachmentFile);

                return true;
            }
            else
            {
                WriteLog.WriteToFile($"Document Center attachment not found for ASN {ASNNumber}");
                return false;
            }
            return true;
        }

        public bool UpdateAndSaveASNInvAttachment1(BPCASNHeader1 header, string AttachmentName, string BaseFileName)
        {
            try
            {
                /* _dbContext.BPCAttachments.Where(x => x.AttachmentID.ToString() == header.InvDocReferenceNo).FirstOrDefault();*/
                BPCAttachment att = (from tb in _dbContext.BPCAttachments
                                     where tb.Client == header.Client && tb.Company == header.Company && tb.Type == header.Type &&
                                     tb.PatnerID == header.PatnerID && tb.ReferenceNo == AttachmentName && tb.AttachmentName == AttachmentName
                                     select tb).FirstOrDefault();
                if (att != null)
                {
                    WriteLog.WriteToFile($"Invoice attachment found for ASN {header.ASNNumber}");
                    header.InvDocReferenceNo = att.AttachmentID.ToString();
                    CreateOutboxTempFolder();
                    string writerFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Outbox");
                    //var FileName = $"{ASNNumber}_{InvoiceNumber}_{AttachmentName}";
                    //var FileName = header.Company + DateTime.Now.ToString("ddMMyyyyhh") + "_" + header.ASNNumber + Path.GetExtension(AttachmentName);
                    //var FileName = header.Company + DateTime.Now.ToString("ddMMyyyy") + "_" + header.ASNNumber + Path.GetExtension(AttachmentName);
                    var FileName = BaseFileName + Path.GetExtension(AttachmentName);
                    string writerPath = Path.Combine(writerFolder, FileName);
                    File.WriteAllBytes(writerPath, att.AttachmentFile);
                }
                else
                {
                    WriteLog.WriteToFile($"Invoice attachment not found for ASN {header.ASNNumber}");
                    return false;
                }
                return true;
            }
            //catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/SaveASNInvAttachment", ex); throw new Exception("Something went wrong"); }
            //catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/SaveASNInvAttachment", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASNRepository/UpdateAndSaveASNInvAttachment1/Exception : - " + ex.Message);
                return false;
            }

        }

        public bool SaveASNInvAttachment(BPCASNHeader header, string AttachmentName, byte[] AttachmentContent)
        {
            try
            {
                //WriteLog.WriteErrorLog("Enter the GetPRWithVendorDetails method");
                CreateOutboxTempFolder();
                string writerFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Outbox");
                //var FileName = $"{ASNNumber}_{InvoiceNumber}_{AttachmentName}";
                //var FileName = header.Company + DateTime.Now.ToString("ddMMyyyyhh") + "_" + header.ASNNumber + Path.GetExtension(AttachmentName);
                var FileName = header.Company + DateTime.Now.ToString("ddMMyyyy") + "_" + header.ASNNumber + Path.GetExtension(AttachmentName);
                string writerPath = Path.Combine(writerFolder, FileName);
                File.WriteAllBytes(writerPath, AttachmentContent);
                return true;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/SaveASNInvAttachment", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/SaveASNInvAttachment", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASNRepository/SaveASNInvAttachment/Exception : - " + ex.Message);
                return false;
            }

        }

        public bool DeleteASNFiles(string ASNNumber)
        {
            try
            {
                WriteLog.WriteToFile($"Delete ASN Files started for ASN {ASNNumber}");
                //CreateOutboxTempFolder();
                string writerFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Outbox");

                if (Directory.GetFiles(writerFolder).Length > 0) //if file found in folder
                {
                    string[] txtList = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Outbox"));
                    foreach (string f in txtList)
                    {
                        FileInfo fi = new FileInfo(f);
                        var fileName = Path.GetFileNameWithoutExtension(f);
                        var splitedFileNames = fileName.Split('_');
                        if (splitedFileNames.Length > 1)
                        {
                            if (splitedFileNames[1] == ASNNumber)
                            {
                                System.GC.Collect();
                                System.GC.WaitForPendingFinalizers();
                                fi.Delete();
                                //File.Delete(f);
                            }
                        }


                    }
                }
                return true;
            }
            catch (SqlException ex)
            {
                WriteLog.WriteToFile("ASNRepository/DeleteASNFiles", ex);
                return false;
            }
            catch (InvalidOperationException ex)
            {
                WriteLog.WriteToFile("ASNRepository/DeleteASNFiles", ex);
                return false;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASNRepository/SaveASNInvAttachment/Exception : - " + ex.Message);
                return false;
            }
        }

        public bool SendASNAttachmentsToFTP()
        {
            try
            {
                bool status = false;
                //var docCenters = (from tb in _dbContext.BPCDocumentCenters
                //                  join tb1 in _dbContext.BPCAttachments on tb.AttachmentReferenceNo equals tb1.AttachmentID.ToString()
                //                  where tb.ASNNumber == ASNNumber
                //                  select tb1);
                ////var documentrefno = _dbContext.BPCDocumentCenters.Where(x => x.ASNNumber == ASNNumber).ToList();


                try
                {

                    string writerFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Outbox");
                    IConfiguration FTPDetailsConfig = _configuration.GetSection("FTPDetails");
                    string FTPOutbox = FTPDetailsConfig.GetValue<string>("Outbox");
                    string FTPUsername = FTPDetailsConfig.GetValue<string>("Username");
                    string FTPPassword = FTPDetailsConfig.GetValue<string>("Password");

                    using (WebClient client = new WebClient())
                    {
                        if (Directory.GetFiles(writerFolder).Length > 0) //if file found in folder
                        {
                            DirectoryInfo dir = new DirectoryInfo(writerFolder);
                            FileInfo[] files = dir.GetFiles();
                            foreach (var file in files)
                            {
                                if (file.Length > 0)
                                {
                                    client.Credentials = new NetworkCredential(FTPUsername, FTPPassword);
                                    byte[] responseArray = client.UploadFile(FTPOutbox + file.Name, file.FullName);
                                    //WriteLog.WriteToFile("SendASNAttachmentsToFTP - File uploaded to Output folder");
                                    status = true;
                                    WriteLog.WriteToFile($"SendASNAttachmentsToFTP - File {file.Name} was successfully uploaded to FTP {FTPOutbox}");
                                    System.IO.File.Delete(file.FullName);
                                    //return status;
                                }
                                else
                                {
                                    status = false;
                                    WriteLog.WriteToFile($"SendASNAttachmentsToFTP - File {file.FullName} has no contents");
                                }
                            }
                        }
                    }
                    return status;
                }

                catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/SendASNAttachmentsToFTP", ex); throw new Exception("Something went wrong"); }
                catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/SendASNAttachmentsToFTP", ex); throw new Exception("Something went wrong"); }
                catch (Exception ex)
                {
                    WriteLog.WriteToFile("ASNRepository/SendASNAttachmentsToFTP/Exception", ex);
                    return false;
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/SendASNAttachmentsToFTP", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/SendASNAttachmentsToFTP", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASNRepository/SendASNAttachmentsToFTP/Exception", ex);
                return false;
            }
        }

        //public async Task<BPCASNHeader> UpdateASN(BPCASNHeader ASN)
        //{
        //    try
        //    {
        //        var entity = _dbContext.Set<BPCASNHeader>().FirstOrDefault(x => x.TransID == ASN.TransID);
        //        if (entity == null)
        //        {
        //            return entity;
        //        }
        //        //_dbContext.Entry(ASN).State = EntityState.Modified;
        //        entity.Name = ASN.Name;
        //        entity.Role = ASN.Role;
        //        entity.LegalName = ASN.LegalName;
        //        entity.AddressLine1 = ASN.AddressLine1;
        //        entity.AddressLine2 = ASN.AddressLine2;
        //        entity.City = ASN.City;
        //        entity.State = ASN.State;
        //        entity.Country = ASN.Country;
        //        entity.PinCode = ASN.PinCode;
        //        entity.Type = ASN.Type;
        //        entity.Phone1 = ASN.Phone1;
        //        entity.Phone2 = ASN.Phone2;
        //        entity.Email1 = ASN.Email1;
        //        entity.Email2 = ASN.Email2;
        //        entity.VendorCode = ASN.VendorCode;
        //        entity.ParentVendor = ASN.ParentVendor;
        //        entity.Status = ASN.Status;
        //        entity.ModifiedBy = ASN.ModifiedBy;
        //        entity.ModifiedOn = DateTime.Now;
        //        await _dbContext.SaveChangesAsync();
        //        return ASN;
        //    }
        //    catch (SqlException ex){ WriteLog.WriteToFile("ASNRepository/UpdateASN", ex); throw new Exception("Something went wrong");}
        //    catch(InvalidOperationException ex){WriteLog.WriteToFile("ASNRepository/UpdateASN", ex); throw new Exception("Something went wrong"); }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public async Task<string> CreateIBD(BPCASNView ASNView, string ASNNumber, string DocNumber)
        {
            try
            {
                var ASNItems = ASNView.ASNItems;
                ASNView.ASNItems = (from tb in ASNView.ASNItems where tb.ASNQty > 0 select tb).ToList();
                ASNView.ASNNumber = ASNNumber;
                ASNView.DocNumber = DocNumber;
                string IBDAPIBaseAddress = _configuration.GetValue<string>("IBDAPIBaseAddress");
                string HostURI = IBDAPIBaseAddress + "/api/Test/CreateIBD";
                var uri = new Uri(HostURI);
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/json";
                var SerializedObject = JsonConvert.SerializeObject(ASNView);
                byte[] requestBody = Encoding.UTF8.GetBytes(SerializedObject);
                ASNView.ASNItems = ASNItems;
                using (var postStream = await request.GetRequestStreamAsync())
                {
                    await postStream.WriteAsync(requestBody, 0, requestBody.Length);
                }

                try
                {
                    using (var response = (HttpWebResponse)await request.GetResponseAsync())
                    {
                        if (response != null && response.StatusCode == HttpStatusCode.OK)
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            return responseString != "false" ? "Success" : "Failure creating IBD";
                        }
                        else
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            return "Failure creating IBD";
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        var errorMessage = reader.ReadToEnd();
                        errorMessage = errorMessage.Trim('"');
                        //return errorMessage;
                        return "Error in ASN, ASN saved as draft";
                    }
                }
                catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/CreateIBD", ex); throw new Exception("Something went wrong"); }
                catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/CreateIBD", ex); throw new Exception("Something went wrong"); }
                catch (Exception ex)
                {
                    
                        throw ex;
                   
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/CreateIBD", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/CreateIBD", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                return "Failure creating IBD";
            }
        }

        public async Task<string> CreateIBD1(BPCASNView1 ASNView, string ASNNumber, string DocNumber)
        {
            try
            {
                var ASNItems = ASNView.ASNItems;
                ASNView.ASNItems = (from tb in ASNView.ASNItems where tb.ASNQty > 0 select tb).ToList();
                ASNView.ASNNumber = ASNNumber;
                ASNView.DocNumber = DocNumber;
                string IBDAPIBaseAddress = _configuration.GetValue<string>("IBDAPIBaseAddress");
                string HostURI = IBDAPIBaseAddress + "/api/Test/CreateIBD";
                var uri = new Uri(HostURI);
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/json";
                var SerializedObject = JsonConvert.SerializeObject(ASNView);
                byte[] requestBody = Encoding.UTF8.GetBytes(SerializedObject);
                ASNView.ASNItems = ASNItems;
                using (var postStream = await request.GetRequestStreamAsync())
                {
                    await postStream.WriteAsync(requestBody, 0, requestBody.Length);
                }

                try
                {
                    using (var response = (HttpWebResponse)await request.GetResponseAsync())
                    {
                        if (response != null && response.StatusCode == HttpStatusCode.OK)
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            return responseString != "false" ? "Success" : "Failure creating IBD";
                        }
                        else
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            return "Failure creating IBD";
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        var errorMessage = reader.ReadToEnd();
                        errorMessage = errorMessage.Trim('"');
                        //return errorMessage;
                        return "Error in ASN, ASN saved as draft";
                    }
                }
                catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/CreateIBD", ex); throw new Exception("Something went wrong"); }
                catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/CreateIBD", ex); throw new Exception("Something went wrong"); }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/CreateIBD", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/CreateIBD", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                return "Failure creating IBD";
            }
        }

        public async Task<string> CreateSES(BPCASNView ASNView, string ASNNumber, string DocNumber)
        {
            try
            {
                var ASNItems = ASNView.ASNItems;
                ASNView.ASNItems = (from tb in ASNView.ASNItems where tb.ASNQty > 0 select tb).ToList();
                ASNView.ASNNumber = ASNNumber;
                ASNView.DocNumber = DocNumber;
                string IBDAPIBaseAddress = _configuration.GetValue<string>("IBDAPIBaseAddress");
                string HostURI = IBDAPIBaseAddress + "/api/Test/CreateSES";
                var uri = new Uri(HostURI);
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/json";
                var SerializedObject = JsonConvert.SerializeObject(ASNView);
                byte[] requestBody = Encoding.UTF8.GetBytes(SerializedObject);
                ASNView.ASNItems = ASNItems;
                using (var postStream = await request.GetRequestStreamAsync())
                {
                    await postStream.WriteAsync(requestBody, 0, requestBody.Length);
                }

                try
                {
                    using (var response = (HttpWebResponse)await request.GetResponseAsync())
                    {
                        if (response != null && response.StatusCode == HttpStatusCode.OK)
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            return responseString != "false" ? "Success" : "Failure creating SES";
                        }
                        else
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            return "Failure creating SES";
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        var errorMessage = reader.ReadToEnd();
                        errorMessage = errorMessage.Trim('"');
                        //return errorMessage;

                        return "Error in ASN, ASN saved as draft";
                    }
                }
               
                catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/CreateSES", ex); throw new Exception("Something went wrong"); }
                catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/CreateSES", ex); throw new Exception("Something went wrong"); }
                catch (Exception ex)
                {
                   
                        throw ex;
                  
                   
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/CreateSES", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/CreateSES", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                return "Failure creating SES";
            }
        }

        public async Task<string> CreateSES1(BPCASNView1 ASNView, string ASNNumber, string DocNumber)
        {
            try
            {
                var ASNItems = ASNView.ASNItems;
                ASNView.ASNItems = (from tb in ASNView.ASNItems where tb.ASNQty > 0 select tb).ToList();
                ASNView.ASNNumber = ASNNumber;
                ASNView.DocNumber = DocNumber;
                string IBDAPIBaseAddress = _configuration.GetValue<string>("IBDAPIBaseAddress");
                string HostURI = IBDAPIBaseAddress + "/api/Test/CreateSES";
                var uri = new Uri(HostURI);
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/json";
                var SerializedObject = JsonConvert.SerializeObject(ASNView);
                byte[] requestBody = Encoding.UTF8.GetBytes(SerializedObject);
                ASNView.ASNItems = ASNItems;
                using (var postStream = await request.GetRequestStreamAsync())
                {
                    await postStream.WriteAsync(requestBody, 0, requestBody.Length);
                }

                try
                {
                    using (var response = (HttpWebResponse)await request.GetResponseAsync())
                    {
                        if (response != null && response.StatusCode == HttpStatusCode.OK)
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            return responseString != "false" ? "Success" : "Failure creating SES";
                        }
                        else
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            return "Failure creating SES";
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        var errorMessage = reader.ReadToEnd();
                        errorMessage = errorMessage.Trim('"');
                        //return errorMessage;
                        return "Error in ASN, ASN saved as draft";
                    }
                }
                catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/CreateSES", ex); throw new Exception("Something went wrong"); }
                catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/CreateSES", ex); throw new Exception("Something went wrong"); }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/CreateSES", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/CreateSES", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                return "Failure creating SES";
            }
        }

        public async Task<BPCASNHeader> UpdateASN(BPCASNView ASNView, bool OCRvalidate, string OCRAmount, bool shipAndInvAmount, bool ShipInvOcrAmount, bool InvoiceNumber, string InvoiceNumberOCR, string GSTValue, bool GSTcheck, string SupplierGSTValue, bool SupplierGSTCheck)
        {
            var FMResult = "Success";
            var ASNResult = new BPCASNHeader();
            var strategy = _dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var ASN = _dbContext.Set<BPCASNHeader>().FirstOrDefault(x => x.ASNNumber == ASNView.ASNNumber);
                        if (ASN == null)
                        {
                            return ASN;
                        }
                        //_dbContext.Entry(ASN).State = ASNState.Modified;
                        ASN.Client = ASNView.Client;
                        ASN.Company = ASNView.Company;
                        ASN.Type = ASNView.Type;
                        ASN.PatnerID = ASNView.PatnerID;
                        ASN.ASNNumber = ASNView.ASNNumber;
                        ASN.DocNumber = ASNView.DocNumber;
                        ASN.TransportMode = ASNView.TransportMode;
                        ASN.VessleNumber = ASNView.VessleNumber;
                        ASN.CountryOfOrigin = ASNView.CountryOfOrigin;
                        ASN.AWBNumber = ASNView.AWBNumber;
                        ASN.AWBDate = ASNView.AWBDate;
                        ASN.DepartureDate = ASNView.DepartureDate;
                        ASN.ArrivalDate = ASNView.ArrivalDate;
                        ASN.ShippingAgency = ASNView.ShippingAgency;
                        ASN.GrossWeight = ASNView.GrossWeight;
                        ASN.GrossWeightUOM = ASNView.GrossWeightUOM;
                        ASN.NetWeight = ASNView.NetWeight;
                        ASN.NetWeightUOM = ASNView.NetWeightUOM;
                        ASN.VolumetricWeight = ASNView.VolumetricWeight;
                        ASN.VolumetricWeightUOM = ASNView.VolumetricWeightUOM;
                        ASN.NumberOfPacks = ASNView.NumberOfPacks;
                        ASN.InvoiceNumber = ASNView.InvoiceNumber;
                        ASN.InvoiceDate = ASNView.InvoiceDate;
                        ASN.POBasicPrice = ASNView.POBasicPrice;
                        ASN.TaxAmount = ASNView.TaxAmount;
                        ASN.InvoiceAmount = ASNView.InvoiceAmount;
                        ASN.InvoiceAmountUOM = ASNView.InvoiceAmountUOM;
                        ASN.ModifiedBy = ASNView.ModifiedBy;
                        ASN.IsSubmitted = ASNView.IsSubmitted;
                        ASN.ArrivalDateInterval = ASNView.ArrivalDateInterval;
                        ASN.BillOfLading = ASNView.BillOfLading;
                        ASN.TransporterName = ASNView.TransporterName;
                        ASN.AccessibleValue = ASNView.AccessibleValue;
                        ASN.ContactPerson = ASNView.ContactPerson;
                        ASN.ContactPersonNo = ASNView.ContactPersonNo;
                        ASN.Field1 = ASNView.Field1;
                        ASN.Field2 = ASNView.Field2;
                        ASN.Field3 = ASNView.Field3;
                        ASN.Field4 = ASNView.Field4;
                        ASN.Field5 = ASNView.Field5;
                        ASN.Field6 = ASNView.Field6;
                        ASN.Field7 = ASNView.Field7;
                        ASN.Field8 = ASNView.Field8;
                        ASN.Field9 = ASNView.Field9;
                        ASN.Field10 = ASNView.Field10;
                        ASN.ModifiedOn = DateTime.Now;
                        //ASN.Status = ASN.IsSubmitted ? "Submitted" : "Saved";
                        ASN.Status = ASNView.Status;
                        ASN.IsBuyerApprovalRequired = ASNView.IsBuyerApprovalRequired;
                        ASN.BuyerApprovalStatus = ASNView.BuyerApprovalStatus;
                        if (ASN.IsSubmitted)
                        {
                            ASN.ASNDate = ASNView.ASNDate;
                            if (ASN.Status == "ShipmentNotRelevant")
                            {
                                ASN.Status = "GateEntry";
                                ASN.Field2 = "ShipmentNotRelevant";
                                ASN.ASNDate = DateTime.Now;
                            }
                        }
                        await _dbContext.SaveChangesAsync();



                        //if (ASN.IsSubmitted)
                        //{
                        //    //await UpdatePOStatus(ASN.DocNumber);

                        //    var OFH = _dbContext.BPCOFHeaders.Where(x => x.DocNumber == ASN.DocNumber).FirstOrDefault();
                        //    if (OFH != null && OFH.DocType == "SER")
                        //    {
                        //        WriteLog.WriteToFile($"SES creation started for ASN {ASN.ASNNumber}");
                        //        FMResult = await CreateSES(ASNView, ASN.ASNNumber, ASN.DocNumber);
                        //    }
                        //    else
                        //    {
                        //        WriteLog.WriteToFile($"IBD creation started for ASN {ASN.ASNNumber}");
                        //        FMResult = await CreateIBD(ASNView, ASN.ASNNumber, ASN.DocNumber);
                        //    }
                        //    if (FMResult == "Success")
                        //    {
                        //        ASN.Field1 = "";
                        //    }
                        //    else
                        //    {
                        //        // ASN.ASNDate = ASNView.ASNDate;
                        //        ASN.Status = "Saved";
                        //        ASN.IsSubmitted = false;
                        //        ASN.ASNDate = (DateTime?)null;
                        //        await _dbContext.SaveChangesAsync();
                        //        ASN.Field1 = FMResult;
                        //        //transaction.Commit();
                        //        //transaction.Dispose();
                        //        //throw new Exception(FMResult);
                        //    }
                        //}

                        var asnIts = _dbContext.BPCASNItems.Where(x => x.ASNNumber == ASNView.ASNNumber).ToList();

                        foreach (var x in asnIts)
                        {

                            await UpdateDeletedPOItemQty(x, ASN.DocNumber);
                            _dbContext.BPCASNItems.Remove(x);
                        }

                        await _dbContext.SaveChangesAsync();
                        bool IsNonZeroOpenQty = await CreateASNItems(ASNView.ASNItems, ASN.ASNNumber, ASN.DocNumber, ASN.IsSubmitted);

                        _dbContext.BPCASNPacks.Where(x => x.ASNNumber == ASNView.ASNNumber).ToList().ForEach(x => _dbContext.BPCASNPacks.Remove(x));
                        await _dbContext.SaveChangesAsync();
                        await CreateASNPacks(ASNView.ASNPacks, ASN.ASNNumber, ASN.DocNumber);
                        _dbContext.BPCDocumentCenters.Where(x => x.ASNNumber == ASNView.ASNNumber).ToList().ForEach(x => _dbContext.BPCDocumentCenters.Remove(x));
                        await _dbContext.SaveChangesAsync();
                        await CreateDocumentCenters(ASNView.DocumentCenters, ASN.ASNNumber);

                        if (ASN.IsSubmitted)
                        {
                            var BaseFileName = ASN.Company + DateTime.Now.ToString("ddMMyyyy") + "_" + ASN.ASNNumber;
                            bool ASNXmlDataResult = CreateASNXmlData(ASN, ASNView.ASNItems, BaseFileName, OCRvalidate);
                            bool InvoiceResult = UpdateAndSaveASNInvAttachment(ASN, ASNView.InvAttachmentName, BaseFileName);
                            var documentinvoices = _dbContext.BPCDocumentCenters.Where(x => x.ASNNumber == ASN.ASNNumber).ToList();
                            int i = 0;
                            foreach (BPCDocumentCenter documentinvoice in documentinvoices)
                            {
                                i++;
                                if (documentinvoice.AttachmentReferenceNo != null)
                                {

                                    bool DocInvoiceResult = SaveASNDocAttachment(ASN, documentinvoice.Filename, BaseFileName, i);
                                }
                            }
                            if (ASNXmlDataResult && InvoiceResult)
                            {
                                ASN.Field1 = "";
                                var OFH = _dbContext.BPCOFHeaders.Where(x => x.DocNumber == ASN.DocNumber).FirstOrDefault();
                                if (OFH != null && OFH.DocType == "SER")
                                {
                                    WriteLog.WriteToFile($"SES creation started for ASN {ASN.ASNNumber}");
                                    FMResult = await CreateSES(ASNView, ASN.ASNNumber, ASN.DocNumber);
                                }
                                else
                                {
                                    WriteLog.WriteToFile($"IBD creation started for ASN {ASN.ASNNumber}");
                                    FMResult = await CreateIBD(ASNView, ASN.ASNNumber, ASN.DocNumber);
                                }
                                if (FMResult == "Success")
                                {
                                    ASN.Field1 = "";
                                }
                                else
                                {
                                    // ASN.ASNDate = ASNView.ASNDate;
                                    ASN.Status = "Saved";
                                    ASN.IsSubmitted = false;
                                    ASN.ASNDate = (DateTime?)null;
                                    ASN.Field1 = FMResult;
                                    await _dbContext.SaveChangesAsync();
                                    DeleteASNFiles(ASN.ASNNumber);
                                    //transaction.Commit();
                                    //transaction.Dispose();
                                    //throw new Exception(FMResult);
                                }
                            }
                            else
                            {
                                ASN.Status = "Saved";
                                ASN.IsSubmitted = false;
                                ASN.ASNDate = (DateTime?)null;
                                ASN.Field1 = ASNXmlDataResult ? InvoiceResult ? "Error in ASN FTP Files, ASN saved as draft" : "Error in ASN Invoice document, ASN saved as draft" : "Error in ASN Xml document,ASN saved as draft";
                                await _dbContext.SaveChangesAsync();
                                DeleteASNFiles(ASN.ASNNumber);
                            }
                        }
                        else
                        {
                            string InvoiceResult = UpdateASNInvAttachment(ASN, ASNView.InvAttachmentName);
                        }

                        if (ASN.IsSubmitted && FMResult == "Success")
                        {
                            ASNView.ASNNumber = ASN.ASNNumber;
                            await FindPOCreatorAndSendASNNotification(ASNView);
                            if (IsNonZeroOpenQty)
                            {
                                await UpdatePOStatus(ASN, "PartialASN");
                            }
                            else
                            {
                                await UpdatePOStatus(ASN, "DueForGate");
                            }
                        }

                        if (ASN.IsBuyerApprovalRequired && string.IsNullOrEmpty(ASN.BuyerApprovalStatus))
                        {
                            await FindPOCreatorAndSendASNApprovalNotification(ASNView,OCRAmount,shipAndInvAmount,ShipInvOcrAmount, InvoiceNumber,InvoiceNumberOCR, GSTValue,  GSTcheck,SupplierGSTValue, SupplierGSTCheck);
                        }

                        transaction.Commit();
                        transaction.Dispose();
                        ASNResult = ASN;
                        return ASN;
                    }
                    catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/UpdateASN", ex); throw new Exception("Something went wrong"); }
                    catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/UpdateASN", ex); throw new Exception("Something went wrong"); }
                    catch (Exception ex)
                    {
                        //if (FMResult == "Success")
                        //{
                        //    transaction.Rollback();
                        //    transaction.Dispose();
                        //}
                        transaction.Rollback();
                        transaction.Dispose();
                        throw ex;
                    }
                }
            });
            return ASNResult;
        }

        public async Task<BPCASNHeader1> UpdateASN1(BPCASNView1 ASNView ,bool OCRvalidate, string OCRAmount, bool shipAndInvAmount, bool ShipInvOcrAmount, bool InvoiceNumber, string InvoiceNumberOCR, string GSTValue, bool GSTcheck, string SupplierGSTValue, bool SupplierGSTCheck)
        {
            var FMResult = "Success";
           
            var ASNResult = new BPCASNHeader1();
            var strategy = _dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var ASN = _dbContext.Set<BPCASNHeader1>().FirstOrDefault(x => x.ASNNumber == ASNView.ASNNumber);
                        if (ASN == null)
                        {
                            return ASN;
                        }
                        //_dbContext.Entry(ASN).State = ASNState.Modified;
                        ASN.Client = ASNView.Client;
                        ASN.Company = ASNView.Company;
                        ASN.Type = ASNView.Type;
                        ASN.PatnerID = ASNView.PatnerID;
                        ASN.ASNNumber = ASNView.ASNNumber;
                        //ASN.DocNumber = ASNView.DocNumber;
                        ASN.TransportMode = ASNView.TransportMode;
                        ASN.VessleNumber = ASNView.VessleNumber;
                        ASN.CountryOfOrigin = ASNView.CountryOfOrigin;
                        ASN.AWBNumber = ASNView.AWBNumber;
                        ASN.AWBDate = ASNView.AWBDate;
                        ASN.DepartureDate = ASNView.DepartureDate;
                        ASN.ArrivalDate = ASNView.ArrivalDate;
                        ASN.ShippingAgency = ASNView.ShippingAgency;
                        ASN.GrossWeight = ASNView.GrossWeight;
                        ASN.GrossWeightUOM = ASNView.GrossWeightUOM;
                        ASN.NetWeight = ASNView.NetWeight;
                        ASN.NetWeightUOM = ASNView.NetWeightUOM;
                        ASN.VolumetricWeight = ASNView.VolumetricWeight;
                        ASN.VolumetricWeightUOM = ASNView.VolumetricWeightUOM;
                        ASN.NumberOfPacks = ASNView.NumberOfPacks;
                        ASN.InvoiceNumber = ASNView.InvoiceNumber;
                        ASN.InvoiceDate = ASNView.InvoiceDate;
                        ASN.POBasicPrice = ASNView.POBasicPrice;
                        ASN.TaxAmount = ASNView.TaxAmount;
                        ASN.InvoiceAmount = ASNView.InvoiceAmount;
                        ASN.InvoiceAmountUOM = ASNView.InvoiceAmountUOM;
                        ASN.ModifiedBy = ASNView.ModifiedBy;
                        ASN.IsSubmitted = ASNView.IsSubmitted;
                        ASN.ArrivalDateInterval = ASNView.ArrivalDateInterval;
                        ASN.BillOfLading = ASNView.BillOfLading;
                        ASN.TransporterName = ASNView.TransporterName;
                        ASN.AccessibleValue = ASNView.AccessibleValue;
                        ASN.ContactPerson = ASNView.ContactPerson;
                        ASN.ContactPersonNo = ASNView.ContactPersonNo;
                        ASN.Field1 = ASNView.Field1;
                        ASN.Field2 = ASNView.Field2;
                        ASN.Field3 = ASNView.Field3;
                        ASN.Field4 = ASNView.Field4;
                        ASN.Field5 = ASNView.Field5;
                        ASN.Field6 = ASNView.Field6;
                        ASN.Field7 = ASNView.Field7;
                        ASN.Field8 = ASNView.Field8;
                        ASN.Field9 = ASNView.Field9;
                        ASN.Field10 = ASNView.Field10;
                        ASN.ModifiedOn = DateTime.Now;
                        //ASN.Status = ASN.IsSubmitted ? "Submitted" : "Saved";
                        ASN.Status = ASNView.Status;
                        ASN.IsBuyerApprovalRequired = ASNView.IsBuyerApprovalRequired;
                        ASN.BuyerApprovalStatus = ASNView.BuyerApprovalStatus;
                        if (ASN.IsSubmitted)
                        {
                            ASN.ASNDate = ASNView.ASNDate;
                            if (ASN.Status == "ShipmentNotRelevant")
                            {
                                ASN.Status = "GateEntry";
                                ASN.Field2 = "ShipmentNotRelevant";
                                ASN.ASNDate = DateTime.Now;
                            }
                        }
                        await _dbContext.SaveChangesAsync();



                        //if (ASN.IsSubmitted)
                        //{
                        //    //await UpdatePOStatus(ASN.DocNumber);

                        //    var OFH = _dbContext.BPCOFHeaders.Where(x => x.DocNumber == ASN.DocNumber).FirstOrDefault();
                        //    if (OFH != null && OFH.DocType == "SER")
                        //    {
                        //        WriteLog.WriteToFile($"SES creation started for ASN {ASN.ASNNumber}");
                        //        FMResult = await CreateSES(ASNView, ASN.ASNNumber, ASN.DocNumber);
                        //    }
                        //    else
                        //    {
                        //        WriteLog.WriteToFile($"IBD creation started for ASN {ASN.ASNNumber}");
                        //        FMResult = await CreateIBD(ASNView, ASN.ASNNumber, ASN.DocNumber);
                        //    }
                        //    if (FMResult == "Success")
                        //    {
                        //        ASN.Field1 = "";
                        //    }
                        //    else
                        //    {
                        //        // ASN.ASNDate = ASNView.ASNDate;
                        //        ASN.Status = "Saved";
                        //        ASN.IsSubmitted = false;
                        //        ASN.ASNDate = (DateTime?)null;
                        //        await _dbContext.SaveChangesAsync();
                        //        ASN.Field1 = FMResult;
                        //        //transaction.Commit();
                        //        //transaction.Dispose();
                        //        //throw new Exception(FMResult);
                        //    }
                        //}

                        var asnIts = _dbContext.BPCASNItem1.Where(x => x.ASNNumber == ASNView.ASNNumber).ToList();
                        foreach (var x in asnIts)
                        {
                            await UpdateDeletedPOItemQty1(x);
                            _dbContext.BPCASNItem1.Remove(x);
                        }
                        await _dbContext.SaveChangesAsync();
                        var asnItemViews = await CreateASNItems1(ASNView.ASNItems, ASN.ASNNumber, ASN.DocNumber, ASN.IsSubmitted);

                        _dbContext.BPCASNPacks.Where(x => x.ASNNumber == ASNView.ASNNumber).ToList().ForEach(x => _dbContext.BPCASNPacks.Remove(x));
                        await _dbContext.SaveChangesAsync();
                        await CreateASNPacks(ASNView.ASNPacks, ASN.ASNNumber, ASN.DocNumber);
                        _dbContext.BPCDocumentCenters.Where(x => x.ASNNumber == ASNView.ASNNumber).ToList().ForEach(x => _dbContext.BPCDocumentCenters.Remove(x));
                        await _dbContext.SaveChangesAsync();
                        await CreateDocumentCenters(ASNView.DocumentCenters, ASN.ASNNumber);

                        if (ASN.IsSubmitted)
                        {
                            var BaseFileName = ASN.Company + DateTime.Now.ToString("ddMMyyyy") + "_" + ASN.ASNNumber;
                            bool ASNXmlDataResult = CreateASNXmlData1(ASN, ASNView.DocNumbers, ASNView.ASNItems, BaseFileName,OCRvalidate);
                            bool InvoiceResult = UpdateAndSaveASNInvAttachment1(ASN, ASNView.InvAttachmentName, BaseFileName);
                            if (ASNXmlDataResult && InvoiceResult)
                            {
                                ASN.Field1 = "";
                                var OFH = _dbContext.BPCOFHeaders.Where(x => x.DocNumber == ASN.DocNumber).FirstOrDefault();
                                if (OFH != null && OFH.DocType == "SER")
                                {
                                    WriteLog.WriteToFile($"SES creation started for ASN {ASN.ASNNumber}");
                                    FMResult = await CreateSES1(ASNView, ASN.ASNNumber, ASN.DocNumber);
                                }
                                else
                                {
                                    WriteLog.WriteToFile($"IBD creation started for ASN {ASN.ASNNumber}");
                                    FMResult = await CreateIBD1(ASNView, ASN.ASNNumber, ASN.DocNumber);
                                }
                                if (FMResult == "Success")
                                {
                                    ASN.Field1 = "";
                                }
                                else
                                {
                                    // ASN.ASNDate = ASNView.ASNDate;
                                    ASN.Status = "Saved";
                                    ASN.IsSubmitted = false;
                                    ASN.ASNDate = (DateTime?)null;
                                    ASN.Field1 = FMResult;
                                    await _dbContext.SaveChangesAsync();
                                    DeleteASNFiles(ASN.ASNNumber);
                                    //transaction.Commit();
                                    //transaction.Dispose();
                                    //throw new Exception(FMResult);
                                }
                            }
                            else
                            {
                                ASN.Status = "Saved";
                                ASN.IsSubmitted = false;
                                ASN.ASNDate = (DateTime?)null;
                                ASN.Field1 = ASNXmlDataResult ? InvoiceResult ? "Error in ASN FTP Files, ASN saved as draft" : "Error in ASN Invoice document, ASN saved as draft" : "Error in ASN Xml document,ASN saved as draft";
                                await _dbContext.SaveChangesAsync();
                                DeleteASNFiles(ASN.ASNNumber);
                            }
                        }
                        else
                        {
                            string InvoiceResult = UpdateASNInvAttachment1(ASN, ASNView.InvAttachmentName);
                        }

                        if (ASN.IsSubmitted && FMResult == "Success")
                        {
                            ASNView.ASNNumber = ASN.ASNNumber;
                            await FindPOCreatorAndSendASNNotification1(ASNView,OCRAmount,shipAndInvAmount,ShipInvOcrAmount,InvoiceNumber, InvoiceNumberOCR, GSTValue, GSTcheck);
                            var Groups = asnItemViews.GroupBy(x => new { x.Client, x.Company, x.Type, x.PatnerID, x.DocNumber });
                            var DocNumber = "";
                            foreach (var g in Groups)
                            {
                                bool IsNonZeroOpenQty1 = false;
                                foreach (var y in g)
                                {
                                    DocNumber = y.DocNumber;
                                    if (!IsNonZeroOpenQty1)
                                    {
                                        IsNonZeroOpenQty1 = y.ItemOpenQty.HasValue && y.ItemOpenQty.Value > 0;
                                    }
                                }
                                if (IsNonZeroOpenQty1)
                                {
                                    await UpdatePOStatus1(ASN, DocNumber, "PartialASN");
                                }
                                else
                                {
                                    await UpdatePOStatus1(ASN, DocNumber, "DueForGate");
                                }
                            }

                            //if (IsNonZeroOpenQty)
                            //{
                            //    await UpdatePOStatus1(ASN, "PartialASN");
                            //}
                            //else
                            //{
                            //    await UpdatePOStatus1(ASN, "DueForGate");
                            //}
                        }

                        if (ASN.IsBuyerApprovalRequired && string.IsNullOrEmpty(ASN.BuyerApprovalStatus))
                        {
                            await FindPOCreatorAndSendASNApprovalNotification1(ASNView,OCRAmount,shipAndInvAmount,ShipInvOcrAmount,  InvoiceNumber,  InvoiceNumberOCR,  GSTValue,  GSTcheck, SupplierGSTValue,  SupplierGSTCheck);
                        }

                        transaction.Commit();
                        transaction.Dispose();
                        ASNResult = ASN;
                        return ASN;
                    }
                    catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/UpdateASN", ex); throw new Exception("Something went wrong"); }
                    catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/UpdateASN", ex); throw new Exception("Something went wrong"); }
                    catch (Exception ex)
                    {
                        //if (FMResult == "Success")
                        //{
                        //    transaction.Rollback();
                        //    transaction.Dispose();
                        //}
                        transaction.Rollback();
                        transaction.Dispose();
                        throw ex;
                    }
                }
            });
            return ASNResult;
        }


        public async Task UpdateASNApprovalStatus(BPCASNViewApproval viewApproval)
        {
            try
            {
                BPCASNHeader header = (from tb in _dbContext.BPCASNHeaders
                                       where tb.Client == viewApproval.Client && tb.Company == viewApproval.Company && tb.Type == viewApproval.Type &&
                                       tb.PatnerID == viewApproval.PatnerID && tb.ASNNumber == viewApproval.ASNNumber
                                       select tb).FirstOrDefault();
                if (header != null)
                {
                    header.BuyerApprovalStatus = viewApproval.BuyerApprovalStatus;
                    header.BuyerApprovalOn = DateTime.Now;
                    await _dbContext.SaveChangesAsync();
                    await SendASNReleaseNotification(viewApproval);
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/UpdateASNApprovalStatus", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/UpdateASNApprovalStatus", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASNFieldMasterRepository/UpdateASNApprovalStatus : - ", ex);
                throw ex;
            }
        }

        public async Task UpdateASN1ApprovalStatus(BPCASNViewApproval viewApproval)
        {
            try
            {
                BPCASNHeader1 header = (from tb in _dbContext.BPCASNHeader1
                                        where tb.Client == viewApproval.Client && tb.Company == viewApproval.Company && tb.Type == viewApproval.Type &&
                                        tb.PatnerID == viewApproval.PatnerID && tb.ASNNumber == viewApproval.ASNNumber
                                        select tb).FirstOrDefault();
                if (header != null)
                {
                    header.BuyerApprovalStatus = viewApproval.BuyerApprovalStatus;
                    header.BuyerApprovalOn = DateTime.Now;
                    await _dbContext.SaveChangesAsync();
                    await SendASN1ReleaseNotification(viewApproval);
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/UpdateASN1ApprovalStatus", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/UpdateASN1ApprovalStatus", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASNFieldMasterRepository/UpdateASN1ApprovalStatus : - ", ex);
                throw ex;
            }
        }



        public async Task SendASNReleaseNotification(BPCASNViewApproval viewApproval)
        {
            try
            {
                var today = DateTime.Now;
                var result = (from tb in _dbContext.BPCASNHeaders
                              where tb.ASNNumber == viewApproval.ASNNumber
                              select tb).ToList();
                var PatnerIDs = result.Select(x => x.PatnerID).Distinct().ToList();
                var Items = result.GroupBy(x => x.PatnerID).ToList();
                var UserViews = await GetUserViewsByPatnerIDs(PatnerIDs);
                foreach (var item in Items)
                {
                    var email = UserViews.Where(x => x.UserName == item.Key).Select(x => x.Email).FirstOrDefault();
                    if (!string.IsNullOrEmpty(email))
                    {
                        await SendMail(email, viewApproval);
                    }
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/SendASNReleaseNotification", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/SendASNReleaseNotification", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SendASN1ReleaseNotification(BPCASNViewApproval viewApproval)
        {
            try
            {
                var today = DateTime.Now;
                var result = (from tb in _dbContext.BPCASNHeader1
                              where tb.ASNNumber == viewApproval.ASNNumber
                              select tb).ToList();
                var PatnerIDs = result.Select(x => x.PatnerID).Distinct().ToList();
                var Items = result.GroupBy(x => x.PatnerID).ToList();
                var UserViews = await GetUserViewsByPatnerIDs(PatnerIDs);
                foreach (var item in Items)
                {
                    var email = UserViews.Where(x => x.UserName == item.Key).Select(x => x.Email).FirstOrDefault();
                    if (!string.IsNullOrEmpty(email))
                    {
                        await SendMail(email, viewApproval);
                    }
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/SendASN1ReleaseNotification", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/SendASN1ReleaseNotification", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<UserView>> GetUserViewsByPatnerIDs(List<string> PatnerIDs)
        {
            try
            {
                string BaseAddress = _configuration.GetValue<string>("APIBaseAddress");
                string HostURI = BaseAddress + "/authenticationapi/Master/GetUserViewsByPatnerIDs";
                var uri = new Uri(HostURI);
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/json";
                var SerializedObject = JsonConvert.SerializeObject(PatnerIDs);
                byte[] requestBody = Encoding.UTF8.GetBytes(SerializedObject);

                using (var postStream = await request.GetRequestStreamAsync())
                {
                    await postStream.WriteAsync(requestBody, 0, requestBody.Length);
                }

                try
                {
                    using (var response = (HttpWebResponse)await request.GetResponseAsync())
                    {
                        if (response != null && response.StatusCode == HttpStatusCode.OK)
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            var userViews = JsonConvert.DeserializeObject<List<UserView>>(responseString);
                            return userViews;
                        }
                        else
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            WriteLog.WriteToFile($"PORespository/GetUserViewsByPatnerIDs : - {responseString}");
                            return null;
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        var errorMessage = reader.ReadToEnd();

                        throw ex;
                    }
                }
                catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetUserViewsByPatnerIDs", ex); throw new Exception("Something went wrong"); }
                catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetUserViewsByPatnerIDs", ex); throw new Exception("Something went wrong"); }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("PORespository/GetUserViewsByPatnerIDs", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PORespository/GetUserViewsByPatnerIDs", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> SendMail(string toEmail, BPCASNViewApproval viewApproval)
        {

            try
            {
                var STMPDetailsConfig = _configuration.GetSection("STMPDetails");
                string hostName = STMPDetailsConfig["Host"];
                string SMTPEmail = STMPDetailsConfig["Email"];
                //string SMTPEmailPassword = STMPDetailsConfig["Password"];
                string SMTPPort = STMPDetailsConfig["Port"];
                string siteURL = _configuration.GetValue<string>("SiteURL");

                var message = new MailMessage();
                string subject = "";
                StringBuilder sb = new StringBuilder();
                //UserName = string.IsNullOrEmpty(UserName) ? toEmail.Split('@')[0] : UserName;
                var body = $@"<html><head></head><body> <div style='border:1px solid #dbdbdb;'> <div style='padding: 20px 20px; background-color: #fff06769;text-align: center;font-family: Segoe UI;'> <p> 
                              <h2>Emami Vendor Portal</h2> </p> </div> <div style='background-color: #f8f7f7;padding: 20px 20px;font-family: Segoe UI'> 
                              <div> 
                              <p> Dear Sir/Madam ,</p>
                              ";
                sb.Append(@body);
                var content = $@"<p>ASN {viewApproval.ASNNumber} has been {viewApproval.BuyerApprovalStatus} by Buyer</p>";
                sb.Append(content);
                //string styles = @"<style>
                //                        table {
                //                          /*font-family: arial, sans-serif;*/
                //                          border-collapse: collapse;
                //                          width: 100%;
                //                          margin:20px 0px;
                //                        }

                //                        td, th {
                //                          border: 1px solid #dddddd;
                //                          text-align: left;
                //                          padding: 8px;
                //                        }

                //                        tr:nth-child(even) {
                //                          /*background-color: #dddddd;*/
                //                        }
                //                        .lar{
                //                            width: 25%;
                //                         }
                //                        .lar1{
                //                            width: 20%;
                //                         }

                //                      </style>";
                //sb.Append(styles);
                //string table = @"<table>
                //                              <tr>
                //                                <th>Doc Number</th>
                //                                <th>Item</th>
                //                                <th>Material</th>
                //                                <th>Material Text</th>
                //                                <th>Delivery date</th>
                //                                <th>Order Qty</th>
                //                                <th>Completed Qty</th>
                //                              </tr>";
                //foreach (var item in items)
                //{
                //    //var lic = (from tb in _ctx.LicenseMasters where tb.LicenseID == item.LicenseID select tb).FirstOrDefault();
                //    //var des = lic != null ? lic.Description : item.LicenseID.ToString();
                //    table += $@"<tr>
                //                         <td>{item.DocNumber}</td>
                //                         <td>{item.Item}</td>
                //                         <td>{item.Material}</td>
                //                         <td class='lar'>{item.MaterialText}</td>
                //                         <td>{(item.DeliveryDate.HasValue ? item.DeliveryDate.Value.ToString("dd/MM/yyyy") : "")}</td>
                //                         <td>{item.OrderedQty}</td>
                //                         <td>{item.CompletedQty}</td>
                //                        </tr>";
                //};
                //table += @"</table>";
                //sb.Append(table);
                //var today = DateTime.Now;
                //foreach (var item in items)
                //{
                //    if (item.DeliveryDate.HasValue)
                //    {
                //        var dateDifference = (today - item.DeliveryDate.Value).Days;
                //        string Plant = item.PlantCode;
                //        var plantMaster = _dbContext.BPCPlantMasters.FirstOrDefault(x => x.PlantCode == item.PlantCode);
                //        if (plantMaster != null)
                //        {
                //            Plant = plantMaster.PlantText ?? plantMaster.PlantCode;
                //        }
                //        var content = $@"<p>PO Number {item.DocNumber}/{item.Item} raised at EMAMI LTD, {Plant}
                //                         <p>Delivery date - {item.DeliveryDate.Value.ToString("dd/MM/yyyy")}
                //                         for Material {item.Material} - {item.MaterialText} – HEAVY is due by {dateDifference} day/s</p>";
                //        sb.Append(content);
                //    }

                //};
                if (viewApproval.BuyerApprovalStatus == "Approved")
                {
                    var content1 = "<p>Kindly process the ASN dispatch details.</p>";
                    sb.Append(content1);
                }

                var regards = @"<br><p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";
                sb.Append(regards);
                subject = "ASN Approval - Alert";
                SmtpClient client = new SmtpClient();
                client.Port = Convert.ToInt32(SMTPPort);
                client.Host = hostName;
                client.EnableSsl = false;
                client.Timeout = 60000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                //client.UseDefaultCredentials = false;
                //client.Credentials = new System.Net.NetworkCredential(SMTPEmail, SMTPEmailPassword);
                MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, subject, sb.ToString());
                reportEmail.BodyEncoding = UTF8Encoding.UTF8;
                reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                reportEmail.IsBodyHtml = true;
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                await client.SendMailAsync(reportEmail);
                WriteLog.WriteToFile($"ASNRepository - ASN Approval Notification sent successfully to {toEmail}");
                return true;
            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        WriteLog.WriteToFile("ASNRepository/SendMail/MailboxBusy/MailboxUnavailable/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                    else
                    {
                        WriteLog.WriteToFile("ASNRepository/SendMail/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                }
                WriteLog.WriteToFile("ASNRepository/SendMail/SmtpFailedRecipientsException:- " + ex.Message, ex);
                return false;
            }
            catch (SmtpException ex)
            {
                WriteLog.WriteToFile("ASNRepository/SendMail/SmtpException:- " + ex.Message, ex);
                return false;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/SendMail", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/SendMail", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASNRepository/SendMail/Exception:- " + ex.Message, ex);
                return false;
            }


            //string subject = "";
            //string ApplicationUrl = ConfigurationManager.AppSettings["ApplicationUrl"].ToString();
            //StringBuilder sb = new StringBuilder();
            //sb.Append("Dear Sir/Madam,<br/>");
            //string content = "";
            //if (mailModel.CNType == CNType.Compliance)
            //{
            //    content = mailModel.ActivityType == ActivityType.Create ? "New Compliance has been released with the following details:" :
            //              mailModel.ActivityType == ActivityType.Update ? "Compliance has been updated with the following details:" :
            //              "Compliance has been closed. Please find the following details";
            //}
            //else if (mailModel.CNType == CNType.License)
            //{
            //    content = mailModel.ActivityType == ActivityType.Create ? "New License has been released with the following details:" :
            //              mailModel.ActivityType == ActivityType.Update ? "License has been updated with the following details:" :
            //              "License has been closed. Please find the following details";

            //}
            //sb.Append(String.Format("<p>{0}</p>", content));
            //sb.Append(String.Format("<p><b>Unit</b> : {0}</p>", mailModel.Unit));
            //if (mailModel.ActivityType == ActivityType.Create)
            //{
            //    sb.Append(String.Format("<p><b>Created by</b> : {0}</p>", mailModel.PerformedBy));
            //}
            //else if (mailModel.ActivityType == ActivityType.Update)
            //{
            //    sb.Append(String.Format("<p><b>Updated by</b> : {0}</p>", mailModel.PerformedBy));
            //}
            //else if (mailModel.ActivityType == ActivityType.Close)
            //{
            //    sb.Append(String.Format("<p><b>Closed by</b> : {0}</p>", mailModel.PerformedBy));
            //}
            //if (mailModel.CNType == CNType.Compliance)
            //{
            //    sb.Append(String.Format("<p><b>Compliance Request Number</b> : {0}</p>", mailModel.ReferenceNumber));
            //    sb.Append(String.Format("<p><b>Open date</b> : {0}</p>", mailModel.OpenDate.ToString("dd/MM/yyyy")));
            //    sb.Append(String.Format("<p><b>End date</b> : {0}</p>", mailModel.EndDate.ToString("dd/MM/yyyy")));
            //}
            //if (mailModel.CNType == CNType.License)
            //{
            //    sb.Append(String.Format("<p><b>License Request Number</b> : {0}</p>", mailModel.ReferenceNumber));
            //    //sb.Append(String.Format("<p><b>Open date</b> : {0}</p>", mailModel.OpenDate.ToString("dd/MM/yyyy")));
            //    //sb.Append(String.Format("<p><b>End date</b> : {0}</p>", mailModel.EndDate.ToString("dd/MM/yyyy")));
            //}
            //if (mailModel.ActivityType == ActivityType.Update && updatedLicenseItems != null && updatedLicenseItems.Count > 0)
            //{
            //    string styles = @"<style>
            //                        table {
            //                          /*font-family: arial, sans-serif;*/
            //                          border-collapse: collapse;
            //                          width: 100%;
            //                        }

            //                        td, th {
            //                          border: 1px solid #dddddd;
            //                          text-align: left;
            //                          padding: 8px;
            //                        }

            //                        tr:nth-child(even) {
            //                          /*background-color: #dddddd;*/
            //                        }
            //                        .lar{
            //                            width: 25%;
            //                         }
            //                        .lar1{
            //                            width: 20%;
            //                         }

            //                      </style>";
            //    sb.Append(styles);
            //    string table = @"<table>
            //                          <tr>
            //                            <th>License/NOC</th>
            //                            <th>Yes/No</th>
            //                            <th>One Time</th>
            //                            <th>Validity</th>
            //                            <th>Renewal Due Date</th>
            //                            <th>Remarks</th>
            //                          </tr>";
            //    foreach (var item in updatedLicenseItems)
            //    {
            //        //var lic = (from tb in _ctx.LicenseMasters where tb.LicenseID == item.LicenseID select tb).FirstOrDefault();
            //        //var des = lic != null ? lic.Description : item.LicenseID.ToString();
            //        table += $@"<tr>
            //                     <td class='lar'>{item.Description}</td>
            //                     <td>{item.Status}</td>
            //                     <td>{item.OneTime}</td>
            //                     <td>{(item.Validity.HasValue ? item.Validity.Value.ToString("dd/MM/yyyy") : "")}</td>
            //                     <td>{(item.RenewalDueDate.HasValue ? item.RenewalDueDate.Value.ToString("dd/MM/yyyy") : "")}</td>
            //                     <td class='lar1'>{item.Remarks}</td>
            //                    </tr>";
            //    };
            //    table += @"</table>";
            //    sb.Append(table);
            //}

            //sb.Append(String.Format("<p>Click <a href='{0}' target='_blank'>here</a> to login to the application</p>", ApplicationUrl));
            //sb.Append(String.Format("<p>This is a system generated notification, please do not reply.</p>"));
            //sb.Append(String.Format("<p>Regards,</p>"));
            //sb.Append(String.Format("<p>System Administrator</p>"));




            ////sb.Append("</p></div><p>UBL Admin,</p><p style='font-size:14px;color:#ffffff;background-color:#00008B;height:20px;padding:4px;'>© United Breweries Limited</p>");
            //subject = mailModel.Subject;

            //string hostName = ConfigurationManager.AppSettings["HostName"];
            //string SMTPEmail = ConfigurationManager.AppSettings["SMTPEmail"];
            ////string SMTPEmailPassword = ConfigurationManager.AppSettings["SMTPEmailPassword"]; // Comment this line for UBL deployement
            //var message = new MailMessage();
            //SmtpClient client = new SmtpClient();
            //client.Port = Convert.ToInt16(ConfigurationManager.AppSettings["SMTPPort"]);
            //client.Host = ConfigurationManager.AppSettings["HostName"];
            //client.EnableSsl = false;// change this to false for UBL deployement
            //client.Timeout = 60000;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            ////client.UseDefaultCredentials = false; // Comment this line for UBL deployement
            ////client.Credentials = new System.Net.NetworkCredential(SMTPEmail, SMTPEmailPassword);// Comment this line for UBL deployement
            //MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, subject, sb.ToString());
            //reportEmail.BodyEncoding = UTF8Encoding.UTF8;
            //reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            //reportEmail.IsBodyHtml = true;
            //await client.SendMailAsync(reportEmail);
            //WriteLog.WriteProcessLog($"License/SendMail : Mail has been sent successfully {toEmail}");
            //string hostName = ConfigurationManager.AppSettings["HostName"];
            //string SMTPEmail = ConfigurationManager.AppSettings["SMTPEmail"];
            //var message = new MailMessage();
            //SmtpClient client = new SmtpClient();
            //client.Port = Convert.ToInt16(ConfigurationManager.AppSettings["SMTPPort"]);
            //client.Host = ConfigurationManager.AppSettings["HostName"];
            //client.EnableSsl = false;
            //client.Timeout = 60000;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //string SMTPEmailPassword = ConfigurationManager.AppSettings["SMTPEmailPassword"];
            //client.UseDefaultCredentials = false;
            //client.Credentials = new System.Net.NetworkCredential(SMTPEmail, SMTPEmailPassword);
            //MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, subject, sb.ToString());
            //reportEmail.BodyEncoding = UTF8Encoding.UTF8;
            //reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            //reportEmail.IsBodyHtml = true;
            //await client.SendMailAsync(reportEmail);

        }




        public async Task<bool> FindPOCreatorAndSendASNNotification1(BPCASNView1 ASNView, string OCRAmount, bool shipAndInvAmount, bool ShipInvOcrAmount, bool InvoiceNumber, string InvoiceNumberOCR, string GSTValue, bool GSTcheck)
        {
            try
            {
                List<BPCOFHeaderUserView> users = new List<BPCOFHeaderUserView>();
                var ofs = _dbContext.BPCOFHeaders.Where(x => x.Client == ASNView.Client && x.Company == ASNView.Company && x.Type == ASNView.Type && x.PatnerID == ASNView.PatnerID && ASNView.DocNumbers.Any(y => y == x.DocNumber)).ToList();
                foreach (var of in ofs)
                {
                    if (!string.IsNullOrEmpty(of.Email))
                    {
                        BPCOFHeaderUserView view = new BPCOFHeaderUserView();
                        view.DocNumber = of.DocNumber;
                        view.POCreator = of.POCreator;
                        view.Email = of.Email;
                        users.Add(view);
                    }
                }
                if (users.Count > 0)
                {
                    await SendASNNotificationMail1(ASNView, users,OCRAmount,shipAndInvAmount,ShipInvOcrAmount,InvoiceNumber,InvoiceNumberOCR,GSTValue,GSTcheck);
                }
                return true;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("FactRepository/FindPOCreatorAndSendASNNotification1:- " + ex.Message, ex);
                return false;
            }

        }
        public async Task<bool> FindPOCreatorAndSendASNNotification(BPCASNView ASNView)
        {
            try
            {
                List<BPCOFHeaderUserView> users = new List<BPCOFHeaderUserView>();
                var ofs = _dbContext.BPCOFHeaders.Where(x => x.Client == ASNView.Client && x.Company == ASNView.Company && x.Type == ASNView.Type && x.PatnerID == ASNView.PatnerID && x.DocNumber == ASNView.DocNumber).ToList();
                foreach (var of in ofs)
                {
                    if (!string.IsNullOrEmpty(of.Email))
                    {
                        BPCOFHeaderUserView view = new BPCOFHeaderUserView();
                        view.DocNumber = of.DocNumber;
                        view.POCreator = of.POCreator;
                        view.Email = of.Email;
                        users.Add(view);
                    }
                }
                if (users.Count > 0)
                {
                    await SendASNNotificationMail(ASNView, users);
                }
                return true;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("FactRepository/FindPOCreatorAndSendASNNotification:- " + ex.Message, ex);
                return false;
            }

        }

        public async Task<bool> FindPOCreatorAndSendASNApprovalNotification(BPCASNView ASNView, string OCRAmount, bool shipAndInvAmount, bool ShipInvOcrAmount, bool InvoiceNumber, string InvoiceNumberOCR, string GSTValue, bool GSTcheck,string SupplierGSTValue,bool SupplierGSTCheck)
        {
            try
            {
                List<BPCOFHeaderUserView> users = new List<BPCOFHeaderUserView>();
                var ofs = _dbContext.BPCOFHeaders.Where(x => x.Client == ASNView.Client && x.Company == ASNView.Company && x.Type == ASNView.Type && x.PatnerID == ASNView.PatnerID && x.DocNumber == ASNView.DocNumber).ToList();
                foreach (var of in ofs)
                {
                    if (!string.IsNullOrEmpty(of.Email))
                    {
                        BPCOFHeaderUserView view = new BPCOFHeaderUserView();
                        view.DocNumber = of.DocNumber;
                        view.POCreator = of.POCreator;
                        
                        view.Email = of.Email;
                        users.Add(view);
                    }
                }
                if (users.Count > 0)
                {
                    await SendASNApprovalNotificationMail(ASNView, users,OCRAmount,shipAndInvAmount,ShipInvOcrAmount,InvoiceNumber,InvoiceNumberOCR,GSTValue,GSTcheck, SupplierGSTValue, SupplierGSTCheck);
                }
                return true;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("FactRepository/FindPOCreatorAndSendASNNotification:- " + ex.Message, ex);
                return false;
            }

        }

        public async Task<bool> FindPOCreatorAndSendASNApprovalNotification1(BPCASNView1 ASNView, string OCRAmount, bool shipAndInvAmount, bool ShipInvOcrAmount, bool InvoiceNumber, string InvoiceNumberOCR, string GSTValue, bool GSTcheck, string SupplierGSTValue, bool SupplierGSTCheck)
        {
            try
            {
                List<BPCOFHeaderUserView> users = new List<BPCOFHeaderUserView>();
                var ofs = _dbContext.BPCOFHeaders.Where(x => x.Client == ASNView.Client && x.Company == ASNView.Company && x.Type == ASNView.Type && x.PatnerID == ASNView.PatnerID && ASNView.DocNumbers.Any(y => y == x.DocNumber)).ToList();
                foreach (var of in ofs)
                {
                    if (!string.IsNullOrEmpty(of.Email))
                    {
                        BPCOFHeaderUserView view = new BPCOFHeaderUserView();
                        view.DocNumber = of.DocNumber;
                        view.POCreator = of.POCreator;
                        view.Email = of.Email;
                        users.Add(view);
                    }
                }
                if (users.Count > 0)
                {
                    await SendASNApprovalNotificationMail1(ASNView, users,OCRAmount,shipAndInvAmount,ShipInvOcrAmount,InvoiceNumber,InvoiceNumberOCR ,GSTValue, GSTcheck,SupplierGSTValue,SupplierGSTCheck);
                }
                return true;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("FactRepository/FindPOCreatorAndSendASNNotification:- " + ex.Message, ex);
                return false;
            }

        }

        public async Task<bool> SendASNNotificationMail(BPCASNView ASNView, List<BPCOFHeaderUserView> users)
        {
            try
            {
                var STMPDetailsConfig = _configuration.GetSection("STMPDetails");
                string hostName = STMPDetailsConfig["Host"];
                string SMTPEmail = STMPDetailsConfig["Email"];
                //string SMTPEmailPassword = STMPDetailsConfig["Password"];
                string SMTPPort = STMPDetailsConfig["Port"];
                string siteURL = _configuration.GetValue<string>("SiteURL");

                var message = new MailMessage();
                string subject = "";
                StringBuilder sb = new StringBuilder();
                //UserName = string.IsNullOrEmpty(UserName) ? toEmail.Split('@')[0] : UserName;
                var body = @"<style>
                              h3 {
                               margin: 5px 0px !important;
                             }
                             table {
                                  /*font-family: arial, sans-serif;*/
                                  border-collapse: collapse;
                                  width: 100%;
                                  margin:20px 0px;
                                  }

                              td, th {
                                  border: 1px solid #dddddd;
                                  text-align: left;
                                  padding: 8px;
                                  }

                             tr:nth-child(even) {
                                  /*background-color: #dddddd;*/
                                  }
                             .lar{
                                  width: 25%;
                                 }
                             .lar1{
                                  width: 20%;
                                  }
                            </style>";


                body = body + $@"<html><head></head><body> <div style='border:1px solid #dbdbdb;'> <div style='padding: 20px 20px; background-color: #fff06769;text-align: center;font-family: Segoe UI;'> <p> 
                              <h2>Emami Vendor Portal</h2> </p> </div> <div style='font-family: Segoe UI'> <div style='padding: 20px 20px;border:1px solid white;background-color: white !important'> 
                              <p>Dear Sir/Madam,</p>";
                sb.Append(@body);

                string Plant = ASNView.Plant;
                var plantMaster = _dbContext.BPCPlantMasters.FirstOrDefault(x => x.PlantCode == ASNView.Plant);
                if (plantMaster != null)
                {
                    Plant = plantMaster.PlantText ?? plantMaster.PlantCode;
                }

                //body = body + $@"<p>ASN has been created for PO {ASNView.DocNumber}, Please find the details</p>

                //              <h3><u>ASN</u></h3> 
                //                             <span>Doc Number : </span>{ASNView.DocNumber}<br> 
                //                             <span>ASN Number : </span>{ASNView.ASNNumber}<br> 
                //                             <span>Departure date : </span>{(ASNView.DepartureDate.HasValue ? ASNView.DepartureDate.Value.ToString("dd/MM/yyyy") : "")}<br> 
                //                             <span>Arrival date : </span>{(ASNView.ArrivalDate.HasValue ? ASNView.ArrivalDate.Value.ToString("dd/MM/yyyy") : "")}<br> 
                //                             <span>Shipping agency : </span>{ASNView.ShippingAgency}<br> 
                //                             <span>Vessel number : </span>{ASNView.VessleNumber}<br> 
                //                             <span>Net weight : </span>{ASNView.NetWeight}<br> 
                //                             <span>Gross number : </span>{ASNView.GrossWeight}<br> 
                //                             <span>AWB number : </span>{ASNView.AWBNumber}<br><br> 

                //              <h3><u>Invoice</u></h3> 
                //                          <span>Invoice number : </span>{ASNView.InvoiceNumber}<br> 
                //                          <span>Invoice date : </span>{(ASNView.InvoiceDate.HasValue ? ASNView.InvoiceDate.Value.ToString("dd/MM/yyyy") : "")}<br> 
                //                          <span>Invoice value : </span>{(ASNView.InvoiceAmount)}<br><br> 

                //              <h3><u>Shipment details</u></h3>";
                //sb.Append(@body);

                //string table = @"<table>
                //                              <tr>
                //                                <th>Doc Number</th>
                //                                <th>Item</th>
                //                                <th>Material</th>
                //                                <th>Material Text</th>
                //                                <th>Delivery date</th>
                //                                <th>Order Qty</th>
                //                                <th>ASN Qty</th>
                //                              </tr>";
                //foreach (var item in ASNView.ASNItems)
                //{
                //    //var lic = (from tb in _ctx.LicenseMasters where tb.LicenseID == item.LicenseID select tb).FirstOrDefault();
                //    //var des = lic != null ? lic.Description : item.LicenseID.ToString();
                //    table += $@"<tr>
                //                         <td>{ASNView.DocNumber}</td>
                //                         <td>{item.Item}</td>
                //                         <td>{item.Material}</td>
                //                         <td class='lar'>{item.MaterialText}</td>
                //                         <td>{(item.DeliveryDate.HasValue ? item.DeliveryDate.Value.ToString("dd/MM/yyyy") : "")}</td>
                //                         <td>{item.OrderedQty}</td>
                //                         <td>{item.ASNQty}</td>
                //                        </tr>";
                //};
                //table += @"</table><br>";
                //sb.Append(table);

                var headContent = $@"<p>{ASNView.CreatedBy} - {ASNView.ModifiedBy} has successfully raised ASN – {ASNView.ASNNumber} against PO No-";
                sb.Append(headContent);

                var index = 0;

                foreach (var item in ASNView.ASNItems)
                {
                    if (index == ASNView.ASNItems.Count - 1)
                    {
                        var content = $@"{item.DocNumber}/{item.Item}";
                        sb.Append(content);
                    }
                    else
                    {
                        var content = $@"{item.DocNumber}/{item.Item} ,";
                        sb.Append(content);
                    }
                    index++;
                };

                var headContent1 = $@" with Invoiced Qty - ";
                sb.Append(headContent1);

                index = 0;

                foreach (var item in ASNView.ASNItems)
                {
                    if (index == ASNView.ASNItems.Count - 1)
                    {
                        var content = $@"{item.ASNQty}";
                        sb.Append(content);
                    }
                    else
                    {
                        var content = $@"{item.ASNQty} ,";
                        sb.Append(content);
                    }
                    index++;
                };

                var headContent2 = $@" at EMAMI LTD, {Plant} </p><br>";
                sb.Append(headContent2);

                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                sb.Append(regards);

                subject = "ASN Creation";
                SmtpClient client = new SmtpClient();
                client.Port = Convert.ToInt32(SMTPPort);
                client.Host = hostName;
                client.EnableSsl = false;
                client.Timeout = 60000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                //client.UseDefaultCredentials = false;
                //client.Credentials = new System.Net.NetworkCredential(SMTPEmail, SMTPEmailPassword);
                //MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, subject, sb.ToString());
                MailMessage reportEmail = new MailMessage();
                reportEmail.From = new MailAddress(SMTPEmail);
                var Emails = users.Select(x => x.Email).Distinct().ToList();
                foreach (var email in Emails)
                {
                    reportEmail.To.Add(email);
                }

                //foreach (var attach in attachments)
                //{
                //    Stream stream = new MemoryStream(attach.AttachmentFile);
                //    var attachment = new System.Net.Mail.Attachment(stream, attach.AttachmentName, attach.ContentType);
                //    reportEmail.Attachments.Add(attachment);
                //}
                reportEmail.Subject = subject;
                reportEmail.Body = sb.ToString();
                reportEmail.BodyEncoding = UTF8Encoding.UTF8;
                reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                reportEmail.IsBodyHtml = true;
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                await client.SendMailAsync(reportEmail);
                WriteLog.WriteToFile($"ASN Details sent successfully to {string.Join(",", Emails)}");
                return true;
            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        WriteLog.WriteToFile("ASNRepository/SendASNNotificationMail/MailboxBusy/MailboxUnavailable/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                    else
                    {
                        WriteLog.WriteToFile("ASNRepository/SendASNNotificationMail/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                }
                WriteLog.WriteToFile("ASNRepository/SendASNNotificationMail/SmtpFailedRecipientsException:- " + ex.Message, ex);
                return false;
            }
            catch (SmtpException ex)
            {
                WriteLog.WriteToFile("ASNRepository/SendASNNotificationMail/SmtpException:- " + ex.Message, ex);
                return false;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/SendASNNotificationMail", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/SendASNNotificationMail", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASNRepository/SendASNNotificationMail/Exception:- " + ex.Message, ex);
                return false;
            }
        }

        public async Task<bool> SendASNNotificationMail1(BPCASNView1 ASNView, List<BPCOFHeaderUserView> users, string OCRAmount, bool shipAndInvAmount, bool ShipInvOcrAmount, bool InvoiceNumber, string InvoiceNumberOCR, string GSTValue, bool GSTcheck)
        {
            try
            {
                var STMPDetailsConfig = _configuration.GetSection("STMPDetails");
                string hostName = STMPDetailsConfig["Host"];
                string SMTPEmail = STMPDetailsConfig["Email"];
                //string SMTPEmailPassword = STMPDetailsConfig["Password"];
                string SMTPPort = STMPDetailsConfig["Port"];
                string siteURL = _configuration.GetValue<string>("SiteURL");

                var message = new MailMessage();
                string subject = "";
                StringBuilder sb = new StringBuilder();
                //UserName = string.IsNullOrEmpty(UserName) ? toEmail.Split('@')[0] : UserName;
                var body = @"<style>
                              h3 {
                               margin: 5px 0px !important;
                             }
                             table {
                                  /*font-family: arial, sans-serif;*/
                                  border-collapse: collapse;
                                  width: 100%;
                                  margin:20px 0px;
                                  }

                              td, th {
                                  border: 1px solid #dddddd;
                                  text-align: left;
                                  padding: 8px;
                                  }

                             tr:nth-child(even) {
                                  /*background-color: #dddddd;*/
                                  }
                             .lar{
                                  width: 25%;
                                 }
                             .lar1{
                                  width: 20%;
                                  }
                            </style>";


                body = body + $@"<html><head></head><body> <div style='border:1px solid #dbdbdb;'> <div style='padding: 20px 20px; background-color: #fff06769;text-align: center;font-family: Segoe UI;'> <p> 
                              <h2>Emami Vendor Portal</h2> </p> </div> <div style='font-family: Segoe UI'> <div style='padding: 20px 20px;border:1px solid white;background-color: white !important'> 
                              <p>Dear Sir/Madam, </p>";
                sb.Append(@body);

                string Plant = ASNView.Plant;
                var plantMaster = _dbContext.BPCPlantMasters.FirstOrDefault(x => x.PlantCode == ASNView.Plant);
                if (plantMaster != null)
                {
                    Plant = plantMaster.PlantText ?? plantMaster.PlantCode;
                }

                //body = body + $@"</p> <p>ASN has been created for PO {(ASNView.DocNumbers.Count > 0 ? string.Join(",", ASNView.DocNumbers) : "")}, Please find the details</p>

                //              <h3><u>ASN</u></h3> 
                //                             <span>Doc Number : </span>{ASNView.DocNumber}<br> 
                //                             <span>ASN Number : </span>{ASNView.ASNNumber}<br> 
                //                             <span>Departure date : </span>{(ASNView.DepartureDate.HasValue ? ASNView.DepartureDate.Value.ToString("dd/MM/yyyy") : "")}<br> 
                //                             <span>Arrival date : </span>{(ASNView.ArrivalDate.HasValue ? ASNView.ArrivalDate.Value.ToString("dd/MM/yyyy") : "")}<br> 
                //                             <span>Shipping agency : </span>{ASNView.ShippingAgency}<br> 
                //                             <span>Vessel number : </span>{ASNView.VessleNumber}<br> 
                //                             <span>Net weight : </span>{ASNView.NetWeight}<br> 
                //                             <span>Gross number : </span>{ASNView.GrossWeight}<br> 
                //                             <span>AWB number : </span>{ASNView.AWBNumber}<br><br> 

                //              <h3><u>Invoice</u></h3> 
                //                          <span>Invoice number : </span>{ASNView.InvoiceNumber}<br> 
                //                          <span>Invoice date : </span>{(ASNView.InvoiceDate.HasValue ? ASNView.InvoiceDate.Value.ToString("dd/MM/yyyy") : "")}<br> 
                //                          <span>Invoice value : </span>{(ASNView.InvoiceAmount)}<br><br> 

                //              <h3><u>Shipment details</u></h3>";
                //sb.Append(@body);

                //string table = @"<table>
                //                              <tr>
                //                                <th>Doc Number</th>
                //                                <th>Item</th>
                //                                <th>Material</th>
                //                                <th>Material Text</th>
                //                                <th>Delivery date</th>
                //                                <th>Order Qty</th>
                //                                <th>ASN Qty</th>
                //                              </tr>";
                //foreach (var item in ASNView.ASNItems)
                //{
                //    //var lic = (from tb in _ctx.LicenseMasters where tb.LicenseID == item.LicenseID select tb).FirstOrDefault();
                //    //var des = lic != null ? lic.Description : item.LicenseID.ToString();
                //    table += $@"<tr>
                //                         <td>{item.DocNumber}</td>
                //                         <td>{item.Item}</td>
                //                         <td>{item.Material}</td>
                //                         <td class='lar'>{item.MaterialText}</td>
                //                         <td>{(item.DeliveryDate.HasValue ? item.DeliveryDate.Value.ToString("dd/MM/yyyy") : "")}</td>
                //                         <td>{item.OrderedQty}</td>
                //                         <td>{item.ASNQty}</td>
                //                        </tr>";
                //};
                //table += @"</table><br>";
                //sb.Append(table);
                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";

                var headContent = $@"<p>{ASNView.CreatedBy} - {ASNView.ModifiedBy} has successfully raised ASN – {ASNView.ASNNumber} against PO No-";
                sb.Append(headContent);

                var index = 0;

                foreach (var item in ASNView.ASNItems)
                {
                    if (index == ASNView.ASNItems.Count - 1)
                    {
                        var content = $@"{item.DocNumber}/{item.Item}";
                        sb.Append(content);
                    }
                    else
                    {
                        var content = $@"{item.DocNumber}/{item.Item} ,";
                        sb.Append(content);
                    }
                    index++;
                };

                var headContent1 = $@" with Invoiced Qty - ";
                sb.Append(headContent1);

                index = 0;

                foreach (var item in ASNView.ASNItems)
                {
                    if (index == ASNView.ASNItems.Count - 1)
                    {
                        var content = $@"{item.ASNQty}";
                        sb.Append(content);
                    }
                    else
                    {
                        var content = $@"{item.ASNQty} ,";
                        sb.Append(content);
                    }
                    index++;
                };

                var headContent2 = $@" at EMAMI LTD, {Plant} </p><br>";
                sb.Append(headContent2);

                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";
                sb.Append(regards);

                subject = "ASN Creation";
                SmtpClient client = new SmtpClient();
                client.Port = Convert.ToInt32(SMTPPort);
                client.Host = hostName;
                client.EnableSsl = false;
                client.Timeout = 60000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                //client.UseDefaultCredentials = false;
                //client.Credentials = new System.Net.NetworkCredential(SMTPEmail, SMTPEmailPassword);
                //MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, subject, sb.ToString());
                MailMessage reportEmail = new MailMessage();
                reportEmail.From = new MailAddress(SMTPEmail);
                var Emails = users.Select(x => x.Email).Distinct().ToList();
                foreach (var email in Emails)
                {
                    reportEmail.To.Add(email);
                }

                //foreach (var attach in attachments)
                //{
                //    Stream stream = new MemoryStream(attach.AttachmentFile);
                //    var attachment = new System.Net.Mail.Attachment(stream, attach.AttachmentName, attach.ContentType);
                //    reportEmail.Attachments.Add(attachment);
                //}
                reportEmail.Subject = subject;
                reportEmail.Body = sb.ToString();
                reportEmail.BodyEncoding = UTF8Encoding.UTF8;
                reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                reportEmail.IsBodyHtml = true;
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                await client.SendMailAsync(reportEmail);
                WriteLog.WriteToFile($"ASN Details sent successfully to {string.Join(",", Emails)}");
                return true;
            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        WriteLog.WriteToFile("ASNRepository/SendASNNotificationMail1/MailboxBusy/MailboxUnavailable/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                    else
                    {
                        WriteLog.WriteToFile("ASNRepository/SendASNNotificationMail1/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                }
                WriteLog.WriteToFile("ASNRepository/SendASNNotificationMail1/SmtpFailedRecipientsException:- " + ex.Message, ex);
                return false;
            }
            catch (SmtpException ex)
            {
                WriteLog.WriteToFile("ASNRepository/SendASNNotificationMail1/SmtpException:- " + ex.Message, ex);
                return false;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/SendASNNotificationMail1", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/SendASNNotificationMail1", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASNRepository/SendASNNotificationMail1/Exception:- " + ex.Message, ex);
                return false;
            }
        }

        public async Task<bool> SendASNApprovalNotificationMail(BPCASNView ASNView, List<BPCOFHeaderUserView> users, string OCRAmount, bool shipAndInvAmount, bool ShipInvOcrAmount, bool InvoiceNumber, string InvoiceNumberOCR, string GSTValue, bool GSTcheck,string SupplierGSTValue,bool SupplierGSTCheck)
        {
            try
            {
                var STMPDetailsConfig = _configuration.GetSection("STMPDetails");
                string hostName = STMPDetailsConfig["Host"];
                string SMTPEmail = STMPDetailsConfig["Email"];
                //string SMTPEmailPassword = STMPDetailsConfig["Password"];
                string SMTPPort = STMPDetailsConfig["Port"];
                string SiteURL = _configuration.GetValue<string>("SiteURL");

                var message = new MailMessage();
                string subject = "";
                StringBuilder sb = new StringBuilder();
                //UserName = string.IsNullOrEmpty(UserName) ? toEmail.Split('@')[0] : UserName;
                var body = @"<style>
                              h3 {
                               margin: 5px 0px !important;
                             }
                             table {
                                  /*font-family: arial, sans-serif;*/
                                  border-collapse: collapse;
                                  width: 100%;
                                  margin:20px 0px;
                                  }

                              td, th {
                                  border: 1px solid #dddddd;
                                  text-align: left;
                                  padding: 8px;
                                  }

                             tr:nth-child(even) {
                                  /*background-color: #dddddd;*/
                                  }
                             .lar{
                                  width: 25%;
                                 }
                             .lar1{
                                  width: 20%;
                                  }
                            </style>";


                body = body + $@"<html><head></head><body> <div style='border:1px solid #dbdbdb;'> <div style='padding: 20px 20px; background-color: #fff06769;text-align: center;font-family: Segoe UI;'> <p> 
                              <h2>Emami Vendor Portal</h2> </p> </div> <div style='font-family: Segoe UI'> <div style='padding: 20px 20px;border:1px solid white;background-color: white !important'> 
                              <p>Dear Sir/Madam,</p>";
                sb.Append(@body);

                //string Plant = ASNView.Plant;
                //var plantMaster = _dbContext.BPCPlantMasters.FirstOrDefault(x => x.PlantCode == ASNView.Plant);
                //if (plantMaster != null)
                //{
                //    Plant = plantMaster.PlantText ?? plantMaster.PlantCode;
                //}

                //body = body + $@"<p>ASN has been created for PO {ASNView.DocNumber}, Please find the details</p>

                //              <h3><u>ASN</u></h3> 
                //                             <span>Doc Number : </span>{ASNView.DocNumber}<br> 
                //                             <span>ASN Number : </span>{ASNView.ASNNumber}<br> 
                //                             <span>Departure date : </span>{(ASNView.DepartureDate.HasValue ? ASNView.DepartureDate.Value.ToString("dd/MM/yyyy") : "")}<br> 
                //                             <span>Arrival date : </span>{(ASNView.ArrivalDate.HasValue ? ASNView.ArrivalDate.Value.ToString("dd/MM/yyyy") : "")}<br> 
                //                             <span>Shipping agency : </span>{ASNView.ShippingAgency}<br> 
                //                             <span>Vessel number : </span>{ASNView.VessleNumber}<br> 
                //                             <span>Net weight : </span>{ASNView.NetWeight}<br> 
                //                             <span>Gross number : </span>{ASNView.GrossWeight}<br> 
                //                             <span>AWB number : </span>{ASNView.AWBNumber}<br><br> 

                //              <h3><u>Invoice</u></h3> 
                //                          <span>Invoice number : </span>{ASNView.InvoiceNumber}<br> 
                //                          <span>Invoice date : </span>{(ASNView.InvoiceDate.HasValue ? ASNView.InvoiceDate.Value.ToString("dd/MM/yyyy") : "")}<br> 
                //                          <span>Invoice value : </span>{(ASNView.InvoiceAmount)}<br><br> 

                //              <h3><u>Shipment details</u></h3>";
                //sb.Append(@body);

                //string table = @"<table>
                //                              <tr>
                //                                <th>Doc Number</th>
                //                                <th>Item</th>
                //                                <th>Material</th>
                //                                <th>Material Text</th>
                //                                <th>Delivery date</th>
                //                                <th>Order Qty</th>
                //                                <th>ASN Qty</th>
                //                              </tr>";
                //foreach (var item in ASNView.ASNItems)
                //{
                //    //var lic = (from tb in _ctx.LicenseMasters where tb.LicenseID == item.LicenseID select tb).FirstOrDefault();
                //    //var des = lic != null ? lic.Description : item.LicenseID.ToString();
                //    table += $@"<tr>
                //                         <td>{ASNView.DocNumber}</td>
                //                         <td>{item.Item}</td>
                //                         <td>{item.Material}</td>
                //                         <td class='lar'>{item.MaterialText}</td>
                //                         <td>{(item.DeliveryDate.HasValue ? item.DeliveryDate.Value.ToString("dd/MM/yyyy") : "")}</td>
                //                         <td>{item.OrderedQty}</td>
                //                         <td>{item.ASNQty}</td>
                //                        </tr>";
                //};
                //table += @"</table><br>";
                //sb.Append(table);

                //var headContent = $@"<p>{ASNView.CreatedBy} - {ASNView.ModifiedBy} has successfully raised ASN – {ASNView.ASNNumber} against PO No-";
               
                if (shipAndInvAmount)
                {
                    if (!InvoiceNumber && GSTcheck && !SupplierGSTCheck)
                    {
                        //ASN 66909117 for PO No.4500021345 and vendor code 70 has been saved as
                        var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value is greater than shipment value.</p>";
                        sb.Append(content);
                        var content1 = $@"<p>Invoice Number - {InvoiceNumberOCR} and Supplier GSTIN - {SupplierGSTValue} not match.</p>";
                        sb.Append(content1);
                        var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                        sb.Append(content2);

                        var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                        //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                        sb.Append(regards);
                    }
                    else if (!InvoiceNumber && !GSTcheck && !SupplierGSTCheck)
                    {
                        var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value is greater than shipment value.</p>";
                        sb.Append(content);
                        var content1 = $@"<p>Invoice Number - {InvoiceNumberOCR},Buyer GSTIN - {GSTValue} and Supplier GSTIN - {SupplierGSTValue} not match.</p>";
                        sb.Append(content1);
                        var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                        sb.Append(content2);

                        var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                        //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                        sb.Append(regards);
                    }
                    else if (InvoiceNumber && !GSTcheck && !SupplierGSTCheck)
                    {
                        var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value is greater than shipment value.</p>";
                        sb.Append(content);
                        var content1 = $@"<p>Buyer GSTIN - {GSTValue} and Supplier GSTIN - {SupplierGSTValue} not match.</p>";
                        sb.Append(content1);
                        var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                        sb.Append(content2);

                        var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                        //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                        sb.Append(regards);
                    }
                    else if (!InvoiceNumber && !GSTcheck &&  SupplierGSTCheck)
                    {
                        var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value is greater than shipment value.</p>";
                        sb.Append(content);
                        var content1 = $@"<p>Invoice Number - {InvoiceNumberOCR} and Buyer GSTIN - {GSTValue} not match.</p>";
                        sb.Append(content1);
                        var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                        sb.Append(content2);

                        var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                        //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                        sb.Append(regards);
                    }
                    else if (InvoiceNumber && !GSTcheck && SupplierGSTCheck)
                    {
                        var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value is greater than shipment value.</p>";
                        sb.Append(content);
                        var content1 = $@"<p>Buyer GSTIN - {GSTValue} not match.</p>";
                        sb.Append(content1);

                        var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                        sb.Append(content2);

                        var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                        //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                        sb.Append(regards);
                    }
                    else if (InvoiceNumber && GSTcheck && !SupplierGSTCheck)
                    {
                        var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value is greater than shipment value.</p>";
                        sb.Append(content);
                        var content1 = $@"<p>Supplier GSTIN - {SupplierGSTValue} not match.</p>";
                        sb.Append(content1);

                        var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                        sb.Append(content2);

                        var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                        //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                        sb.Append(regards);
                    }
                    else if (!InvoiceNumber && GSTcheck && SupplierGSTCheck)
                    {
                        var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value is greater than shipment value.</p>";
                        sb.Append(content);
                        var content1 = $@"<p>Invoice Number - {InvoiceNumberOCR} not match with given data.</p>";
                        sb.Append(content1);

                        var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                        sb.Append(content2);

                        var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                        //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                        sb.Append(regards);
                    }
                   
                   else
                    {
                        var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value is greater than shipment value.</p>";
                        sb.Append(content);

                        var content1 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                        sb.Append(content1);

                        var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                        //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                        sb.Append(regards);
                    }
                    

                }
                else if (ShipInvOcrAmount) {
                    if (!InvoiceNumber && GSTcheck && !SupplierGSTCheck)
                    {

                        var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value and shipment value are not match with invoice document amount value - {OCRAmount}.</p>";
                        sb.Append(content);
                        var content1 = $@"<p>Invoice Number - {InvoiceNumberOCR} and Supplier GSTIN - {SupplierGSTValue} not match.</p>";
                        sb.Append(content1);
                        var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                        sb.Append(content2);

                        var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                        //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                        sb.Append(regards);
                    }
                   else if (!InvoiceNumber && !GSTcheck && !SupplierGSTCheck)
                    {
                        var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value and shipment value are not match with invoice document amount value - {OCRAmount}.</p>";
                        sb.Append(content);
                        var content1 = $@"<p>Invoice Number - {InvoiceNumberOCR},Buyer GSTIN - {GSTValue} and Supplier GSTIN - {SupplierGSTValue} not match.</p>";
                        sb.Append(content1);
                        var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                        sb.Append(content2);

                        var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                        //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                        sb.Append(regards);
                    }
                    else if (InvoiceNumber && !GSTcheck && !SupplierGSTCheck)
                    {
                        var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value and shipment value are not match with invoice document amount value - {OCRAmount}.</p>";
                        sb.Append(content);
                        var content1 = $@"<p>Buyer GSTIN - {GSTValue} and Supplier GSTIN - {SupplierGSTValue} not match.</p>";
                        sb.Append(content1);
                        var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                        sb.Append(content2);

                        var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                        //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                        sb.Append(regards);
                    }
                    else if (!InvoiceNumber && !GSTcheck && SupplierGSTCheck)
                    {
                        var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value and shipment value are not match with invoice document amount value - {OCRAmount}.</p>";
                        sb.Append(content);
                        var content1 = $@"<p>Invoice Number - {InvoiceNumberOCR} and Buyer GSTIN - {GSTValue} not match.</p>";
                        sb.Append(content1);
                        var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                        sb.Append(content2);

                        var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                        //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                        sb.Append(regards);
                    }
                    else if (InvoiceNumber && !GSTcheck && SupplierGSTCheck)
                    {
                        var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value and shipment value are not match with invoice document amount value - {OCRAmount}.</p>";
                        sb.Append(content);
                        var content1 = $@"<p>Buyer GSTIN - {GSTValue} not match.</p>";
                        sb.Append(content1);

                        var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                        sb.Append(content2);

                        var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                        //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                        sb.Append(regards);
                    }
                    else if (!InvoiceNumber && GSTcheck && SupplierGSTCheck)
                    {
                        var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value and shipment value are not match with invoice document amount value - {OCRAmount}.</p>";
                        sb.Append(content);
                        var content1 = $@"<p>Invoice Number - {InvoiceNumberOCR} not match with given data.</p>";
                        sb.Append(content1);

                        var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                        sb.Append(content2);

                        var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                        //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                        sb.Append(regards);
                    }
                    else if (InvoiceNumber && GSTcheck && !SupplierGSTCheck)
                    {
                        var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value and shipment value are not match with invoice document amount value - {OCRAmount}.</p>";
                        sb.Append(content);
                        var content1 = $@"<p>Supplier GSTIN - {SupplierGSTValue} not match with given data.</p>";
                        sb.Append(content1);

                        var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                        sb.Append(content2);

                        var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                        //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                        sb.Append(regards);
                    }
                    else
                    {
                        var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value and shipment value are not match with invoice document amount value - {OCRAmount}.</p>";
                        sb.Append(content);

                        var content1 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                        sb.Append(content1);

                        var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                        //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                        sb.Append(regards);
                    }
                    
                }
                if (InvoiceNumber && GSTcheck && !ShipInvOcrAmount && !shipAndInvAmount && !SupplierGSTCheck)
                {
                    var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since Supplier GSTIN - {SupplierGSTValue} not match with given data</p>";
                    sb.Append(content);

                    var content1 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                    sb.Append(content1);

                    var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                    //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                    sb.Append(regards);
                }
                else if (!InvoiceNumber && GSTcheck && !ShipInvOcrAmount && !shipAndInvAmount && !SupplierGSTCheck)
                {

                    var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice Number - {InvoiceNumberOCR} and Supplier GSTIN - {SupplierGSTValue} not match with given data.</p>";
                    sb.Append(content);
                    var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                    sb.Append(content2);

                    var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                    //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                    sb.Append(regards);
                }
                else if (!InvoiceNumber && !GSTcheck && !ShipInvOcrAmount && !shipAndInvAmount && !SupplierGSTCheck)
                {

                    var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice Number - {InvoiceNumberOCR},Buyer GSTIN - {GSTValue} and Supplier GSTIN - {SupplierGSTValue} not match with given data.</p>";
                    sb.Append(content);
                    var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                    sb.Append(content2);

                    var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                    //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                    sb.Append(regards);
                }
                else if (!InvoiceNumber && !GSTcheck && !ShipInvOcrAmount && !shipAndInvAmount && SupplierGSTCheck)
                {
                    
                    var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice Number - {InvoiceNumberOCR} and Buyer GSTIN - {GSTValue} not match with given data.</p>";
                    sb.Append(content);
                    var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                    sb.Append(content2);

                    var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                    //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                    sb.Append(regards);
                }
                else if (InvoiceNumber && !GSTcheck && !ShipInvOcrAmount && !shipAndInvAmount && SupplierGSTCheck)
                {
                    var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since Buyer GSTIN - {GSTValue} not match with given data</p>";
                    sb.Append(content);

                    var content1 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                    sb.Append(content1);

                    var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                    //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                    sb.Append(regards);
                }
                else if (!InvoiceNumber && GSTcheck && !ShipInvOcrAmount && !shipAndInvAmount && SupplierGSTCheck)
                {
                    var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice Number - {InvoiceNumberOCR} not match with given data</p>";
                    sb.Append(content);

                    var content1 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                    sb.Append(content1);

                    var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                    //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                    sb.Append(regards);
                }
              

                //subject = "ASN Creation";
                subject = "ASN Approval - Alert";
                SmtpClient client = new SmtpClient();
                client.Port = Convert.ToInt32(SMTPPort);
                client.Host = hostName;
                client.EnableSsl = false;
                client.Timeout = 60000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                //client.UseDefaultCredentials = false;
                //client.Credentials = new System.Net.NetworkCredential(SMTPEmail, SMTPEmailPassword);
                //MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, subject, sb.ToString());
                MailMessage reportEmail = new MailMessage();
                reportEmail.From = new MailAddress(SMTPEmail);
                var Emails = users.Select(x => x.Email).Distinct().ToList();
                foreach (var email in Emails)
                {
                    reportEmail.To.Add(email);
                }

                //foreach (var attach in attachments)
                //{
                //    Stream stream = new MemoryStream(attach.AttachmentFile);
                //    var attachment = new System.Net.Mail.Attachment(stream, attach.AttachmentName, attach.ContentType);
                //    reportEmail.Attachments.Add(attachment);
                //}
                reportEmail.Subject = subject;
                reportEmail.Body = sb.ToString();
                reportEmail.BodyEncoding = UTF8Encoding.UTF8;
                reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                reportEmail.IsBodyHtml = true;
                
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                await client.SendMailAsync(reportEmail);
                WriteLog.WriteToFile($"ASN Approval request has been sent successfully to {string.Join(",", Emails)}");
                return true;
            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        WriteLog.WriteToFile("ASNRepository/SendASNNotificationMail/MailboxBusy/MailboxUnavailable/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                    else
                    {
                        WriteLog.WriteToFile("ASNRepository/SendASNNotificationMail/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                }
                WriteLog.WriteToFile("ASNRepository/SendASNNotificationMail/SmtpFailedRecipientsException:- " + ex.Message, ex);
                return false;
            }
            catch (SmtpException ex)
            {
                WriteLog.WriteToFile("ASNRepository/SendASNNotificationMail/SmtpException:- " + ex.Message, ex);
                return false;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/SendASNNotificationMail", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/SendASNNotificationMail", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASNRepository/SendASNNotificationMail/Exception:- " + ex.Message, ex);
                return false;
            }
        }

        public async Task<bool> SendASNApprovalNotificationMail1(BPCASNView1 ASNView, List<BPCOFHeaderUserView> users, string OCRAmount, bool shipAndInvAmount, bool ShipInvOcrAmount, bool InvoiceNumber, string InvoiceNumberOCR, string GSTValue, bool GSTcheck, string SupplierGSTValue, bool SupplierGSTCheck)
        {
            try
            {
                var STMPDetailsConfig = _configuration.GetSection("STMPDetails");
                string hostName = STMPDetailsConfig["Host"];
                string SMTPEmail = STMPDetailsConfig["Email"];
                //string SMTPEmailPassword = STMPDetailsConfig["Password"];
                string SMTPPort = STMPDetailsConfig["Port"];
                string SiteURL = _configuration.GetValue<string>("SiteURL");
               
                var message = new MailMessage();
                string subject = "";
                StringBuilder sb = new StringBuilder();
                //UserName = string.IsNullOrEmpty(UserName) ? toEmail.Split('@')[0] : UserName;
                var body = @"<style>
                              h3 {
                               margin: 5px 0px !important;
                             }
                             table {
                                  /*font-family: arial, sans-serif;*/
                                  border-collapse: collapse;
                                  width: 100%;
                                  margin:20px 0px;
                                  }

                              td, th {
                                  border: 1px solid #dddddd;
                                  text-align: left;
                                  padding: 8px;
                                  }

                             tr:nth-child(even) {
                                  /*background-color: #dddddd;*/
                                  }
                             .lar{
                                  width: 25%;
                                 }
                             .lar1{
                                  width: 20%;
                                  }
                            </style>";


                body = body + $@"<html><head></head><body> <div style='border:1px solid #dbdbdb;'> <div style='padding: 20px 20px; background-color: #fff06769;text-align: center;font-family: Segoe UI;'> <p> 
                              <h2>Emami Vendor Portal</h2> </p> </div> <div style='font-family: Segoe UI'> <div style='padding: 20px 20px;border:1px solid white;background-color: white !important'> 
                              <p>Dear Sir/Madam,</p>";
                sb.Append(@body);

                //string Plant = ASNView.Plant;
                //var plantMaster = _dbContext.BPCPlantMasters.FirstOrDefault(x => x.PlantCode == ASNView.Plant);
                //if (plantMaster != null)
                //{
                //    Plant = plantMaster.PlantText ?? plantMaster.PlantCode;
                //}

                //body = body + $@"<p>ASN has been created for PO {ASNView.DocNumber}, Please find the details</p>

                //              <h3><u>ASN</u></h3> 
                //                             <span>Doc Number : </span>{ASNView.DocNumber}<br> 
                //                             <span>ASN Number : </span>{ASNView.ASNNumber}<br> 
                //                             <span>Departure date : </span>{(ASNView.DepartureDate.HasValue ? ASNView.DepartureDate.Value.ToString("dd/MM/yyyy") : "")}<br> 
                //                             <span>Arrival date : </span>{(ASNView.ArrivalDate.HasValue ? ASNView.ArrivalDate.Value.ToString("dd/MM/yyyy") : "")}<br> 
                //                             <span>Shipping agency : </span>{ASNView.ShippingAgency}<br> 
                //                             <span>Vessel number : </span>{ASNView.VessleNumber}<br> 
                //                             <span>Net weight : </span>{ASNView.NetWeight}<br> 
                //                             <span>Gross number : </span>{ASNView.GrossWeight}<br> 
                //                             <span>AWB number : </span>{ASNView.AWBNumber}<br><br> 

                //              <h3><u>Invoice</u></h3> 
                //                          <span>Invoice number : </span>{ASNView.InvoiceNumber}<br> 
                //                          <span>Invoice date : </span>{(ASNView.InvoiceDate.HasValue ? ASNView.InvoiceDate.Value.ToString("dd/MM/yyyy") : "")}<br> 
                //                          <span>Invoice value : </span>{(ASNView.InvoiceAmount)}<br><br> 

                //              <h3><u>Shipment details</u></h3>";
                //sb.Append(@body);

                //string table = @"<table>
                //                              <tr>
                //                                <th>Doc Number</th>
                //                                <th>Item</th>
                //                                <th>Material</th>
                //                                <th>Material Text</th>
                //                                <th>Delivery date</th>
                //                                <th>Order Qty</th>
                //                                <th>ASN Qty</th>
                //                              </tr>";
                //foreach (var item in ASNView.ASNItems)
                //{
                //    //var lic = (from tb in _ctx.LicenseMasters where tb.LicenseID == item.LicenseID select tb).FirstOrDefault();
                //    //var des = lic != null ? lic.Description : item.LicenseID.ToString();
                //    table += $@"<tr>
                //                         <td>{ASNView.DocNumber}</td>
                //                         <td>{item.Item}</td>
                //                         <td>{item.Material}</td>
                //                         <td class='lar'>{item.MaterialText}</td>
                //                         <td>{(item.DeliveryDate.HasValue ? item.DeliveryDate.Value.ToString("dd/MM/yyyy") : "")}</td>
                //                         <td>{item.OrderedQty}</td>
                //                         <td>{item.ASNQty}</td>
                //                        </tr>";
                //};
                //table += @"</table><br>";
                //sb.Append(table);

                //var headContent = $@"<p>{ASNView.CreatedBy} - {ASNView.ModifiedBy} has successfully raised ASN – {ASNView.ASNNumber} against PO No-";

                //string ASNitem = "";
                //if (ASNView.ASNItems.Count == 1)
                //{
                //    ASNitem = ASNView.ASNItems[0].Item.ToString();
                    
                //}
                //else
                //{
                //    for (int i = 0; i < ASNView.ASNItems.Count; i++)
                //    {
                //        ASNitem = String.Join(",", ASNView.ASNItems[i].Item);
                //    }
                //}
                if (shipAndInvAmount)
                {
                    if(ASNView.DocNumber != "")
                    {
                        if (!InvoiceNumber && GSTcheck && !SupplierGSTCheck)
                        {
                            var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value is greater than shipment value.</p>";
                            sb.Append(content);
                            var content1 = $@"<p>Invoice Number - {InvoiceNumberOCR} and Supplier GSTIN - {SupplierGSTValue} not match.</p>";
                            sb.Append(content1);
                            var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                            sb.Append(content2);

                            var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                            //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                            sb.Append(regards);
                        }
                        else if (!InvoiceNumber && !GSTcheck && !SupplierGSTCheck)
                        {
                            var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value is greater than shipment value.</p>";
                            sb.Append(content);
                            var content1 = $@"<p>Invoice Number - {InvoiceNumberOCR},Buyer GSTIN - {GSTValue} and Supplier GSTIN - {SupplierGSTValue} not match.</p>";
                            sb.Append(content1);
                            var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                            sb.Append(content2);

                            var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                            //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                            sb.Append(regards);
                        }
                        else if (InvoiceNumber && !GSTcheck && !SupplierGSTCheck)
                        {
                            var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value is greater than shipment value.</p>";
                            sb.Append(content);
                            var content1 = $@"<p>Buyer GSTIN - {GSTValue} and Supplier GSTIN - {SupplierGSTValue} not match.</p>";
                            sb.Append(content1);
                            var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                            sb.Append(content2);

                            var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                            //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                            sb.Append(regards);
                        }
                        else if (!InvoiceNumber && !GSTcheck && SupplierGSTCheck)
                        {
                            var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value is greater than shipment value.</p>";
                            sb.Append(content);
                            var content1 = $@"<p>Invoice Number - {InvoiceNumberOCR} and Buyer GSTIN - {GSTValue} not match.</p>";
                            sb.Append(content1);
                            var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                            sb.Append(content2);

                            var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                            //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                            sb.Append(regards);
                        }
                        else if (InvoiceNumber && !GSTcheck && SupplierGSTCheck)
                        {
                            var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value is greater than shipment value.</p>";
                            sb.Append(content);
                            var content1 = $@"<p>Buyer GSTIN - {GSTValue} not match.</p>";
                            sb.Append(content1);

                            var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                            sb.Append(content2);

                            var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                            //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                            sb.Append(regards);
                        }
                        else if (InvoiceNumber && GSTcheck && !SupplierGSTCheck)
                        {
                            var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value is greater than shipment value.</p>";
                            sb.Append(content);
                            var content1 = $@"<p>Supplier GSTIN - {SupplierGSTValue} not match.</p>";
                            sb.Append(content1);

                            var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                            sb.Append(content2);

                            var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                            //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                            sb.Append(regards);
                        }
                        else if (!InvoiceNumber && GSTcheck && SupplierGSTCheck)
                        {
                            var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value is greater than shipment value.</p>";
                            sb.Append(content);
                            var content1 = $@"<p>Invoice Number - {InvoiceNumberOCR} not match with given data.</p>";
                            sb.Append(content1);

                            var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                            sb.Append(content2);

                            var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                            //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                            sb.Append(regards);
                        }

                        else
                        {
                            var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value is greater than shipment value.</p>";
                            sb.Append(content);

                            var content1 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                            sb.Append(content1);

                            var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                            //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                            sb.Append(regards);
                        }

                    }

                    else if (ASNView.DocNumber == "")
                    {
                        if(ASNView.DocNumbers.Count == 1)
                        {
                            if (!InvoiceNumber && GSTcheck && !SupplierGSTCheck)
                            {
                              
                                var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumbers[0]} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value is greater than shipment value.</p>";
                                sb.Append(content);
                                var content1 = $@"<p>Invoice Number - {InvoiceNumberOCR} and Supplier GSTIN - {SupplierGSTValue} not match.</p>";
                                sb.Append(content1);
                                var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                                sb.Append(content2);

                                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                                sb.Append(regards);
                            }
                            else if (!InvoiceNumber && !GSTcheck && !SupplierGSTCheck)
                            {
                               
                                var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumbers[0]} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value is greater than shipment value.</p>";
                                sb.Append(content);
                                var content1 = $@"<p>Invoice Number - {InvoiceNumberOCR},Buyer GSTIN - {GSTValue} and Supplier GSTIN - {SupplierGSTValue} not match.</p>";
                                sb.Append(content1);
                                var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                                sb.Append(content2);

                                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                                sb.Append(regards);
                            }
                            else if (InvoiceNumber && !GSTcheck && !SupplierGSTCheck)
                            {
                                
                                var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumbers[0]} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value is greater than shipment value.</p>";
                                sb.Append(content);
                                var content1 = $@"<p>Buyer GSTIN - {GSTValue} and Supplier GSTIN - {SupplierGSTValue} not match.</p>";
                                sb.Append(content1);
                                var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                                sb.Append(content2);

                                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                                sb.Append(regards);
                            }
                            else if (!InvoiceNumber && !GSTcheck && SupplierGSTCheck)
                            {
                                
                                var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumbers[0]} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value is greater than shipment value.</p>";
                                sb.Append(content);
                                var content1 = $@"<p>Invoice Number - {InvoiceNumberOCR} and Buyer GSTIN - {GSTValue} not match.</p>";
                                sb.Append(content1);
                                var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                                sb.Append(content2);

                                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                                sb.Append(regards);
                            }
                            else if (InvoiceNumber && !GSTcheck && SupplierGSTCheck)
                            {
                                
                                var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumbers[0]} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value is greater than shipment value.</p>";
                                sb.Append(content);
                                var content1 = $@"<p>Buyer GSTIN - {GSTValue} not match.</p>";
                                sb.Append(content1);

                                var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                                sb.Append(content2);

                                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                                sb.Append(regards);
                            }
                            else if (InvoiceNumber && GSTcheck && !SupplierGSTCheck)
                            {
                                
                                var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumbers[0]} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value is greater than shipment value.</p>";
                                sb.Append(content);
                                var content1 = $@"<p>Supplier GSTIN - {SupplierGSTValue} not match.</p>";
                                sb.Append(content1);

                                var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                                sb.Append(content2);

                                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                                sb.Append(regards);
                            }
                            else if (!InvoiceNumber && GSTcheck && SupplierGSTCheck)
                            {
                               
                                var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumbers[0]} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value is greater than shipment value.</p>";
                                sb.Append(content);
                                var content1 = $@"<p>Invoice Number - {InvoiceNumberOCR} not match with given data.</p>";
                                sb.Append(content1);

                                var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                                sb.Append(content2);

                                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                                sb.Append(regards);
                            }

                            else
                            {
                             
                                var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumbers[0]} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value is greater than shipment value.</p>";
                                sb.Append(content);

                                var content1 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                                sb.Append(content1);

                                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                                sb.Append(regards);
                            }

                        }
                        else
                        {
                            var PONumbers = String.Join(",", ASNView.DocNumbers);
                            if (!InvoiceNumber && GSTcheck && !SupplierGSTCheck)
                            {
                                var str1 = String.Join(",", ASNView.DocNumbers);
                                var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {PONumbers} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value is greater than shipment value.</p>";
                                sb.Append(content);
                                var content1 = $@"<p>Invoice Number - {InvoiceNumberOCR} and Supplier GSTIN - {SupplierGSTValue} not match.</p>";
                                sb.Append(content1);
                                var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                                sb.Append(content2);

                                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                                sb.Append(regards);
                            }
                            else if (!InvoiceNumber && !GSTcheck && !SupplierGSTCheck)
                            {
                                var str1 = String.Join(",", ASNView.DocNumbers);
                                var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {PONumbers} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value is greater than shipment value.</p>";
                                sb.Append(content);
                                var content1 = $@"<p>Invoice Number - {InvoiceNumberOCR},Buyer GSTIN - {GSTValue} and Supplier GSTIN - {SupplierGSTValue} not match.</p>";
                                sb.Append(content1);
                                var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                                sb.Append(content2);

                                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                                sb.Append(regards);
                            }
                            else if (InvoiceNumber && !GSTcheck && !SupplierGSTCheck)
                            {
                                var str1 = String.Join(",", ASNView.DocNumbers);
                                var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {PONumbers} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value is greater than shipment value.</p>";
                                sb.Append(content);
                                var content1 = $@"<p>Buyer GSTIN - {GSTValue} and Supplier GSTIN - {SupplierGSTValue} not match.</p>";
                                sb.Append(content1);
                                var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                                sb.Append(content2);

                                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                                sb.Append(regards);
                            }
                            else if (!InvoiceNumber && !GSTcheck && SupplierGSTCheck)
                            {
                                var str1 = String.Join(",", ASNView.DocNumbers);
                                var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {PONumbers} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value is greater than shipment value.</p>";
                                sb.Append(content);
                                var content1 = $@"<p>Invoice Number - {InvoiceNumberOCR} and Buyer GSTIN - {GSTValue} not match.</p>";
                                sb.Append(content1);
                                var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                                sb.Append(content2);

                                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                                sb.Append(regards);
                            }
                            else if (InvoiceNumber && !GSTcheck && SupplierGSTCheck)
                            {
                                var str1 = String.Join(",", ASNView.DocNumbers);
                                var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {PONumbers} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value is greater than shipment value.</p>";
                                sb.Append(content);
                                var content1 = $@"<p>Buyer GSTIN - {GSTValue} not match.</p>";
                                sb.Append(content1);

                                var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                                sb.Append(content2);

                                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                                sb.Append(regards);
                            }
                            else if (InvoiceNumber && GSTcheck && !SupplierGSTCheck)
                            {
                                var str1 = String.Join(",", ASNView.DocNumbers);
                                var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {PONumbers} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value is greater than shipment value.</p>";
                                sb.Append(content);
                                var content1 = $@"<p>Supplier GSTIN - {SupplierGSTValue} not match.</p>";
                                sb.Append(content1);

                                var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                                sb.Append(content2);

                                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                                sb.Append(regards);
                            }
                            else if (!InvoiceNumber && GSTcheck && SupplierGSTCheck)
                            {
                                var str1 = String.Join(",", ASNView.DocNumbers);
                                var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {PONumbers} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value is greater than shipment value.</p>";
                                sb.Append(content);
                                var content1 = $@"<p>Invoice Number - {InvoiceNumberOCR} not match with given data.</p>";
                                sb.Append(content1);

                                var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                                sb.Append(content2);

                                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                                sb.Append(regards);
                            }

                            else
                            {
                                
                                var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {PONumbers} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value is greater than shipment value.</p>";
                                sb.Append(content);

                                var content1 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                                sb.Append(content1);

                                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                                sb.Append(regards);
                            }
                        }
                    }
                }
                else if (ShipInvOcrAmount)
                {
                    if (ASNView.DocNumber != "")
                    {
                        if (!InvoiceNumber && GSTcheck && !SupplierGSTCheck)
                        {
                            var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value and shipment value are not match with invoice document amount value - {OCRAmount}.</p>";
                            sb.Append(content);
                            var content1 = $@"<p>Invoice Number - {InvoiceNumberOCR} and Supplier GSTIN - {SupplierGSTValue} not match.</p>";
                            sb.Append(content1);
                            var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                            sb.Append(content2);

                            var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                            //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                            sb.Append(regards);
                        }
                        else if (!InvoiceNumber && !GSTcheck && !SupplierGSTCheck)
                        {
                            var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value and shipment value are not match with invoice document amount value - {OCRAmount}.</p>";
                            sb.Append(content);
                            var content1 = $@"<p>Invoice Number - {InvoiceNumberOCR},Buyer GSTIN - {GSTValue} and Supplier GSTIN - {SupplierGSTValue} not match.</p>";
                            sb.Append(content1);
                            var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                            sb.Append(content2);

                            var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                            //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                            sb.Append(regards);
                        }
                        else if (InvoiceNumber && !GSTcheck && !SupplierGSTCheck)
                        {
                            var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value and shipment value are not match with invoice document amount value - {OCRAmount}.</p>";
                            sb.Append(content);
                            var content1 = $@"<p>Buyer GSTIN - {GSTValue} and Supplier GSTIN - {SupplierGSTValue} not match.</p>";
                            sb.Append(content1);
                            var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                            sb.Append(content2);

                            var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                            //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                            sb.Append(regards);
                        }
                        else if (!InvoiceNumber && !GSTcheck && SupplierGSTCheck)
                        {
                            var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value and shipment value are not match with invoice document amount value - {OCRAmount}.</p>";
                            sb.Append(content);
                            var content1 = $@"<p>Invoice Number - {InvoiceNumberOCR} and Buyer GSTIN - {GSTValue} not match.</p>";
                            sb.Append(content1);
                            var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                            sb.Append(content2);

                            var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                            //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                            sb.Append(regards);
                        }
                        else if (InvoiceNumber && !GSTcheck && SupplierGSTCheck)
                        {
                            var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value and shipment value are not match with invoice document amount value - {OCRAmount}.</p>";
                            sb.Append(content);
                            var content1 = $@"<p>Buyer GSTIN - {GSTValue} not match.</p>";
                            sb.Append(content1);

                            var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                            sb.Append(content2);

                            var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                            //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                            sb.Append(regards);
                        }
                        else if (!InvoiceNumber && GSTcheck && SupplierGSTCheck)
                        {
                            var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value and shipment value are not match with invoice document amount value - {OCRAmount}.</p>";
                            sb.Append(content);
                            var content1 = $@"<p>Invoice Number - {InvoiceNumberOCR} not match with given data.</p>";
                            sb.Append(content1);

                            var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                            sb.Append(content2);

                            var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                            //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                            sb.Append(regards);
                        }
                        else if (InvoiceNumber && GSTcheck && !SupplierGSTCheck)
                        {
                            var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value and shipment value are not match with invoice document amount value - {OCRAmount}.</p>";
                            sb.Append(content);
                            var content1 = $@"<p>Supplier GSTIN - {SupplierGSTValue} not match with given data.</p>";
                            sb.Append(content1);

                            var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                            sb.Append(content2);

                            var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                            //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                            sb.Append(regards);
                        }
                        else
                        {
                            var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value and shipment value are not match with invoice document amount value - {OCRAmount}.</p>";
                            sb.Append(content);

                            var content1 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                            sb.Append(content1);

                            var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                            //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                            sb.Append(regards);
                        }

                    }
                    else if (ASNView.DocNumber == "")
                    {
                        if (ASNView.DocNumbers.Count == 1)
                        {
                            if (!InvoiceNumber && GSTcheck && !SupplierGSTCheck)
                            {
                                var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumbers[0]} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value and shipment value are not match with invoice document amount value - {OCRAmount}.</p>";
                                sb.Append(content);
                                var content1 = $@"<p>Invoice Number - {InvoiceNumberOCR} and Supplier GSTIN - {SupplierGSTValue} not match.</p>";
                                sb.Append(content1);
                                var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                                sb.Append(content2);

                                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                                sb.Append(regards);
                            }
                            else if (!InvoiceNumber && !GSTcheck && !SupplierGSTCheck)
                            {
                                var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumbers[0]} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value and shipment value are not match with invoice document amount value - {OCRAmount}.</p>";
                                sb.Append(content);
                                var content1 = $@"<p>Invoice Number - {InvoiceNumberOCR},Buyer GSTIN - {GSTValue} and Supplier GSTIN - {SupplierGSTValue} not match.</p>";
                                sb.Append(content1);
                                var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                                sb.Append(content2);

                                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                                sb.Append(regards);
                            }
                            else if (InvoiceNumber && !GSTcheck && !SupplierGSTCheck)
                            {
                                var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumbers[0]} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value and shipment value are not match with invoice document amount value - {OCRAmount}.</p>";
                                sb.Append(content);
                                var content1 = $@"<p>Buyer GSTIN - {GSTValue} and Supplier GSTIN - {SupplierGSTValue} not match.</p>";
                                sb.Append(content1);
                                var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                                sb.Append(content2);

                                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                                sb.Append(regards);
                            }
                            else if (!InvoiceNumber && !GSTcheck && SupplierGSTCheck)
                            {
                                var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumbers[0]} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value and shipment value are not match with invoice document amount value - {OCRAmount}.</p>";
                                sb.Append(content);
                                var content1 = $@"<p>Invoice Number - {InvoiceNumberOCR} and Buyer GSTIN - {GSTValue} not match.</p>";
                                sb.Append(content1);
                                var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                                sb.Append(content2);

                                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                                sb.Append(regards);
                            }
                            else if (InvoiceNumber && !GSTcheck && SupplierGSTCheck)
                            {
                                var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumbers[0]} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value and shipment value are not match with invoice document amount value - {OCRAmount}.</p>";
                                sb.Append(content);
                                var content1 = $@"<p>Buyer GSTIN - {GSTValue} not match.</p>";
                                sb.Append(content1);

                                var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                                sb.Append(content2);

                                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                                sb.Append(regards);
                            }
                            else if (!InvoiceNumber && GSTcheck && SupplierGSTCheck)
                            {
                                var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumbers[0]} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value and shipment value are not match with invoice document amount value - {OCRAmount}.</p>";
                                sb.Append(content);
                                var content1 = $@"<p>Invoice Number - {InvoiceNumberOCR} not match with given data.</p>";
                                sb.Append(content1);

                                var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                                sb.Append(content2);

                                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                                sb.Append(regards);
                            }
                            else if (InvoiceNumber && GSTcheck && !SupplierGSTCheck)
                            {
                                var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumbers[0]} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value and shipment value are not match with invoice document amount value - {OCRAmount}.</p>";
                                sb.Append(content);
                                var content1 = $@"<p>Supplier GSTIN - {SupplierGSTValue} not match with given data.</p>";
                                sb.Append(content1);

                                var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                                sb.Append(content2);

                                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                                sb.Append(regards);
                            }
                            else
                            {
                                var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumbers[0]} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value and shipment value are not match with invoice document amount value - {OCRAmount}.</p>";
                                sb.Append(content);

                                var content1 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                                sb.Append(content1);

                                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                                sb.Append(regards);
                            }

                        }
                        else
                        {
                            var PONumbers = String.Join(",", ASNView.DocNumbers);
                            if (!InvoiceNumber && GSTcheck && !SupplierGSTCheck)
                            {
                                var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {PONumbers} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value and shipment value are not match with invoice document amount value - {OCRAmount}.</p>";
                                sb.Append(content);
                                var content1 = $@"<p>Invoice Number - {InvoiceNumberOCR} and Supplier GSTIN - {SupplierGSTValue} not match.</p>";
                                sb.Append(content1);
                                var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                                sb.Append(content2);

                                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                                sb.Append(regards);
                            }
                            else if (!InvoiceNumber && !GSTcheck && !SupplierGSTCheck)
                            {
                                var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {PONumbers} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value and shipment value are not match with invoice document amount value - {OCRAmount}.</p>";
                                sb.Append(content);
                                var content1 = $@"<p>Invoice Number - {InvoiceNumberOCR},Buyer GSTIN - {GSTValue} and Supplier GSTIN - {SupplierGSTValue} not match.</p>";
                                sb.Append(content1);
                                var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                                sb.Append(content2);

                                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                                sb.Append(regards);
                            }
                            else if (InvoiceNumber && !GSTcheck && !SupplierGSTCheck)
                            {
                                var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {PONumbers} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value and shipment value are not match with invoice document amount value - {OCRAmount}.</p>";
                                sb.Append(content);
                                var content1 = $@"<p>Buyer GSTIN - {GSTValue} and Supplier GSTIN - {SupplierGSTValue} not match.</p>";
                                sb.Append(content1);
                                var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                                sb.Append(content2);

                                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                                sb.Append(regards);
                            }
                            else if (!InvoiceNumber && !GSTcheck && SupplierGSTCheck)
                            {
                                var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {PONumbers} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value and shipment value are not match with invoice document amount value - {OCRAmount}.</p>";
                                sb.Append(content);
                                var content1 = $@"<p>Invoice Number - {InvoiceNumberOCR} and Buyer GSTIN - {GSTValue} not match.</p>";
                                sb.Append(content1);
                                var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                                sb.Append(content2);

                                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                                sb.Append(regards);
                            }
                            else if (InvoiceNumber && !GSTcheck && SupplierGSTCheck)
                            {
                                var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {PONumbers} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value and shipment value are not match with invoice document amount value - {OCRAmount}.</p>";
                                sb.Append(content);
                                var content1 = $@"<p>Buyer GSTIN - {GSTValue} not match.</p>";
                                sb.Append(content1);

                                var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                                sb.Append(content2);

                                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                                sb.Append(regards);
                            }
                            else if (!InvoiceNumber && GSTcheck && SupplierGSTCheck)
                            {
                                var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {PONumbers} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value and shipment value are not match with invoice document amount value - {OCRAmount}.</p>";
                                sb.Append(content);
                                var content1 = $@"<p>Invoice Number - {InvoiceNumberOCR} not match with given data.</p>";
                                sb.Append(content1);

                                var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                                sb.Append(content2);

                                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                                sb.Append(regards);
                            }
                            else if (InvoiceNumber && GSTcheck && !SupplierGSTCheck)
                            {
                                var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {PONumbers} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value and shipment value are not match with invoice document amount value - {OCRAmount}.</p>";
                                sb.Append(content);
                                var content1 = $@"<p>Supplier GSTIN - {SupplierGSTValue} not match with given data.</p>";
                                sb.Append(content1);

                                var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                                sb.Append(content2);

                                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                                sb.Append(regards);
                            }
                            else
                            {
                                var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {PONumbers} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice value and shipment value are not match with invoice document amount value - {OCRAmount}.</p>";
                                sb.Append(content);

                                var content1 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                                sb.Append(content1);

                                var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                                //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                                sb.Append(regards);
                            }

                        }
                    }
                   

                }
                if (ASNView.DocNumber != "")
                {
                    if (InvoiceNumber && GSTcheck && !ShipInvOcrAmount && !shipAndInvAmount && !SupplierGSTCheck)
                    {
                        var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since Supplier GSTIN - {SupplierGSTValue} not match with given data</p>";
                        sb.Append(content);

                        var content1 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                        sb.Append(content1);

                        var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                        //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                        sb.Append(regards);
                    }
                    else if (!InvoiceNumber && GSTcheck && !ShipInvOcrAmount && !shipAndInvAmount && !SupplierGSTCheck)
                    {

                        var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice Number - {InvoiceNumberOCR} and Supplier GSTIN - {SupplierGSTValue} not match with given data.</p>";
                        sb.Append(content);
                        var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                        sb.Append(content2);

                        var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                        //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                        sb.Append(regards);
                    }
                    else if (!InvoiceNumber && !GSTcheck && !ShipInvOcrAmount && !shipAndInvAmount && !SupplierGSTCheck)
                    {

                        var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice Number - {InvoiceNumberOCR},Buyer GSTIN - {GSTValue} and Supplier GSTIN - {SupplierGSTValue} not match with given data.</p>";
                        sb.Append(content);
                        var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                        sb.Append(content2);

                        var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                        //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                        sb.Append(regards);
                    }
                    else if (!InvoiceNumber && !GSTcheck && !ShipInvOcrAmount && !shipAndInvAmount && SupplierGSTCheck)
                    {

                        var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice Number - {InvoiceNumberOCR} and Buyer GSTIN - {GSTValue} not match with given data.</p>";
                        sb.Append(content);
                        var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                        sb.Append(content2);

                        var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                        //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                        sb.Append(regards);
                    }
                    else if (InvoiceNumber && !GSTcheck && !ShipInvOcrAmount && !shipAndInvAmount && SupplierGSTCheck)
                    {
                        var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since Buyer GSTIN - {GSTValue} not match with given data</p>";
                        sb.Append(content);

                        var content1 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                        sb.Append(content1);

                        var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                        //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                        sb.Append(regards);
                    }
                    else if (!InvoiceNumber && GSTcheck && !ShipInvOcrAmount && !shipAndInvAmount && SupplierGSTCheck)
                    {
                        var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumber} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice Number - {InvoiceNumberOCR} not match with given data</p>";
                        sb.Append(content);

                        var content1 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                        sb.Append(content1);

                        var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                        //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                        sb.Append(regards);
                    }

                }
                else if(ASNView.DocNumber == "")
                {
                    if(ASNView.DocNumbers.Count == 1)
                    {
                        if (InvoiceNumber && GSTcheck && !ShipInvOcrAmount && !shipAndInvAmount && !SupplierGSTCheck)
                        {
                            var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumbers[0]} and vendor code  {ASNView.PatnerID} has been saved as draft, since Supplier GSTIN - {SupplierGSTValue} not match with given data</p>";
                            sb.Append(content);

                            var content1 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                            sb.Append(content1);

                            var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                            //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                            sb.Append(regards);
                        }
                        else if (!InvoiceNumber && GSTcheck && !ShipInvOcrAmount && !shipAndInvAmount && !SupplierGSTCheck)
                        {

                            var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumbers[0]} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice Number - {InvoiceNumberOCR} and Supplier GSTIN - {SupplierGSTValue} not match with given data.</p>";
                            sb.Append(content);
                            var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                            sb.Append(content2);

                            var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                            //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                            sb.Append(regards);
                        }
                        else if (!InvoiceNumber && !GSTcheck && !ShipInvOcrAmount && !shipAndInvAmount && !SupplierGSTCheck)
                        {

                            var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumbers[0]} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice Number - {InvoiceNumberOCR},Buyer GSTIN - {GSTValue} and Supplier GSTIN - {SupplierGSTValue} not match with given data.</p>";
                            sb.Append(content);
                            var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                            sb.Append(content2);

                            var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                            //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                            sb.Append(regards);
                        }
                        else if (!InvoiceNumber && !GSTcheck && !ShipInvOcrAmount && !shipAndInvAmount && SupplierGSTCheck)
                        {

                            var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumbers[0]} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice Number - {InvoiceNumberOCR} and Buyer GSTIN - {GSTValue} not match with given data.</p>";
                            sb.Append(content);
                            var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                            sb.Append(content2);

                            var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                            //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                            sb.Append(regards);
                        }
                        else if (InvoiceNumber && !GSTcheck && !ShipInvOcrAmount && !shipAndInvAmount && SupplierGSTCheck)
                        {
                            var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumbers[0]} and vendor code  {ASNView.PatnerID} has been saved as draft, since Buyer GSTIN - {GSTValue} not match with given data</p>";
                            sb.Append(content);

                            var content1 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                            sb.Append(content1);

                            var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                            //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                            sb.Append(regards);
                        }
                        else if (!InvoiceNumber && GSTcheck && !ShipInvOcrAmount && !shipAndInvAmount && SupplierGSTCheck)
                        {
                            var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {ASNView.DocNumbers[0]} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice Number - {InvoiceNumberOCR} not match with given data</p>";
                            sb.Append(content);

                            var content1 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                            sb.Append(content1);

                            var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                            //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                            sb.Append(regards);
                        }

                    }
                    else
                    {
                        var PONumbers = String.Join(",", ASNView.DocNumbers);
                        if (InvoiceNumber && GSTcheck && !ShipInvOcrAmount && !shipAndInvAmount && !SupplierGSTCheck)
                        {
                            var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {PONumbers} and vendor code  {ASNView.PatnerID} has been saved as draft, since Supplier GSTIN - {SupplierGSTValue} not match with given data</p>";
                            sb.Append(content);

                            var content1 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                            sb.Append(content1);

                            var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                            //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                            sb.Append(regards);
                        }
                        else if (!InvoiceNumber && GSTcheck && !ShipInvOcrAmount && !shipAndInvAmount && !SupplierGSTCheck)
                        {

                            var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {PONumbers} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice Number - {InvoiceNumberOCR} and Supplier GSTIN - {SupplierGSTValue} not match with given data.</p>";
                            sb.Append(content);
                            var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                            sb.Append(content2);

                            var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                            //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                            sb.Append(regards);
                        }
                        else if (!InvoiceNumber && !GSTcheck && !ShipInvOcrAmount && !shipAndInvAmount && !SupplierGSTCheck)
                        {

                            var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {PONumbers} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice Number - {InvoiceNumberOCR},Buyer GSTIN - {GSTValue} and Supplier GSTIN - {SupplierGSTValue} not match with given data.</p>";
                            sb.Append(content);
                            var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                            sb.Append(content2);

                            var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                            //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                            sb.Append(regards);
                        }
                        else if (!InvoiceNumber && !GSTcheck && !ShipInvOcrAmount && !shipAndInvAmount && SupplierGSTCheck)
                        {

                            var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {PONumbers} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice Number - {InvoiceNumberOCR} and Buyer GSTIN - {GSTValue} not match with given data.</p>";
                            sb.Append(content);
                            var content2 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                            sb.Append(content2);

                            var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                            //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                            sb.Append(regards);
                        }
                        else if (InvoiceNumber && !GSTcheck && !ShipInvOcrAmount && !shipAndInvAmount && SupplierGSTCheck)
                        {
                            var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {PONumbers} and vendor code  {ASNView.PatnerID} has been saved as draft, since Buyer GSTIN - {GSTValue} not match with given data</p>";
                            sb.Append(content);

                            var content1 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                            sb.Append(content1);

                            var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                            //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                            sb.Append(regards);
                        }
                        else if (!InvoiceNumber && GSTcheck && !ShipInvOcrAmount && !shipAndInvAmount && SupplierGSTCheck)
                        {
                            var content = $@"<p>ASN  {ASNView.ASNNumber} for PO No. {PONumbers} and vendor code  {ASNView.PatnerID} has been saved as draft, since invoice Number - {InvoiceNumberOCR} not match with given data</p>";
                            sb.Append(content);

                            var content1 = @"<p>Kindly approve the ASN to process the dispatch details.</p>
                                  Please Click  " + "<a href=\"" + SiteURL + "/#/auth/login\"" + "> here</a> to login the portal<br><br>";
                            sb.Append(content1);

                            var regards = @"<p>Thanks and Regards,</p> <p>Emami Ltd</p> 
                                <p>This mail is generated automatically. Please do not reply</p>
                                </div> </div> </div></body></html>";

                            //var regards = "<p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                            sb.Append(regards);
                        }

                    }

                }
               


                //subject = "ASN Creation";
                subject = "ASN Approval - Alert ";
                SmtpClient client = new SmtpClient();
                client.Port = Convert.ToInt32(SMTPPort);
                client.Host = hostName;
                client.EnableSsl = false;
                client.Timeout = 60000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                //client.UseDefaultCredentials = false;
                //client.Credentials = new System.Net.NetworkCredential(SMTPEmail, SMTPEmailPassword);
                //MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, subject, sb.ToString());
                MailMessage reportEmail = new MailMessage();
                reportEmail.From = new MailAddress(SMTPEmail);
                var Emails = users.Select(x => x.Email).Distinct().ToList();
                foreach (var email in Emails)
                {
                    reportEmail.To.Add(email);
                }

                //foreach (var attach in attachments)
                //{
                //    Stream stream = new MemoryStream(attach.AttachmentFile);
                //    var attachment = new System.Net.Mail.Attachment(stream, attach.AttachmentName, attach.ContentType);
                //    reportEmail.Attachments.Add(attachment);
                //}
                reportEmail.Subject = subject;
                reportEmail.Body = sb.ToString();
                reportEmail.BodyEncoding = UTF8Encoding.UTF8;
                reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                reportEmail.IsBodyHtml = true;
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                await client.SendMailAsync(reportEmail);
                WriteLog.WriteToFile($"ASN Approval request has been sent successfully to {string.Join(",", Emails)}");
                return true;
            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        WriteLog.WriteToFile("ASNRepository/SendASNNotificationMail/MailboxBusy/MailboxUnavailable/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                    else
                    {
                        WriteLog.WriteToFile("ASNRepository/SendASNNotificationMail/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                }
                WriteLog.WriteToFile("ASNRepository/SendASNNotificationMail/SmtpFailedRecipientsException:- " + ex.Message, ex);
                return false;
            }
            catch (SmtpException ex)
            {
                WriteLog.WriteToFile("ASNRepository/SendASNNotificationMail/SmtpException:- " + ex.Message, ex);
                return false;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/SendASNNotificationMail", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/SendASNNotificationMail", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASNRepository/SendASNNotificationMail/Exception:- " + ex.Message, ex);
                return false;
            }
        }


        public async Task<BPCASNHeader> DeleteASN(BPCASNHeader ASN)
        {
            try
            {
                bool IsNonZeroOpenQty = false;
                //var entity = await _dbContext.Set<BPCASNHeader>().FindAsync(ASN.ASN, ASN.Language);
                var entity = _dbContext.Set<BPCASNHeader>().FirstOrDefault(x => x.ASNNumber == ASN.ASNNumber);
                if (entity == null)
                {
                    return entity;
                }

                var result = _dbContext.Set<BPCASNHeader>().Remove(entity);
                WriteLog.WriteToFile($"ASN deleted successfully {string.Join(" ", entity.ASNNumber)}");
                List<BPCASNItem> items = _dbContext.BPCASNItems.Where(x => x.ASNNumber == entity.ASNNumber).ToList();
                foreach (BPCASNItem item in items)
                {
                    BPCOFItem bPCOFItem = await UpdateDeletedPOItemQty(item, ASN.DocNumber);
                    if (bPCOFItem.OpenQty.HasValue && bPCOFItem.OpenQty.Value > 0)
                    {
                        IsNonZeroOpenQty = true;
                    }
                    _dbContext.BPCASNItems.Remove(item);
                }
                List<BPCASNItemBatch> batches = _dbContext.BPCASNItemBatches.Where(x => x.ASNNumber == entity.ASNNumber).ToList();
                List<BPCASNItemSES> ses = _dbContext.BPCASNItemSESes.Where(x => x.ASNNumber == entity.ASNNumber).ToList();
                foreach (BPCASNItemBatch batch in batches)
                {
                    _dbContext.BPCASNItemBatches.Remove(batch);
                }
                foreach (BPCASNItemSES se in ses)
                {
                    _dbContext.BPCASNItemSESes.Remove(se);
                }
                _dbContext.BPCASNPacks.Where(x => x.ASNNumber == entity.ASNNumber).ToList().ForEach(x => _dbContext.BPCASNPacks.Remove(x));
                _dbContext.BPCDocumentCenters.Where(x => x.ASNNumber == entity.ASNNumber).ToList().ForEach(x => _dbContext.BPCDocumentCenters.Remove(x));
                var ASNs = _dbContext.Set<BPCASNHeader>().Where(x => x.DocNumber == ASN.DocNumber && x.Status != "Cancelled").ToList();
                if (IsNonZeroOpenQty)
                {
                    if (ASNs.Count > 0)
                    {
                        await UpdatePOStatus(ASN, "PartialASN");
                    }
                    else
                    {
                        await UpdatePOStatus(ASN, "DueForASN");
                    }
                }
                
                await _dbContext.SaveChangesAsync();
                return entity;

            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/DeleteASN", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/DeleteASN", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCASNHeader1> DeleteASN1(BPCASNHeader1 ASN)
        {
            try
            {
                bool IsNonZeroOpenQty = false;
                //var entity = await _dbContext.Set<BPCASNHeader>().FindAsync(ASN.ASN, ASN.Language);
                var entity = _dbContext.Set<BPCASNHeader1>().FirstOrDefault(x => x.ASNNumber == ASN.ASNNumber);
                if (entity == null)
                {
                    return entity;
                }

                var result = _dbContext.Set<BPCASNHeader1>().Remove(entity);
                WriteLog.WriteToFile($"ASN deleted successfully {string.Join(" ", entity.ASNNumber)}");
                List<BPCASNItem1> items = _dbContext.BPCASNItem1.Where(x => x.ASNNumber == entity.ASNNumber).ToList();
                foreach (BPCASNItem1 item in items)
                {
                    BPCOFItem bPCOFItem = await UpdateDeletedPOItemQty1(item, ASN.DocNumber);
                    if (bPCOFItem.OpenQty.HasValue && bPCOFItem.OpenQty.Value > 0)
                    {
                        IsNonZeroOpenQty = true;
                    }
                    _dbContext.BPCASNItem1.Remove(item);
                }
                List<BPCASNItemBatch1> batches = _dbContext.BPCASNItemBatch1.Where(x => x.ASNNumber == entity.ASNNumber).ToList();
                List<BPCASNItemSES1> ses = _dbContext.BPCASNItemSES1.Where(x => x.ASNNumber == entity.ASNNumber).ToList();
                foreach (BPCASNItemBatch1 batch in batches)
                {
                    _dbContext.BPCASNItemBatch1.Remove(batch);
                }
                foreach (BPCASNItemSES1 se in ses)
                {
                    _dbContext.BPCASNItemSES1.Remove(se);
                }
                _dbContext.BPCASNPacks.Where(x => x.ASNNumber == entity.ASNNumber).ToList().ForEach(x => _dbContext.BPCASNPacks.Remove(x));
                _dbContext.BPCDocumentCenters.Where(x => x.ASNNumber == entity.ASNNumber).ToList().ForEach(x => _dbContext.BPCDocumentCenters.Remove(x));
                //var ASNs = (from tb in _dbContext.BPCASNHeader1
                //             join tb1 in _dbContext.BPCASNOFMap1 on ASN.ASNNumber equals tb1.ASNNumber
                //             where ASN.ASNNumber == tb1.ASNNumber && tb.Status != "Cancelled"
                //            select tb).ToList();
                var ASNs = _dbContext.BPCASNOFMap1.Where(x => x.ASNNumber == ASN.ASNNumber).ToList();
                //ASNs = _dbContext.Set<BPCASNHeader1>().Where(x => x.DocNumber == ASN.DocNumber && x.Status != "Cancelled").ToList();
                if (IsNonZeroOpenQty)
                {
                    if (ASNs.Count > 0)
                    {
                        foreach(BPCASNOFMap1 ASNValue in ASNs)
                        {
                            await UpdatePOStatus1(ASNValue, "PartialASN");
                        }
                       
                    }
                    else
                    {
                        foreach (BPCASNOFMap1 ASNValue in ASNs)
                        {
                            await UpdatePOStatus1(ASNValue, "DueForASN");
                        }
                    }
                }
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/DeleteASN", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/DeleteASN", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CancelASN(List<CancelASNView> ASNs)
        {
            var log = new BPCLog();
            try
            {
                bool IsNonZeroOpenQty = false;
                log = await _poRepository.CreateBPCLog("CancelASN", ASNs.Count);
                //var entity = await _dbContext.Set<BPCASNHeader>().FindAsync(ASN.ASN, ASN.Language);
                foreach (var ASN in ASNs)
                {
                    var entity = _dbContext.Set<BPCASNHeader>().FirstOrDefault(x => x.Client == ASN.Client && x.Company == ASN.Company && x.Type == ASN.Type &&
                                                                               x.PatnerID == ASN.PatnerID && x.DocNumber == ASN.DocNumber && x.ASNNumber == ASN.ASNNumber);
                    if (entity != null)
                    {
                        entity.Status = "Cancelled";
                        entity.ModifiedBy = "SAP API";
                        entity.ModifiedOn = DateTime.Now;
                        await _dbContext.SaveChangesAsync();

                        var asnItems = _dbContext.BPCASNItems.Where(x => x.Client == ASN.Client && x.Company == ASN.Company && x.Type == ASN.Type &&
                                                                    x.PatnerID == ASN.PatnerID && x.ASNNumber == ASN.ASNNumber).ToList();
                        foreach (var item in asnItems)
                        {
                            BPCOFItem bPCOFItem = await UpdateDeletedPOItemQty(item, ASN.DocNumber);
                            if (bPCOFItem.OpenQty.HasValue && bPCOFItem.OpenQty.Value > 0)
                            {
                                IsNonZeroOpenQty = true;
                            }
                        }
                        var ASNss = _dbContext.Set<BPCASNHeader>().Where(x => x.DocNumber == ASN.DocNumber && x.Status != "Cancelled").ToList();
                        if (IsNonZeroOpenQty)
                        {
                            if (ASNss.Count > 0)
                            {
                                await UpdatePOStatus(entity, "PartialASN");
                            }
                            else
                            {
                                await UpdatePOStatus(entity, "DueForASN");
                            }
                        }
                    }
                    else
                    {
                        var entity1 = _dbContext.Set<BPCASNHeader1>().FirstOrDefault(x => x.Client == ASN.Client && x.Company == ASN.Company && x.Type == ASN.Type &&
                                                                                     x.PatnerID == ASN.PatnerID && x.ASNNumber == ASN.ASNNumber);
                        if (entity1 != null)
                        {
                            entity1.Status = "Cancelled";
                            entity1.ModifiedBy = "SAP API";
                            entity1.ModifiedOn = DateTime.Now;
                            await _dbContext.SaveChangesAsync();

                            var asnItems = _dbContext.BPCASNItem1.Where(x => x.Client == ASN.Client && x.Company == ASN.Company && x.Type == ASN.Type &&
                                                                             x.PatnerID == ASN.PatnerID && x.ASNNumber == ASN.ASNNumber).ToList();
                            foreach (var item in asnItems)
                            {
                                BPCOFItem bPCOFItem = await UpdateDeletedPOItemQty1(item);
                                if (bPCOFItem.OpenQty.HasValue && bPCOFItem.OpenQty.Value > 0)
                                {
                                    IsNonZeroOpenQty = true;
                                }

                                //var ASNss = _dbContext.Set<BPCASNHeader1>().Where(x => item.DocNumber == ASN.DocNumber && x.Status != "Cancelled").ToList();
                                var ASNss = (from tb in _dbContext.BPCASNHeader1
                                             join tb1 in _dbContext.BPCASNOFMap1 on tb.ASNNumber equals tb1.ASNNumber
                                             where tb1.DocNumber == item.DocNumber && tb.Status != "Cancelled"
                                             select tb).ToList();
                                if (IsNonZeroOpenQty)
                                {
                                    if (ASNss.Count > 0)
                                    {
                                        await UpdatePOStatus1(entity1, item.DocNumber, "PartialASN");
                                    }
                                    else
                                    {
                                        await UpdatePOStatus1(entity1, item.DocNumber, "DueForASN");
                                    }
                                }
                            }

                        }
                    }
                }
                if (log != null)
                {
                    await _poRepository.UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToFile("ASN/CancelASN--" + "Unable to generate Log");
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/CancelASN", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/CancelASN", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                if (log != null)
                {
                    await _poRepository.UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToFile("ASN/CancelGate--" + "Unable to generate Log");
                }
                throw ex;
            }
        }

        public async Task CancelGate(List<CancelASNView> ASNs)
        {
            var log = new BPCLog();
            try
            {
                bool IsNonZeroOpenQty = false;
                log = await _poRepository.CreateBPCLog("CancelGate", ASNs.Count);
                //var entity = await _dbContext.Set<BPCASNHeader>().FindAsync(ASN.ASN, ASN.Language);
                foreach (var ASN in ASNs)
                {
                    //var entity = _dbContext.Set<BPCASNHeader>().FirstOrDefault(x => x.ASNNumber == ASN.ASNNumber);
                    //if (entity != null)
                    //{
                    //    entity.Status = "Cancelled";
                    //    await _dbContext.SaveChangesAsync();
                    //}
                    //var asnItems = _dbContext.BPCASNItems.Where(x => x.ASNNumber == ASN.ASNNumber).ToList();
                    //foreach (var item in asnItems)
                    //{
                    //    await UpdateDeletedPOItemQty(item, ASN.DocNumber);
                    //}
                    var entity1 = _dbContext.Set<BPCGateEntry>().FirstOrDefault(x => x.Client == ASN.Client && x.Company == ASN.Company && x.Type == ASN.Type &&
                    x.PatnerID == ASN.PatnerID && x.DocNumber == ASN.DocNumber && x.ASNNumber == ASN.ASNNumber);
                    if (entity1 != null)
                    {
                        entity1.Status = "Cancelled";
                        entity1.ModifiedBy = "SAP API";
                        entity1.ModifiedOn = DateTime.Now;
                        var header = _dbContext.BPCOFHeaders.Where(x => x.Client == ASN.Client && x.Type == ASN.Type && x.Company == ASN.Company && x.PatnerID == ASN.PatnerID && x.DocNumber == ASN.DocNumber).FirstOrDefault();
                        var items = _dbContext.BPCOFItems.Where(x => x.Client == ASN.Client && x.Type == ASN.Type && x.Company == ASN.Company && x.PatnerID == ASN.PatnerID && x.DocNumber == ASN.DocNumber).ToList();
                        var ASNheader = _dbContext.BPCASNHeaders.Where(x => x.Client == ASN.Client && x.Type == ASN.Type && x.Company == ASN.Company && x.PatnerID == ASN.PatnerID && x.ASNNumber == ASN.ASNNumber && x.DocNumber == ASN.DocNumber).FirstOrDefault();
                        //var asnItems = _dbContext.BPCASNItems.Where(x => x.Client == ASN.Client && x.Company == ASN.Company && x.Type == ASN.Type &&
                        //                                                 x.PatnerID == ASN.PatnerID && x.ASNNumber == ASN.ASNNumber).ToList();
                        //foreach (var item in asnItems)
                        //{
                        //    BPCOFItem bPCOFItem = await UpdateDeletedPOItemQty(item, ASN.DocNumber);
                        //    if (bPCOFItem.OpenQty.HasValue && bPCOFItem.OpenQty.Value > 0)
                        //    {
                        //        IsNonZeroOpenQty = true;
                        //    }
                        //}
                        foreach (var item in items)
                        {
                            if (item.OpenQty.HasValue && item.OpenQty.Value > 0)
                            {
                                IsNonZeroOpenQty = true;
                            }
                        }
                        header.Status = IsNonZeroOpenQty ? "PartialASN" : "DueForGate";
                        ASNheader.Status = "GateEntry";
                        ASNheader.ModifiedBy = "SAP API";
                        ASNheader.ModifiedOn = DateTime.Now;
                        await _dbContext.SaveChangesAsync();
                    }
                }
                if (log != null)
                {
                    await _poRepository.UpdateBPCSucessLog(log.LogID);
                }
                else
                {
                    WriteLog.WriteToFile("ASN/CancelGate--" + "Unable to generate Log");
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/CancelGate", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/CancelGate", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                if (log != null)
                {
                    await _poRepository.UpdateBPCFailureLog(log.LogID, ex.Message);
                }
                else
                {
                    WriteLog.WriteToFile("ASN/CancelGate--" + "Unable to generate Log");
                }
                throw ex;
            }
        }


        //public async Task CancelGateEntryByAsnList(ASNListView Asn)
        //{
        //    try
        //    {
        //        var header = _dbContext.BPCOFHeaders.Where(x => x.Client == Asn.Client && x.Type == Asn.Type && x.Company == Asn.Company && x.PatnerID == Asn.PatnerID && x.DocNumber == Asn.DocNumber).FirstOrDefault();
        //        var ASNheader = _dbContext.BPCASNHeaders.Where(x => x.Client == Asn.Client && x.Type == Asn.Type && x.Company == Asn.Company && x.PatnerID == Asn.PatnerID && x.ASNNumber == Asn.ASNNumber && x.DocNumber == Asn.DocNumber).FirstOrDefault();
        //        var Gate = _dbContext.GateHV.Where(x => x.Client == Asn.Client && x.Type == Asn.Type && x.Company == Asn.Company && x.PatnerID == Asn.PatnerID && x.DocNo == Asn.DocNumber).FirstOrDefault();

        //        _dbContext.GateHV.Remove(Gate);
        //        header.Status = "DueForGate";
        //        ASNheader.Status = "GateEntry";
        //        await _dbContext.SaveChangesAsync();
        //    }
        //    catch (SqlException ex){ WriteLog.WriteToFile("ASNRepository/", ex); throw new Exception("Something went wrong");}catch(InvalidOperationException ex){WriteLog.WriteToFile("ASNRepository/", ex); throw new Exception("Something went wrong"); }catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        public List<BPCASNFieldMaster> GetAllASNFieldMaster()
        {
            try
            {
                return _dbContext.BPCASNFieldMasters.ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/GetAllASNFieldMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/GetAllASNFieldMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASNFieldMasterRepository/GetAllASNFieldMaster : - ", ex);
                throw ex;
            }
        }

        public BPCASNPreShipmentMaster GetASNPreShipmentMaster()
        {
            try
            {
                return _dbContext.BPCASNPreShipmentMasters.FirstOrDefault();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/GetASNPreShipmentMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/GetASNPreShipmentMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASNFieldMasterRepository/GetASNPreShipmentMaster : - ", ex);
                throw ex;
            }
        }

        public List<BPCASNPreShipmentMaster> GetAllASNPreShipmentMasters()
        {
            try
            {
                var result = (from tb in _dbContext.BPCASNPreShipmentMasters
                              where tb.IsActive
                              select tb).ToList();
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/GetAllASNPreShipmentMasters", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/GetAllASNPreShipmentMasters", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/GetAllASNPreShipmentMasters : - ", ex);
                return null;

            }
        }


        public async Task<BPCASNPreShipmentMaster> CreateASNPreShipmentMaster(BPCASNPreShipmentMaster ASNPreShipmentMaster)
        {
            BPCASNPreShipmentMaster ASNPreShipmentMasterResult = new BPCASNPreShipmentMaster();
            try
            {
                BPCASNPreShipmentMaster ASNPreShipmentMaster1 = (from tb in _dbContext.BPCASNPreShipmentMasters
                                                                 where tb.IsActive
                                                                 select tb).FirstOrDefault();
                if (ASNPreShipmentMaster1 == null)
                {
                    ASNPreShipmentMaster.CreatedOn = DateTime.Now;
                    ASNPreShipmentMaster.IsActive = true;
                    var result = _dbContext.BPCASNPreShipmentMasters.Add(ASNPreShipmentMaster);
                    await _dbContext.SaveChangesAsync();

                    ASNPreShipmentMasterResult.ID = ASNPreShipmentMaster.ID;
                    ASNPreShipmentMasterResult.PreShipmentDay = ASNPreShipmentMaster.PreShipmentDay;
                }
                else
                {
                    return ASNPreShipmentMasterResult;
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/CreateASNPreShipmentMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/CreateASNPreShipmentMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Master/CreateASNPreShipmentMaster : - ", ex);
                return null;
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
            return ASNPreShipmentMasterResult;
        }

        public async Task<BPCASNPreShipmentMaster> UpdateASNPreShipmentMaster(BPCASNPreShipmentMaster ASNPreShipmentMaster)
        {
            BPCASNPreShipmentMaster ASNPreShipmentMasterResult = new BPCASNPreShipmentMaster();
            try
            {
                BPCASNPreShipmentMaster ASNPreShipmentMaster1 = (from tb in _dbContext.BPCASNPreShipmentMasters
                                                                 where tb.IsActive && tb.ID != ASNPreShipmentMaster.ID
                                                                 select tb).FirstOrDefault();
                if (ASNPreShipmentMaster1 == null)
                {
                    BPCASNPreShipmentMaster ASNPreShipmentMaster2 = (from tb in _dbContext.BPCASNPreShipmentMasters
                                                                     where tb.IsActive && tb.ID == ASNPreShipmentMaster.ID
                                                                     select tb).FirstOrDefault();
                    ASNPreShipmentMaster2.PreShipmentDay = ASNPreShipmentMaster.PreShipmentDay;
                    ASNPreShipmentMaster2.IsActive = true;
                    ASNPreShipmentMaster2.ModifiedOn = DateTime.Now;
                    ASNPreShipmentMaster2.ModifiedBy = ASNPreShipmentMaster.ModifiedBy;
                    await _dbContext.SaveChangesAsync();
                    ASNPreShipmentMasterResult.ID = ASNPreShipmentMaster.ID;
                    ASNPreShipmentMasterResult.PreShipmentDay = ASNPreShipmentMaster.PreShipmentDay;
                }
                else
                {
                    return ASNPreShipmentMasterResult;
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/UpdateASNPreShipmentMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/UpdateASNPreShipmentMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                WriteLog.WriteToFile("Master/UpdateASNPreShipmentMaster : - ", ex);
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
            return ASNPreShipmentMasterResult;
        }

        public async Task<BPCASNPreShipmentMaster> DeleteASNPreShipmentMaster(BPCASNPreShipmentMaster ASNPreShipmentMaster)
        {
            BPCASNPreShipmentMaster ASNPreShipmentMasterResult = new BPCASNPreShipmentMaster();
            try
            {
                BPCASNPreShipmentMaster ASNPreShipmentMaster1 = (from tb in _dbContext.BPCASNPreShipmentMasters
                                                                 where tb.IsActive && tb.ID == ASNPreShipmentMaster.ID
                                                                 select tb).FirstOrDefault();
                if (ASNPreShipmentMaster1 != null)
                {
                    _dbContext.BPCASNPreShipmentMasters.Remove(ASNPreShipmentMaster1);
                    await _dbContext.SaveChangesAsync();
                    ASNPreShipmentMasterResult.ID = ASNPreShipmentMaster.ID;
                    ASNPreShipmentMasterResult.PreShipmentDay = ASNPreShipmentMaster.PreShipmentDay;
                }
                else
                {
                    return ASNPreShipmentMasterResult;
                }
                //ASNPreShipmentMaster1.IsActive = false;
                //ASNPreShipmentMaster1.ModifiedOn = DateTime.Now;
                //ASNPreShipmentMaster1.ModifiedBy = ASNPreShipmentMaster.ModifiedBy;

            }
            catch (SqlException ex) { WriteLog.WriteToFile("MasterRepository/DeleteASNPreShipmentMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("MasterRepository/DeleteASNPreShipmentMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                WriteLog.WriteToFile("Master/DeleteASNPreShipmentMaster : - ", ex);
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
            return ASNPreShipmentMasterResult;
        }


        public List<BPCASNFieldMaster> GetASNFieldMasterByType(string DocType)
        {
            try
            {
                return _dbContext.BPCASNFieldMasters.Where(t => t.DocType == DocType).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/GetASNFieldMasterByType", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/GetASNFieldMasterByType", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<BPCASNFieldMaster> UpdateASNFieldMaster(BPCASNFieldMaster ASNFieldMaster)
        {
            try
            {
                BPCASNFieldMaster ASNFieldMaster2 = (from tb in _dbContext.BPCASNFieldMasters
                                                     where tb.IsActive && tb.ID == ASNFieldMaster.ID
                                                     select tb).FirstOrDefault();
                ASNFieldMaster2.Text = ASNFieldMaster.Text;
                ASNFieldMaster2.DefaultValue = ASNFieldMaster.DefaultValue;
                ASNFieldMaster2.Mandatory = ASNFieldMaster.Mandatory;
                ASNFieldMaster2.Invisible = ASNFieldMaster.Invisible;
                ASNFieldMaster2.IsActive = true;
                ASNFieldMaster2.ModifiedOn = DateTime.Now;
                ASNFieldMaster2.ModifiedBy = ASNFieldMaster.ModifiedBy;
                await _dbContext.SaveChangesAsync();
                return ASNFieldMaster2;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/UpdateASNFieldMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/UpdateASNFieldMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASNFieldMasterRepository/UpdateASNFieldMaster : - ", ex);
                throw ex;
            }
        }

        public int GetArrivalDateIntervalByPO(string DocNumber)
        {
            try
            {
                var result = (from tb in _dbContext.BPCASNHeaders
                              where tb.DocNumber == DocNumber && tb.IsActive
                              orderby tb.ModifiedOn descending
                              select tb.ArrivalDateInterval).FirstOrDefault();
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/GetArrivalDateIntervalByPO", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/GetArrivalDateIntervalByPO", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetArrivalDateIntervalByPOAndPartnerID(string DocNumber)
        {
            try
            {
                //var result = (from tb in _dbContext.BPCASNHeaders
                //              where tb.DocNumber == DocNumber && tb.PatnerID == PartnerID && tb.IsActive
                //              orderby tb.ModifiedOn descending
                //              select tb.ArrivalDateInterval).FirstOrDefault();
                var result = (from tb in _dbContext.BPCASNHeaders
                              where tb.DocNumber == DocNumber && tb.IsActive
                              orderby tb.ModifiedOn descending
                              select tb.ArrivalDateInterval).FirstOrDefault();
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/GetArrivalDateIntervalByPOAndPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/GetArrivalDateIntervalByPOAndPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdatePOStatus(BPCASNHeader header, string Status)
        {
            try
            {
                var result = _dbContext.BPCOFHeaders.Where(x => x.Client == header.Client && x.Company == header.Company && x.Type == x.Type && x.PatnerID == header.PatnerID && x.DocNumber == header.DocNumber).FirstOrDefault();
                if (result != null)
                {
                    if (Status == "DueForGate")
                    {
                        // result.Status = result.Status == "PartialGate" ? "PartialGate" : Status;
                        result.Status = "PartialGate";
                    }
                    else
                    {
                        result.Status = Status;
                    }
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/UpdatePOStatus", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/UpdatePOStatus", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdatePOStatus1(BPCASNOFMap1 header, string Status)
        {
            try
            {
                var result = _dbContext.BPCOFHeaders.Where(x => x.Client == header.Client && x.Company == header.Company && x.Type == x.Type && x.PatnerID == header.PatnerID && x.DocNumber == header.DocNumber).FirstOrDefault();
                if (result != null)
                {
                    if (Status == "DueForGate")
                    {
                        // result.Status = result.Status == "PartialGate" ? "PartialGate" : Status;
                        result.Status = "PartialGate";
                    }
                    else
                    {
                        result.Status = Status;
                    }
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/UpdatePOStatus", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/UpdatePOStatus", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdatePOStatus1(BPCASNHeader1 header, string DocNumber, string Status)
        {
            try
            {
                var result = _dbContext.BPCOFHeaders.Where(x => x.Client == header.Client && x.Company == header.Company && x.Type == x.Type && x.PatnerID == header.PatnerID && x.DocNumber == DocNumber).FirstOrDefault();
                if (result != null)
                {
                    if (Status == "DueForGate")
                    {
                        // result.Status = result.Status == "PartialGate" ? "PartialGate" : Status;
                        result.Status = "PartialGate";
                    }
                    else
                    {
                        result.Status = Status;
                    }
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/UpdatePOStatus", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/UpdatePOStatus", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> CreateASNItems(List<BPCASNItemView> ASNitems, string ASNNumber, string DocNumber, bool IsSubmitted)
        {
            try
            {
                bool IsNonZeroOpenQty = false;
                foreach (BPCASNItemView item in ASNitems)
                {
                    BPCASNItem ASNItem = new BPCASNItem();
                    ASNItem.Client = item.Client;
                    ASNItem.Company = item.Company;
                    ASNItem.Type = item.Type;
                    ASNItem.PatnerID = item.PatnerID;
                    ASNItem.ASNNumber = ASNNumber;
                    ASNItem.Item = item.Item;
                    ASNItem.Material = item.Material;
                    ASNItem.MaterialText = item.MaterialText;
                    ASNItem.DeliveryDate = item.DeliveryDate;
                    ASNItem.OrderedQty = item.OrderedQty;
                    ASNItem.CompletedQty = item.CompletedQty;
                    ASNItem.TransitQty = item.TransitQty;
                    ASNItem.OpenQty = item.OpenQty;
                    ASNItem.ASNQty = item.ASNQty;
                    ASNItem.UOM = item.UOM;
                    ASNItem.UnitPrice = item.UnitPrice;
                    ASNItem.PlantCode = item.PlantCode;
                    ASNItem.Value = item.Value;
                    ASNItem.TaxAmount = item.TaxAmount;
                    ASNItem.TaxCode = item.TaxCode;
                    ASNItem.IsFreightAvailable = item.IsFreightAvailable;
                    ASNItem.FreightAmount = item.FreightAmount;
                    ASNItem.ToleranceUpperLimit = item.ToleranceUpperLimit;
                    ASNItem.ToleranceLowerLimit = item.ToleranceLowerLimit;
                    ASNItem.HSN = item.HSN;
                    ASNItem.CreatedBy = item.CreatedBy;
                    ASNItem.ModifiedBy = item.ModifiedBy;
                    ASNItem.IsActive = true;
                    ASNItem.CreatedOn = DateTime.Now;
                    //item.DocNumber = DocNumber;
                    var result = _dbContext.BPCASNItems.Add(ASNItem);
                    if (IsSubmitted)
                    {
                        var poItem = await UpdatePOItemQty(ASNItem, DocNumber);
                        if (!IsNonZeroOpenQty)
                        {
                            IsNonZeroOpenQty = poItem.OpenQty.HasValue && poItem.OpenQty.Value > 0;
                        }
                    }

                    _dbContext.BPCASNItemBatches.Where(x => x.ASNNumber == ASNNumber && x.Item == item.Item).ToList().ForEach(x => _dbContext.BPCASNItemBatches.Remove(x));
                    await _dbContext.SaveChangesAsync();
                    if (item.ASNItemBatches != null && item.ASNItemBatches.Count > 0)
                    {
                        await CreateASNItemBatches(ASNItem, item.ASNItemBatches, ASNNumber, DocNumber);
                    }

                    _dbContext.BPCASNItemSESes.Where(x => x.ASNNumber == ASNNumber && x.Item == item.Item).ToList().ForEach(x => _dbContext.BPCASNItemSESes.Remove(x));
                    await _dbContext.SaveChangesAsync();
                    if (item.ASNItemSESes != null && item.ASNItemSESes.Count > 0)
                    {
                        await CreateASNItemServices(ASNItem, item.ASNItemSESes, ASNNumber, DocNumber);
                    }
                }
                await _dbContext.SaveChangesAsync();
                return IsNonZeroOpenQty;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/CreateASNItems", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/CreateASNItems", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<BPCASNItemView1>> CreateASNItems1(List<BPCASNItemView1> ASNitems, string ASNNumber, string DocNumber, bool IsSubmitted)
        {
            try
            {
                bool IsNonZeroOpenQty = false;
                foreach (BPCASNItemView1 item in ASNitems)
                {
                    BPCASNItem1 ASNItem = new BPCASNItem1();
                    ASNItem.Client = item.Client;
                    ASNItem.Company = item.Company;
                    ASNItem.Type = item.Type;
                    ASNItem.PatnerID = item.PatnerID;
                    ASNItem.ASNNumber = ASNNumber;
                    ASNItem.DocNumber = item.DocNumber;
                    ASNItem.Item = item.Item;
                    ASNItem.Material = item.Material;
                    ASNItem.MaterialText = item.MaterialText;
                    ASNItem.DeliveryDate = item.DeliveryDate;
                    ASNItem.OrderedQty = item.OrderedQty;
                    ASNItem.CompletedQty = item.CompletedQty;
                    ASNItem.TransitQty = item.TransitQty;
                    ASNItem.OpenQty = item.OpenQty;
                    ASNItem.ASNQty = item.ASNQty;
                    ASNItem.UOM = item.UOM;
                    ASNItem.UnitPrice = item.UnitPrice;
                    ASNItem.PlantCode = item.PlantCode;
                    ASNItem.Value = item.Value;
                    ASNItem.TaxAmount = item.TaxAmount;
                    ASNItem.TaxCode = item.TaxCode;
                    ASNItem.IsFreightAvailable = item.IsFreightAvailable;
                    ASNItem.FreightAmount = item.FreightAmount;
                    ASNItem.ToleranceUpperLimit = item.ToleranceUpperLimit;
                    ASNItem.ToleranceLowerLimit = item.ToleranceLowerLimit;
                    ASNItem.HSN = item.HSN;
                    ASNItem.CreatedBy = item.CreatedBy;
                    ASNItem.ModifiedBy = item.ModifiedBy;
                    ASNItem.IsActive = true;
                    ASNItem.CreatedOn = DateTime.Now;
                    var result = _dbContext.BPCASNItem1.Add(ASNItem);
                    if (IsSubmitted)
                    {
                        var poItem = await UpdatePOItemQty1(ASNItem, item.DocNumber);
                        item.ItemOpenQty = poItem.OpenQty;
                        if (!IsNonZeroOpenQty)
                        {
                            IsNonZeroOpenQty = poItem.OpenQty.HasValue && poItem.OpenQty.Value > 0;
                        }
                    }

                    _dbContext.BPCASNItemBatch1.Where(x => x.DocNumber == item.DocNumber && x.ASNNumber == ASNNumber && x.Item == item.Item).ToList().ForEach(x => _dbContext.BPCASNItemBatch1.Remove(x));
                    await _dbContext.SaveChangesAsync();
                    if (item.ASNItemBatches != null && item.ASNItemBatches.Count > 0)
                    {
                        await CreateASNItemBatches1(ASNItem, item.ASNItemBatches, ASNNumber, item.DocNumber);
                    }

                    _dbContext.BPCASNItemSES1.Where(x => x.DocNumber == item.DocNumber && x.ASNNumber == ASNNumber && x.Item == item.Item).ToList().ForEach(x => _dbContext.BPCASNItemSES1.Remove(x));
                    await _dbContext.SaveChangesAsync();
                    if (item.ASNItemSESes != null && item.ASNItemSESes.Count > 0)
                    {
                        await CreateASNItemServices1(ASNItem, item.ASNItemSESes, ASNNumber, item.DocNumber);
                    }
                }
                await _dbContext.SaveChangesAsync();
                return ASNitems;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/CreateASNItems1", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/CreateASNItems1", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CreateASNItemBatches(BPCASNItem ASNItem, List<BPCASNItemBatch> ASNitemBatches, string ASNNumber, string DocNumber)
        {
            try
            {
                foreach (BPCASNItemBatch item in ASNitemBatches)
                {
                    item.Client = item.Client ?? ASNItem.Client;
                    item.Company = item.Company ?? ASNItem.Company;
                    item.Type = item.Type ?? ASNItem.Type;
                    item.PatnerID = item.PatnerID ?? ASNItem.PatnerID;
                    item.ASNNumber = ASNNumber;
                    item.IsActive = true;
                    item.CreatedOn = DateTime.Now;
                    var result = _dbContext.BPCASNItemBatches.Add(item);
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/CreateASNItemBatches", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/CreateASNItemBatches", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CreateASNItemBatches1(BPCASNItem1 ASNItem, List<BPCASNItemBatch1> ASNitemBatches, string ASNNumber, string DocNumber)
        {
            try
            {
                foreach (BPCASNItemBatch1 item in ASNitemBatches)
                {
                    item.Client = item.Client ?? ASNItem.Client;
                    item.Company = item.Company ?? ASNItem.Company;
                    item.Type = item.Type ?? ASNItem.Type;
                    item.PatnerID = item.PatnerID ?? ASNItem.PatnerID;
                    item.ASNNumber = ASNNumber;
                    item.DocNumber = ASNItem.DocNumber;
                    item.IsActive = true;
                    item.CreatedOn = DateTime.Now;
                    var result = _dbContext.BPCASNItemBatch1.Add(item);
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/CreateASNItemBatches1", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/CreateASNItemBatches1", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CreateASNItemServices(BPCASNItem ASNItem, List<BPCASNItemSES> ASNitemServices, string ASNNumber, string DocNumber)
        {
            try
            {
                foreach (BPCASNItemSES item in ASNitemServices)
                {
                    item.Client = item.Client ?? ASNItem.Client;
                    item.Company = item.Company ?? ASNItem.Company;
                    item.Type = item.Type ?? ASNItem.Type;
                    item.PatnerID = item.PatnerID ?? ASNItem.PatnerID;
                    item.ASNNumber = ASNNumber;
                    item.IsActive = true;
                    item.CreatedOn = DateTime.Now;
                    var result = _dbContext.BPCASNItemSESes.Add(item);
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/CreateASNItemServices", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/CreateASNItemServices", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CreateASNItemServices1(BPCASNItem1 ASNItem, List<BPCASNItemSES1> ASNitemServices, string ASNNumber, string DocNumber)
        {
            try
            {
                foreach (BPCASNItemSES1 item in ASNitemServices)
                {
                    item.Client = item.Client ?? ASNItem.Client;
                    item.Company = item.Company ?? ASNItem.Company;
                    item.Type = item.Type ?? ASNItem.Type;
                    item.PatnerID = item.PatnerID ?? ASNItem.PatnerID;
                    item.ASNNumber = ASNNumber;
                    item.DocNumber = ASNItem.DocNumber;
                    item.IsActive = true;
                    item.CreatedOn = DateTime.Now;
                    var result = _dbContext.BPCASNItemSES1.Add(item);
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/CreateASNItemServices1", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/CreateASNItemServices1", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCOFItem> UpdatePOItemQty(BPCASNItem item, string DocNumber)
        {
            try
            {
                var poItem = _dbContext.BPCOFItems.Where(x => x.DocNumber == DocNumber && x.Item == item.Item).FirstOrDefault();
                if (poItem != null)
                {
                    poItem.OrderedQty = item.OrderedQty;
                    poItem.TransitQty = poItem.TransitQty + item.ASNQty;
                    poItem.OpenQty = poItem.OrderedQty - poItem.TransitQty - poItem.CompletedQty;
                    if (poItem.OpenQty < 0)
                    {
                        var ToleranceQty = ((poItem.OrderedQty * poItem.ToleranceUpperLimit) / 100);
                        if (ToleranceQty == null)
                        {
                            ToleranceQty = 0;
                        }
                        if ((poItem.OpenQty + ToleranceQty) < 0)
                        {
                            throw new Exception("Trying to create ASN for more than accepted qty");
                        }

                    }
                }
                await _dbContext.SaveChangesAsync();
                return poItem;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/UpdatePOItemQty", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/UpdatePOItemQty", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCOFItem> UpdatePOItemQty1(BPCASNItem1 item, string DocNumber)
        {
            try
            {
                var poItem = _dbContext.BPCOFItems.Where(x => x.DocNumber == DocNumber && x.Item == item.Item).FirstOrDefault();
                if (poItem != null)
                {
                    poItem.OrderedQty = item.OrderedQty;
                    poItem.TransitQty = poItem.TransitQty + item.ASNQty;
                    poItem.OpenQty = poItem.OrderedQty - poItem.TransitQty - poItem.CompletedQty;
                    if (poItem.OpenQty < 0)
                    {
                        //throw new Exception("Trying to create ASN for more than open qty");
                        var ToleranceQty = ((poItem.OrderedQty * poItem.ToleranceUpperLimit) / 100);
                        if ((poItem.OpenQty + ToleranceQty) < 0)
                        {
                            throw new Exception("Trying to create ASN for more than accepted qty");
                        }
                    }
                }
                await _dbContext.SaveChangesAsync();
                return poItem;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/UpdatePOItemQty1", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/UpdatePOItemQty1", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCOFItem> UpdateDeletedPOItemQty(BPCASNItem item, string DocNumber)
        {
            try
            {
                var poItem = _dbContext.BPCOFItems.Where(x => x.DocNumber == DocNumber && x.Item == item.Item).FirstOrDefault();
                if (poItem != null)
                {
                    //poItem.OrderedQty = item.OrderedQty;
                    poItem.TransitQty = poItem.TransitQty - item.ASNQty;
                    poItem.OpenQty = poItem.OrderedQty - poItem.TransitQty - poItem.CompletedQty;
                    //poItem.OpenQty = poItem.OpenQty + item.ASNQty;
                    //poItem.TransitQty = poItem.TransitQty - item.ASNQty;
                }
                await _dbContext.SaveChangesAsync();
                return poItem;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/UpdateDeletedPOItemQty", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/UpdateDeletedPOItemQty", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCOFItem> UpdateDeletedPOItemQty1(BPCASNItem1 item, string DocNumber)
        {
            try
            {
                var poItem = _dbContext.BPCOFItems.Where(x => x.DocNumber == item.DocNumber && x.Item == item.Item).FirstOrDefault();
                if (poItem != null)
                {
                    //poItem.OrderedQty = item.OrderedQty;
                    poItem.TransitQty = poItem.TransitQty - item.ASNQty;
                    poItem.OpenQty = poItem.OrderedQty - poItem.TransitQty - poItem.CompletedQty;
                    //poItem.OpenQty = poItem.OpenQty + item.ASNQty;
                    //poItem.TransitQty = poItem.TransitQty - item.ASNQty;
                }
                await _dbContext.SaveChangesAsync();
                return poItem;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/UpdateDeletedPOItemQty", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/UpdateDeletedPOItemQty", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPCOFItem> UpdateDeletedPOItemQty1(BPCASNItem1 item)
        {
            try
            {
                var poItem = _dbContext.BPCOFItems.Where(x => x.DocNumber == item.DocNumber && x.Item == item.Item).FirstOrDefault();
                if (poItem != null)
                {
                    //poItem.OrderedQty = item.OrderedQty;
                    poItem.TransitQty = poItem.TransitQty - item.ASNQty;
                    poItem.OpenQty = poItem.OrderedQty - poItem.TransitQty - poItem.CompletedQty;
                    //poItem.OpenQty = poItem.OpenQty + item.ASNQty;
                    //poItem.TransitQty = poItem.TransitQty - item.ASNQty;
                }
                await _dbContext.SaveChangesAsync();
                return poItem;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/UpdateDeletedPOItemQty1", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/UpdateDeletedPOItemQty1", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<string> GetASNOFsByASN(string ASNNumber)
        {
            try
            {
                return _dbContext.BPCASNOFMap1.Where(x => x.ASNNumber == ASNNumber).Select(x => x.DocNumber).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/GetASNOFsByASN", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/GetASNOFsByASN", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BPCASNItem> GetASNItemsByASN(string ASNNumber)
        {
            try
            {
                return _dbContext.BPCASNItems.Where(x => x.ASNNumber == ASNNumber).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/GetASNItemsByASN", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/GetASNItemsByASN", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public List<BPCASNItemView> GetASNItemsWithBatchesByASN(string ASNNumber)
        //{
        //    try
        //    {
        //        var ASN = (from tb in _dbContext.BPCASNHeaders where tb.ASNNumber == ASNNumber select tb).FirstOrDefault();
        //        List<BPCASNItemView> bPCASNItemViews = new List<BPCASNItemView>();
        //        if (ASN != null && !string.IsNullOrEmpty(ASN.DocNumber))
        //        {
        //            var result = _dbContext.BPCASNItems.Where(x => x.ASNNumber == ASNNumber).ToList();
        //            result.ForEach(item =>
        //            {
        //                BPCASNItemView ASNItem = new BPCASNItemView();
        //                var ofItem = _dbContext.BPCOFItems.Where(x => x.DocNumber == ASN.DocNumber && x.Item == item.Item).FirstOrDefault();
        //                ASNItem.Client = item.Client;
        //                ASNItem.Company = item.Company;
        //                ASNItem.Type = item.Type;
        //                ASNItem.PatnerID = item.PatnerID;
        //                ASNItem.ASNNumber = ASNNumber;
        //                ASNItem.Item = item.Item;
        //                ASNItem.Material = item.Material;
        //                ASNItem.MaterialText = item.MaterialText;
        //                if (!ASN.IsSubmitted)
        //                {
        //                    if (ofItem != null)
        //                    {
        //                        if (ofItem.OrderedQty.HasValue)
        //                            ASNItem.OrderedQty = ofItem.OrderedQty.Value;
        //                        if (ofItem.CompletedQty.HasValue)
        //                            ASNItem.CompletedQty = ofItem.CompletedQty.Value;
        //                        if (ofItem.TransitQty.HasValue)
        //                            ASNItem.TransitQty = ofItem.TransitQty.Value;
        //                        if (ofItem.OpenQty.HasValue)
        //                            ASNItem.OpenQty = ofItem.OpenQty.Value;
        //                    }
        //                }
        //                else
        //                {
        //                    ASNItem.OrderedQty = item.OrderedQty;
        //                    ASNItem.CompletedQty = item.CompletedQty;
        //                    ASNItem.TransitQty = item.TransitQty;
        //                    ASNItem.OpenQty = item.OpenQty;
        //                }
        //                ASNItem.ASNQty = item.ASNQty;
        //                ASNItem.UOM = item.UOM;
        //                ASNItem.UnitPrice = item.UnitPrice;
        //                ASNItem.PlantCode = item.PlantCode;
        //                ASNItem.Value = item.Value;
        //                ASNItem.TaxAmount = item.TaxAmount;
        //                ASNItem.TaxCode = item.TaxCode;
        //                ASNItem.HSN = item.HSN;
        //                ASNItem.IsActive = item.IsActive;
        //                ASNItem.CreatedOn = item.CreatedOn;
        //                ASNItem.CreatedBy = item.CreatedBy;
        //                ASNItem.ModifiedOn = item.ModifiedOn;
        //                ASNItem.ModifiedBy = item.ModifiedBy;
        //                ASNItem.ASNItemBatches = _dbContext.BPCASNItemBatches.Where(x => x.ASNNumber == item.ASNNumber && x.Item == item.Item).ToList();
        //                ASNItem.ASNItemSESes = _dbContext.BPCASNItemSESes.Where(x => x.ASNNumber == item.ASNNumber && x.Item == item.Item).ToList();
        //                bPCASNItemViews.Add(ASNItem);
        //            });
        //        }

        //        return bPCASNItemViews;
        //    }
        //    catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/GetASNItemsWithBatchesByASN", ex); throw new Exception("Something went wrong"); }
        //    catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/GetASNItemsWithBatchesByASN", ex); throw new Exception("Something went wrong"); }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public List<BPCASNItemView> GetASNItemsWithBatchesByASN(string ASNNumber)
        {
            try
            {
                var ASN = (from tb in _dbContext.BPCASNHeaders where tb.ASNNumber == ASNNumber select tb).FirstOrDefault();
                List<BPCASNItemView> bPCASNItemViews = new List<BPCASNItemView>();
                if (ASN != null && !string.IsNullOrEmpty(ASN.DocNumber))
                {
                    var result = _dbContext.BPCASNItems.Where(x => x.ASNNumber == ASNNumber).ToList();
                    result.ForEach(item =>
                    {
                        BPCASNItemView ASNItem = new BPCASNItemView();
                        var ofItem = _dbContext.BPCOFItems.Where(x => x.DocNumber == ASN.DocNumber && x.Item == item.Item).FirstOrDefault();
                        ASNItem.Client = item.Client;
                        ASNItem.Company = item.Company;
                        ASNItem.Type = item.Type;
                        ASNItem.PatnerID = item.PatnerID;
                        ASNItem.ASNNumber = ASNNumber;
                        ASNItem.Item = item.Item;
                        ASNItem.Material = item.Material;
                        ASNItem.MaterialText = item.MaterialText;
                        if (!ASN.IsSubmitted)
                        {
                            if (ofItem != null)
                            {
                                if (ofItem.OrderedQty.HasValue)
                                    ASNItem.OrderedQty = ofItem.OrderedQty.Value;
                                if (ofItem.CompletedQty.HasValue)
                                    ASNItem.CompletedQty = ofItem.CompletedQty.Value;
                                if (ofItem.TransitQty.HasValue)
                                    ASNItem.TransitQty = ofItem.TransitQty.Value;
                                if (ofItem.OpenQty.HasValue)
                                    ASNItem.OpenQty = ofItem.OpenQty.Value;
                            }
                        }
                        else
                        {
                            ASNItem.OrderedQty = item.OrderedQty;
                            ASNItem.CompletedQty = item.CompletedQty;
                            ASNItem.TransitQty = item.TransitQty;
                            ASNItem.OpenQty = item.OpenQty;
                        }
                        ASNItem.ASNQty = item.ASNQty;
                        ASNItem.UOM = item.UOM;
                        ASNItem.UnitPrice = item.UnitPrice;
                        ASNItem.PlantCode = item.PlantCode;
                        ASNItem.Value = item.Value;
                        ASNItem.TaxAmount = item.TaxAmount;
                        ASNItem.TaxCode = item.TaxCode;
                        ASNItem.IsFreightAvailable = item.IsFreightAvailable;
                        ASNItem.FreightAmount = item.FreightAmount;
                        ASNItem.ToleranceUpperLimit = item.ToleranceUpperLimit;
                        ASNItem.ToleranceLowerLimit = item.ToleranceLowerLimit;
                        ASNItem.HSN = item.HSN;
                        ASNItem.IsActive = item.IsActive;
                        ASNItem.CreatedOn = item.CreatedOn;
                        ASNItem.CreatedBy = item.CreatedBy;
                        ASNItem.ModifiedOn = item.ModifiedOn;
                        ASNItem.ModifiedBy = item.ModifiedBy;
                        ASNItem.ASNItemBatches = _dbContext.BPCASNItemBatches.Where(x => x.ASNNumber == item.ASNNumber && x.Item == item.Item).ToList();
                        ASNItem.ASNItemSESes = _dbContext.BPCASNItemSESes.Where(x => x.ASNNumber == item.ASNNumber && x.Item == item.Item).ToList();
                        bPCASNItemViews.Add(ASNItem);
                    });
                }

                return bPCASNItemViews;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/GetASNItemsWithBatchesByASN", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/GetASNItemsWithBatchesByASN", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCASNItemView1> GetASNItem1WithBatchesByASN(string ASNNumber)
        {
            try
            {
                var ASN = (from tb in _dbContext.BPCASNHeader1 where tb.ASNNumber == ASNNumber select tb).FirstOrDefault();
                List<BPCASNItemView1> bPCASNItemViews = new List<BPCASNItemView1>();
                if (ASN != null)
                {
                    var result = _dbContext.BPCASNItem1.Where(x => x.ASNNumber == ASNNumber).ToList();
                    result.ForEach(item =>
                    {
                        BPCASNItemView1 ASNItem = new BPCASNItemView1();
                        var ofItem = _dbContext.BPCOFItems.Where(x => x.DocNumber == item.DocNumber && x.Item == item.Item).FirstOrDefault();
                        ASNItem.Client = item.Client;
                        ASNItem.Company = item.Company;
                        ASNItem.Type = item.Type;
                        ASNItem.PatnerID = item.PatnerID;
                        ASNItem.ASNNumber = ASNNumber;
                        ASNItem.DocNumber = item.DocNumber;
                        ASNItem.Item = item.Item;
                        ASNItem.Material = item.Material;
                        ASNItem.MaterialText = item.MaterialText;
                        if (!ASN.IsSubmitted)
                        {
                            if (ofItem != null)
                            {
                                if (ofItem.OrderedQty.HasValue)
                                    ASNItem.OrderedQty = ofItem.OrderedQty.Value;
                                if (ofItem.CompletedQty.HasValue)
                                    ASNItem.CompletedQty = ofItem.CompletedQty.Value;
                                if (ofItem.TransitQty.HasValue)
                                    ASNItem.TransitQty = ofItem.TransitQty.Value;
                                if (ofItem.OpenQty.HasValue)
                                    ASNItem.OpenQty = ofItem.OpenQty.Value;
                            }
                        }
                        else
                        {
                            ASNItem.OrderedQty = item.OrderedQty;
                            ASNItem.CompletedQty = item.CompletedQty;
                            ASNItem.TransitQty = item.TransitQty;
                            ASNItem.OpenQty = item.OpenQty;
                        }
                        ASNItem.ASNQty = item.ASNQty;
                        ASNItem.UOM = item.UOM;
                        ASNItem.UnitPrice = item.UnitPrice;
                        ASNItem.PlantCode = item.PlantCode;
                        ASNItem.Value = item.Value;
                        ASNItem.TaxAmount = item.TaxAmount;
                        ASNItem.TaxCode = item.TaxCode;
                        ASNItem.IsFreightAvailable = item.IsFreightAvailable;
                        ASNItem.FreightAmount = item.FreightAmount;
                        ASNItem.ToleranceUpperLimit = item.ToleranceUpperLimit;
                        ASNItem.ToleranceLowerLimit = item.ToleranceLowerLimit;
                        ASNItem.HSN = item.HSN;
                        ASNItem.IsActive = item.IsActive;
                        ASNItem.CreatedOn = item.CreatedOn;
                        ASNItem.CreatedBy = item.CreatedBy;
                        ASNItem.ModifiedOn = item.ModifiedOn;
                        ASNItem.ModifiedBy = item.ModifiedBy;
                        ASNItem.ASNItemBatches = _dbContext.BPCASNItemBatch1.Where(x => x.DocNumber == item.DocNumber && x.ASNNumber == item.ASNNumber && x.Item == item.Item).ToList();
                        ASNItem.ASNItemSESes = _dbContext.BPCASNItemSES1.Where(x => x.DocNumber == item.DocNumber && x.ASNNumber == item.ASNNumber && x.Item == item.Item).ToList();
                        bPCASNItemViews.Add(ASNItem);
                    });
                }

                return bPCASNItemViews;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/GetASNItemsWithBatchesByASN", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/GetASNItemsWithBatchesByASN", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCASNItemBatch> GetASNItemBatchesByASN(string ASNNumber)
        {
            try
            {
                return _dbContext.BPCASNItemBatches.Where(x => x.ASNNumber == ASNNumber).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/GetASNItemBatchesByASN", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/GetASNItemBatchesByASN", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CreateASNPacks(List<BPCASNPack> ASNPacks, string ASNNumber, string DocNumber)
        {
            try
            {
                foreach (BPCASNPack pack in ASNPacks)
                {
                    pack.ASNNumber = ASNNumber;
                    pack.IsActive = true;
                    pack.CreatedOn = DateTime.Now;
                    var result = _dbContext.BPCASNPacks.Add(pack);
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/CreateASNPacks", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/CreateASNPacks", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCASNPack> GetASNPacksByASN(string ASNNumber)
        {
            try
            {
                return _dbContext.BPCASNPacks.Where(x => x.ASNNumber == ASNNumber).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/GetASNPacksByASN", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/GetASNPacksByASN", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CreateDocumentCenters(List<BPCDocumentCenter> DocumentCenters, string ASNNumber)
        {
            try
            {
                foreach (BPCDocumentCenter item in DocumentCenters)
                {
                    item.ID = 0;
                    item.ASNNumber = ASNNumber;
                    item.IsActive = true;
                    item.CreatedOn = DateTime.Now;
                    var result = _dbContext.BPCDocumentCenters.Add(item);
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/CreateDocumentCenters", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/CreateDocumentCenters", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCDocumentCenter> GetDocumentCentersByASN(string ASNNumber)
        {
            try
            {
                return _dbContext.BPCDocumentCenters.Where(x => x.ASNNumber == ASNNumber).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/GetDocumentCentersByASN", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/GetDocumentCentersByASN", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AddInvoiceAttachment(BPCAttachment BPAttachment)
        {

            var strategy = _dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {

                    try
                    {
                        BPAttachment.IsActive = true;
                        BPAttachment.CreatedOn = DateTime.Now;
                        var result = _dbContext.BPCAttachments.Add(BPAttachment);
                        await _dbContext.SaveChangesAsync();

                        //var asn = _dbContext.BPCASNHeaders.Where(x => x.ASNNumber == ASNNumber).FirstOrDefault();
                        //if (asn != null)
                        //{
                        //    BPAttachment.IsActive = true;
                        //    BPAttachment.CreatedOn = DateTime.Now;
                        //    var result = _dbContext.BPCAttachments.Add(BPAttachment);
                        //    asn.InvDocReferenceNo = result.Entity.AttachmentID.ToString();
                        //    await _dbContext.SaveChangesAsync();
                        //    SaveASNInvAttachment(asn, BPAttachment.AttachmentName, BPAttachment.AttachmentFile);

                        //    //if (asn.IsSubmitted && IsIBDRequired=="Yes")
                        //    //{
                        //    //    var OFH = _dbContext.BPCOFHeaders.Where(x => x.DocNumber == asn.DocNumber).FirstOrDefault();
                        //    //    if (OFH != null && OFH.DocType == "SER")
                        //    //    {
                        //    //        WriteLog.WriteToFile($"SES creation started for ASN {asn.ASNNumber}");
                        //    //        FMResult = await CreateSES(ASNView, asn.ASNNumber, asn.DocNumber);
                        //    //    }
                        //    //    else
                        //    //    {
                        //    //        WriteLog.WriteToFile($"IBD creation started for ASN {ASN.ASNNumber}");
                        //    //        FMResult = await CreateIBD(ASNView, ASN.ASNNumber, ASN.DocNumber);
                        //    //    }

                        //    //    if (FMResult == "Success")
                        //    //    {
                        //    //        asn.Field1 = "";
                        //    //    }
                        //    //    else
                        //    //    {
                        //    //        // ASN.ASNDate = ASNView.ASNDate;
                        //    //        asn.Status = "Saved";
                        //    //        asn.IsSubmitted = false;
                        //    //        asn.ASNDate = (DateTime?)null;
                        //    //        asn.Field1 = FMResult;
                        //    //        await _dbContext.SaveChangesAsync();
                        //    //        //transaction.Commit();
                        //    //        //transaction.Dispose();
                        //    //        //throw new Exception(FMResult);
                        //    //    }

                        //    //}
                        //}
                        transaction.Commit();
                        transaction.Dispose();
                    }
                    catch (SqlException ex)
                    {
                        WriteLog.WriteToFile("ASNRepository/AddInvoiceAttachment", ex);
                        //DeleteASNFiles(ASNNumber);
                        throw new Exception("Something went wrong");
                    }
                    catch (InvalidOperationException ex)
                    {
                        WriteLog.WriteToFile("ASNRepository/AddInvoiceAttachment", ex);
                        //DeleteASNFiles(ASNNumber);
                        throw new Exception("Something went wrong");
                    }
                    catch (Exception ex)
                    {
                        //DeleteASNFiles(ASNNumber);
                        transaction.Rollback();
                        transaction.Dispose();
                        throw ex;
                    }
                }
            });
        }

        public async Task AddDocumentCenterAttachment(List<BPCAttachment> BPAttachments, string ASNNumber,string IsSubmitted,string Company)
        {
            try
            
            {
                int i = 0;
                foreach (BPCAttachment BPAttachment in BPAttachments)
                {
                    i++;
                //    for (int i = 0; i < BPAttachments.Count; i++)
                //{
                    BPAttachment.IsActive = true;
                    BPAttachment.CreatedOn = DateTime.Now;
                    var result = _dbContext.BPCAttachments.Add(BPAttachment);
                    await _dbContext.SaveChangesAsync();
                    var docCenter = _dbContext.BPCDocumentCenters.Where(x => x.ASNNumber == ASNNumber && x.Filename == BPAttachment.AttachmentName).FirstOrDefault();
                    if (docCenter != null)
                    {
                        docCenter.AttachmentReferenceNo = result.Entity.AttachmentID.ToString();
                    }
                    await _dbContext.SaveChangesAsync();
                    if (docCenter != null)
                    {
                        var docCenterMaster = _dbContext.BPCDocumentCenterMasters.Where(x => x.DocumentType == docCenter.DocumentType).FirstOrDefault();
                        //if (docCenterMaster != null && !string.IsNullOrEmpty(docCenterMaster.ForwardMail))
                        //{
                        //    //new Thread(async () =>
                        //    //{
                        //    //    await SendMailDocCenterUser(docCenterMaster.ForwardMail, BPAttachment);
                        //    //}).Start();
                        //    //await SendMailDocCenterUser(docCenterMaster.ForwardMail, BPAttachment);
                        //}
                    }
                    if (IsSubmitted == "true")
                    {
                        var documentinvoices = _dbContext.BPCDocumentCenters.Where(x => x.ASNNumber == ASNNumber).ToList();

                        var BaseFileName = Company + DateTime.Now.ToString("ddMMyyyy") + "_" + ASNNumber;
                        foreach (BPCDocumentCenter documentinvoice in documentinvoices)
                        {
                            if (documentinvoice.AttachmentReferenceNo != null)
                            {

                                bool DocInvoiceResult = UpdateAndSaveASNDocAttachment(documentinvoice.Filename, documentinvoice.AttachmentReferenceNo, BaseFileName, i, ASNNumber);
                            }
                        }
                    }
                }

                //}
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/AddDocumentCenterAttachment", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/AddDocumentCenterAttachment", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BPCAttachment GetAttachmentByName(string AttachmentName, string ASNNumber)
        {
            try
            {
                return _dbContext.BPCAttachments.FirstOrDefault(x => x.AttachmentName == AttachmentName);
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/GetAttachmentByName", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/GetAttachmentByName", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public BPCAttachment DowloandAttachmentByID(int AttachmentID)
        {
            try
            {
                return _dbContext.BPCAttachments.FirstOrDefault(x => x.AttachmentID == AttachmentID);
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/DowloandAttachmentByID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/DowloandAttachmentByID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BPCInvoiceAttachment GetInvoiceAttachmentByASN(string ASNNumber, string InvDocReferenceNo)
        {
            try
            {
                return (from tb in _dbContext.BPCASNHeaders
                        join tb1 in _dbContext.BPCAttachments on tb.InvDocReferenceNo equals tb1.AttachmentID.ToString()
                        where tb.ASNNumber == ASNNumber && tb.InvDocReferenceNo == InvDocReferenceNo
                        select new BPCInvoiceAttachment()
                        {
                            AttachmentID = tb1.AttachmentID,
                            AttachmentName = tb1.AttachmentName,
                            ContentType = tb1.ContentType,
                            ContentLength = tb1.ContentLength
                        }).FirstOrDefault();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/GetInvoiceAttachmentByASN", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/GetInvoiceAttachmentByASN", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BPCInvoiceAttachment GetInvoiceAttachment1ByASN(string ASNNumber, string InvDocReferenceNo)
        {
            try
            {
                return (from tb in _dbContext.BPCASNHeader1
                        join tb1 in _dbContext.BPCAttachments on tb.InvDocReferenceNo equals tb1.AttachmentID.ToString()
                        where tb.ASNNumber == ASNNumber && tb.InvDocReferenceNo == InvDocReferenceNo
                        select new BPCInvoiceAttachment()
                        {
                            AttachmentID = tb1.AttachmentID,
                            AttachmentName = tb1.AttachmentName,
                            ContentType = tb1.ContentType,
                            ContentLength = tb1.ContentLength
                        }).FirstOrDefault();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/GetInvoiceAttachment1ByASN", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/GetInvoiceAttachment1ByASN", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public byte[] CreateASNPdf(string ASNNumber, bool FTPFlag)
        {
            try
            {
                BPCASNHeader header = _dbContext.BPCASNHeaders.Where(x => x.ASNNumber == ASNNumber).FirstOrDefault();
                List<BPCASNItem> asnItems = _dbContext.BPCASNItems.Where(x => x.ASNNumber == ASNNumber).ToList();
                int asnItemsCount = asnItems.Count;
                List<BPCASNPack> asnPacks = _dbContext.BPCASNPacks.Where(x => x.ASNNumber == ASNNumber).ToList();

                WriteLog.WriteToFile($"ASN Pdf creation started for ASN {ASNNumber}");

                if (header.IsSubmitted && FTPFlag)
                {
                    // BPCAttachment att = _dbContext.BPCAttachments.Where(x => x.AttachmentID.ToString() == header.InvDocReferenceNo).FirstOrDefault();
                    //SaveASNInvAttachment(header, att.AttachmentName, att.AttachmentFile);
                    WriteLog.WriteToFile($"Send ASNAttachments To FTP started for ASN {ASNNumber}");
                    SendASNAttachmentsToFTP();
                }

                var headerFont = FontFactory.GetFont("Arial", 15, Font.BOLD);
                var titleFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
                var titleBlueFont = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLUE);
                var boldFont = FontFactory.GetFont("Arial", 8, Font.BOLD);
                var boldBlueFont = FontFactory.GetFont("Arial", 8, Font.BOLD, BaseColor.BLUE);
                var normalFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                var normalBlueFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLUE);

                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                {
                    Document document = new Document(PageSize.A4, 10, 10, 10, 10);
                    PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                    PdfContentByte cb = new PdfContentByte(writer);
                    document.Open();

                    //Header table
                    PdfPTable headerTable = new PdfPTable(2) { WidthPercentage = 100 };
                    headerTable.SetWidths(new float[] { 5f, 95f });
                    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                    iTextSharp.text.Image img1 = iTextSharp.text.Image.GetInstance(path + "/truck.png");
                    img1.ScaleAbsolute(15f, 12f);
                    PdfPCell Imgcell = new PdfPCell(img1);
                    Imgcell.Padding = 5f;
                    Imgcell.PaddingBottom = 12f;
                    Imgcell.Border = Rectangle.NO_BORDER;
                    Imgcell.VerticalAlignment = Element.ALIGN_BOTTOM;
                    headerTable.AddCell(Imgcell);
                    PdfPCell headercell = new PdfPCell(new Phrase("Active ASNs", headerFont));
                    headercell.Padding = 5f;
                    headercell.PaddingBottom = 14f;
                    headercell.Border = Rectangle.NO_BORDER;
                    headercell.VerticalAlignment = Element.ALIGN_BOTTOM;
                    headerTable.AddCell(headercell);

                    document.Add(headerTable);

                    //ASN Table
                    //PdfPTable ASNTable = new PdfPTable(7) { WidthPercentage = 100 };

                    //PdfPCell cell = new PdfPCell(new Phrase("ASN Number", titleBlueFont));
                    //cell.Padding = 5f;
                    //ASNTable.AddCell(cell);
                    //cell = new PdfPCell(new Phrase("Vendor", titleFont));
                    //cell.Padding = 5f;
                    //ASNTable.AddCell(cell);
                    //cell = new PdfPCell(new Phrase("Mode", titleFont));
                    //cell.Padding = 5f;
                    //ASNTable.AddCell(cell);
                    //cell = new PdfPCell(new Phrase("Item", titleFont));
                    //cell.Padding = 5f;
                    //ASNTable.AddCell(cell);
                    //cell = new PdfPCell(new Phrase("Material", titleFont));
                    //cell.Padding = 5f;
                    //ASNTable.AddCell(cell);
                    //cell = new PdfPCell(new Phrase("Material Text", titleFont));
                    //cell.Padding = 5f;
                    //ASNTable.AddCell(cell);
                    //cell = new PdfPCell(new Phrase("Created Date", titleFont));
                    //cell.Padding = 5f;
                    //ASNTable.AddCell(cell);

                    //cell = new PdfPCell(new Phrase(header.ASNNumber, normalBlueFont));
                    //cell.Rowspan = asnItemsCount;
                    //cell.Border = Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER;
                    //cell.Padding = 5f;
                    //ASNTable.AddCell(cell);
                    //cell = new PdfPCell(new Phrase(header.TransporterName, normalFont));
                    //cell.Rowspan = asnItemsCount;
                    //cell.Border = Rectangle.BOTTOM_BORDER;
                    //cell.Padding = 5f;
                    //ASNTable.AddCell(cell);
                    //cell = new PdfPCell(new Phrase(header.TransportMode, normalFont));
                    //cell.Rowspan = asnItemsCount;
                    //cell.Border = Rectangle.BOTTOM_BORDER;
                    //cell.Padding = 5f;
                    //ASNTable.AddCell(cell);
                    PdfPCell cell = new PdfPCell();


                    //ASNHeader table
                    PdfPTable ASNHeaderTable = new PdfPTable(2) { WidthPercentage = 100 };
                    //ASNHeaderTable.SetWidths(new float[] { 35f, 25f, 20f, 20f });
                    ASNHeaderTable.SetWidths(new float[] { 50f, 50f });

                    ////Barcode table
                    //PdfPTable BarcodeTable = new PdfPTable(1);
                    //cell = new PdfPCell(new Phrase("Shipment Number", titleFont));
                    //cell.Border = Rectangle.NO_BORDER;
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //BarcodeTable.AddCell(cell);

                    //cell = new PdfPCell(new Phrase("1234", normalFont));
                    //cell.Border = Rectangle.NO_BORDER;
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //BarcodeTable.AddCell(cell);

                    //Barcode128 code128 = new Barcode128();
                    //code128.CodeType = Barcode.CODE128;
                    //code128.BarHeight = 18f;
                    //code128.ChecksumText = false;
                    //code128.GenerateChecksum = false;
                    //code128.StartStopText = true;
                    //code128.Code = "ABCD";
                    //code128.Font = null;
                    //// Generate barcode image
                    //iTextSharp.text.Image image128 = code128.CreateImageWithBarcode(cb, null, null);
                    //image128.Border = Rectangle.NO_BORDER;
                    //// Add image to table cell
                    //BarcodeTable.AddCell(image128);
                    //ASNHeaderTable.AddCell(BarcodeTable);

                    //QRCode table
                    PdfPTable QRCodeTable = new PdfPTable(1);
                    cell = new PdfPCell(new Phrase("Shipment Number", titleFont));
                    cell.Border = Rectangle.NO_BORDER;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    QRCodeTable.AddCell(cell);

                    cell = new PdfPCell(new Phrase(header.ASNNumber, normalFont));
                    cell.Border = Rectangle.NO_BORDER;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    QRCodeTable.AddCell(cell);

                    //CreateTemp_QRCodeFolder();
                    string QRCodeGeneratorlevel = _configuration["QRCodeGeneratorlevel"];
                    StringBuilder sb = new StringBuilder();
                    string ASNData = GetASNXmlData(header, asnItems);
                    var imageData = GenerateQR(100, 100, ASNData);
                    System.Drawing.Bitmap imge;
                    using (var ms = new MemoryStream(imageData))
                    {
                        imge = new System.Drawing.Bitmap(ms);
                    }
                    iTextSharp.text.Image image128 = iTextSharp.text.Image.GetInstance(imge, System.Drawing.Imaging.ImageFormat.Bmp);
                    Imgcell = new PdfPCell(image128);
                    Imgcell.Padding = 2f;
                    Imgcell.Border = Rectangle.NO_BORDER;
                    Imgcell.HorizontalAlignment = Element.ALIGN_CENTER;
                    QRCodeTable.AddCell(Imgcell);
                    ASNHeaderTable.AddCell(QRCodeTable);


                    //QRCodeGenerator.ECCLevel eccLevel = (QRCodeGenerator.ECCLevel)(QRCodeGeneratorlevel == "L" ? 0 : QRCodeGeneratorlevel == "M" ? 1 : QRCodeGeneratorlevel == "Q" ? 2 : 3);
                    //using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                    //using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(ASNData, eccLevel))
                    //{
                    //    using (QRCode qrCode = new QRCode(qrCodeData))
                    //    {
                    //        System.Drawing.Bitmap imge = qrCode.GetGraphic(150, System.Drawing.Color.Black, System.Drawing.Color.White, true);
                    //        iTextSharp.text.Image image128 = iTextSharp.text.Image.GetInstance(imge, System.Drawing.Imaging.ImageFormat.Bmp);

                    //        image128.ScaleAbsolute(250f, 250f);
                    //        Imgcell = new PdfPCell(image128);
                    //        Imgcell.Padding = 2f;
                    //        Imgcell.Border = Rectangle.NO_BORDER;
                    //        Imgcell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //        QRCodeTable.AddCell(Imgcell);
                    //        ASNHeaderTable.AddCell(QRCodeTable);
                    //    }
                    //}


                    //ASN details
                    PdfPTable ASNDetailTable = new PdfPTable(1);
                    cell = new PdfPCell(new Phrase($"Transporter : {header.TransporterName}", normalFont));
                    cell.Border = Rectangle.NO_BORDER;
                    ASNDetailTable.AddCell(cell);

                    cell = new PdfPCell(new Phrase($"Vessle No : {header.VessleNumber}", normalFont));
                    cell.Border = Rectangle.NO_BORDER;
                    ASNDetailTable.AddCell(cell);

                    cell = new PdfPCell(new Phrase($"AWB Number : {header.AWBNumber}", normalFont));
                    cell.Border = Rectangle.NO_BORDER;
                    ASNDetailTable.AddCell(cell);

                    var arrDate = header.ArrivalDate.HasValue ? header.ArrivalDate.Value.ToString("dd/MM/yyyy") : "";
                    cell = new PdfPCell(new Phrase($"Ship/Del date : {arrDate}", normalFont));
                    cell.Border = Rectangle.NO_BORDER;
                    ASNDetailTable.AddCell(cell);
                    ASNHeaderTable.AddCell(ASNDetailTable);

                    //From Address
                    //PdfPTable FromAddressTable = new PdfPTable(1);
                    //cell = new PdfPCell(new Phrase("From", titleFont));
                    //cell.Border = Rectangle.NO_BORDER;
                    //FromAddressTable.AddCell(cell);

                    //cell = new PdfPCell(new Phrase("Exalca", normalFont));
                    //cell.Border = Rectangle.NO_BORDER;
                    //FromAddressTable.AddCell(cell);

                    //cell = new PdfPCell(new Phrase("Bangalore", normalFont));
                    //cell.Border = Rectangle.NO_BORDER;
                    //FromAddressTable.AddCell(cell);

                    //cell = new PdfPCell(new Phrase("Karnataka", normalFont));
                    //cell.Border = Rectangle.NO_BORDER;
                    //FromAddressTable.AddCell(cell);
                    //ASNHeaderTable.AddCell(FromAddressTable);

                    //To address
                    //if (asnItemsCount > 0)
                    //{
                    //    var PlantCode = asnItems[0].PlantCode;
                    //    var Plant = _dbContext.BPCPlantMasters.Where(x => x.PlantCode == PlantCode).FirstOrDefault();
                    //    if (Plant != null)
                    //    {
                    //        PdfPTable ToAddressTable = new PdfPTable(1);
                    //        cell = new PdfPCell(new Phrase(Plant.PlantText, titleFont));
                    //        cell.Border = Rectangle.NO_BORDER;
                    //        ToAddressTable.AddCell(cell);

                    //        cell = new PdfPCell(new Phrase(Plant.AddressLine1, normalFont));
                    //        cell.Border = Rectangle.NO_BORDER;
                    //        ToAddressTable.AddCell(cell);

                    //        cell = new PdfPCell(new Phrase(Plant.City, normalFont));
                    //        cell.Border = Rectangle.NO_BORDER;
                    //        ToAddressTable.AddCell(cell);

                    //        cell = new PdfPCell(new Phrase(Plant.State, normalFont));
                    //        cell.Border = Rectangle.NO_BORDER;
                    //        ToAddressTable.AddCell(cell);

                    //        cell = new PdfPCell(new Phrase(Plant.Country + " , " + Plant.PinCode, normalFont));
                    //        cell.Border = Rectangle.NO_BORDER;
                    //        ToAddressTable.AddCell(cell);
                    //        ASNHeaderTable.AddCell(ToAddressTable);
                    //    }
                    //    else
                    //    {
                    //        PdfPTable ToAddressTable = new PdfPTable(1);
                    //        cell = new PdfPCell(new Phrase("To", titleFont));
                    //        cell.Border = Rectangle.NO_BORDER;
                    //        ToAddressTable.AddCell(cell);

                    //        cell = new PdfPCell(new Phrase("BPCloud", normalFont));
                    //        cell.Border = Rectangle.NO_BORDER;
                    //        ToAddressTable.AddCell(cell);

                    //        cell = new PdfPCell(new Phrase("Bangalore", normalFont));
                    //        cell.Border = Rectangle.NO_BORDER;
                    //        ToAddressTable.AddCell(cell);

                    //        cell = new PdfPCell(new Phrase("Karnataka", normalFont));
                    //        cell.Border = Rectangle.NO_BORDER;
                    //        ToAddressTable.AddCell(cell);
                    //        ASNHeaderTable.AddCell(ToAddressTable);
                    //    }
                    //}
                    //else
                    //{
                    //    PdfPTable ToAddressTable = new PdfPTable(1);
                    //    cell = new PdfPCell(new Phrase("To", titleFont));
                    //    cell.Border = Rectangle.NO_BORDER;
                    //    ToAddressTable.AddCell(cell);

                    //    cell = new PdfPCell(new Phrase("BPCloud", normalFont));
                    //    cell.Border = Rectangle.NO_BORDER;
                    //    ToAddressTable.AddCell(cell);

                    //    cell = new PdfPCell(new Phrase("Bangalore", normalFont));
                    //    cell.Border = Rectangle.NO_BORDER;
                    //    ToAddressTable.AddCell(cell);

                    //    cell = new PdfPCell(new Phrase("Karnataka", normalFont));
                    //    cell.Border = Rectangle.NO_BORDER;
                    //    ToAddressTable.AddCell(cell);
                    //    ASNHeaderTable.AddCell(ToAddressTable);
                    //}


                    document.Add(ASNHeaderTable);

                    PdfPTable ASNItemTable = new PdfPTable(8) { WidthPercentage = 100 };
                    ASNItemTable.SpacingBefore = 20f;


                    cell = new PdfPCell(new Phrase("PO", titleFont));
                    cell.Padding = 5f;
                    ASNItemTable.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Item", titleFont));
                    cell.Padding = 5f;
                    ASNItemTable.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Material", titleFont));
                    cell.Padding = 5f;
                    ASNItemTable.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Material Text", titleFont));
                    cell.Padding = 5f;
                    ASNItemTable.AddCell(cell);
                    cell = new PdfPCell(new Phrase("ASN Qty", titleFont));
                    cell.Padding = 5f;
                    ASNItemTable.AddCell(cell);
                    cell = new PdfPCell(new Phrase("UOM", titleFont));
                    cell.Padding = 5f;
                    ASNItemTable.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Batch", titleFont));
                    cell.Padding = 5f;
                    ASNItemTable.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Manufacture Date", titleFont));
                    cell.Padding = 5f;
                    ASNItemTable.AddCell(cell);

                    int i = 1;
                    foreach (var item in asnItems)
                    {
                        List<BPCASNItemBatch> asnItemPatch = _dbContext.BPCASNItemBatches.Where(x => x.ASNNumber == ASNNumber && x.Item == item.Item).ToList();
                        if (asnItemPatch.Count > 0)
                        {
                            foreach (var patchItem in asnItemPatch)
                            {
                                cell = new PdfPCell(new Phrase(header.DocNumber, normalFont));
                                if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER;
                                else cell.Border = Rectangle.LEFT_BORDER;
                                cell.Padding = 5f;
                                ASNItemTable.AddCell(cell);

                                cell = new PdfPCell(new Phrase(item.Item, normalFont));
                                if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER;
                                else cell.Border = Rectangle.NO_BORDER;
                                cell.Padding = 5f;
                                ASNItemTable.AddCell(cell);

                                cell = new PdfPCell(new Phrase(item.Material, normalFont));
                                if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER;
                                else cell.Border = Rectangle.NO_BORDER;
                                cell.Padding = 5f;
                                ASNItemTable.AddCell(cell);

                                cell = new PdfPCell(new Phrase(item.MaterialText, normalFont));
                                if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER;
                                else cell.Border = Rectangle.NO_BORDER;
                                cell.Padding = 5f;
                                ASNItemTable.AddCell(cell);

                                cell = new PdfPCell(new Phrase(patchItem.Qty.ToString(), normalFont));
                                if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER;
                                else cell.Border = Rectangle.NO_BORDER;
                                cell.Padding = 5f;
                                ASNItemTable.AddCell(cell);

                                cell = new PdfPCell(new Phrase(item.UOM, normalFont));
                                if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER;
                                else cell.Border = Rectangle.NO_BORDER;
                                cell.Padding = 5f;
                                ASNItemTable.AddCell(cell);


                                cell = new PdfPCell(new Phrase(patchItem.Batch, normalFont));
                                if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER;
                                else cell.Border = Rectangle.NO_BORDER;
                                cell.Padding = 5f;
                                ASNItemTable.AddCell(cell);

                                cell = new PdfPCell(new Phrase(patchItem.ManufactureDate.HasValue ? patchItem.ManufactureDate.Value.ToString("dd/MM/yyyy") : "", normalFont));
                                if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;
                                else cell.Border = Rectangle.RIGHT_BORDER;
                                cell.Padding = 5f;
                                ASNItemTable.AddCell(cell);
                            }
                        }
                        else
                        {
                            cell = new PdfPCell(new Phrase(header.DocNumber, normalFont));
                            if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER;
                            else cell.Border = Rectangle.LEFT_BORDER;
                            cell.Padding = 5f;
                            ASNItemTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase(item.Item, normalFont));
                            if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER;
                            else cell.Border = Rectangle.NO_BORDER;
                            cell.Padding = 5f;
                            ASNItemTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase(item.Material, normalFont));
                            if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER;
                            else cell.Border = Rectangle.NO_BORDER;
                            cell.Padding = 5f;
                            ASNItemTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase(item.MaterialText, normalFont));
                            if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER;
                            else cell.Border = Rectangle.NO_BORDER;
                            cell.Padding = 5f;
                            ASNItemTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase(item.ASNQty.ToString(), normalFont));
                            if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER;
                            else cell.Border = Rectangle.NO_BORDER;
                            cell.Padding = 5f;
                            ASNItemTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase(item.UOM, normalFont));
                            if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER;
                            else cell.Border = Rectangle.NO_BORDER;
                            cell.Padding = 5f;
                            ASNItemTable.AddCell(cell);


                            cell = new PdfPCell(new Phrase("", normalFont));
                            if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER;
                            else cell.Border = Rectangle.NO_BORDER;
                            cell.Padding = 5f;
                            ASNItemTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase("", normalFont));
                            if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;
                            else cell.Border = Rectangle.RIGHT_BORDER;
                            cell.Padding = 5f;
                            ASNItemTable.AddCell(cell);
                        }

                        //if (i == 1)
                        //{
                        //    cell = new PdfPCell(new Phrase(header.CreatedOn.ToString("dd/MM/yyyy"), normalFont));
                        //    cell.Rowspan = asnItemsCount;
                        //    cell.Border = Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;
                        //    cell.Padding = 5f;
                        //    ASNItemTable.AddCell(cell);
                        //}
                        i++;
                    }
                    document.Add(ASNItemTable);

                    document.Close();
                    byte[] bytes = memoryStream.ToArray();
                    memoryStream.Close();
                    WriteLog.WriteToFile($"ASN Pdf creation ended for ASN {ASNNumber}");
                    return bytes;
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/CreateASNPdf", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/CreateASNPdf", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public byte[] CreateASNPdf1(string ASNNumber, bool FTPFlag)
        {
            try
            {
                BPCASNHeader1 header = _dbContext.BPCASNHeader1.Where(x => x.ASNNumber == ASNNumber).FirstOrDefault();
                List<BPCASNItem1> asnItems = _dbContext.BPCASNItem1.Where(x => x.ASNNumber == ASNNumber).ToList();
                int asnItemsCount = asnItems.Count;
                List<BPCASNPack> asnPacks = _dbContext.BPCASNPacks.Where(x => x.ASNNumber == ASNNumber).ToList();

                WriteLog.WriteToFile($"ASN Pdf creation started for ASN {ASNNumber}");

                if (header.IsSubmitted && FTPFlag)
                {
                    // BPCAttachment att = _dbContext.BPCAttachments.Where(x => x.AttachmentID.ToString() == header.InvDocReferenceNo).FirstOrDefault();
                    //SaveASNInvAttachment(header, att.AttachmentName, att.AttachmentFile);
                    WriteLog.WriteToFile($"Send ASNAttachments To FTP started for ASN {ASNNumber}");
                    SendASNAttachmentsToFTP();
                }

                var headerFont = FontFactory.GetFont("Arial", 15, Font.BOLD);
                var titleFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
                var titleBlueFont = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLUE);
                var boldFont = FontFactory.GetFont("Arial", 8, Font.BOLD);
                var boldBlueFont = FontFactory.GetFont("Arial", 8, Font.BOLD, BaseColor.BLUE);
                var normalFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                var normalBlueFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLUE);

                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                {
                    Document document = new Document(PageSize.A4, 10, 10, 10, 10);
                    PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                    PdfContentByte cb = new PdfContentByte(writer);
                    document.Open();

                    //Header table
                    PdfPTable headerTable = new PdfPTable(2) { WidthPercentage = 100 };
                    headerTable.SetWidths(new float[] { 5f, 95f });
                    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                    iTextSharp.text.Image img1 = iTextSharp.text.Image.GetInstance(path + "/truck.png");
                    img1.ScaleAbsolute(15f, 12f);
                    PdfPCell Imgcell = new PdfPCell(img1);
                    Imgcell.Padding = 5f;
                    Imgcell.PaddingBottom = 12f;
                    Imgcell.Border = Rectangle.NO_BORDER;
                    Imgcell.VerticalAlignment = Element.ALIGN_BOTTOM;
                    headerTable.AddCell(Imgcell);
                    PdfPCell headercell = new PdfPCell(new Phrase("Active ASNs", headerFont));
                    headercell.Padding = 5f;
                    headercell.PaddingBottom = 14f;
                    headercell.Border = Rectangle.NO_BORDER;
                    headercell.VerticalAlignment = Element.ALIGN_BOTTOM;
                    headerTable.AddCell(headercell);

                    document.Add(headerTable);

                    //ASN Table
                    //PdfPTable ASNTable = new PdfPTable(7) { WidthPercentage = 100 };

                    //PdfPCell cell = new PdfPCell(new Phrase("ASN Number", titleBlueFont));
                    //cell.Padding = 5f;
                    //ASNTable.AddCell(cell);
                    //cell = new PdfPCell(new Phrase("Vendor", titleFont));
                    //cell.Padding = 5f;
                    //ASNTable.AddCell(cell);
                    //cell = new PdfPCell(new Phrase("Mode", titleFont));
                    //cell.Padding = 5f;
                    //ASNTable.AddCell(cell);
                    //cell = new PdfPCell(new Phrase("Item", titleFont));
                    //cell.Padding = 5f;
                    //ASNTable.AddCell(cell);
                    //cell = new PdfPCell(new Phrase("Material", titleFont));
                    //cell.Padding = 5f;
                    //ASNTable.AddCell(cell);
                    //cell = new PdfPCell(new Phrase("Material Text", titleFont));
                    //cell.Padding = 5f;
                    //ASNTable.AddCell(cell);
                    //cell = new PdfPCell(new Phrase("Created Date", titleFont));
                    //cell.Padding = 5f;
                    //ASNTable.AddCell(cell);

                    //cell = new PdfPCell(new Phrase(header.ASNNumber, normalBlueFont));
                    //cell.Rowspan = asnItemsCount;
                    //cell.Border = Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER;
                    //cell.Padding = 5f;
                    //ASNTable.AddCell(cell);
                    //cell = new PdfPCell(new Phrase(header.TransporterName, normalFont));
                    //cell.Rowspan = asnItemsCount;
                    //cell.Border = Rectangle.BOTTOM_BORDER;
                    //cell.Padding = 5f;
                    //ASNTable.AddCell(cell);
                    //cell = new PdfPCell(new Phrase(header.TransportMode, normalFont));
                    //cell.Rowspan = asnItemsCount;
                    //cell.Border = Rectangle.BOTTOM_BORDER;
                    //cell.Padding = 5f;
                    //ASNTable.AddCell(cell);
                    PdfPCell cell = new PdfPCell();


                    //ASNHeader table
                    PdfPTable ASNHeaderTable = new PdfPTable(2) { WidthPercentage = 100 };
                    //ASNHeaderTable.SetWidths(new float[] { 35f, 25f, 20f, 20f });
                    ASNHeaderTable.SetWidths(new float[] { 50f, 50f });

                    ////Barcode table
                    //PdfPTable BarcodeTable = new PdfPTable(1);
                    //cell = new PdfPCell(new Phrase("Shipment Number", titleFont));
                    //cell.Border = Rectangle.NO_BORDER;
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //BarcodeTable.AddCell(cell);

                    //cell = new PdfPCell(new Phrase("1234", normalFont));
                    //cell.Border = Rectangle.NO_BORDER;
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //BarcodeTable.AddCell(cell);

                    //Barcode128 code128 = new Barcode128();
                    //code128.CodeType = Barcode.CODE128;
                    //code128.BarHeight = 18f;
                    //code128.ChecksumText = false;
                    //code128.GenerateChecksum = false;
                    //code128.StartStopText = true;
                    //code128.Code = "ABCD";
                    //code128.Font = null;
                    //// Generate barcode image
                    //iTextSharp.text.Image image128 = code128.CreateImageWithBarcode(cb, null, null);
                    //image128.Border = Rectangle.NO_BORDER;
                    //// Add image to table cell
                    //BarcodeTable.AddCell(image128);
                    //ASNHeaderTable.AddCell(BarcodeTable);

                    //QRCode table
                    PdfPTable QRCodeTable = new PdfPTable(1);
                    cell = new PdfPCell(new Phrase("Shipment Number", titleFont));
                    cell.Border = Rectangle.NO_BORDER;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    QRCodeTable.AddCell(cell);

                    cell = new PdfPCell(new Phrase(header.ASNNumber, normalFont));
                    cell.Border = Rectangle.NO_BORDER;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    QRCodeTable.AddCell(cell);

                    //CreateTemp_QRCodeFolder();
                    string QRCodeGeneratorlevel = _configuration["QRCodeGeneratorlevel"];
                    StringBuilder sb = new StringBuilder();
                    string ASNData = GetASNXmlData1(header, asnItems);
                    var imageData = GenerateQR(100, 100, ASNData);
                    System.Drawing.Bitmap imge;
                    using (var ms = new MemoryStream(imageData))
                    {
                        imge = new System.Drawing.Bitmap(ms);
                    }
                    iTextSharp.text.Image image128 = iTextSharp.text.Image.GetInstance(imge, System.Drawing.Imaging.ImageFormat.Bmp);
                    Imgcell = new PdfPCell(image128);
                    Imgcell.Padding = 2f;
                    Imgcell.Border = Rectangle.NO_BORDER;
                    Imgcell.HorizontalAlignment = Element.ALIGN_CENTER;
                    QRCodeTable.AddCell(Imgcell);
                    ASNHeaderTable.AddCell(QRCodeTable);


                    //QRCodeGenerator.ECCLevel eccLevel = (QRCodeGenerator.ECCLevel)(QRCodeGeneratorlevel == "L" ? 0 : QRCodeGeneratorlevel == "M" ? 1 : QRCodeGeneratorlevel == "Q" ? 2 : 3);
                    //using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                    //using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(ASNData, eccLevel))
                    //{
                    //    using (QRCode qrCode = new QRCode(qrCodeData))
                    //    {
                    //        System.Drawing.Bitmap imge = qrCode.GetGraphic(150, System.Drawing.Color.Black, System.Drawing.Color.White, true);
                    //        iTextSharp.text.Image image128 = iTextSharp.text.Image.GetInstance(imge, System.Drawing.Imaging.ImageFormat.Bmp);

                    //        image128.ScaleAbsolute(250f, 250f);
                    //        Imgcell = new PdfPCell(image128);
                    //        Imgcell.Padding = 2f;
                    //        Imgcell.Border = Rectangle.NO_BORDER;
                    //        Imgcell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //        QRCodeTable.AddCell(Imgcell);
                    //        ASNHeaderTable.AddCell(QRCodeTable);
                    //    }
                    //}


                    //ASN details
                    PdfPTable ASNDetailTable = new PdfPTable(1);
                    cell = new PdfPCell(new Phrase($"Transporter : {header.TransporterName}", normalFont));
                    cell.Border = Rectangle.NO_BORDER;
                    ASNDetailTable.AddCell(cell);

                    cell = new PdfPCell(new Phrase($"Vessle No : {header.VessleNumber}", normalFont));
                    cell.Border = Rectangle.NO_BORDER;
                    ASNDetailTable.AddCell(cell);

                    cell = new PdfPCell(new Phrase($"AWB Number : {header.AWBNumber}", normalFont));
                    cell.Border = Rectangle.NO_BORDER;
                    ASNDetailTable.AddCell(cell);

                    var arrDate = header.ArrivalDate.HasValue ? header.ArrivalDate.Value.ToString("dd/MM/yyyy") : "";
                    cell = new PdfPCell(new Phrase($"Ship/Del date : {arrDate}", normalFont));
                    cell.Border = Rectangle.NO_BORDER;
                    ASNDetailTable.AddCell(cell);
                    ASNHeaderTable.AddCell(ASNDetailTable);

                    //From Address
                    //PdfPTable FromAddressTable = new PdfPTable(1);
                    //cell = new PdfPCell(new Phrase("From", titleFont));
                    //cell.Border = Rectangle.NO_BORDER;
                    //FromAddressTable.AddCell(cell);

                    //cell = new PdfPCell(new Phrase("Exalca", normalFont));
                    //cell.Border = Rectangle.NO_BORDER;
                    //FromAddressTable.AddCell(cell);

                    //cell = new PdfPCell(new Phrase("Bangalore", normalFont));
                    //cell.Border = Rectangle.NO_BORDER;
                    //FromAddressTable.AddCell(cell);

                    //cell = new PdfPCell(new Phrase("Karnataka", normalFont));
                    //cell.Border = Rectangle.NO_BORDER;
                    //FromAddressTable.AddCell(cell);
                    //ASNHeaderTable.AddCell(FromAddressTable);

                    //To address
                    //if (asnItemsCount > 0)
                    //{
                    //    var PlantCode = asnItems[0].PlantCode;
                    //    var Plant = _dbContext.BPCPlantMasters.Where(x => x.PlantCode == PlantCode).FirstOrDefault();
                    //    if (Plant != null)
                    //    {
                    //        PdfPTable ToAddressTable = new PdfPTable(1);
                    //        cell = new PdfPCell(new Phrase(Plant.PlantText, titleFont));
                    //        cell.Border = Rectangle.NO_BORDER;
                    //        ToAddressTable.AddCell(cell);

                    //        cell = new PdfPCell(new Phrase(Plant.AddressLine1, normalFont));
                    //        cell.Border = Rectangle.NO_BORDER;
                    //        ToAddressTable.AddCell(cell);

                    //        cell = new PdfPCell(new Phrase(Plant.City, normalFont));
                    //        cell.Border = Rectangle.NO_BORDER;
                    //        ToAddressTable.AddCell(cell);

                    //        cell = new PdfPCell(new Phrase(Plant.State, normalFont));
                    //        cell.Border = Rectangle.NO_BORDER;
                    //        ToAddressTable.AddCell(cell);

                    //        cell = new PdfPCell(new Phrase(Plant.Country + " , " + Plant.PinCode, normalFont));
                    //        cell.Border = Rectangle.NO_BORDER;
                    //        ToAddressTable.AddCell(cell);
                    //        ASNHeaderTable.AddCell(ToAddressTable);
                    //    }
                    //    else
                    //    {
                    //        PdfPTable ToAddressTable = new PdfPTable(1);
                    //        cell = new PdfPCell(new Phrase("To", titleFont));
                    //        cell.Border = Rectangle.NO_BORDER;
                    //        ToAddressTable.AddCell(cell);

                    //        cell = new PdfPCell(new Phrase("BPCloud", normalFont));
                    //        cell.Border = Rectangle.NO_BORDER;
                    //        ToAddressTable.AddCell(cell);

                    //        cell = new PdfPCell(new Phrase("Bangalore", normalFont));
                    //        cell.Border = Rectangle.NO_BORDER;
                    //        ToAddressTable.AddCell(cell);

                    //        cell = new PdfPCell(new Phrase("Karnataka", normalFont));
                    //        cell.Border = Rectangle.NO_BORDER;
                    //        ToAddressTable.AddCell(cell);
                    //        ASNHeaderTable.AddCell(ToAddressTable);
                    //    }
                    //}
                    //else
                    //{
                    //    PdfPTable ToAddressTable = new PdfPTable(1);
                    //    cell = new PdfPCell(new Phrase("To", titleFont));
                    //    cell.Border = Rectangle.NO_BORDER;
                    //    ToAddressTable.AddCell(cell);

                    //    cell = new PdfPCell(new Phrase("BPCloud", normalFont));
                    //    cell.Border = Rectangle.NO_BORDER;
                    //    ToAddressTable.AddCell(cell);

                    //    cell = new PdfPCell(new Phrase("Bangalore", normalFont));
                    //    cell.Border = Rectangle.NO_BORDER;
                    //    ToAddressTable.AddCell(cell);

                    //    cell = new PdfPCell(new Phrase("Karnataka", normalFont));
                    //    cell.Border = Rectangle.NO_BORDER;
                    //    ToAddressTable.AddCell(cell);
                    //    ASNHeaderTable.AddCell(ToAddressTable);
                    //}


                    document.Add(ASNHeaderTable);

                    PdfPTable ASNItemTable = new PdfPTable(8) { WidthPercentage = 100 };
                    ASNItemTable.SpacingBefore = 20f;


                    cell = new PdfPCell(new Phrase("PO", titleFont));
                    cell.Padding = 5f;
                    ASNItemTable.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Item", titleFont));
                    cell.Padding = 5f;
                    ASNItemTable.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Material", titleFont));
                    cell.Padding = 5f;
                    ASNItemTable.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Material Text", titleFont));
                    cell.Padding = 5f;
                    ASNItemTable.AddCell(cell);
                    cell = new PdfPCell(new Phrase("ASN Qty", titleFont));
                    cell.Padding = 5f;
                    ASNItemTable.AddCell(cell);
                    cell = new PdfPCell(new Phrase("UOM", titleFont));
                    cell.Padding = 5f;
                    ASNItemTable.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Batch", titleFont));
                    cell.Padding = 5f;
                    ASNItemTable.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Manufacture Date", titleFont));
                    cell.Padding = 5f;
                    ASNItemTable.AddCell(cell);

                    int i = 1;
                    foreach (var item in asnItems)
                    {
                        List<BPCASNItemBatch1> asnItemPatch = _dbContext.BPCASNItemBatch1.Where(x => x.DocNumber == item.DocNumber && x.ASNNumber == ASNNumber && x.Item == item.Item).ToList();
                        if (asnItemPatch.Count > 0)
                        {
                            foreach (var patchItem in asnItemPatch)
                            {
                                cell = new PdfPCell(new Phrase(item.DocNumber, normalFont));
                                if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER;
                                else cell.Border = Rectangle.LEFT_BORDER;
                                cell.Padding = 5f;
                                ASNItemTable.AddCell(cell);

                                cell = new PdfPCell(new Phrase(item.Item, normalFont));
                                if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER;
                                else cell.Border = Rectangle.NO_BORDER;
                                cell.Padding = 5f;
                                ASNItemTable.AddCell(cell);

                                cell = new PdfPCell(new Phrase(item.Material, normalFont));
                                if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER;
                                else cell.Border = Rectangle.NO_BORDER;
                                cell.Padding = 5f;
                                ASNItemTable.AddCell(cell);

                                cell = new PdfPCell(new Phrase(item.MaterialText, normalFont));
                                if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER;
                                else cell.Border = Rectangle.NO_BORDER;
                                cell.Padding = 5f;
                                ASNItemTable.AddCell(cell);

                                cell = new PdfPCell(new Phrase(patchItem.Qty.ToString(), normalFont));
                                if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER;
                                else cell.Border = Rectangle.NO_BORDER;
                                cell.Padding = 5f;
                                ASNItemTable.AddCell(cell);

                                cell = new PdfPCell(new Phrase(item.UOM, normalFont));
                                if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER;
                                else cell.Border = Rectangle.NO_BORDER;
                                cell.Padding = 5f;
                                ASNItemTable.AddCell(cell);


                                cell = new PdfPCell(new Phrase(patchItem.Batch, normalFont));
                                if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER;
                                else cell.Border = Rectangle.NO_BORDER;
                                cell.Padding = 5f;
                                ASNItemTable.AddCell(cell);

                                cell = new PdfPCell(new Phrase(patchItem.ManufactureDate.HasValue ? patchItem.ManufactureDate.Value.ToString("dd/MM/yyyy") : "", normalFont));
                                if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;
                                else cell.Border = Rectangle.RIGHT_BORDER;
                                cell.Padding = 5f;
                                ASNItemTable.AddCell(cell);
                            }
                        }
                        else
                        {
                            cell = new PdfPCell(new Phrase(item.DocNumber, normalFont));
                            if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER;
                            else cell.Border = Rectangle.LEFT_BORDER;
                            cell.Padding = 5f;
                            ASNItemTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase(item.Item, normalFont));
                            if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER;
                            else cell.Border = Rectangle.NO_BORDER;
                            cell.Padding = 5f;
                            ASNItemTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase(item.Material, normalFont));
                            if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER;
                            else cell.Border = Rectangle.NO_BORDER;
                            cell.Padding = 5f;
                            ASNItemTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase(item.MaterialText, normalFont));
                            if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER;
                            else cell.Border = Rectangle.NO_BORDER;
                            cell.Padding = 5f;
                            ASNItemTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase(item.ASNQty.ToString(), normalFont));
                            if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER;
                            else cell.Border = Rectangle.NO_BORDER;
                            cell.Padding = 5f;
                            ASNItemTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase(item.UOM, normalFont));
                            if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER;
                            else cell.Border = Rectangle.NO_BORDER;
                            cell.Padding = 5f;
                            ASNItemTable.AddCell(cell);


                            cell = new PdfPCell(new Phrase("", normalFont));
                            if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER;
                            else cell.Border = Rectangle.NO_BORDER;
                            cell.Padding = 5f;
                            ASNItemTable.AddCell(cell);

                            cell = new PdfPCell(new Phrase("", normalFont));
                            if (i == asnItemsCount) cell.Border = Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;
                            else cell.Border = Rectangle.RIGHT_BORDER;
                            cell.Padding = 5f;
                            ASNItemTable.AddCell(cell);
                        }

                        //if (i == 1)
                        //{
                        //    cell = new PdfPCell(new Phrase(header.CreatedOn.ToString("dd/MM/yyyy"), normalFont));
                        //    cell.Rowspan = asnItemsCount;
                        //    cell.Border = Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;
                        //    cell.Padding = 5f;
                        //    ASNItemTable.AddCell(cell);
                        //}
                        i++;
                    }
                    document.Add(ASNItemTable);

                    document.Close();
                    byte[] bytes = memoryStream.ToArray();
                    memoryStream.Close();
                    WriteLog.WriteToFile($"ASN Pdf creation ended for ASN {ASNNumber}");
                    return bytes;
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/CreateASNPdf", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/CreateASNPdf", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public byte[] GenerateQR(int width, int height, string text)
        {

            //var qrCodeWriter = new ZXing.BarcodeWriterPixelData
            //{
            //    Format = ZXing.BarcodeFormat.QR_CODE,
            //    Options = new QrCodeEncodingOptions { Height = height, Width = width, Margin = 0, ErrorCorrection = ErrorCorrectionLevel.H }
            //};

            //var pixelData = qrCodeWriter.Write(text);

            //using (var bitmap = new System.Drawing.Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
            //{
            //    using (var ms = new MemoryStream())
            //    {
            //        var bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, pixelData.Width, pixelData.Height),
            //        System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            //        try
            //        {
            //            // we assume that the row stride of the bitmap is aligned to 4 byte multiplied by the width of the image
            //            System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0,
            //            pixelData.Pixels.Length);
            //        }
            //        finally
            //        {
            //            bitmap.UnlockBits(bitmapData);
            //        }
            //        // save to stream as PNG
            //        bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            //    }
            //    return bitmap;
            //}
            //var path = "";
            //var writer = new BarcodeWriter();
            //writer.Format = BarcodeFormat.QR_CODE;
            //var result = writer.Write(text);
            //var barcodeBitmap = new Bitmap(result);
            //using (MemoryStream memory = new MemoryStream())
            //{
            //    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            //    {
            //        barcodeBitmap.Save(memory, ImageFormat.Jpeg);
            //        byte[] bytes = memory.ToArray();
            //        fs.Write(bytes, 0, bytes.Length);
            //    }
            //}
            //return barcodeBitmap;
            //BarcodeWriter<System.Drawing.Bitmap> writer = new BarcodeWriter<System.Drawing.Bitmap>();
            //QrCodeEncodingOptions qr = new QrCodeEncodingOptions()
            //{
            //    ErrorCorrection = ZXing.QrCode.Internal.ErrorCorrectionLevel.H,
            //    Height = height,
            //    Width = width,
            //};
            //writer.Options = qr;
            var writer = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions { Height = height, Width = width, Margin = 0, ErrorCorrection = ErrorCorrectionLevel.H }
            };
            writer.Format = BarcodeFormat.QR_CODE;
            var pixelData = writer.Write(text);
            using (var bitmap = new System.Drawing.Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
            using (var ms = new MemoryStream())
            {
                // lock the data area for fast access
                var bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, pixelData.Width, pixelData.Height),
                   System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                try
                {
                    // we assume that the row stride of the bitmap is aligned to 4 byte multiplied by the width of the image
                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0,
                       pixelData.Pixels.Length);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }
                // save to stream as PNG
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }

        public static void CreateQRCodeTempFolder()
        {
            try
            {
                //if folder - "\bin\debug\Internal_Folder" not exist,create folder "Internal_Folder" in debug
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "QRCode_Folder");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                else
                {
                    if (Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "QRCode_Folder")).Length > 0) //if file found in folder
                    {
                        string[] txtList = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "QRCode_Folder"), "*.png");
                        foreach (string f in txtList)
                        {
                            System.GC.Collect();
                            System.GC.WaitForPendingFinalizers();
                            File.Delete(f);
                        }
                    }
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/CreateQRCodeTempFolder", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/CreateQRCodeTempFolder", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASNRepository/CreateTemp_QRCodeFolder : - ", ex);
            }
        }

        public static void CreateOutboxTempFolder()
        {
            try
            {
                //if folder - "\bin\debug\Internal_Folder" not exist,create folder "Internal_Folder" in debug
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Outbox");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                else
                {
                    if (Directory.GetFiles(path).Length > 0) //if file found in folder
                    {
                        string[] txtList = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Outbox"));
                        foreach (string f in txtList)
                        {
                            FileInfo fi = new FileInfo(f);
                            if (fi.CreationTime < DateTime.Now.AddMinutes(-30))
                            {
                                System.GC.Collect();
                                System.GC.WaitForPendingFinalizers();
                                fi.Delete();
                                //File.Delete(f);
                            }
                        }
                    }
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/CreateOutboxTempFolder", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/CreateOutboxTempFolder", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASNRepository/CreateOutboxTempFolder : - ", ex);
            }
        }

        public string GetASNXmlData(BPCASNHeader header, List<BPCASNItem> asnItems)
        {
            try
            {
                //WriteLog.WriteErrorLog("Enter the GetPRWithVendorDetails method");
                CreateQRCodeTempFolder();
                Random r = new Random();
                int num = r.Next(1, 9999999);
                string writerFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "QRCode_Folder");
                var FileName = header.ASNNumber + num + ".xml";
                string writerPath = Path.Combine(writerFolder, FileName);
                using (var sw = new StringWriter())
                {
                    XmlWriter writer = XmlWriter.Create(sw);

                    WriteLog.WriteToFile($"ASN Xml data collection started for ASN {header.ASNNumber}");
                    writer.WriteStartDocument();
                    writer.WriteStartElement("ASNDATA");
                    writer.WriteElementString("VID", header.PatnerID);
                    writer.WriteElementString("PO", header.DocNumber);
                    writer.WriteElementString("ASN", header.ASNNumber);
                    writer.WriteElementString("MOT", header.TransportMode);
                    writer.WriteElementString("DEPDATE", header.DepartureDate.HasValue ? header.DepartureDate.Value.ToString("dd/MM/yyyy") : "");
                    writer.WriteElementString("ARRDATE", header.ArrivalDate.HasValue ? header.ArrivalDate.Value.ToString("dd/MM/yyyy") : "");
                    writer.WriteElementString("SHIPPINGAGENCY", header.ShippingAgency);
                    writer.WriteElementString("TRUCKNNO", header.VessleNumber);
                    writer.WriteElementString("NETWEIGHT", header.NetWeight.HasValue ? header.NetWeight.Value.ToString() : "");
                    writer.WriteElementString("GROSSWEIGHT", header.GrossWeight.HasValue ? header.GrossWeight.Value.ToString() : "");
                    writer.WriteElementString("COUNTRYOFORGIN", header.CountryOfOrigin);
                    writer.WriteElementString("AWBNO", header.AWBNumber);
                    writer.WriteElementString("ARRDATE", header.ArrivalDate.HasValue ? header.ArrivalDate.Value.ToString("dd/MM/yyyy") : "");
                    writer.WriteElementString("BOL", header.BillOfLading);
                    writer.WriteElementString("TRANPORTER", header.TransporterName);
                    writer.WriteElementString("CONPERSON", header.ContactPerson);
                    writer.WriteElementString("CONNO", header.ContactPersonNo);

                    writer.WriteStartElement("SHIPITEMS");

                    foreach (var item in asnItems)
                    {
                        List<BPCASNItemBatch> asnItemPatch = _dbContext.BPCASNItemBatches.Where(x => x.ASNNumber == header.ASNNumber && x.Item == item.Item).ToList();
                        foreach (var patchItem in asnItemPatch)
                        {
                            writer.WriteStartElement("ITEM");
                            writer.WriteElementString("ITEMNUMBER", item.Item);
                            writer.WriteElementString("METERIAL", item.Material);
                            //writer.WriteElementString("HEADER_ID", item.HEADER_ID);
                            writer.WriteElementString("METERIALDESC", item.MaterialText);
                            writer.WriteElementString("DELDATE", item.DeliveryDate.HasValue ? item.DeliveryDate.Value.ToString("dd/MM/yyyy") : "");
                            writer.WriteElementString("ORDERQTY", item.OrderedQty.ToString());
                            writer.WriteElementString("GRQTY", item.CompletedQty.ToString());
                            writer.WriteElementString("PIPELINEQTY", item.ASNQty.ToString());
                            writer.WriteElementString("OPENQTY", item.OpenQty.ToString());
                            writer.WriteElementString("UOM", item.UOM);
                            writer.WriteElementString("BATCH", patchItem.Batch);
                            writer.WriteElementString("MFDATE", patchItem.ManufactureDate.HasValue ? patchItem.ManufactureDate.Value.ToString("dd/MM/yyyy") : "");
                            writer.WriteElementString("EXPDATE", patchItem.ExpiryDate.HasValue ? patchItem.ExpiryDate.Value.ToString("dd/MM/yyyy") : "");
                            writer.WriteEndElement();
                        }
                    }

                    writer.WriteEndElement();


                    writer.WriteStartElement("INVLIST");
                    writer.WriteStartElement("LIST");
                    writer.WriteElementString("INVNO", header.InvoiceNumber);
                    writer.WriteElementString("DATE", header.InvoiceDate.HasValue ? header.InvoiceDate.Value.ToString("dd/MM/yyyy") : "");
                    writer.WriteElementString("VALUE", header.InvoiceAmount.HasValue ? header.InvoiceAmount.Value.ToString() : "");
                    writer.WriteElementString("CUR", header.InvoiceAmountUOM);
                    writer.WriteEndElement();
                    writer.WriteEndElement();

                    writer.WriteEndElement();

                    writer.Flush();
                    writer.Close();
                    WriteLog.WriteToFile($"ASN Xml data collection ended for ASN {header.ASNNumber}");
                    return sw.ToString();
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/GetASNXmlData", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/GetASNXmlData", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASNRepository/GetASNXmlData/Exception : - " + ex.Message);
                return null;
            }

        }

        public string GetASNXmlData1(BPCASNHeader1 header, List<BPCASNItem1> asnItems)
        {
            try
            {
                //WriteLog.WriteErrorLog("Enter the GetPRWithVendorDetails method");
                CreateQRCodeTempFolder();
                Random r = new Random();
                int num = r.Next(1, 9999999);
                string writerFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "QRCode_Folder");
                var FileName = header.ASNNumber + num + ".xml";
                string writerPath = Path.Combine(writerFolder, FileName);
                using (var sw = new StringWriter())
                {
                    XmlWriter writer = XmlWriter.Create(sw);

                    WriteLog.WriteToFile($"ASN Xml data collection started for ASN {header.ASNNumber}");
                    writer.WriteStartDocument();
                    writer.WriteStartElement("ASNDATA");
                    writer.WriteElementString("VID", header.PatnerID);
                    writer.WriteElementString("PO", header.DocNumber);
                    writer.WriteElementString("ASN", header.ASNNumber);
                    writer.WriteElementString("MOT", header.TransportMode);
                    writer.WriteElementString("DEPDATE", header.DepartureDate.HasValue ? header.DepartureDate.Value.ToString("dd/MM/yyyy") : "");
                    writer.WriteElementString("ARRDATE", header.ArrivalDate.HasValue ? header.ArrivalDate.Value.ToString("dd/MM/yyyy") : "");
                    writer.WriteElementString("SHIPPINGAGENCY", header.ShippingAgency);
                    writer.WriteElementString("TRUCKNNO", header.VessleNumber);
                    writer.WriteElementString("NETWEIGHT", header.NetWeight.HasValue ? header.NetWeight.Value.ToString() : "");
                    writer.WriteElementString("GROSSWEIGHT", header.GrossWeight.HasValue ? header.GrossWeight.Value.ToString() : "");
                    writer.WriteElementString("COUNTRYOFORGIN", header.CountryOfOrigin);
                    writer.WriteElementString("AWBNO", header.AWBNumber);
                    writer.WriteElementString("ARRDATE", header.ArrivalDate.HasValue ? header.ArrivalDate.Value.ToString("dd/MM/yyyy") : "");
                    writer.WriteElementString("BOL", header.BillOfLading);
                    writer.WriteElementString("TRANPORTER", header.TransporterName);
                    writer.WriteElementString("CONPERSON", header.ContactPerson);
                    writer.WriteElementString("CONNO", header.ContactPersonNo);

                    writer.WriteStartElement("SHIPITEMS");

                    foreach (var item in asnItems)
                    {
                        List<BPCASNItemBatch1> asnItemPatch = _dbContext.BPCASNItemBatch1.Where(x => x.DocNumber == item.DocNumber && x.ASNNumber == header.ASNNumber && x.Item == item.Item).ToList();
                        foreach (var patchItem in asnItemPatch)
                        {
                            writer.WriteStartElement("ITEM");
                            writer.WriteElementString("ITEMNUMBER", item.Item);
                            writer.WriteElementString("METERIAL", item.Material);
                            //writer.WriteElementString("HEADER_ID", item.HEADER_ID);
                            writer.WriteElementString("METERIALDESC", item.MaterialText);
                            writer.WriteElementString("DELDATE", item.DeliveryDate.HasValue ? item.DeliveryDate.Value.ToString("dd/MM/yyyy") : "");
                            writer.WriteElementString("ORDERQTY", item.OrderedQty.ToString());
                            writer.WriteElementString("GRQTY", item.CompletedQty.ToString());
                            writer.WriteElementString("PIPELINEQTY", item.ASNQty.ToString());
                            writer.WriteElementString("OPENQTY", item.OpenQty.ToString());
                            writer.WriteElementString("UOM", item.UOM);
                            writer.WriteElementString("BATCH", patchItem.Batch);
                            writer.WriteElementString("MFDATE", patchItem.ManufactureDate.HasValue ? patchItem.ManufactureDate.Value.ToString("dd/MM/yyyy") : "");
                            writer.WriteElementString("EXPDATE", patchItem.ExpiryDate.HasValue ? patchItem.ExpiryDate.Value.ToString("dd/MM/yyyy") : "");
                            writer.WriteEndElement();
                        }
                    }

                    writer.WriteEndElement();


                    writer.WriteStartElement("INVLIST");
                    writer.WriteStartElement("LIST");
                    writer.WriteElementString("INVNO", header.InvoiceNumber);
                    writer.WriteElementString("DATE", header.InvoiceDate.HasValue ? header.InvoiceDate.Value.ToString("dd/MM/yyyy") : "");
                    writer.WriteElementString("VALUE", header.InvoiceAmount.HasValue ? header.InvoiceAmount.Value.ToString() : "");
                    writer.WriteElementString("CUR", header.InvoiceAmountUOM);
                    writer.WriteEndElement();
                    writer.WriteEndElement();

                    writer.WriteEndElement();

                    writer.Flush();
                    writer.Close();
                    WriteLog.WriteToFile($"ASN Xml data collection ended for ASN {header.ASNNumber}");
                    return sw.ToString();
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/GetASNXmlData", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/GetASNXmlData", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASNRepository/GetASNXmlData/Exception : - " + ex.Message);
                return null;
            }

        }

        public async Task<bool> SendMailDocCenterUser(string toEmail, BPCAttachment BPCAttachment)
        {
            try
            {
                //string hostName = ConfigurationManager.AppSettings["HostName"];
                //string SMTPEmail = ConfigurationManager.AppSettings["SMTPEmail"];
                ////string fromEmail = ConfigurationManager.AppSettings["FromEmail"];
                //string SMTPEmailPassword = ConfigurationManager.AppSettings["SMTPEmailPassword"];
                //string SMTPPort = ConfigurationManager.AppSettings["SMTPPort"];
                var STMPDetailsConfig = _configuration.GetSection("STMPDetails");
                string hostName = STMPDetailsConfig["Host"];
                string SMTPEmail = STMPDetailsConfig["Email"];
                //string fromEmail = ConfigurationManager.AppSettings["FromEmail"];
                //string SMTPEmailPassword = STMPDetailsConfig["Password"];
                string SMTPPort = STMPDetailsConfig["Port"];
                string siteURL = _configuration["SiteURL"];
                //var message = new MailMessage();
                string subject = "";
                StringBuilder sb = new StringBuilder();
                //string UserName = _dbContext.TBL_User_Master.Where(x => x.Email == toEmail).Select(y => y.UserName).FirstOrDefault();
                //UserName = string.IsNullOrEmpty(UserName) ? toEmail.Split('@')[0] : UserName;
                sb.Append(string.Format("Dear {0},<br/>", toEmail));
                sb.Append("<p>Please find the attachment added while creating documenter center in BP Cloud_VP.</p>");
                sb.Append("<p>Please Login by clicking <a href=\"" + siteURL + "/#/auth/login\">here</a></p>");
                sb.Append("<p>Regards,</p><p>Admin</p>");
                subject = "BP Cloud Vendor portal";
                SmtpClient client = new SmtpClient();
                client.Port = Convert.ToInt32(SMTPPort);
                client.Host = hostName;
                client.EnableSsl = false;
                client.Timeout = 60000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                //client.UseDefaultCredentials = false;
                //client.Credentials = new System.Net.NetworkCredential(SMTPEmail.Trim(), SMTPEmailPassword.Trim());
                MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, subject, sb.ToString());
                reportEmail.BodyEncoding = UTF8Encoding.UTF8;
                reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                reportEmail.IsBodyHtml = true;
                if (BPCAttachment.AttachmentName != null)
                {
                    Attachment attachment = new Attachment(new MemoryStream(BPCAttachment.AttachmentFile), MediaTypeNames.Application.Octet);
                    ContentDisposition disposition = attachment.ContentDisposition;
                    disposition.CreationDate = File.GetCreationTime(BPCAttachment.AttachmentName);
                    disposition.ModificationDate = File.GetLastWriteTime(BPCAttachment.AttachmentName);
                    disposition.ReadDate = File.GetLastAccessTime(BPCAttachment.AttachmentName);
                    disposition.FileName = BPCAttachment.AttachmentName;
                    disposition.Size = BPCAttachment.ContentLength;
                    disposition.DispositionType = DispositionTypeNames.Attachment;
                    reportEmail.Attachments.Add(attachment);
                }
                await client.SendMailAsync(reportEmail);
                return true;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("ASNRepository/SendMailDocCenterUser", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("ASNRepository/SendMailDocCenterUser", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASNRepository/SendMail : - ", ex);
                return false;
                //throw ex;
            }
        }
        public bool GSTValidate(string gst,string Plant)
        {
            bool gstvalidate = false;
            var CheckGst = _dbContext.BPCGSTIN.Where(x => x.Plant == Plant).ToList();
            if (CheckGst.Count() > 0)
            {
                for (int i=0;i<CheckGst.Count();i++)
                {
                    if (CheckGst[i].GSTIN.ToString() == gst)
                    {
                        gstvalidate= true;
                        
                        break;
                    }
                    else
                    {
                        gstvalidate = false;
                        
                    }
                }
                
            }
            return gstvalidate;
        }
     
        public class UserView
        {
            public Guid UserID { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
        }

    }
}
