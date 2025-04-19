namespace CleanArchitecture.Domain.Entities;

public class Role : BaseAuditableEntity
{
	public Role()
	{
	}

	public Role(string name, string description)
	{
		Name = name;
		Description = description;
	}

	public string Name { get; private set; }
	public string? Description { get; private set; }
}