namespace Super.Model.Sequences.Query {
	sealed class Take : IAlterSelection
	{
		readonly uint _take;

		public Take(uint take) => _take = take;

		public Selection Get(Selection parameter)
			=> new Selection(parameter.Start, _take);
	}
}