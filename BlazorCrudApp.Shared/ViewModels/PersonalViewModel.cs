namespace BlazorCrudApp.Shared.ViewModels;

public class PersonalViewModel : BaseViewModel<int>
{
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public DateTime? DateOfBirth { get; set; }
}
