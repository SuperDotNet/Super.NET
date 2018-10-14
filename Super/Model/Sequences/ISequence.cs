using Super.Model.Selection;
using Super.Model.Sequences.Query;
using Super.Model.Sources;

namespace Super.Model.Sequences
{
	public interface ISequence<in _, T> : ISource<ISelect<_, T[]>>,
	                                      ISelect<IElement<T>, ISelect<_, T>>,
	                                      ISelect<IAlterSelection, ISequence<_, T>>,
	                                      ISelect<ISelectView<T>, ISequence<_, T>> {}
}