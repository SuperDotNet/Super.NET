using Super.Model.Selection;

namespace Super.Text.Formatting
{
	public interface ISelectFormatter<in T> : ISelect<string, T, string> {}
}