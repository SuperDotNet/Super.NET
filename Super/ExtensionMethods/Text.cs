using Super.Model.Selection;
using Super.Reflection;
using Super.Runtime;
using Super.Runtime.Activation;
using Super.Text;
using Super.Text.Formatting;
using System;
using System.Collections.Generic;

namespace Super.ExtensionMethods
{
	public static class Text
	{
		public static KeyValuePair<Type, Func<object, IFormattable>> Register<T>(this IFormatter<T> @this)
			=> Pairs.Create(Types<T>.Identity, @this.To(I<Formatters<T>>.Default).In(Cast<object>.Default).ToDelegate());

		public static KeyValuePair<Type, Func<object, IFormattable>> Register<T>(this INamedFormatter<T> @this)
			=> Pairs.Create(Types<T>.Identity,
			                @this.To(I<Formatters<T>>.Default).In(Cast<object>.Default).ToDelegate());
	}
}
