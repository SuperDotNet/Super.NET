using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using System;
using System.Buffers;

namespace Super.Model.Sequences.Query.Construction
{
	public interface INode<in _, T> : IResult<ISelect<_, T[]>>,
	                                          ISelect<IPartition, INode<_, T>>,
	                                          ISelect<IBodyBuilder<T>, INode<_, T>>
	{
		INode<_, TTo> Get<TTo>(IContents<T, TTo> parameter);

		ISelect<_, TTo> Get<TTo>(IElement<T, TTo> parameter);
	}

	sealed class Start<_, T> : Instance<ISelect<_, T[]>>, INode<_, T>
	{
		readonly IPartition<T> _partition;

		public Start(ISelect<_, T[]> origin) : this(origin, Partition<T>.Default) {}

		public Start(ISelect<_, T[]> origin, IPartition<T> partition) : base(origin) => _partition = partition;

		public INode<_, T> Get(IPartition parameter) => new Open<_, T>(Get(), _partition.Get(parameter));

		public INode<_, T> Get(IBodyBuilder<T> parameter)
			=> new Node<_, T>(new Enter<_, T>(Get(), Lease<T>.Default),
			                             new PartitionedBuilder<T>(_partition, parameter));

		public INode<_, TTo> Get<TTo>(IContents<T, TTo> parameter)
			=> new ContentNode<_, T, TTo>(new Enter<_, T>(Get()),
			                              new ContentContainer<T, TTo>(_partition, parameter));

		public ISelect<_, TTo> Get<TTo>(IElement<T, TTo> parameter)
			=> new Exit<_, T, T, TTo>(new Enter<_, T>(Get()),
			                          new Content<T>(_partition.Get(parameter is ILimitAware aware ? aware.Get() : 2)),
			                          parameter);
	}

	sealed class Open<_, T> : INode<_, T>
	{
		readonly ISelect<_, T[]> _origin;
		readonly IPartition<T>   _partition;

		public Open(ISelect<_, T[]> origin, IPartition<T> partition)
		{
			_origin    = origin;
			_partition = partition;
		}

		public ISelect<_, T[]> Get() => new Exit<_, T>(_origin, _partition.Get(Assigned<uint>.Unassigned));

		public INode<_, T> Get(IPartition parameter) => new Open<_, T>(_origin, _partition.Get(parameter));

		public INode<_, T> Get(IBodyBuilder<T> parameter)
			=> new Node<_, T>(new Enter<_, T>(_origin, Lease<T>.Default),
			                             new PartitionedBuilder<T>(_partition, parameter));

		public INode<_, TTo> Get<TTo>(IContents<T, TTo> parameter)
			=> new ContentNode<_, T, TTo>(new Enter<_, T>(_origin),
			                              new ContentContainer<T, TTo>(_partition, parameter));

		public ISelect<_, TTo> Get<TTo>(IElement<T, TTo> parameter)
			=> new Exit<_, T, T, TTo>(new Enter<_, T>(_origin),
			                          new Content<T>(_partition.Get(parameter is ILimitAware limit ? limit.Get() : 2)),
			                          parameter);
	}

	sealed class Node<_, T> : INode<_, T>
	{
		readonly ISelect<_, Store<T>> _origin;
		readonly IPartition<T>          _partition;

		public Node(ISelect<_, Store<T>> origin, IBodyBuilder<T> builder)
			: this(origin, new Partition<T>(builder)) {}

		public Node(ISelect<_, Store<T>> origin) : this(origin, Partition<T>.Default) {}

		public Node(ISelect<_, Store<T>> origin, IPartition<T> partition)
		{
			_origin    = origin;
			_partition = partition;
		}

		public ISelect<_, T[]> Get() => new Exit<_, T>(_origin, _partition.Get(Assigned<uint>.Unassigned));

		public INode<_, T> Get(IPartition parameter)
			=> new Node<_, T>(_origin, _partition.Get(parameter));

		public INode<_, T> Get(IBodyBuilder<T> parameter)
			=> new Node<_, T>(_origin, new PartitionedBuilder<T>(_partition, parameter));

		public INode<_, TTo> Get<TTo>(IContents<T, TTo> parameter)
			=> new ContentNode<_, T, TTo>(_origin, new ContentContainer<T, TTo>(_partition, parameter));

		public ISelect<_, TTo> Get<TTo>(IElement<T, TTo> parameter)
		{
			var content = new Content<T>(_partition.Get(parameter is ILimitAware limit ? limit.Get() : 2)).Returned();
			var result  = new Exit<_, T, T, TTo>(_origin, content, parameter);
			return result;
		}
	}

	sealed class Content<T> : IContent<T, T>
	{
		readonly IBody<T>   _body;
		readonly IStores<T> _stores;

		public Content(IBody<T> body) : this(body, Leases<T>.Default) {}

		public Content(IBody<T> body, IStores<T> stores)
		{
			_body   = body;
			_stores = stores;
		}

		public Store<T> Get(Store<T> parameter)
		{
			var view   = _body.Get(new ArrayView<T>(parameter.Instance, 0, parameter.Length));
			var result = view.Start > 0 || view.Length != parameter.Length ? view.ToStore(_stores) : view.Array;
			return result;
		}
	}

	public interface IEnter<T> : ISelect<T[], Store<T>> {}

	sealed class Enter<T> : Select<T[], Store<T>>, IEnter<T>
	{
		public static Enter<T> Default { get; } = new Enter<T>();

		Enter() : base(x => new Store<T>(x)) {}
	}

	sealed class Lease<T> : IEnter<T>
	{
		public static Lease<T> Default { get; } = new Lease<T>();

		Lease() : this(ArrayPool<T>.Shared) {}

		readonly ArrayPool<T> _pool;

		public Lease(ArrayPool<T> pool) => _pool = pool;

		public Store<T> Get(T[] parameter)
			=> new Store<T>(parameter.CopyInto(_pool.Rent(parameter.Length)), (uint)parameter.Length);
	}

	sealed class Enter<_, T> : ISelect<_, Store<T>>
	{
		readonly ISelect<_, T[]> _origin;
		readonly IEnter<T>       _enter;

		public Enter(ISelect<_, T[]> origin) : this(origin, Enter<T>.Default) {}

		public Enter(ISelect<_, T[]> origin, IEnter<T> enter)
		{
			_origin = origin;
			_enter  = enter;
		}

		public Store<T> Get(_ parameter) => _enter.Get(_origin.Get(parameter));
	}

	sealed class Exit<_, T> : ISelect<_, T[]>
	{
		readonly static Action<T[]>            Return = Return<T>.Default.Execute;
		readonly        ISelect<_, Store<T>> _origin;
		readonly        IBody<T>               _body;
		readonly        Action<T[]>            _return;

		public Exit(ISelect<_, T[]> origin, IBody<T> body) : this(origin, body, Return) {}

		public Exit(ISelect<_, T[]> origin, IBody<T> body, Action<T[]> @return)
			: this(new Origin<_, T>(origin), body, @return) {}

		public Exit(ISelect<_, Store<T>> origin, IBody<T> body) : this(origin, body, Return) {}

		public Exit(ISelect<_, Store<T>> origin, IBody<T> body, Action<T[]> @return)
		{
			_origin = origin;
			_body   = body;
			_return = @return;
		}

		public T[] Get(_ parameter)
		{
			var storage = _origin.Get(parameter);
			var result = _body.Get(new ArrayView<T>(storage.Instance, 0, storage.Length))
			                  .ToArray();

			if (storage.Requested)
			{
				_return(storage.Instance);
			}

			return result;
		}
	}

	sealed class Origin<_, T> : ISelect<_, Store<T>>
	{
		readonly ISelect<_, T[]> _origin;
		readonly IEnter<T>       _enter;

		public Origin(ISelect<_, T[]> origin) : this(origin, Enter<T>.Default) {}

		public Origin(ISelect<_, T[]> origin, IEnter<T> enter)
		{
			_origin = origin;
			_enter  = enter;
		}

		public Store<T> Get(_ parameter) => _enter.Get(_origin.Get(parameter));
	}

	public interface IContentContainer<TIn, TOut> : ISelect<IStores<TOut>, Assigned<uint>, IContent<TIn, TOut>> {}

	sealed class ContentContainer<TIn, TOut> : IContentContainer<TIn, TOut>
	{
		readonly IPartition<TIn>      _partition;
		readonly IContents<TIn, TOut> _contents;

		public ContentContainer(IPartition<TIn> partition, IContents<TIn, TOut> contents)
		{
			_contents  = contents;
			_partition = partition;
		}

		public Func<Assigned<uint>, IContent<TIn, TOut>> Get(IStores<TOut> parameter)
			=> new SelectedContent<TIn, TOut>(_partition, _contents, parameter).Get;
	}

	sealed class SelectedContent<TIn, TOut> : ISelectedContent<TIn, TOut>
	{
		readonly IContents<TIn, TOut> _contents;
		readonly IPartition<TIn>      _partition;
		readonly IStores<TOut>        _stores;

		public SelectedContent(IContents<TIn, TOut> contents, IStores<TOut> stores)
			: this(Partition<TIn>.Default, contents, stores) {}

		public SelectedContent(IPartition<TIn> partition, IContents<TIn, TOut> contents, IStores<TOut> stores)
		{
			_contents  = contents;
			_partition = partition;
			_stores    = stores;
		}

		public IContent<TIn, TOut> Get(Assigned<uint> parameter)
			=> _contents.Get(new Parameter<TIn, TOut>(_partition.Get(parameter), _stores, parameter));
	}

	sealed class ContentNode<_, TIn, TOut> : INode<_, TOut>
	{
		readonly ISelect<_, Store<TIn>>     _origin;
		readonly IContentContainer<TIn, TOut> _container;

		public ContentNode(ISelect<_, Store<TIn>> origin, IContentContainer<TIn, TOut> container)
		{
			_origin    = origin;
			_container = container;
		}

		public ISelect<_, TOut[]> Get() => new Exit<_, TIn, TOut>(_origin, _container.Get(Stores<TOut>.Default)
		                                                                             .Invoke()
		                                                                             .Returned());

		public INode<_, TOut> Get(IPartition parameter)
			=> new PartitionedContentNode<_, TIn, TOut>(_origin, _container, Partition<TOut>.Default.Get(parameter));

		public INode<_, TOut> Get(IBodyBuilder<TOut> parameter)
			=> new PartitionedContentNode<_, TIn, TOut>(_origin, _container, parameter);

		public INode<_, TTo> Get<TTo>(IContents<TOut, TTo> parameter)
		{
			var container = new LinkedContents<TIn, TOut, TTo>(_container.Get(Leases<TOut>.Default), parameter);
			var result    = new ContentNode<_, TIn, TTo>(_origin, container);
			return result;
		}

		public ISelect<_, TTo> Get<TTo>(IElement<TOut, TTo> parameter)
		{
			var content = _container.Get(Leases<TOut>.Default)(parameter is ILimitAware limit ? limit.Get() : 2)
			                        .Returned();
			var result = new Exit<_, TIn, TOut, TTo>(_origin, content, parameter);
			return result;
		}
	}

	sealed class PartitionedContentNode<_, TIn, TOut> : INode<_, TOut>
	{
		readonly ISelect<_, Store<TIn>>     _origin;
		readonly IContentContainer<TIn, TOut> _container;
		readonly IPartition<TOut>             _body;

		public PartitionedContentNode(ISelect<_, Store<TIn>> origin, IContentContainer<TIn, TOut> container,
		                              IBodyBuilder<TOut> builder)
			: this(origin, container, new Partition<TOut>(builder)) {}

		public PartitionedContentNode(ISelect<_, Store<TIn>> origin, IContentContainer<TIn, TOut> container)
			: this(origin, container, Partition<TOut>.Default) {}

		public PartitionedContentNode(ISelect<_, Store<TIn>> origin, IContentContainer<TIn, TOut> container,
		                              IPartition<TOut> body)
		{
			_origin    = origin;
			_container = container;
			_body      = body;
		}

		IContent<TIn, TOut> Content(Assigned<uint> limit)
			=> new Content<TIn, TOut>(_container.Get(Leases<TOut>.Default)(Assigned<uint>.Unassigned),
			                          _body.Get(limit)).Returned();

		public ISelect<_, TOut[]> Get() => new Exit<_, TIn, TOut>(_origin, Content(Assigned<uint>.Unassigned));

		public INode<_, TOut> Get(IPartition parameter)
			=> new PartitionedContentNode<_, TIn, TOut>(_origin, _container, _body.Get(parameter));

		public INode<_, TOut> Get(IBodyBuilder<TOut> parameter)
			=> new PartitionedContentNode<_, TIn, TOut>(_origin, _container,
			                                            new PartitionedBuilder<TOut>(_body, parameter));

		public INode<_, TTo> Get<TTo>(IContents<TOut, TTo> parameter)
		{
			var container = new LinkedContainer<TIn, TOut, TTo>(_container.Get(Leases<TOut>.Default),
			                                                    new ContentContainer<TOut, TTo>(_body, parameter));
			var result = new PartitionedContentNode<_, TIn, TTo>(_origin, container);
			return result;
		}

		public ISelect<_, TTo> Get<TTo>(IElement<TOut, TTo> parameter)
			=> new Exit<_, TIn, TOut, TTo>(_origin, Content(parameter is ILimitAware limit ? limit.Get() : 2),
			                               parameter);
	}

	sealed class Content<TIn, TOut> : IContent<TIn, TOut>
	{
		readonly IContent<TIn, TOut> _content;
		readonly IBody<TOut>         _body;
		readonly IStores<TOut>       _stores;

		public Content(IContent<TIn, TOut> content, IBody<TOut> body) : this(content, body, Stores<TOut>.Default) {}

		public Content(IContent<TIn, TOut> content, IBody<TOut> body, IStores<TOut> stores)
		{
			_content = content;
			_body    = body;
			_stores  = stores;
		}

		public Store<TOut> Get(Store<TIn> parameter)
		{
			var content = _content.Get(parameter);
			var view    = _body.Get(new ArrayView<TOut>(content.Instance, 0, content.Length));
			var result  = view.ToStore(_stores);
			return result;
		}
	}

	public interface ISelectedContent<TIn, TOut> : ISelect<Assigned<uint>, IContent<TIn, TOut>> {}

	sealed class LinkedContainer<TIn, TOut, TTo> : IContentContainer<TIn, TTo>
	{
		readonly Func<Assigned<uint>, IContent<TIn, TOut>> _from;
		readonly IContentContainer<TOut, TTo>              _to;

		public LinkedContainer(Func<Assigned<uint>, IContent<TIn, TOut>> from, IContentContainer<TOut, TTo> to)
		{
			_from = from;
			_to   = to;
		}

		public Func<Assigned<uint>, IContent<TIn, TTo>> Get(IStores<TTo> parameter)
			=> new Next<TIn, TOut, TTo>(_from, _to.Get(parameter)).Get;
	}

	sealed class Next<TIn, TOut, TTo> : ISelectedContent<TIn, TTo>
	{
		readonly Func<Assigned<uint>, IContent<TIn, TOut>> _from;
		readonly Func<Assigned<uint>, IContent<TOut, TTo>> _to;

		public Next(Func<Assigned<uint>, IContent<TIn, TOut>> from, Func<Assigned<uint>, IContent<TOut, TTo>> to)
		{
			_from = from;
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

			public Store<TTo> Get(Store<TIn> parameter) => _current.Get(_previous.Get(parameter));
		}
	}

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
			=> new Next<TIn, TOut, TTo>(_from, new SelectedContent<TOut, TTo>(_to, parameter).Get).Get;
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

	public interface IPartition : IAlteration<Selection> {}

	public readonly struct Partitioning
	{
		public Partitioning(Selection selection, Assigned<uint> limit)
		{
			Selection = selection;
			Limit     = limit;
		}

		public Selection Selection { get; }

		public Assigned<uint> Limit { get; }
	}

	public interface IPartition<T> : ISelect<Assigned<uint>, IBody<T>>, ISelect<IPartition, IPartition<T>> {}

	sealed class Partition<T> : IPartition<T>
	{
		public static Partition<T> Default { get; } = new Partition<T>();

		Partition() : this(BodyBuilder<T>.Default) {}

		readonly IBodyBuilder<T> _builder;
		readonly Selection       _selection;

		public Partition(IBodyBuilder<T> builder) : this(builder, Selection.Default) {}

		public Partition(IBodyBuilder<T> builder, Selection selection)
		{
			_builder   = builder;
			_selection = selection;
		}

		public IBody<T> Get(Assigned<uint> parameter) => _builder.Get(new Partitioning(_selection, parameter));

		public IPartition<T> Get(IPartition parameter) => new Partition<T>(_builder, parameter.Get(_selection));
	}

	sealed class Exit<_, TIn, TOut, TTo> : ISelect<_, TTo>
	{
		readonly ISelect<_, Store<TIn>> _origin;
		readonly IContent<TIn, TOut>      _content;
		readonly IElement<TOut, TTo>      _element;

		public Exit(ISelect<_, Store<TIn>> origin, IContent<TIn, TOut> content, IElement<TOut, TTo> element)
		{
			_origin  = origin;
			_content = content;
			_element = element;
		}

		public TTo Get(_ parameter) => _element.Get(_content.Get(_origin.Get(parameter)));
	}

	public sealed class ReturnedContents<TIn, TOut> : IContents<TIn, TOut>
	{
		readonly IContents<TIn, TOut> _contents;

		public ReturnedContents(IContents<TIn, TOut> projections) => _contents = projections;

		public IContent<TIn, TOut> Get(Parameter<TIn, TOut> parameter) => _contents.Get(parameter).Returned();
	}

	public interface IContents<TIn, TOut> : ISelect<Parameter<TIn, TOut>, IContent<TIn, TOut>> {}

	public delegate IContent<TIn, TOut> Create<TIn, TOut, in TParameter>(IBody<TIn> body,
	                                                                     IStores<TOut> stores, TParameter parameter,
	                                                                     Assigned<uint> limit);

	sealed class ReturnedContent<TIn, TOut> : IContent<TIn, TOut>
	{
		readonly IContent<TIn, TOut> _content;
		readonly Action<TIn[]>       _return;

		public ReturnedContent(IContent<TIn, TOut> content) : this(content, Return<TIn>.Default.Execute) {}

		public ReturnedContent(IContent<TIn, TOut> content, Action<TIn[]> @return)
		{
			_content = content;
			_return  = @return;
		}

		public Store<TOut> Get(Store<TIn> parameter)
		{
			var result = _content.Get(parameter);
			if (parameter.Requested)
			{
				_return(parameter.Instance);
			}

			return result;
		}
	}

	public readonly struct Parameter<TIn, TOut>
	{
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

	public interface IBodyBuilder<T> : ISelect<Partitioning, IBody<T>> {}

	sealed class PartitionedBuilder<T> : IBodyBuilder<T>
	{
		readonly IPartition<T>   _partition;
		readonly IBodyBuilder<T> _body;

		public PartitionedBuilder(IPartition<T> partition, IBodyBuilder<T> body)
		{
			_partition = partition;
			_body      = body;
		}

		public IBody<T> Get(Partitioning parameter)
			=> new LinkedBody<T>(_partition.Get(parameter.Limit), _body.Get(parameter));
	}

	sealed class Body<T> : IBody<T>
	{
		readonly Selection _selection;

		public Body(Selection selection) => _selection = selection;

		public ArrayView<T> Get(ArrayView<T> parameter)
			=> new ArrayView<T>(parameter.Array, _selection.Start,
			                    _selection.Length.IsAssigned
				                    ? Math.Min(parameter.Length - _selection.Start, _selection.Length)
				                    : parameter.Length - _selection.Start);
	}

	sealed class BodyBuilder<T> : Select<Partitioning, IBody<T>>, IBodyBuilder<T>
	{
		public static BodyBuilder<T> Default { get; } = new BodyBuilder<T>();

		BodyBuilder() : base(x => new Body<T>(x.Selection)) {}
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

	public class BodyBuilder<T, TParameter> : IBodyBuilder<T>
	{
		readonly Create<T, TParameter> _create;
		readonly TParameter            _parameter;

		public BodyBuilder(Create<T, TParameter> create, TParameter parameter)
		{
			_create    = create;
			_parameter = parameter;
		}

		public IBody<T> Get(Partitioning parameter) => _create(_parameter, parameter.Selection, parameter.Limit);
	}

	public delegate IBody<T> Create<T, in TParameter>(TParameter parameter, Selection selection, Assigned<uint> limit);

	sealed class Exit<_, TIn, TOut> : ISelect<_, TOut[]>
	{
		readonly ISelect<_, Store<TIn>>       _origin;
		readonly IContent<TIn, TOut>            _content;
		readonly ISelect<Store<TOut>, TOut[]> _complete;

		public Exit(ISelect<_, Store<TIn>> origin, IContent<TIn, TOut> content)
			: this(origin, content, Complete<TOut>.Default) {}

		public Exit(ISelect<_, Store<TIn>> origin, IContent<TIn, TOut> content,
		            ISelect<Store<TOut>, TOut[]> complete)
		{
			_origin   = origin;
			_content  = content;
			_complete = complete;
		}

		public TOut[] Get(_ parameter) => _complete.Get(_content.Get(_origin.Get(parameter)));
	}
}
