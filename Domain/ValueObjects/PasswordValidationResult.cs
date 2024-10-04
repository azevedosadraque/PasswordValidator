namespace Domain.ValueObjects
{
    public record PasswordValidatorResult(bool IsValid, IReadOnlyList<string> Errors);
}
