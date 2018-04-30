using Super.Model.Selection;
using Super.Model.Selection.Stores;
using System;

namespace Super.Runtime.Activation
{
	public sealed class Singletons : ReferenceValueTable<Type, object>, ISingletons
	{
		public static Singletons Default { get; } = new Singletons();

		Singletons() : base(Default<Type, object>.Instance
		                                         .Unless(HasSingletonProperty.Default,
		                                                 Implementations.SingletonProperty
		                                                                .Select(SingletonPropertyDelegates.Default)
		                                                                .Invoke())
		                                         .Get) {}
	}
}