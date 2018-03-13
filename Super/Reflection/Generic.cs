using System;
using System.Collections.Immutable;
using System.Reflection;
using Super.ExtensionMethods;
using Super.Model.Sources;

namespace Super.Reflection
{
	public interface IGeneric<in T1, out T> : ISource<ImmutableArray<TypeInfo>, Func<T1, T>> {}

	public interface IGeneric<in T1, in T2, out T> : ISource<ImmutableArray<TypeInfo>, Func<T1, T2, T>> {}

	public interface IGeneric<in T1, in T2, in T3, out T> : ISource<ImmutableArray<TypeInfo>, Func<T1, T2, T3, T>> {}

	public interface IGeneric<in T1, in T2, in T3, in T4, out T>
		: ISource<ImmutableArray<TypeInfo>, Func<T1, T2, T3, T4, T>> {}

	public class Generic<T> : GenericAdapterBase<Func<T>>, IGeneric<T>
	{
		readonly static ISource<TypeInfo, Func<T>> Activator =
			new GenericActivators<Func<T>>(GenericSingleton.Default).ToReferenceStore();

		public Generic(Type definition) : this(definition, Activator) {}

		public Generic(Type definition, ISource<TypeInfo, Func<T>> source) : base(definition, source) {}
	}

	public class Generic<T1, T> : GenericAdapterBase<Func<T1, T>>, IGeneric<T1, T>
	{
		readonly static ISource<TypeInfo, Func<T1, T>> Activator =
			new GenericActivators<Func<T1, T>>(typeof(T1)).ToReferenceStore();

		public Generic(Type definition) : this(definition, Activator) {}

		public Generic(Type definition, ISource<TypeInfo, Func<T1, T>> source) : base(definition, source) {}
	}

	public class Generic<T1, T2, T> : GenericAdapterBase<Func<T1, T2, T>>, IGeneric<T1, T2, T>
	{
		readonly static ISource<TypeInfo, Func<T1, T2, T>> Activator
			= new GenericActivators<Func<T1, T2, T>>(typeof(T1), typeof(T2)).ToStore();

		public Generic(Type definition) : this(definition, Activator) {}

		public Generic(Type definition, ISource<TypeInfo, Func<T1, T2, T>> source)
			: base(definition, source) {}
	}

	public class Generic<T1, T2, T3, T> : GenericAdapterBase<Func<T1, T2, T3, T>>, IGeneric<T1, T2, T3, T>
	{
		readonly static ISource<TypeInfo, Func<T1, T2, T3, T>> Activator
			= new GenericActivators<Func<T1, T2, T3, T>>(typeof(T1), typeof(T2), typeof(T3)).ToStore();

		public Generic(Type definition) : this(definition, Activator) {}

		public Generic(Type definition, ISource<TypeInfo, Func<T1, T2, T3, T>> source)
			: base(definition, source) {}
	}

	public class Generic<T1, T2, T3, T4, T> : GenericAdapterBase<Func<T1, T2, T3, T4, T>>, IGeneric<T1, T2, T3, T4, T>
	{
		readonly static ISource<TypeInfo, Func<T1, T2, T3, T4, T>> Activator
			= new GenericActivators<Func<T1, T2, T3, T4, T>>(typeof(T1), typeof(T2), typeof(T3), typeof(T4)).ToStore();

		public Generic(Type definition) : this(definition, Activator) {}

		public Generic(Type definition, ISource<TypeInfo, Func<T1, T2, T3, T4, T>> source)
			: base(definition, source) {}
	}
}