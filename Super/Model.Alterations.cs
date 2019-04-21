using Super.Model.Selection.Alterations;
using System.Collections.Generic;
using System.Linq;

namespace Super
{
	// ReSharper disable once MismatchedFileName

	public static partial class ExtensionMethods
	{
		public static T Alter<T>(this IEnumerable<IAlteration<T>> @this, T seed)
			=> @this.Aggregate(seed, (current, alteration) => alteration.Get(current));

		/*public static TOut Alter<T, TOut>(this IEnumerable<T> @this, Func<T, TOut> alter)
			=> @this.Select(alter).Last();*/

		/*public static IAlteration<T> ToAlteration<T>(this ISelect<T, T> @this) => @this.ToDelegate().ToAlteration();

		public static IAlteration<T> ToAlteration<T>(this Func<T, T> @this) => Alterations<T>.Default.Get(@this);*/
	}
}