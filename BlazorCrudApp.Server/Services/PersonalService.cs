using BlazorCrudApp.Server.Data;
using BlazorCrudApp.Server.Extensions;
using BlazorCrudApp.Shared;
using BlazorCrudApp.Shared.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace BlazorCrudApp.Server.Services;

public interface IPersonalService : IBaseService<int,PersonalModel>
{
	// more custom ...
}

public class PersonalService : DbContextConnection, IPersonalService
{
	public PersonalService(IDbContextFactory<ApplicationDbContext> dbContextFactory) : base(dbContextFactory)
	{
	}

	public async Task<ApiResponse> AddAsync(PersonalModel model)
	{
		using var context = await Connection.CreateDbContextAsync();
		if (await context.Personals.AnyAsync(s => s.FirstName == model.FirstName && s.LastName == model.LastName && s.DateOfBirth == model.DateOfBirth))
			return ApiResponse.ErrorResponse("Duplicate item.");

		var newPersonal = new Personal
		{
			FirstName = model.FirstName,
			LastName = model.LastName,
			DateOfBirth = model.DateOfBirth.GetValueOrDefault()
		};

		try
		{
			await context.Personals.AddAsync(newPersonal);
			await context.SaveChangesAsync();
			return ApiResponse.SuccessResponse(newPersonal.Id);
		}
		catch (Exception ex)
		{
			return ApiResponse.ErrorResponse($"Failed to add {ex.Message}");
		}
	}

	public async Task<ApiResponse> ArchiveAsync(ArchiveModel<int> archiveModel)
	{
		using var context = await Connection.CreateDbContextAsync();
		var row = await context.Personals.FindAsync(archiveModel.Id);
		if (row is null)
			return ApiResponse.ErrorResponse("Not found.");

		try
		{
			row.DateDeleted = !archiveModel.Archive ? DateTime.UtcNow : null;

			context.Personals.Update(row);
			await context.SaveChangesAsync();

			return ApiResponse.SuccessResponse();
		}
		catch (Exception ex)
		{
			return ApiResponse.ErrorResponse($"Failed to archive {ex.Message}");
		}
	}

	public async Task<DataResponse<dynamic>> GetAsync(DataTableParams param)
	{
		using var context = await Connection.CreateDbContextAsync();
		var predicate = PredicateBuilder.New<Data.Personal>(true);
		if (param.Search.IsNotEmpty())
			predicate = predicate.And(p => p.FirstName.Contains(param.Search!) || p.LastName.Contains(param.Search));

		if (param.IsDeleted)
			predicate = predicate.And(p => p.DateDeleted.HasValue);

		if (!param.IsDeleted)
			predicate = predicate.And(p => !p.DateDeleted.HasValue);

		var rows = await context.Personals.AsNoTracking()
			.Where(predicate)
			.Select(c => new
			{
				c.Id,
				c.FirstName,
				c.LastName,
				c.DateOfBirth,
				c.DateCreated,
				c.DateModified,
				c.DateDeleted
			})
			.OrderByDescending(o => o.Id)
			.Skip(param.Start)
			.Take(param.Length)
			.ToListAsync<dynamic>();

		var totalCount = await context.Personals.CountAsync(predicate);

		return DataResponse<dynamic>.DataSource(
			data: rows,
			total: totalCount
		);
	}

	public async Task<PersonalModel> GetByIdAsync(int id)
	{
		using var context = await Connection.CreateDbContextAsync();
		var row = await context.Personals.FindAsync(id);
		if (row is null)
			return null;

		return new PersonalModel
		{
			FirstName = row.FirstName,
			LastName = row.LastName,
			DateOfBirth = row.DateOfBirth,
		};
	}

	public async Task<ApiResponse> UpdateAsync(PersonalModel model)
	{
		using var context = await Connection.CreateDbContextAsync();
		if (await context.Personals.AnyAsync(s => s.Id != model.Id && s.FirstName == model.FirstName && s.LastName == model.LastName && s.DateOfBirth == model.DateOfBirth))
			return ApiResponse.ErrorResponse("Duplicate item.");

		var row = await context.Personals.FindAsync(model.Id);
		if (row is null)
			return ApiResponse.ErrorResponse("Not found.");

		row.FirstName = model.FirstName; 
		row.LastName = model.LastName;
		row.DateOfBirth = model.DateOfBirth.GetValueOrDefault();
		row.DateModified = DateTime.UtcNow;

		try
		{
			context.Personals.Update(row);
			await context.SaveChangesAsync();
			return ApiResponse.SuccessResponse();
		}
		catch (Exception ex)
		{
			return ApiResponse.ErrorResponse($"Failed to update {ex.Message}");
		}
	}
}
