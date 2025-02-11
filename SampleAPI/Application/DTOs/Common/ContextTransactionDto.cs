namespace Application.DTOs.Common;

public class ContextTransactionDto<TKey>
	where TKey : struct
{
    // Primary constructor with all arguments
    public ContextTransactionDto(TKey id, ContextTransactionStatus status, string? message = null)
    {
        Id = id;
        Status = status;
        Message = message ?? status.ToDisplay(DisplayProperty.Name);
    }

    public ContextTransactionDto(TKey id, ContextTransactionStatus status) : this(id, status, null) { }

    public ContextTransactionDto(ContextTransactionStatus status, string? message = null) : this(default, status, message) { }

    public ContextTransactionDto(ContextTransactionStatus status) : this(default, status, null) { }

    public TKey Id { get; init; }
    public ContextTransactionStatus Status { get; init; }
    public string Message { get; init; }
};

public class ContextTransactionDto : ContextTransactionDto<int>
{
    public ContextTransactionDto(int id, ContextTransactionStatus status, string message) : base(id, status, message) { }
    
    public ContextTransactionDto(int id, ContextTransactionStatus status) : base(id, status) { }

    public ContextTransactionDto(ContextTransactionStatus status, string? message = null) : base(status, message) { }

    public ContextTransactionDto(ContextTransactionStatus status) : base(status) { }
}