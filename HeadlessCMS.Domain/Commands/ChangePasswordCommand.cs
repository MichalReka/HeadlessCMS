namespace HeadlessCMS.Domain.Commands
{
    public class ChangePasswordCommand
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public Guid UserId { get; set; }
    }
}
