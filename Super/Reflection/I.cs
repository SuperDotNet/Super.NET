using Super.Model.Instances;
using System.Reflection;

namespace Super.Reflection
{
	public sealed class I<T> : Instance<TypeInfo>, IInfer
	{
		public static I<T> Default { get; } = new I<T>();

		I() : base(Types<T>.Key) {}
	}

	public sealed class Source<T, TParameter, TResult> : Instance<TypeInfo>, IInfer
	{
		public static Source<T, TParameter, TResult> Default { get; } = new Source<T, TParameter, TResult>();

		Source() : base(Types<T>.Key) {}
	}

	
	public sealed class Cast<T> : Instance<TypeInfo>, IInfer
	{
		public static Cast<T> Default { get; } = new Cast<T>();

		Cast() : base(Types<T>.Key) {}
	}
}