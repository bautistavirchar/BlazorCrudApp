using BlazorCrudApp.Client.Components.Dialogs;
using BlazorCrudApp.Client.Services;
using BlazorCrudApp.Shared;
using BlazorCrudApp.Shared.Models;
using BlazorCrudApp.Shared.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorCrudApp.Client.Pages;

public partial class PersonalPage
{
	[Inject] IPersonalService PersonalService { get; set; } = default!;
	[Inject] IDialogService DialogService { get; set; } = default!;
	[Inject] IToastService ToastService { get; set; } = default!;

	GridItemsProvider<PersonalViewModel> gridItemsProvider = default!;
	PaginationState pagination = new PaginationState { ItemsPerPage = 10 };
	FluentDataGrid<PersonalViewModel> _grid = default!;
	string? SearchKey { get; set; }
	bool Active { get; set; } = true;
	protected override void OnInitialized()
	{
		gridItemsProvider = async req =>
		{
			var param = new DataTableParams
			{
				IsDeleted = !Active,
				Start = req.StartIndex,
				Length = pagination.ItemsPerPage
			};
			if (SearchKey.IsNotEmpty())
				param.Search = SearchKey;

			var response = await PersonalService.GetAsync(param);
			return GridItemsProviderResult.From(
				items: response.Data,
				totalItemCount: response.Total
			);
		};
	}

	async Task ShowDialogAsync(PersonalViewModel? personalViewModel = null)
	{
		var content = new PersonalModel();
		if (personalViewModel is not null)
		{
			content = new PersonalModel
			{
				Id = personalViewModel.Id,
				FirstName = personalViewModel.FirstName,
				LastName = personalViewModel.LastName,
				DateOfBirth = personalViewModel.DateOfBirth
			};
		}
		var title = content.Id < 1 ? "New" : "Edit";
		var dialog = await DialogService.ShowDialogAsync<PersonDialog>(content, new DialogParameters
		{
			Height = "400px",
			Title = $"{title} Person",
			PreventDismissOnOverlayClick = true,
			PreventScroll = true,
		});
		var result = await dialog.Result;
		if (!result.Cancelled)
		{
			await _grid.RefreshDataAsync();
		}
	}

	async Task ArchiveAsync(PersonalViewModel personalViewModel)
	{
		var title = Active ? "Archive" : "Restore";
		var dialog = await DialogService.ShowConfirmationAsync(
			message: Active ? "Archive?" : "Restore?",
			primaryText: Active ? "Archive" : "Restore",
			secondaryText: "Cancel",
			title: $"{title} {personalViewModel.FirstName}?");

		var result = await dialog.Result;
		if (!result.Cancelled)
		{
			// call api to archive
			var response = await PersonalService.ArchiveAsync(new ArchiveModel<int>
			{
				Id = personalViewModel.Id,
				Archive = !Active
			});
			if (response.Success)
			{
				ToastService.ShowSuccess("Successfully archived.");
				await _grid.RefreshDataAsync();
				return;
			}

			ToastService.ShowError(response.ErrorMessage);
		}
	}
}
