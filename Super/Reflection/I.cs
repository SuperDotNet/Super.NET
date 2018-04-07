using Super.Model.Instances;
using System.Reflection;

namespace Super.Reflection
{
	public sealed class I<T> : Instance<TypeInfo>, IInfer
	{
		public static I<T> Default { get; } = new I<T>();

		I() : base(Types<T>.Key) {}
	}

	public sealed class I<T, TParameter, TResult> : Instance<TypeInfo>, IInfer
	{
		public static I<T, TParameter, TResult> Default { get; } = new I<T, TParameter, TResult>();

		I() : base(Types<T>.Key) {}
	}
}