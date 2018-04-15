using Super.ExtensionMethods;

namespace Super.Model.Sources
{
	public class Decorated<T> : Delegated<T>
	{
		public Decorated(ISource<T> source) : base(source.ToDelegate()) {}
	}
}