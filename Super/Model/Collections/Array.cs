using Super.Model.Selection;
using Super.Model.Sources;
using Super.Reflection.Types;
using Super.Runtime.Environment;
using System;

namespace Super.Model.Collections
{
	static class Implementations
	{
		public static ISelect<Type, uint> Size { get; } = DefaultComponent<ISize>.Default.Emit().ToStore();
	}

	sealed class Size<T> : FixedDeferredSingleton<Type, uint>
	{
		public static Size<T> Default { get; } = new Size<T>();

		Size() : base(Implementations.Size, Type<T>.Instance) {}
	}

	public interface ISize : ISelect<Type, uint> {}
}