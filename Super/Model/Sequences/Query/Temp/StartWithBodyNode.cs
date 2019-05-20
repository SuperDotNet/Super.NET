using Super.Model.Commands;
using Super.Model.Results;
using Super.Model.Selection;
using System;
using System.Buffers;

namespace Super.Model.Sequences.Query.Temp
{
	public static class Extensions
	{
		/*public static IBody<T> Get<T>(this IBodyBuilder<T> @this) => @this.Get(SelfProjection<T>.Default);*/

		/*public static ISelect<Storage<T>, T[]> Select<T>(this IBody<T> @this) => new Complete<T, T>(@this);*/

		/*public static Parameter<TIn, TOut> With<TIn, TOut>(this IBody<TIn> @this, IStores<TOut> stores)
			=> @this.With(stores, Assigned<uint>.Unassigned);*/

		/*public static Parameter<TIn, TOut> With<TIn, TOut>(this IBody<TIn> @this,
		                                                   IStores<TOut> stores, Assigned<uint> limit)
			=> new Parameter<TIn, TOut>(@this, stores, limit);*/
	}

	public interface IBodyBuilder<T> : ISelect<Assigned<uint>, IBody<T>> {}

	sealed class BodyBuilder<T> : FixedResult<Assigned<uint>, IBody<T>>, IBodyBuilder<T>
	{
		public static BodyBuilder<T> Default { get; } = new BodyBuilder<T>();

		BodyBuilder() : base(EmptyBody<T>.Default) {}
	}

	public delegate IBody<T> Create<T, in TParameter>(TParameter parameter, Assigned<uint> limit);

	public interface IBody<T> : IBody<T, T> {}

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

	public interface ISequenceNode<in _, T> : IResult<ISelect<_, T[]>>, ISelect<IBodyBuilder<T>, ISequenceNode<_, T>>
	{
		ISequenceNode<_, TTo> Get<TTo>(IContents<T, TTo> parameter);

		ISelect<_, TTo> Get<TTo>(IElement<T, TTo> parameter);
	}

	sealed class Start<_, T> : Instance<ISelect<_, T[]>>, ISequenceNode<_, T>
	{
		public Start(ISelect<_, T[]> origin) : base(origin) {}

		public ISequenceNode<_, T> Get(IBodyBuilder<T> parameter)
			=> parameter is IEnterAware<T> enter
				   ? new StoreWithBodyNode<_, T>(new Enter<_, T>(Get(), enter.Enter), parameter)
				   : (ISequenceNode<_, T>)new StartWithBodyNode<_, T>(Get(), parameter);

		public ISequenceNode<_, TTo> Get<TTo>(IContents<T, TTo> parameter)
			=> new ContentContainerNode<_, T, TTo>(new Enter<_, T>(Get()), parameter);

		public ISelect<_, TTo> Get<TTo>(IElement<T, TTo> parameter) => null;
	}

	sealed class StartWithBodyNode<_, T> : ISequenceNode<_, T>
	{
		readonly ISelect<_, T[]> _origin;
		readonly IBodyBuilder<T> _body;

		public StartWithBodyNode(ISelect<_, T[]> origin, IBodyBuilder<T> body)
		{
			_origin = origin;
			_body   = body;
		}

		public ISelect<_, T[]> Get() => new Exit<_, T>(_origin, _body.Get());

		public ISequenceNode<_, T> Get(IBodyBuilder<T> parameter)
			=> parameter is IEnterAware<T> enter
				   ? new StoreWithBodyNode<_, T>(new Enter<_, T>(Get(), enter.Enter),
				                                 new LinkedBodyBuilder<T>(_body, parameter))
				   : (ISequenceNode<_, T>)
				   new StartWithBodyNode<_, T>(_origin, new LinkedBodyBuilder<T>(_body, parameter));

		public ISequenceNode<_, TTo> Get<TTo>(IContents<T, TTo> parameter) => default;

		public ISelect<_, TTo> Get<TTo>(IElement<T, TTo> parameter) => null;
	}

	sealed class Enter<_, T> : ISelect<_, Storage<T>>
	{
		readonly ISelect<_, T[]> _origin;
		readonly IEnter<T>       _enter;

		public Enter(ISelect<_, T[]> origin) : this(origin, Enter<T>.Default) {}

		public Enter(ISelect<_, T[]> origin, IEnter<T> enter)
		{
			_origin = origin;
			_enter  = enter;
		}

		public Storage<T> Get(_ parameter) => _enter.Get(_origin.Get(parameter));
	}

	sealed class Exit<_, T> : ISelect<_, T[]>
	{
		readonly ISelect<_, T[]> _origin;
		readonly IEnter<T>       _enter;
		readonly IBody<T>        _body;

		public Exit(ISelect<_, T[]> origin, IBody<T> body) : this(origin, Enter<T>.Default, body) {}

		public Exit(ISelect<_, T[]> origin, IEnter<T> enter, IBody<T> body)
		{
			_origin = origin;
			_enter  = enter;
			_body   = body;
		}

		public T[] Get(_ parameter)
		{
			var storage = _enter.Get(_origin.Get(parameter));
			var result = _body.Get(new ArrayView<T>(storage.Instance, 0, storage.Length))
			                  .ToArray();
			return result;
		}
	}

	sealed class StoreWithBodyNode<_, T> : ISequenceNode<_, T>
	{
		readonly ISelect<_, Storage<T>> _origin;
		readonly IBodyBuilder<T>        _body;

		public StoreWithBodyNode(ISelect<_, Storage<T>> origin, IBodyBuilder<T> body)
		{
			_origin = origin;
			_body   = body;
		}

		public ISelect<_, T[]> Get() => null;

		public ISequenceNode<_, T> Get(IBodyBuilder<T> parameter) => null;

		public ISequenceNode<_, TTo> Get<TTo>(IContents<T, TTo> parameter)
			=> new ContentContainerNode<_, T, TTo>(_origin, new ContentContainer<T, TTo>(parameter, _body));

		public ISelect<_, TTo> Get<TTo>(IElement<T, TTo> parameter) => null;
	}

	public interface IContentContainer<TIn, TOut> : ISelect<IStores<TOut>, Assigned<uint>, IContent<TIn, TOut>> {}

	sealed class ContentContainer<TIn, TOut> : IContentContainer<TIn, TOut>
	{
		readonly IContents<TIn, TOut> _contents;
		readonly IBodyBuilder<TIn>    _body;

		public ContentContainer(IContents<TIn, TOut> contents) : this(contents, BodyBuilder<TIn>.Default) {}

		public ContentContainer(IContents<TIn, TOut> contents, IBodyBuilder<TIn> body)
		{
			_contents = contents;
			_body     = body;
		}

		public Func<Assigned<uint>, IContent<TIn, TOut>> Get(IStores<TOut> parameter)
			=> new SelectedContent<TIn, TOut>(_contents, _body, parameter).Get;
	}

	sealed class SelectedContent<TIn, TOut> : ISelectedContent<TIn, TOut>
	{
		readonly IContents<TIn, TOut> _contents;
		readonly IBodyBuilder<TIn>    _body;
		readonly IStores<TOut>        _stores;

		public SelectedContent(IContents<TIn, TOut> contents, IStores<TOut> stores)
			: this(contents, BodyBuilder<TIn>.Default, stores) {}

		public SelectedContent(IContents<TIn, TOut> contents, IBodyBuilder<TIn> body, IStores<TOut> stores)
		{
			_contents = contents;
			_body     = body;
			_stores   = stores;
		}

		public IContent<TIn, TOut> Get(Assigned<uint> parameter)
			=> _contents.Get(new Parameter<TIn, TOut>(_body.Get(parameter), _stores, parameter));
	}

	sealed class ContentContainerNode<_, TIn, TOut> : ISequenceNode<_, TOut>
	{
		readonly ISelect<_, Storage<TIn>>     _origin;
		readonly IContentContainer<TIn, TOut> _container;

		public ContentContainerNode(ISelect<_, Storage<TIn>> origin, IContents<TIn, TOut> contents)
			: this(origin, new ContentContainer<TIn, TOut>(contents)) {}

		public ContentContainerNode(ISelect<_, Storage<TIn>> origin, IContentContainer<TIn, TOut> container)
		{
			_origin    = origin;
			_container = container;
		}

		public ISelect<_, TOut[]> Get()
			=> new Exit<_, TIn, TOut>(_origin, _container.Get(DefaultStores<TOut>.Default).Invoke());

		public ISequenceNode<_, TOut> Get(IBodyBuilder<TOut> parameter) => null;

		public ISequenceNode<_, TTo> Get<TTo>(IContents<TOut, TTo> parameter)
		{
			var select    = _container.Get(Leases<TOut>.Default);
			var container = new LinkedContents<TIn, TOut, TTo>(select, parameter);
			var result    = new ContentContainerNode<_, TIn, TTo>(_origin, container);
			return result;
		}

		public ISelect<_, TTo> Get<TTo>(IElement<TOut, TTo> parameter)
		{
			var content = _container.Get(Leases<TOut>.Default)(parameter is ILimitAware limit ? limit.Get() : 2);
			var result  = new Exit<_, TIn, TOut, TTo>(_origin, content, parameter);
			return result;
		}
	}

	public interface ISelectedContent<TIn, TOut> : ISelect<Assigned<uint>, IContent<TIn, TOut>> {}

	sealed class LinkedContents<TIn, TOut, TTo> : IContentContainer<TIn, TTo>
	{
		readonly Func<Assigned<uint>, IContent<TIn, TOut>> _from;
		readonly IContents<TOut, TTo>                      _to;

		public LinkedContents(Func<Assigned<uint>, IContent<TIn, TOut>> from, IContents<TOut, TTo> to)
		{
			_from = from;
			_to   = to;
		}

		public Func<Assigned<uint>, IContent<TIn, TTo>> Get(IStores<TTo> parameter)
			=> new Next(_from, new SelectedContent<TOut, TTo>(_to, parameter).Get).Get;

		sealed class Next : ISelectedContent<TIn, TTo>
		{
			readonly Func<Assigned<uint>, IContent<TIn, TOut>> _from;
			readonly Func<Assigned<uint>, IContent<TOut, TTo>> _to;

			public Next(Func<Assigned<uint>, IContent<TIn, TOut>> from, Func<Assigned<uint>, IContent<TOut, TTo>> to)
			{
				_from = @from;
				_to   = to;
			}

			public IContent<TIn, TTo> Get(Assigned<uint> parameter) => new Content(_from(parameter), _to(parameter));

			sealed class Content : IContent<TIn, TTo>
			{
				readonly IContent<TIn, TOut> _previous;
				readonly IContent<TOut, TTo> _current;

				public Content(IContent<TIn, TOut> previous, IContent<TOut, TTo> current)
				{
					_previous = previous;
					_current  = current;
				}

				public Storage<TTo> Get(Storage<TIn> parameter) => _current.Get(_previous.Get(parameter));
			}
		}
	}

	sealed class LinkedBodyBuilder<T> : IBodyBuilder<T>
	{
		readonly IBodyBuilder<T> _previous, _current;

		public LinkedBodyBuilder(IBodyBuilder<T> previous, IBodyBuilder<T> current)
		{
			_previous = previous;
			_current  = current;
		}

		public IBody<T> Get(Assigned<uint> parameter)
			=> new LinkedBody<T>(_previous.Get(parameter), _current.Get(parameter));
	}

	sealed class LinkedBody<T> : IBody<T>
	{
		readonly IBody<T> _previous, _current;

		public LinkedBody(IBody<T> previous, IBody<T> current)
		{
			_previous = previous;
			_current  = current;
		}

		public ArrayView<T> Get(ArrayView<T> parameter) => _current.Get(_previous.Get(parameter));
	}

	sealed class FirstOrDefault<T> : Instance<uint>, IElement<T>, ILimitAware
	{
		public static FirstOrDefault<T> Default { get; } = new FirstOrDefault<T>();

		FirstOrDefault() : base(1) {}

		public T Get(Storage<T> parameter) => parameter.Length > 0 ? parameter.Instance[0] : default;
	}

	/*public class FirstWhere<T> : IElement<T>
	{
		readonly Func<T, bool> _where;
		readonly Func<T>       _default;

		public FirstWhere(ICondition<T> where) : this(where.Get) {}

		public FirstWhere(Func<T, bool> where) : this(where, () => default) {}

		public FirstWhere(Func<T, bool> where, Func<T> @default)
		{
			_where   = where;
			_default = @default;
		}

		public T Get(ArrayView<T> parameter)
		{
			var length = parameter.Length;

			for (var i = parameter.Start; i < length; i++)
			{
				var item = parameter.Array[i];
				if (_where(item))
				{
					return item;
				}
			}

			return _default();
		}
	}*/

	public interface ILimitAware : IResult<uint> {}

	public interface IElement<T> : IElement<T, T> {}

	public interface IElement<TFrom, out TTo> : ISelect<Storage<TFrom>, TTo> {}

	sealed class Exit<_, TIn, TOut, TTo> : ISelect<_, TTo>
	{
		readonly ISelect<_, Storage<TIn>> _origin;
		readonly IContent<TIn, TOut>      _content;
		readonly IElement<TOut, TTo>      _element;

		public Exit(ISelect<_, Storage<TIn>> origin, IContent<TIn, TOut> content,
		            IElement<TOut, TTo> element)
		{
			_origin  = origin;
			_content = content;
			_element = element;
		}

		public TTo Get(_ parameter)
		{
			var storage = _content.Get(_origin.Get(parameter));
			var result  = _element.Get(storage);

			// TODO: Optimize.
			if (storage.Requested)
			{
				Return<TOut>.Default.Execute(storage.Instance);
			}

			return result;
		}
	}

	public interface IStores<T> : ISelect<uint, Storage<T>> {}

	public sealed class DefaultStores<T> : Select<uint, Storage<T>>, IStores<T>
	{
		public static DefaultStores<T> Default { get; } = new DefaultStores<T>();

		DefaultStores() : base(x => new T[x]) {}
	}

	public class Stores<T> : Select<uint, Storage<T>>, IStores<T>
	{
		readonly ICommand<T[]> _return;

		public Stores(IStores<T> stores, ICommand<T[]> @return) : base(stores.Get) => _return = @return;

		public void Execute(T[] parameter)
		{
			_return.Execute(parameter);
		}
	}

	public sealed class Leases<T> : Stores<T>
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

	public interface IContents<TIn, TOut> : ISelect<Parameter<TIn, TOut>, IContent<TIn, TOut>> {}

	public interface IContent<TIn, TOut> : ISelect<Storage<TIn>, Storage<TOut>> {}

	public delegate IContent<TIn, TOut> Create<TIn, TOut, in TParameter>(IBody<TIn> body,
	                                                                     IStores<TOut> stores, TParameter parameter,
	                                                                     Assigned<uint> limit);

	sealed class Selection<TIn, TOut> : IContent<TIn, TOut>
	{
		readonly IBody<TIn>      _body;
		readonly Func<TIn, TOut> _select;
		readonly Assigned<uint>  _limit;
		readonly IStores<TOut>   _stores;

		// ReSharper disable once TooManyDependencies
		public Selection(IBody<TIn> body, IStores<TOut> stores, Func<TIn, TOut> select, Assigned<uint> limit)
		{
			_body   = body;
			_stores = stores;
			_select = select;
			_limit  = limit;
		}

		public Storage<TOut> Get(Storage<TIn> parameter)
		{
			var content = _body.Get(new ArrayView<TIn>(parameter.Instance, 0, parameter.Length));
			var length = _limit.IsAssigned
				             ? Math.Min(_limit.Instance, content.Length)
				             : content.Length;
			var result = _stores.Get(length);
			var @in    = content.Array;
			var @out   = result.Instance;

			for (var i = 0u; i < length; i++)
			{
				@out[i] = _select(@in[i]);
			}

			// TODO: Implement in Return decorator.
			if (parameter.Requested)
			{
				Return<TIn>.Default.Execute(parameter.Instance);
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

	public readonly struct Parameter<TIn, TOut>
	{
		public Parameter(IStores<TOut> stores) : this(stores, Assigned<uint>.Unassigned) {}

		public Parameter(IStores<TOut> stores, Assigned<uint> limit)
			: this(EmptyBody<TIn>.Default, stores, limit) {}

		public Parameter(IBody<TIn> body, IStores<TOut> stores, Assigned<uint> limit)
		{
			Body   = body;
			Stores = stores;
			Limit  = limit;
		}

		public IBody<TIn> Body { get; }

		public IStores<TOut> Stores { get; }

		public Assigned<uint> Limit { get; }
	}

	public interface IEnterAware<T>
	{
		IEnter<T> Enter { get; }
	}

	sealed class EmptyBody<T> : IBody<T>
	{
		public static EmptyBody<T> Default { get; } = new EmptyBody<T>();

		EmptyBody() {}

		public ArrayView<T> Get(ArrayView<T> parameter) => parameter;
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

		public IBody<T> Get(Assigned<uint> parameter) => _create(_argument, parameter);
	}

	public class Builder<TIn, TOut, TParameter> : IContents<TIn, TOut>
	{
		readonly Create<TIn, TOut, TParameter> _create;
		readonly TParameter                    _argument;

		public Builder(Create<TIn, TOut, TParameter> create, TParameter argument)
		{
			_create   = create;
			_argument = argument;
		}

		public IContent<TIn, TOut> Get(Parameter<TIn, TOut> parameter)
			=> _create(parameter.Body, parameter.Stores, _argument, parameter.Limit);
	}

	public static class Build
	{
		public sealed class Select<TIn, TOut> : Builder<TIn, TOut, Func<TIn, TOut>>
		{
			public Select(Func<TIn, TOut> argument)
				: base((shape, stores, parameter, limit) => new Selection<TIn, TOut>(shape, stores, parameter, limit),
				       argument) {}
		}

		public sealed class Where<T> : Builder<T, Func<T, bool>>, IEnterAware<T>
		{
			public Where(Func<T, bool> where) : this(where, Lease<T>.Default) {}

			public Where(Func<T, bool> where, IEnter<T> enter)
				: base((parameter, limit) => new Temp.Where<T>(parameter, limit), where) => Enter = enter;

			public IEnter<T> Enter { get; }
		}

		public sealed class Skip<T> : Builder<T, uint>
		{
			public Skip(uint count) : base((parameter, limit) => new Temp.Skip<T>(parameter, limit), count) {}
		}

		public sealed class Take<T> : Builder<T, uint>
		{
			public Take(uint count) : base((parameter, limit) => new Temp.Take<T>(parameter, limit), count) {}
		}
	}

	sealed class Where<T> : IBody<T>
	{
		readonly Func<T, bool>  _where;
		readonly Assigned<uint> _limit;

		public Where(Func<T, bool> where, Assigned<uint> limit)
		{
			_where = where;
			_limit = limit;
		}

		public ArrayView<T> Get(ArrayView<T> parameter)
		{
			var to    = parameter.Start + parameter.Length;
			var array = parameter.Array;
			var count = 0u;
			var limit = _limit.IsAssigned ? _limit.Instance : parameter.Length - parameter.Start;
			for (var i = parameter.Start; i < to && count < limit; i++)
			{
				var item = array[i];
				if (_where(item))
				{
					array[count++] = item;
				}
			}

			return new ArrayView<T>(parameter.Array, 0, count);
		}
	}

	sealed class Skip<T> : IBody<T>
	{
		readonly uint           _count;
		readonly Assigned<uint> _limit;

		public Skip(uint count, Assigned<uint> limit)
		{
			_count = count;
			_limit = limit;
		}

		public ArrayView<T> Get(ArrayView<T> parameter)
		{
			var length = Math.Max(0, (parameter.Length - (int)_count).Clamp0());
			var capped = _limit.IsAssigned ? Math.Min(length, _limit.Instance) : length;
			var result = new ArrayView<T>(parameter.Array, Math.Min(parameter.Length, parameter.Start + _count),
			                              capped);
			return result;
		}
	}

	sealed class Take<T> : IBody<T>
	{
		readonly uint           _count;
		readonly Assigned<uint> _limit;

		public Take(uint count, Assigned<uint> limit)
		{
			_count = count;
			_limit = limit;
		}

		public ArrayView<T> Get(ArrayView<T> parameter)
		{
			var count = _limit.IsAssigned ? Math.Min(_count, _limit) : _count;
			var result = new ArrayView<T>(parameter.Array, parameter.Start,
			                              Math.Min(parameter.Length - parameter.Start, count));
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

	sealed class Exit<_, TIn, TOut> : ISelect<_, TOut[]>
	{
		readonly ISelect<_, Storage<TIn>>       _origin;
		readonly IContent<TIn, TOut>            _content;
		readonly ISelect<Storage<TOut>, TOut[]> _complete;

		public Exit(ISelect<_, Storage<TIn>> origin, IContent<TIn, TOut> content)
			: this(origin, content, Complete<TOut>.Default) {}

		public Exit(ISelect<_, Storage<TIn>> origin, IContent<TIn, TOut> content,
		            ISelect<Storage<TOut>, TOut[]> complete)
		{
			_origin   = origin;
			_content  = content;
			_complete = complete;
		}

		public TOut[] Get(_ parameter) => _complete.Get(_content.Get(_origin.Get(parameter)));
	}
}