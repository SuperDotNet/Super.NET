using Super.Model.Commands;
using Super.Model.Results;
using Super.Model.Selection.Conditions;
using Super.Model.Sequences;
using Super.Runtime;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Super.Compose.Commands
{
	public sealed class Context
	{
		public static Context Default { get; } = new Context();

		Context() {}

		public Extent Of => Extent.Default;
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

		public Extent<Array<T>> Immutable => DefaultExtent<Array<T>>.Default;
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

		public ICommand<T> Empty => EmptyCommand<T>.Default;

		public Model.Selection.Adapters.Action<T> Calling(Action<T> body)
			=> new Model.Selection.Adapters.Action<T>(body);
	}
}
