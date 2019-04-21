using System;

namespace Super.Model.Selection.Conditions
{
	class GuardedCondition<T, TException> : ICondition<T> where TException : Exception
	{
		readonly Func<T, TException> _exception;
		readonly ICondition<T>       _condition;

		public GuardedCondition(ICondition<T> condition, Func<T, TException> exception)
		{
			_condition = condition;
			_exception = exception;
		}

		public bool Get(T parameter)
		{
			if (!_condition.Get(parameter))
			{
				throw _exception(parameter);
			}

			return true;
		}
	}
}