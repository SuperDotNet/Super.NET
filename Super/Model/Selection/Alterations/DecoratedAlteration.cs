namespace Super.Model.Selection.Alterations
{
	public class DecoratedAlteration<T> : DelegatedAlteration<T>
	{
		public DecoratedAlteration(ISelect<T, T> @select) : base(@select.Get) {}

		public DecoratedAlteration(IAlteration<T> alteration) : base(alteration.Get) {}
	}
}