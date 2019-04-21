using System;

namespace Super.Model.Selection.Conditions
{
	public class DelegatedCondition<T> : ICondition<T>
	{
		readonly Func<T, bool> _delegate;

		public DelegatedCondition(Func<T, bool> @delegate) => _delegate = @delegate;

		public bool Get(T parameter) => _delegate(parameter);
	}
}