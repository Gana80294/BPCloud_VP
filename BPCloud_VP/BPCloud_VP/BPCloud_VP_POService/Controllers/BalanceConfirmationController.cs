using BPCloud_VP_POService.Models;
using BPCloud_VP_POService.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP_POService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BalanceConfirmationController : ControllerBase
    {
        private readonly IBalanceConfirmationRepository _balanceRepo;
        public BalanceConfirmationController(IBalanceConfirmationRepository balanceRepo)
        {
            _balanceRepo = balanceRepo;
        }
        
        public IActionResult GetAllBCHeaders()
        {
            try
            {
                return Ok(_balanceRepo.GetBalanceConfirmationHeaders());
            }
            catch (Exception ex)
            {

                WriteLog.WriteToFile("BalanceConfirmation/GetAllBCHeaders", ex);
                return BadRequest(ex);
            }
        }
        public IActionResult GetAllBCItems()
        {
            try
            {
                return Ok(_balanceRepo.GetBalanceConfirmationItems());
            }
            catch (Exception ex)
            {

                WriteLog.WriteToFile("BalanceConfirmation/GetAllBCItems", ex);
                return BadRequest(ex);
            }
        }
        public IActionResult GetCurrentBCHeader()
        {
            try
            {
                return Ok(_balanceRepo.GetCurrentHeader());
            }
            catch (Exception ex)
            {

                WriteLog.WriteToFile("BalanceConfirmation/GetCurrentBCHeader", ex);
                return BadRequest(ex);
            }
        }
        public IActionResult GetCurrentBCItems()
        {
            try
            {
                return Ok(_balanceRepo.GetCurrentItems());
            }
            catch (Exception ex)
            {

                WriteLog.WriteToFile("BalanceConfirmation/GetCurrentBCItems", ex);
                return BadRequest(ex);
            }
        }
        public IActionResult GetCurrentBCItemsByPeroid()
        {
            try
            {
                return Ok(_balanceRepo.GetCurrentBCItemsByPeroid());
            }
            catch (Exception ex)
            {

                WriteLog.WriteToFile("BalanceConfirmation/GetCurrentBCItems", ex);
                return BadRequest(ex);
            }
        }
        [HttpPost]
        public async Task<IActionResult> AcceptBC(ConfirmationDeatils confirmationDeatils)
        {
            try
            {
                await _balanceRepo.AcceptBC(confirmationDeatils);
                return Ok();
            }
            catch (Exception ex)
            {

                WriteLog.WriteToFile("BalanceConfirmation/GetCurrentBCItems", ex);
                return BadRequest(ex);
            }
        }

        #region BC

        [HttpPost]
        public async Task<IActionResult> CreateBalConfirmDetails([FromBody] List<BalanceConfirmationHeader> BalanceConfirmationHeaders)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _balanceRepo.CreateBalConfirmDetails(BalanceConfirmationHeaders);
                if (result != null)
                {
                    return Ok("Data are inserted successfully");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("BalanceConfirmation/CreateBalConfirmDetails", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBalConfirmItemDetails([FromBody] List<BalanceConfirmationItem> BalanceConfirmationItems)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _balanceRepo.CreateBalConfirmItemDetails(BalanceConfirmationItems);
                if (result != null)
                {
                    return Ok("Data are inserted successfully");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("BalanceConfirmation/CreateBalConfirmItemDetails", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBalanceConfirmationHeader(BalanceConfirmationHeader header)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _balanceRepo.UpdateBalanceConfirmationHeader(header);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("BalanceConfirmation/UpdateBalanceConfirmationHeader", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBalanceConfirmationItem(BalanceConfirmationItem item)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _balanceRepo.UpdateBalanceConfirmationItem(item);
                return Ok(result);
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("BalanceConfirmation/UpdateBalanceConfirmationItem", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

    }
}
