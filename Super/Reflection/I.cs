using System.Reflection;
using Super.Model.Results;
using Super.Reflection.Types;
using Super.Runtime;

namespace Super.Reflection
{
	public static class I
	{
		public static I<T> A<T>() => I<T>.Default;

		public static I<object> Any() => I<object>.Default;

		public static I<None> None() => I<None>.Default;
	}

	public sealed class I<T> : Instance<TypeInfo>, IInfer
	{
		public static I<T> Default { get; } = new I<T>();

		I() : base(Type<T>.Metadata) {}
	}
}