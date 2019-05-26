using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Super.Model.Selection;
using Super.Runtime.Activation;

namespace Super.Reflection.Types
{
	sealed class Interfaces : ISelect<TypeInfo, IEnumerable<Type>>, IActivateUsing<Type>
	{
		public static Interfaces Default { get; } = new Interfaces();

		Interfaces() : this(x => Default.Get(x)) {}

		readonly Func<Type, IEnumerable<Type>> _selector;

		public Interfaces(Func<Type, IEnumerable<Type>> selector) => _selector = selector;

		public IEnumerable<Type> Get(TypeInfo parameter)
			=> parameter.ImplementedInterfaces.SelectMany(_selector).Prepend(parameter);
	}
}