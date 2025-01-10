using BlazorCrudApp.Shared;
using BlazorCrudApp.Shared.Models;
using BlazorCrudApp.Shared.ViewModels;
using Refit;

namespace BlazorCrudApp.Client.Services;

public interface IPersonalService
{
	[Post("/api/personal")]
	Task<DataResponse<PersonalViewModel>> GetAsync(DataTableParams dataTableParams);

	[Post("/api/personal/add")]
	Task<Shared.ApiResponse<int>> AddAsync(PersonalModel personalModel);

	[Patch("/api/personal/update")]
	Task<ApiResponse> UpdateAsync(PersonalModel personalModel);

	[Post("/api/personal/archive")]
	Task<ApiResponse> ArchiveAsync(ArchiveModel<int> archiveModel);
}
