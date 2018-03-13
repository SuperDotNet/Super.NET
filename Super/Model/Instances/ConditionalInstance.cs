using System;
using Super.Model.Specifications;

namespace Super.Model.Instances
{
	public class ConditionalInstance<T> : IInstance<T>
	{
		readonly Func<T>       _false;
		readonly Func<T>       _source;
		readonly Func<T, bool> _specification;

		public ConditionalInstance(ISpecification<T> specification, IInstance<T> instance, IInstance<T> @false)
			: this(specification.IsSatisfiedBy, instance.Get, @false.Get) {}

		public ConditionalInstance(Func<T, bool> specification, Func<T> source, Func<T> @false)
		{
			_specification = specification;
			_source        = source;
			_false         = @false;
		}

		public T Get()
		{
			var instance = _source();
			var result   = _specification(instance) ? instance : _false();
			return result;
		}
	}
}