using System;
using System.Collections.Generic;
using System.Linq;
using Super.Model.Sources;
using Super.Model.Sources.Alterations;

namespace Super.ExtensionMethods
{
	partial class Sources
	{
		public static T Alter<T>(this IEnumerable<IAlteration<T>> @this, T seed)
			=> @this.Aggregate(seed, (current, alteration) => alteration.Get(current));

		public static TResult Alter<T, TResult>(this IEnumerable<T> @this, Func<T, TResult> alter)
			=> @this.Select(alter).Last();

		public static IAlteration<T> ToAlteration<T>(this ISource<T, T> @this) => @this.ToDelegate().ToAlteration();

		public static IAlteration<T> ToAlteration<T>(this Func<T, T> @this) => Alterations<T>.Default.Get(@this);
	}
}