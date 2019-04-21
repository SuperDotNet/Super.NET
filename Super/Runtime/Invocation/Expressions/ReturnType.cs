using Super.Model.Results;
using Super.Reflection.Types;
using System;

namespace Super.Runtime.Invocation.Expressions
{
	public sealed class ReturnType<T> : Instance<Type>
	{
		public static ReturnType<T> Default { get; } = new ReturnType<T>();

		ReturnType() : base(Type<T>.Metadata.GetDeclaredMethod(nameof(Func<object>.Invoke)).ReturnType) {}
	}
}