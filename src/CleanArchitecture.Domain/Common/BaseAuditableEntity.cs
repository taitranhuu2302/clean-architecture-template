namespace CleanArchitecture.Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
	public DateTimeOffset Created { get; set; }

	public string? CreatedBy { get; set; }

	public DateTimeOffset LastModified { get; set; }

	public string? LastModifiedBy { get; set; }

	public void UpdateLastModified(string? userId)
	{
		LastModifiedBy = userId;
		LastModified = DateTime.UtcNow;
	}

	public void UpdateCreated(string? userId)
	{
		CreatedBy = userId;
		Created = DateTime.UtcNow;
	}
}