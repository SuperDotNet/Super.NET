using System.Text;
using Super.Model.Sources;

namespace Super.Runtime
{
	sealed class StringToBytesCoercer : DelegatedSource<string, byte[]>
	{
		public static StringToBytesCoercer Default { get; } = new StringToBytesCoercer();

		StringToBytesCoercer() : this(Encoding.UTF8) {}

		public StringToBytesCoercer(Encoding encoding) : base(encoding.GetBytes) {}
	}
}