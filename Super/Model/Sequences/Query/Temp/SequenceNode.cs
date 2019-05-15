using Super.Model.Results;
using Super.Model.Selection;
using System;

namespace Super.Model.Sequences.Query.Temp
{
	public static class Extensions
	{
		public static IBody<T> Get<T>(this IBodyBuilder<T> @this) => @this.Get(SelfProjection<T>.Default);

		public static IBody<T> Get<T>(this IBodyBuilder<T> @this, IBody<T> previous) => @this.Get(@previous.With());

		public static ISelect<Store<T>, T[]> Select<T>(this IBody<T> @this) => new Complete<T, T>(@this);

		public static Argument<T> With<T>(this IBody<T> @this) => @this.With(Assigned<uint>.Unassigned);

		public static Argument<T> With<T>(this IBody<T> @this, Assigned<uint> limit) => new Argument<T>(@this, limit);
	}

	public interface IBodyBuilder<T> : ISelect<Argument<T>, IBody<T>> {}

	public delegate IBody<T> Create<T, in TParameter>(IBody<T> body, TParameter parameter,
	                                                  Assigned<uint> limit);

	public interface IBody<T> : IBody<T, T> {}

	public interface IBody<TIn, TOut> : ISelect<ArrayView<TIn>, ArrayView<TOut>> {}

	public readonly struct Storage<T>
	{
		public Storage(T[] instance) : this(instance, (uint)instance.Length, false) {}

		public Storage(T[] instance, uint length, bool requested = true)
		{
			Instance  = instance;
			Length    = length;
			Requested = requested;
		}

		public T[] Instance { get; }

		public uint Length { get; }

		public bool Requested { get; }
	}

	sealed class Start<_, T> : Instance<ISelect<_, T[]>>, ISequenceNode<_, T>
	{
		public Start(ISelect<_, T[]> start) : base(start) {}

		public ISequenceNode<_, T> Get(IBodyBuilder<T> parameter) => new SequenceNode<_, T>(Get(), parameter.Get());
	}

	public interface ISequenceNode<in _, T> : IResult<ISelect<_, T[]>>,
	                                          ISelect<IBodyBuilder<T>, ISequenceNode<_, T>>
	{
		/*ISequenceNode<_, TOut> Get<TOut>(IProjection<T, TOut> parameter);*/
	}

	/*public interface IProjections<TIn, TOut> : ISelect<IStores<TOut>, IContinuation<TIn, TOut>> {}*/

	sealed class SequenceNode<_, T> : ISequenceNode<_, T>
	{
		readonly ISelect<_, T[]> _start;
		readonly IBody<T>        _current;

		public SequenceNode(ISelect<_, T[]> start) : this(start, SelfProjection<T>.Default) {}

		public SequenceNode(ISelect<_, T[]> start, IBody<T> current)
		{
			_start   = start;
			_current = current;
		}

		public ISelect<_, T[]> Get() => _start.Select(Enter<T>.Default).Select(new Complete<T, T>(_current));

		public ISequenceNode<_, T> Get(IBodyBuilder<T> parameter)
			=> new SequenceNode<_, T>(_start, parameter.Get(_current));
	}

	public interface IStores<T> : ISelect<uint, Storage<T>> {}

	public interface IContent<TIn, TOut> : ISelect<Storage<TIn>, Storage<TOut>> {}

	sealed class Content<TIn, TOut> : IContent<TIn, TOut>
	{
		readonly IBody<TIn>      _body;
		readonly Func<TIn, TOut> _select;
		readonly IStores<TOut>   _stores;

		public Content(IBody<TIn> body, Func<TIn, TOut> select, IStores<TOut> stores)
		{
			_body   = body;
			_select = select;
			_stores = stores;
		}

		public Storage<TOut> Get(Storage<TIn> parameter)
		{
			var content = _body.Get(new ArrayView<TIn>(parameter.Instance, 0, parameter.Length));
			var length  = content.Length;
			var result  = _stores.Get(length);
			var @in     = content.Array;
			var @out    = result.Instance;

			for (var i = 0u; i < length; i++)
			{
				@out[i] = _select(@in[i]);
			}

			return result;
		}
	}

	public interface IEnter<T> : ISelect<T[], Store<T>> {}

	sealed class Enter<T> : IEnter<T>
	{
		public static Enter<T> Default { get; } = new Enter<T>();

		Enter() {}

		public Store<T> Get(T[] parameter) => new Store<T>(parameter);
	}

	public interface IProjection<T> : IProjection<T, T> {}

	public interface IProjection<TIn, TOut> : ISelect<IBody<TIn>, IBody<TOut>> {}

	sealed class SelfProjection<T> : IBody<T>
	{
		public static SelfProjection<T> Default { get; } = new SelfProjection<T>();

		SelfProjection() {}

		public ArrayView<T> Get(ArrayView<T> parameter) => parameter;
	}

	public readonly struct Argument<T>
	{
		public Argument(IBody<T> previous) : this(previous, Assigned<uint>.Unassigned) {}

		public Argument(IBody<T> previous, Assigned<uint> limit)
		{
			Previous = previous;
			Limit    = limit;
		}

		public IBody<T> Previous { get; }
		public Assigned<uint> Limit { get; }
	}

	public class Builder<T, TParameter> : IBodyBuilder<T>
	{
		readonly Create<T, TParameter> _create;
		readonly TParameter            _argument;

		public Builder(Create<T, TParameter> create, TParameter argument)
		{
			_create   = create;
			_argument = argument;
		}

		public IBody<T> Get(Argument<T> parameter) => _create(parameter.Previous, _argument, parameter.Limit);
	}

	public static class Build
	{
		public sealed class Where<T> : Builder<T, Func<T, bool>>
		{
			public Where(Func<T, bool> where)
				: base((body, parameter, limit) => new Temp.Where<T>(body, parameter, limit), where) {}
		}

		public sealed class Skip<T> : Builder<T, uint>
		{
			public Skip(uint count)
				: base((body, parameter, limit) => new Temp.Skip<T>(body, parameter, limit), count) {}
		}

		public sealed class Take<T> : Builder<T, uint>
		{
			public Take(uint count)
				: base((body, parameter, limit) => new Temp.Take<T>(body, limit.IsAssigned
					                                                     ? Math.Min(parameter, limit)
					                                                     : parameter), count) {}
		}
	}

	sealed class Where<T> : IBody<T>
	{
		readonly IBody<T>       _previous;
		readonly Func<T, bool>  _where;
		readonly Assigned<uint> _limit;

		public Where(IBody<T> previous, Func<T, bool> where, Assigned<uint> limit)
		{
			_previous = previous;
			_where    = where;
			_limit    = limit;
		}

		public ArrayView<T> Get(ArrayView<T> parameter)
		{
			var view  = _previous.Get(parameter);
			var to    = view.Start + view.Length;
			var array = view.Array;
			var count = 0u;
			var limit = _limit.Or(view.Length);
			for (var i = view.Start; i < to && count < limit; i++)
			{
				var item = array[i];
				if (_where(item))
				{
					array[count++] = item;
				}
			}

			return new ArrayView<T>(view.Array, 0, count);
		}
	}

	sealed class Skip<T> : IBody<T>
	{
		readonly IBody<T>       _previous;
		readonly uint           _count;
		readonly Assigned<uint> _limit;

		public Skip(IBody<T> previous, uint count, Assigned<uint> limit)
		{
			_previous = previous;
			_count    = count;
			_limit    = limit;
		}

		public ArrayView<T> Get(ArrayView<T> parameter)
		{
			var previous = _previous.Get(parameter);
			var length   = Math.Max(0, (previous.Length - (int)_count).Clamp0());
			var capped   = _limit.IsAssigned ? Math.Min(length, _limit.Instance) : length;
			var result = new ArrayView<T>(previous.Array, Math.Min(previous.Length, previous.Start + _count),
			                              capped);
			return result;
		}
	}

	sealed class Take<T> : IBody<T>
	{
		readonly IBody<T> _previous;
		readonly uint     _count;

		public Take(IBody<T> previous, uint count)
		{
			_previous = previous;
			_count    = count;
		}

		public ArrayView<T> Get(ArrayView<T> parameter)
		{
			var previous = _previous.Get(parameter);
			var result   = new ArrayView<T>(previous.Array, previous.Start, Math.Min(previous.Length, _count));
			return result;
		}
	}

	sealed class Complete<TIn, TOut> : ISelect<Store<TIn>, TOut[]>
	{
		readonly IBody<TIn, TOut> _body;
		readonly Action<TIn[]>    _return;

		public Complete(IBody<TIn, TOut> body) : this(body, Return<TIn>.Default.Execute) {}

		public Complete(IBody<TIn, TOut> body, Action<TIn[]> @return)
		{
			_body   = body;
			_return = @return;
		}

		public TOut[] Get(Store<TIn> parameter)
		{
			var result = _body.Get(new ArrayView<TIn>(parameter.Instance, 0, parameter.Length())).ToArray();

			if (parameter.Length.IsAssigned)
			{
				_return(parameter.Instance);
			}

			return result;
		}
	}

	/*sealed class Complete<T> : ISelect<Store<T>, T[]>
	{
		public static Complete<T> Default { get; } = new Complete<T>();

		Complete() : this(Return<T>.Default.Execute) {}

		readonly Action<T[]> _return;

		public Complete(Action<T[]> @return) => _return = @return;

		public T[] Get(Store<T> parameter)
		{
			if (parameter.Length.IsAssigned)
			{
				var result = parameter.Instance.CopyInto(new T[parameter.Length], 0, parameter.Length);
				_return(parameter.Instance);
				return result;
			}

			return parameter.Instance;
		}
	}*/

	/*public interface IContext<T> : IContext<T, T> {}

	public interface IContext<TIn, TOut> : ISelect<ArrayView<TIn>, Assigned<ArrayView<TOut>>> {}

	sealed class DefaultContext<T> : IContext<T>
	{
		public static DefaultContext<T> Default { get; } = new DefaultContext<T>();

		DefaultContext() {}

		public Assigned<ArrayView<T>> Get(ArrayView<T> parameter) => parameter;
	}

	/*public readonly struct State<T>
	{
		public State(Store<T> result, ArrayView<T> current)
		{
			Result  = result;
			Current = current;
		}

		public Store<T> Result { get; }

		public ArrayView<T> Current { get; }
	}#1#

	sealed class Where<T> : IContext<T>
	{
		readonly IContext<T>   _previous;
		readonly Func<T, bool> _where;

		public Where(Func<T, bool> where) : this(DefaultContext<T>.Default, where) {}

		public Where(IContext<T> previous, Func<T, bool> where)
		{
			_previous = previous;
			_where    = @where;
		}

		public Assigned<ArrayView<T>> Get(ArrayView<T> parameter)
		{
			var previous = _previous.Get(parameter);
			if (previous.IsAssigned)
			{
				var to    = parameter.Start + parameter.Length;
				var array = parameter.Array;
				var count = 0u;
				for (var i = parameter.Start; i < to; i++)
				{
					var item = array[i];
					if (_where(item))
					{
						array[count++] = item;
					}
				}

				return new ArrayView<T>(parameter.Array, 0, count);
			}

			return previous;
		}
	}

	sealed class Take<T> : IContext<T>
	{
		readonly IContext<T> _previous;
		readonly uint        _count;

		public Take(uint count) : this(DefaultContext<T>.Default, count) {}

		public Take(IContext<T> previous, uint count)
		{
			_previous = previous;
			_count    = count;
		}

		public Assigned<ArrayView<T>> Get(ArrayView<T> parameter)
		{
			var previous = _previous.Get(parameter);
			return previous.IsAssigned
				       ? (Assigned<ArrayView<T>>)new ArrayView<T>(previous.Instance.Array, previous.Instance.Start,
				                                                  Math.Min(previous.Instance.Length, _count))
				       : previous;
		}
	}

	sealed class Skip<T> : IContext<T>
	{
		readonly IContext<T> _previous;
		readonly uint        _count;

		public Skip(uint count) : this(DefaultContext<T>.Default, count) {}

		public Skip(IContext<T> previous, uint count)
		{
			_previous = previous;
			_count    = count;
		}

		public Assigned<ArrayView<T>> Get(ArrayView<T> parameter)
		{
			var previous = _previous.Get(parameter);
			return previous.IsAssigned
				       ? (Assigned<ArrayView<T>>)new ArrayView<T>(parameter.Array,
				                                                  Math.Min(parameter.Length, parameter.Start + _count),
				                                                  Math.Max(parameter.Start,
				                                                           (parameter.Length - (int)_count).Clamp0()))
				       : previous;
		}
	}*/
}