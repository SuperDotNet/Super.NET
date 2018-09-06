using System;
using Super.Model.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Super.Model.Selection;

namespace Super.Reflection.Types
{
	public static partial class Implementations
	{
		public static ISelect<TypeInfo, ReadOnlyMemory<TypeInfo>> GenericInterfaces { get; } =
			Types.GenericInterfaces.Default.ToSequence().ToStore();

	}

	sealed class GenericInterfaces : IStream<TypeInfo, TypeInfo>
	{
		public static GenericInterfaces Default { get; } = new GenericInterfaces();

		GenericInterfaces() : this(AllInterfaces.Default) {}

		readonly IStream<TypeInfo, TypeInfo> _stream;

		public GenericInterfaces(IStream<TypeInfo, TypeInfo> stream) => _stream = stream;

		public IEnumerable<TypeInfo> Get(TypeInfo parameter) => _stream.Get(parameter)
		                                                               .Where(x => x.IsGenericType)
		                                                               .Distinct();
	}
}