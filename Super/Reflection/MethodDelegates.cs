using Super.ExtensionMethods;
using System;
using System.Reflection;
using Super.Model.Selection;

namespace Super.Reflection
{
	sealed class MethodDelegates : ISelect<MethodInfo, Delegate>
	{
		readonly Type _type;

		public MethodDelegates(Type type) => _type = type;

		public Delegate Get(MethodInfo parameter) => parameter.CreateDelegate(_type);
	}

	sealed class MethodDelegates<T> : Decorated<MethodInfo, T>
	{
		public static MethodDelegates<T> Default { get; } = new MethodDelegates<T>();

		MethodDelegates() : base(new MethodDelegates(Type<T>.Instance).Out(Cast<T>.Default)) {}
	}
}