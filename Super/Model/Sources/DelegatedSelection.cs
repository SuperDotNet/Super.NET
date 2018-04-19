using Super.Model.Selection;
using Super.Runtime.Activation;
using System;

namespace Super.Model.Sources
{
	public class DelegatedSelection<TParameter, TResult> : ISource<TResult>
	{
		readonly Func<TParameter>          _parameter;
		readonly Func<TParameter, TResult> _source;

		public DelegatedSelection(ISelect<TParameter, TResult> @select) : this(select, Activation<TParameter>.Default) {}

		public DelegatedSelection(ISelect<TParameter, TResult> @select, ISource<TParameter> parameter)
			: this(select.ToDelegate(), parameter.ToDelegate()) {}

		public DelegatedSelection(Func<TParameter, TResult> source, Func<TParameter> parameter)
		{
			_source    = source;
			_parameter = parameter;
		}

		public TResult Get() => _source(_parameter());
	}
}