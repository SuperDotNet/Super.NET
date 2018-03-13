using System;

namespace Super.Model.Instances
{
	public class Unwrap<T> : IInstance<T>
	{
		readonly Func<Func<T>> _delegate;

		public Unwrap(IInstance<Func<T>> instance) : this(ExtensionMethods.Sources.ToDelegate(instance)) {}

		public Unwrap(Func<Func<T>> @delegate) => _delegate = @delegate;

		public T Get() => _delegate()();
	}
}