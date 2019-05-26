using System;
using System.Collections.Generic;

namespace Super.Runtime.Objects
{
	public interface IProjection : IReadOnlyDictionary<string, object>
	{
		Type InstanceType { get; }
	}
}