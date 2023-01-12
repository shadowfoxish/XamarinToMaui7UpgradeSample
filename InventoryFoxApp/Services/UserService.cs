using InventoryFoxApp.Exceptions;
using InventoryFoxApp.Interfaces.Services;
using InventoryFoxApp.ServiceModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;


namespace InventoryFoxApp.Services
{
	public class UserService : IUserService
	{
		private readonly IBackofficeRestClient _client;

		public UserService(IBackofficeRestClient restClient)
		{
			_client = restClient;
		}

		public async Task<bool> Authenticate(string username, string pin)
		{
			Uri request = new Uri(_client.BaseUri, "api/User/WarehouseLogin");

			WarehouseLoginCredential c = new WarehouseLoginCredential()
			{
				Pin = pin,
				Username = username
			};

			SecureStorage.RemoveAll();

			UserInfo userInfo = await _client.Post<UserInfo, WarehouseLoginCredential>(request, c);

			if (userInfo != null)
			{
				ProcessNewUserInfo(userInfo);
				return true;
			}
			else
			{
				return false;
			}
		}

		private void ProcessNewUserInfo(UserInfo userInfo)
		{
			if (userInfo.Permissions.Any(p => p == nameof(Permissions.WarehouseLogin)))
			{
				SecureStorageSync.Set(Constants.ApiTokenStorageKey, userInfo.BearerToken);
				SecureStorageSync.Set(Constants.NameStorageKey, userInfo.Name);
				SecureStorageSync.Set(Constants.LocationId_Key, userInfo.LocationId.ToString());
				SecureStorageSync.Set(Constants.LocationName_Key, userInfo.LocationName);
				SecureStorageSync.Set(Constants.LoginExpirationKey, userInfo.Expiration.ToString("s"));

				foreach (string perm in Enum.GetNames(typeof(Permissions)))
				{
					if (userInfo.Permissions.Contains(perm))
					{
						SecureStorageSync.Set(Constants.Permission_Prefix + perm, "Y");
					}
					else
					{
						SecureStorageSync.Set(Constants.Permission_Prefix + perm, "N");
					}
				}
			}
			else
			{
				//User authenticated, but doesn't have permission to use the app.
				throw new PermissionDeniedException("You are logged in, but not authorized to use this application.");
			}
		}

		public async Task<DateTime> RenewAccessToken()
		{
			Uri request = new Uri(_client.BaseUri, "api/User/RenewToken");
			UserInfo userInfo = await _client.Get<UserInfo>(request);
			ProcessNewUserInfo(userInfo);
			return userInfo.Expiration;
		}

		public void DemandPermission(Permissions perm)
		{
			bool grant = HasPermission(perm);
			if (!grant)
			{
				throw new PermissionDeniedException(perm);
			}
		}

		public Permissions[] EnumeratePermissions()
		{
			List<Permissions> owned = new List<Permissions>();
			foreach (Permissions p in Enum.GetValues(typeof(Permissions)))
			{
				if (HasPermission(p))
				{
					owned.Add(p);
				}
			}
			return owned.ToArray();
		}

		/// <summary>
		/// Reads locally stored preferences
		/// </summary>
		public UserPreference[] GetUserPreferences()
		{
			string prefsJson = SecureStorageSync.Get(Constants.PreferencesStorageKey);
			return prefsJson != null ? System.Text.Json.JsonSerializer.Deserialize<UserPreference[]>(prefsJson) : new UserPreference[0];
		}

		public string GetUsersName()
		{
			return SecureStorageSync.Get(Constants.NameStorageKey);
		}

		public bool HasPermission(Permissions perm)
		{
			return SecureStorageSync.Get(Constants.Permission_Prefix + perm) == "Y";
		}

		public bool IsLoggedIn()
		{
			string expiry = SecureStorageSync.Get(Constants.LoginExpirationKey);
			if (expiry != null)
			{
				//An expiration is set, so maybe we have a valid token to slip into
				if (DateTime.Now < DateTime.ParseExact(expiry, "s", CultureInfo.CurrentCulture))
				{
					//Not expired token, check for proper permission
					return SecureStorageSync.Get(Constants.Permission_Prefix + nameof(Permissions.WarehouseLogin)) == "Y";
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}

		public void Logout()
		{
			SecureStorage.RemoveAll();
		}

		/// <summary>
		/// Clears the locally stored preferences and asks for updated versions from the server, and sets them to local storage
		/// Pass null to do a server request for all preferences automatically.
		/// </summary>
		public async Task<UserPreference[]> RefreshUserPreferences(UserPreference[] allPrefs = null)
		{
			SecureStorage.Remove(Constants.PreferencesStorageKey);
			if (allPrefs == null)
			{
				Uri request = new Uri(_client.BaseUri, "api/User/Prefs");
				allPrefs = await _client.Get<UserPreference[]>(request);
			}
			SecureStorageSync.Set(Constants.PreferencesStorageKey, System.Text.Json.JsonSerializer.Serialize(allPrefs));
			return allPrefs;
		}

		/// <summary>
		/// Updates the server and locally stored preferences with new values for matching preferences
		/// </summary>
		/// <remarks>This implementation depends on the server returning ALL preferences, even though only some preferences may be specified in params.</remarks>
		public async Task<UserPreference[]> SaveUserPreferences(UserPreference[] updatedPrefs)
		{
			Uri request = new Uri(_client.BaseUri, "api/User/Prefs");
			UserPreference[] prefs = await _client.Post<UserPreference[], UserPreference[]>(request, updatedPrefs);
			return await RefreshUserPreferences(prefs);
		}
	}
}
