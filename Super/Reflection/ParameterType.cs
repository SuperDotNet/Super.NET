using System;
using System.Reflection;
using Super.Model.Sources;

namespace Super.Reflection
{
	sealed class ParameterType : ISource<ParameterInfo, Type>
	{
		public static ParameterType Default { get; } = new ParameterType();

		ParameterType() {}

		public Type Get(ParameterInfo parameter) => parameter.ParameterType;
	}
}