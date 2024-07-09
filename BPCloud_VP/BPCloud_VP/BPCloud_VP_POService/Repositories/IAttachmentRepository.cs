using BPCloud_VP_POService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP_POService.Repositories
{
    public interface IAttachmentRepository
    {
        Task<BPCFLIPAttachment> AddAttachment(BPCFLIPAttachment BPCFLIPAttachment);
        Task<BPCFLIPAttachment> UpdateAttachment(BPCFLIPAttachment BPCFLIPAttachment);
        //List<BPCFLIPAttachment> FilterAttachments(string ProjectName, int AppID = 0, string AppNumber = null);
        //BPCFLIPAttachment FilterAttachment(string ProjectName, string AttchmentName, int AppID = 0, string AppNumber = null, string HeaderNumber = null);
        BPCFLIPAttachment GetAttachmentByName(string AttachmentName);
        BPCFLIPAttachment GetFlipAttachmentByID(string FlipID, string AttachmentName);
        BPCFLIPAttachment GetAttachmentByID(int AttachmentID);
    }
}
