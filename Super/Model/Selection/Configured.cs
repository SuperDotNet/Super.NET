using Super.Compose;
using Super.Model.Commands;
using System;

namespace Super.Model.Selection
{
	sealed class Configured<TIn, TOut> : ISelect<TIn, TOut>
	{
		readonly Action<TIn, TOut> _configuration;
		readonly Func<TIn, TOut>   _source;

		public Configured(ISelect<TIn, TOut> @select, IAssign<TIn, TOut> configuration)
			: this(select.Get, configuration.Assign) {}

		public Configured(Func<TIn, TOut> source, Action<TIn, TOut> configuration)
		{
			_source        = source;
			_configuration = configuration;
		}

		public TOut Get(TIn parameter)
		{
			var result = _source(parameter);
			_configuration(parameter, result);
			return result;
		}
	}

	sealed class Configured<TIn, TOut, TOther> : ISelect<TIn, TOut>
	{
		readonly Action<TIn, TOther> _configuration;
		readonly Func<TIn, TOut>     _source;
		readonly Func<TOut, TOther>  _other;

		public Configured(ISelect<TIn, TOut> @select, IAssign<TIn, TOther> configuration)
			: this(select, Start.A.Selection<TOut>().AndOf<TOther>().By.Cast, configuration) {}

		public Configured(ISelect<TIn, TOut> @select, ISelect<TOut, TOther> other,
		                  IAssign<TIn, TOther> configuration)
			: this(select.Get, other.Get, configuration.Assign) {}

		public Configured(Func<TIn, TOut> source, Func<TOut, TOther> other, Action<TIn, TOther> configuration)
		{
			_source        = source;
			_other         = other;
			_configuration = configuration;
		}

		public TOut Get(TIn parameter)
		{
			var result = _source(parameter);
			_configuration(parameter, _other(result));
			return result;
		}
	}
}