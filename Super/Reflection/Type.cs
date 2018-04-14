using System;
using System.Reflection;

namespace Super.Reflection
{
	public static class Type<T>
	{
		public static Type Instance { get; } = typeof(T);

		public static TypeInfo Metadata { get; } = typeof(T).GetTypeInfo();
	}
}