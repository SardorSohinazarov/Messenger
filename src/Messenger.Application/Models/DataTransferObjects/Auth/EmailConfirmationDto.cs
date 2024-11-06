namespace Messenger.Application.Models.DataTransferObjects.Auth
{
    public class EmailConfirmationDto
    {
        public string Email { get; set; }
        public string ConfirmationCode { get; set; }
    }
}
