using InventoryFoxApp.ServiceModels;
using System;
using System.Threading.Tasks;

namespace InventoryFoxApp.Interfaces.Services
{
	public interface IUserService
	{
		Task<bool> Authenticate(string username, string pin);

		void DemandPermission(Permissions perm);

		Permissions[] EnumeratePermissions();

		UserPreference[] GetUserPreferences();

		string GetUsersName();

		bool HasPermission(Permissions perm);

		bool IsLoggedIn();

		void Logout();

		Task<UserPreference[]> RefreshUserPreferences(UserPreference[] allPrefs = null);

		Task<DateTime> RenewAccessToken();

		Task<UserPreference[]> SaveUserPreferences(UserPreference[] updatedPrefs);
	}
}
