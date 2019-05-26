using Super.Reflection.Types;

namespace Super.Model.Selection
{
	public sealed class SelectionImplementations : GenericImplementations
	{
		public static SelectionImplementations Default { get; } = new SelectionImplementations();

		SelectionImplementations() : base(typeof(ISelect<,>)) {}
	}
}