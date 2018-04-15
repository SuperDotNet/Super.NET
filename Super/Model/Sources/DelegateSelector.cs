using Super.ExtensionMethods;
using Super.Model.Selection;
using System;

namespace Super.Model.Sources
{
	sealed class DelegateSelector<T> : ISelect<ISource<T>, Func<T>>
	{
		public static ISelect<ISource<T>, Func<T>> Default { get; } = new DelegateSelector<T>();

		DelegateSelector() {}

		public Func<T> Get(ISource<T> parameter) => parameter.ToDelegate();
	}
}
