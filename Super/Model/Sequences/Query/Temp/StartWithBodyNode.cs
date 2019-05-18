using Super.Model.Commands;
using Super.Model.Results;
using Super.Model.Selection;
using System;
using System.Buffers;

namespace Super.Model.Sequences.Query.Temp
{
	public static class Extensions
	{
		public static IBody<T> Get<T>(this IBodyBuilder<T> @this) => @this.Get(SelfProjection<T>.Default);

		public static IBody<T> Get<T>(this IBodyBuilder<T> @this, IBody<T> previous) => @this.Get(previous.With());

		public static ISelect<Storage<T>, T[]> Select<T>(this IBody<T> @this) => new Complete<T, T>(@this);

		/*public static Parameter<TIn, TOut> With<TIn, TOut>(this IBody<TIn> @this, IStores<TOut> stores)
			=> @this.With(stores, Assigned<uint>.Unassigned);*/

		public static Parameter<TIn, TOut> With<TIn, TOut>(this IBody<TIn> @this, in DefinitionParameter<TOut> parameter)
			=> @this.With(parameter.Stores, parameter.Limit);

		public static Parameter<TIn, TOut> With<TIn, TOut>(this IBody<TIn> @this,
		                                                   IStores<TOut> stores, Assigned<uint> limit)
			=> new Parameter<TIn, TOut>(@this, stores, limit);

		public static Parameter<T> With<T>(this IBody<T> @this) => @this.With(Assigned<uint>.Unassigned);

		public static Parameter<T> With<T>(this IBody<T> @this, Assigned<uint> limit)
			=> new Parameter<T>(@this, limit);
	}

	public interface IBodyBuilder<T> : ISelect<Parameter<T>, IBody<T>> {}

	public delegate IBody<T> Create<T, in TParameter>(IBody<T> body, TParameter parameter, Assigned<uint> limit);

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
		public Start(ISelect<_, T[]> start) : base(start) {}

		public ISequenceNode<_, T> Get(IBodyBuilder<T> parameter) => new StartWithBodyNode<_, T>(Get(), parameter);

		public ISequenceNode<_, TTo> Get<TTo>(IContents<T, TTo> parameter)
		{
			var previous     = Get().Select(Enter<T>.Default);
			var continuation = new ContentContinuation<_, T, TTo>(previous, parameter);
			var result       = new ContentDefinitionNode<_, TTo>(continuation);
			return result;
		}

		public ISelect<_, TTo> Get<TTo>(IElement<T, TTo> parameter)
			=> Get().Select(Enter<T>.Default).Select(new Element<T, TTo>(parameter));
	}

	sealed class StartWithBodyNode<_, T> : ISequenceNode<_, T>
	{
		readonly ISelect<_, T[]> _origin;
		readonly IBodyBuilder<T> _current;

		public StartWithBodyNode(ISelect<_, T[]> origin, IBodyBuilder<T> current)
		{
			_origin  = origin;
			_current = current;
		}

		public ISelect<_, T[]> Get() => Select().Select(new Complete<T, T>(_current.Get()));

		public ISequenceNode<_, T> Get(IBodyBuilder<T> parameter)
		{
			var builder = new BodyBuilder<T>(_current, parameter);
			var result = parameter is IEnterAware<T> enter
				             ? Promote(enter, builder)
				             : (ISequenceNode<_, T>)new StartWithBodyNode<_, T>(_origin, builder);
			return result;
		}

		public ISequenceNode<_, TTo> Get<TTo>(IContents<T, TTo> parameter)
		{
			var definition   = new ContentDefinition<_, T>(Select());
			var body         = new BodyDefinition<T>(_current);
			var continuation = new ContentContinuation<_, T, TTo>(definition, parameter, body);
			var result       = new ContentDefinitionWithBodyNode<_, TTo>(continuation);
			return result;
		}

		// TODO: select body as well.
		public ISelect<_, TTo> Get<TTo>(IElement<T, TTo> parameter) => Select().Select(new Element<T, TTo>(parameter));

		ISelect<_, Storage<T>> Select()
			=> _origin.Select(_current is IEnterAware<T> enter ? enter.Enter : Enter<T>.Default);

		ContentDefinitionWithBodyNode<_, T> Promote(IEnterAware<T> enter, IBodyBuilder<T> builder)
		{
			var origin     = _origin.Select(enter.Enter);
			var definition = new ContentDefinition<_, T>(origin);
			var body       = new BodyDefinition<T>(builder);
			var result     = new ContentDefinitionWithBodyNode<_, T>(definition, body);
			return result;
		}
	}

	sealed class BodyBuilder<T> : IBodyBuilder<T>
	{
		readonly IBodyBuilder<T> _previous, _current;

		public BodyBuilder(IBodyBuilder<T> previous, IBodyBuilder<T> current)
		{
			_previous = previous;
			_current  = current;
		}

		public IBody<T> Get(Parameter<T> parameter) => new Body<T>(_previous.Get(parameter), _current.Get(parameter));
	}

	sealed class Body<T> : IBody<T>
	{
		readonly IBody<T> _previous;
		readonly IBody<T> _current;

		public Body(IBody<T> previous, IBody<T> current)
		{
			_previous = previous;
			_current  = current;
		}

		public ArrayView<T> Get(ArrayView<T> parameter) => _current.Get(_previous.Get(parameter));
	}

	/*sealed class ContentNode<_, T> : ISequenceNode<_, T>
	{
		readonly ISelect<_, Storage<T>> _origin;

		public ContentNode(ISelect<_, Storage<T>> start) => _origin = start;

		public ISelect<_, T[]> Get() => _origin.Select(Complete<T>.Default);

		public ISequenceNode<_, T> Get(IBodyBuilder<T> parameter)
			=> new ContentWithBodyNode<_, T>(_origin, parameter.Get());

		public ISequenceNode<_, TTo> Get<TTo>(IContents<T, TTo> parameter)
			=> new ContentDefinitionNode<_, TTo>(new ContentDefinition<_, T, TTo>(_origin, parameter));

		public ISelect<_, TTo> Get<TTo>(IElement<T, TTo> parameter) => null;
	}*/

	/*sealed class ContentWithBodyNode<_, T> : ISequenceNode<_, T>
	{
		readonly ISelect<_, Storage<T>> _origin;
		readonly IBody<T>               _current;

		public ContentWithBodyNode(ISelect<_, Storage<T>> origin, IBody<T> current)
		{
			_origin  = origin;
			_current = current;
		}

		public ISelect<_, T[]> Get() => _origin.Select(new Complete<T, T>(_current));

		public ISequenceNode<_, T> Get(IBodyBuilder<T> parameter)
			=> new ContentWithBodyNode<_, T>(_origin, parameter.Get(_current));

		public ISequenceNode<_, TTo> Get<TTo>(IContents<T, TTo> parameter)
			=> new ContentWithBodyNode<_, T, TTo>(_origin, parameter, _current);

		public ISelect<_, TTo> Get<TTo>(IElement<T, TTo> parameter) => null;
	}

	sealed class ContentWithBodyNode<_, TIn, TOut> : ISequenceNode<_, TOut>
	{
		readonly ISelect<_, Storage<TIn>> _origin;
		readonly IContents<TIn, TOut>     _contents;
		readonly IBody<TIn>               _body;

		public ContentWithBodyNode(ISelect<_, Storage<TIn>> origin, IContents<TIn, TOut> contents, IBody<TIn> body)
		{
			_origin   = origin;
			_contents = contents;
			_body     = body;
		}

		ISelect<_, Storage<TOut>> Close(IStores<TOut> stores) => _origin.Select(_contents.Get(_body.With(stores)));

		public ISelect<_, TOut[]> Get() => Close(Stores<TOut>.Default).Select(Complete<TOut>.Default);

		public ISequenceNode<_, TOut> Get(IBodyBuilder<TOut> parameter)
			=> new ContentWithBodyNode<_, TOut>(Close(Leases<TOut>.Default), parameter.Get());

		public ISequenceNode<_, TTo> Get<TTo>(IContents<TOut, TTo> parameter)
			=> new ContentNode<_, TOut>(Close(Leases<TOut>.Default)).Get(parameter);

		public ISelect<_, TTo> Get<TTo>(IElement<TOut, TTo> parameter) => null;
	}*/

	public interface IContentDefinition<in _, T> : ISelect<DefinitionParameter<T>, ISelect<_, Storage<T>>> {}

	sealed class ContentDefinition<_, T> : FixedResult<DefinitionParameter<T>, ISelect<_, Storage<T>>>,
	                                       IContentDefinition<_, T>
	{
		public ContentDefinition(ISelect<_, Storage<T>> select) : base(select) {}
	}

	public readonly struct DefinitionParameter<T>
	{
		public DefinitionParameter(Assigned<uint> limit) : this(Leases<T>.Default, limit) {}

		public DefinitionParameter(IStores<T> stores) : this(stores, Assigned<uint>.Unassigned) {}

		public DefinitionParameter(IStores<T> stores, Assigned<uint> limit)
		{
			Stores = stores;
			Limit  = limit;
		}

		public IStores<T> Stores { get; }

		public Assigned<uint> Limit { get; }
	}

	public interface IBodyDefinition<T> : ISelect<Assigned<uint>, IBody<T>> {}

	sealed class DefaultBodyDefinition<T> : FixedResult<Assigned<uint>, IBody<T>>, IBodyDefinition<T>
	{
		public static DefaultBodyDefinition<T> Default { get; } = new DefaultBodyDefinition<T>();

		DefaultBodyDefinition() : base(SelfProjection<T>.Default) {}
	}

	sealed class BodyDefinition<T> : IBodyDefinition<T>
	{
		readonly IBodyDefinition<T> _previous;
		readonly IBodyBuilder<T>    _current;

		public BodyDefinition(IBodyBuilder<T> current) : this(DefaultBodyDefinition<T>.Default, current) {}

		public BodyDefinition(IBodyDefinition<T> previous, IBodyBuilder<T> current)
		{
			_previous = previous;
			_current  = current;
		}

		public IBody<T> Get(Assigned<uint> parameter)
			=> _current.Get(new Parameter<T>(_previous.Get(parameter), parameter));
	}

	sealed class ContentContinuation<_, T> : IContentDefinition<_, T>
	{
		readonly IContentDefinition<_, T> _previous;
		readonly IBodyDefinition<T>       _body;

		public ContentContinuation(IContentDefinition<_, T> previous, IBodyDefinition<T> body)
		{
			_previous = previous;
			_body     = body;
		}

		public ISelect<_, Storage<T>> Get(DefinitionParameter<T> parameter)
			=> _previous.Get(parameter).Select(new Continue(_body.Get(parameter.Limit), parameter.Stores));

		sealed class Continue : IContent<T, T>
		{
			readonly ISelect<ArrayView<T>, ArrayView<T>> _body;
			readonly IStores<T>                          _stores;
			readonly Action<T[]>                         _return;

			public Continue(ISelect<ArrayView<T>, ArrayView<T>> body, IStores<T> stores)
				: this(body, stores, Return<T>.Default.Execute) {}

			public Continue(ISelect<ArrayView<T>, ArrayView<T>> body, IStores<T> stores, Action<T[]> @return)
			{
				_body   = body;
				_stores = stores;
				_return = @return;
			}

			public Storage<T> Get(Storage<T> parameter)
			{
				var view   = _body.Get(new ArrayView<T>(parameter.Instance, 0, parameter.Length));
				var result = _stores.Get(view.Length - view.Start);

				view.ToArray(result.Instance);

				if (parameter.Requested)
				{
					_return(parameter.Instance);
				}

				return result;
			}
		}
	}

	sealed class ContentContinuation<_, TIn, TOut> : IContentDefinition<_, TOut>
	{
		readonly IContentDefinition<_, TIn> _previous;
		readonly IContents<TIn, TOut>       _contents;
		readonly IBodyDefinition<TIn>       _body;

		public ContentContinuation(ISelect<_, Storage<TIn>> previous, IContents<TIn, TOut> contents)
			: this(new ContentDefinition<_, TIn>(previous), contents) {}

		public ContentContinuation(IContentDefinition<_, TIn> previous, IContents<TIn, TOut> contents)
			: this(previous, contents, DefaultBodyDefinition<TIn>.Default) {}

		public ContentContinuation(IContentDefinition<_, TIn> previous, IContents<TIn, TOut> contents,
		                           IBodyDefinition<TIn> body)
		{
			_previous = previous;
			_contents = contents;
			_body     = body;
		}

		public ISelect<_, Storage<TOut>> Get(DefinitionParameter<TOut> parameter)
			=> _previous.Get(new DefinitionParameter<TIn>(parameter.Limit))
			            .Select(_contents.Get(_body.Get(parameter.Limit).With(in parameter)));
	}

	sealed class ContentDefinitionNode<_, T> : ISequenceNode<_, T>
	{
		readonly IContentDefinition<_, T> _definition;

		public ContentDefinitionNode(IContentDefinition<_, T> definition) => _definition = definition;

		public ISelect<_, T[]> Get()
			=> _definition.Get(new DefinitionParameter<T>(Stores<T>.Default)).Select(Complete<T>.Default);

		public ISequenceNode<_, T> Get(IBodyBuilder<T> parameter)
			=> new ContentDefinitionWithBodyNode<_, T>(_definition, new BodyDefinition<T>(parameter));

		public ISequenceNode<_, TTo> Get<TTo>(IContents<T, TTo> parameter)
			=> new ContentDefinitionNode<_, TTo>(new ContentContinuation<_, T, TTo>(_definition, parameter));

		public ISelect<_, TTo> Get<TTo>(IElement<T, TTo> parameter)
			=> _definition.Get(new DefinitionParameter<T>(2)).Select(new Element<T, TTo>(parameter));
	}

	sealed class ContentDefinitionWithBodyNode<_, T> : ISequenceNode<_, T>
	{
		readonly IContentDefinition<_, T> _definition;
		readonly IBodyDefinition<T>       _body;

		public ContentDefinitionWithBodyNode(IContentDefinition<_, T> definition)
			: this(definition, DefaultBodyDefinition<T>.Default) {}

		public ContentDefinitionWithBodyNode(IContentDefinition<_, T> definition, IBodyDefinition<T> body)
		{
			_definition = definition;
			_body       = body;
		}

		public ISelect<_, T[]> Get()
			=> new ContentContinuation<_, T>(_definition, _body).Get(new DefinitionParameter<T>(Stores<T>.Default))
			                                                    .Select(Complete<T>.Default);

		public ISequenceNode<_, T> Get(IBodyBuilder<T> parameter)
			=> new ContentDefinitionWithBodyNode<_, T>(_definition, new BodyDefinition<T>(_body, parameter));

		public ISequenceNode<_, TTo> Get<TTo>(IContents<T, TTo> parameter)
			=> new ContentDefinitionWithBodyNode<_, TTo>(new ContentContinuation<_, T, TTo>(_definition, parameter,
			                                                                                _body));

		public ISelect<_, TTo> Get<TTo>(IElement<T, TTo> parameter)
			=> new ContentContinuation<_, T>(_definition, _body).Get(new DefinitionParameter<T>(2))
			                                                    .Select(new Element<T, TTo>(parameter));
	}

	class Element<TIn, TOut> : ISelect<Storage<TIn>, TOut>
	{
		readonly ISelect<ArrayView<TIn>, TOut> _select;
		readonly Action<TIn[]>                 _return;

		public Element(ISelect<ArrayView<TIn>, TOut> select) : this(select, Return<TIn>.Default.Execute) {}

		public Element(ISelect<ArrayView<TIn>, TOut> select, Action<TIn[]> @return)
		{
			_select = select;
			_return = @return;
		}

		public TOut Get(Storage<TIn> parameter)
		{
			var @in    = parameter.Instance;
			var result = _select.Get(new ArrayView<TIn>(@in, 0, parameter.Length));

			if (parameter.Requested)
			{
				_return(@in);
			}

			return result;
		}
	}

	public interface IStores<T> : ISelect<uint, Storage<T>> {}

	public sealed class Stores<T> : Select<uint, Storage<T>>, IStores<T>
	{
		public static Stores<T> Default { get; } = new Stores<T>();

		Stores() : base(x => new Storage<T>(new T[x])) {}
	}

	public class Storages<T> : Select<uint, Storage<T>>, IStores<T>
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

	public interface IContents<TIn, TOut> : ISelect<Parameter<TIn, TOut>, IContent<TIn, TOut>> {}

	public interface IContent<TIn, TOut> : ISelect<Storage<TIn>, Storage<TOut>> {}

	public delegate IContent<TIn, TOut> Create<TIn, TOut, in TParameter>(IBody<TIn> body,
	                                                                     IStores<TOut> stores, TParameter parameter,
	                                                                     Assigned<uint> limit);

	sealed class Content<TIn, TOut> : IContent<TIn, TOut>
	{
		readonly IBody<TIn>      _body;
		readonly Func<TIn, TOut> _select;
		readonly IStores<TOut>   _stores;
		readonly Assigned<uint>  _limit;

		// ReSharper disable once TooManyDependencies
		public Content(IBody<TIn> body, IStores<TOut> stores, Func<TIn, TOut> select, Assigned<uint> limit)
		{
			_body   = body;
			_stores = stores;
			_select = select;
			_limit  = limit;
		}

		public Storage<TOut> Get(Storage<TIn> parameter)
		{
			var content = _body.Get(new ArrayView<TIn>(parameter.Instance, 0, parameter.Length));
			var length  = _limit.IsAssigned ? Math.Min(_limit.Instance, content.Length) : content.Length;
			var result  = _stores.Get(length);
			var @in     = content.Array;
			var @out    = result.Instance;

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

	/*public interface IProjection<T> : IProjection<T, T> {}

	public interface IProjection<TIn, TOut> : ISelect<IShape<TIn>, IShape<TOut>> {}*/

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

	public readonly struct Parameter<T>
	{
		public Parameter(IBody<T> previous, Assigned<uint> limit)
		{
			Previous = previous;
			Limit    = limit;
		}

		public IBody<T> Previous { get; }
		public Assigned<uint> Limit { get; }
	}

	public interface IEnterAware<T>
	{
		IEnter<T> Enter { get; }
	}

	class EnterAware<T> : IEnterAware<T>
	{
		public EnterAware(IEnter<T> enter) => Enter = enter;

		public IEnter<T> Enter { get; }
	}

	sealed class SelfProjection<T> : IBody<T>
	{
		public static SelfProjection<T> Default { get; } = new SelfProjection<T>();

		SelfProjection() {}

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

		public IBody<T> Get(Parameter<T> parameter) => _create(parameter.Previous, _argument, parameter.Limit);
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
				: base((shape, stores, parameter, limit) => new Content<TIn, TOut>(shape, stores, parameter, limit),
				       argument) {}
		}

		public sealed class Where<T> : Builder<T, Func<T, bool>>, IEnterAware<T>
		{
			public Where(Func<T, bool> where) : this(where, Lease<T>.Default) {}

			public Where(Func<T, bool> where, IEnter<T> enter)
				: base((body, parameter, limit) => new Temp.Where<T>(body, parameter, limit), where) => Enter = enter;

			public IEnter<T> Enter { get; }
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
		readonly ISelect<ArrayView<T>, ArrayView<T>> _previous;
		readonly Func<T, bool>                       _where;
		readonly Assigned<uint>                      _limit;

		public Where(ISelect<ArrayView<T>, ArrayView<T>> previous, Func<T, bool> where, Assigned<uint> limit)
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
			var limit = _limit.IsAssigned ? _limit.Instance : view.Length - view.Start;
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
		readonly ISelect<ArrayView<T>, ArrayView<T>> _previous;
		readonly uint                                _count;
		readonly Assigned<uint>                      _limit;

		public Skip(ISelect<ArrayView<T>, ArrayView<T>> previous, uint count, Assigned<uint> limit)
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
		readonly ISelect<ArrayView<T>, ArrayView<T>> _previous;
		readonly uint                                _count;

		public Take(ISelect<ArrayView<T>, ArrayView<T>> previous, uint count)
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