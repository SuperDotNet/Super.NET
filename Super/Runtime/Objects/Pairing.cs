using Super.Runtime.Invocation;

namespace Super.Runtime.Objects
{
	sealed class Pairing : Invocation1<string, object, Pair<string, object>>
	{
		public Pairing(string parameter) : base(Pairs.Create, parameter) {}
	}
}