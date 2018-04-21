﻿using AutoFixture.Kernel;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Super.Application.Testing
{
	public sealed class DefaultEngineParts : AutoFixture.DefaultEngineParts
	{
		public static DefaultEngineParts Default { get; } = new DefaultEngineParts();

		DefaultEngineParts() : this(OptionalParameterAlteration.Default, GreedyConstructorAlteration.Default) {}

		readonly ImmutableArray<ISpecimenBuilderTransformation> _transformers;

		[UsedImplicitly]
		public DefaultEngineParts(params ISpecimenBuilderTransformation[] transformations)
			: this(transformations.ToImmutableArray()) {}

		[UsedImplicitly]
		public DefaultEngineParts(ImmutableArray<ISpecimenBuilderTransformation> transformers)
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