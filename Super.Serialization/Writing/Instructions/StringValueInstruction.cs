using Super.Model;
using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Selection.Conditions;
using Super.Model.Sequences;
using Super.Platform;
using System;
using System.Buffers;
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

	/*public readonly struct Escape
	{
		public Escape(ReadOnlyMemory<char> source, uint start, byte[] destination)
		{
			Source      = source;
			Start       = start;
			Destination = destination;
		}

		public ReadOnlyMemory<char> Source { get; }

		public byte[] Destination { get; }

		public uint Start { get; }
	}*/

	/*public interface IEscapeString : ISelect<Composition<string>, uint> {}*/

	public sealed class HexFormat : Instance<StandardFormat>
	{
		public static HexFormat Default { get; } = new HexFormat();

		HexFormat() : base(new StandardFormat('x', 4)) {}
	}

	public readonly struct Utf16Surrogate
	{
		public Utf16Surrogate(ushort high, ushort low)
		{
			High = high;
			Low  = low;
		}

		public ushort High { get; }

		public ushort Low { get; }
	}

	public static class SpecialCharacters
	{
		public const byte Backspace = (byte)'b',
		                  Line      = (byte)'n',
		                  Return    = (byte)'r',
		                  Feed      = (byte)'f',
		                  Tab       = (byte)'t',
		                  Slash     = (byte)'\\';
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
		readonly static int Start = Ranges.Default.Low.Start.Value;

		public static StringInstruction Default { get; } = new StringInstruction();

		StringInstruction() : this(EscapeIndex.Default, Ranges.Default.All, Maps.Default.Get(),
		                           DefaultEscapeFactor.Default) {}

		readonly IEscapeIndex _index;
		readonly Range        _range;
		readonly Array<byte>  _map;
		readonly uint         _factor;

		// ReSharper disable once TooManyDependencies
		public StringInstruction(IEscapeIndex index, Range range, Array<byte> map, uint factor)
		{
			_index  = index;
			_range  = range;
			_map    = map;
			_factor = factor;
		}

		// ReSharper disable once ExcessiveIndentation
		public uint Get(Composition<string> parameter)
		{
			var source = parameter.Instance.AsSpan();

			var current = (int)parameter.Index;
			var amount  = 0;
			var last    = 0;
			var result  = 0;

			var length = parameter.Instance.Length;
			for (var i = 0; i < length; i++)
			{
				var character = source[i];

				if (_index.Get(character))
				{
					var next = -1;
					if (_range.IsOrContains(character))
					{
						i++;

						if (i >= length || character >= Start)
						{
							throw new InvalidOperationException($"{character} is not a valid escaped character.");
						}

						if (!Ranges.Default.Low.IsOrContains(source[i]))
						{
							throw
								new InvalidOperationException($"{source[i]} is not a valid an escaped character.");
						}

						next = source[i];
					}

					var index = current + result;
					var span  = parameter.Output.AsSpan(index);
					var write = amount > 0 ? Utf8.Get(source.Slice(last, amount), span) : 0;
					if (next == -1)
					{
						var slice = span.Slice(write);
						slice[0] =  SpecialCharacters.Slash;
						slice[1] =  _map[character];
						result   += write + 2;
					}

					amount = 0;
					last   = i + 1;
				}
				else
				{
					amount++;
				}
			}

			return (uint)(amount > 0
				              ? result + Utf8.Get(source.Slice(last, amount), parameter.Output.AsSpan(current + result))
				              : result);
		}

/*
		// ReSharper disable once TooManyArguments
		// ReSharper disable once MethodTooLong
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		int Adjust(ushort current, int next, Span<byte> destination)
		{

			return 2;

			/*switch (current)
			{
				default:
				{
					/*var i = index + 1;
					destination[i++] = (byte)'u';
					{
						i += ;

						/*i += current.TryFormat(span.Slice(i), out var written, Format)
							     ? written
							     : 0;#3#
					}

					if (next != -1)
					{

						destination[i++] = (byte)'\\';
						destination[i++] = (byte)'u';
						i += Utf8Formatter.TryFormat(current, destination.AsSpan().Slice(i), out var written, HexFormat.Default)
							     ? written
							     : throw new
								       InvalidOperationException($"Could not successfully convert value to Utf-8 data: {current}");
					}

					return (uint)(i - index);#2#
					throw new NotImplementedException();
				}
			}#1#
		}
*/

		public uint Get(string parameter) => (uint)parameter.Length * _factor;
	}
}