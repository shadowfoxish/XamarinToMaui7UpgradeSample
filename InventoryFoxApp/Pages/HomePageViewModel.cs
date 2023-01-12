using InventoryFoxApp.Interfaces;
using InventoryFoxApp.Interfaces.Services;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Windows.Input;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace InventoryFoxApp.Pages
{
	public class HomePageViewModel : NavUserViewModelBase
	{
		private string api;

		private string version;

		public HomePageViewModel(IUserService userService, IMessagePopupPlatformService popup, IOptions<FoxConfig> config) : base(userService, popup)
		{
			api = config.Value.BackofficeApiUrl;
			version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			//HomePage doesnt require any permissions, but user does need to be logged in

		}

		public string Api
		{
			get {
				return api;
			}
			set {
				api = value;
				NotifyPropertyChanged(nameof(api));
			}
		}

		public string Version
		{
			get {
				return version;
			}
			set {
				version = value;
				NotifyPropertyChanged(nameof(Version));
			}
		}
	}
}
