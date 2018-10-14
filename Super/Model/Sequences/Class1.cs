using Super.Model.Selection;
using Super.Model.Selection.Stores;
using Super.Model.Sources;
using Super.Runtime.Activation;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Super.Model.Sequences
{
	public interface IArray<T> : ISource<Array<T>> {}

	public class ArrayInstance<T> : Source<Array<T>>, IArray<T>
	{
		public ArrayInstance(IEnumerable<T> enumerable) : this(enumerable.Fixed()) {}

		public ArrayInstance(params T[] instance) : base(instance) {}
	}

	public class ArrayStore<T> : DeferredSingleton<Array<T>>, IArray<T>, IActivateMarker<ISource<Array<Type>>>, IActivateMarker<Func<Array<T>>>
	{
		public ArrayStore(ISource<Array<T>> source) : this(source.Get) {}

		public ArrayStore(Func<Array<T>> source) : base(source) {}
	}

	public class DelegatedArray<T> : DelegatedSource<Array<T>>, IArray<T>
	{
		public DelegatedArray(Func<Array<T>> source) : base(source) {}
	}

	public class DecoratedArray<T> : DecoratedSource<Array<T>>, IArray<T>
	{
		public DecoratedArray(ISource<Array<T>> source) : base(source) {}
	}

	public interface IArray<in _, T> : ISelect<_, Array<T>> {}

	public class ArrayStore<_, T> : Store<_, Array<T>>, IArray<_, T>
	{
		public ArrayStore(Func<_, Array<T>> source) : base(source) {}
	}

	sealed class Result<T> : Select<IEnumerable<T>, Array<T>>
	{
		public static Result<T> Default { get; } = new Result<T>();

		Result() : base(x => new Array<T>(x.Fixed())) {}
	}

	sealed class Immutable<T>  : Select<IEnumerable<T>, ImmutableArray<T>>
	{
		public static Immutable<T> Default { get; } = new Immutable<T>();

		Immutable() : base(x => x.ToImmutableArray()) {}
	}
}