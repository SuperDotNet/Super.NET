using System.Collections.Generic;
using Super.Runtime.Activation;

namespace Super.Model.Specifications
{
	public class EqualitySpecification<T> : ISpecification<T>, IActivateMarker<T>
	{
		readonly IEqualityComparer<T> _comparer;

		readonly T _source;

		public EqualitySpecification(T source) : this(source, EqualityComparer<T>.Default) {}

		public EqualitySpecification(T source, IEqualityComparer<T> comparer)
		{
			_source   = source;
			_comparer = comparer;
		}

		public bool IsSatisfiedBy(T parameter) => _comparer.Equals(parameter, _source);
	}
}