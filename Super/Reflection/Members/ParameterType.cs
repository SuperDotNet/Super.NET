using System;
using System.Reflection;
using Super.Model.Selection;

namespace Super.Reflection.Members
{
	sealed class ParameterType : Select<ParameterInfo, Type>
	{
		public static ParameterType Default { get; } = new ParameterType();

		ParameterType() : base(x => x.ParameterType) {}
	}
}