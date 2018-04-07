using Super.ExtensionMethods;
using Super.Model.Specifications;
using System;

namespace Super.Model.Instances
{
	class ValidatedInstance<T> : IInstance<T>
	{
		readonly Func<T, bool> _specification;
		readonly Func<T> _instance, _fallback;

		public ValidatedInstance(ISpecification<T> specification, IInstance<T> instance, IInstance<T> fallback)
			: this(specification.ToDelegate(), instance.ToDelegate(), fallback.ToDelegate()) {}

		public ValidatedInstance(Func<T, bool> specification, Func<T> instance, Func<T> fallback)
		{
			_specification = specification;
			_instance = instance;
			_fallback = fallback;
		}

		public T Get()
		{
			var instance = _instance();
			var result = _specification(instance) ? instance : _fallback();
			return result;
		}
	}
}
