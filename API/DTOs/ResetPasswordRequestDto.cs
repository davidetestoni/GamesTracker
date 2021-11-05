using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class ResetPasswordRequestDto
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
    }
}
