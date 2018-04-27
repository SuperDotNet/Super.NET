using Super.Model.Sources;
using Super.Runtime.Activation;
using System;

namespace Super.Model.Selection
{
	public class Select<TParameter, TResult> : ISelect<TParameter, TResult>,
	                                           IActivateMarker<Func<TParameter, TResult>>
	{
		readonly Func<TParameter, TResult> _source;

		public Select(Func<TParameter, TResult> source) => _source = source;

		public TResult Get(TParameter parameter) => _source(parameter);
	}

	public class DelegatedInstanceSelector<TParameter, TResult> : ISelect<TParameter, TResult>,
	                                                              IActivateMarker<ISource<ISelect<TParameter, TResult>>>,
	                                                              IActivateMarker<Func<Func<TParameter, TResult>>>
	{
		readonly Func<Func<TParameter, TResult>> _source;

		public DelegatedInstanceSelector(ISource<ISelect<TParameter, TResult>> source)
			: this(source.Exit(DelegateSelector<TParameter, TResult>.Default)) {}

		public DelegatedInstanceSelector(ISource<Func<TParameter, TResult>> source) : this(source.Get) {}

		public DelegatedInstanceSelector(Func<Func<TParameter, TResult>> source) => _source = source;

		public TResult Get(TParameter parameter) => _source()(parameter);
	}
}