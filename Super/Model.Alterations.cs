using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Runtime.Invocation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Super
{
	public static partial class ExtensionMethods
	{
		public static T Alter<T>(this IEnumerable<IAlteration<T>> @this, T seed)
			=> @this.Aggregate(seed, (current, alteration) => alteration.Get(current));

		public static TResult Alter<T, TResult>(this IEnumerable<T> @this, Func<T, TResult> alter)
			=> @this.Select(alter).Last();

		public static IAlteration<T> ToAlteration<T>(this ISelect<T, T> @this) => @this.ToDelegate().ToAlteration();

		public static IAlteration<T> ToAlteration<T>(this Func<T, T> @this) => Alterations<T>.Default.Get(@this);

		public static ISelect<TParameter, TResult> Protect<TParameter, TResult>(this ISelect<TParameter, TResult> @this)
			=> ProtectAlteration<TParameter, TResult>.Default.Get(@this);

		public static ISelect<TParameter, TResult> Stripe<TParameter, TResult>(this ISelect<TParameter, TResult> @this)
			=> StripedAlteration<TParameter, TResult>.Default.Get(@this);
	}
}