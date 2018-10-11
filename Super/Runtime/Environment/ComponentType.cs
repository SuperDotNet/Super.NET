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
using System.Collections.Immutable;
using Implementations = Super.Reflection.Types.Implementations;

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

		ComponentTypesDefinition() : this(Types.Default, ComponentTypesPredicate.Default, x => x.Sort().Access()) {}

		public ComponentTypesDefinition(IArrays<Type> types, ISelectSequence<Type> where,
		                                Func<ISelect<Type, IEnumerable<Type>>, ISelect<Type, ReadOnlyMemory<Type>>>
			                                select)
			: base(types.Select(x => x.AsEnumerable())
			            .Select(where)
			            .Select(x => x.ToImmutableArray())
			            .Select(I<ComponentTypesSelector>.Default)
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

	sealed class SourceDefinition : MakeGenericType
	{
		public static SourceDefinition Default { get; } = new SourceDefinition();

		SourceDefinition() : base(typeof(ISource<>)) {}
	}

	sealed class Selections : ISelect<Type, ISelect<Type, Type>>
	{
		public static Selections Default { get; } = new Selections();

		Selections() : this(In<Type>.Start(Default<Type, Type>.Instance)
		                            .Unless(IsDefinedGenericType.Default, Make.Instance)) {}

		readonly ISelect<Type, ISelect<Type, Type>> _default;

		public Selections(ISelect<Type, ISelect<Type, Type>> @default) => _default = @default;

		public ISelect<Type, Type> Get(Type parameter)
			=> _default.Get(parameter)
			           .Unless(parameter.To(SourceDefinition.Default.Get)
			                            .To(I<Specification>.Default))
			           .Unless(parameter.To(I<Specification>.Default));

		sealed class Make : ISelect<Type, ISelect<Type, Type>>, IActivateMarker<Type>
		{
			public static Make Instance { get; } = new Make();

			Make() : this(TypeMetadata.Default.Select(Implementations.GenericInterfaceImplementations)
			                          .Select(x => new Any(x)),
			              GenericArguments.Default.Select(I<GenericTypeBuilder>.Default)) {}

			readonly ISelect<Type, ISpecification<Type>> _specification;
			readonly ISelect<Type, ISelect<Type, Type>>  _source;

			public Make(ISelect<Type, ISpecification<Type>> specification,
			            ISelect<Type, ISelect<Type, Type>> source)
			{
				_specification = specification;
				_source        = source;
			}

			public ISelect<Type, Type> Get(Type parameter)
				=> IsGenericTypeDefinition.Default
				                          .And(_specification.Get(parameter))
				                          .Then(_source.Get(parameter));
		}

		sealed class Any : ISpecification<Type>, IActivateMarker<ISpecification<Type>>
		{
			readonly ISpecification<IEnumerable<Type>> _specification;

			public Any(ISpecification<Type> specification) : this(new OneItemIs<Type>(specification.IsSatisfiedBy)) {}

			Any(ISpecification<IEnumerable<Type>> specification) => _specification = specification;

			public bool IsSatisfiedBy(Type parameter) => _specification.IsSatisfiedBy(Implementations.GenericInterfaces.Get(parameter).Reference());
		}

		sealed class Specification : Specification<Type, Type>, IActivateMarker<Type>
		{
			public Specification(Type type) : base(new IsAssignableFrom(type).IsSatisfiedBy, Delegates<Type>.Self) {}
		}
	}

	sealed class ComponentTypesSelector : ISelect<Type, IEnumerable<Type>>, IActivateMarker<ImmutableArray<Type>>
	{
		readonly ImmutableArray<Type>            _types;
		readonly ISpecification<Type>            _specification;
		readonly Func<Type, ISelect<Type, Type>> _selections;

		[UsedImplicitly]
		public ComponentTypesSelector(ImmutableArray<Type> types)
			: this(types, IsAssigned<Type>.Default, Selections.Default.Get) {}

		public ComponentTypesSelector(ImmutableArray<Type> types,
		                              ISpecification<Type> specification,
		                              Func<Type, ISelect<Type, Type>> selections)
		{
			_types         = types;
			_specification = specification;
			_selections    = selections;
		}

		public IEnumerable<Type> Get(Type parameter)
		{
			var select = _selections(parameter);
			var length = _types.Length;
			for (var i = 0; i < length; i++)
			{
				var type = select.Get(_types[i]);
				if (_specification.IsSatisfiedBy(type))
				{
					yield return type;
				}
			}
		}
	}
}