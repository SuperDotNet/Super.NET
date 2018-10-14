namespace Super.Model.Sequences.Query {
	sealed class Skip : IAlterSelection
	{
		readonly uint _skip;

		public Skip(uint skip) => _skip = skip;

		public Selection Get(Selection parameter)
			=> new Selection(parameter.Start + _skip, parameter.Length);
	}
}