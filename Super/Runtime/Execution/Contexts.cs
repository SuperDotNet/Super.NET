using Super.Model.Collections;
using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Selection.Stores;
using Super.Model.Specifications;
using System;
using System.Reactive;

namespace Super.Runtime.Execution
{
	interface IContext : IMembership<IDisposable>, IDisposable
	{
		IContext Parent { get; }

		/*ContextDetails Details { get; }*/
	}

	sealed class DisposeContext : DecoratedCommand<Unit>
	{
		public static DisposeContext Default { get; } = new DisposeContext();

		DisposeContext() : this(AssignedContext.Default.AsSpecification().Any(), AssociatedResources.Default) {}

		public DisposeContext(ISpecification<object> assigned, ISpecification<object, IDisposable> resources)
			: this(assigned, resources, resources) {}

		public DisposeContext(ISpecification<object> assigned, ISpecification<object> resources,
		                      ISelect<object, IDisposable> select)
			: base(ExecutionContext.Default
			                       .Enter(select.Enter(DisposeCommand.Default)
			                                    .And(AssignedContext.Default.Clear(), ClearResources.Default)
			                                    .Select()
			                                    .If(resources.And(assigned))
			                                    .ToCommand())) {}
	}

	sealed class ClearResources : RemoveCommand<object, Disposables>
	{
		public static ClearResources Default { get; } = new ClearResources();

		ClearResources() : base(AssociatedResources.Default) {}
	}

	sealed class AssociatedResources : AssociatedResource<object, Disposables>
	{
		public static AssociatedResources Default { get; } = new AssociatedResources();

		AssociatedResources() {}
	}
}