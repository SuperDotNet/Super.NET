using Super.ExtensionMethods;
using Super.Model.Sources;
using System;
using System.Collections.Immutable;
using System.Reflection;

namespace Super.Reflection
{
	public abstract class GenericAdapterBase<T> : DecoratedSource<ImmutableArray<Type>, T>
	{
		protected GenericAdapterBase(Type definition, ISource<TypeInfo, T> source)
			: base(new GenericTypeAlteration(definition).Out(TypeMetadataCoercer.Default)
			                                            .Out(source)) {}
	}
}