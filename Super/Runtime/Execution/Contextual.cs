using Super.Compose;
using Super.Model.Commands;
using Super.Model.Results;
using Super.Model.Selection;
using Super.Reflection.Types;
using System;

namespace Super.Runtime.Execution
{
	public class Contextual<T> : Result<T>
	{
		readonly static bool Attach = IsAssignableFrom<IDisposable>.Default.Get(A.Metadata<T>());

		public Contextual(Func<T> source) : this(Start.A.Selection.Of.Any.By.Calling(source), Attach) {}

		Contextual(ISelect<object, T> select, bool attach)
			: base((attach ? select.SelectOf(Implementations.Resources) : select).Stores()
			                                                                     .Reference()
			                                                                     .In(ExecutionContext.Default)) {}
	}

	static class Implementations
	{
		public static IAssign<object, IDisposable> Resources { get; } = AssociatedResources.Default.ToAssignment();
	}
}