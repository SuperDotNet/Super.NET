using Super.Model.Sources;

namespace Super.Model.Instances
{
	sealed class InstanceValueCoercer<T> : DelegatedSource<IInstance<T>, T>
	{
		public static InstanceValueCoercer<T> Default { get; } = new InstanceValueCoercer<T>();

		InstanceValueCoercer() : base(x => x.Get()) {}
	}

	sealed class Instances<T> : Store<T, Instance<T>>
	{
		public static Instances<T> Default { get; } = new Instances<T>();

		Instances() {}
	}
}