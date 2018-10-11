using Super.Model.Collections;
using Super.Model.Selection;
using Super.Runtime.Activation;
using System;

namespace Super.Reflection.Types
{
	public class MakeGenericType : ISelect<Array<Type>, Type>, IActivateMarker<Type>
	{
		readonly Type _definition;

		public MakeGenericType(Type definition) => _definition = definition;

		public Type Get(Array<Type> parameter) => _definition.MakeGenericType(parameter);
	}

	sealed class GenericTypeBuilder : ISelect<Type, Type>, IActivateMarker<Array<Type>>
	{
		readonly Array<Type> _parameters;

		public GenericTypeBuilder(Array<Type> parameters) => _parameters = parameters;

		public Type Get(Type parameter) => parameter.To(I<MakeGenericType>.Default).Get(_parameters);
	}
}