namespace Core.CustomResults;

public class PasswordValidationInfo(string pattern, string errorMessage)
{
    public string Pattern { get; private set; } = pattern;
    public string ErrorMessage { get; private set; } = errorMessage;
}