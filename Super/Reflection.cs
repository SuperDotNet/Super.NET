using Super.Model.Selection;
using Super.Reflection;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Super
{
	public static partial class ExtensionMethods
	{
		public static T Attribute<T>(this ICustomAttributeProvider @this) => Reflection.Attribute<T>.Default.Get(@this);

		public static IEnumerable<TypeInfo> YieldMetadata(this IEnumerable<Type> @this,
		                                                  Func<TypeInfo, bool> specification = null)
		{
			var select = @this.Select(x => x.GetTypeInfo());
			var result = specification != null ? select.Where(specification) : select;
			return result;
		}

		public static ImmutableArray<TypeInfo> ToMetadata(this IEnumerable<Type> @this,
		                                                  Func<TypeInfo, bool> specification = null)
			=> @this.YieldMetadata(specification)
			        .ToImmutableArray();

		public static IEnumerable<Type> YieldTypes(this IEnumerable<TypeInfo> @this) => @this.Select(x => x.GetTypeInfo());

		public static ImmutableArray<Type> ToTypes(this IEnumerable<TypeInfo> @this)
			=> @this.YieldTypes()
			        .ToImmutableArray();

		public static T Get<T>(this ISelect<Type, object> @this, I<T> parameter) => @this.Get(parameter.Get()).To<T>();

		public static T Get<T>(this ISelect<TypeInfo, object> @this, I<T> parameter) => @this.Get(parameter.Get()).To<T>();

		public static T Get<T>(this ISelect<Type, T> @this, I<T> parameter) => @this.Get(parameter.Get());

		public static T Get<T>(this ISelect<TypeInfo, T> @this, I<T> parameter) => @this.Get(parameter.Get());

		public static T Get<T>(this ISelect<TypeInfo, T> @this, Type parameter)
			=> @this.Get(parameter.GetTypeInfo());

		public static T Get<T>(this ISelect<Type, T> @this, TypeInfo parameter)
			=> @this.Get(parameter.AsType());

		public static bool Has<T>(this ICustomAttributeProvider @this, bool inherit = false) where T : Attribute
			=> inherit.If(IsDefined<T>.Default, IsDefined<T>.Inherited).IsSatisfiedBy(@this);
	}
}