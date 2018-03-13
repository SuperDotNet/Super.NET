using System;

namespace Super.Model.Instances
{
	public class DelegatedInstance<T> : IInstance<T>
	{
		readonly Func<T> _source;

		public DelegatedInstance(Func<T> source) => _source = source;

		public T Get() => _source();
	}
}