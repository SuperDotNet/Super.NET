using Super.Model.Selection.Conditions;

namespace Super.Model.Sequences
{
	public interface IArrayMap<in TIn, T> : IConditional<TIn, Array<T>> {}
}