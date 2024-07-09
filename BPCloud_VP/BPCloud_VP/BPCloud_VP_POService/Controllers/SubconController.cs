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

namespace BPCloud_VP_POService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SubconController : ControllerBase
    {
        private readonly ISubconRepository _SubconRepository;

        public SubconController(ISubconRepository SubconRepository)
        {
            _SubconRepository = SubconRepository;
        }

        [HttpGet]
        public List<BPCOFSubcon> GetSubconByDocAndPartnerID(string DocNumber, string PartnerID)
        {
            try
            {
                var subcons = _SubconRepository.GetSubconByDocAndPartnerID(DocNumber, PartnerID);
                return subcons;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Subcon/GetSubconByDocAndPartnerID", ex);
                return null;
            }
        }
        public List<BPCOFSubcon> GetSubconBySLAndPartnerID(string DocNumber, string Item, string SlLine, string PartnerID)
        {
            try
            {
                var subcons = _SubconRepository.GetSubconBySLAndPartnerID(DocNumber, Item, SlLine, PartnerID);
                return subcons;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Subcon/GetSubconByDocAndPartnerID", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubcon(SubconItems subconItems)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _SubconRepository.CreateSubcon(subconItems.items);
                return Ok();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Subcon/CreateSubcon", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSubcon(BPCOFSubcon subcon)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _SubconRepository.DeleteSubcon(subcon);
                return Ok();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Subcon/DeleteSubcon", ex);
                return BadRequest(ex.Message);
            }
        }

        public List<BPCOFSubconView> GetSubconViewByDocAndPartnerID(string DocNumber, string PartnerID)
        {
            try
            {
                var subcons = _SubconRepository.GetSubconViewByDocAndPartnerID(DocNumber, PartnerID);
                return subcons;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Subcon/GetSubconViewByDocAndPartnerID", ex);
                return null;
            }
        }
    }
}