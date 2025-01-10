namespace BlazorCrudApp.Shared.ViewModels;

public class BaseViewModel<T>
{
	public T Id { get; set; }
	public DateTime DateCreated { get; set; }
	public DateTime? DateModified { get; set; }
	public DateTime? DateDeleted { get; set; }
}
