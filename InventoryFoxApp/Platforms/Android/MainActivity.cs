using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using InventoryFoxApp.Interfaces.Services;
using InventoryFoxApp.Pages;
using InventoryFoxApp.ServiceModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace InventoryFoxApp
{
	[Activity(Label = "InventoryFoxApp", Icon="@mipmap/ic_launcher_round", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
	public class MainActivity : MauiAppCompatActivity
	{

		private IOperatingSystemEventPropagator osEventProp;

		private ISessionManager sessionManager;


		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
		{
			//Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			//Xamarin.Essentials.Platform.Init(this, savedInstanceState);
			//global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

			//LoadApplication(MauiProgram.Init(ConfigureServices));
			osEventProp = (IOperatingSystemEventPropagator)App.ServiceProvider.GetService(typeof(IOperatingSystemEventPropagator));
			sessionManager = (ISessionManager)App.ServiceProvider.GetService(typeof(ISessionManager));
		}

		protected override void OnPause()
		{
			base.OnPause();

		
			sessionManager.Activity();
		}

		protected override void OnResume()
		{
			base.OnResume();

			Log.Info(nameof(MainActivity), "OnResume");

		}

		public override bool DispatchKeyEvent(KeyEvent e)
		{
			sessionManager.Activity();
			//Key UP event and DeviceID is not the virtual keyboard wedge scanner
			if (e.Action == KeyEventActions.Up && e.DeviceId != -1)
			{
				if (e.KeyCode == Keycode.Enter || e.KeyCode == Keycode.NumpadEnter)
				{
					if (osEventProp.EnterKeyPressed())
					{
						return true;
					}
				}
				else if (e.KeyCode == Keycode.F10)
				{
					if (osEventProp.F10KeyPressed())
					{
						return true;
					}
				}
				else if (e.KeyCode == Keycode.F9)
				{
					if (osEventProp.F9KeyPressed())
					{
						return true;
					}
				}
				else if (e.KeyCode == Keycode.F8)
				{
					if (osEventProp.F8KeyPressed())
					{
						return true;
					}
				}
			}
			return base.DispatchKeyEvent(e);
		}
	}
}
