using System.Reflection;
using Super.Model.Selection.Alterations;

namespace Super.Reflection.Collections
{
	public sealed class EnumerableInnerType : Alteration<TypeInfo>
	{
		public static EnumerableInnerType Default { get; } = new EnumerableInnerType();

		EnumerableInnerType() : base(new InnerType(ImplementsGenericEnumerable.Default)) {}
	}
}