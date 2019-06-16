using Super.Model.Selection;

namespace Super.Text
{
	public interface IFormatter<in T> : ISelect<T, string> {}

	public readonly struct Utf8Input
	{
		public Utf8Input(string characters, byte[] destination, in uint start)
		{
			Characters  = characters;
			Destination = destination;
			Start       = start;
		}

		public string Characters { get; }

		public byte[] Destination { get; }

		public uint Start { get; }
	}

	public interface IUtf8 : ISelect<Utf8Input, uint> {}
}