using Super.Model.Selection;
using Super.Runtime;
using Super.Runtime.Objects;
using Super.Text;
using Super.Text.Formatting;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Super
{
	// ReSharper disable once MismatchedFileName

	public static partial class ExtensionMethods
	{
		public static KeyValuePair<Type, Func<string, Func<object, IProjection>>> Entry<T>(
			this IFormattedProjection<T> @this)
		{
			return Pairs.Create(Reflection.Types.Type<T>.Instance,
			                    @this.Select(Super.Start.From<object>().Cast<T>().Select)
			                         .Select(x => x.ToDelegate())
			                         .ToDelegate());
		}

		public static IProjection Get<T>(this IFormattedProjection<T> @this, T parameter) => @this.Get(null)(parameter);

		public static ISelect<T, IProjection> Project<T>(this IFormatter<T> @this,
		                                                 params Expression<Func<T, object>>[] expressions)
			=> new Projection<T>(@this, expressions);

		public static KeyValuePair<string, Func<T, IProjection>> Entry<T>(
			this IFormatEntry<T> @this, params Expression<Func<T, object>>[] expressions)
		{
			var pair = @this.Get();
			return pair.Entry(expressions);
		}

		public static KeyValuePair<string, Func<T, IProjection>> Entry<T>(
			ref this KeyValuePair<string, Func<T, string>> @this,
			params Expression<Func<T, object>>[] expressions)
			=> Pairs.Create(@this.Key, new Projection<T>(@this.Value, expressions).ToDelegate());
	}
}