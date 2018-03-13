using System.Collections.Generic;

namespace Super.Model.Specifications
{
	public class ContainsSpecification<T> : DelegatedSpecification<T>
	{
		public ContainsSpecification(ICollection<T> source) : base(source.Contains) {}

		public sealed override bool IsSatisfiedBy(T parameter) => base.IsSatisfiedBy(parameter);
	}
}