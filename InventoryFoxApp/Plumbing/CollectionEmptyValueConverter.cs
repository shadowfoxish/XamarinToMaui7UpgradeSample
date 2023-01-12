using System;
using System.Collections;
using System.Globalization;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace InventoryFoxApp.Plumbing
{
	public class CollectionEmptyValueConverter : IValueConverter
	{
		public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is ICollection)
			{
				return (value as ICollection).Count == 0;
			}
			else if (value == null)
			{
				return true;
			}
			else
			{
				throw new Exception("CollectionEmptyValueConverter: value not a collection");
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class CollectionNotEmptyValueConverter : CollectionEmptyValueConverter
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return !((bool)base.Convert(value, targetType, parameter, culture));
		}
	}
}
