using JetBrains.Annotations;
using Super.Model.Collections;
using Super.Runtime.Activation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Super.Reflection.Types
{
	public static partial class Implementations
	{
		public static IArray<TypeInfo, TypeInfo> AllInterfaces { get; } = Types.AllInterfaces.Default.ToArray().ToStore();
	}

	sealed class AllInterfaces : ISequence<TypeInfo, TypeInfo>
	{
		public static AllInterfaces Default { get; } = new AllInterfaces();

		AllInterfaces() : this(Defaults.Selector) {}

		readonly Func<TypeInfo, IEnumerable<TypeInfo>> _selector;

		public AllInterfaces(Func<TypeInfo, IEnumerable<TypeInfo>> selector) => _selector = selector;

		public IEnumerable<TypeInfo> Get(TypeInfo parameter)
			=> _selector(parameter).Where(x => x.IsInterface).Distinct();
	}


	static class Defaults
	{
		public static Func<TypeInfo, IEnumerable<TypeInfo>> Selector { get; } = In<TypeInfo>.Activate<Interfaces>().Get;
	}

	sealed class Interfaces : Enumerable<TypeInfo>, IActivateMarker<TypeInfo>
	{
		readonly TypeInfo                              _metadata;
		readonly Func<TypeInfo, IEnumerable<TypeInfo>> _selector;

		[UsedImplicitly]
		public Interfaces(TypeInfo metadata) : this(metadata, Defaults.Selector) {}

		public Interfaces(TypeInfo metadata, Func<TypeInfo, IEnumerable<TypeInfo>> selector)
		{
			_metadata = metadata;
			_selector = selector;
		}

		public override IEnumerator<TypeInfo> GetEnumerator()
		{
			yield return _metadata;
			foreach (var metadata in _metadata.ImplementedInterfaces
			                                  .YieldMetadata()
			                                  .SelectMany(_selector))
			{
				yield return metadata;
			}
		}
	}
}