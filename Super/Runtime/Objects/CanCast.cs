using Super.Model.Specifications;
using Super.Reflection.Types;

namespace Super.Runtime.Objects
{
	public sealed class CanCast<TFrom, TTo> : AllSpecification<TFrom>
	{
		public static CanCast<TFrom, TTo> Default { get; } = new CanCast<TFrom, TTo>();

		CanCast() : base(Start.When<TFrom>().Assigned(), Start.From<TFrom>().Metadata().Out(IsAssignableFrom<TTo>.Default)) {}
	}
}