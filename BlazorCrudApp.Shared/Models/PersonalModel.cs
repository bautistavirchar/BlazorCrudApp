namespace BlazorCrudApp.Shared.Models;

public class PersonalModel : BaseModel<int>
{
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public DateTime? DateOfBirth { get; set; }
}
