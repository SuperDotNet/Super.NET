using Super.Runtime.Activation;
using System;
using System.Reactive;

namespace Super.Model.Specifications
{
	public class DelegatedSpecification<T> : ISpecification<T>
	{
		readonly Func<T, bool> _delegate;

		public DelegatedSpecification(Func<T, bool> @delegate) => _delegate = @delegate;

		public bool IsSatisfiedBy(T parameter) => _delegate(parameter);
	}

	public class DelegatedResultSpecification : ISpecification, IActivateMarker<Func<bool>>
	{
		readonly Func<bool> _delegate;

		public DelegatedResultSpecification(Func<bool> @delegate) => _delegate = @delegate;

		public bool IsSatisfiedBy(Unit parameter) => _delegate();
	}
}