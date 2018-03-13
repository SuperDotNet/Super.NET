using System;
using Super.ExtensionMethods;
using Super.Model.Sources;

namespace Super.Runtime.Activation
{
	public sealed class Singletons : DecoratedSource<Type, object>, ISingletons
	{
		public static Singletons Default { get; } = new Singletons();

		Singletons() : base(HasSingletonProperty.Default
		                                        .If(SingletonProperty.Default
		                                                             .Out(SingletonPropertyDelegates.Default)
		                                                             .Out(DelegateCoercer<object>.Default))
		                                        .ToReferenceStore()) {}
	}
}