using Super.Reflection.Types;

namespace Super.Model.Selection
{
	public sealed class ImplementsSelection : ImplementsGenericType
	{
		public static ImplementsSelection Default { get; } = new ImplementsSelection();

		ImplementsSelection() : base(typeof(ISelect<,>)) {}
	}

	public sealed class SelectionImplementations : GenericImplementations
	{
		public static SelectionImplementations Default { get; } = new SelectionImplementations();

		SelectionImplementations() : base(typeof(ISelect<,>)) {}
	}
}
