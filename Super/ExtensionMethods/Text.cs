using Super.Model.Selection;
using Super.Reflection;
using Super.Runtime;
using Super.Runtime.Activation;
using Super.Runtime.Objects;
using Super.Text.Formatting;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Super.ExtensionMethods
{
	public static class Text
	{
		public static ISelect<object, Func<object, IFormattable>> Register<T>(this ISelectFormatter<T> @this)
			=> @this.To(I<Formatters<T>>.Default)
			        .In(Cast<object>.Default)
			        .ToDelegate()
			        .OrDefault(IsTypeSpecification<T>.Default);

		public static ISelect<string, TParameter, TResult> AsDefault<TParameter, TResult>(this ISelect<TParameter, TResult> @this)
			=> @this.ToDelegate().Allow(I<string>.Default);

		public static KeyValuePair<string, Func<T, Projection>> Project<T>(this IFormat<T> @this,
		                                                                   params Expression<Func<T, object>>[] expressions)
			=> @this.Get().Project(expressions);

		public static KeyValuePair<string, Func<T, Projection>> Project<T>(this KeyValuePair<string, Func<T, string>> @this,
		                                                                   params Expression<Func<T, object>>[] expressions)
			=> Pairs.Create(@this.Key, new Projection<T>(@this.Value, expressions).ToDelegate());
	}
}