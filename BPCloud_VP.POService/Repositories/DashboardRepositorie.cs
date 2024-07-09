using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using BPCloud_VP_POService.DBContexts;
using BPCloud_VP_POService.Models;
using Microsoft.EntityFrameworkCore;
using ZXing.OneD;

namespace BPCloud_VP_POService.Repositories
{
    public class DashboardRepositorie : IDashboardRepositorie
    {
        private readonly POContext _dbContext;
        private readonly IAIACTRepository _aIACTRepository;

        public DashboardRepositorie(POContext dbContext, IAIACTRepository aIACTRepository)
        {
            _dbContext = dbContext;
            _aIACTRepository = aIACTRepository;
            
        }
       
        #region Order Fulfiment(OF)

        public List<BPCOFHeaderView> GetOfsByPartnerID(string PartnerID)
        {
            try
            {
                var bPCOFHeaders = _dbContext.BPCOFHeaders.Where(x => (x.Status == "DueForACK" || x.Status == "DueForASN" || x.Status == "PartialASN" || x.Status == "DueForGate" || x.Status == "PartialGate" || x.Status == "DueForGRN" || x.Status == "PartialGRN")
                 && x.PatnerID == PartnerID).ToList();
                List<BPCOFHeaderView> bPCOFHeaderViews = GetOfHeaderViewsFromOfHeaders(bPCOFHeaders);
                return bPCOFHeaderViews;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/GetOfsByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/GetOfsByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BPCOFHeaderView> GetOfPODetails(string GetOfPODetails)
        {
            try
            {
                string[] Plants = GetOfPODetails.Split(',');
                for (int i = 0; i < Plants.Length; i++)
                {
                    Plants[i] = Plants[i].Trim();
                }
                List<BPCOFHeader> bPCOFHeaders = new List<BPCOFHeader>();
                List<BPCOFHeader> bPCOFHeader = new List<BPCOFHeader>();
                for (int i = 0; i < Plants.Length; i++)
                {
                     bPCOFHeaders = _dbContext.BPCOFHeaders.Where(x => (x.Status == "DueForACK" || x.Status == "DueForASN" || x.Status == "PartialASN" || x.Status == "DueForGate" || x.Status == "PartialGate" || x.Status == "DueForGRN" || x.Status == "PartialGRN") && x.Plant == Plants[i]).ToList();
                 
                    bPCOFHeader.AddRange(bPCOFHeaders);
                }
                List<BPCOFHeaderView> bPCOFHeaderViews = GetOfHeaderViewsOfHeaders(bPCOFHeader);
                return bPCOFHeaderViews;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/GetOfsByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/GetOfsByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCOFHeaderView> GetOfHeaderViewsFromOfHeaders(List<BPCOFHeader> bPCOFHeaders)
        {
            try
            {
                List<BPCOFHeaderView> bPCOFHeaderViews = new List<BPCOFHeaderView>();
                foreach (var bPCOFHeader in bPCOFHeaders)
                {
                    BPCOFHeaderView bPCOFHeaderView = new BPCOFHeaderView();
                    bPCOFHeaderView.Client = bPCOFHeader.Client;
                    bPCOFHeaderView.PatnerID = bPCOFHeader.PatnerID;
                    bPCOFHeaderView.Company = bPCOFHeader.Company;
                    bPCOFHeaderView.Type = bPCOFHeader.Type;
                    bPCOFHeaderView.DocNumber = bPCOFHeader.DocNumber;
                    bPCOFHeaderView.DocDate = bPCOFHeader.DocDate;
                    bPCOFHeaderView.DocVersion = bPCOFHeader.DocVersion;
                    bPCOFHeaderView.Status = bPCOFHeader.Status;
                    bPCOFHeaderView.Currency = bPCOFHeader.Currency;
                    bPCOFHeaderView.DocType = bPCOFHeader.DocType;
                    bPCOFHeaderView.Plant = bPCOFHeader.Plant;
                    bPCOFHeaderView.PlantName = bPCOFHeader.PlantName;
                    bPCOFHeaderView.ReleasedStatus = bPCOFHeader.ReleasedStatus;
                    //var ofAttachments = _dbContext.BPCAttachments.Where(x => x.PatnerID.ToLower() == bPCOFHeader.PatnerID.ToLower()
                    //&& x.ReferenceNo.ToLower() == bPCOFHeader.DocNumber.ToLower() && x.IsActive).ToList();
                    //if (ofAttachments != null && ofAttachments.Count > 0)
                    //{
                    //    bPCOFHeaderView.DocCount = ofAttachments.Count;
                    //}
                    //else
                    //{
                    //    bPCOFHeaderView.DocCount = 0;
                    //}
                    bPCOFHeaderView.DocCount = (from tb in _dbContext.BPCAttachments
                                                where  tb.ReferenceNo == bPCOFHeader.DocNumber && tb.IsActive
                                                select tb.AttachmentID).Count();
                    //bool isAllGateEntryCompleted = true;
                    //var ASNLists = _dbContext.BPCASNHeaders.Where(x => x.Client == bPCOFHeader.Client && x.Type == bPCOFHeader.Type && x.Company == bPCOFHeader.Company && x.PatnerID == bPCOFHeader.PatnerID && x.DocNumber == bPCOFHeader.DocNumber).ToList();

                    //foreach (var asnl in ASNLists)
                    //{
                    //    if (asnl.Status.ToLower() != "gateentry completed")
                    //    {
                    //        isAllGateEntryCompleted = false;
                    //    }
                    //}
                    //header.Status = isAllGateEntryCompleted ? "DueForGRN" : "PartialGate";

                    // ACK Status
                    if (bPCOFHeader.Status == "DueForACK")
                    {
                        bool IsDeliveryDateOver = false;
                        var toDay = DateTime.Now;
                        var ItemList = _dbContext.BPCOFItems.Where(x => x.Client == bPCOFHeader.Client && x.Type == bPCOFHeader.Type && x.Company == bPCOFHeader.Company && x.PatnerID == bPCOFHeader.PatnerID && x.DocNumber == bPCOFHeader.DocNumber).ToList();
                        ItemList.ForEach(x =>
                        {
                            if (x.DeliveryDate.HasValue)
                            {
                                if (x.DeliveryDate.Value.Date < toDay.Date)
                                {
                                    IsDeliveryDateOver = true;
                                }
                            }
                        });
                        if (IsDeliveryDateOver)
                        {
                            bPCOFHeader.Status = "DueForASN";
                            bPCOFHeaderView.Status = "DueForASN";
                            _dbContext.SaveChanges();
                        }
                    }

                    //Gate Status
                    switch (bPCOFHeaderView.Status)
                    {
                        case "DueForACK":
                            bPCOFHeaderView.GateStatus = "DueForGate";
                            break;
                        case "DueForASN":
                            bPCOFHeaderView.GateStatus = "DueForGate";
                            break;
                        case "DueForGate":
                            bPCOFHeaderView.GateStatus = "DueForGate";
                            break;
                        case "PartialASN":
                            bPCOFHeaderView.GateStatus = "PartialGate";
                            break;
                        case "PartialGate":
                            bPCOFHeaderView.GateStatus = "PartialGate";
                            break;
                        case "DueForGRN":
                            bPCOFHeaderView.GateStatus = "Completed";
                            break;
                        default:
                            bPCOFHeaderView.GateStatus = "Completed";
                            break;
                    }

                    //GRN Status
                    var GRNList = _dbContext.BPCOFGRGIs.Where(x => x.Client == bPCOFHeader.Client && x.Type == bPCOFHeader.Type && x.Company == bPCOFHeader.Company && x.PatnerID == bPCOFHeader.PatnerID && x.DocNumber == bPCOFHeader.DocNumber).ToList();
                    switch (bPCOFHeaderView.Status)
                    {
                        case "DueForACK":
                            bPCOFHeaderView.GRNStatus = "DueForGRN";
                            break;
                        case "DueForASN":
                            bPCOFHeaderView.GRNStatus = "DueForGRN";
                            break;
                        case "DueForGate":
                            bPCOFHeaderView.GRNStatus = "DueForGRN";
                            break;
                        case "DueForGRN":
                            bPCOFHeaderView.GRNStatus = GRNList.Count > 0 ? "PartialGRN" : "DueForGRN";
                            break;
                        case "PartialASN":
                            bPCOFHeaderView.GRNStatus = GRNList.Count > 0 ? "PartialGRN" : "DueForGRN";
                            break;
                        case "PartialGate":
                            bPCOFHeaderView.GRNStatus = GRNList.Count > 0 ? "PartialGRN" : "DueForGRN";
                            break;
                        case "PartialGRN":
                            bPCOFHeaderView.GRNStatus = "PartialGRN";
                            break;
                        case "Completed":
                            bPCOFHeaderView.GRNStatus = "Completed";
                            break;
                        default:
                            bPCOFHeaderView.GRNStatus = "Completed";
                            break;

                    }

                    bPCOFHeaderViews.Add(bPCOFHeaderView);
                }
                return bPCOFHeaderViews;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/GetOfHeaderViewsFromOfHeaders", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/GetOfHeaderViewsFromOfHeaders", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCOFHeaderView> GetOfHeaderViewsOfHeaders(List<BPCOFHeader> bPCOFHeaders)
        {
            try
            {
                List<BPCOFHeaderView> bPCOFHeaderViews = new List<BPCOFHeaderView>();
                foreach (var bPCOFHeader in bPCOFHeaders)
                {
                    BPCOFHeaderView bPCOFHeaderView = new BPCOFHeaderView();
                    bPCOFHeaderView.Client = bPCOFHeader.Client;
                    bPCOFHeaderView.PatnerID = bPCOFHeader.PatnerID;
                    bPCOFHeaderView.Company = bPCOFHeader.Company;
                    bPCOFHeaderView.Type = bPCOFHeader.Type;
                    bPCOFHeaderView.DocNumber = bPCOFHeader.DocNumber;
                    bPCOFHeaderView.DocDate = bPCOFHeader.DocDate;
                    bPCOFHeaderView.DocVersion = bPCOFHeader.DocVersion;
                    bPCOFHeaderView.Status = bPCOFHeader.Status;
                    bPCOFHeaderView.Currency = bPCOFHeader.Currency;
                    bPCOFHeaderView.DocType = bPCOFHeader.DocType;
                    bPCOFHeaderView.Plant = bPCOFHeader.Plant;
                    bPCOFHeaderView.PlantName = bPCOFHeader.PlantName;
                    bPCOFHeaderView.ReleasedStatus = bPCOFHeader.ReleasedStatus;
                    //var ofAttachments = _dbContext.BPCAttachments.Where(x => x.PatnerID.ToLower() == bPCOFHeader.PatnerID.ToLower()
                    //&& x.ReferenceNo.ToLower() == bPCOFHeader.DocNumber.ToLower() && x.IsActive).ToList();
                    //if (ofAttachments != null && ofAttachments.Count > 0)
                    //{
                    //    bPCOFHeaderView.DocCount = ofAttachments.Count;
                    //}
                    //else
                    //{
                    //    bPCOFHeaderView.DocCount = 0;
                    //}
                    bPCOFHeaderView.DocCount = (from tb in _dbContext.BPCAttachments
                                                where tb.ReferenceNo == bPCOFHeader.DocNumber && tb.IsActive
                                                select tb.AttachmentID).Count();
                    //bool isAllGateEntryCompleted = true;
                    //var ASNLists = _dbContext.BPCASNHeaders.Where(x => x.Client == bPCOFHeader.Client && x.Type == bPCOFHeader.Type && x.Company == bPCOFHeader.Company && x.PatnerID == bPCOFHeader.PatnerID && x.DocNumber == bPCOFHeader.DocNumber).ToList();

                    //foreach (var asnl in ASNLists)
                    //{
                    //    if (asnl.Status.ToLower() != "gateentry completed")
                    //    {
                    //        isAllGateEntryCompleted = false;
                    //    }
                    //}
                    //header.Status = isAllGateEntryCompleted ? "DueForGRN" : "PartialGate";

                    // ACK Status
                    if (bPCOFHeader.Status == "DueForACK")
                    {
                        bool IsDeliveryDateOver = false;
                        var toDay = DateTime.Now;
                        var ItemList = _dbContext.BPCOFItems.Where(x => x.Client == bPCOFHeader.Client && x.Type == bPCOFHeader.Type && x.Company == bPCOFHeader.Company && x.PatnerID == bPCOFHeader.PatnerID && x.DocNumber == bPCOFHeader.DocNumber).ToList();
                        ItemList.ForEach(x =>
                        {
                            if (x.DeliveryDate.HasValue)
                            {
                                if (x.DeliveryDate.Value.Date < toDay.Date)
                                {
                                    IsDeliveryDateOver = true;
                                }
                            }
                        });
                        if (IsDeliveryDateOver)
                        {
                            bPCOFHeader.Status = "DueForASN";
                            bPCOFHeaderView.Status = "DueForASN";
                            _dbContext.SaveChanges();
                        }
                    }

                    //Gate Status
                    switch (bPCOFHeaderView.Status)
                    {
                        case "DueForACK":
                            bPCOFHeaderView.GateStatus = "DueForGate";
                            break;
                        case "DueForASN":
                            bPCOFHeaderView.GateStatus = "DueForGate";
                            break;
                        case "DueForGate":
                            bPCOFHeaderView.GateStatus = "DueForGate";
                            break;
                        case "PartialASN":
                            bPCOFHeaderView.GateStatus = "PartialGate";
                            break;
                        case "PartialGate":
                            bPCOFHeaderView.GateStatus = "PartialGate";
                            break;
                        case "DueForGRN":
                            bPCOFHeaderView.GateStatus = "Completed";
                            break;
                        default:
                            bPCOFHeaderView.GateStatus = "Completed";
                            break;
                    }

                    //GRN Status
                    var GRNList = _dbContext.BPCOFGRGIs.Where(x => x.Client == bPCOFHeader.Client && x.Type == bPCOFHeader.Type && x.Company == bPCOFHeader.Company && x.PatnerID == bPCOFHeader.PatnerID && x.DocNumber == bPCOFHeader.DocNumber).ToList();
                    switch (bPCOFHeaderView.Status)
                    {
                        case "DueForACK":
                            bPCOFHeaderView.GRNStatus = "DueForGRN";
                            break;
                        case "DueForASN":
                            bPCOFHeaderView.GRNStatus = "DueForGRN";
                            break;
                        case "DueForGate":
                            bPCOFHeaderView.GRNStatus = "DueForGRN";
                            break;
                        case "DueForGRN":
                            bPCOFHeaderView.GRNStatus = GRNList.Count > 0 ? "PartialGRN" : "DueForGRN";
                            break;
                        case "PartialASN":
                            bPCOFHeaderView.GRNStatus = GRNList.Count > 0 ? "PartialGRN" : "DueForGRN";
                            break;
                        case "PartialGate":
                            bPCOFHeaderView.GRNStatus = GRNList.Count > 0 ? "PartialGRN" : "DueForGRN";
                            break;
                        case "PartialGRN":
                            bPCOFHeaderView.GRNStatus = "PartialGRN";
                            break;
                        case "Completed":
                            bPCOFHeaderView.GRNStatus = "Completed";
                            break;
                        default:
                            bPCOFHeaderView.GRNStatus = "Completed";
                            break;

                    }

                    bPCOFHeaderViews.Add(bPCOFHeaderView);
                }
                return bPCOFHeaderViews;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/GetOfHeaderViewsFromOfHeaders", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/GetOfHeaderViewsFromOfHeaders", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BPCOFHeaderView> GetOfsByOption(OfOption ofOption)
        {
            try
            {
                bool IsDocNumber = !string.IsNullOrEmpty(ofOption.DocNumber);
                bool IsFromDate = ofOption.FromDate.HasValue;
                bool IsToDate = ofOption.ToDate.HasValue;
                bool IsStatus = !string.IsNullOrEmpty(ofOption.Status);
                List<BPCOFHeaderView> bPCOFHeaderViews = new List<BPCOFHeaderView>();
                //bool IsPlant = !string.IsNullOrEmpty(ofOption.Plant);
                if (IsStatus)
                {
                    IsStatus = ofOption.Status.ToLower() != "all" ;
                }
                bool IsDoctype = !string.IsNullOrEmpty(ofOption.DocType);
                if (IsDoctype)
                {
                    IsDoctype = ofOption.DocType.ToLower() != "all" ;
                }
                if (ofOption.Plant.Length > 0)
                {
                    foreach (var plant in ofOption.Plant)
                    {
                        if (IsStatus || IsDocNumber || IsFromDate || IsToDate || plant != null || IsDoctype)
                        {

                            var result = (from tb in _dbContext.BPCOFHeaders
                                          where (!IsDocNumber || tb.DocNumber == ofOption.DocNumber) &&
                                          (!IsFromDate || tb.DocDate.Value.Date >= ofOption.FromDate.Value.Date) &&
                                          (!IsToDate || tb.DocDate.Value.Date <= ofOption.ToDate.Value.Date) &&
                                          (!IsStatus || tb.Status == ofOption.Status) && (!IsDoctype || tb.DocType == ofOption.DocType) &&
                                           (tb.Plant == plant) && tb.IsActive == true
                                          select tb).ToList();
                            List<BPCOFHeaderView> bPCOFHeaderView = GetOfHeaderViewsFromOfHeaders(result);
                            bPCOFHeaderViews.AddRange(bPCOFHeaderView);
                        }
                    }
                }

                else
                {
                    if (IsStatus || IsDocNumber || IsFromDate || IsToDate || IsDoctype)
                    {
                        var result = (from tb in _dbContext.BPCOFHeaders
                                      where (!IsDocNumber || tb.DocNumber == ofOption.DocNumber) &&
                                      (!IsFromDate || tb.DocDate.Value.Date >= ofOption.FromDate.Value.Date) &&
                                      (!IsToDate || tb.DocDate.Value.Date <= ofOption.ToDate.Value.Date) &&
                                      (!IsStatus || tb.Status == ofOption.Status) && (!IsDoctype || tb.DocType == ofOption.DocType) &&
                                      tb.PatnerID == ofOption.PartnerID && tb.IsActive == true
                                      select tb).ToList();
                        List<BPCOFHeaderView> bPCOFHeaderView = GetOfHeaderViewsFromOfHeaders(result);
                        bPCOFHeaderViews.AddRange(bPCOFHeaderView);
                    }
                }
                
                return bPCOFHeaderViews;




                //// Only (From and To Dates)
                //if (ofOption.FromDate != null && ofOption.ToDate != null && string.IsNullOrEmpty(ofOption.Status) && string.IsNullOrEmpty(ofOption.DocType))
                //{
                //    var query = (from tb in _dbContext.BPCOFHeaders
                //                 where (!ofOption.FromDate.HasValue || tb.DocDate.Value.Date >= ofOption.FromDate.Value.Date)
                //                 && (!ofOption.ToDate.HasValue || tb.DocDate.Value.Date <= ofOption.ToDate.Value.Date) && tb.PatnerID == ofOption.PartnerID &&
                //                 tb.IsActive == true
                //                 select tb).ToList();
                //    List<BPCOFHeaderView> bPCOFHeaderViews = GetOfHeaderViewsFromOfHeaders(query);
                //    return bPCOFHeaderViews;
                //}
                //// Only Status
                //else if (!string.IsNullOrEmpty(ofOption.Status) && string.IsNullOrEmpty(ofOption.DocType) && ofOption.FromDate == null && ofOption.ToDate == null)
                //{
                //    List<BPCOFHeader> query = new List<BPCOFHeader>();
                //    if (ofOption.Status == "All")
                //    {
                //        query = (from tb in _dbContext.BPCOFHeaders
                //                 where tb.PatnerID == ofOption.PartnerID &&
                //                 tb.IsActive == true
                //                 select tb).ToList();
                //        List<BPCOFHeaderView> bPCOFHeaderViews = GetOfHeaderViewsFromOfHeaders(query);
                //        return bPCOFHeaderViews;
                //    }
                //    else
                //    {
                //        query = (from tb in _dbContext.BPCOFHeaders
                //                 where tb.Status.ToLower() == ofOption.Status.ToLower() && tb.PatnerID == ofOption.PartnerID &&
                //                 tb.IsActive == true
                //                 select tb).ToList();
                //        List<BPCOFHeaderView> bPCOFHeaderViews = GetOfHeaderViewsFromOfHeaders(query);
                //        return bPCOFHeaderViews;
                //    }
                //}
                //// Only DocType
                //else if (!string.IsNullOrEmpty(ofOption.DocType) && string.IsNullOrEmpty(ofOption.Status) && ofOption.FromDate == null && ofOption.ToDate == null)
                //{
                //    List<BPCOFHeader> query = new List<BPCOFHeader>();
                //    if (ofOption.DocType == "All")
                //    {
                //        query = (from tb in _dbContext.BPCOFHeaders
                //                 where tb.PatnerID == ofOption.PartnerID &&
                //                 tb.IsActive == true
                //                 select tb).ToList();
                //        List<BPCOFHeaderView> bPCOFHeaderViews = GetOfHeaderViewsFromOfHeaders(query);
                //        return bPCOFHeaderViews;
                //    }
                //    else
                //    {
                //        query = (from tb in _dbContext.BPCOFHeaders
                //                 where tb.DocType.ToLower() == ofOption.DocType.ToLower() && tb.PatnerID == ofOption.PartnerID &&
                //                 tb.IsActive == true
                //                 select tb).ToList();
                //        List<BPCOFHeaderView> bPCOFHeaderViews = GetOfHeaderViewsFromOfHeaders(query);
                //        return bPCOFHeaderViews;
                //    }
                //}
                //// Status AND (From and To Dates)
                //else if (!string.IsNullOrEmpty(ofOption.Status) && string.IsNullOrEmpty(ofOption.DocType) && ofOption.FromDate != null && ofOption.ToDate != null)
                //{
                //    List<BPCOFHeader> query = new List<BPCOFHeader>();
                //    if (ofOption.Status == "All")
                //    {
                //        query = (from tb in _dbContext.BPCOFHeaders
                //                 where tb.PatnerID == ofOption.PartnerID &&
                //                 (!ofOption.FromDate.HasValue || tb.DocDate.Value.Date >= ofOption.FromDate.Value.Date) &&
                //                 (!ofOption.ToDate.HasValue || tb.DocDate.Value.Date <= ofOption.ToDate.Value.Date) &&
                //                 tb.IsActive == true
                //                 select tb).ToList();
                //        List<BPCOFHeaderView> bPCOFHeaderViews = GetOfHeaderViewsFromOfHeaders(query);
                //        return bPCOFHeaderViews;
                //    }
                //    else
                //    {

                //        query = (from tb in _dbContext.BPCOFHeaders
                //                 where tb.Status.ToLower() == ofOption.Status.ToLower() && tb.PatnerID == ofOption.PartnerID &&
                //                 (!ofOption.FromDate.HasValue || tb.DocDate.Value.Date >= ofOption.FromDate.Value.Date) &&
                //                 (!ofOption.ToDate.HasValue || tb.DocDate.Value.Date <= ofOption.ToDate.Value.Date) &&
                //                 tb.IsActive == true
                //                 select tb).ToList();
                //        List<BPCOFHeaderView> bPCOFHeaderViews = GetOfHeaderViewsFromOfHeaders(query);
                //        return bPCOFHeaderViews;
                //    }
                //}
                //// DocType AND (From and To Dates)
                //else if (!string.IsNullOrEmpty(ofOption.DocType) && string.IsNullOrEmpty(ofOption.Status) && ofOption.FromDate != null && ofOption.ToDate != null)
                //{
                //    List<BPCOFHeader> query = new List<BPCOFHeader>();
                //    if (ofOption.DocType == "All")
                //    {
                //        query = (from tb in _dbContext.BPCOFHeaders
                //                 where tb.PatnerID == ofOption.PartnerID &&
                //                 (!ofOption.FromDate.HasValue || tb.DocDate.Value.Date >= ofOption.FromDate.Value.Date) &&
                //                 (!ofOption.ToDate.HasValue || tb.DocDate.Value.Date <= ofOption.ToDate.Value.Date) &&
                //                 tb.IsActive == true
                //                 select tb).ToList();
                //        List<BPCOFHeaderView> bPCOFHeaderViews = GetOfHeaderViewsFromOfHeaders(query);
                //        return bPCOFHeaderViews;
                //    }
                //    else
                //    {
                //        query = (from tb in _dbContext.BPCOFHeaders
                //                 where tb.DocType.ToLower() == ofOption.DocType.ToLower() && tb.PatnerID == ofOption.PartnerID &&
                //                 (!ofOption.FromDate.HasValue || tb.DocDate.Value.Date >= ofOption.FromDate.Value.Date) &&
                //                 (!ofOption.ToDate.HasValue || tb.DocDate.Value.Date <= ofOption.ToDate.Value.Date) &&
                //                 tb.IsActive == true
                //                 select tb).ToList();
                //        List<BPCOFHeaderView> bPCOFHeaderViews = GetOfHeaderViewsFromOfHeaders(query);
                //        return bPCOFHeaderViews;
                //    }
                //}
                //// DocType AND Status
                //else if (!string.IsNullOrEmpty(ofOption.DocType) && !string.IsNullOrEmpty(ofOption.Status) && ofOption.FromDate == null && ofOption.ToDate == null)
                //{
                //    List<BPCOFHeader> query = new List<BPCOFHeader>();
                //    if (ofOption.DocType == "All" && ofOption.Status == "All")
                //    {
                //        query = (from tb in _dbContext.BPCOFHeaders
                //                 where tb.PatnerID == ofOption.PartnerID &&
                //                 tb.IsActive == true
                //                 select tb).ToList();
                //        List<BPCOFHeaderView> bPCOFHeaderViews = GetOfHeaderViewsFromOfHeaders(query);
                //        return bPCOFHeaderViews;
                //    }
                //    else if (ofOption.DocType == "All" && ofOption.Status != "All")
                //    {
                //        query = (from tb in _dbContext.BPCOFHeaders
                //                 where tb.PatnerID == ofOption.PartnerID &&
                //                 tb.Status.ToLower() == ofOption.Status.ToLower() &&
                //                 tb.IsActive == true
                //                 select tb).ToList();
                //        List<BPCOFHeaderView> bPCOFHeaderViews = GetOfHeaderViewsFromOfHeaders(query);
                //        return bPCOFHeaderViews;
                //    }
                //    else if (ofOption.DocType != "All" && ofOption.Status == "All")
                //    {
                //        query = (from tb in _dbContext.BPCOFHeaders
                //                 where tb.PatnerID == ofOption.PartnerID &&
                //                 tb.DocType.ToLower() == ofOption.DocType.ToLower() &&
                //                 tb.IsActive == true
                //                 select tb).ToList();
                //        List<BPCOFHeaderView> bPCOFHeaderViews = GetOfHeaderViewsFromOfHeaders(query);
                //        return bPCOFHeaderViews;
                //    }
                //    else
                //    {
                //        query = (from tb in _dbContext.BPCOFHeaders
                //                 where tb.DocType.ToLower() == ofOption.DocType.ToLower() && tb.Status.ToLower() == tb.Status.ToLower() && tb.PatnerID == ofOption.PartnerID &&
                //                 tb.IsActive == true
                //                 select tb).ToList();
                //        List<BPCOFHeaderView> bPCOFHeaderViews = GetOfHeaderViewsFromOfHeaders(query);
                //        return bPCOFHeaderViews;
                //    }
                //}
                ////Status AND DocType AND (From and To Dates)
                //else if (!string.IsNullOrEmpty(ofOption.Status) && !string.IsNullOrEmpty(ofOption.DocType) && ofOption.FromDate != null && ofOption.ToDate != null)
                //{
                //    List<BPCOFHeader> query = new List<BPCOFHeader>();
                //    if (ofOption.Status == "All" && ofOption.DocType == "All")
                //    {
                //        query = (from tb in _dbContext.BPCOFHeaders
                //                 where tb.PatnerID == ofOption.PartnerID &&
                //                 //Where(v => v.Date.Value >= fromdate && v.Date.Value <= toDate)
                //                 (!ofOption.FromDate.HasValue || tb.DocDate.Value.Date >= ofOption.FromDate.Value.Date) &&
                //                 (!ofOption.ToDate.HasValue || tb.DocDate.Value.Date <= ofOption.ToDate.Value.Date) &&
                //                 tb.IsActive == true
                //                 select tb).ToList();
                //        List<BPCOFHeaderView> bPCOFHeaderViews = GetOfHeaderViewsFromOfHeaders(query);
                //        return bPCOFHeaderViews;
                //    }
                //    else if (ofOption.Status == "All" && ofOption.DocType != "All")
                //    {
                //        query = (from tb in _dbContext.BPCOFHeaders
                //                 where tb.PatnerID == ofOption.PartnerID &&
                //                 tb.DocType.ToLower() == ofOption.DocType.ToLower() &&
                //                 (!ofOption.FromDate.HasValue || tb.DocDate.Value.Date >= ofOption.FromDate.Value.Date) &&
                //                 (!ofOption.ToDate.HasValue || tb.DocDate.Value.Date <= ofOption.ToDate.Value.Date) &&
                //                 tb.IsActive == true
                //                 select tb).ToList();
                //        List<BPCOFHeaderView> bPCOFHeaderViews = GetOfHeaderViewsFromOfHeaders(query);
                //        return bPCOFHeaderViews;
                //    }
                //    else if (ofOption.Status != "All" && ofOption.DocType == "All")
                //    {
                //        query = (from tb in _dbContext.BPCOFHeaders
                //                 where tb.PatnerID == ofOption.PartnerID &&
                //                 tb.Status.ToLower() == ofOption.Status.ToLower() &&
                //                 (!ofOption.FromDate.HasValue || tb.DocDate.Value.Date >= ofOption.FromDate.Value.Date) &&
                //                 (!ofOption.ToDate.HasValue || tb.DocDate.Value.Date <= ofOption.ToDate.Value.Date) &&
                //                 tb.IsActive == true
                //                 select tb).ToList();
                //        List<BPCOFHeaderView> bPCOFHeaderViews = GetOfHeaderViewsFromOfHeaders(query);
                //        return bPCOFHeaderViews;
                //    }
                //    else
                //    {
                //        query = (from tb in _dbContext.BPCOFHeaders
                //                 where tb.Status.ToLower() == ofOption.Status.ToLower() && tb.DocType.ToLower() == ofOption.DocType.ToLower() &&
                //                 tb.PatnerID == ofOption.PartnerID &&
                //                 (!ofOption.FromDate.HasValue || tb.DocDate.Value.Date >= ofOption.FromDate.Value.Date) &&
                //                 (!ofOption.ToDate.HasValue || tb.DocDate.Value.Date <= ofOption.ToDate.Value.Date) &&
                //                 tb.IsActive == true
                //                 select tb).ToList();
                //        List<BPCOFHeaderView> bPCOFHeaderViews = GetOfHeaderViewsFromOfHeaders(query);
                //        return bPCOFHeaderViews;
                //    }
                //}
                ////All Null
                //else
                //{
                //    List<BPCOFHeader> query = new List<BPCOFHeader>();
                //    query = (from tb in _dbContext.BPCOFHeaders
                //             where (tb.DocDate == ofOption.FromDate) && tb.PatnerID == ofOption.PartnerID &&
                //             tb.IsActive == true
                //             select tb).ToList();
                //    if (query.Count != 0)
                //    {
                //        List<BPCOFHeaderView> bPCOFHeaderViews2 = GetOfHeaderViewsFromOfHeaders(query);
                //        return bPCOFHeaderViews2;
                //    }
                //    else
                //    {
                //        query = (from tb in _dbContext.BPCOFHeaders
                //                 where (tb.DocDate == ofOption.ToDate) && tb.PatnerID == ofOption.PartnerID &&
                //                 tb.IsActive == true
                //                 select tb).ToList();
                //        if (query.Count != 0)
                //        {
                //            List<BPCOFHeaderView> bPCOFHeaderViews3 = GetOfHeaderViewsFromOfHeaders(query);
                //            return bPCOFHeaderViews3;
                //        }
                //        else
                //        {
                //            query = (from tb in _dbContext.BPCOFHeaders
                //                     where tb.Status == ofOption.Status && tb.PatnerID == ofOption.PartnerID &&
                //                     tb.IsActive == true
                //                     select tb).ToList();
                //            if (query.Count != 0)
                //            {
                //                List<BPCOFHeaderView> bPCOFHeaderViews4 = GetOfHeaderViewsFromOfHeaders(query);
                //                return bPCOFHeaderViews4;
                //            }
                //            else if (ofOption.Status == "All")
                //            {
                //                query = (from tb in _dbContext.BPCOFHeaders
                //                         where tb.PatnerID == ofOption.PartnerID &&
                //                         tb.IsActive == true
                //                         select tb).ToList();
                //            }
                //            else
                //            {
                //                query = (from tb in _dbContext.BPCOFHeaders
                //                         where tb.DocType == ofOption.DocType && tb.PatnerID == ofOption.PartnerID &&
                //                         tb.IsActive == true
                //                         select tb).ToList();
                //                if (query.Count != 0)
                //                {
                //                    List<BPCOFHeaderView> bPCOFHeaderViews1 = GetOfHeaderViewsFromOfHeaders(query);
                //                    return bPCOFHeaderViews1;
                //                }
                //                else if (ofOption.DocType == "All")
                //                {
                //                    query = (from tb in _dbContext.BPCOFHeaders
                //                             where tb.PatnerID == ofOption.PartnerID &&
                //                             tb.IsActive == true
                //                             select tb).ToList();
                //                }
                //            }
                //        }
                //    }
                //    List<BPCOFHeaderView> bPCOFHeaderViews = GetOfHeaderViewsFromOfHeaders(query);
                //    return bPCOFHeaderViews;
                //}
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/GetOfsByOption", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/GetOfsByOption", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("DashboardRepositorie/GetOfsByOption", ex);
                return null;
            }
        }
        
      

        public List<FulfilmentDetails> GetVendorDoughnutChartData(string PartnerID)
        {
            try
            {

                List<FulfilmentDetails> fulfilmentDetails = new List<FulfilmentDetails>();
                Dictionary<string, string> LabelList = new Dictionary<string, string>();
                LabelList.Add("DueForACK", "Due For ACK");
                LabelList.Add("DueForASN", "Due For ASN");
                LabelList.Add("DueForGate", "Due For Gate");
                LabelList.Add("DueForGRN", "Due For GRN");

                var allResult = (from tb in _dbContext.BPCOFHeaders
                                 where tb.PatnerID == PartnerID && (tb.Status == "DueForACK" || tb.Status == "DueForASN" || tb.Status == "DueForGate" || tb.Status == "DueForGRN") && tb.IsActive == true
                                 select tb).ToList().GroupBy(x => x.Status).ToList();
                foreach (var label in allResult)
                {
                    var fulfilmentDetail = new FulfilmentDetails();
                    fulfilmentDetail.label = LabelList[label.Key];
                    fulfilmentDetail.Name = label.Key;
                    fulfilmentDetail.Value = label.Count().ToString();
                    fulfilmentDetails.Add(fulfilmentDetail);
                }
                return fulfilmentDetails;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/GetVendorDoughnutChartData", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/GetVendorDoughnutChartData", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<FulfilmentDetails> GetVendorDoughnutChartDataByPlant(string GetPlantByUser)
        {
            try
            {
                string[] PlantsIDs = GetPlantByUser.Split(',');
                for (int i = 0; i < PlantsIDs.Length; i++)
                {
                    PlantsIDs[i] = PlantsIDs[i].Trim();
                }
                //string[]  PartnersID = PartnerIDs.Distinct().ToArray();
                List<FulfilmentDetails> fulfilmentDetails = new List<FulfilmentDetails>();
                Dictionary<string, string> LabelList = new Dictionary<string, string>();
                LabelList.Add("DueForACK", "Due For ACK");
                LabelList.Add("DueForASN", "Due For ASN");
                LabelList.Add("DueForGate", "Due For Gate");
                LabelList.Add("DueForGRN", "Due For GRN");

                //List<BPCOFHeader> bpcHeader =new List<BPCOFHeader>();
                List<BPCOFHeader> bpcHeaders = new List<BPCOFHeader>();
                List<BPCOFHeader> bpcHeader = new List<BPCOFHeader>();
                //bpcHeader = _dbContext.BPCOFHeaders.Where(x => x.PatnerID == PartnersID[i] && (x.Status == "DueForACK" || x.Status == "DueForASN" || x.Status == "DueForGate" || x.Status == "DueForGRN") && x.IsActive == true).GroupBy(x => x.Status).Select(y => y.FirstOrDefault()).ToList();
                //var allResult = (from tb in _dbContext.BPCOFHeaders
                //                 where PartnersID.Contains(tb.PatnerID) && (tb.Status == "DueForACK" || tb.Status == "DueForASN" || tb.Status == "DueForGate" || tb.Status == "DueForGRN") && tb.IsActive == true
                //                 select tb).ToList().GroupBy(x => x.Status).ToList();

                //List<BPCOFHeader> bPCOFHeaders = new List<BPCOFHeader>();
                //List<BPCOFHeader> bPCOFHeader = new List<BPCOFHeader>();


                for (int i = 0; i < PlantsIDs.Length; i++)
                {
                    //bpcHeader = _dbContext.BPCOFHeaders.Where(x => x.PatnerID == PartnersID[i] && (x.Status == "DueForACK" || x.Status == "DueForASN" || x.Status == "DueForGate" || x.Status == "DueForGRN") && x.IsActive == true).GroupBy(x => x.Status).Select(y => y.FirstOrDefault()).ToList();
                    bpcHeaders = _dbContext.BPCOFHeaders.Where(x => x.Plant == PlantsIDs[i] && (x.Status == "DueForACK" || x.Status == "DueForASN" || x.Status == "DueForGate" || x.Status == "DueForGRN") && x.IsActive == true).ToList();
                    bpcHeader.AddRange(bpcHeaders);
                    
                }
                var allResult = bpcHeader.GroupBy(x=> x.Status).ToList();
                ////.Add(allResult);
                foreach (var label in allResult)
                {
                    var fulfilmentDetail = new FulfilmentDetails();
                    fulfilmentDetail.label = LabelList[label.Key];
                    fulfilmentDetail.Name = label.Key;
                    fulfilmentDetail.Value = label.Count().ToString();
                    fulfilmentDetails.Add(fulfilmentDetail);
                }
                //bpcHeaders.Add(bpcHeader);
            //}

                return fulfilmentDetails;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/GetVendorDoughnutChartData", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/GetVendorDoughnutChartData", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FulfilmentStatus GetOfStatusByPartnerID(string PartnerID)
        {
            try
            {
                FulfilmentStatus fulfilmentStatus = new FulfilmentStatus();
                FulfilmentDetails OpenDetails = new FulfilmentDetails();
                FulfilmentDetails ScheduledDetails = new FulfilmentDetails();
                FulfilmentDetails InProgressDetails = new FulfilmentDetails();
                FulfilmentDetails PendingDetails = new FulfilmentDetails();
                var allResult = (from tb in _dbContext.BPCOFHeaders
                                 where tb.PatnerID == PartnerID && (tb.Status == "DueForACK" || tb.Status == "DueForASN" || tb.Status == "DueForGate" || tb.Status == "DueForGRN") && tb.IsActive == true
                                 select tb).ToList();
                var OpenDetailsResult = (from tb in _dbContext.BPCOFHeaders
                                         where tb.PatnerID == PartnerID && tb.Status == "DueForACK" && tb.IsActive == true
                                         select tb).ToList();
                var ScheduledDetailsResult = (from tb in _dbContext.BPCOFHeaders
                                              where tb.PatnerID == PartnerID && tb.Status == "DueForASN" && tb.IsActive == true
                                              select tb).ToList();
                var InProgressDetailsResult = (from tb in _dbContext.BPCOFHeaders
                                               where tb.PatnerID == PartnerID && tb.Status == "DueForGate" && tb.IsActive == true
                                               select tb).ToList();
                var PendingDetailsResult = (from tb in _dbContext.BPCOFHeaders
                                            where tb.PatnerID == PartnerID && tb.Status == "DueForGRN" && tb.IsActive == true
                                            select tb).ToList();
                if (OpenDetailsResult != null && OpenDetailsResult.Count > 0)
                {
                    OpenDetails.Name = "Due For ACK";
                    double num4 = (double)OpenDetailsResult.Count / (double)allResult.Count;
                    double valRes = num4 * 100;
                    decimal OpenDetailsValue = Math.Round(Convert.ToDecimal(valRes));
                    OpenDetails.Value = OpenDetailsValue.ToString();
                    OpenDetails.label = OpenDetailsValue.ToString();
                }
                if (ScheduledDetailsResult != null && ScheduledDetailsResult.Count > 0)
                {
                    double num4 = (double)ScheduledDetailsResult.Count / (double)allResult.Count;
                    double valRes = num4 * 100;
                    decimal ScheduledDetailsValue = Math.Round(Convert.ToDecimal(valRes));
                    ScheduledDetails.Name = "Due For ASN";
                    ScheduledDetails.Value = ScheduledDetailsValue.ToString();
                    ScheduledDetails.label = ScheduledDetailsValue.ToString();
                }
                //else
                //{
                //    ScheduledDetails.Name = "Due For ASN";
                //    ScheduledDetails.Value = "0";
                //    ScheduledDetails.label = "0";
                //}
                if (InProgressDetailsResult != null && InProgressDetailsResult.Count > 0)
                {
                    double num4 = (double)InProgressDetailsResult.Count / (double)allResult.Count;
                    double valRes = num4 * 100;
                    decimal InProgressDetailsValue = Math.Round(Convert.ToDecimal(valRes));
                    InProgressDetails.Name = "Due For Gate";
                    InProgressDetails.Value = InProgressDetailsValue.ToString();
                    InProgressDetails.label = InProgressDetailsValue.ToString();
                }
                if (PendingDetailsResult != null && PendingDetailsResult.Count > 0)
                {
                    double num4 = (double)PendingDetailsResult.Count / (double)allResult.Count;
                    double valRes = num4 * 100;
                    decimal PendingDetailsValue = Math.Round(Convert.ToDecimal(valRes));
                    PendingDetails.Name = "Due For GRN";
                    PendingDetails.Value = PendingDetailsValue.ToString();
                    PendingDetails.label = PendingDetailsValue.ToString();
                }
                //if (entity2.Count > 0)
                //{
                //    FulfilmentDetails OpenDetails = new FulfilmentDetails();
                //    FulfilmentDetails ScheduledDetails = new FulfilmentDetails();
                //    FulfilmentDetails InProgressDetails = new FulfilmentDetails();
                //    FulfilmentDetails PendingDetails = new FulfilmentDetails();
                //    foreach (var item in entity2)
                //    {

                //        if (item.Status == "DueForACK")
                //        {
                //            OpenDetails.Name = item.KRA;
                //            OpenDetails.Value = item.KRAValue;
                //            OpenDetails.label = item.KRAValue;
                //        }
                //        else if (item.Status == "DueForASN")
                //        {
                //            ScheduledDetails.Name = item.KRA;
                //            ScheduledDetails.Value = item.KRAValue;
                //            ScheduledDetails.label = item.KRAValue;
                //        }
                //        else if (item.Status == "DueForGate")
                //        {
                //            InProgressDetails.Name = item.KRA;
                //            InProgressDetails.Value = item.KRAValue;
                //            InProgressDetails.label = item.KRAValue;
                //        }
                //        else if (item.Status == "DueForGRN")
                //        {
                //            PendingDetails.Name = item.KRA;
                //            PendingDetails.Value = item.KRAValue;
                //            PendingDetails.label = item.KRAValue;
                //        }
                //        fulfilmentStatus.OpenDetails = OpenDetails;
                //        fulfilmentStatus.ScheduledDetails = ScheduledDetails;
                //        fulfilmentStatus.InProgressDetails = InProgressDetails;
                //        fulfilmentStatus.PendingDetails = PendingDetails;
                //    }
                //}
                fulfilmentStatus.OpenDetails = OpenDetails;
                fulfilmentStatus.ScheduledDetails = ScheduledDetails;
                fulfilmentStatus.InProgressDetails = InProgressDetails;
                fulfilmentStatus.PendingDetails = PendingDetails;
                return fulfilmentStatus;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/GetOfStatusByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/GetOfStatusByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ItemDetails> GetOfItemsByPartnerIDAndDocNumber(string PartnerID, string PO)
        {
            try
            {

                var itemDetails = (from tb in _dbContext.BPCOFHeaders
                                   where tb.DocNumber == PO
                                   join tb1 in _dbContext.BPCOFItems on tb.DocNumber equals tb1.DocNumber
                                   where tb1.PatnerID == PartnerID && tb.IsActive == true
                                   select new ItemDetails
                                   {
                                       Item = tb1.Item,
                                       Material = tb1.Material,
                                       MaterialText = tb1.MaterialText,
                                       DeliveryDate = tb1.DeliveryDate,
                                       Proposeddeliverydate = tb1.AckDeliveryDate,
                                       GRQty = tb1.CompletedQty,
                                       OrderQty = tb1.OrderedQty,
                                       PipelineQty = (double)tb1.TransitQty,
                                       OpenQty = tb1.OpenQty,
                                       UOM = tb1.UOM,
                                       PlantCode = tb1.PlantCode,
                                       UnitPrice = tb1.UnitPrice,
                                       Value = tb1.Value,
                                       TaxAmount = tb1.TaxAmount,
                                       TaxCode = tb1.TaxCode
                                   }).Distinct().ToList();
                return itemDetails;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/GetOfItemsByPartnerIDAndDocNumber", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/GetOfItemsByPartnerIDAndDocNumber", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("DashboardRepositorie/GetOfItemsByPartnerIDAndDocNumber", ex);
                return null;
            }
        }

        public List<ASNDetails> GetOfASNsByPartnerIDAndDocNumber(string PartnerID, string PO)
        {
            try
            {

                var asnDetails = (from tb in _dbContext.BPCASNHeaders
                                  join tb1 in _dbContext.BPCOFHeaders on tb.DocNumber equals tb1.DocNumber
                                  where tb.DocNumber == PO && tb1.PatnerID == PartnerID && tb.IsActive == true
                                  select new ASNDetails
                                  {
                                      ASN = tb.ASNNumber,
                                      Date = tb.CreatedOn,
                                      Status = tb1.Status,
                                      Truck = tb.VessleNumber
                                  }).Distinct().ToList();

                return asnDetails;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/GetOfASNsByPartnerIDAndDocNumber", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/GetOfASNsByPartnerIDAndDocNumber", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("DashboardRepositorie/GetOfASNsByPartnerIDAndDocNumber", ex);
                return null;
            }
        }

        public List<GRNDetails> GetOfGRGIsByPartnerIDAndDocNumber(string PartnerID, string PO)
        {
            try
            {

                var grnDetails = (from tb in _dbContext.BPCOFGRGIs
                                  where tb.DocNumber == PO
                                  join tb1 in _dbContext.BPCOFHeaders on tb.DocNumber equals tb1.DocNumber
                                  where tb.DocNumber == PO && tb.PatnerID == PartnerID && tb.IsActive == true
                                  select new GRNDetails
                                  {
                                      Item = tb.Item,
                                      MaterialText = tb.MaterialText,
                                      GRNDate = tb.DeliveryDate,
                                      Qty = tb.GRGIQty,
                                      Status = tb1.Status
                                  }).Distinct().ToList();

                return grnDetails;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/GetOfGRGIsByPartnerIDAndDocNumber", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/GetOfGRGIsByPartnerIDAndDocNumber", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("DashboardRepositorie/GetOfGRGIsByPartnerIDAndDocNumber", ex);
                return null;
            }
        }
        public List<GRNDetails> GetGRGIsByPartnerIDAndDocNumber(string PartnerID, string DocNumber)
        {
            try
            {

                var grnDetails = (from tb in _dbContext.BPCOFGRGIs
                                  where tb.DocNumber == DocNumber
                                  join tb1 in _dbContext.BPCOFHeaders on tb.DocNumber equals tb1.DocNumber
                                  where tb.DocNumber == DocNumber && tb.PatnerID == PartnerID && tb.IsActive && tb.GRGIQty >= 0
                                  select new GRNDetails
                                  {
                                      Item = tb.Item,
                                      MaterialText = tb.MaterialText,
                                      GRNDate = tb.DeliveryDate,
                                      Qty = tb.GRGIQty,
                                      Status = tb1.Status
                                  }).Distinct().ToList();

                return grnDetails;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/GetGRGIsByPartnerIDAndDocNumber", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/GetGRGIsByPartnerIDAndDocNumber", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("DashboardRepositorie/GetGRGIsByPartnerIDAndDocNumber", ex);
                return null;
            }
        }
        public List<GRNDetails> GetCancelGRGIsByPartnerIDAndDocNumber(string PartnerID, string DocNumber)
        {
            try
            {

                var grnDetails = (from tb in _dbContext.BPCOFGRGIs
                                  where tb.DocNumber == DocNumber
                                  join tb1 in _dbContext.BPCOFHeaders on tb.DocNumber equals tb1.DocNumber
                                  where tb.DocNumber == DocNumber && tb.PatnerID == PartnerID && tb.IsActive && tb.GRGIQty < 0
                                  select new GRNDetails
                                  {
                                      Item = tb.Item,
                                      MaterialText = tb.MaterialText,
                                      GRNDate = tb.DeliveryDate,
                                      Qty = tb.GRGIQty,
                                      Status = tb1.Status
                                  }).Distinct().ToList();

                return grnDetails;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/GetCancelGRGIsByPartnerIDAndDocNumber", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/GetCancelGRGIsByPartnerIDAndDocNumber", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("DashboardRepositorie/GetCancelGRGIsByPartnerIDAndDocNumber", ex);
                return null;
            }
        }

        public List<QADetails> GetOfQMsByPartnerIDAndDocNumber(string PartnerID, string PO)
        {
            try
            {

                var qaDetails = (from tb in _dbContext.BPCOFQMs
                                 where tb.DocNumber == PO && tb.PatnerID == PartnerID && tb.IsActive == true
                                 select new QADetails
                                 {
                                     Item = tb.Item,
                                     MaterialText = tb.MaterialText,
                                     Date = tb.CreatedOn,
                                     LotQty = tb.LotQty,
                                     RejQty = tb.RejQty,
                                     RejReason = tb.RejReason
                                 }).Distinct().ToList();

                return qaDetails;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/GetOfQMsByPartnerIDAndDocNumber", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/GetOfQMsByPartnerIDAndDocNumber", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("DashboardRepositorie/GetOfQMsByPartnerIDAndDocNumber", ex);
                return null;
            }
        }

        public List<SLDetails> GetOfSLsByPartnerIDAndDocNumber(string PartnerID, string PO)
        {
            try
            {

                var slDetails = (from tb in _dbContext.BPCOFScheduleLines
                                 where tb.DocNumber == PO
                                 join tb1 in _dbContext.BPCOFHeaders on tb.DocNumber equals tb1.DocNumber
                                 where tb.DocNumber == PO && tb.PatnerID == PartnerID && tb.IsActive == true
                                 select new SLDetails
                                 {
                                     Item = tb.Item,
                                     DeliveryDate = tb.DeliveryDate,
                                     SlLine = tb.SlLine,
                                     OrderedQty = tb.OrderedQty,
                                     DocNumber = tb.DocNumber,
                                     OpenQty = tb.OpenQty,
                                 }).Distinct().ToList();

                return slDetails;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/GetOfSLsByPartnerIDAndDocNumber", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/GetOfSLsByPartnerIDAndDocNumber", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("DashboardRepositorie/GetOfSLsByPartnerIDAndDocNumber", ex);
                return null;
            }
        }

        public List<DocumentDetails> GetOfDocumentsByPartnerIDAndDocNumber(string PartnerID, string PO)
        {
            try
            {
                var documentDetails = (from tb in _dbContext.BPCAttachments
                                       where tb.ReferenceNo == PO
                                       join tb1 in _dbContext.BPCOFHeaders on tb.ReferenceNo equals tb1.DocNumber
                                       where tb.ReferenceNo == PO && tb.PatnerID == PartnerID && tb.IsActive == true
                                       select new DocumentDetails()
                                       {
                                           AttachmentID = tb.AttachmentID,
                                           AttachmentName = tb.AttachmentName,
                                           ContentType = tb.ContentType,
                                           ContentLength = tb.ContentLength,
                                           ReferenceNo = tb.ReferenceNo,
                                           PatnerID = tb1.PatnerID,
                                           CreatedOn = tb.CreatedOn,
                                       }).ToList();

                return documentDetails;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/GetOfDocumentsByPartnerIDAndDocNumber", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/GetOfDocumentsByPartnerIDAndDocNumber", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("DashboardRepositorie/GetOfDocumentsByPartnerIDAndDocNumber", ex);
                return null;
            }
        }

        public List<FlipDetails> GetOfFlipsByPartnerIDAndDocNumber(string PartnerID, string PO)
        {
            try
            {

                var flipDetails = (from tb in _dbContext.BPCFLIPHeaders
                                   where tb.DocNumber == PO
                                   join tb1 in _dbContext.BPCOFHeaders on tb.DocNumber equals tb1.DocNumber
                                   where tb.DocNumber == PO && tb.PatnerID == PartnerID && tb.IsActive == true
                                   select new FlipDetails
                                   {
                                       FLIPID = tb.FLIPID,
                                       InvoiceAmount = tb.InvoiceAmount,
                                       InvoiceDate = tb.InvoiceDate,
                                       DocNumber = tb.DocNumber,
                                       InvoiceNumber = tb.InvoiceNumber,
                                       InvoiceType = tb.InvoiceType,
                                       InvoiceCurrency = tb.InvoiceCurrency
                                   }).Distinct().ToList();

                return flipDetails;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/GetOfFlipsByPartnerIDAndDocNumber", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/GetOfFlipsByPartnerIDAndDocNumber", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("DashboardRepositorie/GetOfFlipsByPartnerIDAndDocNumber", ex);
                return null;
            }
        }
        
        //    public async Task<BPCAttachment> UploadOfAttachmentByDocNumber(BPCAttachment BPAttachment, string DocNumber)
        //{
        //    try
        //    {
        //        BPAttachment.IsActive = true;
        //        BPAttachment.CreatedOn = DateTime.Now;
        //        BPAttachment.PatnerID = PartnerID;
        //        BPAttachment.ReferenceNo = DocNumber;
        //        var result = _dbContext.BPCAttachments.Add(BPAttachment);
        //        await _dbContext.SaveChangesAsync();
        //        var bPCOFHeader = _dbContext.BPCOFHeaders.Where(x => x.DocNumber == DocNumber).FirstOrDefault();
        //        if (bPCOFHeader != null)
        //        {
        //            bPCOFHeader.RefDoc = result.Entity.AttachmentID.ToString();
        //        }
        //        await _dbContext.SaveChangesAsync();
        //        return result.Entity;
        //    }
        //    catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/UploadOfAttachment", ex); throw new Exception("Something went wrong"); }
        //    catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/UploadOfAttachment", ex); throw new Exception("Something went wrong"); }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public async Task<BPCAttachment> UploadOfAttachment(string PartnerID, BPCAttachment BPAttachment, string DocNumber)
        {
            try
            {
                BPAttachment.IsActive = true;
                BPAttachment.CreatedOn = DateTime.Now;
                BPAttachment.PatnerID = PartnerID;
                BPAttachment.ReferenceNo = DocNumber;
                var result = _dbContext.BPCAttachments.Add(BPAttachment);
                await _dbContext.SaveChangesAsync();
                var bPCOFHeader = _dbContext.BPCOFHeaders.Where(x => x.DocNumber == DocNumber).FirstOrDefault();
                if (bPCOFHeader != null)
                {
                    bPCOFHeader.RefDoc = result.Entity.AttachmentID.ToString();
                }
                await _dbContext.SaveChangesAsync();
                return result.Entity;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/UploadOfAttachment", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/UploadOfAttachment", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BPCAttachment GetAttachmentByName(string PartnerID, string AttachmentName, string DocNumber)
        {
            try
            {
                return _dbContext.BPCAttachments.FirstOrDefault(x => x.AttachmentName == AttachmentName && x.PatnerID.ToLower() == PartnerID.ToLower()
                && x.ReferenceNo.ToLower() == DocNumber.ToLower());
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/GetAttachmentByName", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/GetAttachmentByName", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public BPCAttachment DownloadOfAttachmentByDocNumber(string AttachmentName, string DocNumber)
        {
            try
            {
                return _dbContext.BPCAttachments.FirstOrDefault(x => x.AttachmentName == AttachmentName 
                && x.ReferenceNo.ToLower() == DocNumber.ToLower());
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/GetAttachmentByName", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/GetAttachmentByName", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        public BPCInvoiceAttachment GetOfAttachmentByPartnerIDAndDocNumber(string PartnerID, string DocNumber)
        {
            try
            {
                return (from tb in _dbContext.BPCOFHeaders
                        join tb1 in _dbContext.BPCAttachments on tb.DocNumber equals tb1.ReferenceNo.ToString()
                        where tb.DocNumber == DocNumber && tb.PatnerID.ToLower() == PartnerID.ToLower()
                        select new BPCInvoiceAttachment()
                        {
                            AttachmentID = tb1.AttachmentID,
                            AttachmentName = tb1.AttachmentName,
                            ContentType = tb1.ContentType,
                            ContentLength = tb1.ContentLength,
                            ReferenceNo = tb1.ReferenceNo,
                            PatnerID = tb1.PatnerID
                        }).FirstOrDefault();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/GetOfAttachmentByPartnerIDAndDocNumber", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/GetOfAttachmentByPartnerIDAndDocNumber", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCInvoiceAttachment> GetOfAttachmentsByPartnerIDAndDocNumber(string PartnerID, string DocNumber)
        {
            try
            {
                var result = (from tb in _dbContext.BPCOFHeaders
                              join tb1 in _dbContext.BPCAttachments on tb.DocNumber equals tb1.ReferenceNo
                              where tb1.ReferenceNo == DocNumber 
                              select new BPCInvoiceAttachment()
                              {
                                  AttachmentID = tb1.AttachmentID,
                                  AttachmentName = tb1.AttachmentName,
                                  ContentType = tb1.ContentType,
                                  ContentLength = tb1.ContentLength,
                                  ReferenceNo = tb1.ReferenceNo,
                                  PatnerID = tb1.PatnerID
                              }).ToList().GroupBy(x => x.AttachmentID);
                var result1 = result.Select(x => x.FirstOrDefault()).ToList(); ;
                return result1;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/GetOfAttachmentsByPartnerIDAndDocNumber", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/GetOfAttachmentsByPartnerIDAndDocNumber", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BPCInvoiceAttachment> GetOfAttachmentsByDocNumber(string DocNumber)
        {
            try
            {
                var result = (from tb in _dbContext.BPCOFHeaders
                              join tb1 in _dbContext.BPCAttachments on tb.DocNumber equals tb1.ReferenceNo
                              where tb1.ReferenceNo == DocNumber 
                              select new BPCInvoiceAttachment()
                              {
                                  AttachmentID = tb1.AttachmentID,
                                  AttachmentName = tb1.AttachmentName,
                                  ContentType = tb1.ContentType,
                                  ContentLength = tb1.ContentLength,
                                  ReferenceNo = tb1.ReferenceNo,
                                  PatnerID = tb1.PatnerID
                              }).ToList().GroupBy(x => x.AttachmentID);
                var result1 = result.Select(x => x.FirstOrDefault()).ToList(); ;
                return result1;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/GetOfAttachmentsByPartnerIDAndDocNumber", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/GetOfAttachmentsByPartnerIDAndDocNumber", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        #endregion

        #region Ramanji Methods
        public List<PODetails> GetPODetails(string PartnerID)
        {
            try
            {
                var query = (from tb in _dbContext.BPCOFHeaders
                             where (tb.Status == "Open" || tb.Status == "ACK") && tb.PatnerID == PartnerID && tb.IsActive == true
                             select new PODetails
                             {
                                 PO = tb.DocNumber,
                                 PODate = tb.DocDate,
                                 Version = tb.DocVersion,
                                 Status = tb.Status,
                                 Currency = tb.Currency
                             }).ToList();
                return query;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/GetPODetails", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/GetPODetails", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("DashboardRepositorie/GetPODetails", ex);
                return null;
            }
        }

        public List<PODetails> GetAllPOBasedOnDate(POSearch pOSearch)
        {
            try
            {

                if (pOSearch.FromDate != null && pOSearch.ToDate != null && pOSearch.Status == null && pOSearch.Status == "")
                {
                    List<PODetails> query = new List<PODetails>();
                    query = (from tb in _dbContext.BPCOFHeaders
                             where (tb.DocDate >= pOSearch.FromDate) && (tb.DocDate <= pOSearch.ToDate) && tb.PatnerID == pOSearch.PartnerID &&
                             tb.IsActive == true
                             select new PODetails
                             {
                                 PO = tb.DocNumber,
                                 PODate = tb.DocDate,
                                 Version = tb.DocVersion,
                                 Status = tb.Status,
                                 Currency = tb.Currency
                             }).ToList();
                    return query;
                }
                else if (pOSearch.Status != null && pOSearch.Status != "" && pOSearch.FromDate != null && pOSearch.ToDate != null)
                {
                    List<PODetails> query = new List<PODetails>();
                    if (pOSearch.Status == "Open")
                    {
                        query = (from tb in _dbContext.BPCOFHeaders
                                 where tb.Status == "Open" && tb.PatnerID == pOSearch.PartnerID &&
                                 //Where(v => v.Date.Value >= fromdate && v.Date.Value <= toDate)
                                 (tb.DocDate >= pOSearch.FromDate) &&
                                 (tb.DocDate <= pOSearch.ToDate) &&
                                 tb.IsActive == true
                                 select new PODetails
                                 {
                                     PO = tb.DocNumber,
                                     PODate = tb.DocDate,
                                     Version = tb.DocVersion,
                                     Status = tb.Status,
                                     Currency = tb.Currency
                                 }).ToList();
                        return query;
                    }
                    else if (pOSearch.Status == "Completed")
                    {
                        query = (from tb in _dbContext.BPCOFHeaders
                                 where tb.Status == "GRN" && tb.PatnerID == pOSearch.PartnerID &&
                                 //Where(v => v.Date.Value >= fromdate && v.Date.Value <= toDate)
                                 (tb.DocDate >= pOSearch.FromDate) &&
                                 (tb.DocDate <= pOSearch.ToDate) &&
                                 tb.IsActive == true
                                 select new PODetails
                                 {
                                     PO = tb.DocNumber,
                                     PODate = tb.DocDate,
                                     Version = tb.DocVersion,
                                     Status = tb.Status,
                                     Currency = tb.Currency
                                 }).ToList();
                        return query;
                    }
                    else if (pOSearch.Status == "All")
                    {
                        query = (from tb in _dbContext.BPCOFHeaders
                                 where tb.PatnerID == pOSearch.PartnerID &&
                                 //Where(v => v.Date.Value >= fromdate && v.Date.Value <= toDate)
                                 (tb.DocDate >= pOSearch.FromDate) &&
                                 (tb.DocDate <= pOSearch.ToDate) &&
                                 tb.IsActive == true
                                 select new PODetails
                                 {
                                     PO = tb.DocNumber,
                                     PODate = tb.DocDate,
                                     Version = tb.DocVersion,
                                     Status = tb.Status,
                                     Currency = tb.Currency
                                 }).ToList();
                        return query;
                    }
                    return query;
                }
                else
                {
                    List<PODetails> query = new List<PODetails>();
                    query = (from tb in _dbContext.BPCOFHeaders
                             where (tb.DocDate == pOSearch.FromDate) && tb.PatnerID == pOSearch.PartnerID &&
                             tb.IsActive == true
                             select new PODetails
                             {
                                 PO = tb.DocNumber,
                                 PODate = tb.DocDate,
                                 Version = tb.DocVersion,
                                 Status = tb.Status,
                                 Currency = tb.Currency
                             }).ToList();
                    if (query.Count != 0)
                    {
                        return query;
                    }
                    else
                    {
                        query = (from tb in _dbContext.BPCOFHeaders
                                 where (tb.DocDate == pOSearch.ToDate) && tb.PatnerID == pOSearch.PartnerID &&
                                 tb.IsActive == true
                                 select new PODetails
                                 {
                                     PO = tb.DocNumber,
                                     PODate = tb.DocDate,
                                     Version = tb.DocVersion,
                                     Status = tb.Status,
                                     Currency = tb.Currency
                                 }).ToList();
                        if (query.Count != 0)
                        {
                            return query;
                        }
                        else
                        {
                            query = (from tb in _dbContext.BPCOFHeaders
                                     where tb.Status == pOSearch.Status && tb.PatnerID == pOSearch.PartnerID &&
                                     tb.IsActive == true
                                     select new PODetails
                                     {
                                         PO = tb.DocNumber,
                                         PODate = tb.DocDate,
                                         Version = tb.DocVersion,
                                         Status = tb.Status,
                                         Currency = tb.Currency
                                     }).ToList();
                            if (query.Count != 0)
                            {
                                return query;
                            }
                            else if (pOSearch.Status == "All")
                            {
                                query = (from tb in _dbContext.BPCOFHeaders
                                         where tb.PatnerID == pOSearch.PartnerID &&
                                         tb.IsActive == true
                                         select new PODetails
                                         {
                                             PO = tb.DocNumber,
                                             PODate = tb.DocDate,
                                             Version = tb.DocVersion,
                                             Status = tb.Status,
                                             Currency = tb.Currency
                                         }).ToList();
                            }
                            else
                            {
                                query = (from tb in _dbContext.BPCOFHeaders
                                         where tb.Status == "GRN" && tb.PatnerID == pOSearch.PartnerID &&
                                         tb.IsActive == true
                                         select new PODetails
                                         {
                                             PO = tb.DocNumber,
                                             PODate = tb.DocDate,
                                             Version = tb.DocVersion,
                                             Status = tb.Status,
                                             Currency = tb.Currency
                                         }).ToList();
                            }
                        }
                    }
                    return query;
                    //return null;
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/GetAllPOBasedOnDate", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/GetAllPOBasedOnDate", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("DashboardRepositorie/GetAllPOBasedOnDate", ex);
                return null;
            }
        }
        
             public OrderFulfilmentDetails GetOrderFulfilmentDetailsByDocNumber(string PO)
        {
            try
            {
                OrderFulfilmentDetails orderFulfilmentDetails = new OrderFulfilmentDetails();

                var poDetails = (from tb in _dbContext.BPCOFHeaders
                                 where tb.DocNumber == PO  && tb.IsActive == true
                                 select tb).FirstOrDefault();

                var asnDetails = (from tb in _dbContext.BPCASNHeaders
                                  join tb1 in _dbContext.BPCOFHeaders on tb.DocNumber equals tb1.DocNumber
                                  where tb.DocNumber == PO && tb.IsActive == true
                                  select new ASNDetails
                                  {
                                      ASN = tb.ASNNumber,
                                      Date = tb.CreatedOn,
                                      Status = tb1.Status,
                                      Truck = tb.VessleNumber
                                  }).Distinct().ToList();
                var itemDetails = (from tb in _dbContext.BPCOFHeaders
                                   where tb.DocNumber == PO
                                   join tb1 in _dbContext.BPCOFItems on tb.DocNumber equals tb1.DocNumber
                                   where tb.IsActive == true
                                   select new ItemDetails
                                   {
                                       Item = tb1.Item,
                                       Material = tb1.Material,
                                       MaterialText = tb1.MaterialText,
                                       DeliveryDate = tb1.DeliveryDate,
                                       Proposeddeliverydate = tb1.AckDeliveryDate,
                                       GRQty = tb1.CompletedQty,
                                       OrderQty = tb1.OrderedQty,
                                       PipelineQty = tb1.TransitQty,
                                       OpenQty = tb1.OpenQty,
                                       UOM = tb1.UOM,
                                       PlantCode = tb1.PlantCode,
                                       UnitPrice = tb1.UnitPrice,
                                       Value = tb1.Value,
                                       TaxAmount = tb1.TaxAmount,
                                       TaxCode = tb1.TaxCode
                                   }).Distinct().ToList();
                var grnDetails = (from tb in _dbContext.BPCOFGRGIs
                                  where tb.DocNumber == PO
                                  join tb1 in _dbContext.BPCOFHeaders on tb.DocNumber equals tb1.DocNumber
                                  where tb.DocNumber == PO && tb.IsActive && tb.GRGIQty >= 0
                                  select new GRNDetails
                                  {
                                      GRGIDoc = tb.GRGIDoc,
                                      Item = tb.Item,
                                      Material = tb.Material,
                                      MaterialText = tb.MaterialText,
                                      GRNDate = tb.DeliveryDate,
                                      //DeliveryDate = tb.DeliveryDate
                                      Qty = tb.GRGIQty,
                                      Status = tb1.Status
                                  }).Distinct().ToList();
                var qaDetails = (from tb in _dbContext.BPCOFQMs
                                 where tb.DocNumber == PO && tb.IsActive == true
                                 select new QADetails
                                 {
                                     Item = tb.Item,
                                     SerialNumber = tb.SerialNumber,
                                     Material = tb.Material,
                                     MaterialText = tb.MaterialText,
                                     Date = tb.CreatedOn,
                                     LotQty = tb.LotQty,
                                     RejQty = tb.RejQty,
                                     RejReason = tb.RejReason
                                 }).Distinct().ToList();

                var slDetails = (from tb in _dbContext.BPCOFScheduleLines
                                 where tb.DocNumber == PO
                                 join tb1 in _dbContext.BPCOFHeaders on tb.DocNumber equals tb1.DocNumber
                                 join tb2 in _dbContext.BPCOFItems on tb.DocNumber equals tb2.DocNumber
                                 where tb.DocNumber == PO && tb.IsActive == true &&
                                 tb.Item == tb2.Item
                                 select new SLDetails
                                 {
                                     Item = tb.Item,
                                     DeliveryDate = tb.DeliveryDate,
                                     SlLine = tb.SlLine,
                                     OrderedQty = tb.OrderedQty,
                                     DocNumber = tb.DocNumber,
                                     Material = tb2.Material,
                                     Description = tb2.MaterialText,
                                     OpenQty = tb.OpenQty,
                                     UOM = tb2.UOM
                                 }).Distinct().ToList();

                var ReturnDetails = (from tb in _dbContext.BPCOFGRGIs
                                     where tb.DocNumber == PO
                                     join tb1 in _dbContext.BPCOFHeaders on tb.DocNumber equals tb1.DocNumber
                                     where tb.DocNumber == PO && tb.IsActive && tb.GRGIQty < 0
                                     select new GRNDetails
                                     {
                                         GRGIDoc = tb.GRGIDoc,
                                         Item = tb.Item,
                                         Material = tb.Material,
                                         MaterialText = tb.MaterialText,
                                         GRNDate = tb.DeliveryDate,
                                         //DeliveryDate = tb.DeliveryDate
                                         Qty = tb.GRGIQty,
                                         Status = tb1.Status
                                     }).Distinct().ToList();
                //(from tb in _dbContext.BPCPIHeaders
                //                     where tb.PatnerID == PO
                //                     join tb1 in _dbContext.BPCOFHeaders on tb.PatnerID equals tb1.DocNumber
                //                     where tb.PatnerID == PO && tb.PatnerID == PartnerID && tb.IsActive == true
                //                     select new BPCPIHeader
                //                     {
                //                         Date = tb.Date,
                //                         DeliveryNote = tb.DeliveryNote,
                //                         Type = tb.Type,
                //                         Material = tb.Material,
                //                         Description = tb.Description,
                //                         Qty = tb.Qty,
                //                         Reason = tb.Reason
                //                     }).Distinct().ToList();


                var documentDetails = (from tb in _dbContext.BPCOFHeaders
                                       join tb1 in _dbContext.BPCAttachments on tb.DocNumber equals tb1.ReferenceNo
                                       where tb.DocNumber.ToLower() == PO.ToLower()
                                       select new DocumentDetails()
                                       {
                                           AttachmentID = tb1.AttachmentID,
                                           AttachmentName = tb1.AttachmentName,
                                           ContentType = tb1.ContentType,
                                           ContentLength = tb1.ContentLength,
                                           ReferenceNo = tb1.ReferenceNo,
                                           PatnerID = tb1.PatnerID,
                                           CreatedOn = tb1.CreatedOn,
                                       }).ToList();

                var flipDetails = (from tb in _dbContext.BPCFLIPHeaders
                                   where tb.DocNumber == PO
                                   join tb1 in _dbContext.BPCOFHeaders on tb.DocNumber equals tb1.DocNumber
                                   where tb.DocNumber == PO && tb.IsActive == true
                                   select new FlipDetails
                                   {
                                       FLIPID = tb.FLIPID,
                                       InvoiceAmount = tb.InvoiceAmount,
                                       InvoiceDate = tb.InvoiceDate,
                                       DocNumber = tb.DocNumber,
                                       InvoiceNumber = tb.InvoiceNumber,
                                       InvoiceType = tb.InvoiceType,
                                       InvoiceCurrency = tb.InvoiceCurrency
                                   }).Distinct().ToList();


                orderFulfilmentDetails.ItemCount = itemDetails.Count();
                orderFulfilmentDetails.ASNCount = asnDetails.Count();
                orderFulfilmentDetails.GRNCount = grnDetails.Count();
                orderFulfilmentDetails.QACount = qaDetails.Count();
                orderFulfilmentDetails.SLCount = slDetails.Count();
                orderFulfilmentDetails.DocumentCount = documentDetails.Count();
                orderFulfilmentDetails.FlipCount = flipDetails.Count();
                orderFulfilmentDetails.ReturnCount = ReturnDetails.Count();
                orderFulfilmentDetails.PONumber = poDetails.DocNumber;
                orderFulfilmentDetails.PODate = poDetails.DocDate;
                orderFulfilmentDetails.Version = poDetails.DocVersion;
                orderFulfilmentDetails.Currency = poDetails.Currency;
                orderFulfilmentDetails.ACKDate = poDetails.AckDate;
                orderFulfilmentDetails.Status = poDetails.Status;
                orderFulfilmentDetails.aSNDetails = asnDetails;
                orderFulfilmentDetails.ReturnDetails = ReturnDetails;
                orderFulfilmentDetails.itemDetails = itemDetails;
                orderFulfilmentDetails.gRNDetails = grnDetails;
                orderFulfilmentDetails.qADetails = qaDetails;
                orderFulfilmentDetails.slDetails = slDetails;
                orderFulfilmentDetails.documentDetails = documentDetails;
                orderFulfilmentDetails.flipDetails = flipDetails;

                // ACK Status
                if (poDetails != null)
                {
                    if (poDetails.Status == "DueForACK")
                    {
                        bool IsDeliveryDateOver = false;
                        var toDay = DateTime.Now;
                        var ItemList = _dbContext.BPCOFItems.Where(x => x.Client == poDetails.Client && x.Type == poDetails.Type && x.Company == poDetails.Company && x.PatnerID == poDetails.PatnerID && x.DocNumber == poDetails.DocNumber).ToList();
                        ItemList.ForEach(x =>
                        {
                            if (x.DeliveryDate.HasValue)
                            {
                                if (x.DeliveryDate.Value.Date < toDay.Date)
                                {
                                    IsDeliveryDateOver = true;
                                }
                            }
                        });
                        if (IsDeliveryDateOver)
                        {
                            poDetails.Status = "DueForASN";
                            orderFulfilmentDetails.Status = "DueForASN";
                            _dbContext.SaveChanges();
                        }
                    }
                }


                //Gate Status
                switch (orderFulfilmentDetails.Status)
                {
                    case "DueForACK":
                        orderFulfilmentDetails.GateStatus = "DueForGate";
                        break;
                    case "DueForASN":
                        orderFulfilmentDetails.GateStatus = "DueForGate";
                        break;
                    case "DueForGate":
                        orderFulfilmentDetails.GateStatus = "DueForGate";
                        break;
                    case "PartialASN":
                        orderFulfilmentDetails.GateStatus = "PartialGate";
                        break;
                    case "PartialGate":
                        orderFulfilmentDetails.GateStatus = "PartialGate";
                        break;
                    case "DueForGRN":
                        orderFulfilmentDetails.GateStatus = "Completed";
                        break;
                    default:
                        orderFulfilmentDetails.GateStatus = "Completed";
                        break;
                }

                //GRN Status
                var GRNList = _dbContext.BPCOFGRGIs.Where(x => x.Client == poDetails.Client && x.Type == poDetails.Type && x.Company == poDetails.Company && x.PatnerID == poDetails.PatnerID && x.DocNumber == poDetails.DocNumber).ToList();
                switch (orderFulfilmentDetails.Status)
                {
                    case "DueForACK":
                        orderFulfilmentDetails.GRNStatus = "DueForGRN";
                        break;
                    case "DueForASN":
                        orderFulfilmentDetails.GRNStatus = "DueForGRN";
                        break;
                    case "DueForGate":
                        orderFulfilmentDetails.GRNStatus = "DueForGRN";
                        break;
                    case "DueForGRN":
                        orderFulfilmentDetails.GRNStatus = GRNList.Count > 0 ? "PartialGRN" : "DueForGRN";
                        break;
                    case "PartialASN":
                        orderFulfilmentDetails.GRNStatus = GRNList.Count > 0 ? "PartialGRN" : "DueForGRN";
                        break;
                    case "PartialGate":
                        orderFulfilmentDetails.GRNStatus = GRNList.Count > 0 ? "PartialGRN" : "DueForGRN";
                        break;
                    case "PartialGRN":
                        orderFulfilmentDetails.GRNStatus = "PartialGRN";
                        break;
                    case "Completed":
                        orderFulfilmentDetails.GRNStatus = "Completed";
                        break;
                    default:
                        orderFulfilmentDetails.GRNStatus = "Completed";
                        break;
                }

                return orderFulfilmentDetails;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/GetOrderFulfilmentDetails", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/GetOrderFulfilmentDetails", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("DashboardRepositorie/GetOrderFulfilmentDetails", ex);
                return null;
            }
        }


        public OrderFulfilmentDetails GetOrderFulfilmentDetails(string PO, string PartnerID)
        {
            try
            {
                OrderFulfilmentDetails orderFulfilmentDetails = new OrderFulfilmentDetails();

                var poDetails = (from tb in _dbContext.BPCOFHeaders
                                 where tb.DocNumber == PO && tb.PatnerID == PartnerID && tb.IsActive == true
                                 select tb).FirstOrDefault();

                var asnDetails = (from tb in _dbContext.BPCASNHeaders
                                  join tb1 in _dbContext.BPCOFHeaders on tb.DocNumber equals tb1.DocNumber
                                  where tb.DocNumber == PO && tb1.PatnerID == PartnerID && tb.IsActive == true
                                  select new ASNDetails
                                  {
                                      ASN = tb.ASNNumber,
                                      Date = tb.CreatedOn,
                                      Status = tb1.Status,
                                      Truck = tb.VessleNumber
                                  }).Distinct().ToList();
                var itemDetails = (from tb in _dbContext.BPCOFHeaders
                                   where tb.DocNumber == PO
                                   join tb1 in _dbContext.BPCOFItems on tb.DocNumber equals tb1.DocNumber
                                   where tb1.PatnerID == PartnerID && tb.IsActive == true
                                   select new ItemDetails
                                   {
                                       Item = tb1.Item,
                                       Material = tb1.Material,
                                       MaterialText = tb1.MaterialText,
                                       DeliveryDate = tb1.DeliveryDate,
                                       Proposeddeliverydate = tb1.AckDeliveryDate,
                                       GRQty = tb1.CompletedQty,
                                       OrderQty = tb1.OrderedQty,
                                       PipelineQty = tb1.TransitQty,
                                       OpenQty = tb1.OpenQty,
                                       UOM = tb1.UOM,
                                       PlantCode = tb1.PlantCode,
                                       UnitPrice = tb1.UnitPrice,
                                       Value = tb1.Value,
                                       TaxAmount = tb1.TaxAmount,
                                       TaxCode = tb1.TaxCode
                                   }).Distinct().ToList();
                var grnDetails = (from tb in _dbContext.BPCOFGRGIs
                                  where tb.DocNumber == PO
                                  join tb1 in _dbContext.BPCOFHeaders on tb.DocNumber equals tb1.DocNumber
                                  where tb.DocNumber == PO && tb.PatnerID == PartnerID && tb.IsActive && tb.GRGIQty >= 0
                                  select new GRNDetails
                                  {
                                      GRGIDoc = tb.GRGIDoc,
                                      Item = tb.Item,
                                      Material = tb.Material,
                                      MaterialText = tb.MaterialText,
                                      GRNDate = tb.DeliveryDate,
                                      //DeliveryDate = tb.DeliveryDate
                                      Qty = tb.GRGIQty,
                                      Status = tb1.Status
                                  }).Distinct().ToList();
                var qaDetails = (from tb in _dbContext.BPCOFQMs
                                 where tb.DocNumber == PO && tb.PatnerID == PartnerID && tb.IsActive == true
                                 select new QADetails
                                 {
                                     Item = tb.Item,
                                     SerialNumber = tb.SerialNumber,
                                     Material = tb.Material,
                                     MaterialText = tb.MaterialText,
                                     Date = tb.CreatedOn,
                                     LotQty = tb.LotQty,
                                     RejQty = tb.RejQty,
                                     RejReason = tb.RejReason
                                 }).Distinct().ToList();

                var slDetails = (from tb in _dbContext.BPCOFScheduleLines
                                 where tb.DocNumber == PO
                                 join tb1 in _dbContext.BPCOFHeaders on tb.DocNumber equals tb1.DocNumber
                                 join tb2 in _dbContext.BPCOFItems on tb.DocNumber equals tb2.DocNumber
                                 where tb.DocNumber == PO && tb.PatnerID == PartnerID && tb.IsActive == true &&
                                 tb.Item == tb2.Item
                                 select new SLDetails
                                 {
                                     Item = tb.Item,
                                     DeliveryDate = tb.DeliveryDate,
                                     SlLine = tb.SlLine,
                                     OrderedQty = tb.OrderedQty,
                                     DocNumber = tb.DocNumber,
                                     Material = tb2.Material,
                                     Description = tb2.MaterialText,
                                     OpenQty = tb.OpenQty,
                                     UOM = tb2.UOM
                                 }).Distinct().ToList();

                var ReturnDetails = (from tb in _dbContext.BPCOFGRGIs
                                     where tb.DocNumber == PO
                                     join tb1 in _dbContext.BPCOFHeaders on tb.DocNumber equals tb1.DocNumber
                                     where tb.DocNumber == PO && tb.PatnerID == PartnerID && tb.IsActive && tb.GRGIQty < 0
                                     select new GRNDetails
                                     {
                                         GRGIDoc = tb.GRGIDoc,
                                         Item = tb.Item,
                                         Material = tb.Material,
                                         MaterialText = tb.MaterialText,
                                         GRNDate = tb.DeliveryDate,
                                         //DeliveryDate = tb.DeliveryDate
                                         Qty = tb.GRGIQty,
                                         Status = tb1.Status
                                     }).Distinct().ToList();
                //(from tb in _dbContext.BPCPIHeaders
                //                     where tb.PatnerID == PO
                //                     join tb1 in _dbContext.BPCOFHeaders on tb.PatnerID equals tb1.DocNumber
                //                     where tb.PatnerID == PO && tb.PatnerID == PartnerID && tb.IsActive == true
                //                     select new BPCPIHeader
                //                     {
                //                         Date = tb.Date,
                //                         DeliveryNote = tb.DeliveryNote,
                //                         Type = tb.Type,
                //                         Material = tb.Material,
                //                         Description = tb.Description,
                //                         Qty = tb.Qty,
                //                         Reason = tb.Reason
                //                     }).Distinct().ToList();


                var documentDetails = (from tb in _dbContext.BPCOFHeaders
                                       join tb1 in _dbContext.BPCAttachments on tb.DocNumber equals tb1.ReferenceNo
                                       where tb.DocNumber.ToLower() == PO.ToLower() && tb.PatnerID.ToLower() == PartnerID.ToLower()
                                       select new DocumentDetails()
                                       {
                                           AttachmentID = tb1.AttachmentID,
                                           AttachmentName = tb1.AttachmentName,
                                           ContentType = tb1.ContentType,
                                           ContentLength = tb1.ContentLength,
                                           ReferenceNo = tb1.ReferenceNo,
                                           PatnerID = tb1.PatnerID,
                                           CreatedOn = tb1.CreatedOn,
                                       }).ToList();

                var flipDetails = (from tb in _dbContext.BPCFLIPHeaders
                                   where tb.DocNumber == PO
                                   join tb1 in _dbContext.BPCOFHeaders on tb.DocNumber equals tb1.DocNumber
                                   where tb.DocNumber == PO && tb.PatnerID == PartnerID && tb.IsActive == true
                                   select new FlipDetails
                                   {
                                       FLIPID = tb.FLIPID,
                                       InvoiceAmount = tb.InvoiceAmount,
                                       InvoiceDate = tb.InvoiceDate,
                                       DocNumber = tb.DocNumber,
                                       InvoiceNumber = tb.InvoiceNumber,
                                       InvoiceType = tb.InvoiceType,
                                       InvoiceCurrency = tb.InvoiceCurrency
                                   }).Distinct().ToList();


                orderFulfilmentDetails.ItemCount = itemDetails.Count();
                orderFulfilmentDetails.ASNCount = asnDetails.Count();
                orderFulfilmentDetails.GRNCount = grnDetails.Count();
                orderFulfilmentDetails.QACount = qaDetails.Count();
                orderFulfilmentDetails.SLCount = slDetails.Count();
                orderFulfilmentDetails.DocumentCount = documentDetails.Count();
                orderFulfilmentDetails.FlipCount = flipDetails.Count();
                orderFulfilmentDetails.ReturnCount = ReturnDetails.Count();
                orderFulfilmentDetails.PONumber = poDetails.DocNumber;
                orderFulfilmentDetails.PODate = poDetails.DocDate;
                orderFulfilmentDetails.Version = poDetails.DocVersion;
                orderFulfilmentDetails.Currency = poDetails.Currency;
                orderFulfilmentDetails.ACKDate = poDetails.AckDate;
                orderFulfilmentDetails.Status = poDetails.Status;
                orderFulfilmentDetails.aSNDetails = asnDetails;
                orderFulfilmentDetails.ReturnDetails = ReturnDetails;
                orderFulfilmentDetails.itemDetails = itemDetails;
                orderFulfilmentDetails.gRNDetails = grnDetails;
                orderFulfilmentDetails.qADetails = qaDetails;
                orderFulfilmentDetails.slDetails = slDetails;
                orderFulfilmentDetails.documentDetails = documentDetails;
                orderFulfilmentDetails.flipDetails = flipDetails;

                // ACK Status
                if (poDetails != null)
                {
                    if (poDetails.Status == "DueForACK")
                    {
                        bool IsDeliveryDateOver = false;
                        var toDay = DateTime.Now;
                        var ItemList = _dbContext.BPCOFItems.Where(x => x.Client == poDetails.Client && x.Type == poDetails.Type && x.Company == poDetails.Company && x.PatnerID == poDetails.PatnerID && x.DocNumber == poDetails.DocNumber).ToList();
                        ItemList.ForEach(x =>
                        {
                            if (x.DeliveryDate.HasValue)
                            {
                                if (x.DeliveryDate.Value.Date < toDay.Date)
                                {
                                    IsDeliveryDateOver = true;
                                }
                            }
                        });
                        if (IsDeliveryDateOver)
                        {
                            poDetails.Status = "DueForASN";
                            orderFulfilmentDetails.Status = "DueForASN";
                            _dbContext.SaveChanges();
                        }
                    }
                }


                //Gate Status
                switch (orderFulfilmentDetails.Status)
                {
                    case "DueForACK":
                        orderFulfilmentDetails.GateStatus = "DueForGate";
                        break;
                    case "DueForASN":
                        orderFulfilmentDetails.GateStatus = "DueForGate";
                        break;
                    case "DueForGate":
                        orderFulfilmentDetails.GateStatus = "DueForGate";
                        break;
                    case "PartialASN":
                        orderFulfilmentDetails.GateStatus = "PartialGate";
                        break;
                    case "PartialGate":
                        orderFulfilmentDetails.GateStatus = "PartialGate";
                        break;
                    case "DueForGRN":
                        orderFulfilmentDetails.GateStatus = "Completed";
                        break;
                    default:
                        orderFulfilmentDetails.GateStatus = "Completed";
                        break;
                }

                //GRN Status
                var GRNList = _dbContext.BPCOFGRGIs.Where(x => x.Client == poDetails.Client && x.Type == poDetails.Type && x.Company == poDetails.Company && x.PatnerID == poDetails.PatnerID && x.DocNumber == poDetails.DocNumber).ToList();
                switch (orderFulfilmentDetails.Status)
                {
                    case "DueForACK":
                        orderFulfilmentDetails.GRNStatus = "DueForGRN";
                        break;
                    case "DueForASN":
                        orderFulfilmentDetails.GRNStatus = "DueForGRN";
                        break;
                    case "DueForGate":
                        orderFulfilmentDetails.GRNStatus = "DueForGRN";
                        break;
                    case "DueForGRN":
                        orderFulfilmentDetails.GRNStatus = GRNList.Count > 0 ? "PartialGRN" : "DueForGRN";
                        break;
                    case "PartialASN":
                        orderFulfilmentDetails.GRNStatus = GRNList.Count > 0 ? "PartialGRN" : "DueForGRN";
                        break;
                    case "PartialGate":
                        orderFulfilmentDetails.GRNStatus = GRNList.Count > 0 ? "PartialGRN" : "DueForGRN";
                        break;
                    case "PartialGRN":
                        orderFulfilmentDetails.GRNStatus = "PartialGRN";
                        break;
                    case "Completed":
                        orderFulfilmentDetails.GRNStatus = "Completed";
                        break;
                    default:
                        orderFulfilmentDetails.GRNStatus = "Completed";
                        break;
                }

                return orderFulfilmentDetails;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/GetOrderFulfilmentDetails", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/GetOrderFulfilmentDetails", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("DashboardRepositorie/GetOrderFulfilmentDetails", ex);
                return null;
            }
        }

        public BPCPlantMaster GetItemPlantDetails(string PlantCode)
        {
            try
            {
                var result = (from tb in _dbContext.BPCPlantMasters
                              where tb.PlantCode == PlantCode && tb.IsActive
                              select tb).FirstOrDefault();
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/GetItemPlantDetails", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/GetItemPlantDetails", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("DashboardRepositorie/GetItemPlantDetails", ex);
                return null;
            }
        }

        public async Task<BPCOFHeader> CreateAcknowledgement(Acknowledgement acknowledgement)
        {
            var bPCOFHeader = new BPCOFHeader();
            var strategy = _dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var query = (from tb in _dbContext.BPCOFHeaders
                                     where tb.DocNumber == acknowledgement.PONumber && tb.IsActive == true
                                     select tb).FirstOrDefault();
                        if (query != null)
                        {
                            foreach (var item in acknowledgement.ItemDetails)
                            {
                                var query1 = (from tb in _dbContext.BPCOFItems
                                              where tb.DocNumber == query.DocNumber && tb.Item == item.Item && tb.IsActive == true
                                              select tb).FirstOrDefault();
                                var ackdate = Convert.ToDateTime(item.Proposeddeliverydate);
                                query1.AckDeliveryDate = ackdate;
                                query1.AckStatus = "DueForASN";
                                query1.ModifiedOn = DateTime.Now;
                                _dbContext.BPCOFItems.Update(query1);
                                await _dbContext.SaveChangesAsync();
                                var ackdate1 = Convert.ToDateTime(DateTime.Now);
                                query.AckDate = ackdate1;
                                query.Status = "DueForASN";
                                query.AckStatus = "ACK";
                                query.ModifiedOn = DateTime.Now;
                                _dbContext.BPCOFHeaders.Update(query);
                                await _dbContext.SaveChangesAsync();
                            }
                            var aiact = _dbContext.BPCOFAIACTs.Where(x => x.PatnerID.ToLower() == query.PatnerID.ToLower()
                             && x.DocNumber.ToLower() == query.DocNumber.ToLower() && x.Type.ToLower() == "action").FirstOrDefault();
                            if (aiact != null)
                            {
                                aiact.ModifiedOn = DateTime.Now;
                                aiact.Status = "DueForASN";
                                aiact.ActionText = "ASN";
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
                                bPCOFAIACT.Status = "Acknowledged";
                                bPCOFAIACT.ActionText = "ASN";
                                bPCOFAIACT.Date = DateTime.Now;
                                bPCOFAIACT.Time = DateTime.Now.ToLongTimeString();
                                await _aIACTRepository.CreateAIACT(bPCOFAIACT);
                            }
                        }
                        transaction.Commit();
                        transaction.Dispose();
                        bPCOFHeader = query;
                        return bPCOFHeader;
                    }
                    catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/CreateAcknowledgement", ex); throw new Exception("Something went wrong"); }
                    catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/CreateAcknowledgement", ex); throw new Exception("Something went wrong"); }
                    catch (Exception ex)
                    {
                        WriteLog.WriteToFile("DashboardRepositorie/CreateAcknowledgement", ex);
                        transaction.Commit();
                        transaction.Dispose();
                        throw ex;
                    }

                }
            });
            return bPCOFHeader;
        }

        public async Task<Acknowledgement> UpdatePOItems(Acknowledgement acknowledgement)
        {
            var bPCOFHeader = new BPCOFHeader();
            var strategy = _dbContext.Database.CreateExecutionStrategy();
            //await strategy.ExecuteAsync(async () =>
            //{
            //    using (var transaction = _dbContext.Database.BeginTransaction())
            //    {
            try
            {
                foreach (var item in acknowledgement.ItemDetails)
                {
                    var query1 = (from tb in _dbContext.BPCOFItems
                                  where tb.DocNumber == acknowledgement.PONumber && tb.Item == item.Item && tb.IsActive == true
                                  select tb).FirstOrDefault();
                    var ackdate = Convert.ToDateTime(item.Proposeddeliverydate);
                    query1.AckDeliveryDate = ackdate;
                    query1.ModifiedOn = DateTime.Now;
                    await _dbContext.SaveChangesAsync();
                }
                return acknowledgement;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/UpdatePOItems", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/UpdatePOItems", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("DashboardRepositorie/UpdatePOItems", ex);
                throw ex;
            }

            // }
            //});
        }


        #endregion

        public List<SODetails> GetSODetails(string Client, string Company, string Type, string PartnerID)
        {
            try
            {
                var result = (from tb in _dbContext.BPCOFHeaders
                              where tb.Type == Type && tb.PatnerID == PartnerID && tb.Client == Client && tb.Company == Company
                              select new SODetails
                              {
                                  PatnerID = tb.PatnerID,
                                  //PIRNumber = tb.PIRNumber,
                                  //PIRType = tb.PIRType,
                                  //SO = tb.DocNumber,
                                  SODate = tb.DocDate,
                                  Status = tb.Status,
                                  //Version = tb.DocVersion,
                                  //Status = "Open",
                                  Currency = tb.Currency,
                                  DocNumber = tb.DocNumber,
                                  DocVersion = tb.DocVersion
                              }).ToList();
                //var result = (from tb in _dbContext.BPCPIHeaders
                //              where tb.Type == Type && tb.PatnerID == PartnerID && tb.IsActive == true
                //              select new SODetails
                //              {
                //                  PatnerID = tb.PatnerID,
                //                  PIRNumber = tb.PIRNumber,
                //                  PIRType = tb.PIRType,
                //                  //SO = tb.DocNumber,
                //                  SODate = tb.Date,
                //                  //Version = tb.DocVersion,
                //                  Status = "Open",
                //                  Currency = tb.Currency,
                //              }).ToList();
                foreach (SODetails sODetail in result)
                {

                    //BPCOFHeader BPCOFHeader = (from tb in _dbContext.BPCOFHeaders
                    //                           join tb1 in _dbContext.BPCPIHeaders on tb.DocNumber equals tb1.DocumentNumber
                    //                           where tb1.PIRNumber == sODetail.PIRNumber
                    //                           select tb).FirstOrDefault();
                    if (result != null)
                    {
                        sODetail.SO = sODetail.DocNumber;
                        sODetail.Version = sODetail.DocVersion;
                        if (sODetail.Status == "10")
                        {
                            sODetail.Status = "order";
                        }



                        if (sODetail.Status == "20")
                        {
                            sODetail.Status = "partial_Dispatch";
                        }



                        if (sODetail.Status == "30")
                        {
                            sODetail.Status = "Fully_Dispatch";
                        }



                        if (sODetail.Status == "40")
                        {
                            sODetail.Status = "partial_Recived";
                        }
                        if (sODetail.Status == "50")
                        {
                            sODetail.Status = "Fully_Recived";
                        }



                        if (sODetail.Status == "60")
                        {
                            sODetail.Status = "partial_paid";
                        }
                        if (sODetail.Status == "70")
                        {
                            sODetail.Status = "fully_paid";
                        }
                        //sODetail.Status = "SO";
                        //var BPCOFGRGI = (from tb in _dbContext.BPCOFGRGIs
                        //                 where tb.PatnerID.ToLower() == sODetail.PatnerID.ToLower() &&
                        //                 tb.DocNumber.ToLower() == sODetail.SO.ToLower() && tb.IsActive
                        //                 select tb).FirstOrDefault();
                        //if (BPCOFGRGI != null)
                        //{
                        //    sODetail.Status = "Shipped";
                        //}
                        //var BPCInvoice = (from tb in _dbContext.BPCInvoices
                        //                  where tb.PatnerID.ToLower() == sODetail.PatnerID.ToLower() &&
                        //                  tb.PoReference.ToLower() == sODetail.SO.ToLower() && tb.IsActive
                        //                  select tb).FirstOrDefault();
                        //if (BPCInvoice != null)
                        //{
                        //    sODetail.Status = "Invoiced";
                        //    if (BPCInvoice.PaidAmount > 0)
                        //    {
                        //        sODetail.Status = "Receipt";
                        //    }
                        //}
                        //var ofAttachments = _dbContext.BPCAttachments.Where(x => x.PatnerID.ToLower() == sODetail.PatnerID.ToLower()
                        //&& x.ReferenceNo.ToLower() == sODetail.SO.ToLower() && x.IsActive).ToList();
                        //if (ofAttachments != null && ofAttachments.Count > 0)
                        //{
                        //    sODetail.DocCount = ofAttachments.Count;
                        //}
                        //else
                        //{
                        //    sODetail.DocCount = 0;
                        //}

                        sODetail.DocCount = (from tb in _dbContext.BPCAttachments
                                             where tb.PatnerID == sODetail.PatnerID && tb.ReferenceNo == sODetail.SO && tb.IsActive
                                             select tb.AttachmentID).Count();
                    }
                }
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/GetSODetails", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/GetSODetails", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("DashboardRepositorie/GetSODetails", ex);
                throw ex;
            }
        }
        #region SOLook up Documnet
        public List<BPCAttachment> GetAttachmentByPatnerIdAndDocNum(string PartnerID, string DocNum)
        {
            try
            {
                var Doc_result = _dbContext.BPCAttachments.Where(x => x.PatnerID == PartnerID && x.ReferenceNo == DocNum).ToList();
                //return _dbContext.BPCPIHeaders.Where(x => x.PIRType == "PI").ToList();
                return Doc_result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/GetAttachmentByPatnerIdAndDocNum", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/GetAttachmentByPatnerIdAndDocNum", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Dashboard/GetAttachmentByPatnerIdAndDocNum", ex);
                return null;
            }
        }
        #endregion
        public List<SODetails> GetFilteredSODetailsByPartnerID(string Type, string PartnerID, DateTime? FromDate = null, DateTime? ToDate = null, string Status = null)
        {
            try
            {
                bool IsFromDate = FromDate.HasValue;
                bool IsToDate = ToDate.HasValue;
                bool IsStatus = !string.IsNullOrEmpty(Status);
                if (IsStatus)
                {
                    IsStatus = Status.ToLower() != "all";
                }
                var result1 = (from tb in _dbContext.BPCOFHeaders
                               where tb.PatnerID == PartnerID && tb.IsActive && tb.Type == Type
                               && (!IsFromDate || (tb.DocDate.HasValue && tb.DocDate.Value.Date >= FromDate.Value.Date))
                               && (!IsToDate || (tb.DocDate.HasValue && tb.DocDate.Value.Date <= ToDate.Value.Date))

                               select new SODetails
                               {
                                   //PatnerID = tb.PatnerID,
                                   //PIRNumber = tb.PIRNumber,
                                   //PIRType = tb.PIRType,

                                   //SODate = tb.Date,

                                   //Status = "Open",
                                   //Currency = tb.Currency,

                                   PatnerID = tb.PatnerID,

                                   SODate = tb.DocDate,
                                   Status = tb.Status,

                                   Currency = tb.Currency,
                                   DocNumber = tb.DocNumber,
                                   DocVersion = tb.DocVersion
                               }).ToList().GroupBy(x => x.DocNumber);
                var result = result1.Select(x => x.FirstOrDefault()).ToList();
                foreach (SODetails sODetail in result)
                {

                    //BPCOFHeader BPCOFHeader = (from tb in _dbContext.BPCOFHeaders
                    //                           join tb1 in _dbContext.BPCPIHeaders on tb.DocNumber equals tb1.DocumentNumber
                    //                           where tb1.PIRNumber == sODetail.PIRNumber
                    //                           select tb).FirstOrDefault();
                    if (result != null)
                    {
                        sODetail.SO = sODetail.DocNumber;
                        sODetail.Version = sODetail.DocVersion;
                        //if (sODetail.Status == "10")
                        //{
                        //    //sODetail.Status = "order";
                        //}



                        //if (sODetail.Status == "20")
                        //{
                        //    //sODetail.Status = "partial_Dispatch";
                        //}



                        //if (sODetail.Status == "30")
                        //{
                        //    //sODetail.Status = "Fully_Dispatch";
                        //}



                        //if (sODetail.Status == "40")
                        //{
                        //    //sODetail.Status = "partial_Recived";
                        //}
                        //if (sODetail.Status == "50")
                        //{
                        //    //sODetail.Status = "Fully_Recived";
                        //}



                        //if (sODetail.Status == "60")
                        //{
                        //    //sODetail.Status = "partial_paid";
                        //}
                        //if (sODetail.Status == "70")
                        //{
                        //    //sODetail.Status = "fully_paid";
                        //}
                        //sODetail.Status = "SO";
                        //var BPCOFGRGI = (from tb in _dbContext.BPCOFGRGIs
                        //                 where tb.PatnerID.ToLower() == sODetail.PatnerID.ToLower() &&
                        //                 tb.DocNumber.ToLower() == sODetail.SO.ToLower() && tb.IsActive
                        //                 select tb).FirstOrDefault();
                        //if (BPCOFGRGI != null)
                        //{
                        //    sODetail.Status = "Shipped";
                        //}
                        //var BPCInvoice = (from tb in _dbContext.BPCInvoices
                        //                  where tb.PatnerID.ToLower() == sODetail.PatnerID.ToLower() &&
                        //                  tb.PoReference.ToLower() == sODetail.SO.ToLower() && tb.IsActive
                        //                  select tb).FirstOrDefault();
                        //if (BPCInvoice != null)
                        //{
                        //    sODetail.Status = "Invoiced";
                        //    if (BPCInvoice.PaidAmount > 0)
                        //    {
                        //        sODetail.Status = "Receipt";
                        //    }
                        //}

                        //var ofAttachments = _dbContext.BPCAttachments.Where(x => x.PatnerID.ToLower() == sODetail.PatnerID.ToLower()
                        //&& x.ReferenceNo.ToLower() == sODetail.SO.ToLower() && x.IsActive).ToList();
                        //if (ofAttachments != null && ofAttachments.Count > 0)
                        //{
                        //    sODetail.DocCount = ofAttachments.Count;
                        //}
                        //else
                        //{
                        //    sODetail.DocCount = 0;
                        //}

                        sODetail.DocCount = (from tb in _dbContext.BPCAttachments
                                             where tb.PatnerID == sODetail.PatnerID && tb.ReferenceNo == sODetail.SO && tb.IsActive
                                             select tb.AttachmentID).Count();
                    }
                }
                if (IsStatus)
                {

                    var FilteredResult = (from tb in result
                                          where tb.Status == Status
                                          select tb).ToList();
                    foreach (SODetails sODetail in FilteredResult)
                    {
                        if (sODetail.Status == "10")
                        {
                            sODetail.Status = "order";
                        }



                        if (sODetail.Status == "20")
                        {
                            sODetail.Status = "partial_Dispatch";
                        }



                        if (sODetail.Status == "30")
                        {
                            sODetail.Status = "Fully_Dispatch";
                        }



                        if (sODetail.Status == "40")
                        {
                            sODetail.Status = "partial_Recived";
                        }
                        if (sODetail.Status == "50")
                        {
                            sODetail.Status = "Fully_Recived";
                        }



                        if (sODetail.Status == "60")
                        {
                            sODetail.Status = "partial_paid";
                        }
                        if (sODetail.Status == "70")
                        {
                            sODetail.Status = "fully_paid";
                        }
                    }
                    return FilteredResult;

                }
                else
                {
                    foreach (SODetails sODetail in result)
                    {
                        if (sODetail.Status == "10")
                        {
                            sODetail.Status = "order";
                        }



                        if (sODetail.Status == "20")
                        {
                            sODetail.Status = "partial_Dispatch";
                        }



                        if (sODetail.Status == "30")
                        {
                            sODetail.Status = "Fully_Dispatch";
                        }



                        if (sODetail.Status == "40")
                        {
                            sODetail.Status = "partial_Recived";
                        }
                        if (sODetail.Status == "50")
                        {
                            sODetail.Status = "Fully_Recived";
                        }



                        if (sODetail.Status == "60")
                        {
                            sODetail.Status = "partial_paid";
                        }
                        if (sODetail.Status == "70")
                        {
                            sODetail.Status = "fully_paid";
                        }
                    }
                    return result;
                }
                //return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/GetFilteredSODetailsByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/GetFilteredSODetailsByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BPCOFHeader GetBPCOFHeader(string partnerId, string ReferenceNo)
        {
            try
            {
                var bpcHeader = _dbContext.BPCOFHeaders.FirstOrDefault(x => x.PatnerID == partnerId && x.DocNumber == ReferenceNo);
                return bpcHeader;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/GetBPCOFHeader", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/GetBPCOFHeader", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public BPCOFHeader GetBPCOFHeaderByDocNumber(string ReferenceNo)
        {
            try
            {
                var bpcHeader = _dbContext.BPCOFHeaders.FirstOrDefault(x => x.DocNumber == ReferenceNo);
                return bpcHeader;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DashboardRepository/GetBPCOFHeader", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DashboardRepository/GetBPCOFHeader", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
    public class FulfilmentDetails
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string label { get; set; }
    }
    public class FulfilmentStatus
    {
        public FulfilmentDetails OpenDetails { get; set; }
        public FulfilmentDetails ScheduledDetails { get; set; }
        public FulfilmentDetails InProgressDetails { get; set; }
        public FulfilmentDetails PendingDetails { get; set; }
    }
    //public class FulfilmentStatus
    //{
    //    public FulfilmentDetails OpenDetails { get; set; }
    //    public FulfilmentDetails ScheduledDetails { get; set; }
    //    public FulfilmentDetails InProgressDetails { get; set; }
    //    public FulfilmentDetails PendingDetails { get; set; }
    //}
}
