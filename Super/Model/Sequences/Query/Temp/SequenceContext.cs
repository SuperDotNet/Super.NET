namespace Super.Model.Sequences.Query.Temp
{
	sealed class SequenceContext {}

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