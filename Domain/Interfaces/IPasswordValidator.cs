namespace Domain.Interfaces
{
    public interface IPasswordValidator
    {
        bool IsValid(string password);
    }
}
