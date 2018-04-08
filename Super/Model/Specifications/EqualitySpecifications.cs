using Super.Model.Selection.Stores;

namespace Super.Model.Specifications
{
	sealed class EqualitySpecifications<T> : Store<T, EqualitySpecification<T>>
	{
		public static EqualitySpecifications<T> Default { get; } = new EqualitySpecifications<T>();

		EqualitySpecifications() : base(x => new EqualitySpecification<T>(x)) {}
	}
}