using System.Threading;
using Super.Model.Sources;

namespace Super.Runtime.Execution {
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