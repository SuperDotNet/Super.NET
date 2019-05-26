using System.Reflection;
using Super.Compose;
using Super.Model.Selection;

namespace Super.Reflection
{
	sealed class AttributeProvider<T> : Select<T, ICustomAttributeProvider>
	{
		public static AttributeProvider<T> Default { get; } = new AttributeProvider<T>();

		AttributeProvider() : base(Start.A.Selection<T>()
		                                .By.Metadata
		                                .UnlessIsOf(Start.A.Selection<ICustomAttributeProvider>().By.Self)) {}
	}
}