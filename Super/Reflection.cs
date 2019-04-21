using Super.Model.Selection;
using Super.Reflection;
using System;
using System.Reflection;

namespace Super
{
	// ReSharper disable once MismatchedFileName

	public static partial class ExtensionMethods
	{
		public static T Attribute<T>(this ICustomAttributeProvider @this) where T : Attribute
			=> Reflection.Attribute<T>.Default.Get(@this);

		public static T Get<T>(this ISelect<TypeInfo, T> @this, Type parameter) => @this.Get(parameter.GetTypeInfo());

		public static bool Has<T>(this ICustomAttributeProvider @this, bool inherit = false) where T : Attribute
			=> inherit.If(IsDefined<T>.Default, IsDefined<T>.Inherited).IsSatisfiedBy(@this);
	}
}