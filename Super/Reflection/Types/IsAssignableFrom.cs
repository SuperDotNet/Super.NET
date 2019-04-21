using Super.Runtime.Activation;
using System;
using System.Reflection;
using Super.Model.Selection.Conditions;

namespace Super.Reflection.Types
{
	public sealed class IsAssignableFrom<T> : DecoratedCondition<TypeInfo>
	{
		public static IsAssignableFrom<T> Default { get; } = new IsAssignableFrom<T>();

		IsAssignableFrom() : base(new IsAssignableFrom(Type<T>.Metadata)) {}
	}

	public sealed class IsAssignableFrom : DelegatedCondition<Type>, IActivateUsing<Type>
	{
		public IsAssignableFrom(Type type) : base(x => type.IsAssignableFrom(x)) {}
	}
}