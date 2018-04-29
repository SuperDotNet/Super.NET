using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Reflection;
using Super.Reflection.Types;
using Super.Runtime.Activation;
using System;

namespace Super.Runtime.Execution
{
	public class Contextual<T> : DelegatedSelection<object, T>, IActivateMarker<ISource<T>>, IActivateMarker<Func<T>>
	{
		public Contextual(Func<T> source) : this(source.Out()) {}

		public Contextual(ISource<T> source) : this(source.Out(I<object>.Default)
		                                                  .To(x => x.Cast(I<IDisposable>.Default)
		                                                            .Configure(Implementations.Assign)
		                                                            .Cast(I<T>.Default)
		                                                            .Unless(IsType<T, IDisposable>.Default.Inverse(), x))
		                                                  .ToStore()) {}

		public Contextual(ISelect<object, T> source) : base(source, ExecutionContext.Default) {}
	}

	/*sealed class ContextualResource<T> : ISelect<object, T>, IActivateMarker<ISource<T>>
	{
		readonly ISource<T>                       _source;
		readonly IAssignable<object, IDisposable> _assignable;

		public ContextualResource(ISource<T> source) : this(source, Implementations.Assign) {}

		public ContextualResource(ISource<T> source, IAssignable<object, IDisposable> assignable)
		{
			_source     = source;
			_assignable = assignable;
		}

		public T Get(object parameter)
		{
			var result = _source.Get();
			if (result is IDisposable disposable)
			{
				_assignable.Assign(parameter, disposable);
			}

			return result;
		}
	}*/

	static class Implementations
	{
		public static IAssignable<object, IDisposable> Assign { get; } = AssociatedResources.Default.ToAssignment();
	}
}