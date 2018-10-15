using Super.Model.Selection.Stores;
using System;

namespace Super.Runtime.Activation
{
	public sealed class Singletons : ReferenceValueTable<Type, object>, ISingletons
	{
		public static Singletons Default { get; } = new Singletons();

		Singletons() : base(Start.From<Type>()
		                         .Default<object>()
		                         .Unless(HasSingletonProperty.Default,
		                                 SingletonProperty.Default
		                                                  .Select(SingletonPropertyDelegates.Default)
		                                                  .Invoke())
		                         .Get) {}
	}
}