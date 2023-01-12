using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryFoxApp.ServiceModels
{
	public class Problem
	{
		public string Type
		{
			get; set;
		}
		public string Title
		{
			get; set;
		}
		public decimal Status
		{
			get; set;
		}
		public string Detail
		{
			get; set;
		}
		public string TraceId
		{
			get; set;
		}
	}
}
