using BlazorCrudApp.Server.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BlazorCrudApp.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[AuthorizeMiddleware]
public class IControllerBase<TService> : ControllerBase
{
	protected readonly TService _service;
	public IControllerBase(TService service) => _service = service;
}
