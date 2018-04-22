using Super.Model.Selection;
using Super.Model.Sources;
using System;
using System.Linq;

namespace Super.Runtime.Environment
{
	sealed class ComponentTypeLocator : ISelect<Type, Type>
	{
		public static ComponentTypeLocator Default { get; } = new ComponentTypeLocator();

		ComponentTypeLocator() : this(ComponentTypeCandidates.Default) {}

		readonly ITypeCandidates _candidates;

		public ComponentTypeLocator(ITypeCandidates candidates) => _candidates = candidates;

		public Type Get(Type parameter)
		{
			var candidates = _candidates.Get();
			var result = candidates.FirstOrDefault(parameter.IsAssignableFrom) ??
			             candidates.FirstOrDefault(typeof(ISource<>).MakeGenericType(parameter).IsAssignableFrom);
			return result;
		}
	}
}