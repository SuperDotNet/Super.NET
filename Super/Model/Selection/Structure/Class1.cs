using System;
using System.Runtime.CompilerServices;

namespace Super.Model.Selection.Structure
{
	public interface IStructure<TIn, out TOut>
	{
		TOut Get(in TIn parameter);
	}

	public interface IStructure<T> : IStructure<T, T> where T : struct {}

	sealed class Self<T> : IStructure<T> where T : struct
	{
		public static Self<T> Default { get; } = new Self<T>();

		Self() {}

		public T Get(in T parameter) => parameter;
	}

	class Structure<TIn, TFrom, TTo> : IStructure<TIn, TTo> where TIn : struct where TFrom : struct
	{
		readonly Selection<TIn, TFrom>  _source;
		readonly Selection<TFrom, TTo> _select;

		public Structure(Selection<TIn, TFrom> source, Selection<TFrom, TTo> select)
		{
			_select = select;
			_source = source;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TTo Get(in TIn parameter) => _select(_source(parameter));
	}

	class DelegatedStructure<TIn, TOut> : IStructure<TIn, TOut> where TIn : struct
	{
		readonly Selection<TIn, TOut> _select;

		public DelegatedStructure(Selection<TIn, TOut> select) => _select = select;

		public TOut Get(in TIn parameter) => _select(in parameter);
	}


	class StructureSelection<TIn, TFrom, TTo> : ISelect<TIn, TTo> where TFrom : struct
	{
		readonly Func<TIn, TFrom>      _source;
		readonly Selection<TFrom, TTo> _select;

		public StructureSelection(Func<TIn, TFrom> source, Selection<TFrom, TTo> select)
		{
			_select = select;
			_source = source;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TTo Get(TIn parameter) => _select(_source(parameter));
	}

	public delegate TOut Selection<TIn, out TOut>(in TIn parameter);
}
