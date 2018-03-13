using System;
using Super.Model.Sources;

namespace Super.Runtime
{
	sealed class InstanceTypeCoercer<T> : ISource<T, Type>
	{
		public static InstanceTypeCoercer<T> Default { get; } = new InstanceTypeCoercer<T>();

		InstanceTypeCoercer() {}

		public Type Get(T parameter) => parameter.GetType();
	}
}