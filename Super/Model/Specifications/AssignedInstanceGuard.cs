using Super.Model.Selection;
using Super.Runtime;
using System;

namespace Super.Model.Specifications
{
	sealed class AssignedInstanceGuard<T> : GuardedSpecification<T, InvalidOperationException>
	{
		public AssignedInstanceGuard(ISelect<T, string> message) : this(IsModified<T>.Default, message) {}

		public AssignedInstanceGuard(ISpecification<T> specification, ISelect<T, string> message)
			: this(specification,
			       Exception<InvalidOperationException>.Default.In(message).Get) {}

		public AssignedInstanceGuard(ISpecification<T> specification, Func<T, InvalidOperationException> exception)
			: base(specification, exception) {}
	}
}