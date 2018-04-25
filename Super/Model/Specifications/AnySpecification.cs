using System;
using System.Collections.Immutable;
using System.Linq;

namespace Super.Model.Specifications
{
	public class AnySpecification<T> : ISpecification<T>
	{
		readonly ImmutableArray<Func<T, bool>> _specifications;

		public AnySpecification(params ISpecification<T>[] specifications) : this(specifications.Select(x => x.ToDelegate()).ToImmutableArray()) {}

		public AnySpecification(params Func<T, bool>[] specifications) : this(specifications.ToImmutableArray()) {}

		public AnySpecification(ImmutableArray<Func<T, bool>> specifications) => _specifications = specifications;

		public bool IsSatisfiedBy(T parameter)
		{
			var length = _specifications.Length;
			for (var i = 0; i < length; i++)
			{
				if (_specifications[i](parameter))
				{
					return true;
				}
			}

			return false;
		}
	}
}