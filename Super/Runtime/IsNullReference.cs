using Super.Model.Selection.Conditions;

namespace Super.Runtime
{
	sealed class IsNullReference : Condition<object>
	{
		public static IsNullReference Default { get; } = new IsNullReference();

		IsNullReference() : base(EqualsNullReference.Default.Get) {}
	}
}