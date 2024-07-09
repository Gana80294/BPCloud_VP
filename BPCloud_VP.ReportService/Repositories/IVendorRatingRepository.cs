using BPCloud_VP.ReportService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP.ReportService.Repositories
{
    public interface IVendorRatingRepository
    {
        List<BPCReportVR> GetVendorRatingReports(string PartnerID);
        List<BPCReportVR> GetVendorRatingReportByDate(VendorRatingReportOption vendorRatingReportOption);
        List<BPCReportVR> GetVendorRatingReportByOption(VendorRatingReportOption vendorRatingReportOption);
        List<BPCReportVR> GetVendorRatingReportByStatus(VendorRatingReportOption vendorRatingReportOption);
    }
}
