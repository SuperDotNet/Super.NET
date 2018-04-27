using Super.Model.Selection;
using Super.Reflection.Types;
using Super.Runtime.Objects;
using System;
using System.Reflection;

namespace Super.Reflection.Members
{
	sealed class MethodDelegates : ISelect<MethodInfo, Delegate>
	{
		readonly Type _type;

		public MethodDelegates(Type type) => _type = type;

		public Delegate Get(MethodInfo parameter) => parameter.CreateDelegate(_type);
	}

	sealed class MethodDelegates<T> : DecoratedSelect<MethodInfo, T> where T : class
	{
		public static MethodDelegates<T> Default { get; } = new MethodDelegates<T>();

		MethodDelegates() : base(new MethodDelegates(Type<T>.Instance).Select(CastSelector<Delegate, T>.Default)) {}
	}
}