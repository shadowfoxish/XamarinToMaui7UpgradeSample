using System;
using System.Globalization;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace InventoryFoxApp.Plumbing
{
	//Example
	/*
	 *  
	 *			 
	 *	<ContentPage.Resources>
	 *		<fox:TrueIfConverter x:Key="TrueIfConverter"></fox:TrueIfConverter>
	 *	</ContentPage.Resources>
	 *  ...
	 *	<StackLayout IsVisible="{Binding Path=Step, Converter={StaticResource Key=TrueIfConverter}, ConverterParameter={Static self:CurrentStep.CountGetLocation}}">
	 *		<Label >Scan/Enter the Receiving Location</Label>
	 *		<Entry Text="{Binding Path=ReceivingLocation}"></Entry>
	 *		<Button Text="Lookup" Command="{Binding Path=DoLocationLookup}"></Button>
	 *	</StackLayout>
	 */

	public class TrueIfConverter : IValueConverter
	{
		public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value.ToString().Equals(parameter.ToString());
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
	public class TrueIfNotConverter : TrueIfConverter
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return !((bool)base.Convert(value, targetType, parameter, culture));
		}
	}


	public class LessThanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return decimal.Parse(value.ToString()) < decimal.Parse(parameter.ToString());
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
