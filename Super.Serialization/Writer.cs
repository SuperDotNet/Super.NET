using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Sequences;
using System;
using System.Buffers;
using System.Buffers.Text;
using System.Runtime.CompilerServices;
using System.Text;

namespace Super.Serialization
{
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

	sealed class Declaration<T> : Instance<uint>, IAdvance<T>
	{
		public static Declaration<T> Default { get; } = new Declaration<T>();

		Declaration() : this(Encoding.UTF8.GetBytes($@"<?xml version=""1.0""?>{Environment.NewLine}")) {}

		
		readonly byte[] _content;

		public Declaration(byte[] content) : base((uint)content.Length) => _content = content;

		public uint Get(Composition<T> parameter)
		{
			_content.CopyTo(parameter.Output, parameter.Index);
			return (uint)_content.Length;
		}
	}

	sealed class OpenContent<T> : Instance<uint>, IAdvance<T>
	{
		readonly byte[] _name;
		readonly byte   _first, _last;

		public OpenContent(string name) : this(Encoding.UTF8.GetBytes(name), Open.Default, Close.Default) {}

		public OpenContent(byte[] name, byte first, byte last) : base((uint)name.Length + 2u)
		{
			_name  = name;
			_first = first;
			_last  = last;
		}

		public uint Get(Composition<T> parameter)
		{
			var output = parameter.Output;
			var result = parameter.Index;
			output[result++] = _first;
			_name.CopyTo(output, result);
			output[result += (uint)_name.Length] = _last;
			return ++result - parameter.Index;
		}
	}

	sealed class CloseContent<T> : Instance<uint>, IAdvance<T>
	{
		readonly byte[] _name;
		readonly byte   _first;
		readonly byte   _marker;
		readonly byte   _last;

		// ReSharper disable once TooManyDependencies
		public CloseContent(string name)
			: this(Encoding.UTF8.GetBytes(name), Open.Default, Marker.Default, Close.Default) {}

		public CloseContent(byte[] name, byte first, byte marker, byte last) : base((uint)name.Length + 3u)
		{
			_name   = name;
			_first  = first;
			_marker = marker;
			_last   = last;
		}

		public uint Get(Composition<T> parameter)
		{
			var output = parameter.Output;
			var result = parameter.Index;
			output[result++] = _first;
			output[result++] = _marker;
			_name.CopyTo(output, result);
			output[result += (uint)_name.Length] = _last;
			return ++result - parameter.Index;
		}
	}

	class XmlElementWriter<T> : ElementWriter<T>
	{
		public XmlElementWriter(string name, IEmit<T> content)
			: base(new Emit<T>(new OpenContent<T>(name)), content, new Emit<T>(new CloseContent<T>(name))) {}
	}

	public class XmlDocumentEmitter<T> : IEmit<T>
	{
		readonly IEmit<T> _declaration;
		readonly IEmit<T> _content;

		public XmlDocumentEmitter(IEmit<T> declaration, IEmit<T> content)
		{
			_declaration = declaration;
			_content     = content;
		}

		public Model.Sequences.Store<byte> Get(Composition<T> parameter)
		{
			var declaration =
				_declaration.Get(new Composition<T>(parameter.Output, parameter.Instance, parameter.Index));
			var result = _content.Get(new Composition<T>(declaration.Instance, parameter.Instance, declaration.Length));
			return result;
		}
	}

	class ElementWriter<T> : IEmit<T>
	{
		readonly IEmit<T> _start, _content, _finish;

		public ElementWriter(IEmit<T> start, IEmit<T> content, IEmit<T> finish)
		{
			_start   = start;
			_content = content;
			_finish  = finish;
		}

		public Model.Sequences.Store<byte> Get(Composition<T> parameter)
		{
			var start   = _start.Get(new Composition<T>(parameter.Output, parameter.Instance, parameter.Index));
			var content = _content.Get(new Composition<T>(start.Instance, parameter.Instance, start.Length));
			var result  = _finish.Get(new Composition<T>(content.Instance, parameter.Instance, content.Length));
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

	class Emit<T> : IEmit<T>
	{
		readonly uint       _size;
		readonly Advance<T> _advance;

		public Emit(IAdvance<T> advance) : this(advance.Get(), advance.Get) {}

		public Emit(uint size, Advance<T> advance)
		{
			_size    = size;
			_advance = advance;
		}

		public Model.Sequences.Store<byte> Get(Composition<T> parameter)
		{
			var composition = parameter.Index + _size >= parameter.Output.Length
				                  ? new Composition<T>(parameter.Output.Copy(in _size), parameter.Instance,
				                                       parameter.Index)
				                  : parameter;
			return new Model.Sequences.Store<byte>(composition.Output, parameter.Index + _advance(composition));
		}
	}

	public interface IEmit<T> : ISelect<Composition<T>, Model.Sequences.Store<byte>> {}

	sealed class EmptyEmit<T> : IEmit<T>
	{
		public static EmptyEmit<T> Default { get; } = new EmptyEmit<T>();

		EmptyEmit() {}

		public Model.Sequences.Store<byte> Get(Composition<T> parameter)
			=> new Model.Sequences.Store<byte>(parameter.Output, parameter.Index);
	}

	sealed class PositiveNumber : Emit<uint>
	{
		public static PositiveNumber Default { get; } = new PositiveNumber();

		PositiveNumber()
			: base(20, x => Utf8Formatter.TryFormat(x.Instance, x.Output.AsSpan((int)x.Index), out var count)
				                ? (uint)count
				                : throw new
					                  InvalidOperationException($"Could not format '{x.Instance}' into its UTF8 equivalent.")) {}
	}

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