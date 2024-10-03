using Domain.Common.Dto;

namespace Domain.Interfaces
{
    public interface IPasswordValidator
    {
        ValidationPasswordDto IsValid(string password);
    }
}
