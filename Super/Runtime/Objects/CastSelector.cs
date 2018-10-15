using Super.Model.Selection;
using System;

namespace Super.Runtime.Objects
{
	sealed class CastSelector<TFrom, TTo> : ISelect<TFrom, TTo>
	{
		public static CastSelector<TFrom, TTo> Default { get; } = new CastSelector<TFrom, TTo>();

		CastSelector() {}

		public TTo Get(TFrom parameter)
			=> parameter is TTo to
				   ? to
				   : throw new
					     InvalidOperationException($"Could not cast {parameter?.GetType() ?? typeof(TFrom)} to {typeof(TTo)}");
	}
}