using BPCloud_VP.ReportService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP.ReportService.Repositories
{
    public interface IDOLRepository
    {
        List<BPCReportDOL> GetAllReportDOLByPartnerID(string PartnerID);
    }
}
