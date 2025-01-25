using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.HasKey(e => e.Id);

		builder.Property(e => e.Email)
			.HasMaxLength(256)
			.IsRequired();

		builder.Property(e => e.Password)
			.IsRequired();

		builder.Property(e => e.Salt)
			.IsRequired();

		builder.Property(e => e.FirstName)
			.HasMaxLength(256);

		builder.Property(e => e.LastName)
			.HasMaxLength(256);

		builder.ToTable("users");
	}
}