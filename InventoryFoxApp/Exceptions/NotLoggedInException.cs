using System;

namespace InventoryFoxApp.Exceptions
{
	[Serializable]
	public class NotLoggedInException : Exception
	{
		public NotLoggedInException()
		{
		}

		public NotLoggedInException(string message) : base(message)
		{
		}

		public NotLoggedInException(string message, Exception inner) : base(message, inner)
		{
		}

		protected NotLoggedInException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
