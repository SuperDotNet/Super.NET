﻿using System;
using FluentAssertions;
using Super.Runtime.Objects;
using Xunit;

namespace Super.Testing.Application.Runtime.Objects
{
	public sealed class ProjectionsTests
	{
		[Fact]
		void Verify()
		{
			var projection = ApplicationDomainProjection.Default.Default(AppDomain.CurrentDomain);
			projection.InstanceType.Should().Be(typeof(AppDomain));
			projection.Keys
			          .Should()
			          .BeEquivalentTo(nameof(AppDomain.FriendlyName).Yield(nameof(AppDomain.Id)));
			var dynamic = projection.ToStore();
			var name    = dynamic.Get("FriendlyName");
			name.Should().Be(AppDomain.CurrentDomain.FriendlyName);
			var id = dynamic.Get("Id");
			id.Should().Be(AppDomain.CurrentDomain.Id);
		}

		[Fact]
		void VerifyDefaultSelection()
		{
			var projection = ApplicationDomainProjection.Default.Get(null)(AppDomain.CurrentDomain);
			projection.InstanceType.Should().Be(typeof(AppDomain));
			projection.Keys
			          .Should()
			          .BeEquivalentTo(nameof(AppDomain.FriendlyName).Yield(nameof(AppDomain.Id)));
			var dynamic = projection.ToStore();
			var name    = dynamic.Get("FriendlyName");
			name.Should().Be(AppDomain.CurrentDomain.FriendlyName);
			var id = dynamic.Get("Id");
			id.Should().Be(AppDomain.CurrentDomain.Id);
		}

		[Fact]
		void VerifySelection()
		{
			var projection = ApplicationDomainProjection.Default.Get("F")(AppDomain.CurrentDomain);
			projection.InstanceType.Should().Be(typeof(AppDomain));
			projection.Keys
			          .Should()
			          .BeEquivalentTo(nameof(AppDomain.FriendlyName)
			                          .Yield(nameof(AppDomain.Id))
			                          .Append(nameof(AppDomain.IsFullyTrusted)));
		}
	}
}