using System;
using Super.ExtensionMethods;
using Super.Model.Sources;
using Super.Runtime.Activation;

namespace Super.Model.Selection
{
	public class Delegated<TParameter, TResult> : ISelect<TParameter, TResult>,
	                                                    IActivateMarker<Func<TParameter, TResult>>
	{
		readonly Func<TParameter, TResult> _source;

		public Delegated(Func<TParameter, TResult> source) => _source = source;

		public TResult Get(TParameter parameter) => _source(parameter);
	}

	public class DelegatedInstanceSelector<TParameter, TResult> : ISelect<TParameter, TResult>,
	                                                            IActivateMarker<Func<Func<TParameter, TResult>>>
	{
		readonly Func<Func<TParameter, TResult>> _source;

		public DelegatedInstanceSelector(ISource<ISelect<TParameter, TResult>> source)
			: this(source.Select(DelegateSelector<TParameter, TResult>.Default)) {}

		public DelegatedInstanceSelector(ISource<Func<TParameter, TResult>> source) : this(source.ToDelegate()) {}

		public DelegatedInstanceSelector(Func<Func<TParameter, TResult>> source) => _source = source;

		public TResult Get(TParameter parameter) => _source()(parameter);
	}
}