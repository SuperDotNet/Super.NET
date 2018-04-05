using Super.Runtime.Activation;
using System;

namespace Super.Model.Specifications
{
	public class DelegatedSpecification<T> : ISpecification<T>
	{
		readonly Func<T, bool> _delegate;

		public DelegatedSpecification(Func<T, bool> @delegate) => _delegate = @delegate;

		public bool IsSatisfiedBy(T parameter) => _delegate(parameter);
	}

	public class FixedDelegatedSpecification<T> : ISpecification<T>, IActivateMarker<Func<bool>>
	{
		readonly Func<bool> _delegate;

		public FixedDelegatedSpecification(Func<bool> @delegate) => _delegate = @delegate;

		public bool IsSatisfiedBy(T _) => _delegate();
	}
}