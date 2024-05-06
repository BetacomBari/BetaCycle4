namespace BetaCycle4.Models
{
    public class CredentialDB
    {
        public string EmailAddressEncrypt { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public int CredentialsCnnId { get; set; }
        public string? ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordExpiry { get; set; }
    }
}
