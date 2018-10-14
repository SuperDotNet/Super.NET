using Super.Model.Sequences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Super.Reflection.Selection
{
	public sealed class Metadata : DeferredArray<MemberInfo>
	{
		public Metadata(IEnumerable<Type> types) : this(types.YieldMetadata(), PublicMembers.Default.Get) {}

		public Metadata(IEnumerable<TypeInfo> types, Func<TypeInfo, IEnumerable<MemberInfo>> select)
			: this(types.Fixed(), select) {}

		public Metadata(TypeInfo[] types, Func<TypeInfo, IEnumerable<MemberInfo>> select)
			: base(types.Concat(types.SelectMany(select)).Distinct()) {}
	}
}