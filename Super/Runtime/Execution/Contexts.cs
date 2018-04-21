using JetBrains.Annotations;
using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Selection.Stores;
using Super.Runtime.Activation;
using System;
using System.Collections.Generic;
using System.Reactive;

namespace Super.Runtime.Execution
{
	interface IContext : /*IMembership<IDisposable>,*/ IDisposable
	{
		IContext Parent { get; }

		/*ContextDetails Details { get; }*/
	}

	sealed class DisposeContext : ValidatedCommand<IContext>
	{
		public DisposeContext(ISpecification<IContext, IDisposable> resources)
			: base(resources, resources.Out(DisposeCommand.Default.ToConfiguration()).ToCommand()) {}
	}

	sealed class AssociatedResources : ReferenceValueTable<IContext, Disposables>
	{
		public static AssociatedResources Default { get; } = new AssociatedResources();

		AssociatedResources() : base(Activator<Disposables>.Default.Allow().ToDelegate()) {}
	}

	/*sealed class RootContext : Context
	{
		public RootContext() : base(new Disposables(), new ContextDetails("Root Execution Context"), disposable) {}
	}

	class Context : Disposables, IContext
	{
		public Context(IContext parent, ContextDetails details, IDisposable disposable)
			: this(parent, details, disposable.Dispose) {}

		public Context(IContext parent, ContextDetails details, Action callback) : base(callback)
		{
			Parent = parent;
			Details = details;
		}

		public IContext Parent { get; }
		public ContextDetails Details { get; }
	}*/



	/*sealed class RootContextValue : Ambient<object>
	{
		public RootContextValue(ISource<object> source, IMutable<object> mutable) : base(source, new LogicalResource<IDisposable>()) {}
	}*/

	/*sealed class ContextResources : DecoratedTable<object, ICommand<IDisposable>>
	{
		public static ContextResources Default { get; } = new ContextResources();

		ContextResources() : base(ReferenceValueTables<Exception, Exception>.Default.Get(x => )) {}
	}*/


	/*sealed class RootExecutionContext : ISource<IDisposable>
	{
		public static RootExecutionContext Default { get; } = new RootExecutionContext();

		RootExecutionContext() {}

		/*RootExecutionContext() : this(DomainUnload.Default.Get()) {}

		readonly IObservable<EventPattern<EventArgs>> _handler;

		public RootExecutionContext(IObservable<EventPattern<EventArgs>> handler) => _handler = handler;#1#

		public IDisposable Get()
		{
			var disposables = new Disposables();
			var result = new RootContext(disposables);
			/*_handler.Subscribe(result.Dispose);#1#
			return result;
		}
	}*/

	sealed class Contexts : IContexts
	{
		readonly Stack<object> _context;

		[UsedImplicitly]
		public Contexts() : this(new ContextDetails("Root Execution Context")) {}

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
					throw new
						InvalidOperationException("An attempt was made to dispose of the root execution context, which is not allowed.");
			}

			_context.Pop();
		}
	}
}