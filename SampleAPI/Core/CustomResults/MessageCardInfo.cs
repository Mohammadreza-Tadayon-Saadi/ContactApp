namespace Core.CustomResults;

public record MessageInfo(MessageCardStatus MessageCardStatus, string Message);

public enum MessageCardStatus
{
    Success = 0,
    Warning = 1,
    Error = 2,
}