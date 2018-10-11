using Super.Runtime.Activation;
using System;
using System.Reactive;

namespace Super.Model.Specifications
{
	public class DelegatedResultSpecification : DelegatedResultSpecification<Unit>, ISpecification
	{
		public DelegatedResultSpecification(Func<bool> @delegate) : base(@delegate) {}
	}

	public class DelegatedResultSpecification<T> : ISpecification<T>, IActivateMarker<Func<bool>>
	{
		readonly Func<bool> _delegate;

		public DelegatedResultSpecification(Func<bool> @delegate) => _delegate = @delegate;

		public bool IsSatisfiedBy(T _) => _delegate();
	}
}