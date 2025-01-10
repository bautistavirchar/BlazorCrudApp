namespace BlazorCrudApp.Shared;

public class ArchiveModel<TId>
{
	public TId Id { get; set; }
	public bool Archive { get; set; }
}