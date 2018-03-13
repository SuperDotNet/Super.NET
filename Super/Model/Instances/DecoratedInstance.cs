using System.Reactive;
using Super.ExtensionMethods;
using Super.Model.Sources;

namespace Super.Model.Instances
{
	public class DecoratedInstance<T> : DelegatedInstance<T>
	{
		public DecoratedInstance(ISource<Unit, T> source) : base(source.Get) {}

		public DecoratedInstance(IInstance<T> instance) : base(instance.Get) {}
	}
}