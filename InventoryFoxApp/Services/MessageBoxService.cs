using InventoryFoxApp.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace InventoryFoxApp.Services
{
	public class MessageBoxService : IMessageBoxService
	{
		private static Page CurrentMainPage
		{
			get {
				return Application.Current.MainPage;
			}

		}

		public async Task DisplayPrompt(string title, string message)
		{
			Vibration.Vibrate(100.0d);
			await CurrentMainPage.DisplayAlert(title, message, "Ok");
		}

		public async Task<string> DisplayChoice(string title, string cancelOption, params string[] options)
		{
			Vibration.Vibrate(250.0d);
			var action = await CurrentMainPage.DisplayActionSheet(title, cancelOption, null, options);
			return action;
		}

		public Task<string> DisplayChoiceYesNoCancel(string title)
		{
			return DisplayChoice(title, "Cancel", "Yes", "No");
		}
	}
}
