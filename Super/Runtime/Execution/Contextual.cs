using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Reflection;
using System;

namespace Super.Runtime.Execution
{
	public class Contextual<T> : DelegatedSelection<object, T>
	{
		public Contextual(Func<T> source) : this(source.ToSource()) {}

		public Contextual(ISource<T> source) : this(source.Any()
		                                                  .As(I<IDisposable>.Default,
		                                                      x => x.Configure(Implementations.Assign))
		                                                  .ToStore()) {}

		public Contextual(ISelect<object, T> source)
			: base(source, ExecutionContext.Default) {}
	}

	static class Implementations
	{
		public static IAssignable<object, IDisposable> Assign { get; } = AssociatedResources.Default.ToAssignment();
	}
}