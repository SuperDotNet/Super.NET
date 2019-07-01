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
using System.Runtime.CompilerServices;

namespace Super.Serialization.Writing.Instructions
{
	sealed class DefaultEscapeFactor : Instance<uint>
	{
		public static DefaultEscapeFactor Default { get; } = new DefaultEscapeFactor();

		DefaultEscapeFactor() : base(6u) {}
	}

	public interface IEscapeIndex : ICondition<byte>, ICondition<char> {}

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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Get(byte parameter) => _map[parameter] == 0;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Get(char parameter) => _map[parameter] == 0;
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

	public sealed class MapsJr : IArray<byte[]>
	{
		public static MapsJr Default { get; } = new MapsJr();

		MapsJr() : this(new Dictionary<byte, string>
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

		public MapsJr(IReadOnlyDictionary<byte, string> source)
			: this(source.ToDictionary(x => x.Key, x => x.Value.Select(y => (byte)y).ToArray()), byte.MaxValue) {}

		public MapsJr(IReadOnlyDictionary<byte, byte[]> source, uint max)
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

	public interface IEscapist : ISelect<EscapeInput, uint> {}

	sealed class Escapist : IEscapist
	{
		public static Escapist Default { get; } = new Escapist();

		Escapist() : this(MapsJr.Default.Get()) {}

		readonly Array<byte[]> _tokens;

		public Escapist(Array<byte[]> tokens) => _tokens  = tokens;

		public uint Get(EscapeInput parameter)
		{
			var source      = parameter.Source.Span;
			var destination = parameter.Destination.Span;
			var result      = 0;

			var length = source.Length;
			for (var i = 0; i < length; i++)
			{
				var character = source[i];

				var step = Step(character, destination, result);

				result += step > 0
					          ? step
					          : Rune(ref i, character, source, destination.Slice(result));
			}

			return (uint)result;
		}

		// ReSharper disable once TooManyArguments
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static int Rune(ref int index, char character, ReadOnlySpan<char> source, Span<byte> destination)
		{
			if (System.Text.Rune.TryCreate(character, out var single))
			{
				return single.EncodeToUtf8(destination);
			}

			var check = index + 1 < source.Length;
			if (check && System.Text.Rune.TryCreate(character, source[index + 1], out var @double))
			{
				index++;
				return @double.EncodeToUtf8(destination);
			}

			var next = check ? source[index + 1] : '\0';
			throw new InvalidOperationException($"{character}{next} is not a valid unicode.");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		int Step(char character, Span<byte> destination, int result)
		{
			if (character <= byte.MaxValue)
			{
				var tokens = _tokens[character];
				if (tokens != null)
				{
					for (var j = 0; j < tokens.Length; j++)
					{
						destination[result + j] = tokens[j];
					}

					return tokens.Length;
				}

				destination[result] = (byte)character;
				return 1;
			}

			return 0;
		}
	}

	public readonly struct EscapeInput
	{
		public EscapeInput(ReadOnlyMemory<char> source, Memory<byte> destination)
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

		StringInstruction() : this(Escapist.Default, Allowed.Default, DefaultEscapeFactor.Default) {}

		readonly IEscapist   _escapist;
		readonly Array<byte> _map;
		readonly uint        _factor;

		public StringInstruction(IEscapist escapist, Array<byte> map, uint factor)
		{
			_escapist = escapist;
			_map      = map;
			_factor   = factor;
		}

		public uint Get(Composition<string> parameter)
		{
			var instance = parameter.Instance;
			var length   = instance.Length;
			for (var i = 0; i < length; i++)
			{
				var character = instance[i];
				if (character > byte.MaxValue || _map[character] == 0)
				{
					var count = Utf8.Get(instance.AsSpan(0, i), parameter.View);
					var input = new EscapeInput(instance.AsMemory(i),
					                            parameter.Output.AsMemory((int)parameter.Index + count));
					return (uint)count + _escapist.Get(input);
				}
			}

			return (uint)Utf8.Get(instance, parameter.View);
		}

		public uint Get(string parameter) => (uint)parameter.Length * _factor;
	}

	sealed class EscapingStringInstruction : IInstruction<string>
	{
		readonly static Range Low   = Ranges.Default.Low;
		readonly static int   Start = Ranges.Default.Low.Start.Value;

		public static EscapingStringInstruction Default { get; } = new EscapingStringInstruction();

		EscapingStringInstruction()
			: this(Maps.Default.Get(), DefaultEscapeFactor.Default, HexFormat.Default) {}

		readonly Array<byte>    _map;
		readonly uint           _factor;
		readonly StandardFormat _format;

		public EscapingStringInstruction(Array<byte> map, uint factor, StandardFormat format)
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

				var upper = character > byte.MaxValue;
				if (upper || EscapeIndex.Default.Get(character))
				{
					var index = current + result;
					var span  = parameter.Output.AsSpan(index);
					var write = amount > 0 ? Utf8.Get(source.Slice(marker, amount), span) : 0;
					var slice = span.Slice(write);

					slice[0] = SpecialCharacters.Slash;

					if (upper)
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