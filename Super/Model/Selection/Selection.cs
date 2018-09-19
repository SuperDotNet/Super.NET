using System;
using System.Runtime.CompilerServices;

namespace Super.Model.Selection
{
	sealed class Selection<TParameter, TFrom, TTo> : ISelect<TParameter, TTo>
	{
		readonly Func<TFrom, TTo>        _select;
		readonly Func<TParameter, TFrom> _source;

		public Selection(Func<TParameter, TFrom> source, Func<TFrom, TTo> @select)
		{
			_select = @select;
			_source = source;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TTo Get(TParameter parameter) => _select(_source(parameter));
	}

	class EnhancedSelection<TIn, TFrom, TTo> : ISelect<TIn, TTo> where TFrom : struct
	{
		readonly Func<TIn, TFrom>      _source;
		readonly Selection<TFrom, TTo> _select;

		public EnhancedSelection(Func<TIn, TFrom> source, Selection<TFrom, TTo> select)
		{
			_select = select;
			_source = source;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TTo Get(TIn parameter) => _select(_source(parameter));
	}

	public delegate TOut Selection<TIn, out TOut>(in TIn parameter);
}