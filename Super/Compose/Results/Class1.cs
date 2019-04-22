using Super.Model.Commands;
using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Selection.Conditions;
using Super.Model.Sequences;
using Super.Runtime;
using Super.Runtime.Activation;
using Super.Runtime.Environment;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Super.Compose.Results
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

		public ArrayExtent<T> Array => ArrayExtent<T>.Default;

		public Extent<Array<T>> Immutable => DefaultExtent<Array<T>>.Default;
	}

	public sealed class ArrayExtent<T> : Extent<T[]>
	{
		public static ArrayExtent<T> Default { get; } = new ArrayExtent<T>();

		ArrayExtent() {}

		public IResult<T[]> New(uint size) => New<int, T[]>.Default.In((int)size);
	}

	public class Extent<T>
	{
		protected Extent() {}

		public Selections As => Selections.Instance;

		public Context<T> By => Context<T>.Instance;

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

		public IResult<T> Activation() => Activator<T>.Default;

		public IResult<T> Singleton() => Singleton<T>.Default;

		public IResult<T> Instantiation() => New<T>.Default;

		public Location<T> Location => Location<T>.Default;

		public IResult<T> Default() => Model.Results.Default<T>.Instance;

		public IResult<T> Using(T instance) => new Instance<T>(instance);

		public IResult<T> Using(ISelect<None, T> source) => new Model.Results.Result<T>(source.Get);

		public IResult<T> Using(IResult<T> instance) => instance;

		public IResult<T> Using<TResult>() where TResult : class, IResult<T>
			=> Activator<TResult>.Default.Get().To(Using);

		public IResult<T> Calling(Func<T> select) => new Model.Results.Result<T>(select);
	}

	public sealed class Location<T> : Model.Selection.Adapters.Result<T>
	{
		public static Location<T> Default { get; } = new Location<T>();

		Location() : base(() => DefaultComponent<T>.Default) {}

		public Alternatives Or => Alternatives.Instance;

		public sealed class Alternatives
		{
			public static Alternatives Instance { get; } = new Alternatives();

			Alternatives() {}

			public IResult<T> Throw() => DefaultComponentLocator<T>.Default;

			public IResult<T> Default(T instance) => new DefaultComponent<T>(instance);

			public IResult<T> Default(Func<T> result) => new DefaultComponent<T>(result);
		}
	}
}