using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Super.ExtensionMethods;

namespace Super.Reflection.Types
{
	public sealed class AllInterfaces : IAllInterfaces
	{
		public static AllInterfaces Default { get; } = new AllInterfaces();

		AllInterfaces() => _selector = Yield;

		readonly Func<TypeInfo, IEnumerable<TypeInfo>> _selector;

		public ImmutableArray<TypeInfo> Get(TypeInfo parameter) => Yield(parameter).ToImmutableArray();

		IEnumerable<TypeInfo> Yield(TypeInfo parameter) =>
			parameter.Yield()
			         .Concat(parameter.ImplementedInterfaces.YieldMetadata()
			                          .SelectMany(_selector))
			         .Where(x => x.IsInterface)
			         .Distinct();
	}
}