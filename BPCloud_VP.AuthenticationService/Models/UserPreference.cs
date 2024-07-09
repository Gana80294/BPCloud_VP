using System.ComponentModel.DataAnnotations;

namespace BPCloud_VP.AuthenticationService.Models
{
    public class UserPreference
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public Guid UserID { get; set; }
        public string NavbarPrimaryBackground { get; set; }
        public string NavbarSecondaryBackground { get; set; }
        public string ToolbarBackground { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        [Required]
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
