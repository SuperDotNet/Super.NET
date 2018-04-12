using System;
using Super.Model.Selection;

namespace Super.Runtime.Objects
{
	sealed class CastSelector<TFrom, TTo> : ISelect<TFrom, TTo>
	{
		public static CastSelector<TFrom, TTo> Default { get; } = new CastSelector<TFrom, TTo>();

		CastSelector() {}

		public TTo Get(TFrom parameter) => parameter is TTo to
			                                   ? to
			                                   : throw new
				                                     InvalidOperationException($"Could not cast {typeof(TFrom)} to {typeof(TTo)}");
	}
}