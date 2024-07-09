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
    public class OverviewRepository : IOverviewRepository
    {
        private readonly ReportContext _dbContext;
        public OverviewRepository(ReportContext context, IConfiguration configuration)
        {
            _dbContext = context;
        }
        public List<BPCReportOV> GetOverviewReports(string PartnerID)
        {
            try
            {
                List<BPCReportOV> overviewReports = new List<BPCReportOV>();
                if (!string.IsNullOrEmpty(PartnerID))
                {
                    overviewReports = _dbContext.BPCReportOVs.Where(x => x.PatnerID.ToLower() == PartnerID.ToLower() && x.IsActive).ToList();
                }
                return overviewReports;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("OverviewRepository/GetOverviewReports", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("OverviewRepository/GetOverviewReports", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("OverviewRepository/GetOverviewReports : - ", ex);
                throw ex;
            }
        }
        public List<BPCReportOV> GetOverviewReportByDate(OverviewReportOption overviewOption)
        {
            try
            {
                List<BPCReportOV> overviewReports = new List<BPCReportOV>();
                if (overviewOption.FromDate.HasValue && overviewOption.FromDate != null && overviewOption.ToDate.HasValue && overviewOption.ToDate != null)
                {
                    overviewReports = (from tb in _dbContext.BPCReportOVs
                                       where tb.IsActive && tb.PatnerID.ToLower() == overviewOption.PartnerID.ToLower()
                                       && (tb.CreatedOn.Date >= overviewOption.FromDate.Value.Date)
                                       && (tb.CreatedOn.Date <= overviewOption.ToDate.Value.Date)
                                       select tb).ToList();
                }
                else if (overviewOption.FromDate.HasValue && overviewOption.FromDate != null && !overviewOption.ToDate.HasValue && overviewOption.ToDate == null)
                {
                    overviewReports = (from tb in _dbContext.BPCReportOVs
                                       where tb.IsActive && tb.PatnerID.ToLower() == overviewOption.PartnerID.ToLower()
                                       && (tb.CreatedOn.Date >= overviewOption.FromDate.Value.Date)
                                       //&& (tb.CreatedOn.Date <= overviewOption.ToDate.Value.Date)
                                       select tb).ToList();
                }
                else if (!overviewOption.FromDate.HasValue && overviewOption.FromDate == null && overviewOption.ToDate.HasValue && overviewOption.ToDate != null)
                {
                    overviewReports = (from tb in _dbContext.BPCReportOVs
                                       where tb.IsActive && tb.PatnerID.ToLower() == overviewOption.PartnerID.ToLower()
                                       //&& (tb.CreatedOn.Date >= overviewOption.FromDate.Value.Date)
                                       && (tb.CreatedOn.Date <= overviewOption.ToDate.Value.Date)
                                       select tb).ToList();
                }
                return overviewReports;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("OverviewRepository/GetOverviewReportByDate", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("OverviewRepository/GetOverviewReportByDate", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("OverviewRepository/GetOverviewReportByDate : - ", ex);
                throw ex;
            }
        }
        public List<BPCReportOV> GetOverviewReportByOption(OverviewReportOption overviewOption)
        {
            try
            {
                List<BPCReportOV> overviewReports = new List<BPCReportOV>();
                if (!string.IsNullOrEmpty(overviewOption.Material) && string.IsNullOrEmpty(overviewOption.PO))
                {
                    overviewReports = (from tb in _dbContext.BPCReportOVs
                                       where tb.IsActive && tb.PatnerID.ToLower() == overviewOption.PartnerID.ToLower()
                                       && tb.Material.ToLower() == overviewOption.Material.ToLower()
                                       select tb).ToList();
                }
                else if (string.IsNullOrEmpty(overviewOption.Material) && !string.IsNullOrEmpty(overviewOption.PO))
                {
                    overviewReports = (from tb in _dbContext.BPCReportOVs
                                       where tb.IsActive && tb.PatnerID.ToLower() == overviewOption.PartnerID.ToLower()
                                       && tb.Material.ToLower() == overviewOption.PO.ToLower()
                                       select tb).ToList();
                }
                else if (!string.IsNullOrEmpty(overviewOption.Material) && !string.IsNullOrEmpty(overviewOption.PO))
                {
                    overviewReports = (from tb in _dbContext.BPCReportOVs
                                       where tb.IsActive && tb.PatnerID.ToLower() == overviewOption.PartnerID.ToLower()
                                       && tb.Material.ToLower() == overviewOption.Material.ToLower()
                                       && tb.Material.ToLower() == overviewOption.PO.ToLower()
                                       select tb).ToList();
                }
                else
                {
                    overviewReports = null;
                }
                return overviewReports;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("OverviewRepository/GetOverviewReportByOption", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("OverviewRepository/GetOverviewReportByOption", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("OverviewRepository/GetOverviewReportByOption : - ", ex);
                throw ex;
            }
        }
        public List<BPCReportOV> GetOverviewReportByStatus(OverviewReportOption overviewOption)
        {
            try
            {
                List<BPCReportOV> overviewReports = new List<BPCReportOV>();
                if (!string.IsNullOrEmpty(overviewOption.Status))
                {
                    overviewReports = (from tb in _dbContext.BPCReportOVs
                                       where tb.IsActive && tb.PatnerID.ToLower() == overviewOption.PartnerID.ToLower()
                                      && tb.Status.ToLower() == overviewOption.Status.ToLower()
                                       select tb).ToList();
                }
                return overviewReports;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("OverviewRepository/GetOverviewReportByStatus", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("OverviewRepository/GetOverviewReportByStatus", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("OverviewRepository/GetOverviewReportByOption : - ", ex);
                throw ex;
            }
        }
    }
}
