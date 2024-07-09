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
    public class VendorRatingRepository : IVendorRatingRepository
    {
        private readonly ReportContext _dbContext;
        public VendorRatingRepository(ReportContext context, IConfiguration configuration)
        {
            _dbContext = context;
        }
        public List<BPCReportVR> GetVendorRatingReports(string PartnerID)
        {
            try
            {
                List<BPCReportVR> vendorRatingReports = new List<BPCReportVR>();
                if (!string.IsNullOrEmpty(PartnerID))
                {
                    vendorRatingReports = _dbContext.BPCReportVRs.Where(x => x.PatnerID.ToLower() == PartnerID.ToLower() && x.IsActive).ToList();
                }
                return vendorRatingReports;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("VendorRatingRepository/GetVendorRatingReports", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("VendorRatingRepository/GetVendorRatingReports", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("VendorRatingRepository/GetVendorRatingReports : - ", ex);
                throw ex;
            }
        }
        public List<BPCReportVR> GetVendorRatingReportByDate(VendorRatingReportOption vendorRatingOption)
        {
            try
            {
                List<BPCReportVR> vendorRatingReports = new List<BPCReportVR>();
                if (vendorRatingOption.FromDate.HasValue && vendorRatingOption.FromDate != null && vendorRatingOption.ToDate.HasValue && vendorRatingOption.ToDate != null)
                {
                    vendorRatingReports = (from tb in _dbContext.BPCReportVRs
                                           where tb.IsActive && tb.PatnerID.ToLower() == vendorRatingOption.PartnerID.ToLower()
                                           && (tb.CreatedOn.Date >= vendorRatingOption.FromDate.Value.Date)
                                           && (tb.CreatedOn.Date <= vendorRatingOption.ToDate.Value.Date)
                                           select tb).ToList();
                }
                else if (vendorRatingOption.FromDate.HasValue && vendorRatingOption.FromDate != null && !vendorRatingOption.ToDate.HasValue && vendorRatingOption.ToDate == null)
                {
                    vendorRatingReports = (from tb in _dbContext.BPCReportVRs
                                           where tb.IsActive && tb.PatnerID.ToLower() == vendorRatingOption.PartnerID.ToLower()
                                           && (tb.CreatedOn.Date >= vendorRatingOption.FromDate.Value.Date)
                                           //&& (tb.CreatedOn.Date <= vendorRatingOption.ToDate.Value.Date)
                                           select tb).ToList();
                }
                else if (!vendorRatingOption.FromDate.HasValue && vendorRatingOption.FromDate == null && vendorRatingOption.ToDate.HasValue && vendorRatingOption.ToDate != null)
                {
                    vendorRatingReports = (from tb in _dbContext.BPCReportVRs
                                           where tb.IsActive && tb.PatnerID.ToLower() == vendorRatingOption.PartnerID.ToLower()
                                           //&& (tb.CreatedOn.Date >= vendorRatingOption.FromDate.Value.Date)
                                           && (tb.CreatedOn.Date <= vendorRatingOption.ToDate.Value.Date)
                                           select tb).ToList();
                }
                return vendorRatingReports;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("VendorRatingRepository/GetVendorRatingReports", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("VendorRatingRepository/GetVendorRatingReports", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("VendorRatingRepository/GetVendorRatingReportByDate : - ", ex);
                throw ex;
            }
        }
        public List<BPCReportVR> GetVendorRatingReportByOption(VendorRatingReportOption vendorRatingOption)
        {
            try
            {
                List<BPCReportVR> vendorRatingReports = new List<BPCReportVR>();
                if (!string.IsNullOrEmpty(vendorRatingOption.Material) && string.IsNullOrEmpty(vendorRatingOption.PO))
                {
                    vendorRatingReports = (from tb in _dbContext.BPCReportVRs
                                           where tb.IsActive && tb.PatnerID.ToLower() == vendorRatingOption.PartnerID.ToLower()
                                           && tb.Material.ToLower() == vendorRatingOption.Material.ToLower()
                                           select tb).ToList();
                }
                else if (string.IsNullOrEmpty(vendorRatingOption.Material) && !string.IsNullOrEmpty(vendorRatingOption.PO))
                {
                    vendorRatingReports = (from tb in _dbContext.BPCReportVRs
                                           where tb.IsActive && tb.PatnerID.ToLower() == vendorRatingOption.PartnerID.ToLower()
                                           && tb.Material.ToLower() == vendorRatingOption.PO.ToLower()
                                           select tb).ToList();
                }
                else if (!string.IsNullOrEmpty(vendorRatingOption.Material) && !string.IsNullOrEmpty(vendorRatingOption.PO))
                {
                    vendorRatingReports = (from tb in _dbContext.BPCReportVRs
                                           where tb.IsActive && tb.PatnerID.ToLower() == vendorRatingOption.PartnerID.ToLower()
                                           && tb.Material.ToLower() == vendorRatingOption.Material.ToLower()
                                           && tb.Material.ToLower() == vendorRatingOption.PO.ToLower()
                                           select tb).ToList();
                }
                else
                {
                    vendorRatingReports = null;
                }
                return vendorRatingReports;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("VendorRatingRepository/GetVendorRatingReportByOption", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("VendorRatingRepository/GetVendorRatingReportByOption", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("VendorRatingRepository/GetVendorRatingReportByOption : - ", ex);
                throw ex;
            }
        }
        public List<BPCReportVR> GetVendorRatingReportByStatus(VendorRatingReportOption vendorRatingOption)
        {
            try
            {
                List<BPCReportVR> vendorRatingReports = new List<BPCReportVR>();
                if (!string.IsNullOrEmpty(vendorRatingOption.Status))
                {
                    vendorRatingReports = (from tb in _dbContext.BPCReportVRs
                                           where tb.IsActive && tb.PatnerID.ToLower() == vendorRatingOption.PartnerID.ToLower()
                                          && tb.Status.ToLower() == vendorRatingOption.Status.ToLower()
                                           select tb).ToList();
                }
                return vendorRatingReports;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("VendorRatingRepository/GetVendorRatingReportByStatus", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("VendorRatingRepository/GetVendorRatingReportByStatus", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("VendorRatingRepository/GetVendorRatingReportByOption : - ", ex);
                throw ex;
            }
        }
    }
}
