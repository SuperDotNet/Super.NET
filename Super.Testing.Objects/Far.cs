using Super.Model.Results;
using Super.Model.Sequences;

namespace Super.Testing.Objects
{
	sealed class Far : Instance<Selection>
	{
		public static Far Default { get; } = new Far();

		Far() : base(new Selection(5000, 300)) {}
	}
}