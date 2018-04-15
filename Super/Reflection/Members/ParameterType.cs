using System;
using System.Reflection;
using Super.Model.Selection;

namespace Super.Reflection.Members
{
	sealed class ParameterType : ISelect<ParameterInfo, Type>
	{
		public static ParameterType Default { get; } = new ParameterType();

		ParameterType() {}

		public Type Get(ParameterInfo parameter) => parameter.ParameterType;
	}
}