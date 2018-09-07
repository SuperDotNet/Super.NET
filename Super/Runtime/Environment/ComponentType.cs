using JetBrains.Annotations;
using Super.Model.Collections;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Reflection.Types;
using Super.Runtime.Activation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Runtime.Environment
{
	sealed class ComponentType : DecoratedSelect<Type, Type>
	{
		public static ComponentType Default { get; } = new ComponentType();

		ComponentType() : base(ComponentTypes.Default.FirstAssigned()) {}
	}

	sealed class ComponentTypesDefinition : DecoratedSource<ISelect<Type, ReadOnlyMemory<Type>>>
	{
		public static ComponentTypesDefinition Default { get; } = new ComponentTypesDefinition();

		ComponentTypesDefinition() : this(Types.Default, ComponentTypesPredicate.Default, x => x.Sort().Materialize()) {}

		public ComponentTypesDefinition(IArray<Type> types, IEnumerableAlteration<Type> where,
		                                Func<ISelect<Type, IEnumerable<Type>>, ISelect<Type, ReadOnlyMemory<Type>>> select)
			: base(types.Select(x => x.AsEnumerable())
			            .Select(where)
			            .Select(I<ComponentTypesSelector>.Default.From)
			            .Select(select)) {}
	}

	sealed class ComponentTypes : DelegatedInstanceSelector<Type, ReadOnlyMemory<Type>>
	{
		public static ComponentTypes Default { get; } = new ComponentTypes();

		ComponentTypes() : base(ComponentTypesDefinition.Default.Select(x => x.ToStore()).ToContextual()) {}
	}

	sealed class ComponentTypesPredicate : WhereSelector<Type>
	{
		public static ComponentTypesPredicate Default { get; } = new ComponentTypesPredicate();

		ComponentTypesPredicate() : this(CanActivate.Default.IsSatisfiedBy) {}

		public ComponentTypesPredicate(Func<Type, bool> @where) : base(@where) {}
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

		[UsedImplicitly]
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