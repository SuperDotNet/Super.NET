using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Super.Model.Sequences.Collections.Groups
{
	class Groups<T> : IArray<IGroup<T>>
	{
		readonly Func<GroupName, IGroup<T>> _factory;
		readonly ImmutableArray<GroupName>  _phases;

		public Groups(IEnumerable<GroupName> phases) : this(phases, x => new Group<T>(x)) {}

		public Groups(IEnumerable<GroupName> phases, Func<GroupName, IGroup<T>> factory)
			: this(phases.ToImmutableArray(), factory) {}

		public Groups(ImmutableArray<GroupName> phases, Func<GroupName, IGroup<T>> factory)
		{
			_phases  = phases;
			_factory = factory;
		}

		public Array<IGroup<T>> Get() => _phases.Select(_factory).ToArray();
	}
}