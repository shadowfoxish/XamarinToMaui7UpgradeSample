using System;

namespace InventoryFoxApp.Interfaces.Services
{
	public interface IOperatingSystemEventPropagator
	{
		Func<bool> EnterKeyCallback
		{
			get; set;
		}

		Func<bool> F10KeyCallback
		{
			get;
			set;
		}

		Func<bool> F8KeyCallback
		{
			get;
			set;
		}

		Func<bool> F9KeyCallback
		{
			get;
			set;
		}
		bool EnterKeyPressed();

		bool F10KeyPressed();

		bool F8KeyPressed();

		bool F9KeyPressed();
	}
}
