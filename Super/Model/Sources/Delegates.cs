using System;
using Super.Model.Sources.Tables;

namespace Super.Model.Sources
{
	sealed class Delegates<TParameter, TResult>
		: DelegatedSource<ISource<TParameter, TResult>, Func<TParameter, TResult>>
	{
		public static Delegates<TParameter, TResult> Default { get; } = new Delegates<TParameter, TResult>();

		Delegates()
			: base(DefaultReferenceValueTables<ISource<TParameter, TResult>, Func<TParameter, TResult>>
			       .Default.Get(x => x.Get)
			       .Get) {}
	}
}