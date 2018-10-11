using Super.Model.Collections;
using Super.Model.Selection;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Super.Reflection.Types
{
	public abstract class GenericAdapterBase<T> : DecoratedSelect<ImmutableArray<Type>, T>
	{
		protected GenericAdapterBase(Type definition, ISelect<TypeInfo, T> select)
			: base(new Select<ImmutableArray<Type>, Array<Type>>(x => new Array<Type>(x.ToArray()))
			                               // TODO: Fix.
			                               .Select(new MakeGenericType(definition))
			                               .Select(TypeMetadata.Default)
			                               .Select(select)) {}
	}
}