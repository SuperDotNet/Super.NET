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
}