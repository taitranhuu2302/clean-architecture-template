using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Common.Models;

public class UserDto
{
	public string Id { get; set; }
	public string Email { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string FullName { get; set; }
	public RoleDto Role { get; set; }

	class Mapping : Profile
	{
		public Mapping()
		{
			CreateMap<User, UserDto>();
		}
	}
}