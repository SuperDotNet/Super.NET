using Super.Model.Selection;
using Super.Reflection.Types;
using Super.Runtime;
using Super.Runtime.Objects;
using Super.Text;
using Super.Text.Formatting;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Super.ExtensionMethods
{
	partial class Objects
	{
		public static KeyValuePair<Type, Func<string, Func<object, IProjection>>> Entry<T>(
			this IFormattedProjection<T> @this)
			=> Pairs.Create(Type<T>.Instance, @this.Out(x => x.ToSelect().In(Cast<object>.Default).ToDelegate()).ToDelegate());

		public static IProjection Get<T>(this IFormattedProjection<T> @this, T parameter) => @this.Get(null)(parameter);

		public static ISelect<T, IProjection> Project<T>(this IFormatter<T> @this,
		                                                params Expression<Func<T, object>>[] expressions)
			=> new Projection<T>(@this, expressions);

		public static KeyValuePair<string, Func<T, IProjection>> Entry<T>(this IFormat<T> @this,
		                                                                 params Expression<Func<T, object>>[] expressions)
			=> @this.Get().Entry(expressions);

		public static KeyValuePair<string, Func<T, IProjection>> Entry<T>(this KeyValuePair<string, Func<T, string>> @this,
		                                                                 params Expression<Func<T, object>>[] expressions)
			=> Pairs.Create(@this.Key, new Projection<T>(@this.Value, expressions).ToDelegate());
	}
}