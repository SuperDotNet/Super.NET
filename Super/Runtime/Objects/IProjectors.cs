using System;
using Super.Model.Selection;

namespace Super.Runtime.Objects
{
	public interface IProjectors : ISelect<Type, string, Func<object, IProjection>> {}
}