using Super.ExtensionMethods;
using Super.Runtime.Invocation;
using System;
using Super.Model.Selection;

namespace Super.Runtime.Activation
{
	public sealed class Singletons : Decorated<Type, object>, ISingletons
	{
		public static Singletons Default { get; } = new Singletons();

		Singletons() : base(HasSingletonProperty.Default
		                                        .If(SingletonProperty.Default
		                                                             .Out(SingletonPropertyDelegates.Default)
		                                                             .Out(Invoke<object>.Default))
		                                        .ToReferenceStore()) {}
	}
}