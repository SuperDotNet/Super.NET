using System;

namespace Super.Model.Instances
{
	public class DeferredSingleton<T> : IInstance<T>
	{
		readonly Lazy<T> _source;

		public DeferredSingleton(IInstance<T> instance) : this(instance.Get) {}

		public DeferredSingleton(Func<T> source) : this(new Lazy<T>(source)) {}

		public DeferredSingleton(Lazy<T> source) => _source = source;

		public T Get() => _source.Value;
	}
}