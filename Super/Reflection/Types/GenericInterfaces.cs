using Super.Model.Collections;
using Super.Model.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Super.Reflection.Types
{
	public static partial class Implementations
	{
		public static ISelect<TypeInfo, ReadOnlyMemory<TypeInfo>> GenericInterfaces { get; } =
			Types.GenericInterfaces.Default.ToArray().ToStore();

	}

	sealed class GenericInterfaces : ISequence<TypeInfo, TypeInfo>
	{
		public static GenericInterfaces Default { get; } = new GenericInterfaces();

		GenericInterfaces() : this(AllInterfaces.Default) {}

		readonly ISequence<TypeInfo, TypeInfo> _sequence;

		public GenericInterfaces(ISequence<TypeInfo, TypeInfo> sequence) => _sequence = sequence;

		public IEnumerable<TypeInfo> Get(TypeInfo parameter) => _sequence.Get(parameter)
		                                                               .Where(x => x.IsGenericType)
		                                                               .Distinct();
	}
}