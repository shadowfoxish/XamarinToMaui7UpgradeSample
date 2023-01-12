namespace InventoryFoxApp
{
	public enum Permissions
	{
		/// <summary>
		/// User is allowed to access the warehouse application
		/// </summary>
		WarehouseLogin,

		/// <summary>
		/// User is allowed to change the location where they work
		/// </summary>
		WarehouseChangeLocation,

		/// <summary>
		/// User is allowed to register barcodes for products
		/// </summary>
		WarehouseRegisterBarcodes,

		/// <summary>
		/// User is an administrator, and can access everything
		/// </summary>
		WarehouseAdministrator,

		/// <summary>
		/// User is allowed to do PO receiving
		/// </summary>
		WarehouseReceivePO,

		/// <summary>
		/// User is allowed to put received items into stock
		/// </summary>
		WarehousePOPutAway,
	}

	public static class Constants
	{
		public const string ApiTokenStorageKey = "BackofficeApiToken";
		public const string LocationId_Key = "LocationId";
		public const string LocationName_Key = "LocationName";
		public const string NameStorageKey = "Name";
		public const string Permission_Prefix = "Perm_";
		public const string PreferencesStorageKey = "Prefs";
		public const string LoginExpirationKey = "Expiration";
	
	}
}
