using System;
using Super.ExtensionMethods;
using Super.Model.Sources;

namespace Super.Model.Commands
{
	sealed class CoercedCommand<TFrom, TTo> : ICommand<TFrom>
	{
		readonly Func<TFrom, TTo> _coercer;
		readonly Action<TTo>      _source;

		public CoercedCommand(ICommand<TTo> source, ISource<TFrom, TTo> coercer)
			: this(source.Execute, coercer.ToDelegate()) {}

		public CoercedCommand(Action<TTo> source, Func<TFrom, TTo> coercer)
		{
			_coercer = coercer;
			_source  = source;
		}

		public void Execute(TFrom parameter) => _source(_coercer(parameter));
	}
}