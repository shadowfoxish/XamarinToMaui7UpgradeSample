namespace InventoryFoxApp.Interfaces.Services
{
	public interface IMessagePopupPlatformService
	{
		Task ShowMessage(string message);
		void HapticError();
		void HapticQuestion();
		Task RepeatLastMessage();
	}
}
