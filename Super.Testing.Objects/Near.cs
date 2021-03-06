﻿using Super.Model.Results;
using Super.Model.Sequences;

namespace Super.Testing.Objects
{
	sealed class Near : Instance<Selection>
	{
		public static Near Default { get; } = new Near();

		Near() : base(new Selection(300, 100)) {}
	}
}