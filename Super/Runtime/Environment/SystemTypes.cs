using Super.Model.Selection;
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
using Activator = Super.Runtime.Activation.Activator;

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

	public sealed class SystemStorageDefinition : Variable<Type>
	{
		public static SystemStorageDefinition Default { get; } = new SystemStorageDefinition();

		SystemStorageDefinition() : base(typeof(Variable<>)) {}
	}

	sealed class SystemStore<T> : DecoratedSource<IMutable<T>>
	{
		public static SystemStore<T> Default { get; } = new SystemStore<T>();

		public SystemStore() : this(SystemStorageDefinition.Default.Select(I<GenericTypeAlteration>.Default)) {}

		public SystemStore(ISource<ISelect<ImmutableArray<Type>, Type>> source)
			: base(Self<Type>.Default
			                 .Sequence()
			                 .Enumerate()
			                 .Select(source)
			                 .Select(Activator.Default)
			                 .Cast(I<IMutable<T>>.Default)
			                 .Out(Type<T>.Instance)) {}
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

		protected SystemAssignment(ISource<T> source) : this(source, new Assignment<T>(SystemStore<T>.Default.Get())) {}

		protected SystemAssignment(ISource<T> source, IAssignment<T> store) : base(source, store) => _store = store;

		public void Execute(T parameter)
		{
			_store.Execute(parameter);
		}

		public bool IsSatisfiedBy(Unit parameter) => _store.IsSatisfiedBy(parameter);
	}
}