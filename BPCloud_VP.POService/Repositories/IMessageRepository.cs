using BPCloud_VP_POService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP_POService.Repositories
{
    public interface IMessageRepository
    {
        BPCCEOMessage GetCEOMessage();
        Task<BPCCEOMessage> CreateCEOMessage(BPCCEOMessage CEOMessage);
        Task<BPCCEOMessage> UpdateCEOMessage(BPCCEOMessage CEOMessage);
        Task<BPCCEOMessage> DeleteCEOMessage(BPCCEOMessage CEOMessage);

        BPCSCOCMessage GetSCOCMessage();
        Task<BPCSCOCMessage> CreateSCOCMessage(BPCSCOCMessage SCOCMessage);
        Task<BPCSCOCMessage> UpdateSCOCMessage(BPCSCOCMessage SCOCMessage);
        Task<BPCSCOCMessage> DeleteSCOCMessage(BPCSCOCMessage SCOCMessage);

        BPCWelcomeMessage GetWelcomeMessage();
        Task<BPCWelcomeMessage> CreateWelcomeMessage(BPCWelcomeMessage BPCWelcomeMessage);
        Task<BPCWelcomeMessage> UpdateWelcomeMessage(BPCWelcomeMessage BPCWelcomeMessage);
        Task<BPCWelcomeMessage> DeleteWelcomeMessage(BPCWelcomeMessage BPCWelcomeMessage);
    }
}
