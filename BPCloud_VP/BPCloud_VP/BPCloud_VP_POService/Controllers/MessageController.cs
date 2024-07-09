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
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository _MessageRepository;

        public MessageController(IMessageRepository MessageRepository)
        {
            _MessageRepository = MessageRepository;
        }

        #region CEOMessage

        [HttpGet]
        public BPCCEOMessage GetCEOMessage()
        {
            try
            {
                var CEOMessage = _MessageRepository.GetCEOMessage();
                return CEOMessage;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetCEOMessage", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCEOMessage(BPCCEOMessage BPCCEOMessage)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _MessageRepository.CreateCEOMessage(BPCCEOMessage);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Message/CreateCEOMessage", ex);
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdateCEOMessage(BPCCEOMessage BPCCEOMessage)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _MessageRepository.UpdateCEOMessage(BPCCEOMessage);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Message/UpdateCEOMessage", ex);
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> DeleteCEOMessage(BPCCEOMessage BPCCEOMessage)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _MessageRepository.DeleteCEOMessage(BPCCEOMessage);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Message/DeleteCEOMessage", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region SCOCMessage

        [HttpGet]
        public BPCSCOCMessage GetSCOCMessage()
        {
            try
            {
                var SCOCMessage = _MessageRepository.GetSCOCMessage();
                return SCOCMessage;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/SCOCMessage", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSCOCMessage(BPCSCOCMessage BPCSCOCMessage)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _MessageRepository.CreateSCOCMessage(BPCSCOCMessage);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Message/CreateSCOCMessage", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSCOCMessage(BPCSCOCMessage BPCSCOCMessage)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _MessageRepository.UpdateSCOCMessage(BPCSCOCMessage);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Message/UpdateSCOCMessage", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSCOCMessage(BPCSCOCMessage BPCSCOCMessage)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _MessageRepository.DeleteSCOCMessage(BPCSCOCMessage);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Message/DeleteSCOCMessage", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion


        #region WelcomeMessage

        [HttpGet]
        public BPCWelcomeMessage GetWelcomeMessage()
        {
            try
            {
                var WelcomeMessage = _MessageRepository.GetWelcomeMessage();
                return WelcomeMessage;
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("ASN/GetWelcomeMessage", ex);
                return null;
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateWelcomeMessage(BPCWelcomeMessage BPCWelcomeMessage)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _MessageRepository.CreateWelcomeMessage(BPCWelcomeMessage);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Message/CreateWelcomeMessage", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateWelcomeMessage(BPCWelcomeMessage BPCWelcomeMessage)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _MessageRepository.UpdateWelcomeMessage(BPCWelcomeMessage);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Message/UpdateWelcomeMessage", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteWelcomeMessage(BPCWelcomeMessage BPCWelcomeMessage)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _MessageRepository.DeleteWelcomeMessage(BPCWelcomeMessage);
                return new OkResult();
            }
            catch (Exception ex)
            {
                WriteLog.WriteToFile("Message/DeleteWelcomeMessage", ex);
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}