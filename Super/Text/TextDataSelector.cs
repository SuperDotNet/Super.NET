using System.Text;
using Super.Model.Selection;

namespace Super.Text
{
	sealed class TextDataSelector : Delegated<string, byte[]>
	{
		public static TextDataSelector Default { get; } = new TextDataSelector();

		TextDataSelector() : this(Encoding.UTF8) {}

		public TextDataSelector(Encoding encoding) : base(encoding.GetBytes) {}
	}
}