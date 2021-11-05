using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class ResetPasswordDto
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(32, MinimumLength = 8)]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        public string TempCode { get; set; } = string.Empty;
    }
}
