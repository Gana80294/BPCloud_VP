using BPCloud_VP.ReportService.DBContexts;
using BPCloud_VP.ReportService.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP.ReportService.Repositories
{
    public class PPMRepository : IPPMRepository
    {
        private readonly ReportContext _dbContext;
        public PPMRepository(ReportContext context, IConfiguration configuration)
        {
            _dbContext = context;
        }
        public List<BPCReportPPMHeader> GetPPMReports(string PartnerID)
        {
            try
            {
                List<BPCReportPPMHeader> ppmReports = new List<BPCReportPPMHeader>();
                if (!string.IsNullOrEmpty(PartnerID))
                {
                    ppmReports = _dbContext.BPCReportPPMHeaders.Where(x => x.PatnerID.ToLower() == PartnerID.ToLower() && x.IsActive).ToList();
                }
                return ppmReports;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PPMRepository/GetPPMReports", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PPMRepository/GetPPMReports", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PPMRepository/GetPPMReports : - ", ex);
                throw ex;
            }
        }
        public List<BPCReportPPMHeader> GetPPMReportByDate(PPMReportOption ppmOption)
        {
            try
            {
                List<BPCReportPPMHeader> ppmReports = new List<BPCReportPPMHeader>();
                if (ppmOption.FromDate.HasValue && ppmOption.FromDate != null && ppmOption.ToDate.HasValue && ppmOption.ToDate != null)
                {
                    ppmReports = (from tb in _dbContext.BPCReportPPMHeaders
                                  where tb.IsActive && tb.PatnerID.ToLower() == ppmOption.PartnerID.ToLower()
                                  && (tb.CreatedOn.Date >= ppmOption.FromDate.Value.Date)
                                  && (tb.CreatedOn.Date <= ppmOption.ToDate.Value.Date)
                                  select tb).ToList();
                }
                else if (ppmOption.FromDate.HasValue && ppmOption.FromDate != null && !ppmOption.ToDate.HasValue && ppmOption.ToDate == null)
                {
                    ppmReports = (from tb in _dbContext.BPCReportPPMHeaders
                                  where tb.IsActive && tb.PatnerID.ToLower() == ppmOption.PartnerID.ToLower()
                                  && (tb.CreatedOn.Date >= ppmOption.FromDate.Value.Date)
                                  //&& (tb.CreatedOn.Date <= ppmOption.ToDate.Value.Date)
                                  select tb).ToList();
                }
                else if (!ppmOption.FromDate.HasValue && ppmOption.FromDate == null && ppmOption.ToDate.HasValue && ppmOption.ToDate != null)
                {
                    ppmReports = (from tb in _dbContext.BPCReportPPMHeaders
                                  where tb.IsActive && tb.PatnerID.ToLower() == ppmOption.PartnerID.ToLower()
                                  //&& (tb.CreatedOn.Date >= ppmOption.FromDate.Value.Date)
                                  && (tb.CreatedOn.Date <= ppmOption.ToDate.Value.Date)
                                  select tb).ToList();
                }
                return ppmReports;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PPMRepository/GetPPMReportByDate", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PPMRepository/GetPPMReportByDate", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PPMRepository/GetPPMReportByDate : - ", ex);
                throw ex;
            }
        }
        public List<BPCReportPPMItem> GetPPMItemReportByPeriod(string PartnerID, DateTime? period = null)
        {
            try
            {
                List<BPCReportPPMItem> ppmItemReports = new List<BPCReportPPMItem>();
                if (period.HasValue && period != null)
                {
                    ppmItemReports = (from tb in _dbContext.BPCReportPPMItems
                                      where tb.IsActive && tb.PatnerID.ToLower() == PartnerID.ToLower()
                                      && (tb.Period.Date <= period.Value.Date)
                                      select tb).ToList();
                    return ppmItemReports;
                }
                else
                {
                    ppmItemReports = null;
                }
                return ppmItemReports;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PPMRepository/GetPPMItemReportByPeriod", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PPMRepository/GetPPMItemReportByPeriod", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PPMRepository/GetPPMItemReportByPeriod : - ", ex);
                throw ex;
            }
        }
        public List<BPCReportPPMHeader> GetPPMReportByStatus(PPMReportOption ppmOption)
        {
            try
            {
                List<BPCReportPPMHeader> ppmReports = new List<BPCReportPPMHeader>();
                if (!string.IsNullOrEmpty(ppmOption.Status))
                {
                    ppmReports = (from tb in _dbContext.BPCReportPPMHeaders
                                  where tb.IsActive && tb.PatnerID.ToLower() == ppmOption.PartnerID.ToLower()
                                  select tb).ToList();
                }
                return ppmReports;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("PPMRepository/GetPPMReportByStatus", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("PPMRepository/GetPPMReportByStatus", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PPMRepository/GetPPMReportByOption : - ", ex);
                throw ex;
            }
        }
    }
}
