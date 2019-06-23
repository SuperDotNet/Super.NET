using Super.Model;
using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Selection.Conditions;
using Super.Model.Sequences;
using Super.Platform;
using System;
using System.Buffers;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;

namespace Super.Serialization.Writing.Instructions
{
	/*sealed class StringInstruction : IInstruction<ReadOnlyMemory<char>>
	{
		public static StringInstruction Default { get; } = new StringInstruction();

		StringInstruction() : this(DefaultComponent<IUtf8>.Default.Get()) {}

		readonly IUtf8 _utf8;

		public StringInstruction(IUtf8 utf8) => _utf8 = utf8;

		public uint Get(Composition<ReadOnlyMemory<char>> parameter)
			=> _utf8.Get(new Utf8Input(parameter.Instance, parameter.Output, parameter.Index));

		public uint Get(ReadOnlyMemory<char> parameter) => (uint)parameter.Length;
	}*/

	sealed class DefaultEscapeFactor : Instance<uint>
	{
		public static DefaultEscapeFactor Default { get; } = new DefaultEscapeFactor();

		DefaultEscapeFactor() : base(6u) {}
	}

	public interface IEscapeIndex : ICondition<char>, ISelect<ReadOnlyMemory<char>, Assigned<uint>> {}

	/// <summary>
	/// ATTRIBUTION: https://github.com/dotnet/corefx/blob/master/src/System.Text.Json/src/System/Text/Json/Writer/JsonWriterHelper.Escaping.cs
	/// </summary>
	sealed class EscapeIndex : IEscapeIndex
	{
		public static EscapeIndex Default { get; } = new EscapeIndex();

		EscapeIndex() : this(new byte[]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			1, 1, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 1, 1, 1, 0,
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1,
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1,
			0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		}) {}

		readonly Array<byte> _map;

		public EscapeIndex(Array<byte> map) => _map = map;

		public Assigned<uint> Get(ReadOnlyMemory<char> parameter)
		{
			var span   = parameter.Span;
			var length = parameter.Length;

			for (var i = 0; i < length; i++)
			{
				if (Get(span[i]))
				{
					return (uint)i;
				}
			}

			return Assigned<uint>.Unassigned;
		}

		public bool Get(char parameter) => parameter > byte.MaxValue || _map[parameter] == 0;
	}

	public sealed class HexFormat : Instance<StandardFormat>
	{
		public static HexFormat Default { get; } = new HexFormat();

		HexFormat() : base(new StandardFormat('x', 4)) {}
	}

	public static class SpecialCharacters
	{
		public const byte Slash = (byte)'\\', Unicode = (byte)'u';
	}

	public sealed class Maps : IArray<byte>
	{
		public static Maps Default { get; } = new Maps();

		Maps() : this(new Dictionary<byte, byte>
		{
			{(byte)'\b', (byte)'b'},
			{(byte)'\n', (byte)'n'},
			{(byte)'\r', (byte)'r'},
			{(byte)'\f', (byte)'f'},
			{(byte)'\t', (byte)'t'},
			{(byte)'\\', (byte)'\\'},
		}) {}

		readonly IReadOnlyDictionary<byte, byte> _source;
		readonly uint                            _max;

		public Maps(IReadOnlyDictionary<byte, byte> source) : this(source, source.Keys.Max() + 1u) {}

		public Maps(IReadOnlyDictionary<byte, byte> source, uint max)
		{
			_source = source;
			_max    = max;
		}

		public Array<byte> Get()
		{
			var result = new byte[_max];
			foreach (var key in _source.Keys)
			{
				result[key] = _source[key];
			}

			return result;
		}
	}

	sealed class StringInstruction : IInstruction<string>
	{
		readonly static Range All   = Ranges.Default.All, Low = Ranges.Default.Low;
		readonly static int   Start = Ranges.Default.Low.Start.Value;

		public static StringInstruction Default { get; } = new StringInstruction();

		StringInstruction()
			: this(Maps.Default.Get(), DefaultEscapeFactor.Default, HexFormat.Default) {}

		readonly Array<byte>    _map;
		readonly uint           _factor;
		readonly StandardFormat _format;

		public StringInstruction(Array<byte> map, uint factor, StandardFormat format)
		{
			_map    = map;
			_factor = factor;
			_format = format;
		}

		// ReSharper disable once ExcessiveIndentation
		public uint Get(Composition<string> parameter)
		{
			var source = parameter.Instance.AsSpan();

			var current = (int)parameter.Index;
			var amount  = 0;
			var marker  = 0;
			var result  = 0;

			var length = parameter.Instance.Length;
			for (var i = 0; i < length; i++)
			{
				var character = source[i];

				if (EscapeIndex.Default.Get(character))
				{
					var index = current + result;
					var span  = parameter.Output.AsSpan(index);
					var write = amount > 0 ? Utf8.Get(source.Slice(marker, amount), span) : 0;
					var slice = span.Slice(write);
					slice[0] = SpecialCharacters.Slash;

					if (All.IsOrContains(character))
					{
						i++;

						if (i >= length || character >= Start)
						{
							throw new InvalidOperationException($"{character} is not a valid unicode surrogate.");
						}

						ushort low = source[i];
						if (!Low.IsOrContains(low))
						{
							throw new InvalidOperationException($"{character}{low} is not a valid unicode surrogate.");
						}

						slice[1] = SpecialCharacters.Unicode;

						if (!Utf8Formatter.TryFormat(character, slice.Slice(2), out _, _format))
						{
							throw new
								InvalidOperationException($"Could not format unicode high surrogate: {character}");
						}

						slice[6] = SpecialCharacters.Slash;
						slice[7] = SpecialCharacters.Unicode;

						if (!Utf8Formatter.TryFormat(low, slice.Slice(8), out _, _format))
						{
							throw new InvalidOperationException($"Could not format unicode low surrogate: {low}");
						}

						result += 12;
					}
					else
					{
						slice[1] =  _map[character];
						result   += 2;
					}

					result += write;

					marker = i + 1;
					amount = 0;
				}
				else
				{
					amount++;
				}
			}

			return (uint)(amount > 0
				              ? result + Utf8.Get(source.Slice(marker, amount),
				                                  parameter.Output.AsSpan(current + result))
				              : result);
		}

		public uint Get(string parameter) => (uint)parameter.Length * _factor;
	}
}