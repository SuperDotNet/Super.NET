using Super.Runtime.Activation;
using System;

namespace Super.Model.Sources
{
	public class DelegatedSource<TParameter, TResult>
		: ISource<TParameter, TResult>, IActivateMarker<Func<TParameter, TResult>>
	{
		readonly Func<TParameter, TResult> _source;

		public DelegatedSource(Func<TParameter, TResult> source) => _source = source;

		public TResult Get(TParameter parameter) => _source(parameter);
	}
}