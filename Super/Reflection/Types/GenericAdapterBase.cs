using System;
using System.Collections.Immutable;
using System.Reflection;
using Super.Model.Selection;

namespace Super.Reflection.Types
{
	public abstract class GenericAdapterBase<T> : DecoratedSelect<ImmutableArray<Type>, T>
	{
		protected GenericAdapterBase(Type definition, ISelect<TypeInfo, T> select)
			: base(new GenericTypeAlteration(definition).Select(TypeMetadataSelector.Default)
			                                            .Select(select)) {}
	}
}