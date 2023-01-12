using InventoryFoxApp.Pages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace InventoryFoxApp.Pages
{
	public class PageBase<TViewModel> : ContentPage where TViewModel : class
	{
		public TViewModel Model
		{
			get; set;
		}

		public PageBase()
		{
			BindingContext = Model = App.ServiceProvider.GetRequiredService<TViewModel>();
		}
	}
}
