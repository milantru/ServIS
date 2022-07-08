using ServISData;
using ServISData.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDbContextFactory<ServISDbContext>(options =>
	{
		var connectionString = ServISDbContextFactory.GetConnectionString();
		options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
	}
);
builder.Services.AddScoped<IServISApi, ServISApi>();

var app = builder.Build();

// Apply migration
var factory = app.Services.GetService<IDbContextFactory<ServISDbContext>>();
factory?.CreateDbContext().Database.Migrate();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
