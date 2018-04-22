using Super.Model.Selection;
using Super.Runtime.Invocation;
using System;

namespace Super.Runtime.Activation
{
	public sealed class Singletons : DecoratedSelect<Type, object>, ISingletons
	{
		public static Singletons Default { get; } = new Singletons();

		Singletons() : base(HasSingletonProperty.Default
		                                        .If(SingletonProperty.Default
		                                                             .Out(SingletonPropertyDelegates.Default)
		                                                             .Out(Call<object>.Default),
		                                            Default<Type, object>.Instance)
		                                        .ToReferenceStore()) {}
	}
}