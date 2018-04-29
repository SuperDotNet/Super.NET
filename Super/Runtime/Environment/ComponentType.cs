using Super.Model.Collections;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Reflection.Types;
using Super.Runtime.Activation;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Super.Runtime.Environment
{
	sealed class ComponentType : DecoratedSelect<Type, Type>
	{
		public static ComponentType Default { get; } = new ComponentType();

		ComponentType() : base(ComponentTypes.Default.FirstAssigned()) {}
	}

	sealed class ComponentTypesDefinition : DecoratedSource<ISelect<Type, ImmutableArray<Type>>>
	{
		public static ComponentTypesDefinition Default { get; } = new ComponentTypesDefinition();

		ComponentTypesDefinition() : base(Types.Default
		                                       .Select(ComponentTypesPredicate.Default)
		                                       .Select(I<ComponentTypesSelector>.Default.From)
		                                       .Select(x => x.Sort().Enumerate())) {}
	}

	sealed class ComponentTypes : DelegatedInstanceSelector<Type, ImmutableArray<Type>>
	{
		public static ComponentTypes Default { get; } = new ComponentTypes();

		ComponentTypes() : base(ComponentTypesDefinition.Default.Select(x => x.ToStore()).ToContextual()) {}
	}

	sealed class ComponentTypesPredicate : WhereSelector<Type>
	{
		public static ComponentTypesPredicate Default { get; } = new ComponentTypesPredicate();

		ComponentTypesPredicate() : base(CanActivate.Default.IsSatisfiedBy) {}
	}

	sealed class SourceDefinition : GenericTypeAlteration
	{
		public static SourceDefinition Default { get; } = new SourceDefinition();

		SourceDefinition() : base(typeof(ISource<>)) {}
	}

	sealed class Predicates : ISelect<Type, IEnumerable<Func<Type, bool>>>
	{
		public static Predicates Default { get; } = new Predicates();

		Predicates() : this(SourceDefinition.Default.Get,
		                    TypeMetadataSelector.Default
		                                        .Select(I<IsAssignableFrom>.Default.From)
		                                        .Select(x => new Func<Type, bool>(x.IsSatisfiedBy))
		                                        .Get) {}

		readonly Func<Type, Type>             _source;
		readonly Func<Type, Func<Type, bool>> _selector;

		public Predicates(Func<Type, Type> source, Func<Type, Func<Type, bool>> selector)
		{
			_source   = source;
			_selector = selector;
		}

		public IEnumerable<Func<Type, bool>> Get(Type parameter) => parameter.Yield(_source(parameter))
		                                                                     .Select(_selector);
	}

	sealed class ComponentTypesSelector : ISelect<Type, IEnumerable<Type>>, IActivateMarker<IEnumerable<Type>>
	{
		readonly static Func<Type, IEnumerable<Func<Type, bool>>> Predicates = Environment.Predicates.Default.Get;

		readonly Func<Func<Type, bool>, IEnumerable<Type>> _predicate;
		readonly Func<Type, IEnumerable<Func<Type, bool>>> _predicates;

		public ComponentTypesSelector(IEnumerable<Type> types) : this(types.Where, Predicates) {}

		public ComponentTypesSelector(Func<Func<Type, bool>, IEnumerable<Type>> predicate,
		                              Func<Type, IEnumerable<Func<Type, bool>>> predicates)
		{
			_predicate  = predicate;
			_predicates = predicates;
		}

		public IEnumerable<Type> Get(Type parameter) => _predicates(parameter)
		                                                .Select(_predicate)
		                                                .Concat();
	}
}