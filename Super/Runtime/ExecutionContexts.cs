using JetBrains.Annotations;
using Super.ExtensionMethods;
using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Reflection;
using Super.Runtime.Activation;
using System;
using System.Collections.Generic;
using System.Reactive;

namespace Super.Runtime
{
	public interface IContexts : ISource<object>, ICommand<object>, ICommand<Unit> {}

	static class Implementations
	{
		public static ISource<IContexts> Contexts { get; } = Ambient.For<Contexts>();
	}

	sealed class Contexts : IContexts
	{
		readonly Stack<object> _context;

		[UsedImplicitly]
		public Contexts() : this(new Context("Root Execution Context")) {}

		public Contexts(object root) : this(new Stack<object>(root.Yield())) {}

		public Contexts(Stack<object> context) => _context = context;

		public object Get() => _context.Peek();

		public void Execute(object parameter)
		{
			_context.Push(parameter);
		}

		public void Execute(Unit parameter)
		{
			switch (_context.Count)
			{
				case 1:
					throw new InvalidOperationException("An attempt was made to dispose of the root execution context, which is not allowed.");
			}
			_context.Pop();
		}
	}

	public interface IChildExecutionContext : ISelect<string, IDisposable> {}

	sealed class ChildExecutionContext : Decorated<string, IDisposable>, IChildExecutionContext
	{
		public static ChildExecutionContext Default { get; } = new ChildExecutionContext();

		ChildExecutionContext() : base(Implementations.Contexts.Select(I<ChildContexts>.Default).Select()) {}
	}

	sealed class ChildContexts : ISelect<string, IDisposable>, IActivateMarker<IContexts>
	{
		readonly Func<string, object> _context;
		readonly IContexts _assign;

		[UsedImplicitly]
		public ChildContexts(IContexts assign) : this(I<Context>.Default.From, assign) {}

		public ChildContexts(Func<string, object> context, IContexts assign)
		{
			_context = context;
			_assign = assign;
		}

		public IDisposable Get(string parameter)
		{
			var child = _context(parameter);
			_assign.Execute(child);
			var result = new DelegatedDisposable(_assign.Execute);
			return result;
		}
	}



	public interface IExecutionContext : ISource<object> {}

	sealed class ExecutionContext : DecoratedSource<object>, IExecutionContext
	{
		public static ExecutionContext Default { get; } = new ExecutionContext();

		ExecutionContext() : base(Implementations.Contexts.Select()) {}
	}
}