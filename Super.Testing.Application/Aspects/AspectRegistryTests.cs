﻿using FluentAssertions;
using JetBrains.Annotations;
using Super.Aspects;
using Super.Compose;
using Super.Model.Selection;
using Super.Model.Selection.Conditions;
using Super.Model.Sequences;
using Super.Reflection.Types;
using System;
using Xunit;

namespace Super.Testing.Application.Aspects
{
	public class AspectRegistryTests
	{
		[Fact]
		void Before()
		{
			AspectRegistry.Default.Get().Open().Should().BeEmpty();
		}

		[Fact]
		void Verify()
		{
			AspectRegistry.Default.Get().Open().Should().BeEmpty();
			AspectRegistry.Default.Execute(new Registration(typeof(Aspect<,>)));
			AspectRegistry.Default.Get().Open().Should().HaveCount(1);
			AspectRegistrations<string, int>.Default.Get(GenericArguments.Default.Get(A.Type<ISelect<string, int>>()))
			                                  .Open()
			                                  .Should()
			                                  .HaveCount(1);
		}

		[Fact]
		void VerifyInvalid()
		{
			AspectRegistry.Default.Get().Open().Should().BeEmpty();
			AspectRegistry.Default.Execute(new Registration(Never<Array<Type>>.Default, typeof(Aspect<,>)));
			AspectRegistry.Default.Get().Open().Should().HaveCount(1);
			AspectRegistrations<string, int>.Default.Get(GenericArguments.Default.Get(A.Type<ISelect<string, int>>()))
			                                  .Open()
			                                  .Should()
			                                  .BeEmpty();
		}

		[Fact]
		void After()
		{
			AspectRegistry.Default.Get().Open().Should().BeEmpty();
		}

		/*sealed class Subject<TIn, TOut> : ISelect<TIn, TOut>
		{
			public TOut Get(TIn parameter) => default;
		}*/

		sealed class Aspect<TIn, TOut> : IAspect<TIn, TOut>
		{
			[UsedImplicitly]
			public static Aspect<TIn, TOut> Default { get; } = new Aspect<TIn, TOut>();

			Aspect() {}

			public ISelect<TIn, TOut> Get(ISelect<TIn, TOut> parameter) => null;
		}
	}
}