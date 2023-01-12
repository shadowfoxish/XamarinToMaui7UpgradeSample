using InventoryFoxApp.Interfaces.Services;
using System;

namespace InventoryFoxApp.Services
{
	public class OperatingSystemEventPropagator : IOperatingSystemEventPropagator
	{
		public OperatingSystemEventPropagator()
		{
		}

		public Func<bool> EnterKeyCallback
		{
			get; set;
		}

		public Func<bool> F10KeyCallback
		{
			get; set;
		}

		public Func<bool> F8KeyCallback
		{
			get; set;
		}

		public Func<bool> F9KeyCallback
		{
			get; set;
		}

		public bool EnterKeyPressed()
		{
			if (EnterKeyCallback != null)
			{
				return EnterKeyCallback();
			}
			else
			{
				return false;
			}
		}

		public bool F10KeyPressed()
		{
			if (F10KeyCallback != null)
			{
				return F10KeyCallback();
			}
			else
			{
				return false;
			}
		}

		public bool F8KeyPressed()
		{
			if (F8KeyCallback != null)
			{
				return F8KeyCallback();
			}
			else
			{
				return false;
			}
		}

		public bool F9KeyPressed()
		{
			if (F9KeyCallback != null)
			{
				return F9KeyCallback();
			}
			else
			{
				return false;
			}
		}
	}
}
