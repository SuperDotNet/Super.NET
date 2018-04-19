using Super.Model.AttachedProperties;

namespace Super
{
	public static partial class ExtensionMethods
	{
		public static TValue Get<THost, TValue>(this THost @this, IProperty<THost, TValue> property) => property.Get(@this);
	}
}