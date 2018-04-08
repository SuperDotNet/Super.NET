using Super.ExtensionMethods;

namespace Super.Model.Selection
{
	public class Decorated<TParameter, TResult> : Delegated<TParameter, TResult>
	{
		public Decorated(ISelect<TParameter, TResult> @select) : base(@select.ToDelegate()) {}
	}
}