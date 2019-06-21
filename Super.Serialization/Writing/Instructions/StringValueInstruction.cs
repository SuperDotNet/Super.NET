using Super.Model;
using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Selection.Conditions;
using Super.Model.Sequences;
using Super.Runtime.Environment;
using Super.Text;
using System;
using System.Runtime.CompilerServices;

namespace Super.Serialization.Writing.Instructions
{
	sealed class StringInstruction : IInstruction<ReadOnlyMemory<char>>
	{
		public static StringInstruction Default { get; } = new StringInstruction();

		StringInstruction() : this(DefaultComponent<IUtf8>.Default.Get()) {}

		readonly IUtf8 _utf8;

		public StringInstruction(IUtf8 utf8) => _utf8 = utf8;

		public uint Get(Composition<ReadOnlyMemory<char>> parameter)
			=> _utf8.Get(new Utf8Input(parameter.Instance, parameter.Output, parameter.Index));

		public uint Get(ReadOnlyMemory<char> parameter) => (uint)parameter.Length;
	}

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

	public readonly struct Escape
	{
		public Escape(ReadOnlyMemory<char> memory, uint index, uint size)
		{
			Memory = memory;
			Index  = index;
			Size   = size;
		}

		public ReadOnlyMemory<char> Memory { get; }

		public uint Index { get; }

		public uint Size { get; }
	}

	public interface IEscapeString : ISelect<Escape, ReadOnlyMemory<char>> {}

	/// <summary>
	/// ATTRIBUTION: https://github.com/dotnet/corefx/blob/master/src/System.Text.Json/src/System/Text/Json/Writer/JsonWriterHelper.Escaping.cs
	/// </summary>
	sealed class EscapeString : IEscapeString
	{
		public static EscapeString Default { get; } = new EscapeString();

		EscapeString() : this(EscapeIndex.Default, Leases<char>.Default) {}

		readonly IEscapeIndex   _index;
		readonly IStorage<char> _storage;

		public EscapeString(IEscapeIndex index, IStorage<char> storage)
		{
			_index   = index;
			_storage = storage;
		}

		// ReSharper disable once ExcessiveIndentation
		public ReadOnlyMemory<char> Get(Escape parameter)
		{
			var length      = parameter.Memory.Length;
			var size        = (int)parameter.Index;
			var store       = _storage.Get(parameter.Size);
			var source      = parameter.Memory.Span;
			var destination = store.Instance;

			source.Slice(0, size).CopyTo(destination.AsSpan());

			for (var i = size; i < length; i++)
			{
				var character = source[i];

				if (_index.Get(character))
				{
					var next = -1;
					if (IsOrWithin(character, 0xD800, 0xDFFF))
					{
						i++;

						if (i < source.Length && character < 0xDC00)
						{
							throw new InvalidOperationException($"{character} is not a valid escaped character.");
						}

						if (!IsOrWithin(source[i], 0xDC00, 0xDFFF))
						{
							throw
								new InvalidOperationException($"{source[i]} is not a valid an escaped character.");
						}

						next = source[i];
					}

					size += Escape(character, next, destination, size);
				}
				else
				{
					destination[size++] = character;
				}
			}

			var result = destination.AsMemory(0, size);

			_storage.Execute(destination);

			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static bool IsOrWithin(ushort value, ushort lower, ushort upper)
			=> (uint)(value - lower) <= (uint)(upper - lower);

		const string HexFormatString = "x4";

		// ReSharper disable once TooManyArguments
		// ReSharper disable once MethodTooLong
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static int Escape(ushort current, int next, Span<char> destination, int index)
		{
			var i = index;

			destination[i++] = '\\';

			switch (current)
			{
				case '\b':
					destination[i++] = 'b';
					break;
				case '\f':
					destination[i++] = 'f';
					break;
				case '\n':
					destination[i++] = 'n';
					break;
				case '\r':
					destination[i++] = 'r';
					break;
				case '\t':
					destination[i++] = 't';
					break;
				case '\\':
					destination[i++] = '\\';
					break;
				default:
				{
					destination[i++] = 'u';
					{
						i += current.TryFormat(destination.Slice(i), out var written, HexFormatString)
							     ? written
							     : 0;
					}

					if (next != -1)
					{
						destination[i++] = '\\';
						destination[i++] = 'u';
						i += next.TryFormat(destination.Slice(i), out var written, HexFormatString)
							     ? written
							     : 0;
					}
				}

					break;
			}

			return i - index;
		}
	}

	sealed class DefaultStringInstruction : IInstruction<ReadOnlyMemory<char>>
	{
		public static DefaultStringInstruction Default { get; } = new DefaultStringInstruction();

		DefaultStringInstruction() : this(StringInstruction.Default.Get, EscapeString.Default,
		                                  DefaultEscapeFactor.Default) {}

		readonly Func<Composition<ReadOnlyMemory<char>>, uint> _instruction;
		readonly IEscapeString                                 _escape;
		readonly uint                                          _factor;

		public DefaultStringInstruction(Func<Composition<ReadOnlyMemory<char>>, uint> instruction, IEscapeString escape,
		                                uint factor)
		{
			_instruction = instruction;
			_escape      = escape;
			_factor      = factor;
		}

		public uint Get(Composition<ReadOnlyMemory<char>> parameter)
		{
			var index = EscapeIndex.Default.Get(parameter.Instance);
			if (index.IsAssigned)
			{
				var escape = new Escape(parameter.Instance, index,
				                        index.Instance + ((uint)parameter.Instance.Length - index) * _factor);
				return _instruction(parameter.Using(_escape.Get(escape)));
			}

			return _instruction(parameter);
		}

		public uint Get(ReadOnlyMemory<char> parameter) => (uint)parameter.Length * _factor;
	}
}