using System;
using Super.Model.Instances;
using Super.Reflection;

namespace Super.Expressions
{
	public sealed class ReturnType<T> : Instance<Type>
	{
		public static ReturnType<T> Default { get; } = new ReturnType<T>();

		ReturnType() : base(Types<T>.Key.GetDeclaredMethod(nameof(Func<object>.Invoke)).ReturnType) {}
	}
}