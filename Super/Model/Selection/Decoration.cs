namespace Super.Model.Selection
{
	public readonly struct Decoration<TParameter, TResult>
	{
		public Decoration(TParameter parameter, TResult result)
		{
			Parameter = parameter;
			Result    = result;
		}

		public TParameter Parameter { get; }

		public TResult Result { get; }
	}
}