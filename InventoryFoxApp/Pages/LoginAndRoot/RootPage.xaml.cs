using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace InventoryFoxApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RootPage : ContentPage
	{
		public RootPage()
		{
			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			//Nothing is shown in the root, it just immediately shows HomePage
			Navigation.PushAsync(App.ServiceProvider.GetService<HomePage>());
		}
	}
}
