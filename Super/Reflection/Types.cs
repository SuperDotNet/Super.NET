using System;
using System.Reflection;

namespace Super.Reflection
{
	public static class Types
	{
		public static Type Void { get; } = typeof(void);
	}

	public static class Types<T>
	{
		public static Type Identity { get; } = typeof(T);

		public static TypeInfo Key { get; } = typeof(T).GetTypeInfo();
	}
}