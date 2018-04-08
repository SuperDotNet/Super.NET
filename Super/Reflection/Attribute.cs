﻿using System;
using System.Reflection;
using Super.Model.Selection;
using Super.Model.Sources;

namespace Super.Reflection
{
	sealed class Attribute<TAttribute, T> : ISelect<MemberInfo, T> where TAttribute : Attribute, ISource<T>
	{
		public static Attribute<TAttribute, T> Default { get; } = new Attribute<TAttribute, T>();

		Attribute() {}

		public T Get(MemberInfo parameter) => parameter.GetCustomAttribute<TAttribute>()
		                                               .Get();
	}
}