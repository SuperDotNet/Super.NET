using System;

namespace Super.Model.Sources
{
	public class DeferredSingleton<T> : ISource<T>
	{
		readonly Lazy<T> _source;

		public DeferredSingleton(ISource<T> source) : this(source.Get) {}

		public DeferredSingleton(Func<T> source) : this(new Lazy<T>(source)) {}

		public DeferredSingleton(Lazy<T> source) => _source = source;

		public T Get() => _source.Value;

		public static implicit operator T(DeferredSingleton<T> source) => source.Get();
	}
}