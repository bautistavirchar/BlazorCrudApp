using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BlazorCrudApp.Server.Extensions;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeMiddlewareAttribute : Attribute, IAuthorizationFilter
{
	private readonly string[] _roles;
	public AuthorizeMiddlewareAttribute(params string[] roles) => _roles = roles;

	public void OnAuthorization(AuthorizationFilterContext context)
	{
		var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
		if (allowAnonymous) return;

		if (context.HttpContext.User.Identity!.IsAuthenticated) return;

		if (_roles.Length > 0)
		{
			foreach (var role in _roles)
			{
				if (!context.HttpContext.User.IsInRole(role))
				{
					context.Result = new ForbidResult();
				}
			}
		}

		context.Result = new UnauthorizedResult();
	}
}