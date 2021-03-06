﻿using System;
using System.Linq;
using System.Threading;
using AutoFixture;

namespace Super.Application.Hosting.xUnit
{
	sealed class ManualPropertyTypesCustomization : CompositeCustomization
	{
		public static ManualPropertyTypesCustomization Default { get; } = new ManualPropertyTypesCustomization();

		ManualPropertyTypesCustomization() : this(typeof(Thread)) {}

		public ManualPropertyTypesCustomization(params Type[] types) :
			base(types.Select(x => new NoAutoPropertiesCustomization(x))) {}
	}
}