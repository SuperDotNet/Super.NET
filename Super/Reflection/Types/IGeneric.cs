using Super.Model.Selection;
using Super.Model.Sequences;
using System;

namespace Super.Reflection.Types
{
	public interface IGeneric<out T> : ISelect<Array<Type>, T> where T : Delegate {}

	/*public interface IGeneric<in T1, out T> : ISelect<(Type T1, Type T), Func<T1, T>> {}

	public interface IGeneric<in T1, in T2, out T> : ISelect<(Type T1, Type T2, Type T), Func<T1, T2, T>> {}

	public interface IGeneric<in T1, in T2, in T3, out T> : ISelect<(Type T1, Type T2, Type T3, Type T), Func<T1, T2, T3, T>> {}

	public interface IGeneric<in T1, in T2, in T3, in T4, out T> : ISelect<(Type T1, Type T2, Type T3, Type T4, Type T), Func<T1, T2, T3, T4, T>> {}*/
}