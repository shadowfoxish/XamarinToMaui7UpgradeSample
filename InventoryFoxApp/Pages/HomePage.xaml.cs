using System;

using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace InventoryFoxApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
		public HomePage(HomePageViewModel vm)
		{
			InitializeComponent();

			this.BindingContext = vm;
			Model.UseNavigation(this.Navigation);
		}

		private HomePageViewModel Model
		{
			get {
				return this.BindingContext as HomePageViewModel;
			}
		}

		private void MenuItemAbout_Clicked(object sender, EventArgs e)
		{
			DisplayAlert("Inventory Fox", $"Version {Model.Version}\nAPI {Model.Api}\nCreated by KRWH for Airline Hydraulics\nBe excellent to each other.\n================\nSPECIAL FUNCTIONS\nF8 - Redisplay last msg\nF9 - Display Session Stats\nF10 - Display App Stack\n", "Ok");
		}
	}
}
