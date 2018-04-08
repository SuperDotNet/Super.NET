using System;
using Super.ExtensionMethods;
using Super.Model.Selection;
using Super.Runtime.Activation;

namespace Super.Model.Sources
{
	public class DelegatedSelection<TParameter, TResult> : ISource<TResult>
	{
		readonly static Func<TParameter>          New = Activation<TParameter>.Default.ToDelegate();
		readonly        Func<TParameter>          _parameter;
		readonly        Func<TParameter, TResult> _source;

		public DelegatedSelection(ISelect<TParameter, TResult> @select) : this(@select.ToDelegate(), New) {}

		public DelegatedSelection(Func<TParameter, TResult> source) : this(source, New) {}

		public DelegatedSelection(Func<TParameter, TResult> source, Func<TParameter> parameter)
		{
			_source    = source;
			_parameter = parameter;
		}

		public TResult Get() => _source(_parameter());
	}
}