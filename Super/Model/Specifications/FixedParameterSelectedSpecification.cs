using System;

namespace Super.Model.Specifications
{
	public class FixedParameterSelectedSpecification<T, TParameter> : ISpecification<T>
	{
		readonly Func<T, Func<TParameter, bool>> _specification;
		readonly TParameter _parameter;

		public FixedParameterSelectedSpecification(Func<T, Func<TParameter, bool>> specification, TParameter parameter)
		{
			_specification = specification;
			_parameter = parameter;
		}

		public bool IsSatisfiedBy(T parameter) => _specification(parameter)(_parameter);
	}
}
