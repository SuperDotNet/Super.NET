namespace Super.Model.Sequences.Query
{
	public class Unlimited : LimitAware
	{
		public Unlimited() : base(Assigned<uint>.Unassigned) {}
	}
}