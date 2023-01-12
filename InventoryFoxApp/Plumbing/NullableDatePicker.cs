using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace InventoryFoxApp.Plumbing
{
	public class NullableDatePicker : DatePicker
	{
		public static readonly BindableProperty NullableDateProperty = BindableProperty.Create(nameof(NullableDate), typeof(DateTime?),
			typeof(NullableDatePicker), defaultValue: DateTime.MinValue, defaultBindingMode: BindingMode.TwoWay, propertyChanged: (obj, oldVal, newVal) => {
				var instance = (NullableDatePicker)obj;
				instance.UpdateDate();
			});
		private string defaultFormat = "yyyy-MM-dd";
		private string placeholder = @"\S\e\t...";
		public NullableDatePicker()
		{
			Format = defaultFormat;
		}

		public DateTime? NullableDate
		{
			get {
				return (DateTime?)GetValue(NullableDateProperty);
			}
			set {
				SetValue(NullableDateProperty, value);
				UpdateDate();
			}
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			if (BindingContext != null)
			{
				UpdateDate();
			}
		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			if (propertyName == nameof(Date))
			{
				NullableDate = Date;
			}
			else if (propertyName == nameof(NullableDate)) 
			{
				if (NullableDate.HasValue)
				{
					Date = NullableDate.Value;
					UpdateDate();
				} 
			}
		}

		private void UpdateDate()
		{
			if (NullableDate.HasValue)
			{
				if (defaultFormat != null)
				{
					Format = defaultFormat; 
				}
				Date = NullableDate.Value;
			}
			else 
			{ 
				Format = placeholder;
			}
		}
	}
}
