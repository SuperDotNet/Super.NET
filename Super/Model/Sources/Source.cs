using Super.ExtensionMethods;
using Super.Model.Selection;

namespace Super.Model.Sources
{
	public class Source<T> : ISource<T>
	{
		public static implicit operator T(Source<T> source) => source.Get();

		readonly T _instance;

		public Source(T instance) => _instance = instance;

		public T Get() => _instance;
	}

	public class Source<TParameter, TResult> : DecoratedSource<TResult>
	{
		public Source(ISelect<TParameter, TResult> @select, TParameter parameter) :
			base(@select.Select(parameter).Singleton()) {}
	}
}