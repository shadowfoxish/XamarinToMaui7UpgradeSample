using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InventoryFoxApp.Interfaces.Services
{
	public interface ISessionManager
	{
		DateTime CurrentAuthTokenExpiration
		{
			get;
		}

		/// <summary>
		/// Updates the last time a user has interacted with the App
		/// </summary>
		void Activity();
		/// <summary>
		/// Updates when the user's current login expires
		/// </summary>
		void SetExpirationDateTime(DateTime expiration);
		/// <summary>
		/// Gets the current session status with respect to activity time and the known expiration
		/// </summary>
		/// <returns></returns>
		SessionStatus GetCurrentStatus();
		/// <summary>
		/// Resets SessionManager to an uninitialized state (call this after logging out)
		/// </summary>
		void Reset();
	}

	public enum SessionStatus
	{
		Valid,
		RenewNeeded,
		Expired,
		NotInitialized,
	}
}
