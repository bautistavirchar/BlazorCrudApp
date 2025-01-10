using BlazorCrudApp.Server.Services;
using BlazorCrudApp.Shared;
using BlazorCrudApp.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlazorCrudApp.Server.Controllers;

public class PersonalController : IControllerBase<IPersonalService>
{
	public PersonalController(IPersonalService service) : base(service)
	{
	}

	[HttpPost]
	public async Task<IActionResult> Get([FromBody] DataTableParams dataTableParams) =>
		Ok(await _service.GetAsync(dataTableParams));

	[HttpPost(nameof(Add))]
	public async Task<IActionResult> Add([FromBody] PersonalModel personalModel) =>
		Ok(await _service.AddAsync(personalModel));

	[HttpPatch(nameof(Update))]
	public async Task<IActionResult> Update([FromBody] PersonalModel personalModel) =>
		Ok(await _service.UpdateAsync(personalModel));

	[HttpGet("{id}")]
	public async Task<IActionResult> Get(int id) =>
		Ok(await _service.GetByIdAsync(id));

	[HttpPost(nameof(Archive))]
	public async Task<IActionResult> Archive([FromBody] ArchiveModel<int> archiveModel) =>
		Ok(await _service.ArchiveAsync(archiveModel));
}
