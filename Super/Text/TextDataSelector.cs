using Super.Model.Selection;
using System.Text;

namespace Super.Text
{
	sealed class TextDataSelector : Delegated<string, byte[]>
	{
		public static TextDataSelector Default { get; } = new TextDataSelector();

		TextDataSelector() : this(Encoding.UTF8) {}

		public TextDataSelector(Encoding encoding) : base(encoding.GetBytes) {}
	}

	public sealed class TextSelector<T> : Message<T>
	{
		public static TextSelector<T> Default { get; } = new TextSelector<T>();

		TextSelector() : base(x => x.ToString()) {}
	}
}