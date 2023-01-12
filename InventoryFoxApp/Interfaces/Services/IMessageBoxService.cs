using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InventoryFoxApp.Interfaces.Services
{
	public interface IMessageBoxService
	{
		Task<string> DisplayChoiceYesNoCancel(string title);
		Task<string> DisplayChoice(string title, string cancelOption, params string[] options);
		Task DisplayPrompt(string title, string message);
	}
}
