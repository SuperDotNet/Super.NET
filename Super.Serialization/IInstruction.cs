using Super.Model.Results;
using Super.Model.Selection;
using Super.Runtime.Environment;
using Super.Text;
using System;
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
}