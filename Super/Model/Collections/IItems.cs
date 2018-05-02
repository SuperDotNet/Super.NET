using Super.Model.Selection;
using Super.Model.Sources;
using Super.Model.Specifications;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;

namespace Super.Model.Collections
{
	public interface IArray<T> : ISource<Array<T>> {}

	public struct Array<T>
	{
		internal readonly T[] _source;

		public Array(T[] source) : this(source, source.Length) {}

		public Array(T[] source, int length)
		{
			_source = source;
			Length  = length;
		}

		public T this[int index] => _source[index];

		public int Length { get; }

		public Array<TTo> Select<TTo>(Converter<T, TTo> converter) => new Array<TTo>(Array.ConvertAll(_source, converter));
	}

	public interface IArray<TFrom, TTo> : ISelect<Array<TFrom>, Array<TTo>> {}

	sealed class ArraySelect<TFrom, TTo> : IArray<TFrom, TTo>
	{
		readonly Converter<TFrom, TTo> _select;

		public ArraySelect(ISelect<TFrom, TTo> select) : this(select.Get) {}

		public ArraySelect(Converter<TFrom, TTo> select) => _select = @select;

		public Array<TTo> Get(Array<TFrom> parameter)
		{
			/*var store  = new TTo[parameter.Length];
			var result = new Array<TTo>(store);
			for (var i = 0; i < store.Length; i++)
			{
				store[i] = _select(parameter[i]);
			}

			return result;*/
			return parameter.Select(_select);
		}
	}

	sealed class ArraySelectInline<TFrom, TTo> : IArray<TFrom, TTo>
	{
		readonly Action<TFrom[], TTo[], int, int> _iterate;

		public ArraySelectInline(Expression<Func<TFrom, TTo>> select)
			: this(InlineSelections<TFrom, TTo>.Default.Compile().Get(select)) {}

		public ArraySelectInline(Action<TFrom[], TTo[], int, int> iterate) => _iterate = iterate;

		public Array<TTo> Get(Array<TFrom> parameter)
		{
			var length = parameter.Length;
			var store  = new TTo[length];
			_iterate(parameter._source, store, 0, length);

			var result = new Array<TTo>(store);
			return result;
		}
	}

	sealed class InlineSelections<TFrom, TTo> : ISelect<Expression<Func<TFrom, TTo>>, Expression<Action<TFrom[], TTo[], int, int>>>
	{
		public static InlineSelections<TFrom, TTo> Default { get; } = new InlineSelections<TFrom, TTo>();

		InlineSelections() : this(Expression.Parameter(typeof(TFrom[]), "source"),
		                          Expression.Parameter(typeof(TTo[]), "destination"),
		                          Expression.Parameter(typeof(int), "start"),
		                          Expression.Parameter(typeof(int), "finish")) {}

		readonly ImmutableArray<ParameterExpression> _input;
		readonly ISelect<string, ParameterExpression> _parameters;

		public InlineSelections(params ParameterExpression[] parameters)
			: this(parameters.ToImmutableArray(), parameters.ToDictionary(x => x.Name).ToTable()) {}

		public InlineSelections(ImmutableArray<ParameterExpression> input, ISelect<string, ParameterExpression> parameters)
		{
			_input = input;
			_parameters = parameters;
		}

		public Expression<Action<TFrom[], TTo[], int, int>> Get(Expression<Func<TFrom, TTo>> parameter)
		{
			var label = Expression.Label();

			var index = Expression.Variable(typeof(int), "index");
			var assigned = Expression.ArrayAccess(_parameters.Get("source"), Expression.PostIncrementAssign(index));
			var inline = new InlineVisitor(parameter.Parameters[0], assigned).Visit(parameter.Body) ?? throw new InvalidOperationException("Inline expression was not found");


			var body = Expression.Block(index.Yield(),
			                            Expression.Assign(index, _parameters.Get("start")),
			                            Expression.Loop(Expression.IfThenElse(Expression.LessThan(index, _parameters.Get("finish")),
			                                                                  Expression.Assign(Expression.ArrayAccess(_parameters.Get("destination"), index),
			                                                                                    inline),
			                                                                  Expression.Break(label)
			                                                                 ),
			                                            label));

			var result = Expression.Lambda<Action<TFrom[], TTo[], int, int>>(body, _input.ToArray());
			return result;
		}
	}

	sealed class InlineVisitor : ExpressionVisitor
	{
		readonly Expression          _argument;
		readonly ParameterExpression _parameter;

		public InlineVisitor(ParameterExpression parameter, Expression argument)
		{
			_parameter = parameter;
			_argument  = argument;
		}

		protected override Expression VisitParameter(ParameterExpression node) => node == _parameter ? _argument : node;
	}

	/*class ArrayConvert<TFrom, TTo> : IArray<TFrom, TTo>
	{
		readonly Converter<TFrom, TTo> _select;

		/*public ArrayConvert(ISelect<TFrom, TTo> select) : this(select.Get) {}#1#

		public ArrayConvert(Converter<TFrom, TTo> select) => _select = select;

		public Array<TTo> Get(Array<TFrom> parameter) => parameter.Select(_select);
	}*/

	class ArrayWhere<TFrom, TTo> : IArray<TFrom, TTo>
	{
		readonly Func<TFrom, TTo> _select;
		readonly Func<TTo, bool>  _where;

		public ArrayWhere(ISelect<TFrom, TTo> select, ISpecification<TTo> where) : this(select.Get, where.IsSatisfiedBy) {}

		public ArrayWhere(Func<TFrom, TTo> select, Func<TTo, bool> where)
		{
			_select = @select;
			_where  = @where;
		}

		public Array<TTo> Get(Array<TFrom> parameter)
		{
			var store = new TTo[parameter.Length];
			//var valid = new Array<TTo>(store);
			var count = 0;
			for (var i = 0; i < store.Length; i++)
			{
				var element = _select(parameter[i]);
				if (_where(element))
				{
					store[count++] = element;
				}
			}

			var to     = new TTo[count];
			var result = new Array<TTo>(to);
			Array.Copy(store, to, count);
			return result;
		}
	}

	public interface IItems<out T> : ISource<IEnumerable<T>> {}

	/*sealed class Sequence<T> : IReadOnlyCollection<T>
	{
		readonly T[] _array;
		readonly int _length;

		public Sequence(T[] array, int length)
		{
			_array  = array;
			_length = length;
		}

		public IEnumerator<T> GetEnumerator()
		{
			for (var i = 0; i < _length; i++)
			{
				yield return _array[i];
			}
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public int Count => _array.Length;
	}*/
}