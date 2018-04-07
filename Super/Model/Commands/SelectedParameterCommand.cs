﻿using Super.ExtensionMethods;
using Super.Model.Sources;
using System;

namespace Super.Model.Commands
{
	sealed class SelectedParameterCommand<TFrom, TTo> : ICommand<TFrom>
	{
		readonly Func<TFrom, TTo> _select;
		readonly Action<TTo>      _source;

		public SelectedParameterCommand(ICommand<TTo> source, ISource<TFrom, TTo> select)
			: this(source.ToDelegate(), select.ToDelegate()) {}

		public SelectedParameterCommand(Action<TTo> source, Func<TFrom, TTo> select)
		{
			_select = @select;
			_source  = source;
		}

		public void Execute(TFrom parameter) => _source(_select(parameter));
	}
}