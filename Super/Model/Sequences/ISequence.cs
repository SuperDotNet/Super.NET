using Super.Model.Selection;
using Super.Model.Sources;

namespace Super.Model.Sequences
{
	public interface ISequence<in _, T> : ISource<ISelect<_, T[]>>,
	                                      ISelect<IAlterSelection, ISequence<_, T>>,
	                                      ISelect<ISegment<T>, ISequence<_, T>> {}

	
}