using Super.Model.Collections;
using Super.Model.Selection;
using Super.Reflection;
using Super.Runtime.Activation;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Super.ExtensionMethods
{
	partial class Model
	{
		public static ISelect<IEnumerable<TFrom>, IEnumerable<TTo>> SelectMany<TFrom, TTo>(this ISelect<TFrom, IEnumerable<TTo>> @this)
			=> @this.ToDelegate().To(I<SelectManySelector<TFrom, TTo>>.Default);

		public static ISelect<IEnumerable<TFrom>, IEnumerable<TTo>> Select<TFrom, TTo>(this ISelect<TFrom, TTo> @this)
			=> @this.ToDelegate().To(I<SelectSelector<TFrom, TTo>>.Default);

		public static IEnumerable<T> AsEnumerable<T>(this ImmutableArray<T> @this)
			=> EnumerableSelector<T>.Default.Get(@this);
	}
}