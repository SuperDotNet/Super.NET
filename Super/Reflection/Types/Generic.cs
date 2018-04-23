using Super.Model.Selection;
using System;
using System.Reflection;

namespace Super.Reflection.Types
{
	public class Generic<T> : GenericAdapterBase<Func<T>>, IGeneric<T>
	{
		public Generic(Type definition) : this(definition, Implementations<T>.Activator) {}

		public Generic(Type definition, ISelect<TypeInfo, Func<T>> select) : base(definition, select) {}
	}

	public class Generic<T1, T> : GenericAdapterBase<Func<T1, T>>, IGeneric<T1, T>
	{
		public Generic(Type definition) : this(definition, Implementations<T1, T>.Activator) {}

		public Generic(Type definition, ISelect<Type, Func<T1, T>> select) : base(definition, select) {}
	}

	public class Generic<T1, T2, T> : GenericAdapterBase<Func<T1, T2, T>>, IGeneric<T1, T2, T>
	{
		public Generic(Type definition) : this(definition, Implementations<T1, T2, T>.Activator) {}

		public Generic(Type definition, ISelect<TypeInfo, Func<T1, T2, T>> select) : base(definition, select) {}
	}

	public class Generic<T1, T2, T3, T> : GenericAdapterBase<Func<T1, T2, T3, T>>, IGeneric<T1, T2, T3, T>
	{
		public Generic(Type definition) : this(definition, Implementations<T1, T2, T3, T>.Activator) {}

		public Generic(Type definition, ISelect<TypeInfo, Func<T1, T2, T3, T>> select) : base(definition, select) {}
	}

	public class Generic<T1, T2, T3, T4, T> : GenericAdapterBase<Func<T1, T2, T3, T4, T>>, IGeneric<T1, T2, T3, T4, T>
	{
		public Generic(Type definition) : this(definition, Implementations<T1, T2, T3, T4, T>.Activator) {}

		public Generic(Type definition, ISelect<TypeInfo, Func<T1, T2, T3, T4, T>> select) : base(definition, select) {}
	}
}