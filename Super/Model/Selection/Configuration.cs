using Super.Model.Commands;
using System;
using System.Reactive;

namespace Super.Model.Selection
{
	sealed class Configuration<TParameter, TResult> : ISelect<TParameter, TResult>
	{
		readonly Action<TParameter, TResult> _configuration;
		readonly Func<TParameter, TResult>   _source;

		public Configuration(ISelect<TParameter, TResult> @select, IAssignable<TParameter, TResult> configuration)
			: this(select.Get, configuration.Assign) {}

		public Configuration(Func<TParameter, TResult> source, Action<TParameter, TResult> configuration)
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

	sealed class Configuration<T> : ISelect<T, Unit>
	{
		readonly Action<T> _action;

		public Configuration(Action<T> action) => _action = action;

		public Unit Get(T parameter)
		{
			_action(parameter);
			return Unit.Default;
		}
	}
}