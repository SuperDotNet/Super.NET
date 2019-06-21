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
		public Escape(ReadOnlyMemory<char> source, uint start, char[] destination)
		{
			Source      = source;
			Start       = start;
			Destination = destination;
		}

		public ReadOnlyMemory<char> Source { get; }

		public uint Start { get; }
		public char[] Destination { get; }
	}

	public interface IEscapeString : ISelect<Escape, ReadOnlyMemory<char>> {}

	/// <summary>
	/// ATTRIBUTION: https://github.com/dotnet/corefx/blob/master/src/System.Text.Json/src/System/Text/Json/Writer/JsonWriterHelper.Escaping.cs
	/// </summary>
	sealed class EscapeString : IEscapeString
	{
		const           string Format = "x4";
		readonly static int    Start  = Ranges.Default.Low.Start.Value;

		public static EscapeString Default { get; } = new EscapeString();

		EscapeString() : this(EscapeIndex.Default, Ranges.Default.Normal) {}

		readonly IEscapeIndex _index;
		readonly Range        _normal;

		public EscapeString(IEscapeIndex index, Range normal)
		{
			_index  = index;
			_normal = normal;
		}

		// ReSharper disable once ExcessiveIndentation
		public ReadOnlyMemory<char> Get(Escape parameter)
		{
			var index  = (int)parameter.Start;
			var source = parameter.Source.Span;

			source.Slice(0, index).CopyTo(parameter.Destination);

			var length = parameter.Source.Length;
			for (var i = index; i < length; i++)
			{
				var character = source[i];

				if (_index.Get(character))
				{
					var next = -1;
					if (_normal.IsOrBetween(character))
					{
						i++;

						if (i < source.Length && character < Start)
						{
							throw new InvalidOperationException($"{character} is not a valid escaped character.");
						}

						if (!Ranges.Default.Low.IsOrBetween(source[i]))
						{
							throw
								new InvalidOperationException($"{source[i]} is not a valid an escaped character.");
						}

						next = source[i];
					}

					index += Escape(character, next, parameter.Destination, index);
				}
				else
				{
					parameter.Destination[index++] = character;
				}
			}

			return parameter.Destination.AsMemory(0, index);
		}

		// ReSharper disable once TooManyArguments
		// ReSharper disable once MethodTooLong
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static int Escape(ushort current, int next, char[] destination, int index)
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
					var span = destination.AsSpan();
					destination[i++] = 'u';
					{
						i += current.TryFormat(span.Slice(i), out var written, Format)
							     ? written
							     : 0;
					}

					if (next != -1)
					{
						destination[i++] = '\\';
						destination[i++] = 'u';
						i += next.TryFormat(span.Slice(i), out var written, Format)
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

		DefaultStringInstruction()
			: this(StringInstruction.Default, EscapeString.Default, DefaultEscapeFactor.Default) {}

		readonly IInstruction<ReadOnlyMemory<char>> _instruction;
		readonly IEscapeString                      _escape;
		readonly uint                               _factor;

		// ReSharper disable once TooManyDependencies
		public DefaultStringInstruction(IInstruction<ReadOnlyMemory<char>> instruction,
		                                IEscapeString escape,
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
				using (var lease = Leases<char>.Default
				                               .Session(index.Instance +
				                                        ((uint)parameter.Instance.Length - index) * _factor))
				{
					return _instruction.Get(parameter.Replace(_escape.Get(new Escape(parameter.Instance, index,
					                                                                 lease.Store))));
				}
			}

			return _instruction.Get(parameter);
		}

		public uint Get(ReadOnlyMemory<char> parameter) => (uint)parameter.Length * _factor;
	}
}