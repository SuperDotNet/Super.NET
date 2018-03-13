using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Super.Model.Sources;

namespace Super.Reflection
{
	public abstract class GenericAdapterBase<T> : ISource<ImmutableArray<TypeInfo>, T>
	{
		readonly Func<ImmutableArray<Type>, Type> _generic;
		readonly Func<Type, TypeInfo>             _metadata;
		readonly Func<TypeInfo, T>                _source;
		readonly Func<TypeInfo, Type>             _type;

		protected GenericAdapterBase(Type definition, ISource<TypeInfo, T> source)
			: this(TypeCoercer.Default.Get, new GenericTypeAlteration(definition).Get, TypeMetadataCoercer.Default.Get,
			       source.Get) {}

		protected GenericAdapterBase(Func<TypeInfo, Type> type, Func<ImmutableArray<Type>, Type> generic,
		                             Func<Type, TypeInfo> metadata, Func<TypeInfo, T> source)
		{
			_type     = type;
			_generic  = generic;
			_metadata = metadata;
			_source   = source;
		}

		public T Get(ImmutableArray<TypeInfo> parameter)
			=> _source(_metadata(_generic(parameter.Select(_type).ToImmutableArray())));
	}
}