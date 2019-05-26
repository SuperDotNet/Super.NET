using System;
using Super.Model.Selection;
using Super.Model.Sequences;
using Super.Runtime.Activation;

namespace Super.Reflection.Types
{
	public class MakeGenericType : ISelect<Array<Type>, Type>, IActivateUsing<Type>
	{
		readonly Type _definition;

		public MakeGenericType(Type definition) => _definition = definition;

		public Type Get(Array<Type> parameter) => _definition.MakeGenericType(parameter);
	}
}