using Super.Model.Results;
using Super.Model.Selection;
using Super.Reflection.Types;
using Super.Runtime.Invocation;
using System;
using System.Buffers;

namespace Super.Model.Sequences.Query.Construction
{
	public interface INode<in _, T> : IResult<ISelect<_, T[]>>,
	                                  ISelect<IProject<T>, INode<_, T>>,
	                                  ISelect<IDefinition<T>, INode<_, T>>,
	                                  ISelect<IElement<T>, ISelect<_, T>>
	{
		INode<_, TTo> Get<TTo>(IProjections<T, TTo> projections);

		ISelect<_, TTo> Get<TTo>(IElement<T, TTo> parameter);
	}

	public class Enter<_, T> : Instance<ISelect<_, T[]>>, INode<_, T>
	{
		readonly ISelect<_, T[]> _open;

		public Enter(ISelect<_, T[]> open) : base(open) => _open = open;

		public INode<_, T> Get(IProject<T> parameter) => new Open<_, T>(_open, new Definition<T>(parameter));

		public INode<_, T> Get(IDefinition<T> definition) => new Open<_, T>(_open, definition);

		public INode<_, TTo> Get<TTo>(IProjections<T, TTo> projections)
			=> new Node<_, T, TTo>(_open.Select(Enter<T>.Default), projections);

		public ISelect<_, T> Get(IElement<T> parameter)
			=> _open.Select(Leased<T>.Default).Select(new Element<T>(parameter));

		public ISelect<_, TTo> Get<TTo>(IElement<T, TTo> parameter)
			=> _open.Select(Leased<T>.Default).Select(new Element<T, TTo>(parameter));
	}

	public sealed class Open<_, T> : INode<_, T>
	{
		readonly ISelect<_, T[]> _input;
		readonly IEnter<T>       _enter;
		readonly IProject<T>     _content;

		public Open(ISelect<_, T[]> input, IDefinition<T> definition)
			: this(input, definition.Enter, definition.Get()) {}

		public Open(ISelect<_, T[]> input, IEnter<T> enter, IProject<T> content)
		{
			_input   = input;
			_enter   = enter;
			_content = content;
		}

		public INode<_, T> Get(IProject<T> parameter)
			=> new Open<_, T>(_input, _enter, new Select<T>(_content, parameter));

		public INode<_, T> Get(IDefinition<T> parameter)
			=> new Node<_, T>(_input.Select(parameter.Enter), new Select<T>(_content, parameter.Get()));

		public ISelect<_, T[]> Get() => _input.Select(_enter).Select(new Complete<T, T>(_content));

		public INode<_, TTo> Get<TTo>(IProjections<T, TTo> projections)
			=> new Node<_, T>(_input.Select(_enter), _content).Get(projections);

		public ISelect<_, T> Get(IElement<T> parameter)
			=> new Node<_, T>(_input.Select(Leased<T>.Default), _content).Get(parameter);

		public ISelect<_, TTo> Get<TTo>(IElement<T, TTo> parameter)
			=> new Node<_, T>(_input.Select(Leased<T>.Default), _content).Get(parameter);
	}

	public sealed class Store<_, T> : INode<_, T>
	{
		readonly ISelect<_, Store<T>> _input;

		public Store(ISelect<_, Store<T>> input) => _input = input;

		public ISelect<_, T[]> Get() => _input.Select(Complete<T>.Default);

		public INode<_, T> Get(IProject<T> parameter) => new Node<_, T>(_input, parameter);

		public INode<_, T> Get(IDefinition<T> parameter) => Get(parameter.Get());

		public INode<_, TTo> Get<TTo>(IProjections<T, TTo> projections) => new Node<_, T, TTo>(_input, projections);

		public ISelect<_, T> Get(IElement<T> parameter) => _input.Select(new Element<T>(parameter));

		public ISelect<_, TTo> Get<TTo>(IElement<T, TTo> parameter) => _input.Select(new Element<T, TTo>(parameter));
	}

	public sealed class Node<_, T> : INode<_, T>
	{
		readonly ISelect<_, Store<T>> _input;
		readonly IProject<T>          _content;

		public Node(ISelect<_, Store<T>> input, IProject<T> content)
		{
			_input   = input;
			_content = content;
		}

		public INode<_, T> Get(IProject<T> parameter) => new Node<_, T>(_input, new Select<T>(_content, parameter));

		public INode<_, T> Get(IDefinition<T> parameter) => Get(parameter.Get());

		public ISelect<_, T[]> Get() => _input.Select(new Complete<T, T>(_content));

		public INode<_, TTo> Get<TTo>(IProjections<T, TTo> projections)
			=> new Node<_, T, TTo>(_input.Select(new Continuation<T, T>(_content)), projections);

		public ISelect<_, T> Get(IElement<T> parameter) => _input.Select(new Element<T>(_content.Select(parameter)));

		public ISelect<_, TTo> Get<TTo>(IElement<T, TTo> parameter)
			=> _input.Select(new Element<T, TTo>(_content.Select(parameter)));
	}

	public sealed class Node<_, TIn, TOut> : INode<_, TOut>
	{
		readonly ISelect<_, Store<TIn>>  _input;
		readonly IProjections<TIn, TOut> _projections;

		public Node(ISelect<_, Store<TIn>> input, IProjections<TIn, TOut> projections)
		{
			_input       = input;
			_projections = projections;
		}

		public ISelect<_, TOut[]> Get() => _input.Select(_projections.Get(Stores<TOut>.Default))
		                                         .Select(Complete<TOut>.Default);

		public INode<_, TOut> Get(IProject<TOut> parameter)
			=> new Node<_, TOut>(_input.Select(_projections.Get(Allotted<TOut>.Default)), parameter);

		public INode<_, TOut> Get(IDefinition<TOut> parameter) => Get(parameter.Get());

		public INode<_, TTo> Get<TTo>(IProjections<TOut, TTo> projections)
			=> new Node<_, TOut, TTo>(_input.Select(_projections.Get(Allotted<TOut>.Default)), projections);

		public ISelect<_, TOut> Get(IElement<TOut> parameter) => _input.Select(_projections.Get(Allotted<TOut>.Default))
		                                                               .Select(new Element<TOut>(parameter));

		public ISelect<_, TTo> Get<TTo>(IElement<TOut, TTo> parameter)
			=> _input.Select(_projections.Get(Allotted<TOut>.Default))
			         .Select(new Element<TOut, TTo>(parameter));
	}

	public interface IDefinition<T> : IResult<IProject<T>>
	{
		IEnter<T> Enter { get; }
	}

	public class Definition<T> : Instance<IProject<T>>, IDefinition<T>
	{
		public Definition(IProject<T> content) : this(Enter<T>.Default, content) {}

		public Definition(IEnter<T> enter, IProject<T> content) : base(content) => Enter = enter;

		public IEnter<T> Enter { get; }
	}

	public sealed class ReturnedProjections<TIn, TOut> : IProjections<TIn, TOut>
	{
		readonly IProjections<TIn, TOut> _projections;

		public ReturnedProjections(IProjections<TIn, TOut> projections) => _projections = projections;

		public IContinuation<TIn, TOut> Get(IStores<TOut> parameter) => _projections.Get(parameter).Returned();
	}

	sealed class Select<T> : IProject<T>
	{
		readonly IProject<T> _first;
		readonly IProject<T> _second;

		public Select(IProject<T> first, IProject<T> second)
		{
			_first  = first;
			_second = second;
		}

		public ArrayView<T> Get(ArrayView<T> parameter) => _second.Get(_first.Get(parameter));
	}

	public interface IProjections<TIn, TOut> : ISelect<IStores<TOut>, IContinuation<TIn, TOut>> {}

	public class Continuations<T, TIn, TOut> : Invocation1<T, IStores<TOut>, IContinuation<TIn, TOut>>,
	                                           IProjections<TIn, TOut>
	{
		public Continuations(T parameter, Type definition) : this(parameter, definition, typeof(TIn), typeof(TOut)) {}

		public Continuations(T parameter, Type definition, params Type[] arguments)
			: this(parameter, definition, arguments.Result()) {}

		public Continuations(T parameter, Type definition, Array<Type> arguments)
			: base(new Generic<T, IStores<TOut>, IContinuation<TIn, TOut>>(definition).Get(arguments), parameter) {}
	}

	public sealed class ReturnedContinuation<TIn, TOut> : IContinuation<TIn, TOut>
	{
		readonly IContinuation<TIn, TOut> _continuation;
		readonly Action<TIn[]>            _return;

		public ReturnedContinuation(IContinuation<TIn, TOut> continuation)
			: this(continuation, Return<TIn>.Default.Execute) {}

		public ReturnedContinuation(IContinuation<TIn, TOut> continuation, Action<TIn[]> @return)
		{
			_continuation = continuation;
			_return       = @return;
		}

		public Store<TOut> Get(Store<TIn> parameter)
		{
			var result = _continuation.Get(parameter);
			if (parameter.Length.IsAssigned)
			{
				_return(parameter.Instance);
			}

			return result;
		}
	}

	public interface IEnter<T> : ISelect<T[], Store<T>> {}

	sealed class Enter<T> : Select<T[], Store<T>>, IEnter<T>
	{
		public static Enter<T> Default { get; } = new Enter<T>();

		Enter() : base(x => new Store<T>(x)) {}
	}

	sealed class Leased<T> : IEnter<T>
	{
		public static Leased<T> Default { get; } = new Leased<T>();

		Leased() : this(ArrayPool<T>.Shared) {}

		readonly ArrayPool<T> _pool;

		public Leased(ArrayPool<T> pool) => _pool = pool;

		public Store<T> Get(T[] parameter)
			=> new Store<T>(parameter.CopyInto(_pool.Rent(parameter.Length)), (uint)parameter.Length);
	}

	sealed class Continuation<TIn, TOut> : IContinuation<TIn, TOut>
	{
		readonly IProject<TIn, TOut> _project;
		readonly IStores<TOut>       _stores;
		readonly Action<TIn[]>       _return;

		public Continuation(IProject<TIn, TOut> project)
			: this(project, Allotted<TOut>.Default, Return<TIn>.Default.Execute) {}

		public Continuation(IProject<TIn, TOut> project, IStores<TOut> stores, Action<TIn[]> @return)
		{
			_project = project;
			_stores  = stores;
			_return  = @return;
		}

		public Store<TOut> Get(Store<TIn> parameter)
		{
			var input = new ArrayView<TIn>(parameter.Instance, 0, parameter.Length.Or((uint)parameter.Instance.Length));

			var view = _project.Get(input);

			var result = _stores.Get(view.Length);

			view.ToArray(result.Instance);

			if (parameter.Length.IsAssigned)
			{
				_return(parameter.Instance);
			}

			return result;
		}
	}

	sealed class Complete<TIn, TOut> : ISelect<Store<TIn>, TOut[]>
	{
		readonly IProject<TIn, TOut> _project;
		readonly Action<TIn[]>       _return;

		public Complete(IProject<TIn, TOut> project) : this(project, Return<TIn>.Default.Execute) {}

		public Complete(IProject<TIn, TOut> project, Action<TIn[]> @return)
		{
			_project = project;
			_return  = @return;
		}

		public TOut[] Get(Store<TIn> parameter)
		{
			var input = new ArrayView<TIn>(parameter.Instance, 0, parameter.Length.Or((uint)parameter.Instance.Length));

			var result = _project.Get(input).ToArray();

			if (parameter.Length.IsAssigned)
			{
				_return(parameter.Instance);
			}

			return result;
		}
	}

	sealed class Complete<T> : ISelect<Store<T>, T[]>
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
	}

	sealed class Element<T> : Element<T, T>
	{
		public Element(ISelect<ArrayView<T>, T> @select) : base(@select) {}
	}

	class Element<TIn, TOut> : ISelect<Store<TIn>, TOut>
	{
		readonly ISelect<ArrayView<TIn>, TOut> _select;
		readonly Action<TIn[]>                 _return;

		public Element(ISelect<ArrayView<TIn>, TOut> select) : this(@select, Return<TIn>.Default.Execute) {}

		public Element(ISelect<ArrayView<TIn>, TOut> select, Action<TIn[]> @return)
		{
			_select = @select;
			_return = @return;
		}

		public TOut Get(Store<TIn> parameter)
		{
			var @in    = parameter.Instance;
			var view   = new ArrayView<TIn>(parameter.Instance, 0, parameter.Length.Or((uint)@in.Length));
			var result = _select.Get(view);

			if (parameter.Length.IsAssigned)
			{
				_return(parameter.Instance);
			}

			return result;
		}
	}
}