using Super.AttachedProperties;

namespace Super.ExtensionMethods
{
	public static class AttachedProperties
	{
		public static TValue Get<THost, TValue>(this THost @this, IProperty<THost, TValue> property) => property.Get(@this);
	}
}