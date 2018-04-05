using Super.ExtensionMethods;
using System;

namespace Super.Model.Sources
{
	sealed class ConfiguringSource<TParameter, TResult> : ISource<TParameter, TResult>
	{
		readonly Action<TParameter, TResult> _configuration;
		readonly Func<TParameter, TResult>   _source;

		public ConfiguringSource(ISource<TParameter, TResult> source, IAssignable<TParameter, TResult> configuration)
			: this(source.ToDelegate(), configuration.Assign) {}

		public ConfiguringSource(Func<TParameter, TResult> source, Action<TParameter, TResult> configuration)
		{
			_source        = source;
			_configuration = configuration;
		}

		public TResult Get(TParameter parameter)
		{
			var result = _source(parameter);
			_configuration(parameter, result);
			return result;
		}
	}
}