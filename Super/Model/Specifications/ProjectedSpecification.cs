using System;

namespace Super.Model.Specifications
{
	public sealed class ProjectedSpecification<TOrigin, TDestination> : ISpecification<TDestination>
	{
		readonly Func<TDestination, TOrigin> _coerce;
		readonly Func<TOrigin, bool>         _delegate;

		public ProjectedSpecification(Func<TOrigin, bool> @delegate, Func<TDestination, TOrigin> coerce)
		{
			_delegate = @delegate;
			_coerce   = coerce;
		}

		public bool IsSatisfiedBy(TDestination parameter) => _delegate(_coerce(parameter));
	}
}