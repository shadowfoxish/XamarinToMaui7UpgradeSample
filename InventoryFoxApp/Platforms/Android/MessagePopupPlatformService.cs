using InventoryFoxApp.Interfaces.Services;
using System.Collections.Generic;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace InventoryFoxApp
{
	public class ToastMessagePopupPlatformService : IMessagePopupPlatformService
	{
		private string lastMessage;

		public void HapticError()
		{
			try
			{
				Vibration.Vibrate();
				//TODO sound?
			}
			catch { }
		}

		public void HapticQuestion()
		{
			try
			{
				Vibration.Vibrate(250.0d);
			}
			catch { }
		}

		public async Task RepeatLastMessage()
		{
			if (lastMessage != null)
			{
				await ShowMessage(lastMessage);
			}
		}

		public async Task ShowMessage(string message)
		{
			lastMessage = message;
			if (MainThread.IsMainThread)
			{
				await PopToast(message);	
			}
			else
			{
				await MainThread.InvokeOnMainThreadAsync(async () =>
				{
					await PopToast(message);
				});
			}
		}

		private async Task PopToast(string message)
		{
			CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
			var toast = Toast.Make(message, ToastDuration.Long, 14);
			await toast.Show(cancellationTokenSource.Token);
		}
	}
}
