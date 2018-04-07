using Super.ExtensionMethods;
using Super.Model.Instances;
using Super.Runtime.Activation;
using System;

namespace Super.Model.Sources
{
	public class DelegatedSource<TParameter, TResult> : ISource<TParameter, TResult>,
	                                                    IActivateMarker<Func<TParameter, TResult>>
	{
		readonly Func<TParameter, TResult> _source;

		public DelegatedSource(Func<TParameter, TResult> source) => _source = source;

		public TResult Get(TParameter parameter) => _source(parameter);
	}

	/*public class Contained<TParameter, TResult> : ISource<TParameter, TResult>,
	                                              IActivateMarker<Func<ISource<TParameter, TResult>>>
	{
		readonly Func<ISource<TParameter, TResult>> _source;

		public Contained(IInstance<ISource<TParameter, TResult>> instance) : this(instance.ToDelegate()) {}

		public Contained(Func<ISource<TParameter, TResult>> source) => _source = source;

		public TResult Get(TParameter parameter) => _source().Get(parameter);
	}*/

	public class DelegatedInstanceSource<TParameter, TResult> : ISource<TParameter, TResult>,
	                                                            IActivateMarker<Func<Func<TParameter, TResult>>>
	{
		readonly Func<Func<TParameter, TResult>> _source;

		public DelegatedInstanceSource(IInstance<ISource<TParameter, TResult>> instance)
			: this(instance.Select(DelegateCoercer<TParameter, TResult>.Default)) {}

		public DelegatedInstanceSource(IInstance<Func<TParameter, TResult>> instance) : this(instance.ToDelegate()) {}

		public DelegatedInstanceSource(Func<Func<TParameter, TResult>> source) => _source = source;

		public TResult Get(TParameter parameter) => _source()(parameter);
	}
}