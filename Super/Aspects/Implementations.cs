using System;
using Super.Compose;
using Super.Model.Sequences;

namespace Super.Aspects
{
	static class Implementations
	{
		public static Func<object, Array<Type>> Arguments { get; }
			= Start.A.Selection.Of.Any.By.Type.Select(AspectImplementationArguments.Default).Get;

		public static Func<Array<IRegistration>> Registrations { get; }
			= AspectRegistry.Default.Get;
	}
}