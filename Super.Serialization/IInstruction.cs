using Super.Model.Results;
using Super.Model.Selection;
using Super.Runtime.Environment;
using Super.Text;
using System;
using System.Buffers;
using System.Buffers.Text;
using System.Runtime.CompilerServices;

namespace Super.Serialization
{
	public interface IInstruction : ISelect<Composition, uint>, IResult<uint> {}

	public class ContentInstruction : Instance<uint>, IInstruction
	{
		readonly byte[] _content;
		readonly uint   _length;

		public ContentInstruction(string content) : this(Encoder.Default.Get(content)) {}

		public ContentInstruction(byte[] content) : this(content, (uint)content.Length) {}

		public ContentInstruction(byte[] content, uint length) : base(length)
		{
			_content = content;
			_length  = length;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint Get(Composition parameter)
		{
			_content.CopyInto(parameter.Output, 0, in _length, parameter.Index);
			return _length;
		}
	}

	public interface IInstruction<T> : ISelect<Composition<T>, uint>, ISelect<T, uint> {}

	class Adapter<T> : IInstruction<T>
	{
		readonly IInstruction _instruction;

		public Adapter(IInstruction instruction) => _instruction = instruction;

		public uint Get(Composition<T> parameter) => _instruction.Get(parameter);

		public uint Get(T parameter) => _instruction.Get();
	}

	sealed class QuotedInstruction<T> : IInstruction<T>
	{
		readonly IInstruction<T> _instruction;
		readonly byte            _quote;

		/*public QuotedInstruction(IInstruction<T> instruction) : this(instruction, DoubleQuote.Default) {}*/

		public QuotedInstruction(IInstruction<T> instruction, byte quote)
		{
			_instruction = instruction;
			_quote       = quote;
		}

		public uint Get(Composition<T> parameter)
		{
			var index = parameter.Index;

			parameter.Output[index++] = _quote;

			index += _instruction.Get(new Composition<T>(parameter.Output, parameter.Instance, in index));

			parameter.Output[index++] = _quote;

			return index - parameter.Index;
		}

		public uint Get(T parameter) => _instruction.Get(parameter) + 2;
	}

	sealed class StringValueInstruction : IInstruction<string>
	{
		public static StringValueInstruction Default { get; } = new StringValueInstruction();

		StringValueInstruction() : this(DefaultComponent<IUtf8>.Default.Get()) {}

		readonly IUtf8 _utf8;

		public StringValueInstruction(IUtf8 utf8) => _utf8 = utf8;

		public uint Get(Composition<string> parameter)
			=> _utf8.Get(new Utf8Input(parameter.Instance, parameter.Output, parameter.Index));

		public uint Get(string parameter) => (uint)parameter.Length;
	}

	sealed class IntegerInstruction : IInstruction<uint>
	{
		public static IntegerInstruction Default { get; } = new IntegerInstruction();

		IntegerInstruction() : this((uint)uint.MaxValue.ToString().Length) {}

		readonly uint _size;

		public IntegerInstruction(uint size) => _size = size;

		public uint Get(Composition<uint> parameter)
			=> Utf8Formatter.TryFormat(parameter.Instance, parameter.Output.AsSpan((int)parameter.Index),
			                           out var count)
				   ? (uint)count
				   : throw new
					     InvalidOperationException($"Could not format '{parameter.Instance}' into its UTF-8 equivalent.");

		public uint Get(uint parameter) => _size;
	}

	sealed class FullIntegerInstruction : IInstruction<int>
	{
		public static FullIntegerInstruction Default { get; } = new FullIntegerInstruction();

		FullIntegerInstruction() : this((uint)int.MinValue.ToString().Length) {}

		readonly uint _size;

		public FullIntegerInstruction(uint size) => _size = size;

		public uint Get(Composition<int> parameter)
			=> Utf8Formatter.TryFormat(parameter.Instance, parameter.Output.AsSpan((int)parameter.Index),
			                           out var count)
				   ? (uint)count
				   : throw new
					     InvalidOperationException($"Could not format '{parameter.Instance}' into its UTF-8 equivalent.");

		public uint Get(int parameter) => _size;
	}

	sealed class ByteInstruction : IInstruction<byte>
	{
		public static ByteInstruction Default { get; } = new ByteInstruction();

		ByteInstruction() : this((uint)byte.MaxValue.ToString().Length) {}

		readonly uint _size;

		public ByteInstruction(uint size) => _size = size;

		public uint Get(Composition<byte> parameter)
			=> Utf8Formatter.TryFormat(parameter.Instance, parameter.Output.AsSpan((int)parameter.Index),
			                           out var count)
				   ? (uint)count
				   : throw new
					     InvalidOperationException($"Could not format '{parameter.Instance}' into its UTF-8 equivalent.");

		public uint Get(byte parameter) => _size;
	}

	sealed class FullByteInstruction : IInstruction<sbyte>
	{
		public static FullByteInstruction Default { get; } = new FullByteInstruction();

		FullByteInstruction() : this((uint)sbyte.MinValue.ToString().Length) {}

		readonly uint _size;

		public FullByteInstruction(uint size) => _size = size;

		public uint Get(Composition<sbyte> parameter)
			=> Utf8Formatter.TryFormat(parameter.Instance, parameter.Output.AsSpan((int)parameter.Index),
			                           out var count)
				   ? (uint)count
				   : throw new
					     InvalidOperationException($"Could not format '{parameter.Instance}' into its UTF-8 equivalent.");

		public uint Get(sbyte parameter) => _size;
	}

	sealed class ShortInstruction : IInstruction<ushort>
	{
		public static ShortInstruction Default { get; } = new ShortInstruction();

		ShortInstruction() : this((uint)ushort.MaxValue.ToString().Length) {}

		readonly uint _size;

		public ShortInstruction(uint size) => _size = size;

		public uint Get(Composition<ushort> parameter)
			=> Utf8Formatter.TryFormat(parameter.Instance, parameter.Output.AsSpan((int)parameter.Index),
			                           out var count)
				   ? (uint)count
				   : throw new
					     InvalidOperationException($"Could not format '{parameter.Instance}' into its UTF-8 equivalent.");

		public uint Get(ushort parameter) => _size;
	}

	sealed class FullShortInstruction : IInstruction<short>
	{
		public static FullShortInstruction Default { get; } = new FullShortInstruction();

		FullShortInstruction() : this((uint)short.MinValue.ToString().Length) {}

		readonly uint _size;

		public FullShortInstruction(uint size) => _size = size;

		public uint Get(Composition<short> parameter)
			=> Utf8Formatter.TryFormat(parameter.Instance, parameter.Output.AsSpan((int)parameter.Index),
			                           out var count)
				   ? (uint)count
				   : throw new
					     InvalidOperationException($"Could not format '{parameter.Instance}' into its UTF-8 equivalent.");

		public uint Get(short parameter) => _size;
	}

	sealed class Integer64Instruction : IInstruction<ulong>
	{
		public static Integer64Instruction Default { get; } = new Integer64Instruction();

		Integer64Instruction() : this((uint)ulong.MaxValue.ToString().Length) {}

		readonly uint _size;

		public Integer64Instruction(uint size) => _size = size;

		public uint Get(Composition<ulong> parameter)
			=> Utf8Formatter.TryFormat(parameter.Instance, parameter.Output.AsSpan((int)parameter.Index),
			                           out var count)
				   ? (uint)count
				   : throw new
					     InvalidOperationException($"Could not format '{parameter.Instance}' into its UTF-8 equivalent.");

		public uint Get(ulong parameter) => _size;
	}

	sealed class FullInteger64Instruction : IInstruction<long>
	{
		public static FullInteger64Instruction Default { get; } = new FullInteger64Instruction();

		FullInteger64Instruction() : this((uint)long.MinValue.ToString().Length) {}

		readonly uint _size;

		public FullInteger64Instruction(uint size) => _size = size;

		public uint Get(Composition<long> parameter)
			=> Utf8Formatter.TryFormat(parameter.Instance, parameter.Output.AsSpan((int)parameter.Index),
			                           out var count)
				   ? (uint)count
				   : throw new
					     InvalidOperationException($"Could not format '{parameter.Instance}' into its UTF-8 equivalent.");

		public uint Get(long parameter) => _size;
	}

	sealed class TrueInstruction : ContentInstruction
	{
		public static TrueInstruction Default { get; } = new TrueInstruction();

		TrueInstruction() : base("true") {}
	}

	sealed class BooleanInstruction : IInstruction<bool>
	{
		public static BooleanInstruction Default { get; } = new BooleanInstruction();

		BooleanInstruction() : this(TrueInstruction.Default.Get, FalseInstruction.Default.Get) {}

		readonly Func<Composition, uint> _true, _false;

		public BooleanInstruction(Func<Composition, uint> @true, Func<Composition, uint> @false)
		{
			_true  = @true;
			_false = @false;
		}

		public uint Get(Composition<bool> parameter) => parameter.Instance ? _true(parameter) : _false(parameter);

		public uint Get(bool parameter) => 5;
	}

	sealed class FalseInstruction : ContentInstruction
	{
		public static FalseInstruction Default { get; } = new FalseInstruction();

		FalseInstruction() : base("false") {}
	}

	sealed class FloatInstruction : IInstruction<float>
	{
		public static FloatInstruction Default { get; } = new FloatInstruction();

		FloatInstruction() {}

		public uint Get(Composition<float> parameter)
			=> Utf8Formatter.TryFormat(parameter.Instance, parameter.Output.AsSpan((int)parameter.Index),
			                           out var count)
				   ? (uint)count
				   : throw new
					     InvalidOperationException($"Could not format '{parameter.Instance}' into its UTF-8 equivalent.");

		public uint Get(float parameter) => 128;
	}

	sealed class DoubleInstruction : IInstruction<double>
	{
		public static DoubleInstruction Default { get; } = new DoubleInstruction();

		DoubleInstruction() {}

		public uint Get(Composition<double> parameter)
			=> Utf8Formatter.TryFormat(parameter.Instance, parameter.Output.AsSpan((int)parameter.Index),
			                           out var count)
				   ? (uint)count
				   : throw new
					     InvalidOperationException($"Could not format '{parameter.Instance}' into its UTF-8 equivalent.");

		public uint Get(double parameter) => 128;
	}

	sealed class DecimalInstruction : IInstruction<decimal>
	{
		public static DecimalInstruction Default { get; } = new DecimalInstruction();

		DecimalInstruction() {}

		public uint Get(Composition<decimal> parameter)
			=> Utf8Formatter.TryFormat(parameter.Instance, parameter.Output.AsSpan((int)parameter.Index),
			                           out var count)
				   ? (uint)count
				   : throw new
					     InvalidOperationException($"Could not format '{parameter.Instance}' into its UTF-8 equivalent.");

		public uint Get(decimal parameter) => 31;
	}

	sealed class GuidInstruction : IInstruction<Guid>
	{
		public static GuidInstruction Default { get; } = new GuidInstruction();

		GuidInstruction() {}

		public uint Get(Composition<Guid> parameter)
			=> Utf8Formatter.TryFormat(parameter.Instance, parameter.Output.AsSpan((int)parameter.Index),
			                           out var count)
				   ? (uint)count
				   : throw new
					     InvalidOperationException($"Could not format '{parameter.Instance}' into its UTF-8 equivalent.");

		public uint Get(Guid parameter) => 36;
	}

	sealed class DateTimeInstruction : IInstruction<DateTime>
	{
		public static DateTimeInstruction Default { get; } = new DateTimeInstruction();

		DateTimeInstruction() : this(Trimmer.Default, 'O') {}

		readonly Trimmer        _trimmer;
		readonly StandardFormat _format;

		public DateTimeInstruction(Trimmer trimmer, StandardFormat format)
		{
			_trimmer = trimmer;
			_format  = format;
		}

		public uint Get(Composition<DateTime> parameter)
		{
			Span<byte> span   = stackalloc byte[27];
			var        format = Utf8Formatter.TryFormat(parameter.Instance, span, out var count, _format);
			if (format)
			{
				var result = _trimmer.Get(span.Slice(0, count));
				span.Slice(0, (int)result)
				    .CopyTo(parameter.Output.AsSpan((int)parameter.Index));
				return result;
			}

			throw new InvalidOperationException($"Could not format '{parameter.Instance}' into its UTF-8 equivalent.");
		}

		public uint Get(DateTime parameter) => 27;
	}

	sealed class DateTimeOffsetInstruction : IInstruction<DateTimeOffset>
	{
		public static DateTimeOffsetInstruction Default { get; } = new DateTimeOffsetInstruction();

		DateTimeOffsetInstruction() : this(Trimmer.Default, 'O') {}

		readonly Trimmer        _trimmer;
		readonly StandardFormat _format;

		public DateTimeOffsetInstruction(Trimmer trimmer, StandardFormat format)
		{
			_trimmer = trimmer;
			_format  = format;
		}

		public uint Get(Composition<DateTimeOffset> parameter)
		{
			Span<byte> span   = stackalloc byte[33];
			var        format = Utf8Formatter.TryFormat(parameter.Instance, span, out var count, _format);
			if (format)
			{
				var result = _trimmer.Get(span.Slice(0, count));
				span.Slice(0, (int)result)
				    .CopyTo(parameter.Output.AsSpan((int)parameter.Index));
				return result;
			}

			throw new InvalidOperationException($"Could not format '{parameter.Instance}' into its UTF-8 equivalent.");
		}

		public uint Get(DateTimeOffset parameter) => 33;
	}

	/// <summary>
	/// ATTRIBUTION: https://github.com/dotnet/corefx/blob/11b548176e6889866dee553ea84b92da3916e73b/src/System.Text.Json/src/System/Text/Json/Writer/JsonWriterHelper.Date.cs
	/// </summary>
	sealed class Trimmer //: ISelect<Memory<byte>, uint>
	{
		public static Trimmer Default { get; } = new Trimmer();

		Trimmer() : this(Colon.Default, '0') {}

		readonly byte _colon;
		readonly uint _zero;

		public Trimmer(byte colon, uint zero)
		{
			_colon = colon;
			_zero  = zero;
		}

		// ReSharper disable once MethodTooLong
		public uint Get(Span<byte> buffer)
		{
			var fraction = (buffer[20] - _zero) * 1_000_000 +
			               (buffer[21] - _zero) * 100_000 +
			               (buffer[22] - _zero) * 10_000 +
			               (buffer[23] - _zero) * 1_000 +
			               (buffer[24] - _zero) * 100 +
			               (buffer[25] - _zero) * 10 +
			               (buffer[26] - _zero);

			var index = 19;

			if (fraction > 0)
			{
				var numFractionDigits = 7;

				while (true)
				{
					var quotient = fraction / 10;
					if (fraction - quotient * 10 != 0)
					{
						break;
					}

					fraction = quotient;
					numFractionDigits--;
				}

				var fractionEnd = 19 + numFractionDigits;

				for (var i = fractionEnd; i > index; i--)
				{
					buffer[i] =  (byte)(fraction % 10 + '0');
					fraction  /= 10;
				}

				index = fractionEnd + 1;
			}

			var result = index;

			if (buffer.Length > 27)
			{
				buffer[index] = buffer[27];

				result = index + 1;

				if (buffer.Length == 33)
				{
					var bufferEnd = index + 5;

					buffer[bufferEnd]     = buffer[32];
					buffer[bufferEnd - 1] = buffer[31];
					buffer[bufferEnd - 2] = _colon;
					buffer[bufferEnd - 3] = buffer[29];
					buffer[bufferEnd - 4] = buffer[28];

					result = bufferEnd + 1;
				}
			}

			return (uint)result;
		}
	}
}