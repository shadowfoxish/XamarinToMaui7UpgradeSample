using System;

namespace InventoryFoxApp.Exceptions
{
	[Serializable]
	public class ApiErrorException : Exception
	{
		public ApiErrorException()
		{
		}

		public ApiErrorException(string message) : base(message)
		{
		}

		public ApiErrorException(string message, Exception inner) : base(message, inner)
		{
		}

		protected ApiErrorException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
