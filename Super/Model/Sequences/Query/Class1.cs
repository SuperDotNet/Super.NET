using Super.Model.Selection;
using Super.Model.Specifications;
using Super.Runtime.Activation;
using Super.Runtime.Invocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Super.Model.Sequences.Query
{
	// TODO: Cull

	public sealed class AllItemsAre<T> : DelegatedSpecification<T[]>, IActivateMarker<Func<T, bool>>
	{
		public AllItemsAre(Func<T, bool> specification) : this(new Predicate<T>(specification)) {}

		public AllItemsAre(Predicate<T> specification)
			: base(new Invocation0<T[], Predicate<T>, bool>(Array.TrueForAll, specification).Get) {}
	}

	public sealed class OneItemIs<T> : DelegatedSpecification<T[]>, IActivateMarker<Func<T, bool>>
	{
		public OneItemIs(Func<T, bool> specification) : this(new Predicate<T>(specification)) {}

		public OneItemIs(Predicate<T> specification)
			: base(new Invocation0<T[], Predicate<T>, bool>(Array.Exists, specification).Get) {}
	}

	public sealed class ArraySelector<TFrom, TTo> : ISelector<TFrom, TTo>
	{
		readonly Func<TFrom, TTo> _select;

		public ArraySelector(ISelect<TFrom, TTo> select) : this(select.Get) {}

		public ArraySelector(Func<TFrom, TTo> select) => _select = select;

		public Array<TTo> Get(Array<TFrom> parameter)
		{
			var length = parameter.Length;
			var result = new TTo[length];

			for (var i = 0; i < length; i++)
			{
				result[i] = _select(parameter[i]);
			}

			return result;
		}
	}

	public class Where<TIn, TOut> : ISelect<TIn, Array<TOut>>
	{
		readonly ISelect<TIn, Array<TOut>> _select;
		readonly Func<TOut, bool>                   _where;

		public Where(ISelect<TIn, Array<TOut>> select, ISpecification<TOut> where)
			: this(select, where.IsSatisfiedBy) {}

		public Where(ISelect<TIn, Array<TOut>> select, Func<TOut, bool> where)
		{
			_select = @select;
			_where  = @where;
		}

		public Array<TOut> Get(TIn parameter)
		{
			var items  = _select.Get(parameter);
			var length = items.Length;

			Span<uint> indexes = stackalloc uint[(int)length];
			var       count   = 0;
			for (var i = 0u; i < length; i++)
			{
				var element = items[i];
				if (_where(element))
				{
					indexes[count++] = i;
				}
			}

			var result = new TOut[count];
			for (var i = 0; i < count; i++)
			{
				result[i] = items[indexes[i]];
			}

			//ArrayPool<TOut>.Shared.Return(items.Array());

			return result;
		}
	}

	public interface ISelector<TFrom, TTo> : ISelect<Array<TFrom>, Array<TTo>> {}

	public sealed class SelectWhere<TFrom, TTo> : ISelector<TFrom, TTo>
	{
		readonly Func<TFrom, TTo> _select;
		readonly Func<TTo, bool>  _where;

		public SelectWhere(ISelect<TFrom, TTo> select) : this(@select, Always<TTo>.Default) {}

		public SelectWhere(Func<TFrom, TTo> select, Expression<Func<TTo, bool>> specification)
			: this(@select, specification.Compile()) {}

		public SelectWhere(ISelect<TFrom, TTo> select, ISpecification<TTo> where) :
			this(select.Get, where.IsSatisfiedBy) {}

		SelectWhere(Func<TFrom, TTo> select, Func<TTo, bool> where)
		{
			_select = select;
			_where  = where;
		}

		public Array<TTo> Get(Array<TFrom> parameter)
		{
			var length = parameter.Length;

			var list = new List<TTo>();
			for (var i = 0; i < length; i++)
			{
				var element = _select(parameter[i]);
				if (_where(element))
				{
					list.Add(element);
				}
			}

			var result = list.ToArray();
			return result;
		}
	}

	public class Where<T> : ISelector<T, T>
	{
		readonly Func<T, bool> _where;

		public Where(ISpecification<T> where) : this(where.IsSatisfiedBy) {}

		public Where(Func<T, bool> where) => _where = where;

		public Array<T> Get(Array<T> parameter)
		{
			var length = parameter.Length;

			Span<int> indexes = stackalloc int[(int)length];
			var       count   = 0;
			for (var i = 0; i < length; i++)
			{
				var element = parameter[i];
				if (_where(element))
				{
					indexes[count++] = i;
				}
			}

			var result = new T[count];
			for (var i = 0; i < count; i++)
			{
				result[i] = parameter[indexes[i]];
			}

			return result;
		}
	}

	sealed class Skip : IAlterSelection
	{
		readonly uint _skip;

		public Skip(uint skip) => _skip = skip;

		public Selection Get(Selection parameter)
			=> new Selection(parameter.Start + _skip, parameter.Length);
	}

	sealed class Take : IAlterSelection
	{
		readonly uint _take;

		public Take(uint take) => _take = take;

		public Selection Get(Selection parameter)
			=> new Selection(parameter.Start, _take);
	}

	sealed class WhereSegment<T> : ISegment<T>
	{
		readonly Func<T, bool> _where;

		public WhereSegment(Func<T, bool> where) => _where = @where;

		public ArrayView<T> Get(ArrayView<T> parameter)
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

			return parameter.Resize(0, count);
		}
	}

	class SelectSelector<TFrom, TTo> : ISelect<IEnumerable<TFrom>, IEnumerable<TTo>>, IActivateMarker<Func<TFrom, TTo>>
	{
		readonly Func<TFrom, TTo> _select;

		public SelectSelector(Func<TFrom, TTo> select) => _select = select;

		public IEnumerable<TTo> Get(IEnumerable<TFrom> parameter) => parameter.Select(_select);
	}

	class SelectManySelector<TFrom, TTo> : ISelect<IEnumerable<TFrom>, IEnumerable<TTo>>,
	                                       IActivateMarker<Func<TFrom, IEnumerable<TTo>>>
	{
		readonly Func<TFrom, IEnumerable<TTo>> _select;

		public SelectManySelector(Func<TFrom, IEnumerable<TTo>> select) => _select = select;

		public IEnumerable<TTo> Get(IEnumerable<TFrom> parameter) => parameter.SelectMany(_select);
	}

	public sealed class Assigned<T> : Where<T> where T : class
	{
		public static Assigned<T> Default { get; } = new Assigned<T>();

		Assigned() : base(x => x != null) {}
	}

	/*public sealed class AssignedValue<T> : WhereSelector<T?> where T : struct
	{
		public static AssignedValue<T> Default { get; } = new AssignedValue<T>();

		AssignedValue() : base(x => x != null) {}
	}*/

	public interface IReduce<T> : ISelect<Array<T>, T> {}

	public sealed class FirstOrDefault<T> : FirstWhere<T>
	{
		public static FirstOrDefault<T> Default { get; } = new FirstOrDefault<T>();

		FirstOrDefault() : base(Always<T>.Default) {}
	}

	public sealed class FirstAssigned<T> : FirstWhere<T> where T : class
	{
		public static FirstAssigned<T> Default { get; } = new FirstAssigned<T>();

		FirstAssigned() : base(x => x != null) {}
	}

	// ReSharper disable once PossibleInfiniteInheritance
	public sealed class FirstAssignedValue<T> : FirstWhere<T?> where T : struct
	{
		public static FirstAssignedValue<T> Default { get; } = new FirstAssignedValue<T>();

		FirstAssignedValue() : base(x => x != null) {}
	}

	public class FirstWhere<T> : IReduce<T>
	{
		readonly Func<T, bool> _where;
		readonly Func<T>       _default;

		public FirstWhere(ISpecification<T> where) : this(where.IsSatisfiedBy) {}

		public FirstWhere(Func<T, bool> where) : this(@where, Sources.Default<T>.Instance.Get) {}

		public FirstWhere(Func<T, bool> where, Func<T> @default)
		{
			_where   = @where;
			_default = @default;
		}

		public T Get(Array<T> parameter)
		{
			var length = parameter.Length;

			for (var i = 0; i < length; i++)
			{
				var item = parameter[i];
				if (_where(item))
				{
					return item;
				}
			}

			return _default.Invoke();
		}
	}
}
