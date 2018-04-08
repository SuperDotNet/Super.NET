using System;

namespace Super.Model.Selection.Stores
{
	public sealed class ReferenceStores<TParameter, TResult>
		: Delegated<Func<TParameter, TResult>, ISelect<TParameter, TResult>>
		where TParameter : class
	{
		public static ISelect<Func<TParameter, TResult>, ISelect<TParameter, TResult>> Default { get; } =
			new ReferenceStores<TParameter, TResult>();

		ReferenceStores() : base(ReferenceTables<TParameter, TResult>.Default.Get) {}
	}
}