using System;
using Super.Model.Selection;
using Super.Model.Sequences;
using Super.Runtime;

namespace Super.Reflection.Types
{
	public class OpenGeneric : Select<Type, Func<Array<Type>, Func<object>>>
	{
		public OpenGeneric(Type definition)
			: base(new ContainsGenericInterfaceGuard(definition).Then()
			                                                    .ToConfiguration()
			                                                    .Select(x => new Generic<object>(x).ToDelegate())) {}
	}
}