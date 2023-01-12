using System;
using System.Globalization;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace InventoryFoxApp.Plumbing
{
	public class SettingsIssueIconConverter : IValueConverter
	{
		private ImageSourceConverter conv = new ImageSourceConverter();
		//When there is an issue, show a special icon, otherwise show the normal one
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object s;
			
			if (value is bool)
			{

				 s = (bool)value ? conv.ConvertFromInvariantString("settings_disabled_24dp") : conv.ConvertFromInvariantString("settings_24dp");
			}
			else
			{
				 s = conv.ConvertFromInvariantString("settings_24dp");
			}
			return s;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
