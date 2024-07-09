using System.ComponentModel.DataAnnotations;

namespace BPCloud_VP.AuthenticationService.Models
{
    public class Client
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Secret { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public ApplicationTypes ApplicationType { get; set; }
        [Required]
        public bool Active { get; set; }
        [Required]
        public int RefreshTokenLifeTime { get; set; }
        [MaxLength(100)]
        public string AllowedOrigin { get; set; }
    }
    public enum ApplicationTypes
    {
        JavaScript = 0,
        NativeConfidential = 1
    };
}
