using Super.Model.Commands;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Reflection.Selection;
using Super.Reflection.Types;
using Super.Runtime.Activation;
using Super.Runtime.Invocation;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive;

namespace Super.Runtime.Environment
{
	public interface ISystemTypes : ISource<ImmutableArray<Type>> {}

	sealed class SystemTypes : SystemAssignment<ImmutableArray<Type>>, ISystemTypes
	{
		public static SystemTypes Default { get; } = new SystemTypes();

		SystemTypes() : base(ComponentAssemblies.Default
		                                        .SelectMany(I<PublicAssemblyTypes>.Default.From)
		                                        .Where(CanActivate.Default.IsSatisfiedBy)
		                                        .ToImmutableArray) {}
	}

	sealed class Stores<TType, T> : Variable<Func<IMutable<T>>>
	{
		public static Stores<TType, T> Default { get; } = new Stores<TType, T>();

		Stores() : base(Activator<TType>.Default.Select(x => x.Cast(I<IMutable<T>>.Default)).Get) {}
	}

	public sealed class StoreType : Variable<Type>
	{
		public static StoreType Default { get; } = new StoreType();

		StoreType() : base(typeof(Variable<>)) {}
	}

	public sealed class SystemStores<T> : DecoratedSource<IMutable<T>>, ICommand<Func<IMutable<T>>>
	{
		readonly static Generic<IMutable<Func<IMutable<T>>>> Generic
			= new Generic<IMutable<Func<IMutable<T>>>>(typeof(Stores<,>));

		public static SystemStores<T> Default { get; } = new SystemStores<T>();

		SystemStores() : this(new GenericTypeAlteration(StoreType.Default.Get()).Get(Type<T>.Instance), Type<T>.Instance) {}

		public SystemStores(params Type[] types) : this(types.ToImmutableArray()) {}

		public SystemStores(ImmutableArray<Type> types) : this(Generic.Invoke().Get(types)) {}

		readonly IMutable<Func<IMutable<T>>> _store;

		public SystemStores(IMutable<Func<IMutable<T>>> store) : base(store.Select(x => x.Invoke()))
			=> _store = store;

		public void Execute(Func<IMutable<T>> parameter)
		{
			_store.Execute(parameter);
		}
	}

	class Component<T> : SystemAssignment<T>
	{
		public Component(T fallback) : this(fallback.ToSource()) {}

		public Component(Func<T> fallback) : this(fallback.ToSource()) {}

		public Component(ISource<T> fallback) : base(ComponentLocator<T>.Default.Or(fallback).ToDelegate()) {}
	}

	public class SystemAssignment<T> : Deferred<T>, IAssignment<T>
	{
		readonly IAssignment<T> _store;

		protected SystemAssignment(Func<T> source) : this(source.ToSource()) {}

		protected SystemAssignment(ISource<T> source) : this(source, new Assignment<T>(SystemStores<T>.Default.Get())) {}

		protected SystemAssignment(ISource<T> source, IAssignment<T> store) : base(source, store) => _store = store;

		public void Execute(T parameter)
		{
			_store.Execute(parameter);
		}

		public bool IsSatisfiedBy(Unit parameter) => _store.IsSatisfiedBy(parameter);
	}
}