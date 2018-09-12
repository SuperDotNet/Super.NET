// ReSharper disable ComplexConditionExpression

using FluentAssertions;
using Super.Model.Collections;
using Super.Model.Selection;
using Super.Model.Sources;
using System;
using System.Linq;
using Xunit;

namespace Super.Testing.Application.Model.Collections
{
	public sealed class WhereTests
	{
		[Fact]
		void Verify()
		{
			var expected = Enumerable.Range(0, 10_000).Select(x => x).ToArray();

			var buffer  = new Buffer<int>(10);
			var current = buffer;
			for (var i = 0; i < expected.Length; i++)
			{
				current.Append(i);
			}
			current.Flush().Should().Equal(expected);
		}

		public interface ISession<T> : ISelect<State<T>, State<T>?>, ISource<State<T>?> {}

		public interface ISequencer<T> : ISelect<ISession<T>, ReadOnlyMemory<T>> {}

		sealed class Sequencer<T> : ISequencer<T>
		{
			public static Sequencer<T> Default { get; } = new Sequencer<T>();

			Sequencer() {}

			public ReadOnlyMemory<T> Get(ISession<T> parameter)
			{
				/*var builder = Build.Array<T>();
				var state = parameter.Get();
				while (state.HasValue)
				{
					builder.Add(state.Value.Element);
					state = parameter.Get(state.Value);
				}
				var result = builder.ToArray();
				return result;*/
				return ReadOnlyMemory<T>.Empty;
			}
		}

		public interface ISequential<T> : ISelect<object, ISession<T>> {}

		public readonly struct State<T>
		{
			public State(ref T element) => Element = element;

			public T Element { get; }
		}

		public interface IIndex<T> : IIndex<int, T> {}

		sealed class Index<T> : IIndex<T>
		{
			readonly T[] _source;
			readonly int _length;

			public Index(T[] source) : this(source, source.Length) {}

			public Index(T[] source, int length)
			{
				_source = source;
				_length = length;
			}

			public bool Next(ref int index, out T value)
			{
				if (++index < _length)
				{
					value = _source[index];
					return true;
				}

				value = default;
				return false;
			}
		}

		public interface IIndex<T, TElement>
		{
			bool Next(ref T index, out TElement value);
		}


		/*public interface IIterator<T> : ISelect<Iteration<T>?, Iteration<T>?> {}

		sealed class ArrayIterator<T> : IIterator<T>
		{
			readonly T[] _source;
			readonly Iteration<T> _first;

			public ArrayIterator(T[] source) : this(source, new Iteration<T>(source.Length == 0 ? default : source[0], 0)) {}

			public ArrayIterator(T[] source, Iteration<T> first)
			{
				_source = source;
				_first = first;
			}

			public Iteration<T>? Get(Iteration<T>? parameter)
			{
				if (parameter.HasValue)
				{
					var next = parameter.Value.Number + 1;
					if (next < _source.Length)
					{
						return new Iteration<T>(_source[next], next);
					}
				}
				else
				{
					return _first;
				}
				return null;
			}
		}

		public struct Iteration<T>
		{
			public Iteration(T subject, int number)
			{
				Subject = subject;
				Number  = number;
			}

			public T Subject { get; }

			public int Number { get; }
		}*/

		/*public class Iterate<T> : ISelect<ArrayState<T>, ArrayState<T>?>
		{
			public static Iterate<T> Default { get; } = new Iterate<T>();

			Iterate() {}

			public ArrayState<T>? Get(ArrayState<T> parameter)
			{
				var source = parameter.Source;
				return parameter.Index < source.Length
					       ? (ArrayState<T>?)new ArrayState<T>(source, parameter.Index + 1)
					       : null;
			}
		}*/

		/*public struct ArrayState<T>
		{
			public ArrayState(T[] source, int index = 0) : this(source, index, source[index]) {}

			public ArrayState(T[] source, int index, T element)
			{
				Source  = source;
				Index   = index;
				Element = element;
			}

			public T[] Source { get; }

			public int Index { get; }

			public T Element { get; }
		}*/

		/*public interface IIterate<TIteration, T> : ISource<TIteration?> where TIteration : struct IIterate<TIteration, T> {}

		struct ArrayIteration<TIteration, T> : IIterate<ArrayIteration<TIteration, T>, T>
		{
			readonly T[] _source;
			readonly int _index;

			public ArrayIteration(T[] source, int index = 0)
			{
				_source = source;
				_index = index;
			}

			public ArrayIteration<TIteration, T>? Get() => new ArrayIteration<TIteration, T>(_source, _index + 1);
		}

		sealed class Iterate<T> : IIterate<T>
		{
			public State<T>? Get()
			{
				return default;
			}
		}

		sealed class Temp<T> : ISelect<IIterate<T>, ReadOnlyMemory<T>>
		{
			public ReadOnlyMemory<T> Get(IIterate<T> parameter) => default;
		}

		public interface IIterator<T> : ISelect<IIterate<T>, ReadOnlyMemory<T>>
		{

		}

		public class Iterate
		{
			public static IIterate<T> From<T>(T[] source)
		}

		public interface IIteration<T> : ISource<IIteration<T>>
		{

		}

		sealed class Iterator<TIteration, T> : ISelect<TIteration, ReadOnlyMemory<T>> where TIteration : IIterate<T>
		{
			readonly IIteration<T> _iteration;

			public Iterator(IIteration<T> iteration) => _iteration = iteration;

			public ReadOnlyMemory<T> Get(TIteration parameter)
			{
				var current = _iteration.Get(parameter);


				var state = parameter.Get();

				return default;
				return default;
			}
		}

		public readonly struct State<T>
		{
			public State(T current) => Current = current;

			public T Current { get; }
		}*/

		/*[Benchmark]
		public unsafe int Span_Array()
		{
			ReadOnlySpan<int> span = new int[BufferSize];
			fixed (int* ptr = &MemoryMarshal.GetReference(span))
			{
				return Native.SumRef(Unsafe.AsRef<int>(ptr), (UIntPtr)span.Length);
			}
		}*/
	}
}