using Super.Compose;
using Super.Model.Commands;
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

		Types() : this(Assemblies.Default.Query()
		                         .Select(I<T>.Default.From)
		                         .SelectMany(x => x.Get().Open())
		                         .Selector()) {}

		public Types(Func<Array<Type>> source) : base(source) {}
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
		                           .Assume()) {}

		public SystemStores(ISelect<Array<Type>, Type> source) : base(Start.A.Selection.Of.System.Type.By.Array()
		                                                                   .Select(source)
		                                                                   .Select(Activator.Default)
		                                                                   .In(A.Type<T>())
		                                                                   .Then()
		                                                                   .Cast<IMutable<T>>()
		                                                                   .Selector()) {}
	}

	sealed class DefaultComponent<T> : Component<T>
	{
		public static DefaultComponent<T> Default { get; } = new DefaultComponent<T>();

		DefaultComponent() : base(Start.A.Result<T>().By.Default()) {}
	}

	public class Component<T> : SystemStore<T>
	{
		public Component(Func<T> @default) : this(@default.Start()) {}

		public Component(IResult<T> @default) : base(@default.Unless(ComponentLocator<T>.Default)) {}
	}

	static class Implementations<T>
	{
		public static IResult<IStore<T>> Store { get; } = Start.A.Selection<IMutable<T>>()
		                                                       .By.StoredActivation<Model.Results.Store<T>>()
		                                                       .In(SystemStores<T>.Default);
	}

	public static class Stores
	{
		public static IResult<T> New<T>(Func<T> create) => new Deferred<T>(create, New<T>());

		public static IStore<T> New<T>() => Implementations<T>.Store.Get();
	}

	public class SystemRegistry<T> : Assume<Array<T>>, IRegistry<T>
	{
		readonly Func<IRegistry<T>> _result;

		/*public SystemRegistry(IResult<Array<T>> elements) : this(Start.A.Selection<T>()
		                                                              .As.Sequence.Immutable.AndOf<Registry<T>>()
		                                                              .By.Instantiation.In(elements)
		                                                              .ToDelegate()) {}*/

		public SystemRegistry() : this(() => new Registry<T>()) {}

		public SystemRegistry(Func<IRegistry<T>> registry) : this(registry.To(Stores.New)) {}

		public SystemRegistry(IResult<IRegistry<T>> result)
			: this(result.Get, result.AsDefined().Then().Delegate().Selector()) {}

		public SystemRegistry(Func<IRegistry<T>> result, Func<Func<Array<T>>> get) : base(get)
			=> _result = result;

		public void Execute(Model.Sequences.Store<T> parameter)
		{
			_result().Execute(parameter);
		}

		public void Execute(T parameter)
		{
			_result().Execute(parameter);
		}
	}

	public interface IRegistry<T> : IArray<T>, IAddRange<T>, ICommand<T> {}

	public interface IAddRange<T> : ICommand<Model.Sequences.Store<T>> {}

	public sealed class AddRange<T> : IAddRange<T>
	{
		readonly IMutable<Array<T>> _array;

		public AddRange(IMutable<Array<T>> array) => _array = array;

		public void Execute(Model.Sequences.Store<T> parameter)
		{
			var current = _array.Get().Open();
			var length  = parameter.Length();
			var to      = (uint)current.Length;
			Array.Resize(ref current, (int)(length + to));
			parameter.Instance.CopyInto(current, 0, length, to);
			_array.Execute(current);
		}
	}

	public sealed class Add<T> : ICommand<T>
	{
		readonly IMutable<Array<T>> _array;

		public Add(IMutable<Array<T>> array) => _array = array;

		public void Execute(T parameter)
		{
			var current = _array.Get().Open();
			var to      = current.Length;
			Array.Resize(ref current, to + 1);
			current[to] = parameter;
			_array.Execute(current);
		}
	}

	public class Registry<T> : ArrayResult<T>, IRegistry<T>
	{
		readonly ICommand<Model.Sequences.Store<T>> _range;
		readonly ICommand<T>                        _add;

		public Registry() : this(Array<T>.Empty) {}

		public Registry(params T[] elements) : this(new Array<T>(elements)) {}

		public Registry(Array<T> elements) : this(new Variable<Array<T>>(elements)) {}

		public Registry(IMutable<Array<T>> source) : this(source, new AddRange<T>(source), new Add<T>(source)) {}

		public Registry(IResult<Array<T>> source, ICommand<Model.Sequences.Store<T>> range, ICommand<T> add)
			: base(source)
		{
			_range = range;
			_add   = add;
		}

		public void Execute(T parameter)
		{
			_add.Execute(parameter);
		}

		public void Execute(Model.Sequences.Store<T> parameter)
		{
			_range.Execute(parameter);
		}
	}

	public class SystemStore<T> : Deferred<T>, IStore<T>
	{
		readonly IStore<T> _store;

		protected SystemStore(Func<T> source) : this(source.Start()) {}

		protected SystemStore(IResult<T> result) : this(result, Stores.New<T>()) {}

		protected SystemStore(IResult<T> result, IStore<T> store) : this(store.Condition, result, store) {}

		protected SystemStore(ICondition<None> condition, IResult<T> result, IStore<T> store) : base(result, store)
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