using System;
using Super.Model.Sources.Tables;

namespace Super.Model.Sources
{
	sealed class SourceDelegates<TParameter, TResult>
		: DelegatedSource<ISource<TParameter, TResult>, Func<TParameter, TResult>>
	{
		public static SourceDelegates<TParameter, TResult> Default { get; } = new SourceDelegates<TParameter, TResult>();

		SourceDelegates()
			: base(DefaultReferenceValueTables<ISource<TParameter, TResult>, Func<TParameter, TResult>>
			       .Default.Get(x => x.Get)
			       .Get) {}
	}
}