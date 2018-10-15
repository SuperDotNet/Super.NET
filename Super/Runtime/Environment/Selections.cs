using Super.Model.Selection;
using Super.Model.Sequences.Query;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Reflection.Types;
using Super.Runtime.Activation;
using System;
using System.Reflection;

namespace Super.Runtime.Environment
{
	sealed class Selections : ISelect<Type, ISelect<Type, Type>>
	{
		public static Selections Default { get; } = new Selections();

		Selections() : this(Start.With(Default<Type, Type>.Instance)
		                         .Out(I<Type>.Default)
		                         .Unless(IsDefinedGenericType.Default, Make.Instance)) {}

		readonly ISelect<Type, ISelect<Type, Type>> _default;

		public Selections(ISelect<Type, ISelect<Type, Type>> @default) => _default = @default;

		public ISelect<Type, Type> Get(Type parameter)
			=> _default.Get(parameter)
			           .Unless(parameter.To(SourceDefinition.Default.Get)
			                            .To(I<Specification>.Default))
			           .Unless(parameter.To(I<Specification>.Default));

		sealed class Specifications : Source<ISelect<Type, ISpecification<Type>>>
		{
			public static Specifications Instance { get; } = new Specifications();

			Specifications() : this(TypeMetadata.Default) {}

			public Specifications(ISelect<Type, TypeInfo> metadata)
				: base(metadata.Select(GenericInterfaceImplementations.Default)
				               .ToDelegate()
				               .Select(I<OneItemIs<Type>>.Default)
				               .Select(metadata.Select(GenericInterfaces.Default).Fixed().Out)) {}
		}

		sealed class Make : ISelect<Type, ISelect<Type, Type>>, IActivateMarker<Type>
		{
			public static Make Instance { get; } = new Make();

			Make() : this(Specifications.Instance.Get(), GenericArguments.Default.Select(I<GenericTypeBuilder>.Default),
			              IsGenericTypeDefinition.Default) {}

			readonly ISelect<Type, ISpecification<Type>> _specification;
			readonly ISelect<Type, ISelect<Type, Type>>  _source;
			readonly ISpecification<Type>                _valid;

			public Make(ISelect<Type, ISpecification<Type>> specification,
			            ISelect<Type, ISelect<Type, Type>> source, ISpecification<Type> valid)
			{
				_specification = specification;
				_source        = source;
				_valid         = valid;
			}

			public ISelect<Type, Type> Get(Type parameter) => _valid.And(_specification.Get(parameter))
			                                                        .Then(_source.Get(parameter));
		}

		sealed class Specification : Specification<Type, Type>, IActivateMarker<Type>
		{
			public Specification(Type type) : base(new IsAssignableFrom(type).IsSatisfiedBy, Delegates<Type>.Self) {}
		}
	}
}