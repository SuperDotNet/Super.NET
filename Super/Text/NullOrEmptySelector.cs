using Super.Model.Selection.Alterations;

namespace Super.Text
{
	public sealed class NullOrEmptySelector : IAlteration<string>
	{
		public static NullOrEmptySelector Default { get; } = new NullOrEmptySelector();

		NullOrEmptySelector() {}

		public string Get(string parameter) => parameter ?? string.Empty;
	}
}