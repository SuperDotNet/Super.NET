using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Super.Model.Collections
{
	// ReSharper disable once PossibleInfiniteInheritance
	class Groups<T> : Enumerable<IGroup<T>>
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

		public override IEnumerator<IGroup<T>> GetEnumerator() => _phases.Select(_factory).GetEnumerator();
	}
}