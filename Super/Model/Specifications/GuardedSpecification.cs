using System;

namespace Super.Model.Specifications
{
	class GuardedSpecification<T, TException> : ISpecification<T> where TException : Exception
	{
		readonly Func<T, TException> _exception;
		readonly ISpecification<T>   _specification;

		public GuardedSpecification(ISpecification<T> specification, Func<T, TException> exception)
		{
			_specification = specification;
			_exception     = exception;
		}

		public bool IsSatisfiedBy(T parameter)
		{
			if (!_specification.IsSatisfiedBy(parameter))
			{
				throw _exception(parameter);
			}

			return true;
		}
	}
}