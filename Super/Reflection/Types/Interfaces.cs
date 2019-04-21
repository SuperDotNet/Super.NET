using Super.Model.Selection;
using Super.Runtime.Activation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Super.Reflection.Types
{
	sealed class Interfaces : ISelect<TypeInfo, IEnumerable<TypeInfo>>, IActivateUsing<TypeInfo>
	{
		public static Interfaces Default { get; } = new Interfaces();

		Interfaces() : this(x => Default.Get(x)) {}

		readonly Func<Type, IEnumerable<TypeInfo>> _selector;

		public Interfaces(Func<Type, IEnumerable<TypeInfo>> selector) => _selector = selector;

		public IEnumerable<TypeInfo> Get(TypeInfo parameter)
		{
			yield return parameter;

			foreach (var metadata in parameter.ImplementedInterfaces
			                                  .SelectMany(_selector))
			{
				yield return metadata;
			}
		}
	}
}