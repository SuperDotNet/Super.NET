using Super.Model.Selection;

namespace Super.Model.Sequences.Query {
	public interface ISelector<TFrom, TTo> : ISelect<Array<TFrom>, Array<TTo>> {}
}