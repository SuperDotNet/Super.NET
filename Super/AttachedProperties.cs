using Super.Model.AttachedProperties;

namespace Super
{
	// ReSharper disable once MismatchedFileName
	public static partial class ExtensionMethods
	{
		public static TValue Get<THost, TValue>(this THost @this, IProperty<THost, TValue> property) => property.Get(@this);
	}
}