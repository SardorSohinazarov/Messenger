using System.ComponentModel.DataAnnotations;

namespace Messenger.Application.Models.DataTransferObjects.Auth.Google
{
    public class GoogleLoginDto
    {
        [Required]
        public string IdToken { get; set; }
    }
}
