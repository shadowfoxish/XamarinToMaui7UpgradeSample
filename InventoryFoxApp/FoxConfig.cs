

namespace InventoryFoxApp
{
	public class FoxConfig
	{
		public string BackofficeApiUrl_qa
		{
			get;
			set;
		}
		public string BackofficeApiUrl_dev
		{
			get; set;
		}
		public string BackofficeApiUrl_jake
		{
			get; set;
		}
		public string BackofficeApiUrl_live
		{
			get; set;
		}
		/// <summary>
		/// How often SessionManager will be polled to auto-logout or auto-renew tokens.
		/// </summary>
		public int SessionManagerPollingIntervalSeconds
		{
			get; set;
		}

		/// <summary>
		/// How close to expiration before a token renewal will be requested
		/// </summary>
		public int TokenRenewThresholdSeconds
		{
			get; set;
		}

		/// <summary>
		/// This time controls when InventoryFox will force a user logout after this amount of inactivity, even if the Token is still valid. 
		/// </summary>
		public int InactivityLogoutSeconds
		{
			get; set;
		}

		public string BackofficeApiUrl
		{
			get {
				string apiSource = Preferences.Get(nameof(BackofficeApiUrl), nameof(BackofficeApiUrl_live));
				switch (apiSource)
				{
					case nameof(BackofficeApiUrl_dev):
						return BackofficeApiUrl_dev;
					case nameof(BackofficeApiUrl_jake):
						return BackofficeApiUrl_jake;
					case nameof(BackofficeApiUrl_qa):
						return BackofficeApiUrl_qa;
					default:
						return BackofficeApiUrl_live;
				}
			}
		}
	}
}
