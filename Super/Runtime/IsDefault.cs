using Super.Model.Selection.Conditions;

namespace Super.Runtime
{
	sealed class IsDefault<T> : EqualityCondition<T>
	{
		public static IsDefault<T> Default { get; } = new IsDefault<T>();

		IsDefault() : base(default) {}
	}
}