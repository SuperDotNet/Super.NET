using Super.Model.Selection;
using System.Text;

namespace Super.Runtime
{
	sealed class StringToBytesSelector : Delegated<string, byte[]>
	{
		public static StringToBytesSelector Default { get; } = new StringToBytesSelector();

		StringToBytesSelector() : this(Encoding.UTF8) {}

		public StringToBytesSelector(Encoding encoding) : base(encoding.GetBytes) {}
	}
}