using Super.ExtensionMethods;

namespace Super.Model.Instances
{
	public class DecoratedInstance<T> : DelegatedInstance<T>
	{
		/*public DecoratedInstance(ISource<Unit, T> source) : base(source.Get) {}*/

		public DecoratedInstance(IInstance<T> instance) : base(instance.ToDelegate()) {}
	}
}