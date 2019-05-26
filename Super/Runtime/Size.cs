using System;
using Super.Model.Results;
using Super.Model.Sequences;
using Super.Reflection.Types;
using Super.Runtime.Environment;

namespace Super.Runtime
{
	sealed class Size<T> : FixedSelectedSingleton<Type, uint>
	{
		public static Size<T> Default { get; } = new Size<T>();

		Size() : base(DefaultComponentLocator<ISize>.Default.Assume(), Type<T>.Instance) {}
	}
}