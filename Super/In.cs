using Super.Model.Selection;
using Super.Model.Selection.Stores;
using Super.Model.Sequences;
using Super.Model.Sequences.Query;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Runtime;
using Super.Runtime.Activation;
using Super.Runtime.Objects;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

namespace Super
{
	/*public sealed class When<T>
	{
		public static When<T> Default { get; } = new When<T>();

		When() {}

		public ISpecification<T> Assigned() => IsAssigned<T>.Default;

		public ISpecification<T> Is(Func<T, bool> specification) => new DelegatedSpecification<T>(specification);
	}*/

	/*public sealed class A<T>
	{

	}*/

	public static class Start
	{
		public static When<T> When<T>() => Super.When<T>.Default;

		/**/

		public static From<T> From<T>() => Super.From<T>.Instance;

		public static From<object> From() => Super.From<object>.Instance;

		/**/

		public static For<T> For<T>() => Super.For<T>.Default;

		/**/

		public static With<T> With<T>() => Super.With<T>.Instance;

		public static ISource<T> With<T>(T instance) => new Source<T>(instance);

		public static ISource<T> With<T>(Func<T> select) => new DelegatedSource<T>(select);

		/*public static ISource<T> Default<T>() => Model.Sources.Default<T>.Instance;*/

		/**/

		public static ISelect<IList<T>, IList<T>> List<T>() => Super.From<T>.Instance.AsList().Self();

		public static ISelect<T[], T[]> Array<T>() => Super.From<T>.Instance.AsArray().Self();

		public static ISequence<T[], T> Sequence<T>() => Array<T>().Sequence();

		public static ISelect<T[], Array<T>> Result<T>() => Sequence<T>().Result();
	}

	public sealed class From<T>
	{
		public static From<T> Instance { get; } = new From<T>();

		From() {}

		public ISelect<T, TOut> Default<TOut>() => Default<T, TOut>.Instance;

		public ISelect<T, Type> Type() => InstanceType<T>.Default;

		public ISelect<T, TypeInfo> Metadata() => InstanceMetadata<T>.Default;

		public ISelect<T, T> Self() => Self<T>.Default;

		//public ISelect<T, T[]> Yield() => Yield<T>.Default;

		public ISelect<T, TOut> New<TOut>() => New<T, TOut>.Default;

		public ISelect<T, TOut> Activate<TOut>() where TOut : IActivateMarker<T> => MarkedActivations<T, TOut>.Default;

		public ISelect<T, TOut> Select<TOut>(Func<T, TOut> select) => Selections<T, TOut>.Default.Get(select);

		public ISelect<T, Func<TIn, TOut>> Delegate<TIn, TOut>(Func<T, Func<TIn, TOut>> select)
			=> new Select<T, Func<TIn, TOut>>(select);

		public ISelect<T, TOut> Cast<TOut>() where TOut : T => CastSelector<T, TOut>.Default;

		public ITable<T, TOut> Table<TOut>() => Table(Default<TOut>().Get);

		public ITable<T, TOut> Table<TOut>(Func<T, TOut> select) => Tables<T, TOut>.Default.Get(select);

		public ISequence<T, T> Sequence() => Yield<T>.Default.Sequence();

		public ISelect<T, Array<T>> Result() => Sequence().Result();
	}

	public static class Extensions
	{
		public static ISpecification<object> Assigned<T>(this When<T> _) where T : class => IsAssigned.Default;

		public static From<T[]> AsArray<T>(this From<T> _) => From<T[]>.Instance;

		public static From<IEnumerable<T>> AsEnumerable<T>(this From<T> _) => From<IEnumerable<T>>.Instance;

		public static From<IList<T>> AsList<T>(this From<T> _) => From<IList<T>>.Instance;

		public static From<ImmutableArray<T>> AsImmutable<T>(this From<T> _) => From<ImmutableArray<T>>.Instance;
	}
}