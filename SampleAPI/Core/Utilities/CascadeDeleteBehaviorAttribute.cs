using Microsoft.EntityFrameworkCore;

namespace Core.Utilities;

[AttributeUsage(AttributeTargets.Class)]
public class CasCadeDeleteBehaviorAttribute(DeleteBehavior deleteBehavior) : Attribute
{
    public DeleteBehavior DeleteBehavior { get; init; } = deleteBehavior;
}