using InventoryFoxApp.Interfaces.Services;
using InventoryFoxApp.Pages;
using InventoryFoxApp.Services;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using Microsoft.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Maui;

namespace InventoryFoxApp
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			//Read the config file
			var a = Assembly.GetExecutingAssembly();
			using var stream = a.GetManifestResourceStream($"{nameof(InventoryFoxApp)}.appsettings.json");
			var config = new ConfigurationBuilder()
				.AddJsonStream(stream)
				.Build();

			//Bootstrap the app
			var builder = MauiApp.CreateBuilder();
			builder.UseMauiApp<App>()
				.UseMauiCommunityToolkit();
			builder.Configuration.AddConfiguration(config);
			ConfigureServices(builder.Configuration, builder.Services);

			return builder
				.Build();			
		}

		private static void ConfigureServices(ConfigurationManager ctx, IServiceCollection services)
		{
			FoxConfig config = new FoxConfig();
			ctx.GetSection(nameof(FoxConfig)).Bind(config);
			services.AddOptions<FoxConfig>(nameof(FoxConfig));
			
			services.AddHttpClient("BackOfficeAPI", c =>
			{
				//This line of code is crashing the app
				//c.BaseAddress = new Uri(config.BackofficeApiUrl);
			});
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IBackofficeRestClient, BackofficeHttpRestClient>();
			services.AddScoped<IOperatingSystemEventPropagator, OperatingSystemEventPropagator>();
			services.AddSingleton<ISessionManager, SessionManager>();
			services.AddScoped<IMessageBoxService, MessageBoxService>();

			services.AddTransient<LoginPageViewModel>();
			services.AddTransient<LoginPage>();

			services.AddTransient<HomePageViewModel>();
			services.AddTransient<HomePage>();


			services.AddSingleton<App>();

			/*Platform level services*/
			services.AddSingleton<IMessagePopupPlatformService, ToastMessagePopupPlatformService>();
		}
	}
}
