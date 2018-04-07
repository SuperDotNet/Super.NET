using Super.ExtensionMethods;
using Super.Model.Commands;
using Super.Model.Instances;
using Super.Runtime.Activation;
using System;
using System.Threading;

namespace Super.Runtime
{
	public class Ambient<T> : DecoratedInstance<T>, IActivateMarker<IInstance<T>>, IDisposable
	{
		readonly Action _dispose;

		public Ambient(Func<T> factory) : this(new DelegatedInstance<T>(factory)) {}

		public Ambient(IInstance<T> source) : this(source, new Logical<T>()) {}

		public Ambient(IInstance<T> source, IMutable<T> mutable) : this(source, mutable, mutable) {}

		public Ambient(IInstance<T> source, IInstance<T> instance, ICommand<T> assign)
			: this(instance.Or(source.Select(assign.ToConfiguration())), assign.Select(default).Execute) {}

		public Ambient(IInstance<T> instance, Action dispose) : base(instance) => _dispose = dispose;

		public void Dispose()
		{
			_dispose();
		}
	}

	public class Logical<T> : IMutable<T>
	{
		readonly AsyncLocal<T> _local;

		public Logical() : this(default(T)) {}

		public Logical(T instance) : this(new AsyncLocal<T> {Value = instance}) {}

		public Logical(AsyncLocal<T> local) => _local = local;

		public T Get() => _local.Value;

		public void Execute(T parameter)
		{
			_local.Value = parameter;
		}
	}
}