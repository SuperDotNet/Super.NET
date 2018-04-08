using System.Reflection;
using Super.Model.Selection.Alterations;

namespace Super.Reflection
{
	public sealed class EnumerableInnerType : DecoratedAlteration<TypeInfo>
	{
		public static EnumerableInnerType Default { get; } = new EnumerableInnerType();

		EnumerableInnerType() : base(new InnerType(ImplementsGenericEnumerable.Default)) {}
	}
}