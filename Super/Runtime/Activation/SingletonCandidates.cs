using Super.Model.Collections;

namespace Super.Runtime.Activation
{
	sealed class SingletonCandidates : Array<string>, ISingletonCandidates
	{
		public static SingletonCandidates Default { get; } = new SingletonCandidates();

		SingletonCandidates() : this("Default", "Instance", "Implementation", "Singleton") {}

		public SingletonCandidates(params string[] items) : base(items) {}
	}
}