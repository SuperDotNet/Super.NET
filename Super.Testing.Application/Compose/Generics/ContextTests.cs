﻿using FluentAssertions;
using JetBrains.Annotations;
using Super.Compose;
using Super.Model.Results;
using Super.Model.Selection.Alterations;
using Xunit;

namespace Super.Testing.Application.Compose.Generics
{
	public sealed class ContextTests
	{
		sealed class Subject<T> : IResult<string>
		{
			[UsedImplicitly]
			public static Subject<T> Default { get; } = new Subject<T>();

			Subject() {}

			public string Get() => typeof(T).AssemblyQualifiedName;
		}

		sealed class SelectedSubject<T> : IAlteration<int>
		{
			readonly uint _seed;

			public SelectedSubject(uint seed) => _seed = seed;

			public int Get(int parameter) => (int)(parameter + _seed) + typeof(T).AssemblyQualifiedName.Length;
		}

		[Fact]
		void Verify()
		{
			var parameters = typeof(int);
			Start.A.Generic(typeof(Subject<>))
			     .Of.Type<string>()
			     .As.Result()
			     .Get(parameters)()
			     .Get()
			     .Should()
			     .Be(parameters.AssemblyQualifiedName);
		}

		[Fact]
		void VerifyParameter()
		{
			const uint start      = 6776u;
			const int  parameter  = 123;
			var        parameters = typeof(ContextTests);
			var        expected   = start + parameter + parameters.AssemblyQualifiedName.Length;
			Start
				.A.Generic(typeof(SelectedSubject<>))
				.Of.Type<IAlteration<int>>()
				.WithParameterOf<uint>()
				.Get(parameters)(start)
				.Get(parameter)
				.Should()
				.Be((int)expected);
		}
	}
}