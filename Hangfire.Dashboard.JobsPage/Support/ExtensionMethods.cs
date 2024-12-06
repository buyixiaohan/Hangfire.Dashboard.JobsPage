﻿using System.Linq;

namespace Hangfire.Dashboard.JobsPage.Support
{
	public static class ExtensionMethods
	{
		public static string ScrubURL(this string seed)
		{
			var _validCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789/\\_-".ToCharArray();
			string result = "";
			foreach (var s in seed.ToCharArray())
			{
				if (_validCharacters.Contains(s))
				{
					result += s;
				}
				else {
                    result += s;
                }
			}
			return result;
		}
	}
}
