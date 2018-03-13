using System.Collections.Immutable;

namespace Super.Model.Specifications
{
	public class AnySpecification<T> : ISpecification<T>
	{
		readonly ImmutableArray<ISpecification<T>> _specifications;

		public AnySpecification(params ISpecification<T>[] specifications) : this(specifications.ToImmutableArray()) {}

		public AnySpecification(ImmutableArray<ISpecification<T>> specifications) => _specifications = specifications;

		public bool IsSatisfiedBy(T parameter)
		{
			var length = _specifications.Length;
			for (var i = 0; i < length; i++)
			{
				if (_specifications[i].IsSatisfiedBy(parameter))
				{
					return true;
				}
			}

			return false;
		}
	}
}