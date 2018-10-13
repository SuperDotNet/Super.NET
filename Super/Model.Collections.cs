using Super.Model.Collections;
using Super.Model.Selection;
using Super.Model.Sequences.Query;
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
		public static IReadOnlyDictionary<TKey, Array<TValue>>
			ToDictionary<TKey, TValue>(this IEnumerable<IGrouping<TKey, TValue>> @this)
			=> @this.ToDictionary(x => x.Key, x => new Array<TValue>(x.ToArray()));

		public static ISelect<_, IReadOnlyDictionary<TKey, Array<T>>>
			Grouping<_, T, TKey>(this ISelect<_, Array<T>> @this, ISelect<T, TKey> select)
			=> @this.Grouping(select.Get);

		public static ISelect<_, IReadOnlyDictionary<TKey, Array<T>>>
			Grouping<_, T, TKey>(this ISelect<_, Array<T>> @this, Func<T, TKey> select)
			=> @this.Select(new Grouping<T, TKey>(select));

		public static ISelect<_, T[]> Fixed<_, T>(this ISelect<_, IEnumerable<T>> @this)
			=> @this.Select(x => x.Fixed());

		public static ISelect<_, Array<T>> Result<_, T>(this ISelect<_, IEnumerable<T>> @this)
			=> @this.Select(Model.Collections.Result<T>.Default);

		public static ISelect<_, IEnumerable<T>> Reference<_, T>(this ISelect<_, Array<T>> @this)
			=> @this.Select(x => x.Reference());

		/*public static ISelect<_, T[]> Capture<_, T>(this ISelect<_, IEnumerable<T>> @this)
			=> @this.Select(x => x.ToArray());*/

		/*public static ISelect<uint, IEnumerable<T>> Take<T>(this ISource<IEnumerable<T>> @this)
			=> @this.Out(new ClassicTake<T>(@this.Get));*/

		/*public static ISequence<T> ToSequence<T>(this IEnumerable<T> @this) => new Sequence<T>(@this);

		public static ISequence<T> ToSequence<T>(this ISource<IEnumerable<T>> @this)
			=> @this as ISequence<T> ?? new DecoratedSequence<T>(@this);*/

		public static IArray<_, T> ToStore<_, T>(this ISelect<_, Array<T>> @this) => @this.ToDelegate().ToStore();

		public static IArray<_, T> ToStore<_, T>(this Func<_, Array<T>> @this) => new ArrayStore<_, T>(@this);

		public static ISelect<IEnumerable<TFrom>, IEnumerable<TTo>> Select<TFrom, TTo>(this ISelect<TFrom, TTo> @this)
			=> @this.ToDelegate().To(I<SelectSelector<TFrom, TTo>>.Default);

		public static ISelect<IEnumerable<TFrom>, IEnumerable<TTo>> SelectMany<TFrom, TTo>(
			this ISelect<TFrom, IEnumerable<TTo>> @this)
			=> @this.ToDelegate().To(I<SelectManySelector<TFrom, TTo>>.Default);

		/**/

		/*public static Definition<_, T> Iterate<_, T>(this ISelect<_, T[]> @this) => new ArrayDefinition<_, T>(@this);

		public static Definition<_, T> Iterate<_, T>(this ISelect<_, IEnumerable<T>> @this)
			=> new EnumerableDefinition<_, T>(@this);

		public static Definition<_, T> Alter<_, T>(this Definition<_, T> @this, IContentAlteration<T> alteration)
			=> @this.Alter(new BodyContentAlteration<T>(alteration));

		public static Definition<_, T> Alter<_, T>(this Definition<_, T> @this, IBodyAlteration<T> alteration)
			=> new Definition<_, T>(@this.Start, alteration.Get(@this.Body), @this.Complete);

		public static Definition<_, T> Skip<_, T>(this Definition<_, T> @this, uint skip)
			=> @this.Alter(new Skip<T>(skip));

		public static Definition<_, T> Take<_, T>(this Definition<_, T> @this, uint take)
			=> @this.Alter(new Take<T>(take));

		public static Definition<_, T> WhereBy<_, T>(this Definition<_, T> @this, Expression<Func<T, bool>> where)
			=> @this.Where(where.Compile());

		public static Definition<_, T> Where<_, T>(this Definition<_, T> @this, Func<T, bool> where)
			=> @this.Alter(new WhereSelection<T>(where));

		public static ISelect<_, T[]> Reference<_, T>(this Definition<_, T> @this)
			=> Composer<_, T>.Default.Get(@this).Select(References<T>.Default);

		public static ISelect<_, Array<T>> Result<_, T>(this Definition<_, T> @this)
			=> @this.Reference().Select(x => new Array<T>(x));

		public static ISelect<_, Array<T>> Array<_, T>(this ISelect<_, T[]> @this)
			=> @this.Select(x => new Array<T>(x));*/

		/**/

		public static ISelect<TIn, Array<TOut>> Where<TIn, TOut>(
			this ISelect<TIn, Array<TOut>> @this, Expression<Func<TOut, bool>> specification)
			=> new Where<TIn, TOut>(@this, specification.Compile());

		public static ISelect<TIn, TOut> FirstAssigned<TIn, TOut>(this ISelect<TIn, Array<TOut>> @this)
			where TOut : class => @this.Select(FirstAssigned<TOut>.Default);

		public static ISelect<TIn, TOut?> FirstAssigned<TIn, TOut>(this ISelect<TIn, Array<TOut?>> @this)
			where TOut : struct => @this.Select(FirstAssignedValue<TOut>.Default);

		public static ISelect<TIn, TOut> Only<TIn, TOut>(this ISelect<TIn, Array<TOut>> @this)
			=> @this.Select(Model.Collections.Only<TOut>.Default);

		public static ISelect<TIn, TOut> Only<TIn, TOut>(this ISelect<TIn, Array<TOut>> @this,
		                                                 Func<TOut, bool> where)
			=> where.To(I<Only<TOut>>.Default).To(@this.Select);

		public static ISelect<TIn, TOut> Single<TIn, TOut>(this ISelect<TIn, Array<TOut>> @this)
			=> @this.Select(Single<TOut>.Default);

		public static ISelect<TIn, TOut> Single<TIn, TOut>(this ISelect<TIn, Array<TOut>> @this,
		                                                   Func<TOut, bool> where)
			=> where.To(I<Single<TOut>>.Default).To(@this.Select);

		public static ISelect<TIn, IEnumerable<TOut>> Yield<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> @this.Select(YieldSelector<TOut>.Default);

		/*public static ISelect<TIn, ReadOnlyMemory<TOut>> Access<TIn, TOut>(this ISelect<TIn, IEnumerable<TOut>> @this)
			=> @this.Select(x => new ReadOnlyMemory<TOut>(x.ToArray()));*/

		/*public static ISelect<TIn, ReadOnlyMemory<TOut>> Access<TIn, TOut>(
			this ISelect<TIn, ImmutableArray<TOut>> @this)
			=> @this.Select(x => new ReadOnlyMemory<TOut>(x.ToArray()));*/

		public static ISelect<_, ImmutableArray<T>> Emit<_, T>(this ISelect<_, IEnumerable<T>> @this)
			=> @this.Select(Immutable<T>.Default);

		/*public static ISelect<TIn, ImmutableArray<TOut>> Emit<TIn, TOut>(this ISelect<TIn, IEnumerable<TOut>> @this)
			=> @this.Select(x => x.ToImmutableArray());*/
	}
}