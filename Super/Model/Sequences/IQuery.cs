using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Sources;

namespace Super.Model.Sequences
{
	public interface IQuery<in _, T> : ISource<ISelect<_, T[]>>, ISelect<Definition<T>, IQuery<_, T>>
	{
		Definition<T> Definition { get; }
	}

	public interface IQueryAlteration<_, T> : IAlteration<IQuery<_, T>> {}

	public class QueryAlteration<_, T> : IQueryAlteration<_, T>
	{
		readonly IAlterDefinition<T> _definition;

		public QueryAlteration(IAlterDefinition<T> definition) => _definition = definition;

		public IQuery<_, T> Get(IQuery<_, T> parameter) => _definition.Select(parameter).Get(parameter.Definition);
	}

	public interface IAlterDefinition<T> : IAlteration<Definition<T>> {}

	sealed class Skip<T> : IAlterDefinition<T>
	{
		readonly uint _skip;

		public Skip(uint skip) => _skip = skip;

		public Definition<T> Get(Definition<T> parameter)
			=> new Definition<T>(parameter.Segment,
			                     new Collections.Selection(parameter.Selection.Start + _skip,
			                                               parameter.Selection.Length - _skip));
	}

	sealed class Take<T> : IAlterDefinition<T>
	{
		readonly uint _take;

		public Take(uint take) => _take = take;

		public Definition<T> Get(Definition<T> parameter)
			=> new Definition<T>(parameter.Segment,
			                     new Collections.Selection(parameter.Selection.Start, _take));
	}
}