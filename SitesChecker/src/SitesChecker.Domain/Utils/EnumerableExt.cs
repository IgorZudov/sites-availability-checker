using System;
using System.Collections.Generic;

namespace SitesChecker.Domain.Utils
{
	public static  class EnumerableExt
	{
		public static void Foreach<T>(this IEnumerable<T> collection, Action<T> action)
		{
			foreach (var value in collection)
			{
				action(value);
			}
		}
	}
}