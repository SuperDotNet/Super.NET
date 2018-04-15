using System.Reflection;
using Super.Model.Sources;
using Super.Reflection.Types;

namespace Super.Reflection
{
	public sealed class I<T> : Source<TypeInfo>, IInfer
	{
		public static I<T> Default { get; } = new I<T>();

		I() : base(Type<T>.Metadata) {}
	}

	public sealed class I<T, TParameter, TResult> : Source<TypeInfo>, IInfer
	{
		public static I<T, TParameter, TResult> Default { get; } = new I<T, TParameter, TResult>();

		I() : base(Type<T>.Metadata) {}
	}
}