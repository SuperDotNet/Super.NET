using Super.ExtensionMethods;

namespace Super.Model.Sources
{
	public class Decorated<T> : Delegated<T>
	{
		/*public DecoratedInstance(ISource<Unit, T> source) : base(source.Get) {}*/

		public Decorated(ISource<T> source) : base(source.ToDelegate()) {}
	}
}