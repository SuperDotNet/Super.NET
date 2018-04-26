using Super.Runtime.Activation;
using System;
using System.Reactive;

namespace Super.Model.Specifications
{
	public class DelegatedResultSpecification : ISpecification, IActivateMarker<Func<bool>>
	{
		readonly Func<bool> _delegate;

		public DelegatedResultSpecification(Func<bool> @delegate) => _delegate = @delegate;

		public bool IsSatisfiedBy(Unit parameter) => _delegate();
	}
}