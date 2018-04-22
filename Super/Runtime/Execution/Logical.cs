using Super.Model.Selection;
using Super.Model.Sources;
using System.Threading;

namespace Super.Runtime.Execution
{
	/*public class LogicalResource<T> : Logical<T> where T : class, IDisposable
	{
		readonly static Action<AsyncLocalValueChangedArgs<T>> Changed = DisposeLogicalValue<T>.Default.ToDelegate();

		public LogicalResource() : this(Changed) {}

		public LogicalResource(Action<AsyncLocalValueChangedArgs<T>> changed) : base(changed) {}
	}*/

	/*sealed class HasExited<T> : AllSpecification<AsyncLocalValueChangedArgs<T>>
	{
		public static HasExited<T> Default { get; } = new HasExited<T>();

		HasExited() : base(In<AsyncLocalValueChangedArgs<T>>.Is(x => x.ThreadContextChanged),
		                   IsReference.Default
		                              .Select(x => x.Current)
		                              .Select<T>()
		                              .Select(InstanceMetadataSelector<AsyncLocalValueChangedArgs<T>>.Default).Select(x => x.Current)) {}
	}*/

	/*sealed class DisposeLogicalValue<T> : ExitLogical<T> where T : class, IDisposable
	{
		public static DisposeLogicalValue<T> Default { get; } = new DisposeLogicalValue<T>();

		DisposeLogicalValue() : base(DisposeCommand.Default.Select(PreviousValue<T>.Default)) {}
	}*/

	sealed class PreviousValue<T> : Select<AsyncLocalValueChangedArgs<T>, T>
	{
		public static PreviousValue<T> Default { get; } = new PreviousValue<T>();

		PreviousValue() : base(x => x.PreviousValue) {}
	}

	/*class ExitLogical<T> : ValidatedCommand<AsyncLocalValueChangedArgs<T>> where T : class
	{
		public ExitLogical(ICommand<AsyncLocalValueChangedArgs<T>> command) : base(HasExited<T>.Default, command) {}
	}*/

	public class Logical<T> : IMutable<T>
	{
		readonly AsyncLocal<T> _local;

		/*public Logical() : this(args => {}) {}

		public Logical(Action<AsyncLocalValueChangedArgs<T>> changed) : this(default, changed) {}

		public Logical(T instance, Action<AsyncLocalValueChangedArgs<T>> changed)
			: this(new AsyncLocal<T>(changed) {Value = instance}) {}*/

		public Logical() : this(new AsyncLocal<T>()) {}

		public Logical(AsyncLocal<T> local) => _local = local;

		public T Get() => _local.Value;

		public void Execute(T parameter)
		{
			_local.Value = parameter;
		}
	}
}