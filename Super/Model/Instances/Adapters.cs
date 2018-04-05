using Super.Model.Sources;

namespace Super.Model.Instances
{
	sealed class Adapters<T> : ReferenceStore<IInstance<T>, SourceAdapter<T>>
	{
		public static Adapters<T> Default { get; } = new Adapters<T>();

		Adapters() {}
	}
}