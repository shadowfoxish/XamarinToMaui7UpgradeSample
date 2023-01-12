using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InventoryFoxApp.Interfaces.Services
{
	public interface IPrintingService
	{
		Task PrintPo150(int quantity, string printerIdTag, params int[] poNumbers);

		Task PrintItemBarcodeLabel(string printerIdTag, int invMastUid, int quantity);
	}
}
