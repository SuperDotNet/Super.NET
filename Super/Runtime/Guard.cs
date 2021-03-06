﻿using System;
using Super.Compose;
using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Selection.Conditions;

namespace Super.Runtime
{
	public class Guard<T, TException> : ICommand<T> where TException : Exception
	{
		readonly Func<T, bool>       _condition;
		readonly Func<T, TException> _exception;

		public Guard(ICondition<T> condition, ISelect<T, string> message) : this(condition.Get, message) {}

		public Guard(Func<T, bool> condition, ISelect<T, string> message)
			: this(condition, Start.A.Selection<string>()
			                       .AndOf<TException>()
			                       .By.Instantiation.To(message.Select)
			                       .Get) {}

		public Guard(Func<T, bool> condition, Func<T, TException> exception)
		{
			_condition = condition;
			_exception = exception;
		}

		public void Execute(T parameter)
		{
			if (_condition(parameter))
			{
				throw _exception(parameter);
			}
		}
	}
}