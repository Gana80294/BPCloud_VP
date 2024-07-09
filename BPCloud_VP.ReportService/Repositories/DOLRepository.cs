using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BPCloud_VP.ReportService.DBContexts;
using BPCloud_VP.ReportService.Models;

namespace BPCloud_VP.ReportService.Repositories
{
    public class DOLRepository : IDOLRepository
    {
        private readonly ReportContext _dbContext;

        public DOLRepository(ReportContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<BPCReportDOL> GetAllReportDOLByPartnerID(string PartnerID)
        {
            try
            {
                return _dbContext.BPCReportDOLs.Where(x => x.PatnerID == PartnerID).ToList();
            }
            catch (SqlException ex) { WriteLog.WriteToFile("DOLRepository/GetAllReportDOLByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (InvalidOperationException ex) { WriteLog.WriteToFile("DOLRepository/GetAllReportDOLByPartnerID", ex); throw new Exception("Something went wrong"); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
