namespace HeadlessCMS.Domain.Interfaces
{
    public interface IPasswordEncryptService
    {
        (string, byte[]) HashPassword(string password);
        bool VerifyHashedPassword(string hashedPassword, byte[] salt, string providedPassword);
    }
}