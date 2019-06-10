using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Sequences;
using System;
using System.Buffers;
using System.Buffers.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Super.Serialization
{
	static class Extensions
	{
		readonly static ArrayPool<byte> Pool = ArrayPool<byte>.Shared;

		readonly static Func<int, byte[]> Rent = Pool.Rent;

		readonly static Action<byte[], bool> Return = Pool.Return;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] Copy(this byte[] @this, in uint size)
		{
			var result = @this.CopyInto(Rent((int)checked(@this.Length + Math.Max(size, @this.Length))), 0,
			                            (uint)@this.Length);

			Return(@this, false);

			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Array<byte> Complete(this in Composition @this, ArrayPool<byte> pool)
			=> Complete(@this, new byte[@this.Index], pool);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Array<byte> Complete(in this Composition @this, byte[] into, ArrayPool<byte> pool)
		{
			var result = @this.Output.CopyInto(into, 0u, @this.Index);
			@this.Output.Clear(@this.Index);
			pool.Return(@this.Output);
			return result;
		}
	}

	public interface IWriter<in T> : ISelect<T, Array<byte>> {}

	sealed class NumberWriter : Writer<uint>
	{
		public static NumberWriter Default { get; } = new NumberWriter();

		NumberWriter() : base(PositiveNumber.Default) {}
	}

	public sealed class DefaultBufferSize : Instance<uint>
	{
		public static DefaultBufferSize Default { get; } = new DefaultBufferSize();

		DefaultBufferSize() : base(1024 * 16) {}
	}

	public interface IAdvance : ISelect<Composition, uint>, IResult<uint> {}

	public interface IAdvance<T> : ISelect<Composition<T>, uint>, IResult<uint> {}

	public interface ICharacter : IResult<byte> {}

	public class Character : Instance<byte>, ICharacter
	{
		public Character(char instance) : base((byte)instance) {}
	}

	public sealed class Open : Character
	{
		public static Open Default { get; } = new Open();

		Open() : base('<') {}
	}

	public sealed class Close : Character
	{
		public static Close Default { get; } = new Close();

		Close() : base('>') {}
	}

	public sealed class Marker : Character
	{
		public static Marker Default { get; } = new Marker();

		Marker() : base('/') {}
	}

	class Content : Instance<uint>, IAdvance
	{
		readonly byte[] _content;

		public Content(string content) : this(Encoding.UTF8.GetBytes(content)) {}

		public Content(byte[] content) : base((uint)content.Length) => _content = content;

		public uint Get(Composition parameter)
		{
			var length = (uint)_content.Length;
			_content.CopyInto(parameter.Output, 0, length, parameter.Index);
			return parameter.Index + length;
		}
	}

	sealed class Declaration : Content
	{
		public static Declaration Default { get; } = new Declaration();

		Declaration() : base($@"<?xml version=""1.0""?>{Environment.NewLine}") {}
	}

	sealed class OpenContent : Content
	{
		public OpenContent(string name) : base(Encoding.UTF8.GetBytes(name)
		                                               .Prepend(Open.Default)
		                                               .Append(Close.Default)
		                                               .ToArray()) {}
	}

	sealed class CloseContent : Content
	{
		public CloseContent(string name) : base(Encoding.UTF8.GetBytes(name)
		                                                .Prepend(Marker.Default)
		                                                .Prepend(Open.Default)
		                                                .Append(Close.Default)
		                                                .ToArray()) {}
	}

	class XmlElementWriter<T> : ElementWriter<T>
	{
		public XmlElementWriter(string name, IEmit<T> content)
			: base(new Emit(new OpenContent(name)), content, new Emit(new CloseContent(name))) {}
	}

	public class XmlDocumentEmitter<T> : IEmit<T>
	{
		readonly IEmit    _declaration;
		readonly IEmit<T> _content;

		public XmlDocumentEmitter(IEmit declaration, IEmit<T> content)
		{
			_declaration = declaration;
			_content     = content;
		}

		public Composition Get(Composition<T> parameter)
		{
			var declaration = _declaration.Get(new Composition(parameter.Output, parameter.Index));
			var result =
				_content.Get(new Composition<T>(declaration.Output, parameter.Instance, declaration.Index));
			return result;
		}
	}

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
		{
			var start   = _start.Get(new Composition(parameter.Output, parameter.Index));
			var content = _content.Get(new Composition<T>(start.Output, parameter.Instance, start.Index));
			var result  = _finish.Get(new Composition(content.Output, content.Index));
			return result;
		}
	}

	class Writer<T> : IWriter<T>
	{
		readonly IEmit<T>        _emitter;
		readonly ArrayPool<byte> _pool;
		readonly uint            _size;

		public Writer(IEmit<T> emitter) : this(emitter, DefaultBufferSize.Default) {}

		public Writer(IEmit<T> emitter, uint size) : this(emitter, ArrayPool<byte>.Shared, size) {}

		public Writer(IEmit<T> emitter, ArrayPool<byte> pool, uint size)
		{
			_emitter = emitter;
			_pool    = pool;
			_size    = size;
		}

		public Array<byte> Get(T parameter) => _emitter.Get(new Composition<T>(_pool.Rent((int)_size), parameter))
		                                               .Complete(_pool);
	}

	public delegate uint Advance<T>(Composition<T> parameter);

	public delegate uint Advance(Composition parameter);

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
			return new Composition(composition.Output, _advance(composition));
		}
	}

	class Emit : IEmit
	{
		readonly uint    _size;
		readonly Advance _advance;

		public Emit(IAdvance advance) : this(advance.Get(), advance.Get) {}

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
			return new Composition(composition.Output, _advance(composition));
		}
	}

	public interface IEmit : IAlteration<Composition> {}

	public interface IEmit<T> : ISelect<Composition<T>, Composition> {}

	sealed class EmptyEmit : IEmit
	{
		public static EmptyEmit Default { get; } = new EmptyEmit();

		EmptyEmit() {}

		public Composition Get(Composition parameter) => parameter;
	}

	sealed class EmptyEmit<T> : IEmit<T>
	{
		public static EmptyEmit<T> Default { get; } = new EmptyEmit<T>();

		EmptyEmit() {}

		public Composition Get(Composition<T> parameter) => new Composition(parameter.Output, parameter.Index);
	}

	sealed class PositiveNumber : Emit<uint>
	{
		public static PositiveNumber Default { get; } = new PositiveNumber();

		PositiveNumber()
			: base(20, x => Utf8Formatter.TryFormat(x.Instance, x.Output.AsSpan((int)x.Index), out var count)
				                ? x.Index + (uint)count
				                : throw new
					                  InvalidOperationException($"Could not format '{x.Instance}' into its UTF8 equivalent.")) {}
	}

	public readonly struct Composition
	{
		public Composition(byte[] output, uint index = 0)
		{
			Output = output;
			Index  = index;
		}

		public byte[] Output { get; }

		public uint Index { get; }
	}

	public readonly struct Composition<T>
	{
		public Composition(byte[] output, T instance, uint index = 0)
		{
			Output   = output;
			Instance = instance;
			Index    = index;
		}

		public byte[] Output { get; }

		public T Instance { get; }

		public uint Index { get; }
	}
}