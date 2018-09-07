using Super.Model.Collections;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Reflection;
using Super.Reflection.Selection;
using Super.Reflection.Types;
using Super.Runtime.Activation;
using Super.Runtime.Invocation;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive;
using System.Reflection;

namespace Super.Runtime.Environment
{
	public sealed class Types : SystemStore<ReadOnlyMemory<Type>>, IArray<Type>
	{
		public static Types Default { get; } = new Types();

		Types() : this(Types<PublicAssemblyTypes>.Default) {}

		public Types(IArray<Type> instance) : base(instance) {}
	}

	public class Types<T> : DecoratedArray<Type> where T : class, IActivateMarker<Assembly>, IArray<Type>
	{
		public static Types<T> Default { get; } = new Types<T>();

		Types() : this(Assemblies.Default) {}

		public Types(ISequence<Assembly> assemblies) : base(assemblies.Select(y => y.Select(I<T>.Default.From)
		                                                                            .SelectMany(x => x.AsEnumerable()))
		                                                              .ToSequence()
		                                                              .ToArray()) {}
	}

	public sealed class StorageTypeDefinition : Variable<Type>
	{
		public static StorageTypeDefinition Default { get; } = new StorageTypeDefinition();

		StorageTypeDefinition() : base(typeof(Variable<>)) {}
	}

	sealed class SystemStores<T> : DecoratedSource<IMutable<T>>
	{
		public static SystemStores<T> Default { get; } = new SystemStores<T>();

		SystemStores() : this(StorageTypeDefinition.Default.Select(I<GenericTypeAlteration>.Default)) {}

		public SystemStores(ISource<ISelect<ImmutableArray<Type>, Type>> source)
			: base(In<Type>.Start()
			               .Sequence()
			               .Enumerate()
			               .Select(source)
			               .Activate(I<IMutable<T>>.Default)
			               .Out(Type<T>.Instance)) {}
	}

	/*public interface IInitialize : ICommand<ImmutableArray<Type>> {}

	class Initialize : IInitialize
	{
		public void Execute(ImmutableArray<Type> parameter) {}
	}*/

	public class Component<T> : SystemStore<T>
	{
		protected Component(T @default) : this(@default.ToSource()) {}

		protected Component(Func<T> @default) : this(@default.Out()) {}

		protected Component(ISource<T> @default) : base(@default.Unless(ComponentLocator<T>.Default).Get) {}
	}

	static class Implementations<T>
	{
		public static ISource<IStore<T>> Store { get; } = SystemStores<T>.Default.Select(I<Store<T>>.Default);
	}

	public class SystemStore<T> : Deferred<T>, IStore<T>
	{
		readonly IStore<T> _store;

		protected SystemStore(T instance) : this(instance.ToSource()) {}

		protected SystemStore(Func<T> source) : this(source.Out()) {}

		protected SystemStore(ISource<T> source) : this(source, Implementations<T>.Store.Get()) {}

		protected SystemStore(ISource<T> source, IStore<T> store) : base(source, store) => _store = store;

		public void Execute(T parameter)
		{
			_store.Execute(parameter);
		}

		public bool IsSatisfiedBy(Unit parameter) => _store.IsSatisfiedBy(parameter);
	}
}