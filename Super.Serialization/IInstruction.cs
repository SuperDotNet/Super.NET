using Super.Model.Results;
using Super.Model.Selection;
using Super.Runtime.Environment;
using Super.Text;
using System;
using System.Buffers.Text;

namespace Super.Serialization
{
	public interface IInstruction : ISelect<Composition, uint>, IResult<uint> {}

	public class Content : Instance<uint>, IInstruction
	{
		readonly byte[] _content;
		readonly uint   _length;

		public Content(string content) : this(Encoder.Default.Get(content)) {}

		public Content(byte[] content) : this(content, (uint)content.Length) {}

		public Content(byte[] content, uint length) : base(length)
		{
			_content = content;
			_length  = length;
		}

		public uint Get(Composition parameter)
		{
			_content.CopyInto(parameter.Output, 0, in _length, parameter.Index);
			return _length;
		}
	}

	public interface IInstruction<T> : ISelect<Composition<T>, uint>, ISelect<T, uint> {}

	sealed class QuotedInstruction<T> : IInstruction<T>
	{
		readonly IInstruction<T> _instruction;
		readonly byte            _quote;

		public QuotedInstruction(IInstruction<T> instruction) : this(instruction, DoubleQuote.Default) {}

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

	sealed class PositiveIntegerInstruction : IInstruction<uint>
	{
		public static PositiveIntegerInstruction Default { get; } = new PositiveIntegerInstruction();

		PositiveIntegerInstruction() {}

		public uint Get(Composition<uint> parameter)
			=> Utf8Formatter.TryFormat(parameter.Instance, parameter.Output.AsSpan((int)parameter.Index),
			                           out var count)
				   ? (uint)count
				   : throw new
					     InvalidOperationException($"Could not format '{parameter.Instance}' into its UTF-8 equivalent.");

		public uint Get(uint parameter) => 10;
	}
}