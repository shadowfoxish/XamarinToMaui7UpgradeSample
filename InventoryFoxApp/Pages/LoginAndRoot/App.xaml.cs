using InventoryFoxApp.Interfaces;
using InventoryFoxApp.Interfaces.Services;
using InventoryFoxApp.Services;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace InventoryFoxApp.Pages
{
	public partial class App : Application
	{
		private readonly IOperatingSystemEventPropagator osEvents;
		private readonly IMessagePopupPlatformService popupService;
		private readonly IUserService userService;
		private readonly ISessionManager sessionManager;
		private readonly FoxConfig configuration;

		private System.Timers.Timer sessionManagerTimer;

		public App(IUserService userSvc, 
			ISessionManager sessionMgr, 
			IOperatingSystemEventPropagator osEventProp, 
			IMessagePopupPlatformService popup,
			IOptions<FoxConfig> config)
		{
			this.osEvents = osEventProp;
			this.popupService = popup;
			this.userService = userSvc;
			this.sessionManager = sessionMgr;
			this.configuration = config.Value;

			InitializeComponent();
			
			//The app will distribute the scan data from the underlying system to the currently displayed page, if possible

			osEvents.EnterKeyCallback = () =>
			{
				var page = (GetCurrentScreenViewModel() as IOperatingSystemEventListener);
				return page?.EnterKeyPressed() ?? false;
			};

			osEvents.F10KeyCallback = () =>
			{
				//Print the modal stack
				string[] pages = new string[App.Current.MainPage.Navigation.ModalStack.Count + App.Current.MainPage.Navigation.NavigationStack.Count];
				int i = 0;
				foreach (var m in App.Current.MainPage.Navigation.ModalStack)
				{
					pages[i] = m.GetType().Name;
					i++;
				}
				foreach (var m in App.Current.MainPage.Navigation.NavigationStack)
				{
					pages[i] = m.GetType().Name;
					i++;
				}
				popupService.ShowMessage(string.Join(Environment.NewLine, pages));
				return true;
			};

			osEvents.F9KeyCallback = () =>
			{
				var sessionManagerEnabled = sessionManagerTimer.Enabled;
				var sessionManagerCurrentStatus = sessionManager.GetCurrentStatus();
				var tokenExpiry = sessionManager.CurrentAuthTokenExpiration;
				var api = this.configuration.BackofficeApiUrl;

				popupService.ShowMessage($"SessionManager Enabled: {sessionManagerEnabled}\nCurrentStatus: {sessionManagerCurrentStatus}\nTokenExpires: {tokenExpiry.ToString("s")}\nAPI: {api}");
				return true;
			};

			osEventProp.F8KeyCallback = () =>
			{
				this.popupService.RepeatLastMessage();
				return true;
			};

			//Are we logged in?
			if (userService.IsLoggedIn())
			{
				ShowHomePage();
			}
			else
			{
				ResetForLogin();
			}
		}

		private async Task SetupSessionManagerThread()
		{
			string resumedLoginExpires = SecureStorageSync.Get(Constants.LoginExpirationKey);
			if (resumedLoginExpires != null)
			{
				DateTime expires = DateTime.ParseExact(resumedLoginExpires, "s", CultureInfo.CurrentCulture);
				sessionManager.SetExpirationDateTime(expires);
				sessionManager.Activity();
				if (sessionManager.GetCurrentStatus() == SessionStatus.RenewNeeded)
				{
					var newExpiration = await RenewLogin();
					sessionManager.SetExpirationDateTime(newExpiration);
				}
			}

			//Temp to resolve issue
			configuration.SessionManagerPollingIntervalSeconds = 10000;

			sessionManagerTimer = new System.Timers.Timer(configuration.SessionManagerPollingIntervalSeconds * 1000);
			sessionManagerTimer.Elapsed += T_Elapsed;
			sessionManagerTimer.Enabled = true;
		}

		private void T_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			//One limitation of this design; if the user logs in with less than the timer's timespan amount (20 seconds)
			// of token lifetime left, they will be logged out when the next cycle comes around. Should be a remote case though.
			MainThread.BeginInvokeOnMainThread(async () =>
			{
				try
				{
					var status = sessionManager.GetCurrentStatus();
					if (status == SessionStatus.RenewNeeded)
					{
						var newExpiration = await RenewLogin();
						sessionManager.SetExpirationDateTime(newExpiration);
					}
					else if (status == SessionStatus.Expired)
					{
						sessionManager.Reset();
						await Logout();
					}
				}
				catch (Exception)
				{

				}
			});
		}

		public static IServiceProvider ServiceProvider
				=>
#if WINDOWS10_0_17763_0_OR_GREATER
			MauiWinUIApplication.Current.Services;
#elif ANDROID
			MauiApplication.Current.Services;
#elif IOS || MACCATALYST
			MauiUIApplicationDelegate.Current.Services;
#else
			null;
#endif

		public object GetCurrentScreenViewModel()
		{
			//If a model is presented, send the event to it instead of a page.
			if (App.Current.MainPage.Navigation.ModalStack.Any())
			{
				return App.Current.MainPage.Navigation.ModalStack.LastOrDefault()?.BindingContext;
			}
			else
			{
				return App.Current.MainPage.Navigation.NavigationStack.LastOrDefault()?.BindingContext;
			}
		}

		public static string ConfigMode
		{
			get; set;
		}

		public void ResetForLogin()
		{
			MainPage = new NavigationPage(  App.ServiceProvider.GetService<LoginPage>());
		}

		public void ShowHomePage()
		{
			MainPage = new NavigationPage(App.ServiceProvider.GetService<HomePage>());
		}

		protected override void OnResume()
		{
		}

		protected override void OnSleep()
		{
		}

		protected override async void OnStart()
		{
			await SetupSessionManagerThread();
		}

		public void SetLoggedIn(DateTime expiration)
		{
			sessionManager.SetExpirationDateTime(expiration);
		}

		public async Task<DateTime> RenewLogin()
		{
			DateTime newExpiration = await userService.RenewAccessToken();
			return newExpiration;
		}

		public async Task Logout()
		{
			userService.Logout();
			//If anything other than the login screen is shown, reset.
			if (!(GetCurrentScreenViewModel() is LoginPageViewModel))
			{
				await App.Current.MainPage.Navigation.PopToRootAsync();
				ResetForLogin();
			}
		}
	}
}
