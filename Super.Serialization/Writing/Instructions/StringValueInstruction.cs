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

	public interface ITextEncoder : ISelect<TextEncoderInput, uint> {}

	sealed class TextEncoder : ITextEncoder
	{
		public static TextEncoder Default { get; } = new TextEncoder();

		TextEncoder() : this(Maps.Default.Get()) {}

		readonly Array<byte[]> _tokens;

		public TextEncoder(Array<byte[]> tokens) => _tokens = tokens;

		// ReSharper disable once ExcessiveIndentation
		public uint Get(TextEncoderInput parameter)
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
		public uint Get(TextEncoderInput parameter)
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

	public readonly struct TextEncoderInput
	{
		public TextEncoderInput(ReadOnlyMemory<char> source, Memory<byte> destination)
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
					var count = Utf8.Default.Get(instance.AsSpan(0, i), parameter.View);
					var input = new TextEncoderInput(instance.AsMemory(i),
					                                 parameter.Output.AsMemory((int)parameter.Index + count));
					return (uint)count + _encoder.Get(input);
				}
			}

			return (uint)Utf8.Default.Get(instance, parameter.View);
		}

		public uint Get(string parameter) => (uint)parameter.Length * _factor;
	}

	sealed class Base64Instruction : IInstruction<Array<byte>>
	{
		public static Base64Instruction Default { get; } = new Base64Instruction();

		Base64Instruction() {}

		public uint Get(Composition<Array<byte>> parameter)
		{
			var status = Base64.EncodeToUtf8(parameter.Instance.Open(), parameter.View, out _, out var result);
			if (status != OperationStatus.Done)
			{
				throw new
					InvalidOperationException($"[{status}] Could not successfully convert value to Utf-8 data: {parameter.Instance}");
			}

			return (uint)result;
		}

		public uint Get(Array<byte> parameter) => (uint)Base64.GetMaxEncodedToUtf8Length((int)parameter.Length);
	}

	sealed class ArrayStart : Token
	{
		public static ArrayStart Default { get; } = new ArrayStart();

		ArrayStart() : base('[') {}
	}

	sealed class ArrayFinish : Token
	{
		public static ArrayFinish Default { get; } = new ArrayFinish();

		ArrayFinish() : base(']') {}
	}

	sealed class ArrayDelimiter : Token
	{
		public static ArrayDelimiter Default { get; } = new ArrayDelimiter();

		ArrayDelimiter() : base(',') {}
	}

	public interface IElementInstructions<T> : ISelect<uint, IInstruction<Array<T>>> {}

	class ElementInstructions<T> : IElementInstructions<T>
	{
		readonly Array<IInstruction<Array<T>>> _instructions;

		public ElementInstructions(Array<IInstruction<Array<T>>> instructions) => _instructions = instructions;

		public IInstruction<Array<T>> Get(uint parameter) => _instructions[parameter];
	}

	sealed class ArrayElementInstruction<T> : IInstruction<Array<T>>
	{
		readonly IInstruction<T> _element;
		readonly uint _index;

		public ArrayElementInstruction(IInstruction<T> element, uint index)
		{
			_element = element;
			_index = index;
		}

		public uint Get(Composition<Array<T>> parameter)
			=> _element.Get(new Composition<T>(parameter.Output, parameter.Instance[_index], parameter.Index));

		public uint Get(Array<T> parameter) => _element.Get(parameter[_index]);
	}

	sealed class ArrayInstructions<T> : IInstructions<Array<T>>
	{
		readonly static IInstruction<Array<T>> Start     = ArrayStart.Default.For<Array<T>>(),
		                                       Delimiter = ArrayDelimiter.Default.For<Array<T>>(),
		                                       Finish    = ArrayFinish.Default.For<Array<T>>();

		readonly IStorage<IInstruction<Array<T>>> _storage;
		readonly IInstruction<Array<T>>           _start, _delimiter, _finish;
		readonly IElementInstructions<T>          _elements;

		public ArrayInstructions(IElementInstructions<T> elements)
			: this(Leases<IInstruction<Array<T>>>.Default, Start, elements, Delimiter, Finish) {}

		// ReSharper disable once TooManyDependencies
		public ArrayInstructions(IStorage<IInstruction<Array<T>>> storage,
		                         IInstruction<Array<T>> start,
		                         IElementInstructions<T> elements,
		                         IInstruction<Array<T>> delimiter,
		                         IInstruction<Array<T>> finish)
		{
			_storage   = storage;
			_start     = start;
			_elements  = elements;
			_delimiter = delimiter;
			_finish    = finish;
		}

		public Session<IInstruction<Array<T>>> Get(Array<T> parameter)
		{
			var result = _storage.Session(parameter.Length * 2 + 1);
			var index  = 0u;
			var length = parameter.Length;
			var last   = length - 1;
			result.Store[index++] = _start;
			for (var i = 0u; i < length; i++)
			{
				result.Store[index++] = _elements.Get(i);
				if (i != last)
				{
					result.Store[index++] = _delimiter;
				}
			}

			result.Store[index] = _finish;

			return result;
		}
	}

	sealed class ArrayInstruction<T> : IInstruction<Array<T>>
	{
		readonly IInstruction<T> _element;
		readonly byte            _start, _delimiter, _finish;

		public ArrayInstruction(IInstruction<T> element)
			: this(element, ArrayStart.Default, ArrayFinish.Default, ArrayDelimiter.Default) {}

		// ReSharper disable once TooManyDependencies
		public ArrayInstruction(IInstruction<T> element, byte start, byte finish, byte delimiter)
		{
			_element   = element;
			_start     = start;
			_finish    = finish;
			_delimiter = delimiter;
		}

		public uint Get(Composition<Array<T>> parameter)
		{
			var length = parameter.Instance.Length;
			var last   = length - 1;
			var result = 0u;
			parameter.Output[parameter.Index + result++] = _start;
			for (var i = 0; i < length; i++)
			{
				result += _element.Get(new Composition<T>(parameter.Output, parameter.Instance[i],
				                                          parameter.Index + result));
				if (i != last)
				{
					parameter.Output[parameter.Index + result++] = _delimiter;
				}
			}

			parameter.Output[parameter.Index + result++] = _finish;
			return result;
		}

		public uint Get(Array<T> parameter)
		{
			var result = 2u;
			var length = parameter.Length;
			for (var i = 0; i < length; i++)
			{
				result += _element.Get(parameter[i]) + 1u;
			}

			return result;
		}
	}
}