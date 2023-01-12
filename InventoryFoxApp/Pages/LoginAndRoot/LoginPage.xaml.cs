using InventoryFoxApp.Pages;
using System.Linq;

using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace InventoryFoxApp.Pages
{
	public partial class LoginPage : ContentPage
	{
		public LoginPage(LoginPageViewModel viewModel)
		{
			this.BindingContext = viewModel;
			Model.UseNavigation(this.Navigation);
		}

		private LoginPageViewModel Model
		{
			get {
				return this.BindingContext as LoginPageViewModel;
			}
		}
		protected async override void OnAppearing()
		{
			try
			{
				base.OnAppearing();
				count = 0;

				if (Model.IsLoggedIn())
				{
					Navigation.InsertPageBefore((Page)App.ServiceProvider.GetService(typeof(HomePage)), this);
					await Navigation.PopToRootAsync();
				}
				else
				{
					InitializeComponent();
				}
			}
			catch { } 
		}
		private int count = 0;
		private async void ImageButton_Clicked(object sender, System.EventArgs e)
		{
			count++;
			if (count == 5)
			{
				string response = await this.DisplayActionSheet("Set connection mode", "Cancel", null, nameof(FoxConfig.BackofficeApiUrl_dev), nameof(FoxConfig.BackofficeApiUrl_jake), nameof(FoxConfig.BackofficeApiUrl_qa), nameof(FoxConfig.BackofficeApiUrl_live));
				if (response != "Cancel")
				{
					Preferences.Set(nameof(FoxConfig.BackofficeApiUrl), response);
					await this.DisplayAlert("Preference Updated", "You will need to restart the application for the change to take effect. This preference will be remembered on the device until you change it or uninstall the app.","Ok");
				}
				count = 0;
			}
		}
	}
}
