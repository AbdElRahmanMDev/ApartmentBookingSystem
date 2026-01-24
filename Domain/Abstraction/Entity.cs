namespace Domain.Abstraction;

public abstract class Entity
{
    private readonly List<IDomainEvents> _domainEvents = new List<IDomainEvents>();

    protected Entity(Guid id)
    {
        Id = id;
    }
    protected Entity() { }
    public Guid Id { get; init; }


    public IReadOnlyList<IDomainEvents> GetDomainEvents()
    {
        return _domainEvents.AsReadOnly();
    }
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
    protected void RaiseDomainEvent(IDomainEvents domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

}
