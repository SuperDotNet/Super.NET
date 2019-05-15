using Super.Model.Commands;
using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Selection.Adapters;
using Super.Model.Selection.Alterations;
using Super.Model.Selection.Conditions;
using Super.Model.Selection.Stores;
using Super.Model.Sequences;
using Super.Model.Sequences.Query;
using Super.Reflection;
using Super.Runtime;
using Super.Runtime.Activation;
using Super.Runtime.Environment;
using Super.Runtime.Objects;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Super.Compose.Selections
{
	public sealed class Context
	{
		public static Context Default { get; } = new Context();

		Context() : this(Extent.Default) {}

		public Context(Extent extent) => Of = extent;

		public Extent Of { get; }
	}

	public sealed class Extent
	{
		public static Extent Default { get; } = new Extent();

		Extent() {}

		public SystemExtents System => SystemExtents.Instance;
		public Extent<object> Any => DefaultExtent<object>.Default;
		public Extent<None> None => DefaultExtent<None>.Default;

		public Extent<T> Type<T>() => DefaultExtent<T>.Default;

		public sealed class SystemExtents
		{
			public static SystemExtents Instance { get; } = new SystemExtents();

			SystemExtents() {}

			public Extent<Type> Type => DefaultExtent<Type>.Default;

			public Extent<TypeInfo> Metadata => DefaultExtent<TypeInfo>.Default;
		}
	}

	public sealed class DefaultExtent<T> : Extent<T>
	{
		public static DefaultExtent<T> Default { get; } = new DefaultExtent<T>();

		DefaultExtent() {}
	}

	public sealed class SequenceExtent<T> : Extent<IEnumerable<T>>
	{
		public static SequenceExtent<T> Default { get; } = new SequenceExtent<T>();

		SequenceExtent() {}

		public Extent<T[]> Array => DefaultExtent<T[]>.Default;

		public Extent<IList<T>> List => DefaultExtent<IList<T>>.Default;

		public Extent<Array<T>> Immutable => DefaultExtent<Array<T>>.Default;
	}

	public class Extent<T>
	{
		protected Extent() {}

		public Selections As => Selections.Instance;

		public Context<T> By => Context<T>.Instance;

		public Extent<T, TOut> AndOf<TOut>() => DefaultExtent<T, TOut>.Default;

		public class Selections
		{
			public static Selections Instance { get; } = new Selections();

			Selections() {}

			public SequenceExtent<T> Sequence => SequenceExtent<T>.Default;
			public Extent<Func<T>> Delegate => DefaultExtent<Func<T>>.Default;
			public Extent<ICondition<T>> Condition => DefaultExtent<ICondition<T>>.Default;
			public Extent<IResult<T>> Result => DefaultExtent<IResult<T>>.Default;
			public Extent<ICommand<T>> Command => DefaultExtent<ICommand<T>>.Default;
		}
	}

	public sealed class Context<T>
	{
		public static Context<T> Instance { get; } = new Context<T>();

		Context() {}

		public ISelect<T, T> Self => Self<T>.Default;

		public ISelect<T, T> Default() => Model.Selection.Default<T>.Instance;

		public ISelect<T, TypeInfo> Metadata => InstanceMetadata<T>.Default;
		public ISelect<T, Type> Type => InstanceType<T>.Default;

		public ISelect<T, TOut> Calling<TOut>(Func<T, TOut> select) => new Select<T, TOut>(select);

		public ISelect<T, TOut> Calling<TOut>(Func<TOut> result) => new DelegatedResult<T, TOut>(result);

		public IAlteration<T> Calling(Func<T, T> result) => new Alteration<T>(result);

		public ISelect<T, TOut> Returning<TOut>(TOut result) => new FixedResult<T, TOut>(result);

		public ISelect<T, TOut> Returning<TOut>(IResult<TOut> result) => Calling(result.Get);

		public ICondition<T> Returning(IResult<bool> condition) => Calling(condition.Get).ToCondition();

		public IAlteration<T> Returning(T result) => Calling(new FixedResult<T, T>(result).Get);

		public ISelect<T, TOut> Default<TOut>() => Default<T, TOut>.Instance;

		public ISelect<T, TOut> Cast<TOut>() where TOut : T => CastOrDefault<T, TOut>.Default;

		public ISelect<T, Array<T>> Array() => Self.Select(Yield<T>.Default).Result();

		public ISelect<T, Func<TIn, TOut>> Delegate<TIn, TOut>(Func<T, Func<TIn, TOut>> select)
			=> new Select<T, Func<TIn, TOut>>(select);


		public ISelect<T, TOut> Activation<TOut>() => Activator<TOut>.Default.ToSelect(I.A<T>());

		public ISelect<T, TOut> StoredActivation<TOut>() where TOut : IActivateUsing<T> => Activations<T, TOut>.Default;

		public ISelect<T, TOut> Singleton<TOut>()
			=> Runtime.Activation.Singleton<TOut>.Default.ToSelect(I.A<T>());

		public ISelect<T, TOut> Instantiation<TOut>() => New<T, TOut>.Default;

		public Location<T, TOut> Location<TOut>() => Location<T, TOut>.Default;
	}

	public sealed class DefaultExtent<TIn, TOut> : Extent<TIn, TOut>
	{
		public static DefaultExtent<TIn, TOut> Default { get; } = new DefaultExtent<TIn, TOut>();

		DefaultExtent() {}
	}

	public sealed class SequenceExtent<TIn, TOut> : Extent<TIn, IEnumerable<TOut>>
	{
		public static SequenceExtent<TIn, TOut> Default { get; } = new SequenceExtent<TIn, TOut>();

		SequenceExtent() {}

		public Extent<TIn, TOut[]> Array => DefaultExtent<TIn, TOut[]>.Default;

		public Extent<TIn, Array<TOut>> Immutable => DefaultExtent<TIn, Array<TOut>>.Default;
	}

	public class Extent<TIn, TOut>
	{
		protected Extent() {}

		public Selections As => Selections.Instance;

		public Context<TIn, TOut> By => Context<TIn, TOut>.Instance;

		public Into<TIn, TOut> Into => Into<TIn, TOut>.Default;

		public class Selections
		{
			public static Selections Instance { get; } = new Selections();

			Selections() {}

			public SequenceExtent<TIn, TOut> Sequence => SequenceExtent<TIn, TOut>.Default;
			public Extent<TIn, Func<TOut>> Delegate => DefaultExtent<TIn, Func<TOut>>.Default;
			public Extent<TIn, ICondition<TOut>> Condition => DefaultExtent<TIn, ICondition<TOut>>.Default;
			public Extent<TIn, IResult<TOut>> Result => DefaultExtent<TIn, IResult<TOut>>.Default;
			public Extent<TIn, ICommand<TOut>> Command => DefaultExtent<TIn, ICommand<TOut>>.Default;
		}
	}

	public sealed class Context<TIn, TOut>
	{
		public static Context<TIn, TOut> Instance { get; } = new Context<TIn, TOut>();

		Context() {}

		public ISelect<TIn, TOut> Activation() => Activator<TOut>.Default.ToSelect(I.A<TIn>());

		public ISelect<TIn, TOut> Singleton() => Singleton<TOut>.Default.ToSelect(I.A<TIn>());

		public ISelect<TIn, TOut> Instantiation => New<TIn, TOut>.Default;

		public Location<TIn, TOut> Location => Location<TIn, TOut>.Default;

		public Cast<TIn, TOut> Cast => Cast<TIn, TOut>.Default;

		public ISelect<TIn, TOut> Returning(TOut result) => new FixedResult<TIn, TOut>(result);

		public ISelect<TIn, TOut> Returning(Func<TOut> result) => new DelegatedResult<TIn, TOut>(result);
	}

	public sealed class Location<TIn, TOut> : Select<TIn, TOut>
	{
		public static Location<TIn, TOut> Default { get; } = new Location<TIn, TOut>();

		Location() : base(_ => DefaultComponent<TOut>.Default) {}

		public Alternatives Or => Alternatives.Instance;

		public sealed class Alternatives
		{
			public static Alternatives Instance { get; } = new Alternatives();

			Alternatives() {}

			public ISelect<TIn, TOut> Throw() => DefaultComponentLocator<TOut>.Default.ToSelect(I.A<TIn>());

			public ISelect<TIn, TOut> Default(TOut instance)
				=> new Component<TOut>(instance.Start()).ToSelect(I.A<TIn>());

			public ISelect<TIn, TOut> Default(Func<TOut> result)
				=> new Component<TOut>(result).ToSelect(I.A<TIn>());
		}
	}

	public sealed class Into<TIn, TOut>
	{
		public static Into<TIn, TOut> Default { get; } = new Into<TIn, TOut>();

		Into() {}

		public ITable<TIn, TOut> Table() => Table(x => default);

		public ITable<TIn, TOut> Table(Func<TIn, TOut> select) => Tables<TIn, TOut>.Default.Get(select);

		public ICondition<TIn> Condition(Func<TIn, bool> condition)
			=> new Model.Selection.Conditions.Condition<TIn>(condition);

		public IAction<TIn> Action(System.Action<TIn> body) => new Model.Selection.Adapters.Action<TIn>(body);
	}

	public sealed class Cast<TIn, TOut> : Select<TIn, TOut>
	{
		public static Cast<TIn, TOut> Default { get; } = new Cast<TIn, TOut>();

		Cast() : base(x => CastOrDefault<TIn, TOut>.Default.Get(x)) {}

		public Alternatives Or => Alternatives.Instance;

		public sealed class Alternatives
		{
			public static Alternatives Instance { get; } = new Alternatives();

			Alternatives() {}

			public ISelect<TIn, TOut> Throw => CastOrDefault<TIn, TOut>.Default;

			public ISelect<TIn, TOut> Result => ResultAwareCast<TIn, TOut>.Default;

			public ISelect<TIn, TOut> Return(TOut result)
				=> new CastOrDefault<TIn, TOut>(new FixedResult<TIn, TOut>(result).Get);

			public ISelect<TIn, TOut> Return(Func<TOut> result)
				=> new CastOrDefault<TIn, TOut>(new DelegatedResult<TIn, TOut>(result).Get);
		}
	}
}