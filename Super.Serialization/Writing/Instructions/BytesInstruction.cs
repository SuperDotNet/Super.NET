/*
using Super.Model.Sequences;
using System;
using System.Buffers;
using System.Buffers.Text;

namespace Super.Serialization.Writing.Instructions
{
	sealed class BytesInstruction : IInstruction<ReadOnlyMemory<byte>>
	{
		readonly static Range All   = Ranges.Default.All, Low = Ranges.Default.Low;
		readonly static int   Start = Ranges.Default.Low.Start.Value;

		public static BytesInstruction Default { get; } = new BytesInstruction();

		BytesInstruction()
			: this(Maps.Default.Get(), DefaultEscapeFactor.Default, HexFormat.Default) {}

		readonly Array<byte>    _map;
		readonly uint           _factor;
		readonly StandardFormat _format;

		public BytesInstruction(Array<byte> map, uint factor, StandardFormat format)
		{
			_map    = map;
			_factor = factor;
			_format = format;
		}

		// ReSharper disable once ExcessiveIndentation
		public uint Get(Composition<ReadOnlyMemory<byte>> parameter)
		{
			var source = parameter.Instance.Span;

			var current = (int)parameter.Index;
			var amount  = 0;
			var marker  = 0;
			var result  = 0;

			var length = parameter.Instance.Length;
			for (var i = 0; i < length; i++)
			{
				var element = source[i];

				if (EscapeIndex.Default.Get(element))
				{
/*
					var index = current + result;
					var span  = parameter.Output.AsSpan(index);
					var write = amount > 0 ? Utf8.Get(source.Slice(marker, amount), span) : 0;
					var slice = span.Slice(write);

					slice[0] = SpecialCharacters.Slash;

					if (All.IsOrContains(element))
					{
						i++;

						if (i >= length || element >= Start)
						{
							throw new InvalidOperationException($"{element} is not a valid unicode surrogate.");
						}

						ushort low = source[i];
						if (!Low.IsOrContains(low))
						{
							throw new InvalidOperationException($"{element}{low} is not a valid unicode surrogate.");
						}

						slice[1] = SpecialCharacters.Unicode;

						if (!Utf8Formatter.TryFormat(element, slice.Slice(2), out _, _format))
						{
							throw new
								InvalidOperationException($"Could not format unicode high surrogate: {element}");
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
						slice[1] =  _map[element];
						result   += 2;
					}

					result += write;

					marker = i + 1;
					amount = 0;
#1#
				}
				else
				{
					amount++;
				}
			}

			/*return (uint)(amount > 0
				              ? result + Utf8.Get(source.Slice(marker, amount),
				                                  parameter.Output.AsSpan(current + result))
				              : result);#1#
			
			return default;
		}

		public uint Get(ReadOnlyMemory<byte> parameter)
			=> (uint)Base64.GetMaxEncodedToUtf8Length(parameter.Length) * _factor;
	}
}
*/
