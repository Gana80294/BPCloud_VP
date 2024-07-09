using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BPCloud_VP.ReportService.Models;
using BPCloud_VP.ReportService.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BPCloud_VP.ReportService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IOverviewRepository _overviewRepository;
        private readonly IPPMRepository _ppmRepository;
        private readonly IVendorRatingRepository _vendorRatingRepository;
        private readonly IDOLRepository _DOLRepository;
        private readonly IFGCPSRepository _FGCPSRepository;
        private readonly IGRRRepository _GRRRepository;
        private readonly IIPRepository _IPRepository;

        public ReportController(IOverviewRepository overviewRepository, IPPMRepository ppmRepository, IVendorRatingRepository vendorRatingRepository,
            IDOLRepository DOLRepository, IFGCPSRepository FGCPSRepository, IGRRRepository GRRRepository, IIPRepository IPRepository)
        {
            _overviewRepository = overviewRepository;
            _ppmRepository = ppmRepository;
            _vendorRatingRepository = vendorRatingRepository;
            _DOLRepository = DOLRepository;
            _FGCPSRepository = FGCPSRepository;
            _GRRRepository = GRRRepository;
            _IPRepository = IPRepository;
        }

        #region Overview Reports

        [HttpGet]
        public List<BPCReportOV> GetOverviewReports(string PartnerID)
        {
            try
            {
                var data = _overviewRepository.GetOverviewReports(PartnerID);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Report/GetOverviewReports", ex);
                return null;
            }
        }

        [HttpPost]
        public List<BPCReportOV> GetOverviewReportByDate(OverviewReportOption overviewReportOption)
        {
            try
            {
                var data = _overviewRepository.GetOverviewReportByDate(overviewReportOption);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Report/GetOverviewReportByDate", ex);
                return null;
            }
        }

        [HttpPost]
        public List<BPCReportOV> GetOverviewReportByOption(OverviewReportOption overviewReportOption)
        {
            try
            {
                var data = _overviewRepository.GetOverviewReportByOption(overviewReportOption);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Report/GetOverviewReportByOption", ex);
                return null;
            }
        }

        [HttpPost]
        public List<BPCReportOV> GetOverviewReportByStatus(OverviewReportOption overviewReportOption)
        {
            try
            {
                var data = _overviewRepository.GetOverviewReportByStatus(overviewReportOption);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Report/GetOverviewReportByStatus", ex);
                return null;
            }
        }

        #endregion

        #region PPM Reports

        [HttpGet]
        public List<BPCReportPPMHeader> GetPPMReports(string PartnerID)
        {
            try
            {
                var data = _ppmRepository.GetPPMReports(PartnerID);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Report/GetPPMReports", ex);
                return null;
            }
        }

        [HttpPost]
        public List<BPCReportPPMHeader> GetPPMReportByDate(PPMReportOption ppmReportOption)
        {
            try
            {
                var data = _ppmRepository.GetPPMReportByDate(ppmReportOption);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Report/GetPPMReportByDate", ex);
                return null;
            }
        }

        [HttpPost]
        public List<BPCReportPPMItem> GetPPMItemReportByPeriod(string PartnerID, DateTime? period = null)
        {
            try
            {
                var data = _ppmRepository.GetPPMItemReportByPeriod(PartnerID, period);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Report/GetPPMItemReportByPeriod", ex);
                return null;
            }
        }

        [HttpPost]
        public List<BPCReportPPMHeader> GetPPMReportByStatus(PPMReportOption ppmReportOption)
        {
            try
            {
                var data = _ppmRepository.GetPPMReportByStatus(ppmReportOption);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Report/GetPPMReportByStatus", ex);
                return null;
            }
        }

        #endregion

        #region VendorRating Reports

        [HttpGet]
        public List<BPCReportVR> GetVendorRatingReports(string PartnerID)
        {
            try
            {
                var data = _vendorRatingRepository.GetVendorRatingReports(PartnerID);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Report/GetVendorRatingReports", ex);
                return null;
            }
        }

        [HttpPost]
        public List<BPCReportVR> GetVendorRatingReportByDate(VendorRatingReportOption vendorRatingReportOption)
        {
            try
            {
                var data = _vendorRatingRepository.GetVendorRatingReportByDate(vendorRatingReportOption);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Report/GetVendorRatingReportByDate", ex);
                return null;
            }
        }

        [HttpPost]
        public List<BPCReportVR> GetVendorRatingReportByOption(VendorRatingReportOption vendorRatingReportOption)
        {
            try
            {
                var data = _vendorRatingRepository.GetVendorRatingReportByOption(vendorRatingReportOption);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Report/GetVendorRatingReportByOption", ex);
                return null;
            }
        }

        [HttpPost]
        public List<BPCReportVR> GetVendorRatingReportByStatus(VendorRatingReportOption vendorRatingReportOption)
        {
            try
            {
                var data = _vendorRatingRepository.GetVendorRatingReportByStatus(vendorRatingReportOption);
                return data;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Report/GetVendorRatingReportByStatus", ex);
                return null;
            }
        }

        #endregion

        [HttpGet]
        public List<BPCReportDOL> GetAllReportDOLByPartnerID(string PartnerID)
        {
            try
            {
                var AllReport = _DOLRepository.GetAllReportDOLByPartnerID(PartnerID);
                return AllReport;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Report/GetAllReportDOLByPartnerID", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCReportFGCPS> GetAllReportFGCPSByPartnerID(string PartnerID)
        {
            try
            {
                var AllReportFGCPS = _FGCPSRepository.GetAllReportFGCPSByPartnerID(PartnerID);
                return AllReportFGCPS;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Report/GetAllReportFGCPSByPartnerID", ex);
                return null;
            }
        }
        [HttpGet]
        public List<BPCReportFGCPS> GetFilteredReportFGCPSByPartnerID(string PartnerID, string Material = null, string MaterialText = null)
        {
            try
            {
                var AllReportFGCPS = _FGCPSRepository.GetFilteredReportFGCPSByPartnerID(PartnerID, Material, MaterialText);
                return AllReportFGCPS;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Report/GetAllReportFGCPSByPartnerID", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCReportGRR> GetAllReportGRRByPartnerID(string PartnerID)
        {
            try
            {
                var AllReportGRR = _GRRRepository.GetAllReportGRRByPartnerID(PartnerID);
                return AllReportGRR;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Report/GetAllReportGRRByPartnerID", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCReportGRR> GetFilteredReportGRRByPartnerID(string PartnerID, string Material = null)
        {
            try
            {
                var AllReportGRR = _GRRRepository.GetFilteredReportGRRByPartnerID(PartnerID, Material);
                return AllReportGRR;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Report/GetFilteredReportGRRByPartnerID", ex);
                return null;
            }
        }
        //[HttpGet]
        //public List<BPCReportGRR> FilterGRRListByPartnerID(string PartnerID, string ASNNumber = null, string DocNumber = null, string Material = null, string Status = null, DateTime? ASNFromDate = null, DateTime? ASNToDate = null)
        //{
        //    try
        //    {
        //        var data = _GRRRepository.GRRecieptListByPartnerID(PartnerID, ASNNumber, DocNumber, Material, Status, ASNFromDate, ASNToDate);
        //        return data;
        //    }
        //    {
        //        WriteLog.WriteToFile("Payment/FilterAccountStatementByPartnerID", ex);
        //        return null;
        //    }
        //}

        [HttpGet]
        public List<BPCReportIP> GetAllReportIPByPartnerID(string PartnerID)
        {
            try
            {
                var AllReportIP = _IPRepository.GetAllReportIPByPartnerID(PartnerID);
                return AllReportIP;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Report/GetAllReportIPByPartnerID", ex);
                return null;
            }
        }

        [HttpGet]
        public List<BPCReportIP> GetFilteredReportIPByPartnerID(string PartnerID, string Material = null, string Method = null)
        {
            try
            {
                var AllReportIP = _IPRepository.GetFilteredReportIPByPartnerID(PartnerID, Material, Method);
                return AllReportIP;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Report/GetFilteredReportIPByPartnerID", ex);
                return null;
            }
        }

        //BPC_PAY_H
        [HttpPost]
        public async Task<IActionResult> CreateBPCPAYH([FromBody] List<BPCPayment> PayH)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _IPRepository.CreateBPCPAYH(PayH);
                return Ok("Data are inserted successfully");
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/CreateBPCPAYH", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateBPCPAYH([FromBody] List<BPCPayment> PAYH)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _IPRepository.UpdateBPCPAYH(PAYH);
                return Ok("Data are inserted successfully");
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/UpdateBPCPAYH", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public List<BPCPayment> GetBPCPAYHByPartnerId(string partnerID)
        {
            try
            {
                if (partnerID == null)
                {
                    return null;
                }
                var result = _IPRepository.GetBPCPAYHByPartnerId(partnerID);
                return result;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/UpdateBPCPAYH", ex);
                return null;
            }
        }
        //BPC_SC_STK
        [HttpPost]
        public async Task<IActionResult> CreateBPCSCSTK([FromBody] List<BPCSCSTK> PayH)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _IPRepository.CreateBPCSCSTK(PayH);
                return Ok("Data are inserted successfully");
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/CreateBPCSCSTK", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBPCSCSTK([FromBody] List<BPCSCSTK> PAYH)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _IPRepository.UpdateBPCSCSTK(PAYH);
                return Ok("Data are inserted successfully");
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/UpdateBPCSCSTK", ex);
                return BadRequest(ex.Message);
            }
        }
    }
}