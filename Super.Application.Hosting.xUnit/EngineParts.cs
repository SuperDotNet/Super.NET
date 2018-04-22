using System.Collections.Generic;
using System.Collections.Immutable;
using AutoFixture;
using AutoFixture.Kernel;
using JetBrains.Annotations;
using Super.Model.Collections;

namespace Super.Application.Hosting.xUnit
{
	sealed class DefaultTransformations : Items<ISpecimenBuilderTransformation>
	{
		public static DefaultTransformations Default { get; } = new DefaultTransformations();

		DefaultTransformations() : base(OptionalParameterAlteration.Default, GreedyConstructorAlteration.Default) {}
	}

	public sealed class EngineParts : DefaultEngineParts
	{
		public static EngineParts Default { get; } = new EngineParts();

		EngineParts() : this(DefaultTransformations.Default.ToImmutableArray()) {}

		readonly ImmutableArray<ISpecimenBuilderTransformation> _transformers;

		public EngineParts(IEnumerable<ISpecimenBuilderTransformation> transformations)
			: this(transformations.ToImmutableArray()) {}

		[UsedImplicitly]
		public EngineParts(ImmutableArray<ISpecimenBuilderTransformation> transformers)
			=> _transformers = transformers;

		public override IEnumerator<ISpecimenBuilder> GetEnumerator()
		{
			using (var enumerator = base.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					yield return Transform(enumerator.Current);
				}
			}
		}

		ISpecimenBuilder Transform(ISpecimenBuilder current)
		{
			foreach (var transformer in _transformers)
			{
				var transformed = transformer.Transform(current);
				if (transformed != null)
				{
					return transformed;
				}
			}

			return current;
		}
	}
}