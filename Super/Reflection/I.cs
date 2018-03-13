using System.Reflection;
using Super.Model.Instances;

namespace Super.Reflection
{
	public sealed class I<T> : Instance<TypeInfo>, ITypes
	{
		public static I<T> Default { get; } = new I<T>();

		I() : base(Types<T>.Key) {}
	}
}