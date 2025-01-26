using CleanArchitecture.Domain.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Extensions;

public static class EntityConfigurationExtension
{
	public static EntityTypeBuilder<T> BuildPrimaryKey<T>(this EntityTypeBuilder<T> builder)
		where T : BaseEntity
	{
		builder.HasKey(t => t.Id);
		builder.Property(t => t.Id).ValueGeneratedOnAdd();
		return builder;
	}
}