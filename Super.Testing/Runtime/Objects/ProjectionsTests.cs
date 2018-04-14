using FluentAssertions;
using Super.ExtensionMethods;
using Super.Runtime.Objects;
using System;
using Xunit;

namespace Super.Testing.Runtime.Objects
{
	public sealed class ProjectionsTests
	{
		[Fact]
		void Verify()
		{
			var projection = ApplicationDomainProjection.Default.Get(AppDomain.CurrentDomain);
			projection.InstanceType.Should().Be(typeof(AppDomain));
			projection.GetDynamicMemberNames()
			          .Should()
			          .BeEquivalentTo(nameof(AppDomain.FriendlyName).Yield(nameof(AppDomain.Id)));
			var    dynamic = projection.To<dynamic>();
			string name    = dynamic.FriendlyName;
			name.Should().Be(AppDomain.CurrentDomain.FriendlyName);
			int id = dynamic.Id;
			id.Should().Be(AppDomain.CurrentDomain.Id);
		}

		[Fact]
		void VerifySelection()
		{
			var projection = ApplicationDomainProjection.Default.Get("F")(AppDomain.CurrentDomain);
			projection.InstanceType.Should().Be(typeof(AppDomain));
			projection.GetDynamicMemberNames()
			          .Should()
			          .BeEquivalentTo(nameof(AppDomain.FriendlyName).Yield(nameof(AppDomain.Id)).Append(nameof(AppDomain.IsFullyTrusted)));
		}

		[Fact]
		void VerifyDefaultSelection()
		{
			var projection = ApplicationDomainProjection.Default.Get(null)(AppDomain.CurrentDomain);
			projection.InstanceType.Should().Be(typeof(AppDomain));
			projection.GetDynamicMemberNames()
			          .Should()
			          .BeEquivalentTo(nameof(AppDomain.FriendlyName).Yield(nameof(AppDomain.Id)));
			var    dynamic = projection.To<dynamic>();
			string name    = dynamic.FriendlyName;
			name.Should().Be(AppDomain.CurrentDomain.FriendlyName);
			int id = dynamic.Id;
			id.Should().Be(AppDomain.CurrentDomain.Id);
		}
	}
}