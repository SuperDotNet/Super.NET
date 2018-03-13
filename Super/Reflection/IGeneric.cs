using System;
using System.Collections.Immutable;
using System.Reflection;
using Super.Model.Sources;

namespace Super.Reflection
{
	public interface IGeneric<out T> : ISource<ImmutableArray<TypeInfo>, Func<T>> {}
}