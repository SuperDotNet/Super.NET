using System;
using System.Runtime.CompilerServices;

namespace Super.Model.Sources.Tables
{
	sealed class DefaultReferenceValueTables<TParameter, TResult>
		: DelegatedSource<Func<TParameter, TResult>, ITable<TParameter, TResult>>
		where TParameter : class
		where TResult : class
	{
		public static DefaultReferenceValueTables<TParameter, TResult> Default { get; }
			= new DefaultReferenceValueTables<TParameter, TResult>();

		DefaultReferenceValueTables()
			: base(x => ReferenceValueTables<TParameter, TResult>.Defaults
			                                                     .Get(x)
			                                                     .Get(new ConditionalWeakTable<TParameter, TResult>())) {}
	}
}