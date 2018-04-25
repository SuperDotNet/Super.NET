using Super.Model.Selection;
using Super.Reflection;
using Super.Runtime;
using Super.Runtime.Activation;
using System;

namespace Super.Model.Specifications
{
	sealed class AssignedInstanceGuard<T> : GuardedSpecification<T, InvalidOperationException>, IActivateMarker<ISelect<T, string>>
	{
		public AssignedInstanceGuard(ISelect<T, string> message) : this(IsAssigned<T>.Default, message) {}

		public AssignedInstanceGuard(ISpecification<T> specification, ISelect<T, string> message)
			: this(specification, message.New(I<InvalidOperationException>.Default).Get) {}

		public AssignedInstanceGuard(ISpecification<T> specification, Func<T, InvalidOperationException> exception)
			: base(specification, exception) {}
	}
}