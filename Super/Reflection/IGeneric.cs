﻿using Super.Model.Sources;
using System;
using System.Collections.Immutable;

namespace Super.Reflection
{
	public interface IGeneric<out T> : ISource<ImmutableArray<Type>, Func<T>> {}

	public interface IGeneric<in T1, out T> : ISource<ImmutableArray<Type>, Func<T1, T>> {}

	public interface IGeneric<in T1, in T2, out T> : ISource<ImmutableArray<Type>, Func<T1, T2, T>> {}

	public interface IGeneric<in T1, in T2, in T3, out T> : ISource<ImmutableArray<Type>, Func<T1, T2, T3, T>> {}

	public interface IGeneric<in T1, in T2, in T3, in T4, out T>
		: ISource<ImmutableArray<Type>, Func<T1, T2, T3, T4, T>> {}
}