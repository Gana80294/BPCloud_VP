using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BPCloud_VP.ReportService.DBContexts;
using BPCloud_VP.ReportService.Models;

namespace BPCloud_VP.ReportService.Repositories
{
    public class GRRRepository : IGRRRepository
    {
        private readonly ReportContext _dbContext;

        public GRRRepository(ReportContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<BPCReportGRR> GetAllReportGRRByPartnerID(string PartnerID)
        {
            try
            {
                return _dbContext.BPCReportGRRs.Where(x => x.PatnerID == PartnerID).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("GRRRepository/GetAllReportGRRByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("GRRRepository/GetAllReportGRRByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCReportGRR> GetFilteredReportGRRByPartnerID(string PartnerID, string Material = null)
        {
            try
            {
                bool IsMaterial = !string.IsNullOrEmpty(Material);
                var result = (from tb in _dbContext.BPCReportGRRs
                              where tb.PatnerID == PartnerID && tb.IsActive && (!IsMaterial || tb.Material == Material)
                              select tb).ToList();
                return result;
            }
            catch (SqlException ex) { WriteLog.WriteToFile("GRRRepository/GetFilteredReportGRRByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("GRRRepository/GetFilteredReportGRRByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
