using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace InventoryFoxApp.Plumbing
{
	public static class StringExtensions
	{
		public static string MakeReadable(this string camelCasedString)
		{
			return Regex.Replace(
				Regex.Replace(
					camelCasedString,
					@"(\P{Ll})(\P{Ll}\p{Ll})",
					"$1 $2"
				),
				@"(\p{Ll})(\P{Ll})",
				"$1 $2").Replace("_", "-");
		}
	}
}
