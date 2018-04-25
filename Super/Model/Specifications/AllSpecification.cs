using System;
using System.Collections.Immutable;
using System.Linq;

namespace Super.Model.Specifications
{
	public class AllSpecification<T> : ISpecification<T>
	{
		readonly ImmutableArray<Func<T, bool>> _specifications;

		public AllSpecification(params ISpecification<T>[] specifications)
			: this(specifications.Select(x => x.ToDelegate()).ToImmutableArray()) {}

		public AllSpecification(params Func<T, bool>[] specifications) : this(specifications.ToImmutableArray()) {}

		public AllSpecification(ImmutableArray<Func<T, bool>> specifications)
			=> _specifications = specifications;

		public bool IsSatisfiedBy(T parameter)
		{
			var length = _specifications.Length;
			for (var i = 0; i < length; i++)
			{
				if (!_specifications[i](parameter))
				{
					return false;
				}
			}

			return true;
		}
	}
}