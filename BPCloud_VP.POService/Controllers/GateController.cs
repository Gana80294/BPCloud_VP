using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BPCloud_VP_POService.Models;
using BPCloud_VP_POService.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BPCloud_VP_POService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GateController : ControllerBase
    {
        private readonly IGateRepository _gateRepo;
        private readonly IDashboardRepositorie _dashboardRepositorie;
        public GateController(IGateRepository gateRepo, IDashboardRepositorie dashboardRepositorie)
        {
            _gateRepo = gateRepo;
            _dashboardRepositorie = dashboardRepositorie;
        }
        [HttpPost]
        public IActionResult CreateAllVechicleTurnAround(BPCGateHoveringVechicles GateHV)
        {
            try
            {
                //List<BPCGateHoveringVechicles> GateHVs = new List<BPCGateHoveringVechicles>();
                //GateHVs.Add(GateHV);
                //var Gate = _gateRepo.CreateAllVechicleTurnAround(GateHVs);
                var result = _gateRepo.CreateHoveringVechicles(GateHV);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/CreateAllVechicleTurnAround", ex);
                return BadRequest(ex);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateGateEntryByAsnList(ASNListView Asn)
        {
            try
            {
               if(!this.ModelState.IsValid)
                {
                    BadRequest();
                }
                var result=await  _gateRepo.CreateGateEntryByAsnList(Asn);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/CreateGateEntryByAsnList", ex);
                return BadRequest(ex);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CancelGateEntryByAsnList(ASNListView Asn)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    BadRequest();
                }
                 await _gateRepo.CancelGateEntryByAsnList(Asn);
                return Ok();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/CreateAllVechicleTurnAround", ex);
                return BadRequest(ex);
            }
        }
        [HttpGet]
        public List<BPCGateHoveringVechicles> GetHoveringVechicleByPartnerId(string PartnerID)
        {
            try
            {
                var result = _gateRepo.GetHoveringVechicleByPartnerId(PartnerID);
                return result;
            }   
            catch (Exception ex)
            {
                WriteLog.WriteToFile("PO/GetOfsByPartnerID", ex);
                return null;
            }
        }
    }
}
