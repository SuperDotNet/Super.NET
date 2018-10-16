using Super.Model.Selection;
using Super.Model.Sequences;
using System;

namespace Super.Reflection.Types
{
	public class GenericDefinition<T> : DecoratedSelect<Array<Type>, T>, IGeneric<T> where T : Delegate
	{
		protected GenericDefinition(Type definition, ISelect<Type, T> activate)
			: base(new MakeGenericType(definition).Select(activate)) {}
	}
}