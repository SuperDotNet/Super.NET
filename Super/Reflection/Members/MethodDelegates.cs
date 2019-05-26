using System;
using System.Reflection;
using Super.Model.Selection;
using Super.Reflection.Types;

namespace Super.Reflection.Members
{
	sealed class MethodDelegates<T> : Select<MethodInfo, T> where T : Delegate
	{
		public static MethodDelegates<T> Default { get; } = new MethodDelegates<T>();

		MethodDelegates() : base(x => (T)x.CreateDelegate(Type<T>.Instance)) {}
	}
}