using Super.Model.Collections;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Reflection;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Super
{
	// ReSharper disable once MismatchedFileName

	public static partial class ExtensionMethods
	{
		/*public static IArray<T> ToArray<T>(this ISequence<T> @this)
			=> @this.ToDelegate().ToArray();*/

		public static ISequence<T> ToSequence<T>(this IEnumerable<T> @this) => new Sequence<T>(@this);

		public static ISequence<T> ToSequence<T>(this ISource<IEnumerable<T>> @this) => new DecoratedSequence<T>(@this);

		public static IArray<T> ToArray<T>(this ISequence<T> @this)
			=> new DelegatedArray<T>(Materialize<T>.Default.Out(@this).Get);

		public static IArray<T> ToStore<T>(this ISequence<T> @this)
			=> new DelegatedArray<T>(Materialize<T>.Default.Out(@this).Singleton().Get);

		public static IArray<T> ToStore<T>(this IArray<T> @this) => @this.ToDelegate().ToStore();

		public static IArray<T> ToStore<T>(this Func<ReadOnlyMemory<T>> @this)
			=> new DelegatedArray<T>(@this.Out().Singleton().Get);


		public static IArray<TFrom, TTo> ToArray<TFrom, TTo>(this ISequence<TFrom, TTo> @this)
			=> @this.ToDelegate().ToArray();

		public static IArray<TFrom, TTo> ToArray<TFrom, TTo>(this Func<TFrom, IEnumerable<TTo>> @this)
			=> new Array<TFrom, TTo>(@this);

		public static IArray<TFrom, TTo> ToStore<TFrom, TTo>(this IArray<TFrom, TTo> @this)
			=> @this.ToDelegate().ToStore();

		public static IArray<TFrom, TTo> ToStore<TFrom, TTo>(this Func<TFrom, ReadOnlyMemory<TTo>> @this)
			=> new ArrayStore<TFrom, TTo>(@this);


		public static IEnumerable<T> Sort<T>(this IEnumerable<T> @this) => SortAlteration<T>.Default.Get(@this);

		public static ISelect<IEnumerable<TFrom>, IEnumerable<TTo>> SelectMany<TFrom, TTo>(
			this ISelect<TFrom, IEnumerable<TTo>> @this)
			=> @this.ToDelegate().To(I<SelectManySelector<TFrom, TTo>>.Default);

		public static ISelect<IEnumerable<TFrom>, IEnumerable<TTo>> Select<TFrom, TTo>(this ISelect<TFrom, TTo> @this)
			=> @this.ToDelegate().To(I<SelectSelector<TFrom, TTo>>.Default);

		/*public static ISelect<TParameter, TResult> Composite<TParameter, TResult>(
			this IEnumerable<ISelect<TParameter, TResult>> @this)
			=> @this.Fixed().To(x => x.Skip(1).Aggregate(x.First(), (current, alteration) => alteration.Or(current)));*/

		/*public static ISelect<TIn, TOut> FirstAssigned<TIn, TOut>(this ISelect<TIn, ImmutableArray<TOut>> @this) where TOut : class
			=> @this.Hide().FirstAssigned();*/

		/*public static ISelect<TIn, TOut> FirstAssigned<TIn, TOut>(this ISelect<TIn, IEnumerable<TOut>> @this) where TOut : class
			=> @this.Select(Model.Collections.Assigned<TOut>.Default)
			        .Select(FirstOrDefaultSelector<TOut>.Default);*/

		public static ISelect<TIn, TOut> FirstAssigned<TIn, TOut>(this ISelect<TIn, ReadOnlyMemory<TOut>> @this) where TOut : class
			=> @this.Select(Assigned<TOut>.Default)
			        .Select(FirstOrDefault<TOut>.Default);

		public static ISelect<TIn, TOut> Only<TIn, TOut>(this ISelect<TIn, IEnumerable<TOut>> @this, Func<TOut, bool> where) where TOut : class
			=> @this.Select(new OnlySelector<TOut>(where));

		public static ISelect<TIn, ImmutableArray<TOut>> Enumerate<TIn, TOut>(this ISelect<TIn, IEnumerable<TOut>> @this)
			=> @this.Select(ImmutableArraySelector<TOut>.Default);

		public static ISelect<TIn, IEnumerable<TOut>> Hide<TIn, TOut>(this ISelect<TIn, ImmutableArray<TOut>> @this)
			=> @this.Select(EnumerableSelector<TOut>.Default);

		public static ISelect<TIn, IEnumerable<TOut>> Sequence<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> @this.Select(YieldSelector<TOut>.Default);


		public static ISelect<TIn, ReadOnlyMemory<TOut>> Materialize<TIn, TOut>(this ISelect<TIn, IEnumerable<TOut>> @this)
			=> @this.Select(Materialize<TOut>.Default);

		public static ISelect<TIn, ReadOnlyMemory<TOut>> ToArray<TIn, TOut>(this ISelect<TIn, ImmutableArray<TOut>> @this)
			=> @this.Select(x => new ReadOnlyMemory<TOut>(x.ToArray()));

		public static ISelect<TIn, ImmutableArray<TOut>> Immutable<TIn, TOut>(this IArray<TIn, TOut> @this)
			=> @this.Select(Immutable<TOut>.Default);

		public static ISelect<TIn, IEnumerable<TOut>> Enumerate<TIn, TOut>(this ISelect<TIn, ReadOnlyMemory<TOut>> @this)
			=> @this.Select(Enumerate<TOut>.Default);
	}
}