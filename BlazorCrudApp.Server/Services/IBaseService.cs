using BlazorCrudApp.Shared;

namespace BlazorCrudApp.Server.Services;
public interface IBaseService<TId, TModel>
{
	Task<TModel> GetByIdAsync(TId id);
	Task<ApiResponse> AddAsync(TModel model);
	Task<ApiResponse> UpdateAsync(TModel model);
	Task<ApiResponse> ArchiveAsync(ArchiveModel<TId> archiveModel);
	Task<DataResponse<dynamic>> GetAsync(DataTableParams param);

}