using System;
using Super.Compose;
using Super.Model.Selection.Conditions;
using Super.Runtime.Activation;

namespace Super.Reflection.Types
{
	public sealed class IsAssignableFrom<T> : Condition<Type>
	{
		public static IsAssignableFrom<T> Default { get; } = new IsAssignableFrom<T>();

		IsAssignableFrom() : base(new IsAssignableFrom(A.Metadata<T>())) {}
	}

	public sealed class IsAssignableFrom : Condition<Type>, IActivateUsing<Type>
	{
		public IsAssignableFrom(Type type) : base(type.IsAssignableFrom) {}
	}
}