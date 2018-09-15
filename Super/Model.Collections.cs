using Super.Model.Collections;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Reflection;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;

namespace Super
{
	// ReSharper disable once MismatchedFileName

	public static partial class ExtensionMethods
	{
		public static ISequence<T> ToSequence<T>(this IEnumerable<T> @this) => new Sequence<T>(@this);

		public static ISequence<T> ToSequence<T>(this ISource<IEnumerable<T>> @this)
			=> @this as ISequence<T> ?? new DecoratedSequence<T>(@this);

		public static IArray<T> ToArray<T>(this ISequence<T> @this)
			=> new DelegatedArray<T>(Access<T>.Default.Out(@this).Get);

		public static IArray<T> ToStore<T>(this ISequence<T> @this)
			=> new DelegatedArray<T>(Access<T>.Default.Out(@this).Singleton().Get);

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

		public static ISelect<IEnumerable<TFrom>, IEnumerable<TTo>> Select<TFrom, TTo>(this ISelect<TFrom, TTo> @this)
			=> @this.ToDelegate().To(I<SelectSelector<TFrom, TTo>>.Default);

		public static ISelect<IEnumerable<TFrom>, IEnumerable<TTo>> SelectMany<TFrom, TTo>(
			this ISelect<TFrom, IEnumerable<TTo>> @this)
			=> @this.ToDelegate().To(I<SelectManySelector<TFrom, TTo>>.Default);

		public static ISelect<TIn, View<TOut>> Iterate<TIn, TOut>(this ISelect<TIn, IEnumerable<TOut>> @this)
			=> @this.Select(Model.Collections.Load<TOut>.Default);

		/*public static ISelect<TIn, View<TTo>> Selection<TIn, TFrom, TTo>(
			this ISelect<TIn, View<TFrom>> @this, Expression<Func<TFrom, TTo>> select, Expression<Func<TTo, bool>> where)
			=> @this.Select(new SelectionWhere<TFrom, TTo>(select, where).Get);*/

		public static ISelect<TIn, View<TTo>> Selection<TIn, TFrom, TTo>(
			this ISelect<TIn, View<TFrom>> @this, Expression<Func<TFrom, TTo>> select)
			=> @this.Select(new Selection<TFrom, TTo>(select));

		/*public static ISelect<TIn, View<TTo>> Selection<TIn, TFrom, TTo>(
			this ISelect<TIn, View<TFrom>> @this, Expression<Func<TFrom, TTo>> select)
			=> new EnhancedResult<TIn, View<TFrom>, View<TTo>>(@this.Get, new ExpressionSelection<TFrom, TTo>(select));*/

		public static ISelect<TIn, View<TOut>> Where<TIn, TOut>(
			this ISelect<TIn, View<TOut>> @this, Expression<Func<TOut, bool>> specification)
			=> @this.Select(new WhereSelection<TOut>(specification.Compile()));

		public static ISelect<TIn, ArraySegment<TOut>> Where2<TIn, TOut>(
			this ISelect<TIn, ArraySegment<TOut>> @this, Expression<Func<TOut, bool>> where)
			=> new Model.Collections.Result<TIn,TOut, TOut>(@this, new ViewSelector<TOut, TOut>(new WhereView<TOut>(where)));



		public static ISelect<TIn, ArraySegment<TOut>> Iterate2<TIn, TOut>(this ISelect<TIn, IEnumerable<TOut>> @this)
			=> @this.Select(Loader<TOut>.Default);

		public static ISelect<TIn, ArraySegment<TTo>> Selection2<TIn, TFrom, TTo>(
			this ISelect<TIn, ArraySegment<TFrom>> @this, Expression<Func<TFrom, TTo>> select)
			=> new Model.Collections.Result<TIn,TFrom, TTo>(@this, new ViewSelector<TFrom, TTo>(new ViewSelect<TFrom, TTo>(select)));


		/*public static ISelect<TIn, View<TOut>> Skip<TIn, TOut>(this ISelect<TIn, View<TOut>> @this, uint skip)
			=> @this.Select(new SkipSelection<TOut>(skip));

		public static ISelect<TIn, View<TOut>> Take<TIn, TOut>(this ISelect<TIn, View<TOut>> @this, uint take)
			=> @this.Select(new TakeSelection<TOut>(take));

		sealed class SkipSelection<T> : ISelection<T, T>
		{
			readonly static ArrayPool<T> Pool = ArrayPool<T>.Shared;

			readonly ArrayPool<T> _pool;
			readonly uint         _skip;

			public SkipSelection(uint skip) : this(Pool, skip) {}

			public SkipSelection(ArrayPool<T> pool, uint skip)
			{
				_pool = pool;
				_skip = skip;
			}

			public View<T> Get(View<T> parameter)
			{
				ref var view        = ref parameter;
				var     size        = view.Used - _skip;
				var     source      = view.Source;
				var     length      = (int)size;
				var     destination = _pool.Rent(length);
				for (var i = _skip; i < size; i++)
				{
					destination[i] = source[i];
				}

				view.Release();

				return new View<T>(_pool, new ArraySegment<T>(destination, 0, length));
			}

			/*public View<T> Get(View<T> parameter) => Get(parameter, (int)_skip);

			View<T> Get(View<T> parameter, int skip)
				=> new View<T>(_pool, new ArraySegment<T>(parameter.Source,
				                                          parameter.Segment.Offset + skip,
				                                          parameter.Segment.Count - skip));#1#
		}

		sealed class TakeSelection<T> : ISelection<T, T>
		{
			readonly static ArrayPool<T> Pool = ArrayPool<T>.Shared;

			readonly ArrayPool<T> _pool;
			readonly uint         _take;

			public TakeSelection(uint take) : this(Pool, take) {}

			public TakeSelection(ArrayPool<T> pool, uint take)
			{
				_pool = pool;
				_take = take;
			}

			/*public View<T> Get(View<T> parameter)
				=> new View<T>(_pool, new ArraySegment<T>(parameter.Source, parameter.Segment.Offset, (int)_take));#1#
			public View<T> Get(View<T> parameter)
			{
				ref var view        = ref parameter;
				var     source      = view.Source;
				var     length      = (int)_take;
				var     destination = _pool.Rent(length);
				for (var i = 0; i < _take; i++)
				{
					destination[i] = source[i];
				}

				view.Release();

				return new View<T>(_pool, new ArraySegment<T>(destination, 0, length));
			}
		}*/

		/*class EnhancedResult<TParameter, TFrom, TTo> : ISelect<TParameter, TTo>
		{
			readonly IEnhancedSelect<TFrom, TTo> _select;
			readonly Func<TParameter, TFrom> _source;

			public EnhancedResult(Func<TParameter, TFrom> source, IEnhancedSelect<TFrom, TTo> @select)
			{
				_select = @select;
				_source = source;
			}

			public TTo Get(TParameter parameter)
			{
				var source = _source(parameter);
				return _select.Get(in source);
			}
		}*/

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