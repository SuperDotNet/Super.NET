using Super.Model.Results;

namespace Super.Model.Sequences.Query
{
	public class LimitAware : Instance<Assigned<uint>>, ILimitAware
	{
		public LimitAware(Assigned<uint> instance) : base(instance) {}
	}
}