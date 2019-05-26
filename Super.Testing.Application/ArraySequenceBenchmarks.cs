using Super.Reflection;
using Super.Testing.Objects;

namespace Super.Testing.Application
{
	public class ArraySequenceBenchmarks : SequenceBenchmarks<uint>
	{
		public ArraySequenceBenchmarks() : base(I.A<ArrayEnumerations<uint>>().From) {}
	}
}