using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Super.Model.Selection;
using Super.Model.Sources;

namespace Super.Reflection
{
	sealed class Attributes<TAttribute, T> : ISelect<MemberInfo, ImmutableArray<T>>
		where TAttribute : Attribute, ISource<T>
	{
		public static Attributes<TAttribute, T> Default { get; } = new Attributes<TAttribute, T>();

		Attributes() {}

		public ImmutableArray<T> Get(MemberInfo parameter) => parameter.GetCustomAttributes<TAttribute>()
		                                                               .Select(x => x.Get())
		                                                               .ToImmutableArray();
	}
}