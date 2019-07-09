using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Sequences;
using Super.Platform;
using System;
using System.Buffers;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;

namespace Super.Serialization.Writing.Instructions
{
	sealed class DefaultEscapeFactor : Instance<uint>
	{
		public static DefaultEscapeFactor Default { get; } = new DefaultEscapeFactor();

		DefaultEscapeFactor() : base(6u) {}
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

	sealed class Maps : IArray<byte[]>
	{
		public static Maps Default { get; } = new Maps();

		Maps() : this(new Dictionary<byte, string>
		{
			{(byte)'\b', "\\b"},
			{(byte)'\n', "\\n"},
			{(byte)'\r', "\\r"},
			{(byte)'\f', "\\f"},
			{(byte)'\t', "\\t"},
			{(byte)'\\', "\\\\"},
		}) {}

		readonly IReadOnlyDictionary<byte, byte[]> _source;
		readonly uint                              _max;

		public Maps(IReadOnlyDictionary<byte, string> source)
			: this(source.ToDictionary(x => x.Key, x => x.Value.Select(y => (byte)y).ToArray()), byte.MaxValue) {}

		public Maps(IReadOnlyDictionary<byte, byte[]> source, uint max)
		{
			_source = source;
			_max    = max;
		}

		public Array<byte[]> Get()
		{
			var result = new byte[_max][];

			foreach (var key in _source.Keys)
			{
				result[key] = _source[key];
			}

			return result;
		}
	}

	public interface ITextEncoder : ISelect<EncoderInput, uint> {}

	sealed class TextEncoder : ITextEncoder
	{
		public static TextEncoder Default { get; } = new TextEncoder();

		TextEncoder() : this(Maps.Default.Get()) {}

		readonly Array<byte[]> _tokens;

		public TextEncoder(Array<byte[]> tokens) => _tokens = tokens;

		// ReSharper disable once ExcessiveIndentation
		public uint Get(EncoderInput parameter)
		{
			var source      = parameter.Source.Span;
			var destination = parameter.Destination.Span;
			var result      = 0;

			var length = source.Length;
			for (var i = 0; i < length; i++)
			{
				var character = source[i];
				if (character <= byte.MaxValue)
				{
					var tokens = _tokens[character];
					if (tokens != null)
					{
						for (var j = 0; j < tokens.Length; j++)
						{
							destination[result + j] = tokens[j];
						}

						result += tokens.Length;
						continue;
					}

					destination[result++] = (byte)character;
					continue;
				}

				if (System.Text.Rune.TryCreate(character, out var single))
				{
					result += single.EncodeToUtf8(destination.Slice(result));
					continue;
				}

				var check = i + 1 < source.Length;
				if (check && System.Text.Rune.TryCreate(character, source[i + 1], out var @double))
				{
					i++;
					result += @double.EncodeToUtf8(destination.Slice(result));
					continue;
				}

				var next = check ? source[i + 1] : '\0';
				throw new InvalidOperationException($"{character}{next} is not a valid unicode.");
			}

			return (uint)result;
		}
	}

	sealed class VerboseTextEncoder : ITextEncoder
	{
		readonly static Range Low   = Ranges.Default.Low;
		readonly static int   Start = Ranges.Default.Low.Start.Value;

		public static VerboseTextEncoder Default { get; } = new VerboseTextEncoder();

		VerboseTextEncoder() : this(Maps.Default.Get(), HexFormat.Default) {}

		readonly Array<byte[]>  _tokens;
		readonly StandardFormat _format;

		public VerboseTextEncoder(Array<byte[]> tokens, StandardFormat format)
		{
			_tokens = tokens;
			_format = format;
		}

		// ReSharper disable once ExcessiveIndentation
		public uint Get(EncoderInput parameter)
		{
			var source      = parameter.Source.Span;
			var destination = parameter.Destination.Span;
			var result      = 0;

			var length = source.Length;
			for (var i = 0; i < length; i++)
			{
				var character = source[i];
				if (character <= byte.MaxValue)
				{
					var tokens = _tokens[character];
					if (tokens != null)
					{
						for (var j = 0; j < tokens.Length; j++)
						{
							destination[result + j] = tokens[j];
						}

						result += tokens.Length;
						continue;
					}

					destination[result++] = (byte)character;
					continue;
				}

				i++;

				var slice = destination.Slice(result);

				if (i >= length || character >= Start)
				{
					throw new InvalidOperationException($"{character} is not a valid unicode surrogate.");
				}

				ushort low = source[i];
				if (!Low.IsOrContains(low))
				{
					throw new InvalidOperationException($"{character}{low} is not a valid unicode surrogate.");
				}

				slice[0] = SpecialCharacters.Slash;
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

			return (uint)result;
		}
	}

	public readonly struct EncoderInput
	{
		public EncoderInput(ReadOnlyMemory<char> source, Memory<byte> destination)
		{
			Source      = source;
			Destination = destination;
		}

		public ReadOnlyMemory<char> Source { get; }

		public Memory<byte> Destination { get; }
	}

	sealed class Allowed : ArrayInstance<byte>
	{
		public static Allowed Default { get; } = new Allowed();

		Allowed() : base(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
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
		                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0) {}
	}

	sealed class StringInstruction : IInstruction<string>
	{
		public static StringInstruction Default { get; } = new StringInstruction();

		StringInstruction() : this(TextEncoder.Default, Allowed.Default, DefaultEscapeFactor.Default) {}

		readonly ITextEncoder _encoder;
		readonly Array<byte>  _allowed;
		readonly uint         _factor;

		public StringInstruction(ITextEncoder encoder) : this(encoder, Allowed.Default, DefaultEscapeFactor.Default) {}

		public StringInstruction(ITextEncoder encoder, Array<byte> allowed, uint factor)
		{
			_encoder = encoder;
			_allowed = allowed;
			_factor  = factor;
		}

		public uint Get(Composition<string> parameter)
		{
			var instance = parameter.Instance;
			var length   = instance.Length;
			for (var i = 0; i < length; i++)
			{
				var character = instance[i];
				if (character > byte.MaxValue || _allowed[character] == 0)
				{
					var count = Utf8.Get(instance.AsSpan(0, i), parameter.View);
					var input = new EncoderInput(instance.AsMemory(i),
					                             parameter.Output.AsMemory((int)parameter.Index + count));
					return (uint)count + _encoder.Get(input);
				}
			}

			return (uint)Utf8.Get(instance, parameter.View);
		}

		public uint Get(string parameter) => (uint)parameter.Length * _factor;
	}

	sealed class Base64Instruction : IInstruction<Array<byte>>
	{
		public static Base64Instruction Default { get; } = new Base64Instruction();

		Base64Instruction() : this(TextEncoder.Default, Allowed.Default) {}

		readonly ITextEncoder _encoder;
		readonly Array<byte>  _allowed;

		public Base64Instruction(ITextEncoder encoder) : this(encoder, Allowed.Default) {}

		public Base64Instruction(ITextEncoder encoder, Array<byte> allowed)
		{
			_encoder = encoder;
			_allowed = allowed;
		}

		public uint Get(Composition<Array<byte>> parameter)
		{
			var instance = parameter.Instance.Open();
			var length   = instance.Length;
			/*for (var i = 0; i < length; i++)
			{
				var character = instance[i];
				if (_allowed[character] == 0)
				{
					var count = Utf8.Get(instance.AsSpan(0, i), parameter.View);
					var input = new EncoderInput(instance.AsMemory(i),
					                             parameter.Output.AsMemory((int)parameter.Index + count));
					return (uint)count + _encoder.Get(input);
				}
			}*/

			Base64.EncodeToUtf8(instance, parameter.View, out _, out var result);

			return (uint)result;
		}

		public uint Get(Array<byte> parameter) => (uint)Base64.GetMaxEncodedToUtf8Length((int)parameter.Length);
	}
}