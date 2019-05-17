using Super.Model.Commands;
using Super.Model.Results;
using Super.Model.Selection;
using System;
using System.Buffers;

namespace Super.Model.Sequences.Query.Temp
{
	public static class Extensions
	{
		public static IShape<T> Get<T>(this IShaper<T> @this) => @this.Get(SelfProjection<T>.Default);

		public static IShape<T> Get<T>(this IShaper<T> @this, IShape<T> previous) => @this.Get(@previous.With());

		public static ISelect<Storage<T>, T[]> Select<T>(this IShape<T> @this) => new Complete<T, T>(@this);

		public static Parameter<TIn, TOut> With<TIn, TOut>(this IShape<TIn> @this, IStores<TOut> stores)
			=> new Parameter<TIn, TOut>(@this, stores);

		public static Parameter<T> With<T>(this IShape<T> @this) => @this.With(Assigned<uint>.Unassigned);

		public static Parameter<T> With<T>(this IShape<T> @this, Assigned<uint> limit)
			=> new Parameter<T>(@this, limit);
	}

	public interface IShaper<T> : ISelect<Parameter<T>, IShape<T>> {}

	public delegate IShape<T> Create<T, in TParameter>(IShape<T> shape, TParameter parameter,
	                                                   Assigned<uint> limit);

	public interface IShape<T> : IBody<T, T> {}

	public interface IBody<TIn, TOut> : ISelect<ArrayView<TIn>, ArrayView<TOut>> {}

	public readonly struct Storage<T>
	{
		public static implicit operator Storage<T>(T[] instance) => new Storage<T>(instance);

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

	public interface ISequenceNode<in _, T> : IResult<ISelect<_, T[]>>, ISelect<IShaper<T>, ISequenceNode<_, T>>
	{
		ISequenceNode<_, TTo> Get<TTo>(IShifter<T, TTo> parameter);
	}

	sealed class Start<_, T> : Instance<ISelect<_, T[]>>, ISequenceNode<_, T>
	{
		public Start(ISelect<_, T[]> start) : base(start) {}

		public ISequenceNode<_, T> Get(IShaper<T> parameter) => new Shaped<_, T>(Get(), parameter.Get());

		public ISequenceNode<_, TTo> Get<TTo>(IShifter<T, TTo> parameter) => new Shaped<_, T>(Get()).Get(parameter);
	}

	sealed class Shaped<_, T> : ISequenceNode<_, T>
	{
		readonly ISelect<_, T[]> _start;
		readonly IShape<T>       _current;

		public Shaped(ISelect<_, T[]> start) : this(start, SelfProjection<T>.Default) {}

		public Shaped(ISelect<_, T[]> start, IShape<T> current)
		{
			_start   = start;
			_current = current;
		}

		public ISelect<_, T[]> Get() => _start.Select(Enter<T>.Default).Select(new Complete<T, T>(_current));

		public ISequenceNode<_, T> Get(IShaper<T> parameter)
			=> new Shaped<_, T>(_start, parameter.Get(_current));

		public ISequenceNode<_, TTo> Get<TTo>(IShifter<T, TTo> parameter)
			=> new ShapedContentNode<_, T, TTo>(_start.Select(Enter<T>.Default), parameter, _current);
	}

	sealed class ContentNode<_, T> : ISequenceNode<_, T>
	{
		readonly ISelect<_, Storage<T>> _start;

		public ContentNode(ISelect<_, Storage<T>> start) => _start = start;

		public ISelect<_, T[]> Get() => _start.Select(Complete<T>.Default);

		public ISequenceNode<_, T> Get(IShaper<T> parameter) => new ShapedContentNode<_, T>(_start, parameter.Get());

		public ISequenceNode<_, TTo> Get<TTo>(IShifter<T, TTo> parameter)
			=> new ContentNode<_, TTo>(_start.Select(parameter.Get(SelfProjection<T>.Default
			                                                                        .With(Leases<TTo>.Default))));
	}

	sealed class ShapedContentNode<_, T> : ISequenceNode<_, T>
	{
		readonly ISelect<_, Storage<T>> _start;
		readonly IShape<T>              _current;

		public ShapedContentNode(ISelect<_, Storage<T>> start, IShape<T> current)
		{
			_start   = start;
			_current = current;
		}

		public ISelect<_, T[]> Get() => _start.Select(new Complete<T, T>(_current));

		public ISequenceNode<_, T> Get(IShaper<T> parameter)
			=> new ShapedContentNode<_, T>(_start, parameter.Get(_current));

		public ISequenceNode<_, TTo> Get<TTo>(IShifter<T, TTo> parameter)
			=> new ShapedContentNode<_, T, TTo>(_start, parameter, _current);
	}

	sealed class ShapedContentNode<_, TIn, TOut> : ISequenceNode<_, TOut>
	{
		readonly ISelect<_, Storage<TIn>> _origin;
		readonly IShifter<TIn, TOut>      _shifter;
		readonly IShape<TIn>              _shape;

		public ShapedContentNode(ISelect<_, Storage<TIn>> origin, IShifter<TIn, TOut> shifter, IShape<TIn> shape)
		{
			_origin  = origin;
			_shifter = shifter;
			_shape   = shape;
		}

		ISelect<_, Storage<TOut>> Close(IStores<TOut> stores) => _origin.Select(_shifter.Get(_shape.With(stores)));

		public ISelect<_, TOut[]> Get() => Close(Stores<TOut>.Default).Select(Complete<TOut>.Default);

		public ISequenceNode<_, TOut> Get(IShaper<TOut> parameter)
			=> new ShapedContentNode<_, TOut>(Close(Leases<TOut>.Default), parameter.Get());

		public ISequenceNode<_, TTo> Get<TTo>(IShifter<TOut, TTo> parameter)
			=> new ContentNode<_, TOut>(Close(Leases<TOut>.Default)).Get(parameter);
	}

	public interface IStores<T> : ISelect<uint, Storage<T>> {}

	public sealed class Stores<T> : Model.Selection.Select<uint, Storage<T>>, IStores<T>
	{
		public static Stores<T> Default { get; } = new Stores<T>();

		Stores() : base(x => new Storage<T>(new T[x])) {}
	}

	public class Storages<T> : Model.Selection.Select<uint, Storage<T>>, IStores<T>
	{
		readonly ICommand<T[]> _return;

		public Storages(IStores<T> stores, ICommand<T[]> @return) : base(stores.Get) => _return = @return;

		public void Execute(T[] parameter)
		{
			_return.Execute(parameter);
		}
	}

	public sealed class Leases<T> : Storages<T>
	{
		public static Leases<T> Default { get; } = new Leases<T>();

		Leases() : base(Allotted<T>.Default, Return<T>.Default) {}
	}

	public sealed class Allotted<T> : IStores<T>
	{
		public static Allotted<T> Default { get; } = new Allotted<T>();

		Allotted() : this(ArrayPool<T>.Shared) {}

		readonly ArrayPool<T> _pool;

		public Allotted(ArrayPool<T> pool) => _pool = pool;

		public Storage<T> Get(uint parameter) => new Storage<T>(_pool.Rent((int)parameter), parameter);
	}

	public interface IShifter<TIn, TOut> : ISelect<Parameter<TIn, TOut>, IShift<TIn, TOut>> {}

	public interface IShift<TIn, TOut> : ISelect<Storage<TIn>, Storage<TOut>> {}

	public delegate IShift<TIn, TOut> Create<TIn, TOut, in TParameter>(IShape<TIn> shape, IStores<TOut> stores,
	                                                                   TParameter parameter, Assigned<uint> limit);

	sealed class Select<TIn, TOut> : IShift<TIn, TOut>
	{
		readonly IShape<TIn>     _shape;
		readonly Func<TIn, TOut> _select;
		readonly IStores<TOut>   _stores;
		readonly Assigned<uint>  _limit;

		// ReSharper disable once TooManyDependencies
		public Select(IShape<TIn> shape, IStores<TOut> stores, Func<TIn, TOut> select, Assigned<uint> limit)
		{
			_shape  = shape;
			_stores = stores;
			_select = select;
			_limit  = limit;
		}

		public Storage<TOut> Get(Storage<TIn> parameter)
		{
			var content = _shape.Get(new ArrayView<TIn>(parameter.Instance, 0, parameter.Length));
			var length  = _limit.IsAssigned ? Math.Min(_limit.Instance, content.Length) : content.Length;
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

	public interface IEnter<T> : ISelect<T[], Storage<T>> {}

	sealed class Enter<T> : IEnter<T>
	{
		public static Enter<T> Default { get; } = new Enter<T>();

		Enter() {}

		public Storage<T> Get(T[] parameter) => parameter;
	}

	sealed class Lease<T> : IEnter<T>
	{
		public static Lease<T> Default { get; } = new Lease<T>();

		Lease() : this(ArrayPool<T>.Shared) {}

		readonly ArrayPool<T> _pool;

		public Lease(ArrayPool<T> pool) => _pool = pool;

		public Storage<T> Get(T[] parameter)
			=> new Storage<T>(parameter.CopyInto(_pool.Rent(parameter.Length)), (uint)parameter.Length);
	}

	public interface IProjection<T> : IProjection<T, T> {}

	public interface IProjection<TIn, TOut> : ISelect<IShape<TIn>, IShape<TOut>> {}

	public readonly struct Parameter<TIn, TOut>
	{
		public Parameter(IShape<TIn> shape, IStores<TOut> stores) : this(shape, stores, Assigned<uint>.Unassigned) {}

		public Parameter(IShape<TIn> shape, IStores<TOut> stores, Assigned<uint> limit)
		{
			Shape  = shape;
			Stores = stores;
			Limit  = limit;
		}

		public IShape<TIn> Shape { get; }

		public IStores<TOut> Stores { get; }

		public Assigned<uint> Limit { get; }
	}

	public readonly struct Parameter<T>
	{
		public Parameter(IShape<T> previous) : this(previous, Assigned<uint>.Unassigned) {}

		public Parameter(IShape<T> previous, Assigned<uint> limit)
		{
			Previous = previous;
			Limit    = limit;
		}

		public IShape<T> Previous { get; }
		public Assigned<uint> Limit { get; }
	}

	sealed class SelfProjection<T> : IShape<T>
	{
		public static SelfProjection<T> Default { get; } = new SelfProjection<T>();

		SelfProjection() {}

		public ArrayView<T> Get(ArrayView<T> parameter) => parameter;
	}

	public class Builder<T, TParameter> : IShaper<T>
	{
		readonly Create<T, TParameter> _create;
		readonly TParameter            _argument;

		public Builder(Create<T, TParameter> create, TParameter argument)
		{
			_create   = create;
			_argument = argument;
		}

		public IShape<T> Get(Parameter<T> parameter) => _create(parameter.Previous, _argument, parameter.Limit);
	}

	public class Builder<TIn, TOut, TParameter> : IShifter<TIn, TOut>
	{
		readonly Create<TIn, TOut, TParameter> _create;
		readonly TParameter                    _argument;

		public Builder(Create<TIn, TOut, TParameter> create, TParameter argument)
		{
			_create   = create;
			_argument = argument;
		}

		public IShift<TIn, TOut> Get(Parameter<TIn, TOut> parameter)
			=> _create(parameter.Shape, parameter.Stores, _argument, parameter.Limit);
	}

	public static class Build
	{
		public sealed class Select<TIn, TOut> : Builder<TIn, TOut, Func<TIn, TOut>>
		{
			public Select(Func<TIn, TOut> argument)
				: base((shape, stores, parameter, limit) => new Temp.Select<TIn, TOut>(shape, stores, parameter, limit),
				       argument) {}
		}

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

	sealed class Where<T> : IShape<T>
	{
		readonly IShape<T>      _previous;
		readonly Func<T, bool>  _where;
		readonly Assigned<uint> _limit;

		public Where(IShape<T> previous, Func<T, bool> where, Assigned<uint> limit)
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

	sealed class Skip<T> : IShape<T>
	{
		readonly IShape<T>      _previous;
		readonly uint           _count;
		readonly Assigned<uint> _limit;

		public Skip(IShape<T> previous, uint count, Assigned<uint> limit)
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

	sealed class Take<T> : IShape<T>
	{
		readonly IShape<T> _previous;
		readonly uint      _count;

		public Take(IShape<T> previous, uint count)
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

	sealed class Complete<T> : ISelect<Storage<T>, T[]>
	{
		public static Complete<T> Default { get; } = new Complete<T>();

		Complete() : this(Return<T>.Default.Execute) {}

		readonly Action<T[]> _return;

		public Complete(Action<T[]> @return) => _return = @return;

		public T[] Get(Storage<T> parameter)
		{
			if (parameter.Requested)
			{
				var result = parameter.Instance.CopyInto(new T[parameter.Length], 0, parameter.Length);
				_return(parameter.Instance);
				return result;
			}

			return parameter.Instance;
		}
	}

	sealed class Complete<TIn, TOut> : ISelect<Storage<TIn>, TOut[]>
	{
		readonly IBody<TIn, TOut> _body;
		readonly Action<TIn[]>    _return;

		public Complete(IBody<TIn, TOut> body) : this(body, Return<TIn>.Default.Execute) {}

		public Complete(IBody<TIn, TOut> body, Action<TIn[]> @return)
		{
			_body   = body;
			_return = @return;
		}

		public TOut[] Get(Storage<TIn> parameter)
		{
			var result = _body.Get(new ArrayView<TIn>(parameter.Instance, 0, parameter.Length)).ToArray();

			if (parameter.Requested)
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