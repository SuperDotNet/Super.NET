using Super.Model.Collections;
using Super.Model.Selection;
using System;
using System.Linq;
using System.Reflection;

namespace Super.Reflection.Types
{
	public static class Implementations
	{
		public static ISelect<TypeInfo, Array<TypeInfo>> AllInterfaces { get; }
			= Interfaces.Default
			            .Select(x => x.Where(y => y.IsInterface).Distinct())
			            .Result()
			            .ToStore();

		public static ISelect<TypeInfo, Array<TypeInfo>> GenericInterfaces { get; }
			= AllInterfaces.Select(x => x.Where(y => y.IsGenericType))
			               .Result()
			               .ToStore();

		public static ISelect<TypeInfo, ISpecification<Type, Array<TypeInfo>>> GenericInterfaceImplementations { get; }
			= GenericInterfaces.Grouping(GenericTypeDefinition.Default)
			                   .Select(I<Model.Sequences.Query.Lookup<Type, TypeInfo>>.Default)
			                   .Select(x => x.Select(GenericTypeDefinition.Default
			                                                              .Unless(IsGenericTypeDefinition.Default,
			                                                                      Self<Type>.Default)))
			                   .ToStore();
	}

	static class Implementations<T>
	{
		public static ISelect<TypeInfo, Func<T>> Activator { get; }
			= new GenericActivators<Func<T>>(GenericSingleton.Default).ToReferenceStore();
	}

	static class Implementations<T1, T>
	{
		public static ISelect<Type, Func<T1, T>> Activator { get; }
			= new GenericActivators<Func<T1, T>>(typeof(T1)).ToReferenceStore();
	}

	static class Implementations<T1, T2, T>
	{
		public static ISelect<TypeInfo, Func<T1, T2, T>> Activator { get; }
			= new GenericActivators<Func<T1, T2, T>>(typeof(T1), typeof(T2)).ToStore();
	}

	static class Implementations<T1, T2, T3, T>
	{
		public static ISelect<TypeInfo, Func<T1, T2, T3, T>> Activator { get; }
			= new GenericActivators<Func<T1, T2, T3, T>>(typeof(T1), typeof(T2), typeof(T3)).ToStore();
	}

	static class Implementations<T1, T2, T3, T4, T>
	{
		public static ISelect<TypeInfo, Func<T1, T2, T3, T4, T>> Activator { get; }
			= new GenericActivators<Func<T1, T2, T3, T4, T>>(typeof(T1), typeof(T2), typeof(T3), typeof(T4)).ToStore();
	}
}