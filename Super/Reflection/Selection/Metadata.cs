using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Super.Model.Collections;

namespace Super.Reflection.Selection
{
	public sealed class Metadata : Enumerable<MemberInfo>
	{
		readonly Func<TypeInfo, IEnumerable<MemberInfo>> _select;
		readonly IEnumerable<TypeInfo>                   _types;

		public Metadata(IEnumerable<Type> types) : this(types, PublicMembers.Default.Get) {}

		public Metadata(IEnumerable<Type> types, Func<TypeInfo, IEnumerable<MemberInfo>> select) :
			this(types.YieldMetadata(), select) {}

		public Metadata(IEnumerable<TypeInfo> types) : this(types, PublicMembers.Default.Get) {}

		public Metadata(IEnumerable<TypeInfo> types, Func<TypeInfo, IEnumerable<MemberInfo>> select)
		{
			_types  = types;
			_select = select;
		}

		public override IEnumerator<MemberInfo> GetEnumerator() => _types.Concat(_types.SelectMany(_select))
		                                                                 .Distinct()
		                                                                 .GetEnumerator();
	}
}