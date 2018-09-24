using Super.Model.Collections;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Reflection;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
// ReSharper disable TooManyArguments

namespace Super
{
	// ReSharper disable once MismatchedFileName

	public static partial class ExtensionMethods
	{
		public static ISequence<T> ToSequence<T>(this IEnumerable<T> @this) => new Sequence<T>(@this);

		public static ISequence<T> ToSequence<T>(this ISource<IEnumerable<T>> @this)
			=> @this as ISequence<T> ?? new DecoratedSequence<T>(@this);

		public static IArrays<T> ToArray<T>(this ISequence<T> @this)
			=> new DelegatedArrays<T>(Access<T>.Default.Out(@this).Get);

		public static IArrays<T> ToStore<T>(this ISequence<T> @this)
			=> new DelegatedArrays<T>(Access<T>.Default.Out(@this).Singleton().Get);

		public static IArrays<T> ToStore<T>(this IArrays<T> @this) => @this.ToDelegate().ToStore();

		public static IArrays<T> ToStore<T>(this Func<ReadOnlyMemory<T>> @this)
			=> new DelegatedArrays<T>(@this.Out().Singleton().Get);

		public static IArray<TFrom, TTo> ToArray<TFrom, TTo>(this ISequence<TFrom, TTo> @this)
			=> @this.ToDelegate().ToArray();

		public static IArray<TFrom, TTo> ToArray<TFrom, TTo>(this Func<TFrom, IEnumerable<TTo>> @this)
			=> new Array<TFrom, TTo>(@this);

		public static IArray<TFrom, TTo> ToStore<TFrom, TTo>(this IArray<TFrom, TTo> @this)
			=> @this.ToDelegate().ToStore();

		public static IArray<TFrom, TTo> ToStore<TFrom, TTo>(this Func<TFrom, ReadOnlyMemory<TTo>> @this)
			=> new ArrayStore<TFrom, TTo>(@this);

		public static ISelect<IEnumerable<TFrom>, IEnumerable<TTo>> Select<TFrom, TTo>(this ISelect<TFrom, TTo> @this)
			=> @this.ToDelegate().To(I<SelectSelector<TFrom, TTo>>.Default);

		public static ISelect<IEnumerable<TFrom>, IEnumerable<TTo>> SelectMany<TFrom, TTo>(
			this ISelect<TFrom, IEnumerable<TTo>> @this)
			=> @this.ToDelegate().To(I<SelectManySelector<TFrom, TTo>>.Default);

		/**/

		public static Definition<_, T> Iterate<_, T>(this ISelect<_, T[]> @this) => new ArrayDefinition<_, T>(@this);
		public static Definition<_, T> Iterate<_, T>(this ISelect<_, IEnumerable<T>> @this)
			=> new EnumerableDefinition<_, T>(@this);

		public static Definition<_, T> Alter<_, T>(this Definition<_, T> @this, IContentAlteration<T> alteration)
			=> @this.Alter(new BodyContentAlteration<T>(alteration));

		public static Definition<_, T> Alter<_, T>(this Definition<_, T> @this, IBodyAlteration<T> alteration)
			=> new Definition<_, T>(@this.Start, alteration.Get(@this.Body), @this.Complete);

		public static Definition<_, T> Skip <_, T>(this Definition<_, T> @this, uint skip) => @this.Alter(new Skip<T>(skip));

		public static Definition<_, T> Take<_, T>(this Definition<_, T> @this, uint take)=> @this.Alter(new Take<T>(take));

		public static Definition<_, T> WhereBy<_, T>(this Definition<_, T> @this, Expression<Func<T, bool>> where)
			=> @this.Where(where.Compile());

		public static Definition<_, T> Where<_, T>(this Definition<_, T> @this, Func<T, bool> where)
			=> @this.Alter(new WhereSelection<T>(where));

		public static ISelect<_, T[]> Reference<_, T>(this Definition<_, T> @this)
			=> Composer<_, T>.Default.Get(@this).Select(References<T>.Default);

		public static ISelect<_, Array<T>> Result<_, T>(this Definition<_, T> @this)
			=> @this.Reference().Select(x => new Array<T>(x));

		/**/

		/*public static ISelect<TIn, ArrayView<TOut>> Iterate<TIn, TOut>(this ISelect<TIn, TOut[]> @this)
			=> @this.Select(x => new ArrayView<TOut>(x));

		public static ISelect<TIn, ArrayView<TTo>> Selection<TIn, TFrom, TTo>(
			this ISelect<TIn, ArrayView<TFrom>> @this, Expression<Func<TFrom, TTo>> select)
			=> @this.Select(new Segmentation<TFrom, TTo>(select));

		public static ISelect<TIn, ArrayView<TTo>> Skip<TIn, TTo>(
			this ISelect<TIn, ArrayView<TTo>> @this, uint skip)
			=> @this.Select(new SkipSelection<TTo>(skip));

		public static ISelect<TIn, ArrayView<TTo>> Take<TIn, TTo>(
			this ISelect<TIn, ArrayView<TTo>> @this, uint take)
			=> @this.Select(new TakeSelection<TTo>(take));

		public static ISelect<TIn, ArrayView<TOut>> Where<TIn, TOut>(
			this ISelect<TIn, ArrayView<TOut>> @this, Expression<Func<TOut, bool>> where)
			=> @this.Select(new WhereSegment<TOut>(where));

		public static ISelect<TIn, Array<TOut>> Emit<TIn, TOut>(this ISelect<TIn, ArrayView<TOut>> @this)
			=> @this.Select(x => x.Get());

		public static ISelect<TIn, Array<TOut>> Release<TIn, TOut>(this ISelect<TIn, ArrayView<TOut>> @this)
			=> @this.Select(Release<TOut>.Default);*/

		public static ISelect<TIn, ReadOnlyMemory<TTo>> Select<TIn, TFrom, TTo>(
			this ISelect<TIn, ReadOnlyMemory<TFrom>> @this, Expression<Func<TFrom, TTo>> select)
			=>        /*@this.Select(new ExpressionSelector<TFrom, TTo>(select))*/
				null; // TODO: FIX!

		public static ISelect<TIn, ReadOnlyMemory<TOut>> Where<TIn, TOut>(
			this ISelect<TIn, ReadOnlyMemory<TOut>> @this, Expression<Func<TOut, bool>> specification)
			=> new Where<TIn, TOut>(@this, specification.Compile());

		public static ISelect<TIn, TOut> FirstAssigned<TIn, TOut>(this ISelect<TIn, ReadOnlyMemory<TOut>> @this)
			where TOut : class => @this.Select(FirstAssigned<TOut>.Default);

		public static ISelect<TIn, TOut?> FirstAssigned<TIn, TOut>(this ISelect<TIn, ReadOnlyMemory<TOut?>> @this)
			where TOut : struct => @this.Select(FirstAssignedValue<TOut>.Default);

		public static ISelect<TIn, TOut> Only<TIn, TOut>(this ISelect<TIn, ReadOnlyMemory<TOut>> @this)
			=> @this.Select(Model.Collections.Only<TOut>.Default);

		public static ISelect<TIn, TOut> Only<TIn, TOut>(this ISelect<TIn, ReadOnlyMemory<TOut>> @this,
		                                                 Func<TOut, bool> where)
			=> @this.Select(I<Only<TOut>>.Default.From(where));

		public static ISelect<TIn, TOut> Single<TIn, TOut>(this ISelect<TIn, ReadOnlyMemory<TOut>> @this)
			=> @this.Select(Single<TOut>.Default);

		public static ISelect<TIn, TOut> Single<TIn, TOut>(this ISelect<TIn, ReadOnlyMemory<TOut>> @this,
		                                                   Func<TOut, bool> where)
			=> @this.Select(I<Single<TOut>>.Default.From(where));

		public static ISelect<TIn, IEnumerable<TOut>> Yield<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> @this.Select(YieldSelector<TOut>.Default);

		public static ISelect<TIn, ReadOnlyMemory<TOut>> Access<TIn, TOut>(this ISelect<TIn, IEnumerable<TOut>> @this)
			=> @this.Select(x => new ReadOnlyMemory<TOut>(x.ToArray()));

		/*public static ISelect<TIn, ReadOnlyMemory<TOut>> Access<TIn, TOut>(
			this ISelect<TIn, ImmutableArray<TOut>> @this)
			=> @this.Select(x => new ReadOnlyMemory<TOut>(x.ToArray()));*/

		/*public static ISelect<TIn, ImmutableArray<TOut>> Emit<TIn, TOut>(this IArray<TIn, TOut> @this)
			=> @this.Select(Immutable<TOut>.Default);*/

		public static ISelect<TIn, ImmutableArray<TOut>> Emit<TIn, TOut>(this ISelect<TIn, IEnumerable<TOut>> @this)
			=> @this.Select(x => x.ToImmutableArray());
	}
}