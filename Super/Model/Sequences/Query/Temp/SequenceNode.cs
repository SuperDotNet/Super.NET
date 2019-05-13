using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Runtime.Invocation;
using System;

namespace Super.Model.Sequences.Query.Temp
{
	public interface ISequenceNode<in _, T> : IResult<ISelect<_, T[]>>,
	                                          ISelect<IProjection<T>, ISequenceNode<_, T>>
	{

	}

	sealed class Start<_, T> : Instance<ISelect<_, T[]>>, ISequenceNode<_, T>
	{
		public Start(ISelect<_, T[]> start) : base(start) {}

		public ISequenceNode<_, T> Get(IProjection<T> parameter) => null;
	}

	sealed class SequenceNode<_, T> : ISequenceNode<_, T>
	{
		readonly ISelect<_, T[]> _start;
		readonly IProject<T>     _current;

		public SequenceNode(ISelect<_, T[]> start) : this(start, EmptyProject<T>.Default) {}

		public SequenceNode(ISelect<_, T[]> start, IProject<T> current)
		{
			_start   = start;
			_current = current;
		}

		public ISelect<_, T[]> Get() => _start.Select(Enter<T>.Default).Select(new Complete<T, T>(_current));

		public ISequenceNode<_, T> Get(IProjection<T> parameter)
			=> new SequenceNode<_, T>(_start, parameter.Get(_current));
	}

	public interface IEnter<T> : ISelect<T[], Store<T>> {}

	sealed class Enter<T> : IEnter<T>
	{
		public static Enter<T> Default { get; } = new Enter<T>();

		Enter() {}

		public Store<T> Get(T[] parameter) => new Store<T>(parameter);
	}

	public interface IProjection<T> : IAlteration<IProject<T>> {}

	sealed class EmptyProject<T> : IProject<T>
	{
		public static EmptyProject<T> Default { get; } = new EmptyProject<T>();

		EmptyProject() {}

		public ArrayView<T> Get(ArrayView<T> parameter) => parameter;
	}

	sealed class Skip<T> : Invocation0<IProject<T>, uint, IProject<T>>, IProjection<T>
	{
		public Skip(uint amount) : base((project, stored) => new Projection(project, stored), amount) {}

		sealed class Projection : IProject<T>
		{
			readonly IProject<T> _previous;
			readonly uint        _count;

			public Projection(IProject<T> previous, uint count)
			{
				_previous = previous;
				_count    = count;
			}

			public ArrayView<T> Get(ArrayView<T> parameter)
			{
				var previous = _previous.Get(parameter);

				var result = new ArrayView<T>(previous.Array, Math.Min(previous.Length, previous.Start + _count),
				                              Math.Max(previous.Start, (previous.Length - (int)_count).Clamp0()));
				return result;
			}
		}
	}

	sealed class Take<T> : Invocation0<IProject<T>, uint, IProject<T>>, IProjection<T>
	{
		public Take(uint amount) : base((project, stored) => new Projection(project, stored), amount) {}

		sealed class Projection : IProject<T>
		{
			readonly IProject<T> _previous;
			readonly uint        _count;

			public Projection(IProject<T> previous, uint count)
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
			var result = _project.Get(new ArrayView<TIn>(parameter.Instance, 0, parameter.Length())).ToArray();

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