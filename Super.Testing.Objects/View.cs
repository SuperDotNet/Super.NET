using Super.Model.Sequences;

namespace Super.Testing.Objects
{
	sealed class View : ArrayInstance<string>
	{
		public static View Default { get; } = new View();

		View() : base(Data.Default) {}
	}
}