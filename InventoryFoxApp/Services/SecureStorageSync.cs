using System.Threading.Tasks;


namespace InventoryFoxApp.Services
{
	public static class SecureStorageSync
	{
		public static string Get(string key)
		{
			Task<string> task = Task.Run(async () => await SecureStorage.GetAsync(key));
			return task.Result;
		}

		public static void Set(string key, string value)
		{
			Task task = Task.Run(async () => await SecureStorage.SetAsync(key, value));
			task.Wait();
		}
	}
}
