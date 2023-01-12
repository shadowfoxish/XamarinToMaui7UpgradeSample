using System;
using System.Collections.Generic;

namespace InventoryFoxApp.ServiceModels
{
	public enum LoginSource : int
	{
		Unset = 0,
		Domain = 1,
		WarehousePin = 2
	}

	public class UserInfo
	{
		public UserInfo()
		{
			Permissions = new List<string>();
		}

		public string BearerToken
		{
			get; set;
		}

		public DateTime Expiration
		{
			get; set;
		}

		public int LocationId
		{
			get; set;
		}

		public string LocationName
		{
			get; set;
		}

		public LoginSource LoginSource
		{
			get; set;
		}

		public string Name
		{
			get; set;
		}

		public List<string> Permissions
		{
			get; set;
		}

		public string UserName
		{
			get; set;
		}
	}
}
