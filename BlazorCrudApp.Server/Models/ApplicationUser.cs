using Microsoft.AspNetCore.Identity;

namespace BlazorCrudApp.Server.Models;

public class ApplicationUser : IdentityUser
{
	public int? AccountId { get; set; }
	public DateTime DateCreated { get; set; } = DateTime.UtcNow;
	public DateTime? DateModified { get; set; }
	public DateTime? DateDeleted { get; set; }
}
