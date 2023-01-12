using InventoryFoxApp.Exceptions;
using InventoryFoxApp.Interfaces.Services;
using InventoryFoxApp.Pages;
using InventoryFoxApp.Plumbing;
using InventoryFoxApp.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace InventoryFoxApp.Interfaces
{
	public abstract class NavUserViewModelBase : NotifierBase
	{
		protected IMessagePopupPlatformService popupService;
		protected Permissions[] RequiredPermissions;
		protected IUserService userService;
		private string _username;

		public NavUserViewModelBase(IUserService userService, IMessagePopupPlatformService popupService) : this(userService, popupService, null)
		{
		}

		public NavUserViewModelBase(IUserService userService, IMessagePopupPlatformService popupService, Permissions[] requiredPermissions)
		{
			this.userService = userService;
			this.popupService = popupService;
			this.RequiredPermissions = requiredPermissions ?? new Permissions[0];
			this.UserName = userService.GetUsersName();
		}

		public void UseNavigation(INavigation navigation)
		{
			this.Navigation = navigation;
		}

		/// <summary>
		/// Do not expose Navigation publically; access to it needs to be controlled/invoked on the Main Thread
		/// </summary>
		private INavigation Navigation
		{
			get; set;
		}

		public async Task PopModalAsync()
		{
			if (MainThread.IsMainThread)
			{
				await this.Navigation.PopModalAsync();
			}
			else
			{
				await MainThread.InvokeOnMainThreadAsync(async () =>
				{
					await this.Navigation.PopModalAsync();
				});
			}
		}

		public async Task PushModalAsync(Page page)
		{
			if (MainThread.IsMainThread)
			{
				await this.Navigation.PushModalAsync(page, true);
			}
			else
			{
				await MainThread.InvokeOnMainThreadAsync(async () =>
				{
					await this.Navigation.PushModalAsync(page, true);
				});
			}
		}

		public async Task PushAsync(Page page)
		{
			if (MainThread.IsMainThread)
			{
				await this.Navigation.PushAsync(page, true);
			}
			else
			{
				await MainThread.InvokeOnMainThreadAsync(async () =>
				{
					await this.Navigation.PushAsync(page, true);
				});
			}
		}

		public async Task PopAsync()
		{
			if (MainThread.IsMainThread)
			{
				await this.Navigation.PopAsync(true);
			}
			else
			{
				await MainThread.InvokeOnMainThreadAsync(async () =>
				{
					await this.Navigation.PopAsync(true);
				});
			}
		}

		public async Task PopToRootAsync()
		{
			if (MainThread.IsMainThread)
			{
				await this.Navigation.PopToRootAsync(true);
			}
			else
			{
				await MainThread.InvokeOnMainThreadAsync(async () =>
				{
					await this.Navigation.PopToRootAsync(true);
				});
			}
		}

		public void InsertPageBeforeLast(Page page)
		{
			if (MainThread.IsMainThread)
			{
				this.Navigation.InsertPageBefore(page, Navigation.NavigationStack.Last());
			}
			else
			{
				MainThread.BeginInvokeOnMainThread(() =>
				{
					this.Navigation.InsertPageBefore(page, Navigation.NavigationStack.Last());
				});
			}
		}

		public string UserName
		{
			get {
				return _username;
			}
			set {
				_username = value;
				NotifyPropertyChanged(nameof(UserName));
			}
		}

		public async Task AccessCheck()
		{
			if (userService.IsLoggedIn())
			{
				//Logged in, lets see if we have the needed permissions
				List<Permissions> missing = new List<Permissions>();
				foreach (Permissions p in RequiredPermissions ?? new Permissions[0])
				{
					if (!(userService.HasPermission(p)))
					{
						missing.Add(p);
					}
				}
				if (missing.Any() && !(userService.HasPermission(Permissions.WarehouseAdministrator)))
				{
					throw new PermissionDeniedException("Missing the following permissions: " + string.Join(", ", missing.Select(m => m.ToString())));
				}
			}
			else
			{
				//Not logged in, redirect to Login

				await this.Navigation.PushModalAsync((Page)App.ServiceProvider.GetService(typeof(LoginPage)));
			}
		}

		public virtual async Task HandleEnterKey()
		{
			return;
		}

		/// <summary>
		/// Calls the HandleError method synchronously. 
		/// </summary>
		public void HandleErrorSynchronous(Exception ex)
		{
			HandleError(ex).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public async Task HandleError(Exception ex)
		{
			if (ex is NotLoggedInException)
			{
				popupService.HapticQuestion();
				await popupService.ShowMessage($"The backend reports your login has expired\n{ex.Message}");
				await (App.Current as App).Logout();
			}
			else if (ex is PermissionDeniedException)
			{
				await popupService.ShowMessage("Access denied.");
			}
			else if (ex is ApiErrorException)
			{
				popupService.HapticError();
				await popupService.ShowMessage("API ERROR: " + ex.Message);
			}
			else
			{
				popupService.HapticError();
				await popupService.ShowMessage("UNKNOWN ERROR " + ex.Message);
			}
		}

		public bool HasPermission(params Permissions[] permissions)
		{
			if (userService.IsLoggedIn())
			{
				List<Permissions> missing = new List<Permissions>();
				foreach (Permissions p in RequiredPermissions ?? new Permissions[0])
				{
					if (!userService.HasPermission(p))
					{
						missing.Add(p);
					}
				}
				return !missing.Any();
			}
			else
			{
				return false;
			}
		}

	}
}
