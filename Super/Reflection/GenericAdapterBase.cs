using Super.ExtensionMethods;
using System;
using System.Collections.Immutable;
using System.Reflection;
using Super.Model.Selection;

namespace Super.Reflection
{
	public abstract class GenericAdapterBase<T> : Decorated<ImmutableArray<Type>, T>
	{
		protected GenericAdapterBase(Type definition, ISelect<TypeInfo, T> @select)
			: base(new GenericTypeAlteration(definition).Out(TypeMetadataSelector.Default)
			                                            .Out(@select)) {}
	}
}