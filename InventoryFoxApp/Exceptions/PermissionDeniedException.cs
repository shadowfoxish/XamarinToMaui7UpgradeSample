using System;

namespace InventoryFoxApp.Exceptions
{
	[Serializable]
	public class PermissionDeniedException : Exception
	{
		public PermissionDeniedException()
		{
		}

		public PermissionDeniedException(Permissions perm) : this(perm.ToString())
		{
		}

		public PermissionDeniedException(string message) : base(message)
		{
		}

		public PermissionDeniedException(string message, Exception inner) : base(message, inner)
		{
		}

		protected PermissionDeniedException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
