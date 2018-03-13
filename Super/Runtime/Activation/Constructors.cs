using System;
using System.Reflection;
using Super.Expressions;

namespace Super.Runtime.Activation
{
	sealed class Constructors<T> : Delegates<ConstructorInfo, Func<T>>
	{
		public static Constructors<T> Default { get; } = new Constructors<T>();

		Constructors() : base(Instances.Default) {}
	}
}