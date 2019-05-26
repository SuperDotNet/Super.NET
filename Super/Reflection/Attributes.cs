using System;

namespace Super.Reflection
{
	sealed class Attributes<T> : AttributesStore<T> where T : Attribute
	{
		public static Attributes<T> Default { get; } = new Attributes<T>();

		Attributes() : base(IsDefined<T>.Inherited, ProvidedAttributes<T>.Inherited) {}
	}
}