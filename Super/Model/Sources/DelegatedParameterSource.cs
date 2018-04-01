using Super.ExtensionMethods;
using Super.Model.Instances;
using Super.Runtime.Activation;
using System;

namespace Super.Model.Sources
{
	public class DelegatedParameterSource<TParameter, TResult> : IInstance<TResult>
	{
		readonly static Func<TParameter>          New = New<TParameter>.Default.ToDelegate();
		readonly        Func<TParameter>          _parameter;
		readonly        Func<TParameter, TResult> _source;

		public DelegatedParameterSource(ISource<TParameter, TResult> source) : this(source.ToDelegate(), New) {}

		public DelegatedParameterSource(Func<TParameter, TResult> source) : this(source, New) {}

		public DelegatedParameterSource(Func<TParameter, TResult> source, Func<TParameter> parameter)
		{
			_source    = source;
			_parameter = parameter;
		}

		public TResult Get() => _source(_parameter());
	}
}