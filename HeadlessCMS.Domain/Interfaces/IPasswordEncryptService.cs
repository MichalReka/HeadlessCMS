namespace HeadlessCMS.Domain.Interfaces
{
    public interface IPasswordEncryptService
    {
        string Encrypt(string password);
    }
}