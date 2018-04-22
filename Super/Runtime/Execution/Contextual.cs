using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Sources;
using System;

namespace Super.Runtime.Execution
{
	public class Contextual<T> : DelegatedSelection<object, T>
	{
		public Contextual(Func<T> source) : this(source.ToSource()) {}

		public Contextual(ISource<T> source) : this(source.Allow().ToStore()) {}

		public Contextual(ISelect<object, T> source) : base(source, ExecutionContext.Default) {}
	}

	static class Implementations
	{
		public static IAssignable<object, IDisposable> Assign { get; } = AssociatedResources.Default.ToAssignment();
	}

	public class ContextualResource : ContextualResource<IDisposable>
	{
		public ContextualResource(Func<IDisposable> source) : base(source) {}

		public ContextualResource(ISource<IDisposable> source) : base(source) {}

		public ContextualResource(ISelect<object, IDisposable> source) : base(source) {}
	}

	public class ContextualResource<T> : Contextual<T>
	{
		public ContextualResource(Func<T> source) : this(source.ToSource()) {}

		public ContextualResource(ISource<T> source) : this(source.Allow().ToStore()) {}

		public ContextualResource(ISelect<object, T> source) : base(source.Out(Cast<IDisposable>.Default)
		                                                                  .Configure(Implementations.Assign)
		                                                                  .Out(Cast<T>.Default)) {}
	}
}