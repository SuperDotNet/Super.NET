using System;

namespace Super.Model.Instances
{
	public class DeferredInstance<T> : IInstance<T>
	{
		readonly Lazy<T> _source;

		public DeferredInstance(IInstance<T> instance) : this(instance.Get) {}

		public DeferredInstance(Func<T> source) : this(new Lazy<T>(source)) {}

		public DeferredInstance(Lazy<T> source) => _source = source;

		public T Get() => _source.Value;
	}
}