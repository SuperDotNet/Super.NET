using Super.Model.Selection;
using System;

namespace Super.Model.Results
{
	sealed class DelegateSelector<T> : ISelect<IResult<T>, Func<T>>
	{
		public static ISelect<IResult<T>, Func<T>> Default { get; } = new DelegateSelector<T>();

		DelegateSelector() {}

		public Func<T> Get(IResult<T> parameter) => parameter.Get;
	}
}
