using Super.Model.Collections;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Sources;
using System.Runtime.CompilerServices;

namespace Super.Model.Sequences
{
	public interface IQuery<in _, T> : ISource<ISelect<_, T[]>>, ISelect<IAlterSelection<T>, IQuery<_, T>> {}

	public interface IAlterSelection<T> : IAlteration<ISelection<T>> {}

	sealed class Skip<T> : IAlterSelection<T>
	{
		readonly uint _skip;

		public Skip(uint skip) => _skip = skip;

		public ISelection<T> Get(ISelection<T> parameter) => new Selection(parameter, _skip);

		sealed class Selection : ISelection<T>
		{
			readonly ISelection<T> _selection;
			readonly uint          _skip;

			public Selection(ISelection<T> selection, uint skip)
			{
				_selection = selection;
				_skip      = skip;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public Session<T> Get(in ArrayView<T> parameter)
			{
				return _selection.Get(new ArrayView<T>(parameter.Array, parameter.Start + _skip,
				                                       parameter.Length));
			}
		}
		/*sealed class Selection : ISelection<T>
		{
			readonly ISelection<T> _selection;
			readonly uint          _skip;

			public Selection(ISelection<T> selection, uint skip)
			{
				_selection = selection;
				_skip      = skip;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ILocal<Session<T>> Get(ILocal<ArrayView<T>> parameter)
			{
				var current = parameter.Get();
				return _selection.Get(Locals.For(new ArrayView<T>(current.Array, current.Start + _skip,
				                                                  current.Length)));
			}
		}*/
	}

	sealed class Take<T> : IAlterSelection<T>
	{
		readonly uint _take;

		public Take(uint take) => _take = take;

		public ISelection<T> Get(ISelection<T> parameter) => new Selection(parameter, _take);

		/*sealed class Selection : ISelection<T>
		{
			readonly ISelection<T> _selection;
			readonly uint          _take;

			public Selection(ISelection<T> selection, uint take)
			{
				_selection = selection;
				_take      = take;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ILocal<Session<T>> Get(ILocal<ArrayView<T>> parameter)
			{
				var current = parameter.Get();
				return _selection.Get(Locals.For(new ArrayView<T>(current.Array, current.Start, _take)));
			}
		}*/
		sealed class Selection : ISelection<T>
		{
			readonly ISelection<T> _selection;
			readonly uint          _take;

			public Selection(ISelection<T> selection, uint take)
			{
				_selection = selection;
				_take      = take;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public Session<T> Get(in ArrayView<T> parameter)
				=> _selection.Get(new ArrayView<T>(parameter.Array, parameter.Start, _take));
		}
	}

	/*class SegmentAlteration<T> : IAlterSelection<T>
	{
		readonly ISegment<T> _segment;

		public SegmentAlteration(ISegment<T> segment) => _segment = segment;

		public Definition<T> Get(Definition<T> parameter)
			=> new Definition<T>(new Segment<T>(parameter.Segment.Select(_segment).Get));
	}*/

	/*sealed class Where<T> : SegmentAlteration<T>
	{
		public Where(Func<T, bool> where) : base(new Segment(where)) {}

		sealed class Segment : ISegment<T>
		{
			readonly Func<T, bool> _where;

			public Segment(Func<T, bool> where) => _where = @where;

			public ArrayView<T> Get(in ArrayView<T> parameter)
			{
				var used  = parameter.Length;
				var array = parameter.Array;
				var count = 0u;
				for (var i = 0; i < used; i++)
				{
					var item = array[i];
					if (_where(item))
					{
						array[count++] = item;
					}
				}

				return parameter.Resize(count);
			}
		}
	}*/
}