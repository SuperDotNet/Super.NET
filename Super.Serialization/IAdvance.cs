using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using System;
using System.Buffers.Text;

namespace Super.Serialization
{
	public delegate uint Advance<T>(Composition<T> parameter);

	public delegate uint Advance(Composition parameter);

	class ElementWriter<T> : IEmit<T>
	{
		readonly IEmit    _start, _finish;
		readonly IEmit<T> _content;

		public ElementWriter(IEmit start, IEmit<T> content, IEmit finish)
		{
			_start   = start;
			_content = content;
			_finish  = finish;
		}

		public Composition Get(Composition<T> parameter)
			=> _finish.Get(_content.Get(_start.Get(parameter).Introduce(parameter.Instance)));
	}

	public interface IEmit : IAlteration<Composition> {}

	class Emit : IEmit
	{
		readonly uint    _size;
		readonly Advance _advance;

		public Emit(IInstruction instruction) : this(instruction.Get(), instruction.Get) {}

		public Emit(uint size, Advance advance)
		{
			_size    = size;
			_advance = advance;
		}

		public Composition Get(Composition parameter)
		{
			var composition = parameter.Index + _size >= parameter.Output.Length
				                  ? new Composition(parameter.Output.Copy(in _size), parameter.Index)
				                  : parameter;
			return new Composition(composition.Output, parameter.Index + _advance(composition));
		}
	}

	public interface IEmit<T> : ISelect<Composition<T>, Composition> {}

	class Emit<T> : IEmit<T>
	{
		readonly uint       _size;
		readonly Advance<T> _advance;

		public Emit(uint size, Advance<T> advance)
		{
			_size    = size;
			_advance = advance;
		}

		public Composition Get(Composition<T> parameter)
		{
			var composition = parameter.Index + _size >= parameter.Output.Length
				                  ? new Composition<T>(parameter.Output.Copy(in _size), parameter.Instance,
				                                       parameter.Index)
				                  : parameter;
			return new Composition(composition.Output, parameter.Index + _advance(composition));
		}
	}

	sealed class PositiveInteger : Emit<uint>
	{
		public static PositiveInteger Default { get; } = new PositiveInteger();

		PositiveInteger()
			: base(10, x => Utf8Formatter.TryFormat(x.Instance, x.Output.AsSpan((int)x.Index), out var count)
				                ? (uint)count
				                : throw new
					                  InvalidOperationException($"Could not format '{x.Instance}' into its UTF-8 equivalent.")) {}
	}
}