using System.Reflection;
using Super.Model.Sources;

namespace Super.Reflection
{
	public sealed class EnumerableInnerType : DecoratedAlteration<TypeInfo>
	{
		public static EnumerableInnerType Default { get; } = new EnumerableInnerType();

		EnumerableInnerType() : base(new InnerType(ImplementsGenericEnumerable.Default)) {}
	}
}