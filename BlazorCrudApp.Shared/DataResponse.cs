namespace BlazorCrudApp.Shared;

public class DataResponse<T>
{
	public IList<T> Data { get; set; } = new List<T>();
	public int Total { get; set; }

	public static DataResponse<T> DataSource(IList<T> data, int total = 0)
		=> new DataResponse<T> { Data = data, Total = total };
}
