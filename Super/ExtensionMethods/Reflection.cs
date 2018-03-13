﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Super.Model.Sources;
using Super.Reflection;

namespace Super.ExtensionMethods
{
	public static class Reflection
	{
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

		public static T Get<T>(this ISource<Type, object> @this, I<T> parameter) => @this.Get(parameter.Get()).To<T>();

		public static T Get<T>(this ISource<TypeInfo, object> @this, I<T> parameter) => @this.Get(parameter.Get()).To<T>();

		public static T Get<T>(this ISource<Type, T> @this, I<T> parameter) => @this.Get(parameter.Get());

		public static T Get<T>(this ISource<TypeInfo, T> @this, I<T> parameter) => @this.Get(parameter.Get());

		public static T Get<T>(this ISource<TypeInfo, T> @this, Type parameter)
			=> @this.Get(parameter.GetTypeInfo());

		public static T Get<T>(this ISource<Type, T> @this, TypeInfo parameter)
			=> @this.Get(parameter.AsType());

		public static bool Has<T>(this ICustomAttributeProvider @this, bool inherit = false) where T : Attribute
			=> @this.IsDefined(Types<T>.Identity, inherit);
	}
}