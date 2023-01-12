using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace InventoryFoxApp.Plumbing
{
	public class FocusOnShowBehavior : Behavior<Entry>
	{
		protected override void OnAttachedTo(Entry bindable)
		{
			bindable.PropertyChanged += Bindable_PropertyChanged;
			base.OnAttachedTo(bindable);
		}

		protected override void OnDetachingFrom(Entry bindable)
		{
			bindable.PropertyChanged -= Bindable_PropertyChanged;
			base.OnDetachingFrom(bindable);
		}

		private void Bindable_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(Entry.IsVisible) && ((Entry)sender).IsVisible == true)
			{
				((Entry)sender).Focus();
			}
		}
	}
}
