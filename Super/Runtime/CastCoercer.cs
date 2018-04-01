using Super.Model.Sources;
using System;

namespace Super.Runtime
{
	sealed class CastCoercer<TFrom, TTo> : ISource<TFrom, TTo>
	{
		public static CastCoercer<TFrom, TTo> Default { get; } = new CastCoercer<TFrom, TTo>();

		CastCoercer() {}

		public TTo Get(TFrom parameter) => parameter is TTo to
			                                   ? to
			                                   : throw new
				                                     InvalidOperationException($"Could not cast {typeof(TFrom)} to {typeof(TTo)}");
	}
}