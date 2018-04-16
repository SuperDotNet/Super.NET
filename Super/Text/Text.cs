using Super.Model.Sources;

namespace Super.Text
{
	public class Text : Source<string>
	{
		protected Text(string instance) : base(instance) {}

		public override string ToString() => Get();
	}

	sealed class None : Text
	{
		public static None Default { get; } = new None();

		None() : base("N/A") {}
	}
}
