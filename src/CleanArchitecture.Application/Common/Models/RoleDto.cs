using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Common.Models;

public class RoleDto : BaseDto
{
	public string Name { get; set; }
	public string? Description { get; set; }

	private class Mapping : Profile
	{
		public Mapping()
		{
			CreateMap<Role, RoleDto>();
		}
	}
}