using Super.ExtensionMethods;
using Super.Model.Containers;
using Super.Model.Sources;
using System;
using System.Reflection;

namespace Super.Reflection
{
	sealed class MethodDelegates : ISource<MethodInfo, Delegate>
	{
		readonly Type _type;

		public MethodDelegates(Type type) => _type = type;

		public Delegate Get(MethodInfo parameter) => parameter.CreateDelegate(_type);
	}

	sealed class MethodDelegates<T> : DecoratedSource<MethodInfo, T>
	{
		public static MethodDelegates<T> Default { get; } = new MethodDelegates<T>();

		MethodDelegates() : base(new MethodDelegates(Types<T>.Identity).Out(Cast<T>.Default)) {}
	}
}