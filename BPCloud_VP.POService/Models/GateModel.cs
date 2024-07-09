using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud_VP_POService.Models
{
    [Table("BPC_Gate_TA")]
    public class BPCGateVechicleTurnAroundTime: CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? EntryTime { get; set; }

        public string Truck { get; set; }
        [MaxLength(450)]
        public string Partner { get; set; }
        [MaxLength(450)]
        public string DocNo { get; set; }

        public string Transporter { get; set; }
        public string Gate { get; set; }
        public string ExitDt { get; set; }
        public string ExitTime { get; set; }
        public string TATime { get; set; }

        public string Exception { get; set; }
    }
    [Table("BPC_Gate_HV")]
    public class BPCGateHoveringVechicles : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }

        public DateTime? Date { get; set; }

        public DateTime? Time { get; set; }

        //public DateTime? CancelDuration { get; set; }
        public string Truck { get; set; }
        [MaxLength(450)]
        public string Partner { get; set; }
        [MaxLength(450)]
        public string DocNo { get; set; }

        public string Transporter { get; set; }
        public string Gate { get; set; }
        public string Plant { get; set; }
    }
    [Table("BPC_GateEntry")]
    public class BPCGateEntry : CommonClass
    {
        [MaxLength(3)]
        public string Client { get; set; }
        [MaxLength(4)]
        public string Company { get; set; }
        [MaxLength(1)]
        public string Type { get; set; }
        [MaxLength(12)]
        public string PatnerID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GateEntryNo { get; set; }
        public string ASNNumber { get; set; }
        public string DocNumber { get; set; }
        //public string Material { get; set; }
        public string VessleNumber { get; set; }
        public DateTime? GateEntryTime { get; set; }
        //public double ASNQty { get; set; }
        public DateTime? DepartureDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public string Plant { get; set; }
        public string Transporter { get; set; }
        public string Status { get; set; }
    }
}
