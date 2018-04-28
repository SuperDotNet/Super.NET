using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Reflection;
using Super.Reflection.Types;
using System;

namespace Super.Runtime.Execution
{
	public class Contextual<T> : DelegatedSelection<object, T>
	{
		public Contextual(Func<T> source) : this(source.ToSource()) {}

		public Contextual(ISource<T> source) : this(source.Out(I<object>.Default)
		                                                  .To(x => x.Unless(IsType<T, IDisposable>.Default,
		                                                                    x.Cast(I<IDisposable>.Default)
		                                                                     .Configure(Implementations.Assign)
		                                                                     .Cast(I<T>.Default)))
		                                                  .ToStore()) {}

		public Contextual(ISelect<object, T> source) : base(source, ExecutionContext.Default) {}
	}

	static class Implementations
	{
		public static IAssignable<object, IDisposable> Assign { get; } = AssociatedResources.Default.ToAssignment();
	}
}