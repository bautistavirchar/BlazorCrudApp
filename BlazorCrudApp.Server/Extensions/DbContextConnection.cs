using BlazorCrudApp.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorCrudApp.Server.Extensions;

public abstract class DbContextConnection
{
	public IDbContextFactory<ApplicationDbContext> Connection { get; }
	public DbContextConnection(IDbContextFactory<ApplicationDbContext> dbContextFactory) => Connection = dbContextFactory;

}
