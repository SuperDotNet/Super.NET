using Super.Model.Selection.Alterations;

namespace Super.Text
{
	public sealed class NullOrEmpty : IAlteration<string>
	{
		public static NullOrEmpty Default { get; } = new NullOrEmpty();

		NullOrEmpty() {}

		public string Get(string parameter) => parameter ?? string.Empty;
	}
}