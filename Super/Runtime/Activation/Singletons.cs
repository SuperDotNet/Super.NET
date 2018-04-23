using Super.Model.Selection;
using Super.Model.Selection.Stores;
using Super.Runtime.Invocation;
using System;

namespace Super.Runtime.Activation
{
	public sealed class Singletons : ReferenceValueTable<Type, object>, ISingletons
	{
		public static Singletons Default { get; } = new Singletons();

		Singletons() : base(HasSingletonProperty.Default
		                                        .If(Implementations.SingletonProperties
		                                                           .Out(SingletonPropertyDelegates.Default)
		                                                           .Out(Call<object>.Default),
		                                            Default<Type, object>.Instance)
		                                        .Get) {}
	}
}