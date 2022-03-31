using BServisData;
using BServisData.Interfaces;
using BServisWebApp.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddDbContextFactory<BServisDbContext>(options =>
	{
		options.UseMySQL(BServisDbContextFactory.GetConnectionString());
	}
);
builder.Services.AddScoped<IBServisApi, BServisApi>();

var app = builder.Build();

// Apply migration
var factory = app.Services.GetService<IDbContextFactory<BServisDbContext>>();
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
