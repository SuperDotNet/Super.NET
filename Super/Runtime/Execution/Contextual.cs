using System;
using Super.Compose;
using Super.Model.Results;
using Super.Model.Selection;
using Super.Reflection.Types;

namespace Super.Runtime.Execution
{
	public class Contextual<T> : Result<T>
	{
		readonly static bool Attach = IsAssignableFrom<IDisposable>.Default.Get(A.Metadata<T>());

		Contextual(ISelect<object, T> select, bool attach)
			: base((attach ? select.Then().Configure(Implementations.Resources).Get() : select)
			       .Stores()
			       .Reference()
			       .In(ExecutionContext.Default)) {}

		public Contextual(Func<T> source) : this(Start.A.Selection.Of.Any.By.Calling(source), Attach) {}
	}
}