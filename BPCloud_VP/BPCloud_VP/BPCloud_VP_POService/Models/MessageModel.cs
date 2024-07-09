using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP_POService.Models
{
    [Table("BPC_CEO_Message")]
    public class BPCCEOMessage : CommonClass
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageID { get; set; }
        public string CEOMessage { get; set; }
        public bool IsReleased { get; set; }
    }
    [Table("BPC_SCOC_Message")]
    public class BPCSCOCMessage : CommonClass
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageID { get; set; }
        public string SCOCMessage { get; set; }
        public bool IsReleased { get; set; }
    }
    [Table("BPC_Welcome_Message")]
    public class BPCWelcomeMessage : CommonClass
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageID { get; set; }
        public string WelcomeMessage { get; set; }
        public bool IsReleased { get; set; }
    }
}
