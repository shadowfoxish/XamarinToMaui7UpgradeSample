using InventoryFoxApp.Interfaces.Services;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace InventoryFoxApp.Services
{
	/// <summary>
	/// The purpose of SessionManager is to auto-logout users after a certain period of inactivity
	/// </summary>
	public class SessionManager : ISessionManager
	{
		private readonly FoxConfig configuration;

		public SessionManager(IOptions<FoxConfig> config)
		{
			this.configuration = config.Value;
		}

		private DateTime lastActivityDate
		{
			get; set;
		}

		public DateTime CurrentAuthTokenExpiration
		{
			get; private set;
		}
		
		public SessionManager()
		{
		}

		private bool Initialized
		{
			get; set;
		} = false;


		public void Activity()
		{
			lastActivityDate = DateTime.Now;
		}

		public void SetExpirationDateTime(DateTime expiration)
		{
			CurrentAuthTokenExpiration = expiration;
			Initialized = true;
		}

		public SessionStatus GetCurrentStatus()
		{
			if (Initialized)
			{
				//If its been over x seconds since the lastActivityDate or the currentAuthToken is expired, log user out
				if ((DateTime.Now - lastActivityDate) > new TimeSpan(0, 0, configuration.InactivityLogoutSeconds) || CurrentAuthTokenExpiration < DateTime.Now)
				{
					return SessionStatus.Expired;
				}
				else if (CurrentAuthTokenExpiration - DateTime.Now <= new TimeSpan(0, 0, configuration.TokenRenewThresholdSeconds))
				{ //If we're within x seconds of expiring, just renew the token
					return SessionStatus.RenewNeeded;
				}
				else
				{
					return SessionStatus.Valid;
				}
			}
			else 
			{
				return SessionStatus.NotInitialized;
			}
		}

		public void Reset()
		{
			Initialized = false;
			CurrentAuthTokenExpiration = DateTime.MinValue;
			lastActivityDate = DateTime.MinValue;
		}
	}
}
