using InventoryFoxApp.Exceptions;
using InventoryFoxApp.Interfaces;
using InventoryFoxApp.Interfaces.Services;
using InventoryFoxApp.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;

using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace InventoryFoxApp.Pages
{
	public class LoginPageViewModel : NavUserViewModelBase, IOperatingSystemEventListener
	{
		private string _api;

		private bool _failedLogin = false;

		private bool _isWorking = false;

		private string _message;

		private string _pin;

		private string _username;

		private string _version;

		public LoginPageViewModel(ILogger<LoginPageViewModel> logger, IOptions<FoxConfig> config, IUserService userService, IMessagePopupPlatformService popup) : base(userService, popup)
		{
			_api = config.Value.BackofficeApiUrl;
			
			LoginCommand =
				new Command(
					async () =>
					{
						this.IsWorking = true;
						await Login();
						this.IsWorking = false;
					});
			string configMode = Preferences.Get(nameof(FoxConfig.BackofficeApiUrl), nameof(FoxConfig.BackofficeApiUrl_live));
			Version = Assembly.GetExecutingAssembly().GetName().Version.ToString() + (configMode != nameof(FoxConfig.BackofficeApiUrl_live) ? " " + configMode : "");
		}

		public bool FailedLogin
		{
			get {
				return _failedLogin;
			}
			set {
				_failedLogin = value;
				NotifyPropertyChanged(nameof(FailedLogin));
			}
		}

		public bool IsWorking
		{
			get {
				return _isWorking;
			}
			set {
				_isWorking = value;
				NotifyPropertyChanged(nameof(IsWorking));
			}
		}

		public ICommand LoginCommand
		{
			private set; get;
		}

		public string Message
		{
			get {
				return _message;
			}
			set {
				_message = value;
				NotifyPropertyChanged(nameof(Message));
			}
		}

		public string Pin
		{
			get {
				return _pin;
			}
			set {
				_pin = value;
				NotifyPropertyChanged(nameof(Pin));
			}
		}

		public string Username
		{
			get {
				return _username;
			}
			set {
				_username = value;
				NotifyPropertyChanged(nameof(Username));
			}
		}

		public string Version
		{
			get {
				return _version;
			}
			set {
				_version = value;
				NotifyPropertyChanged(nameof(Version));
			}
		}

		public bool EnterKeyPressed()
		{
			LoginCommand.Execute(null);
			return true;
		}

		public bool IsLoggedIn()
		{
			return userService.IsLoggedIn();
		}

		public async Task Login()
		{
			try
			{
				bool loggedIn = await base.userService.Authenticate(this.Username, this.Pin);
				if (loggedIn)
				{
					DateTime expires = DateTime.ParseExact(SecureStorageSync.Get(Constants.LoginExpirationKey), "s", CultureInfo.CurrentCulture);
					await base.userService.RefreshUserPreferences();
					(App.Current as App).SetLoggedIn(expires);
					base.InsertPageBeforeLast((Page)App.ServiceProvider.GetService(typeof(HomePage)));
					await base.PopToRootAsync();
				}
				else
				{
					this.FailedLogin = true;
					this.Message = "Failed to log in.";
					this.Pin = "";
					this.Username = "";
					await base.popupService.ShowMessage("Failed to log in");
				}
			}
			catch (PermissionDeniedException ex)
			{
				this.FailedLogin = true;
				this.Message = ex.Message;
				await base.popupService.ShowMessage("Permission denied");
			}
			catch (NotLoggedInException ex)
			{
				this.FailedLogin = true;
				this.Message = ex.Message;
				await base.popupService.ShowMessage("Login failed");
			}
			catch (Exception ex)
			{
				this.FailedLogin = true;
				this.Message = ex.Message;
				await base.popupService.ShowMessage($"Error: {ex.Message}\n{_api}");
			}
		}
	}
}
