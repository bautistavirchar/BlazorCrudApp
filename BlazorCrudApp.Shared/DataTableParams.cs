namespace BlazorCrudApp.Shared;

public class DataTableParams
{
	public int Id { get; set; }
	public string? Search { get; set; }
	public int Start { get; set; }
	public int Length { get; set; } = 10;
	public bool IsDeleted { get; set; }
	public string? LoggedUser { get; set; }
}