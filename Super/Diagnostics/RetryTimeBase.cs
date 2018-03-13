using System;
using Super.Model.Sources;
using Super.Model.Sources.Alterations;

namespace Super.Diagnostics
{
	public abstract class RetryTimeBase : ISource<int, TimeSpan>
	{
		readonly Alter<int> _time;

		protected RetryTimeBase(Alter<int> time) => _time = time;

		public TimeSpan Get(int parameter) => TimeSpan.FromSeconds(_time(parameter));
	}
}