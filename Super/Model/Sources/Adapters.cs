using Super.Model.Instances;

namespace Super.Model.Sources
{
	sealed class Adapters<T> : ReferenceStore<IInstance<T>, SourceAdapter<T>>
	{
		public static Adapters<T> Default { get; } = new Adapters<T>();

		Adapters() {}
	}
}