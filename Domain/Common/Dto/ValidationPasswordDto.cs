namespace Domain.Common.Dto
{
    public class ValidationPasswordDto
    {
        public ValidationPasswordDto(List<string> errors, bool isValid)
        {
            Errors = errors;
            IsValid = isValid;
        }

        public List<string> Errors { get; private set; }
        public bool IsValid { get; private set; }
    }
}
