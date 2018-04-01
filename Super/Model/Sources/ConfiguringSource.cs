using System;
using Super.ExtensionMethods;
using Super.Model.Commands;

namespace Super.Model.Sources
{
	sealed class ConfiguringSource<TParameter, TResult> : ISource<TParameter, TResult>
	{
		readonly Action<(TParameter Parameter, TResult Result)> _configuration;
		readonly Func<TParameter, TResult>                      _source;

		public ConfiguringSource(ISource<TParameter, TResult> source,
		                         ICommand<(TParameter Parameter, TResult Result)> configuration)
			: this(source.ToDelegate(), configuration.Execute) {}

		public ConfiguringSource(Func<TParameter, TResult> source,
		                         Action<(TParameter Parameter, TResult Result)> configuration)
		{
			_source        = source;
			_configuration = configuration;
		}

		public TResult Get(TParameter parameter)
		{
			var result = _source(parameter);
			_configuration((parameter, result));
			return result;
		}
	}
}