using BPCloud_VP.SupportDeskService.DBContexts;
using BPCloud_VP.SupportDeskService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;  
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BPCloud_VP.SupportDeskService.Repositories
{
    public class SupportDeskRepository : ISupportDeskRepository
    {
        private readonly SupportDeskContext _supportDeskContext;
        IConfiguration _configuration;

        public SupportDeskRepository(SupportDeskContext supportDeskContext, IConfiguration configuration)
        {
            _supportDeskContext = supportDeskContext;
            _configuration = configuration;

        }

        public List<SupportMaster> GetSupportMasters()
        {
            try
            {
                var query = (from tb in _supportDeskContext.SupportMasters where tb.IsActive select tb).ToList();
                return query;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetSupportMasters", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetSupportMasters", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<SupportMasterViews> GetSupportMasterViews()
        {
            try
            {
                var query = (from tb in _supportDeskContext.SupportMasters
                             where tb.IsActive
                             select new SupportMasterViews()
                             {
                                 ReasonCode = tb.ReasonCode,
                                 ReasonText = tb.ReasonText
                             }).ToList();
                return query;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetSupportMasterViews", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetSupportMasterViews", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<string> GetReasonCodes()
        {
            try
            {
                var query = (from tb in _supportDeskContext.SupportMasters where tb.IsActive select tb.ReasonCode).ToList();
                return query;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetReasonCodes", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetReasonCodes", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<SupportAppMaster> GetSupportAppMasters()
        {
            try
            {
                var query = (from tb in _supportDeskContext.SupportAppMasters where tb.IsActive == true select tb).ToList();
                return query;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetSupportAppMasters", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetSupportAppMasters", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<SupportMaster> GetSupportMastersByPartnerID(string PartnerID)
        {
            try
            {
                var query = (from tb in _supportDeskContext.SupportMasters where tb.IsActive == true select tb).ToList();
                return query;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetSupportMastersByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetSupportMastersByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<SupportMaster> CreateSupportMaster(SupportMaster supportMaster)
        {
            try
            {
                SupportMaster supportMaster1 = new SupportMaster();
                var query = (from tb in _supportDeskContext.SupportMasters where tb.ReasonCode == supportMaster.ReasonCode select tb).FirstOrDefault();
                if (query == null)
                {
                    supportMaster.CreatedOn = DateTime.Now;
                    supportMaster.IsActive = true;
                    var result = _supportDeskContext.SupportMasters.Add(supportMaster);
                    await _supportDeskContext.SaveChangesAsync();
                    supportMaster1 = result.Entity;
                    return supportMaster1;
                }
                else
                {
                    throw new Exception($"Reason Code {supportMaster.ReasonCode} already exists");
                    //return supportMaster1;
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SupportDeskRepository/CreateSupportMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SupportDeskRepository/CreateSupportMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SupportMaster> UpdateSupportMaster(SupportMaster supportMaster)
        {
            try
            {
                SupportMaster supportMaster1 = new SupportMaster();
                var query = (from tb in _supportDeskContext.SupportMasters where tb.ReasonCode == supportMaster.ReasonCode select tb).FirstOrDefault();
                if (query != null)
                {
                    query.ReasonText = supportMaster.ReasonText;
                    query.ModifiedOn = DateTime.Now;
                    query.ModifiedBy = supportMaster.ModifiedBy;
                    var result = _supportDeskContext.SupportMasters.Update(query);
                    await _supportDeskContext.SaveChangesAsync();
                    supportMaster1 = result.Entity;
                    return supportMaster1;
                }
                else
                {
                    return supportMaster1;
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SupportDeskRepository/UpdateSupportMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SupportDeskRepository/UpdateSupportMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<SupportMaster> DeleteSupportMaster(SupportMaster supportMaster)
        {
            try
            {
                SupportMaster supportMaster1 = new SupportMaster();
                var query = (from tb in _supportDeskContext.SupportMasters where tb.ReasonCode == supportMaster.ReasonCode select tb).FirstOrDefault();
                if (query != null)
                {
                    var result = _supportDeskContext.SupportMasters.Remove(query);
                    await _supportDeskContext.SaveChangesAsync();
                    supportMaster1 = result.Entity;
                    return supportMaster1;
                }
                else
                {
                    return supportMaster1;
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SupportDeskRepository/DeleteSupportMaster", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SupportDeskRepository/DeleteSupportMaster", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SupportDetails GetSupportDetailsByPartnerAndSupportID(string SupportID, string PartnerID)
        {
            try
            {
                SupportDetails supportDetails = new SupportDetails();
                SupportHeaderView supportHeader = new SupportHeaderView();
                List<SupportLog> supportLogs = new List<SupportLog>();
                List<BPCSupportAttachment> bPCSupportAttachments = new List<BPCSupportAttachment>();
                List<BPCSupportAttachment> bPCSupportLogAttachments = new List<BPCSupportAttachment>();
                //var id = Convert.ToInt32(SupportID);
                var query1 = (from tb in _supportDeskContext.SupportHeaders where tb.SupportID == SupportID && (tb.PatnerID.ToLower() == PartnerID.ToLower()) && tb.IsActive == true select tb).FirstOrDefault();
                if (query1 != null)
                {
                    supportHeader.SupportID = SupportID;
                    supportHeader.Client = query1.Client;
                    supportHeader.Company = query1.Company;
                    supportHeader.Date = query1.Date;
                    supportHeader.PatnerID = query1.PatnerID;
                    supportHeader.Plant = query1.Plant;
                    supportHeader.ReasonCode = query1.ReasonCode;
                    supportHeader.Remarks = query1.Remarks;
                    supportHeader.Status = query1.Status;
                    supportHeader.IsResolved = query1.IsResolved;
                    supportHeader.CreatedBy = query1.CreatedBy;
                    supportHeader.CreatedOn = query1.CreatedOn.Value;
                    supportHeader.Reason = _supportDeskContext.SupportMasters.Where(x => x.ReasonCode.ToLower() == query1.ReasonCode.ToLower() && x.IsActive).Select(x => x.ReasonText).FirstOrDefault();
                    //tb.PatnerID == PartnerID &&
                    var query2 = (from tb in _supportDeskContext.SupportLogs where tb.SupportID == SupportID && tb.IsActive == true select tb).ToList();
                    var query3 = (from tb in _supportDeskContext.BPC_Support_Attachment where tb.SupportID == SupportID && tb.IsActive == true select tb).ToList();
                    supportLogs = query2;
                    bPCSupportAttachments = query3;
                    //if(supportLogs !=null && supportLogs.Count > 0)
                    //{
                    //    foreach (var supportLog in supportLogs)
                    //    {
                    //        var query4 = (from tb in _supportDeskContext.BPC_Support_Attachment where tb.SupportLogID == supportLog.ID.ToString() && tb.IsActive == true select tb).ToList();
                    //    }
                    //}
                    supportDetails.supportHeader = supportHeader;
                    supportDetails.supportLogs = supportLogs;
                    supportDetails.supportAttachments = bPCSupportAttachments;

                }
                return supportDetails;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetSupportDetailsByPartnerAndSupportID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetSupportDetailsByPartnerAndSupportID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public SupportDetails GetSupportDetailsByPartnerIDAndDocRefNo(string DocRefNo, string PartnerID)
        {
            try
            {
                SupportDetails supportDetails = new SupportDetails();
                SupportHeaderView supportHeader = new SupportHeaderView();
                List<SupportLog> supportLogs = new List<SupportLog>();
                List<BPCSupportAttachment> bPCSupportAttachments = new List<BPCSupportAttachment>();
                List<BPCSupportAttachment> bPCSupportLogAttachments = new List<BPCSupportAttachment>();
                var query1 = (from tb in _supportDeskContext.SupportHeaders where tb.DocumentRefNo == DocRefNo && (tb.PatnerID.ToLower() == PartnerID.ToLower()) && tb.IsActive == true select tb).FirstOrDefault();
                if (query1 != null)
                {
                    supportHeader.SupportID = query1.SupportID;
                    supportHeader.Client = query1.Client;
                    supportHeader.Company = query1.Company;
                    supportHeader.Date = query1.Date;
                    supportHeader.PatnerID = query1.PatnerID;
                    supportHeader.ReasonCode = query1.ReasonCode;
                    supportHeader.Remarks = query1.Remarks;
                    supportHeader.Status = query1.Status;
                    supportHeader.IsResolved = query1.IsResolved;
                    supportHeader.Reason = _supportDeskContext.SupportMasters.Where(x => x.ReasonCode.ToLower() == query1.ReasonCode.ToLower() && x.IsActive).Select(x => x.ReasonText).FirstOrDefault();
                    //tb.PatnerID == PartnerID &&
                    var query2 = (from tb in _supportDeskContext.SupportLogs where tb.SupportID.ToString() == query1.SupportID.ToString() && tb.IsActive == true select tb).ToList();
                    var query3 = (from tb in _supportDeskContext.BPC_Support_Attachment where tb.SupportID == query1.SupportID.ToString() && tb.IsActive == true select tb).ToList();
                    supportLogs = query2;
                    bPCSupportAttachments = query3;
                    //if(supportLogs !=null && supportLogs.Count > 0)
                    //{
                    //    foreach (var supportLog in supportLogs)
                    //    {
                    //        var query4 = (from tb in _supportDeskContext.BPC_Support_Attachment where tb.SupportLogID == supportLog.ID.ToString() && tb.IsActive == true select tb).ToList();
                    //    }
                    //}
                    supportDetails.supportHeader = supportHeader;
                    supportDetails.supportLogs = supportLogs;
                    supportDetails.supportAttachments = bPCSupportAttachments;
                }
                return supportDetails;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetSupportDetailsByPartnerIDAndDocRefNo", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetSupportDetailsByPartnerIDAndDocRefNo", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<SupportHeaderView> GetSupportTicketsByPartnerID(string PartnerID)
        {
            try
            {
                List<SupportHeaderView> supportHeaderViews = new List<SupportHeaderView>();
                var supportHeaders = (from tb in _supportDeskContext.SupportHeaders
                                      where (tb.PatnerID.ToLower() == PartnerID.ToLower()) && tb.IsActive
                                      orderby tb.CreatedOn descending
                                      select tb).ToList();
                if (supportHeaders != null && supportHeaders.Count > 0)
                {
                    foreach (var supportHeader in supportHeaders)
                    {
                        SupportHeaderView supportHeaderView = new SupportHeaderView();
                        supportHeaderView.SupportID = supportHeader.SupportID;
                        supportHeaderView.Client = supportHeader.Client;
                        supportHeaderView.Company = supportHeader.Company;
                        supportHeaderView.Type = supportHeader.Type;
                        supportHeaderView.PatnerID = supportHeader.PatnerID;
                        supportHeaderView.Status = supportHeader.Status;
                        supportHeaderView.ReasonCode = supportHeader.ReasonCode;
                        supportHeaderView.Remarks = supportHeader.Remarks;
                        supportHeaderView.Date = supportHeader.Date;
                        supportHeaderView.DocumentRefNo = supportHeader.DocumentRefNo;
                        supportHeaderView.CreatedBy = supportHeader.CreatedBy;
                        supportHeaderView.CreatedOn = supportHeader.CreatedOn.Value;
                        supportHeaderView.IsActive = supportHeader.IsActive;
                        supportHeaderView.IsResolved = supportHeader.IsResolved;
                        supportHeaderView.Reason = _supportDeskContext.SupportMasters.Where(x => x.ReasonCode.ToLower() == supportHeader.ReasonCode.ToLower() && x.IsActive).Select(x => x.ReasonText).FirstOrDefault();
                        supportHeaderViews.Add(supportHeaderView);
                    }
                }
                return supportHeaderViews;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetSupportTicketsByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetSupportTicketsByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<SupportHeaderView> GetSupportTicketsByPlants(string GetPlantByUser)
        {
            try
            {
                string[] Plants = GetPlantByUser.Split(',');
                for (int i = 0; i < Plants.Length; i++)
                {
                    Plants[i] = Plants[i].Trim();
                }
                List<SupportHeaderView> supportHeaderViews = new List<SupportHeaderView>();
                //List<SupportHeaderView> supportHeaderView = new List<SupportHeaderView>();
                for (int i = 0; i < Plants.Length; i++)
                {
                    //supportHeaderViews = (from tb in _supportDeskContext.SupportHeaders
                    //                       where (tb.Plant.ToLower() == Plants[i].ToLower()) && tb.IsActive
                    //                       orderby tb.CreatedOn descending
                    //                       select tb).ToList();

                    var supportHeaders = (from tb in _supportDeskContext.SupportHeaders
                                          where (tb.Plant.ToLower() == Plants[i].ToLower()) && tb.IsActive
                                          orderby tb.CreatedOn descending
                                          select tb).ToList();
                    if (supportHeaders != null && supportHeaders.Count > 0)
                    {
                        foreach (var supportHeader in supportHeaders)
                        {
                            
                            SupportHeaderView supportDesk = new SupportHeaderView();
                            supportDesk.SupportID = supportHeader.SupportID;
                            supportDesk.Client = supportHeader.Client;
                            supportDesk.Company = supportHeader.Company;
                            supportDesk.Type = supportHeader.Type;
                            supportDesk.PatnerID = supportHeader.PatnerID;
                            supportDesk.Status = supportHeader.Status;
                            supportDesk.ReasonCode = supportHeader.ReasonCode;
                            supportDesk.Remarks = supportHeader.Remarks;
                            supportDesk.Date = supportHeader.Date;
                            supportDesk.Plant = supportHeader.Plant;
                            supportDesk.DocumentRefNo = supportHeader.DocumentRefNo;
                            supportDesk.CreatedBy = supportHeader.CreatedBy;
                            supportDesk.CreatedOn = supportHeader.CreatedOn.Value;
                            supportDesk.IsActive = supportHeader.IsActive;
                            supportDesk.IsResolved = supportHeader.IsResolved;
                            supportDesk.DocCount = (from tb in _supportDeskContext.BPC_Support_Attachment
                                                      where tb.SupportID == supportHeader.SupportID && tb.IsActive
                                                      select tb.AttachmentID).Count().ToString();
                            supportDesk.Reason = _supportDeskContext.SupportMasters.Where(x => x.ReasonCode.ToLower() == supportHeader.ReasonCode.ToLower() && x.IsActive).Select(x => x.ReasonText).FirstOrDefault();
                            supportHeaderViews.Add(supportDesk);
                        }
                    }
                   
                    
                    //supportHeaderView.AddRange(supportHeaderViews);
                }
                return supportHeaderViews;
                //return supportHeaderView;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetSupportTicketsByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetSupportTicketsByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<SupportHeaderView> GetTicketSearch(Ticketsearch Ticketsearch)
        {
            try
            {
                bool IsDocNumber = !string.IsNullOrEmpty(Ticketsearch.DocNumber);
                bool IsFromDate = Ticketsearch.FromDate.HasValue;
                bool IsToDate = Ticketsearch.ToDate.HasValue;
                bool IsStatus = !string.IsNullOrEmpty(Ticketsearch.Status);
                
                //string[] Plants = Ticketsearch.Plant.Split(',');
                //for (int i = 0; i < Ticketsearch.Plant.Length; i++)
                //{
                //    Plants[i] = Plants[i].Trim();
                //}
                 
                bool IsPartnerID = !string.IsNullOrEmpty(Ticketsearch.PartnerID);
                //if (IsStatus)
                //{
                //    IsStatus = Ticketsearch.Status.ToLower() != "all";
                //}
                //if (IsPlant)
                //{
                //    IsPlant = Ticketsearch.Plant.ToLower() != "all";
                //}
                List<SupportHeaderView> supportHeaderViews = new List<SupportHeaderView>();
                //List<SupportHeader> supportHeaderView = new List<SupportHeader>();
                for (int i = 0; i < Ticketsearch.Plant.Length; i++)
                {

                    
                    var supportHeader = (from tb in _supportDeskContext.SupportHeaders
                                          where (!IsDocNumber || tb.DocumentRefNo == Ticketsearch.DocNumber) &&
                                           (!IsStatus || tb.Status == Ticketsearch.Status) && (!IsFromDate || tb.CreatedOn.Value.Date >= Ticketsearch.FromDate.Value.Date) && (!IsToDate || tb.CreatedOn.Value.Date <= Ticketsearch.ToDate.Value.Date) && (!IsPartnerID || tb.PatnerID == Ticketsearch.PartnerID) &&
                                          (tb.Plant == Ticketsearch.Plant[i])
                                           && tb.IsActive
                                          select tb).ToList();
                    //var supportHeaders = (from tb in _supportDeskContext.SupportHeaders
                    //                      where ((tb.PatnerID.ToLower() == PartnerID.ToLower()
                    //                      && tb.Type.ToLower() == "b") || tb.Plant == Plant) && tb.IsActive
                    //                      orderby tb.CreatedOn descending
                    //                      select tb).ToList();

                    if (supportHeader != null && supportHeader.Count > 0)
                    {
                        foreach (var support_Header in supportHeader)
                        {
                            SupportHeaderView supportHeaderView = new SupportHeaderView();
                            supportHeaderView.SupportID = support_Header.SupportID;
                            supportHeaderView.Client = support_Header.Client;
                            supportHeaderView.Company = support_Header.Company;
                            supportHeaderView.Type = support_Header.Type;
                            supportHeaderView.PatnerID = support_Header.PatnerID;
                            supportHeaderView.Status = support_Header.Status;
                            supportHeaderView.ReasonCode = support_Header.ReasonCode;
                            supportHeaderView.Remarks = support_Header.Remarks;
                            supportHeaderView.Date = support_Header.Date;
                            supportHeaderView.Plant = support_Header.Plant;
                            supportHeaderView.DocumentRefNo = support_Header.DocumentRefNo;
                            supportHeaderView.CreatedBy = support_Header.CreatedBy;
                            supportHeaderView.CreatedOn = support_Header.CreatedOn.Value;
                            supportHeaderView.IsActive = support_Header.IsActive;
                            supportHeaderView.IsResolved = support_Header.IsResolved;
                            supportHeaderView.Reason = _supportDeskContext.SupportMasters.Where(x => x.ReasonCode.ToLower() == support_Header.ReasonCode.ToLower() && x.IsActive).Select(x => x.ReasonText).FirstOrDefault();
                            supportHeaderViews.Add(supportHeaderView);
                        }
                    }
                    


                    //List<SupportHeader> supportHeaderViews = new List<SupportHeader>();
                    //supportHeaderViews = (from tb in _supportDeskContext.SupportHeaders
                    //                      where (!IsDocNumber || tb.DocumentRefNo == Ticketsearch.DocNumber) &&
                    //                       (!IsStatus || tb.Status == Ticketsearch.Status) && (!IsFromDate || tb.CreatedOn.Value.Date >= Ticketsearch.FromDate.Value.Date) && (!IsToDate || tb.CreatedOn.Value.Date <= Ticketsearch.ToDate.Value.Date) && (!IsPartnerID || tb.PatnerID == Ticketsearch.PartnerID) &&
                    //                      ( tb.Plant == Ticketsearch.Plant[i])
                    //                       && tb.IsActive
                    //                      select tb).ToList();

                    //supportHeaderView.AddRange(supportHeaderViews);
                }

                return supportHeaderViews;
                //return supportHeaderView;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetSupportTicketsByPlants", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetSupportTicketsByPlants", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        

        public SupportDetails GetSupportDetailsBySupportID(string SupportID)
        {
            try
            {
                SupportDetails supportDetails = new SupportDetails();
                SupportHeaderView supportHeader = new SupportHeaderView();
                List<SupportLog> supportLogs = new List<SupportLog>();
                List<BPCSupportAttachment> bPCSupportAttachments = new List<BPCSupportAttachment>();
                List<BPCSupportAttachment> bPCSupportLogAttachments = new List<BPCSupportAttachment>();
                //var id = Convert.ToInt32(SupportID);
                var query1 = (from tb in _supportDeskContext.SupportHeaders
                              where tb.SupportID == SupportID && tb.IsActive
                              select tb).FirstOrDefault();
                if (query1 != null)
                {
                    supportHeader.SupportID = SupportID;
                    supportHeader.Client = query1.Client;
                    supportHeader.Company = query1.Company;
                    supportHeader.Date = query1.Date;
                    supportHeader.PatnerID = query1.PatnerID;
                    supportHeader.DocumentRefNo = query1.DocumentRefNo;
                    supportHeader.ReasonCode = query1.ReasonCode;
                    supportHeader.Remarks = query1.Remarks;
                    supportHeader.Plant = query1.Plant;
                    supportHeader.Status = query1.Status;
                    supportHeader.IsResolved = query1.IsResolved;
                    supportHeader.CreatedBy = query1.CreatedBy;
                    supportHeader.Reason = _supportDeskContext.SupportMasters.Where(x => x.ReasonCode.ToLower() == query1.ReasonCode.ToLower() && x.IsActive).Select(x => x.ReasonText).FirstOrDefault();
                    //tb.PatnerID == PartnerID &&
                    var query2 = (from tb in _supportDeskContext.SupportLogs where tb.SupportID == SupportID && tb.IsActive == true select tb).OrderBy(x => x.CreatedOn).ToList();
                    var query3 = (from tb in _supportDeskContext.BPC_Support_Attachment where tb.SupportID == SupportID && tb.IsActive == true select tb).ToList();
                    supportLogs = query2;
                    bPCSupportAttachments = query3;
                    //if(supportLogs !=null && supportLogs.Count > 0)
                    //{
                    //    foreach (var supportLog in supportLogs)
                    //    {
                    //        var query4 = (from tb in _supportDeskContext.BPC_Support_Attachment where tb.SupportLogID == supportLog.ID.ToString() && tb.IsActive == true select tb).ToList();
                    //    }
                    //}
                    supportDetails.supportHeader = supportHeader;
                    supportDetails.supportLogs = supportLogs;
                    supportDetails.supportAttachments = bPCSupportAttachments;
                }
                return supportDetails;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetSupportDetailsBySupportID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetSupportDetailsBySupportID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public SupportDetails GetSupportDetailsByPartnerAndSupportIDAndType(string SupportID, string PartnerID, string Type)
        {
            try
            {
                SupportDetails supportDetails = new SupportDetails();
                SupportHeaderView supportHeader = new SupportHeaderView();
                List<SupportLog> supportLogs = new List<SupportLog>();
                List<BPCSupportAttachment> bPCSupportAttachments = new List<BPCSupportAttachment>();
                List<BPCSupportAttachment> bPCSupportLogAttachments = new List<BPCSupportAttachment>();
                //var id = Convert.ToInt32(SupportID);
                var query1 = (from tb in _supportDeskContext.SupportHeaders
                              where tb.SupportID == SupportID && (tb.PatnerID.ToLower() == PartnerID.ToLower())
                              && tb.Type.ToLower() == Type.ToLower() && tb.IsActive
                              select tb).FirstOrDefault();
                if (query1 != null)
                {
                    supportHeader.SupportID = SupportID;
                    supportHeader.Client = query1.Client;
                    supportHeader.Company = query1.Company;
                    supportHeader.Date = query1.Date;
                    supportHeader.PatnerID = query1.PatnerID;
                    supportHeader.ReasonCode = query1.ReasonCode;
                    supportHeader.Remarks = query1.Remarks;
                    supportHeader.Status = query1.Status;
                    supportHeader.IsResolved = query1.IsResolved;
                    supportHeader.Reason = _supportDeskContext.SupportMasters.Where(x => x.ReasonCode.ToLower() == query1.ReasonCode.ToLower() && x.IsActive).Select(x => x.ReasonText).FirstOrDefault();
                    //tb.PatnerID == PartnerID &&
                    var query2 = (from tb in _supportDeskContext.SupportLogs where tb.SupportID == SupportID && tb.IsActive == true select tb).ToList();
                    var query3 = (from tb in _supportDeskContext.BPC_Support_Attachment where tb.SupportID == SupportID && tb.IsActive == true select tb).ToList();
                    supportLogs = query2;
                    bPCSupportAttachments = query3;
                    //if(supportLogs !=null && supportLogs.Count > 0)
                    //{
                    //    foreach (var supportLog in supportLogs)
                    //    {
                    //        var query4 = (from tb in _supportDeskContext.BPC_Support_Attachment where tb.SupportLogID == supportLog.ID.ToString() && tb.IsActive == true select tb).ToList();
                    //    }
                    //}
                    supportDetails.supportHeader = supportHeader;
                    supportDetails.supportLogs = supportLogs;
                    supportDetails.supportAttachments = bPCSupportAttachments;

                }
                return supportDetails;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetSupportDetailsByPartnerAndSupportIDAndType", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetSupportDetailsByPartnerAndSupportIDAndType", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public SupportDetails GetSupportDetailsByPartnerIDAndDocRefNoAndType(string DocRefNo, string PartnerID, string Type)
        {
            try
            {
                SupportDetails supportDetails = new SupportDetails();
                SupportHeaderView supportHeader = new SupportHeaderView();
                List<SupportLog> supportLogs = new List<SupportLog>();
                List<BPCSupportAttachment> bPCSupportAttachments = new List<BPCSupportAttachment>();
                List<BPCSupportAttachment> bPCSupportLogAttachments = new List<BPCSupportAttachment>();
                var query1 = (from tb in _supportDeskContext.SupportHeaders
                              where tb.DocumentRefNo == DocRefNo && (tb.PatnerID.ToLower() == PartnerID.ToLower())
                              && tb.Type.ToLower() == Type.ToLower() && tb.IsActive
                              select tb).FirstOrDefault();
                if (query1 != null)
                {
                    supportHeader.SupportID = query1.SupportID;
                    supportHeader.Client = query1.Client;
                    supportHeader.Company = query1.Company;
                    supportHeader.Date = query1.Date;
                    supportHeader.PatnerID = query1.PatnerID;
                    supportHeader.ReasonCode = query1.ReasonCode;
                    supportHeader.Remarks = query1.Remarks;
                    supportHeader.Status = query1.Status;
                    supportHeader.IsResolved = query1.IsResolved;
                    supportHeader.Reason = _supportDeskContext.SupportMasters.Where(x => x.ReasonCode.ToLower() == query1.ReasonCode.ToLower() && x.IsActive).Select(x => x.ReasonText).FirstOrDefault();
                    //tb.PatnerID == PartnerID &&
                    var query2 = (from tb in _supportDeskContext.SupportLogs where tb.SupportID.ToString() == query1.SupportID.ToString() && tb.IsActive == true select tb).ToList();
                    var query3 = (from tb in _supportDeskContext.BPC_Support_Attachment where tb.SupportID == query1.SupportID.ToString() && tb.IsActive == true select tb).ToList();
                    supportLogs = query2;
                    bPCSupportAttachments = query3;
                    //if(supportLogs !=null && supportLogs.Count > 0)
                    //{
                    //    foreach (var supportLog in supportLogs)
                    //    {
                    //        var query4 = (from tb in _supportDeskContext.BPC_Support_Attachment where tb.SupportLogID == supportLog.ID.ToString() && tb.IsActive == true select tb).ToList();
                    //    }
                    //}
                    supportDetails.supportHeader = supportHeader;
                    supportDetails.supportLogs = supportLogs;
                    supportDetails.supportAttachments = bPCSupportAttachments;
                }
                return supportDetails;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetSupportDetailsByPartnerIDAndDocRefNoAndType", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetSupportDetailsByPartnerIDAndDocRefNoAndType", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<SupportHeaderView> GetSupportTicketsByPartnerIDAndType(string PartnerID, string Type)
        {
            try
            {
                List<SupportHeaderView> supportHeaderViews = new List<SupportHeaderView>();
                var supportHeaders = (from tb in _supportDeskContext.SupportHeaders
                                      where (tb.PatnerID.ToLower() == PartnerID.ToLower())
                                      && tb.Type.ToLower() == Type.ToLower() && tb.IsActive
                                      select tb).ToList();
                if (supportHeaders != null && supportHeaders.Count > 0)
                {
                    foreach (var supportHeader in supportHeaders)
                    {
                        SupportHeaderView supportHeaderView = new SupportHeaderView();
                        supportHeaderView.SupportID = supportHeader.SupportID;
                        supportHeaderView.Client = supportHeader.Client;
                        supportHeaderView.Company = supportHeader.Company;
                        supportHeaderView.Type = supportHeader.Type;
                        supportHeaderView.PatnerID = supportHeader.PatnerID;
                        supportHeaderView.Status = supportHeader.Status;
                        supportHeaderView.ReasonCode = supportHeader.ReasonCode;
                        supportHeaderView.Remarks = supportHeader.Remarks;
                        supportHeaderView.Date = supportHeader.Date;
                        supportHeaderView.DocumentRefNo = supportHeader.DocumentRefNo;
                        supportHeaderView.CreatedBy = supportHeader.CreatedBy;
                        supportHeaderView.CreatedOn = supportHeader.CreatedOn.Value;
                        supportHeaderView.IsActive = supportHeader.IsActive;
                        supportHeaderView.IsResolved = supportHeader.IsResolved;
                        supportHeaderView.Reason = _supportDeskContext.SupportMasters.Where(x => x.ReasonCode.ToLower() == supportHeader.ReasonCode.ToLower() && x.IsActive).Select(x => x.ReasonText).FirstOrDefault();
                        supportHeaderViews.Add(supportHeaderView);
                    }
                }
                return supportHeaderViews;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetSupportTicketsByPartnerIDAndType", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetSupportTicketsByPartnerIDAndType", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<SupportHeaderView> GetBuyerSupportTickets(string PartnerID, string Plant)
        {
            try
            {
                List<SupportHeaderView> supportHeaderViews = new List<SupportHeaderView>();
                var supportHeaders = (from tb in _supportDeskContext.SupportHeaders
                                      where ((tb.PatnerID.ToLower() == PartnerID.ToLower()
                                      && tb.Type.ToLower() == "b") || tb.Plant == Plant) && tb.IsActive
                                      orderby tb.CreatedOn descending
                                      select tb).ToList();
                if (supportHeaders != null && supportHeaders.Count > 0)
                {
                    foreach (var supportHeader in supportHeaders)
                    {
                        SupportHeaderView supportHeaderView = new SupportHeaderView();
                        supportHeaderView.SupportID = supportHeader.SupportID;
                        supportHeaderView.Client = supportHeader.Client;
                        supportHeaderView.Company = supportHeader.Company;
                        supportHeaderView.Type = supportHeader.Type;
                        supportHeaderView.PatnerID = supportHeader.PatnerID;
                        supportHeaderView.Status = supportHeader.Status;
                        supportHeaderView.ReasonCode = supportHeader.ReasonCode;
                        supportHeaderView.Remarks = supportHeader.Remarks;
                        supportHeaderView.Date = supportHeader.Date;
                        supportHeaderView.DocumentRefNo = supportHeader.DocumentRefNo;
                        supportHeaderView.CreatedBy = supportHeader.CreatedBy;
                        supportHeaderView.CreatedOn = supportHeader.CreatedOn.Value;
                        supportHeaderView.IsActive = supportHeader.IsActive;
                        supportHeaderView.IsResolved = supportHeader.IsResolved;
                        supportHeaderView.Reason = _supportDeskContext.SupportMasters.Where(x => x.ReasonCode.ToLower() == supportHeader.ReasonCode.ToLower() && x.IsActive).Select(x => x.ReasonText).FirstOrDefault();
                        supportHeaderViews.Add(supportHeaderView);
                    }
                }
                return supportHeaderViews;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetBuyerSupportTickets", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetBuyerSupportTickets", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<SupportHeaderView> GetHelpDeskAdminSupportTickets(HelpDeskAdminDetails helpDeskAdminDetails)
        {
            try
            {
                List<SupportHeaderView> supportHeaderViews = new List<SupportHeaderView>();
                var supportHeaders = (from tb in _supportDeskContext.SupportHeaders
                                      where ((helpDeskAdminDetails.Companies.Any(x => x == tb.Company) &&
                                      helpDeskAdminDetails.ReasonCodes.Any(x => x == tb.ReasonCode))
                                      || tb.CreatedBy == helpDeskAdminDetails.PatnerID) && tb.IsActive
                                      orderby tb.CreatedOn descending
                                      select tb).ToList();
                if (supportHeaders != null && supportHeaders.Count > 0)
                {
                    foreach (var supportHeader in supportHeaders)
                    {
                        SupportHeaderView supportHeaderView = new SupportHeaderView();
                        supportHeaderView.SupportID = supportHeader.SupportID;
                        supportHeaderView.Client = supportHeader.Client;
                        supportHeaderView.Company = supportHeader.Company;
                        supportHeaderView.Type = supportHeader.Type;
                        supportHeaderView.PatnerID = supportHeader.PatnerID;
                        supportHeaderView.Status = supportHeader.Status;
                        supportHeaderView.ReasonCode = supportHeader.ReasonCode;
                        supportHeaderView.Remarks = supportHeader.Remarks;
                        supportHeaderView.Date = supportHeader.Date;
                        supportHeaderView.DocumentRefNo = supportHeader.DocumentRefNo;
                        supportHeaderView.CreatedBy = supportHeader.CreatedBy;
                        supportHeaderView.CreatedOn = supportHeader.CreatedOn.Value;
                        supportHeaderView.IsActive = supportHeader.IsActive;
                        supportHeaderView.IsResolved = supportHeader.IsResolved;
                        supportHeaderView.Reason = _supportDeskContext.SupportMasters.Where(x => x.ReasonCode.ToLower() == supportHeader.ReasonCode.ToLower() && x.IsActive).Select(x => x.ReasonText).FirstOrDefault();
                        supportHeaderViews.Add(supportHeaderView);
                    }
                }
                return supportHeaderViews;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetHelpDeskAdminSupportTickets", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetHelpDeskAdminSupportTickets", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<SupportHeader> CreateSupportTicket(SupportHeaderView supportHeader)
        {
            var supportHeader1 = new SupportHeader();
            var strategy = _supportDeskContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _supportDeskContext.Database.BeginTransaction())
                {
                    try
                    {
                        supportHeader.Client = string.IsNullOrEmpty(supportHeader.Client) ? "900" : supportHeader.Client;
                        supportHeader.Company = string.IsNullOrEmpty(supportHeader.Company) ? "1000" : supportHeader.Company;
                        supportHeader.Type = string.IsNullOrEmpty(supportHeader.Type) ? "S" : supportHeader.Type;
                        supportHeader.PatnerID = string.IsNullOrEmpty(supportHeader.PatnerID) ? "helpuser" : supportHeader.PatnerID;

                        Random random = new Random();
                        int supportID = random.Next(100000000, 999999999);
                        SupportHeader ticket = new SupportHeader();
                        var query = (from tb in _supportDeskContext.SupportMasters where tb.ReasonCode == supportHeader.ReasonCode && tb.IsActive == true select tb).FirstOrDefault();
                        ticket.ReasonCode = supportHeader.ReasonCode;
                        ticket.PatnerID = supportHeader.PatnerID;
                        ticket.Status = "Open";
                        ticket.Date = DateTime.Now;
                        ticket.Client = supportHeader.Client;
                        ticket.Company = supportHeader.Company;
                        ticket.Type = supportHeader.Type;
                        ticket.PatnerID = supportHeader.PatnerID;
                        ticket.AppID = supportHeader.AppID;
                        ticket.Plant = supportHeader.Plant;
                        ticket.SupportID = supportID.ToString("D10");
                        ticket.Remarks = supportHeader.Remarks;
                        ticket.DocumentRefNo = supportHeader.DocumentRefNo;
                        ticket.IsActive = true;
                        ticket.IsResolved = false;
                        ticket.CreatedOn = DateTime.Now;
                        ticket.CreatedBy = supportHeader.CreatedBy;
                        ticket.ModifiedOn = DateTime.Now;
                        var result = _supportDeskContext.SupportHeaders.Add(ticket);
                        await _supportDeskContext.SaveChangesAsync();

                        Random random1 = new Random();
                        int supportLogID = random1.Next(100000000, 999999999);
                        SupportLog itemDetails = new SupportLog();
                        itemDetails.Client = supportHeader.Client;
                        itemDetails.Company = supportHeader.Company;
                        itemDetails.Type = supportHeader.Type;
                        itemDetails.PatnerID = supportHeader.PatnerID;
                        itemDetails.SupportID = result.Entity.SupportID;
                        itemDetails.SupportLogID = supportLogID.ToString("D10");
                        itemDetails.Remarks = supportHeader.Remarks;
                        itemDetails.IsResolved = false;
                        itemDetails.CreatedOn = DateTime.Now;
                        itemDetails.CreatedBy = supportHeader.PatnerID;
                        itemDetails.ModifiedOn = DateTime.Now;
                        itemDetails.IsActive = true;
                        var result1 = _supportDeskContext.SupportLogs.Add(itemDetails);
                        await _supportDeskContext.SaveChangesAsync();

                        //sending mail to support ticket users reponsible for tickets issues

                        if (supportHeader.Users != null && supportHeader.Users.Count > 0)
                        {
                            new Thread(async () =>
                            {
                                await SendMailToSupportTicketUsers(supportHeader.Users, supportHeader.PatnerID);
                            }).Start();
                        }
                        else
                        {


                        }

                        transaction.Commit();
                        transaction.Dispose();
                        supportHeader1 = result.Entity;
                        return supportHeader1;
                    }
                    catch (SqlException ex) { WriteLog.WriteToFile("SupportDeskRepository/CreateSupportTicket", ex); throw new Exception("Something went wrong"); }
                    catch (InvalidOperationException ex) { WriteLog.WriteToFile("SupportDeskRepository/CreateSupportTicket", ex); throw new Exception("Something went wrong"); }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        transaction.Dispose();
                        throw ex;
                    }
                }
            });
            return supportHeader1;
        }

        public List<SupportLog> GetSupportLogsByPartnerAndSupportID(string supportID, string PartnerID)
        {
            try
            {
                //tb.PatnerID == PartnerID &&
                var query = (from tb in _supportDeskContext.SupportLogs where tb.SupportID == supportID && tb.IsActive == true select tb).ToList();
                return query;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetSupportLogsByPartnerAndSupportID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetSupportLogsByPartnerAndSupportID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<SupportLog> CreateSupportLog(SupportLog supportLog)
        {
            try
            {
                SupportHeader supportHeader = new SupportHeader();
                Random random = new Random();
                int supportLogID = random.Next(100000000, 999999999);
                //var id = Convert.ToInt32(supportLog.SupportID);
                //tb.PatnerID == PartnerID && tb.PatnerID == supportLog.PatnerID
                var query = (from tb in _supportDeskContext.SupportHeaders where tb.SupportID == supportLog.SupportID select tb).FirstOrDefault();
                if (query != null)
                {
                    query.Status = "Open";
                    query.IsResolved = false;
                    query.ModifiedOn = DateTime.Now;
                    var result1 = _supportDeskContext.SupportHeaders.Update(query);
                    await _supportDeskContext.SaveChangesAsync();
                }
                SupportLog itemDetails = new SupportLog();

                itemDetails.Client = string.IsNullOrEmpty(supportLog.Client) ? query.Client : supportLog.Client;
                itemDetails.Company = string.IsNullOrEmpty(supportLog.Company) ? query.Company : supportLog.Company;
                itemDetails.Type = string.IsNullOrEmpty(supportLog.Type) ? query.Type : supportLog.Type;
                itemDetails.PatnerID = string.IsNullOrEmpty(supportLog.PatnerID) ? query.PatnerID : supportLog.PatnerID;
                //itemDetails.Client = supportLog.Client;
                //itemDetails.Company = supportLog.Company;
                //itemDetails.Type = supportLog.Type;
                //itemDetails.PatnerID = supportLog.PatnerID;
                itemDetails.SupportID = supportLog.SupportID;
                itemDetails.SupportLogID = supportLogID.ToString("D10");
                itemDetails.Remarks = supportLog.Remarks;
                itemDetails.IsResolved = false;
                itemDetails.CreatedOn = DateTime.Now;
                itemDetails.CreatedBy = supportLog.PatnerID;
                itemDetails.ModifiedOn = DateTime.Now;
                itemDetails.IsActive = true;
                var result = _supportDeskContext.SupportLogs.Add(itemDetails);
                await _supportDeskContext.SaveChangesAsync();
                return result.Entity;

            }
            catch (SqlException ex) { WriteLog.WriteToFile("SupportDeskRepository/CreateSupportLog", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SupportDeskRepository/CreateSupportLog", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SupportLog> UpdateSupportLog(SupportLogView supportLog)
        {
            var supportLog2 = new SupportLog();
            var strategy = _supportDeskContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _supportDeskContext.Database.BeginTransaction())
                {
                    try
                    {
                        Random random = new Random();
                        int supportLogID = random.Next(100000000, 999999999);
                        SupportHeader supportHeader = new SupportHeader();
                        //var id = Convert.ToInt32(supportLog.SupportID);
                        //tb.PatnerID == PartnerID &&
                        var query = (from tb in _supportDeskContext.SupportHeaders where tb.SupportID == supportLog.SupportID select tb).FirstOrDefault();
                        if (query != null)
                        {
                            query.Status = "Resolved";
                            query.IsResolved = true;
                            query.ModifiedOn = DateTime.Now;
                            var result1 = _supportDeskContext.SupportHeaders.Update(query);
                            await _supportDeskContext.SaveChangesAsync();
                        }
                        SupportLog supportLog1 = new SupportLog();
                        supportLog1.SupportID = supportLog.SupportID;
                        supportLog1.SupportLogID = supportLogID.ToString("D10");
                        supportLog1.Client = string.IsNullOrEmpty(supportLog.Client) ? query.Client : supportLog.Client;
                        supportLog1.Company = string.IsNullOrEmpty(supportLog.Company) ? query.Company : supportLog.Company;
                        supportLog1.Type = string.IsNullOrEmpty(supportLog.Type) ? query.Type : supportLog.Type;
                        supportLog1.PatnerID = string.IsNullOrEmpty(supportLog.PatnerID) ? query.PatnerID : supportLog.PatnerID;
                        supportLog1.Remarks = supportLog.Remarks;
                        supportLog1.IsResolved = true;
                        supportLog1.CreatedOn = DateTime.Now;
                        supportLog1.CreatedBy = supportLog.PatnerID;
                        supportLog1.ModifiedOn = DateTime.Now;
                        supportLog1.IsActive = true;
                        var result = _supportDeskContext.SupportLogs.Add(supportLog1);
                        await _supportDeskContext.SaveChangesAsync();
                        //if (supportLog1.Status == "Closed")
                        //{
                        //    await SendMailToTicketCreator(supportLog.PatnerEmail, supportLog.PatnerID);
                        //}
                        transaction.Commit();
                        transaction.Dispose();
                        supportLog2 = result.Entity;
                        if (query != null && query.Status == "Resolved" && query.ReasonCode == "1236")
                        {
                            List<UserView> users = await HelpdeskUserByCompany(query.Company);
                            var attachments = (from tb in _supportDeskContext.BPC_Support_Attachment
                                               where tb.SupportID == query.SupportID
                                               select tb).ToList();
                            await SendMail(attachments, users, query.PatnerID);

                        }
                        return supportLog2;

                    }
                    catch (SqlException ex) { WriteLog.WriteToFile("SupportDeskRepository/UpdateSupportLog", ex); throw new Exception("Something went wrong"); }
                    catch (InvalidOperationException ex) { WriteLog.WriteToFile("SupportDeskRepository/UpdateSupportLog", ex); throw new Exception("Something went wrong"); }
                    catch (Exception ex)
                    {
                        transaction.Commit();
                        transaction.Dispose();
                        throw ex;
                    }
                }
            });
            return supportLog2;
        }

        public async Task<List<UserView>> HelpdeskUserByCompany(string Company)
        {
            try
            {
                List<UserView> user = new List<UserView>();
                string BaseAddress = _configuration.GetValue<string>("APIBaseAddress");
                string HostURI = BaseAddress + "/authenticationapi/Master/HelpdeskUserByCompany?Company=" + Company;
                var uri = new Uri(HostURI);
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "GET";
                try
                {
                    using (var response = (HttpWebResponse)await request.GetResponseAsync())
                    {
                        if (response != null && response.StatusCode == HttpStatusCode.OK)
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            user = JsonConvert.DeserializeObject<List<UserView>>(responseString);
                            reader.Close();
                            return user;
                        }
                        else
                        {
                            var reader = new StreamReader(response.GetResponseStream());
                            string responseString = await reader.ReadToEndAsync();
                            reader.Close();
                            return user;
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
                catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/HelpdeskUserByCompany", ex); throw new Exception("Something went wrong"); }
                catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/HelpdeskUserByCompany", ex); throw new Exception("Something went wrong"); }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/HelpdeskUserByCompany", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/HelpdeskUserByCompany", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SendMail(List<BPCSupportAttachment> attachments, List<UserView> users, string PatnerID)
        {
            try
            {
                var STMPDetailsConfig = _configuration.GetSection("STMPDetails");
                string hostName = STMPDetailsConfig["Host"];
                string SMTPEmail = STMPDetailsConfig["Email"];
                string SMTPEmailPassword = STMPDetailsConfig["Password"];
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
                            </style>";


                body = body + $@"<html><head></head><body> <div style='border:1px solid #dbdbdb;'> <div style='padding: 20px 20px; background-color: #fff06769;text-align: center;font-family: Segoe UI;'> <p> 
                              <h2>Emami Vendor Portal</h2> </p> </div> <div style='background-color: #f8f7f7;padding: 20px 20px;font-family: Segoe UI'> <div style='padding: 20px 20px;border:1px solid white;background-color: white !important'> 
                              <p>Dear concern,</p> <p>Master data change ticket of user {PatnerID} has been closed.</p> 
                              <p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>";
                sb.Append(@body);
                subject = "Master data change ticket closure";
                SmtpClient client = new SmtpClient();
                client.Port = Convert.ToInt32(SMTPPort);
                client.Host = hostName;
                client.EnableSsl = true;
                client.Timeout = 60000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(SMTPEmail, SMTPEmailPassword);
                //MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, subject, sb.ToString());
                MailMessage reportEmail = new MailMessage();
                reportEmail.From = new MailAddress(SMTPEmail);
                var Emails = users.Select(x => x.Email).ToList();
                foreach (var email in Emails)
                {
                    reportEmail.To.Add(email);
                }


                foreach (var attach in attachments)
                {
                    Stream stream = new MemoryStream(attach.AttachmentFile);
                    var attachment = new System.Net.Mail.Attachment(stream, attach.AttachmentName, attach.ContentType);
                    reportEmail.Attachments.Add(attachment);
                }
                reportEmail.Subject = subject;
                reportEmail.Body = sb.ToString();
                reportEmail.BodyEncoding = UTF8Encoding.UTF8;
                reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                reportEmail.IsBodyHtml = true;
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                await client.SendMailAsync(reportEmail);
                WriteLog.WriteToFile($"Master data change ticket closure details sent successfully to {string.Join(",", Emails)}");
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
                        WriteLog.WriteToFile("FactService-factRepository/SendMail/MailboxBusy/MailboxUnavailable/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                    else
                    {
                        WriteLog.WriteToFile("FactService-factRepository/SendMail/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                }
                WriteLog.WriteToFile("FactService-factRepository/SendMail/SmtpFailedRecipientsException:- " + ex.Message, ex);
                return false;
            }
            catch (SmtpException ex)
            {
                WriteLog.WriteToFile("FactService-factRepository/SendMail/SmtpException:- " + ex.Message, ex);
                return false;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FactRepository/SendMail", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FactRepository/SendMail", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("FactService-factRepository/SendMail/Exception:- " + ex.Message, ex);
                return false;
            }
        }


        public async Task<SupportLog> ReOpenSupportTicket(SupportLogView supportLog)
        {
            var supportLog2 = new SupportLog();
            var strategy = _supportDeskContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _supportDeskContext.Database.BeginTransaction())
                {
                    try
                    {
                        Random random = new Random();
                        int supportLogID = random.Next(100000000, 999999999);
                        SupportHeader supportHeader = new SupportHeader();
                        //var id = Convert.ToInt32(supportLog.SupportID);
                        //tb.PatnerID == PartnerID &&
                        var query = (from tb in _supportDeskContext.SupportHeaders where tb.SupportID == supportLog.SupportID select tb).FirstOrDefault();
                        if (query != null)
                        {
                            query.Status = "ReOpen";
                            query.IsResolved = false;
                            query.ModifiedOn = DateTime.Now;
                            var result1 = _supportDeskContext.SupportHeaders.Update(query);
                            await _supportDeskContext.SaveChangesAsync();
                        }
                        SupportLog supportLog1 = new SupportLog();
                        supportLog1.SupportID = supportLog.SupportID;
                        supportLog1.SupportLogID = supportLogID.ToString("D10");
                        supportLog1.Client = supportLog.Client;
                        supportLog1.Company = supportLog.Company;
                        supportLog1.Type = supportLog.Type;
                        supportLog1.PatnerID = supportLog.PatnerID;
                        supportLog1.Remarks = supportLog.Remarks;
                        supportLog1.IsResolved = false;
                        supportLog1.CreatedOn = DateTime.Now;
                        supportLog1.CreatedBy = supportLog.PatnerID;
                        supportLog1.ModifiedOn = DateTime.Now;
                        supportLog1.IsActive = true;
                        var result = _supportDeskContext.SupportLogs.Add(supportLog1);
                        await _supportDeskContext.SaveChangesAsync();
                        //if (supportLog1.Status == "Closed")
                        //{
                        //    await SendMailToTicketCreator(supportLog.PatnerEmail, supportLog.PatnerID);
                        //}
                        transaction.Commit();
                        transaction.Dispose();
                        supportLog2 = result.Entity;
                        return supportLog2;

                    }
                    catch (SqlException ex) { WriteLog.WriteToFile("SupportDeskRepository/ReOpenSupportTicket", ex); throw new Exception("Something went wrong"); }
                    catch (InvalidOperationException ex) { WriteLog.WriteToFile("SupportDeskRepository/ReOpenSupportTicket", ex); throw new Exception("Something went wrong"); }
                    catch (Exception ex)
                    {
                        transaction.Commit();
                        transaction.Dispose();
                        throw ex;
                    }
                }
            });
            return supportLog2;
        }

        public async Task<SupportHeader> CloseSupportTicket(SupportHeader supportHeader)
        {
            try
            {
                var query = (from tb in _supportDeskContext.SupportHeaders where tb.SupportID == supportHeader.SupportID && tb.ReasonCode == supportHeader.ReasonCode select tb).FirstOrDefault();
                if (query != null)
                {
                    query.IsResolved = true;
                    query.Status = "Closed";
                    await _supportDeskContext.SaveChangesAsync();
                    return supportHeader;
                }
                else
                {
                    throw new Exception("Record not found");
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SupportDeskRepository/CloseSupportTicket", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SupportDeskRepository/CloseSupportTicket", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> SendMailToSupportTicketUsers(List<User> users, string PatnerID)
        {
            try
            {
                var STMPDetailsConfig = _configuration.GetSection("STMPDetails");
                string hostName = STMPDetailsConfig["Host"];
                string SMTPEmail = STMPDetailsConfig["Email"];
                //string fromEmail = ConfigurationManager.AppSettings["FromEmail"];
                string SMTPEmailPassword = STMPDetailsConfig["Password"];
                string SMTPPort = STMPDetailsConfig["Port"];
                foreach (var user in users)
                {
                    var message = new MailMessage();
                    string subject = "";
                    StringBuilder sb = new StringBuilder();
                    //string UserName = _dbContext.TBL_User_Master.Where(x => x.Email == toEmail).Select(y => y.UserName).FirstOrDefault();
                    //UserName = string.IsNullOrEmpty(UserName) ? toEmail.Split('@')[0] : UserName;
                    sb.Append(string.Format("Dear {0},<br/>", user.Email));
                    sb.Append("<p>Ticket is created</p>");
                    //sb.Append("<p>Please Login by clicking <a href=\"" + siteURL + "/#/auth/login\">here</a></p>");
                    //sb.Append(string.Format("<p>User name: {0}</p>", toEmail));
                    //sb.Append(string.Format("<p>Password: {0}</p>", password));
                    sb.Append("<p>Regards,</p><p>" + PatnerID + " </p>");
                    subject = "Support Desk Ticket";
                    SmtpClient client = new SmtpClient();
                    client.Port = Convert.ToInt32(SMTPPort);
                    client.Host = hostName;
                    client.EnableSsl = true;
                    client.Timeout = 60000;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential(SMTPEmail.Trim(), SMTPEmailPassword.Trim());
                    MailMessage reportEmail = new MailMessage(SMTPEmail, user.Email, subject, sb.ToString());
                    reportEmail.BodyEncoding = UTF8Encoding.UTF8;
                    reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                    reportEmail.IsBodyHtml = true;
                    await client.SendMailAsync(reportEmail);
                }
                return true;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SupportDeskRepository/SendMailToSupportTicketUsers", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SupportDeskRepository/SendMailToSupportTicketUsers", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SendMailToTicketCreator(string Email, string PatnerID)
        {
            try
            {
                var STMPDetailsConfig = _configuration.GetSection("STMPDetails");
                string hostName = STMPDetailsConfig["Host"];
                string SMTPEmail = STMPDetailsConfig["Email"];
                //string fromEmail = ConfigurationManager.AppSettings["FromEmail"];
                string SMTPEmailPassword = STMPDetailsConfig["Password"];
                string SMTPPort = STMPDetailsConfig["Port"];
                var message = new MailMessage();
                string subject = "";
                StringBuilder sb = new StringBuilder();
                //string UserName = _dbContext.TBL_User_Master.Where(x => x.Email == toEmail).Select(y => y.UserName).FirstOrDefault();
                //UserName = string.IsNullOrEmpty(UserName) ? toEmail.Split('@')[0] : UserName;
                sb.Append(string.Format("Dear {0},<br/>", Email));
                sb.Append("<p>Ticket is closed</p>");
                //sb.Append("<p>Please Login by clicking <a href=\"" + siteURL + "/#/auth/login\">here</a></p>");
                //sb.Append(string.Format("<p>User name: {0}</p>", toEmail));
                //sb.Append(string.Format("<p>Password: {0}</p>", password));
                sb.Append("<p>Regards,</p><p>" + PatnerID + " </p>");
                subject = "Support Desk Ticket";
                SmtpClient client = new SmtpClient();
                client.Port = Convert.ToInt32(SMTPPort);
                client.Host = hostName;
                client.EnableSsl = true;
                client.Timeout = 60000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(SMTPEmail.Trim(), SMTPEmailPassword.Trim());
                MailMessage reportEmail = new MailMessage(SMTPEmail, Email, subject, sb.ToString());
                reportEmail.BodyEncoding = UTF8Encoding.UTF8;
                reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                reportEmail.IsBodyHtml = true;
                await client.SendMailAsync(reportEmail);
                return true;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SupportDeskRepository/SendMailToTicketCreator", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SupportDeskRepository/SendMailToTicketCreator", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AddSupportAttachment(List<BPCSupportAttachment> BPAttachments, string SupportID)
        {
            try
            {
                foreach (BPCSupportAttachment BPAttachment in BPAttachments)
                {
                    BPAttachment.SupportID = SupportID;
                    BPAttachment.IsActive = true;
                    BPAttachment.CreatedOn = DateTime.Now;
                    var result = _supportDeskContext.BPC_Support_Attachment.Add(BPAttachment);
                    await _supportDeskContext.SaveChangesAsync();
                    //var docCenter = _supportDeskContext.BPCDocumentCenters.Where(x => x.ASNNumber == ASNNumber && x.Filename == BPAttachment.AttachmentName).FirstOrDefault();
                    //if (docCenter != null)
                    //{
                    //    docCenter.AttachmentReferenceNo = result.Entity.AttachmentID.ToString();
                    //}
                    //await _supportDeskContext.SaveChangesAsync();
                    //if (docCenter != null)
                    //{
                    //    var docCenterMaster = _supportDeskContext.BPCDocumentCenterMasters.Where(x => x.DocumentType == docCenter.DocumentType).FirstOrDefault();
                    //    if (docCenterMaster != null && !string.IsNullOrEmpty(docCenterMaster.ForwardMail))
                    //    {
                    //        new Thread(async () =>
                    //        {
                    //            await SendMailDocCenterUser(docCenterMaster.ForwardMail, BPAttachment);
                    //        }).Start();
                    //        //await SendMailDocCenterUser(docCenterMaster.ForwardMail, BPAttachment);
                    //    }
                    //}
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SupportDeskRepository/AddSupportAttachment", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SupportDeskRepository/AddSupportAttachment", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AddSupportLogAttachment(List<BPCSupportAttachment> BPAttachments, string SupportID, string SupportLogID)
        {
            try
            {
                foreach (BPCSupportAttachment BPAttachment in BPAttachments)
                {
                    BPAttachment.SupportID = SupportID;
                    BPAttachment.SupportLogID = SupportLogID;
                    BPAttachment.IsActive = true;
                    BPAttachment.CreatedOn = DateTime.Now;
                    var result = _supportDeskContext.BPC_Support_Attachment.Add(BPAttachment);
                    await _supportDeskContext.SaveChangesAsync();
                    //var docCenter = _supportDeskContext.BPCDocumentCenters.Where(x => x.ASNNumber == ASNNumber && x.Filename == BPAttachment.AttachmentName).FirstOrDefault();
                    //if (docCenter != null)
                    //{
                    //    docCenter.AttachmentReferenceNo = result.Entity.AttachmentID.ToString();
                    //}
                    //await _supportDeskContext.SaveChangesAsync();
                    //if (docCenter != null)
                    //{
                    //    var docCenterMaster = _supportDeskContext.BPCDocumentCenterMasters.Where(x => x.DocumentType == docCenter.DocumentType).FirstOrDefault();
                    //    if (docCenterMaster != null && !string.IsNullOrEmpty(docCenterMaster.ForwardMail))
                    //    {
                    //        new Thread(async () =>
                    //        {
                    //            await SendMailDocCenterUser(docCenterMaster.ForwardMail, BPAttachment);
                    //        }).Start();
                    //        //await SendMailDocCenterUser(docCenterMaster.ForwardMail, BPAttachment);
                    //    }
                    //}
                }
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SupportDeskRepository/AddSupportLogAttachment", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SupportDeskRepository/AddSupportLogAttachment", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SaveFAQAttachment(BPCFAQAttachment BPCFAQAttachment)
        {
            try
            {
                BPCFAQAttachment.IsActive = true;
                BPCFAQAttachment.CreatedOn = DateTime.Now;
                _supportDeskContext.BPCFAQAttachments.ToList().ForEach(x => _supportDeskContext.BPCFAQAttachments.Remove(x));
                await _supportDeskContext.SaveChangesAsync();
                var result = _supportDeskContext.BPCFAQAttachments.Add(BPCFAQAttachment);
                await _supportDeskContext.SaveChangesAsync();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SupportDeskRepository/SaveFAQAttachment", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SupportDeskRepository/SaveFAQAttachment", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BPCFAQAttachment GetFAQAttachment()
        {
            try
            {
                return _supportDeskContext.BPCFAQAttachments.FirstOrDefault();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetFAQAttachment", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetFAQAttachment", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BPCSupportAttachment GetAttachmentByName(string AttachmentName, string SupportID)
        {
            try
            {
                return _supportDeskContext.BPC_Support_Attachment.FirstOrDefault(x => x.AttachmentName == AttachmentName);
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetAttachmentByName", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetAttachmentByName", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCSupportAttachment> GetSupportAttachments()
        {
            try
            {
                var query4 = (from tb in _supportDeskContext.BPC_Support_Attachment where tb.IsActive == true select tb).ToList();
                return query4;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetSupportAttachments", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("SupportDeskRepository/GetSupportAttachments", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
