using Super.Compose;
using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Selection.Conditions;
using Super.Model.Sequences;
using Super.Reflection;
using Super.Reflection.Selection;
using Super.Reflection.Types;
using Super.Runtime.Activation;
using Super.Runtime.Invocation;
using System;
using System.Linq;
using System.Reflection;
using Activator = Super.Runtime.Activation.Activator;

namespace Super.Runtime.Environment
{
	public sealed class Types : SystemStore<Array<Type>>, IArray<Type>
	{
		public static Types Default { get; } = new Types();

		Types() : this(Types<PublicAssemblyTypes>.Default) {}

		public Types(IArray<Type> instance) : base(instance) {}
	}

	public class Types<T> : ArrayStore<Type> where T : class, IActivateUsing<Assembly>, IArray<Type>
	{
		public static Types<T> Default { get; } = new Types<T>();

		Types() : this(Assemblies.Default.ToSelect()
		                         .Select(y => y.Open()
		                                       .Select(I<T>.Default.From)
		                                       .SelectMany(x => x.Get().Open()))
		                         .Result()) {}

		public Types(ISelect<None, Array<Type>> source) : base(source) {}
	}

	public sealed class StorageTypeDefinition : Variable<Type>
	{
		public static StorageTypeDefinition Default { get; } = new StorageTypeDefinition();

		StorageTypeDefinition() : base(typeof(Variable<>)) {}
	}

	sealed class SystemStores<T> : Model.Results.Result<IMutable<T>>
	{
		public static SystemStores<T> Default { get; } = new SystemStores<T>();

		SystemStores() : this(Start.A.Selection<Type>()
		                           .By.StoredActivation<MakeGenericType>()
		                           .In(StorageTypeDefinition.Default)
		                           .Emit()) {}

		public SystemStores(ISelect<Array<Type>, Type> source) : base(Start.A.Selection.Of.System.Type.By.Array()
		                                                                   .Select(source)
		                                                                   .Select(Activator.Default)
		                                                                   .In(Type<T>.Instance)
		                                                                   .Then()
		                                                                   .Cast<IMutable<T>>()
		                                                                   .Out()) {}
	}

	sealed class DefaultComponent<T> : Component<T>
	{
		public static DefaultComponent<T> Default { get; } = new DefaultComponent<T>();

		DefaultComponent() : this(default(T)) {}

		public DefaultComponent(T @default = default) : base(@default) {}

		public DefaultComponent(Func<T> @default) : base(@default) {}
	}

	public class Component<T> : SystemStore<T>
	{
		protected Component(T @default) : this(@default.Start()) {}

		protected Component(Func<T> @default) : this(@default.Start()) {}

		protected Component(IResult<T> @default) : base(@default.Unless(Make.A<ComponentLocator<T>>()).Get) {}
	}

	static class Implementations<T>
	{
		public static IResult<IStore<T>> Store { get; }
			= Start.A.Selection<IMutable<T>>()
			       .By.StoredActivation<Model.Results.Store<T>>()
			       .In(SystemStores<T>.Default);
	}

	public class SystemStore<T> : Deferred<T>, IStore<T>
	{
		readonly IStore<T> _store;

		protected SystemStore(T instance) : this(instance.Start()) {}

		protected SystemStore(Func<T> source) : this(source.Start()) {}

		protected SystemStore(IResult<T> result) : this(result, Implementations<T>.Store.Get()) {}

		protected SystemStore(IResult<T> result, IStore<T> store)
			: this(store.Condition, result, store) {}

		protected SystemStore(ICondition<None> condition, IResult<T> result, IStore<T> store)
			: base(result.ToSelect(), store)
		{
			Condition = condition;
			_store    = store;
		}

		public void Execute(T parameter)
		{
			_store.Execute(parameter);
		}

		public ICondition<None> Condition { get; }
	}
}