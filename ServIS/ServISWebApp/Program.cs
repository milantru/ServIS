using ServISData;
using ServISData.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Authorization;
using ServISWebApp.Auth;
using Syncfusion.Licensing;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Popups;
using ServISWebApp.Shared;
using ServISWebApp.BackgroundServices;
using ServISData.Models;

internal class Program
{
    /// <summary>
    /// Stores new autogenerated messages in the database.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method creates and adds new <see cref="AutogeneratedMessage"/> objects to the <see cref="ServISDbContext"/>
    /// if they do not already exist in the database. The autogenerated messages are determined
    /// based on the values of the <see cref="AutogeneratedMessage.For"/> enum. The <see cref="AutogeneratedMessage.Subject"/> and <see cref="AutogeneratedMessage.Message"/> properties
    /// of the new autogenerated messages are set to empty strings.
    /// </para>
    /// <para>
    /// This way we can achieve there is always one instance of <see cref="AutogeneratedMessage"/> 
    /// ready for each value of <see cref="AutogeneratedMessage.For"/>
    /// </para>
    /// </remarks>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    private static async Task StoreNewAutogeneratedMessagesAsync(ServISDbContext context)
    {
        var forWhoms = Enum.GetValues<AutogeneratedMessage.For>();
        var autogeneratedMessages = new List<AutogeneratedMessage>();
        foreach (var forWhom in forWhoms)
        {
            var autogeneratedMessageExists = await context.AutogeneratedMessages.AnyAsync(am => am.ForWhom == forWhom);
            if (!autogeneratedMessageExists)
            {
                var autogeneratedMessage = new AutogeneratedMessage()
                {
                    Subject = string.Empty,
                    Message = string.Empty,
                    ForWhom = forWhom
                };

                autogeneratedMessages.Add(autogeneratedMessage);
            }
        }

        context.AutogeneratedMessages.AddRange(autogeneratedMessages);

        context.SaveChanges();
    }

    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthenticationCore();
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Services.AddScoped<ProtectedLocalStorage>();
        builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
        builder.Services.AddDbContextFactory<ServISDbContext>(options =>
        {
            var connectionString = ServISDbContextFactory.GetConnectionString();
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });
        builder.Services.AddSingleton<IServISApi, ServISApi>();
        builder.Services.AddScoped<SfDialogService>();
		builder.Services.AddSyncfusionBlazor();
        builder.Services.AddSingleton(typeof(ISyncfusionStringLocalizer), typeof(SyncfusionLocalizer));
        builder.Services.AddSingleton<EmailManager>(provider =>
        {
            var config = provider.GetRequiredService<IConfiguration>();
            var emailName = config.GetValue<string>("EmailName");
            var emailAddress = config.GetValue<string>("EmailAddress");
            var emailPassword = config.GetValue<string>("EmailAppPassword");
            var logger = provider.GetRequiredService<ILogger<EmailManager>>();

            return new(emailName, emailAddress, emailPassword, logger);
        });
        builder.Services.AddHostedService<EverySecondTimerService>();
        builder.Services.AddHostedService<AuctionEvaluatorService>(provider =>
        {
            var emailManager = provider.GetRequiredService<EmailManager>();
            var api = provider.GetRequiredService<IServISApi>();
            var baseUrl = provider.GetRequiredService<IConfiguration>().GetValue<string>("AppBaseUrl");
            var logger = provider.GetRequiredService<ILogger<AuctionEvaluatorService>>();

			return new(api, emailManager, baseUrl, logger);
        });
        builder.Services.AddScoped<Modals>(factory =>
        {
            var dialogService = factory.GetRequiredService<SfDialogService>();

            return new(dialogService);
        });

        var app = builder.Build();

        app.UseRequestLocalization("sk");

        // Register Syncfusion license
        var syncfusionLicenceKey = builder.Configuration["SyncfusionLicenseKey"];
        SyncfusionLicenseProvider.RegisterLicense(syncfusionLicenceKey);

        // Apply migration
        var factory = app.Services.GetRequiredService<IDbContextFactory<ServISDbContext>>();
        using (var context = factory.CreateDbContext())
        {
            context.Database.Migrate();
        }

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

        using (var context = factory.CreateDbContext())
        {
            await StoreNewAutogeneratedMessagesAsync(context);
        }

        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        app.Run();
    }
}
