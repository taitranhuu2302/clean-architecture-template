using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
	public void Configure(EntityTypeBuilder<Role> builder)
	{
		builder.BuildPrimaryKey();

		builder.Property(r => r.Name)
			.HasMaxLength(50)
			.IsRequired();

		builder.Property(r => r.Description)
			.HasMaxLength(1000);

		builder.ToTable("roles");
	}
}