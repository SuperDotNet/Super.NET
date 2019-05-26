using System;
using Super.Model.Selection;
using Super.Model.Sequences;
using Super.Runtime.Activation;

namespace Super.Reflection.Types
{
	sealed class GenericTypeBuilder : ISelect<Type, Type>, IActivateUsing<Array<Type>>
	{
		readonly Array<Type> _parameters;

		public GenericTypeBuilder(Array<Type> parameters) => _parameters = parameters;

		public Type Get(Type parameter) => parameter.To(I<MakeGenericType>.Default).Get(_parameters);
	}
}