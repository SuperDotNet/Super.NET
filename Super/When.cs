using Super.Model.Specifications;
using Super.Runtime;
using System;

namespace Super
{
	/*public sealed class WhenReference<T> : When<T> where T : class
	{
		public static WhenReference<T> Default { get; } = new WhenReference<T>();

		WhenReference() : base(IsAssigned.Default) {}
	}

	public sealed class WhenValue<T> : When<T> where T : struct
	{
		public static WhenValue<T> Default { get; } = new WhenValue<T>();

		WhenValue() : base(IsAssigned<T>.Default) {}
	}*/

	public sealed class When<T>
	{
		public static When<T> Default { get; } = new When<T>();

		When() {}

		/*readonly ISpecification<T> _assigned;

		public When(ISpecification<T> assigned) => _assigned = assigned;

		public ISpecification<T> Assigned() => _assigned;*/

		public ISpecification<T> Assigned() => IsAssigned<T>.Default;

		public ISpecification<T> Is(Func<T, bool> specification) => new DelegatedSpecification<T>(specification);
	}
}