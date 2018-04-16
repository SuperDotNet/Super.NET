using Super.ExtensionMethods;

namespace Super.Model.Selection
{
	public class Decorated<TParameter, TResult> : Select<TParameter, TResult>
	{
		public Decorated(ISelect<TParameter, TResult> @select) : base(@select.ToDelegate()) {}
	}
}