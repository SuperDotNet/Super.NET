﻿using System.Collections.Generic;
using AutoFixture;
using AutoFixture.Kernel;
using Super.Model.Selection;

namespace Super.Application.Hosting.xUnit
{
	sealed class SelectCustomizations : Select<IFixture, IList<ISpecimenBuilder>>
	{
		public static SelectCustomizations Default { get; } = new SelectCustomizations();

		SelectCustomizations() : base(x => x.Customizations) {}
	}
}