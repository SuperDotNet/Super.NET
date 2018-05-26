using Super.Model.Collections;
using System;
using System.Reflection.Emit;

namespace Super.Platform
{
	public sealed class Size : ISize
	{
		public static Size Default { get; } = new Size();

		Size() {}

		public uint Get(Type type)
		{
			var method    = new DynamicMethod("GetManagedSizeImpl", typeof(uint), null, true);
			var generator = method.GetILGenerator();
			generator.Emit(OpCodes.Sizeof, type);
			generator.Emit(OpCodes.Ret);
			var @delegate = (Func<uint>)method.CreateDelegate(typeof(Func<uint>));
			var result    = @delegate();
			return result;
		}
	}
}