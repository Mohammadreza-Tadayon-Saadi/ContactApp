namespace Infrastructure.Entities.Common;

public abstract class Entity<TKey> where TKey : struct
{
	public TKey Id { get; protected set; }
}

public abstract class Entity : Entity<int>;