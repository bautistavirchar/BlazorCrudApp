using BlazorCrudApp.Server.Data;
using BlazorCrudApp.Server.IoC;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
{
	options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
	{
		sqlOptions.EnableRetryOnFailure(
			maxRetryCount: 5,
			maxRetryDelay: TimeSpan.FromSeconds(30),
			errorNumbersToAdd: null);
	});
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddServices();
builder.Services.AddIdentityServices(builder.Configuration);

// Configure CORS policy
builder.Services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
{
	builder.AllowAnyMethod()
		.SetIsOriginAllowed(_ => true)
		.WithHeaders(HeaderNames.ContentType)
		.AllowCredentials();
}));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
	app.UseWebAssemblyDebugging();
	app.UseDeveloperExceptionPage();
	app.UseSwagger();
	app.UseSwaggerUI();
}
else
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseCors("CorsPolicy");
app.UseAuthorization();
app.UseAuthentication();
app.UseBlazorFrameworkFiles();
app.MapStaticAssets();
app.UseRouting();
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

await app.RunAsync();