using Super.ExtensionMethods;
using Super.Model.Collections;
using Super.Model.Specifications;
using System;
using System.Reflection;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;

namespace Super.Reflection
{
	sealed class IsGenericType : DelegatedSpecification<TypeInfo>
	{
		public static IsGenericType Default { get; } = new IsGenericType();

		IsGenericType() : base(x => x.IsGenericType) {}
	}

	sealed class GenericTypeDefinitionAlteration : DelegatedAlteration<Type>
	{
		public static GenericTypeDefinitionAlteration Default { get; } = new GenericTypeDefinitionAlteration();

		GenericTypeDefinitionAlteration() : base(x => x.GetGenericTypeDefinition()) {}
	}

	sealed class IsConstructedGenericType : DelegatedSpecification<Type>
	{
		public static IsConstructedGenericType Default { get; } = new IsConstructedGenericType();

		IsConstructedGenericType() : base(x => x.IsConstructedGenericType) {}
	}

	sealed class IsClass : DelegatedSpecification<Type>
	{
		public static IsClass Default { get; } = new IsClass();

		IsClass() : base(x => x.IsClass) {}
	}

	sealed class IsGenericTypeDefinition : DelegatedSpecification<Type>
	{
		public static IsGenericTypeDefinition Default { get; } = new IsGenericTypeDefinition();

		IsGenericTypeDefinition() : base(x => x.IsGenericTypeDefinition) {}
	}

	sealed class HasGenericArguments : AllSpecification<TypeInfo>
	{
		public static HasGenericArguments Default { get; } = new HasGenericArguments();

		HasGenericArguments() : base(IsGenericType.Default, HasAny<Type>.Default.Select(GenericArgumentsSelector.Default)) {}
	}

	sealed class GenericArgumentsSelector : Delegated<TypeInfo, Type[]>
	{
		public static GenericArgumentsSelector Default { get; } = new GenericArgumentsSelector();

		GenericArgumentsSelector() : base(x => x.GenericTypeArguments) {}
	}

	sealed class GenericParametersSelector : Delegated<TypeInfo, Type[]>
	{
		public static GenericParametersSelector Default { get; } = new GenericParametersSelector();

		GenericParametersSelector() : base(x => x.GenericTypeParameters) {}
	}
}