using Super.Model.Selection;
using Super.Model.Selection.Conditions;
using Super.Model.Sequences;
using Super.Runtime;
using System;
using System.Linq;

namespace Super.Reflection.Types
{
	public class GenericImplementationArguments : ISelect<Type, Array<Type>>
	{
		readonly ISelect<Type, Array<Type>> _implementations;

		public GenericImplementationArguments(ISelect<Type, Array<Type>> implementations)
			=> _implementations = implementations; 

		public Array<Type> Get(Type parameter) => _implementations.Get(parameter)
		                                                          .Open()
		                                                          .SelectMany(x => x.GenericTypeArguments)
		                                                          .ToArray();
	}

	public class GenericImplementations : ISelect<Type, Array<Type>>
	{
		readonly ISelect<Type, IConditional<Type, Array<Type>>> _implementations;
		readonly Type                                               _definition;

		public GenericImplementations(Type definition) : this(GenericInterfaceImplementations.Default,
		                                                      definition) {}

		public GenericImplementations(ISelect<Type, IConditional<Type, Array<Type>>> implementations,
		                              Type definition)
		{
			_implementations = implementations;
			_definition      = definition;
		}

		public Array<Type> Get(Type parameter) => _implementations.Get(parameter)
		                                                          .Get(_definition)
		                                                          .Open();
	}

	public class OpenGeneric : Select<Type, Func<Array<Type>, Func<object>>>
	{
		public OpenGeneric(Type definition)
			: base(new ContainsGenericInterfaceGuard(definition).Then()
			                                                    .ToConfiguration()
			                                                    .Select(x => new Generic<object>(x).ToDelegate())) {}
	}

	public class Generic<T> : Select<Array<Type>, Func<T>>, IGeneric<T>
	{
		public Generic(Type definition) : base(new MakeGenericType(definition).Select(Delegates.Default.Get)) {}

		sealed class Delegates : ActivationDelegates<Func<T>>
		{
			public static Delegates Default { get; } = new Delegates();

			Delegates() : base(GenericSingleton.Default) {}
		}
	}

	public class Generic<T1, T> : Select<Array<Type>, Func<T1, T>>, IGeneric<T1, T>
	{
		public Generic(Type definition) : base(new MakeGenericType(definition).Select(Delegates.Default.Get)) {}

		sealed class Delegates : ActivationDelegates<Func<T1, T>>
		{
			public static Delegates Default { get; } = new Delegates();

			Delegates() : base(typeof(T1)) {}
		}
	}

	public class Generic<T1, T2, T> : Select<Array<Type>, Func<T1, T2, T>>, IGeneric<T1, T2, T>
	{
		public Generic(Type definition) : base(new MakeGenericType(definition).Select(Delegates.Default.Get)) {}

		sealed class Delegates : ActivationDelegates<Func<T1, T2, T>>
		{
			public static Delegates Default { get; } = new Delegates();

			Delegates() : base(typeof(T1), typeof(T2)) {}
		}
	}

	public class Generic<T1, T2, T3, T> : Select<Array<Type>, Func<T1, T2, T3, T>>, IGeneric<T1, T2, T3, T>
	{
		public Generic(Type definition) : base(new MakeGenericType(definition).Select(Delegates.Default.Get)) {}

		sealed class Delegates : ActivationDelegates<Func<T1, T2, T3, T>>
		{
			public static Delegates Default { get; } = new Delegates();

			Delegates() : base(typeof(T1), typeof(T2), typeof(T3)) {}
		}
	}

	public class Generic<T1, T2, T3, T4, T> : Select<Array<Type>, Func<T1, T2, T3, T4, T>>, IGeneric<T1, T2, T3, T4, T>
	{
		public Generic(Type definition) : base(new MakeGenericType(definition).Select(Delegates.Default.Get)) {}

		sealed class Delegates : ActivationDelegates<Func<T1, T2, T3, T4, T>>
		{
			public static Delegates Default { get; } = new Delegates();

			Delegates() : base(typeof(T1), typeof(T2), typeof(T3)) {}
		}
	}
}