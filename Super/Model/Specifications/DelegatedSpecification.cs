using System;

namespace Super.Model.Specifications
{
	public class DelegatedSpecification<T> : ISpecification<T>
	{
		readonly Func<T, bool> _delegate;

		public DelegatedSpecification(Func<T, bool> @delegate) => _delegate = @delegate;

		public bool IsSatisfiedBy(T parameter) => _delegate(parameter);
	}
}