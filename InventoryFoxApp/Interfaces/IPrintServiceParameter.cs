using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryFoxApp.Interfaces
{
	public interface IPrintServiceParameter
	{
		int Quantity
		{
			get;
			set;
		}
		string PrinterIdTag
		{
			get;
			set;
		}
	}
}
