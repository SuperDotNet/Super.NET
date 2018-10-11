using Super.Model.Selection;
using Super.Runtime.Activation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Super.Reflection.Types
{
	sealed class Interfaces : ISelect<TypeInfo, IEnumerable<TypeInfo>>, IActivateMarker<TypeInfo>
	{
		public static Interfaces Default { get; } = new Interfaces();

		Interfaces() : this(I<Interfaces>.Default.Instance(x => x).Get) {}

		readonly Func<TypeInfo, IEnumerable<TypeInfo>> _selector;

		public Interfaces(Func<TypeInfo, IEnumerable<TypeInfo>> selector) => _selector = selector;

		public IEnumerable<TypeInfo> Get(TypeInfo parameter)
		{
			yield return parameter;

			foreach (var metadata in parameter.ImplementedInterfaces
			                                  .YieldMetadata()
			                                  .SelectMany(_selector))
			{
				yield return metadata;
			}
		}
	}
}