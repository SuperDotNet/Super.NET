using Super.Model.Specifications;

namespace Super.Runtime
{
	sealed class AlteredValueSpecification<T> : InverseSpecification<T>
	{
		public static AlteredValueSpecification<T> Default { get; } = new AlteredValueSpecification<T>();

		AlteredValueSpecification() : base(DefaultValueSpecification<T>.Default) {}
	}
}