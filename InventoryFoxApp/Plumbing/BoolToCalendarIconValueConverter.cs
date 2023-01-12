using System;
using System.Globalization;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace InventoryFoxApp.Plumbing
{
	/// <summary>
	/// When true returns a calendar icon with an "X" on it
	/// When false, returns a calendar icon
	/// </summary>
	public class BoolToCalendarIconValueConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value.Equals(true))
			{
				return "calendar_x_24dp";
			}
			else
			{
				return "calendar_24dp";
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
