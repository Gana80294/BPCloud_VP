using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BPCloud_VP.ReportService.DBContexts;
using BPCloud_VP.ReportService.Models;

namespace BPCloud_VP.ReportService.Repositories
{
    public class FGCPSRepository : IFGCPSRepository
    {
        private readonly ReportContext _dbContext;

        public FGCPSRepository(ReportContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<BPCReportFGCPS> GetAllReportFGCPSByPartnerID(string PartnerID)
        {
            try
            {
                return _dbContext.BPCReportFGCPS.Where(x => x.PatnerID == PartnerID).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FGCPSRepository/GetAllReportFGCPSByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FGCPSRepository/GetAllReportFGCPSByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<BPCReportFGCPS> GetFilteredReportFGCPSByPartnerID(string PartnerID, string Material = null, string MaterialText = null)
        {
            try
            {
                bool IsMaterial = !string.IsNullOrEmpty(Material);
                bool IsMaterialText = !string.IsNullOrEmpty(MaterialText);
                var result = (from tb in _dbContext.BPCReportFGCPS
                              where tb.PatnerID == PartnerID && tb.IsActive
                              && (!IsMaterial || tb.Material == Material) && (!IsMaterialText || tb.MaterialText == MaterialText)
                              select tb).ToList();
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("FGCPSRepository/GetFilteredReportFGCPSByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("FGCPSRepository/GetFilteredReportFGCPSByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
