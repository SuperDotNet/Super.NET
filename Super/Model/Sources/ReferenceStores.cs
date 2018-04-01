using System;
using Super.Model.Sources.Tables;

namespace Super.Model.Sources
{
	public sealed class ReferenceStores<TParameter, TResult>
		: DelegatedSource<Func<TParameter, TResult>, ISource<TParameter, TResult>>
		where TParameter : class
	{
		public static ISource<Func<TParameter, TResult>, ISource<TParameter, TResult>> Default { get; } =
			new ReferenceStores<TParameter, TResult>();

		ReferenceStores() : base(ReferenceTables<TParameter, TResult>.Default.Get) {}
	}
}