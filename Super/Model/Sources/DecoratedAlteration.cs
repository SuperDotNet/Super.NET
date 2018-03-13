using Super.Model.Sources.Alterations;

namespace Super.Model.Sources
{
	public class DecoratedAlteration<T> : DelegatedAlteration<T>
	{
		public DecoratedAlteration(ISource<T, T> source) : base(source.Get) {}

		public DecoratedAlteration(IAlteration<T> alteration) : base(alteration.Get) {}
	}
}