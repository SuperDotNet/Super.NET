using Super.Model.Results;

namespace Super.Serialization
{
	public interface IToken : IResult<byte> {}

	public class Token : Instance<byte>, IToken
	{
		public Token(char instance) : base((byte)instance) {}
	}

	sealed class Colon : Token
	{
		public static Colon Default { get; } = new Colon();

		Colon() : base(':') {}
	}

	sealed class DoubleQuote : Token
	{
		public static DoubleQuote Default { get; } = new DoubleQuote();

		DoubleQuote() : base('"') {}
	}
}