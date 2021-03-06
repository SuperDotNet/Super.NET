﻿using System;
using System.Collections.Generic;
using AutoFixture;
using AutoFixture.Kernel;
using Super.Model.Commands;
using Super.Model.Sequences.Collections.Commands;

namespace Super.Application.Hosting.xUnit
{
	public class InsertCustomization : Command<IFixture>, ICustomization
	{
		public InsertCustomization(ISpecimenBuilder specimen) : this(specimen, x => 0) {}

		public InsertCustomization(ISpecimenBuilder specimen, Func<IList<ISpecimenBuilder>, int> index)
			: base(SelectCustomizations.Default.Then()
			                           .Terminate(new InsertIntoList<ISpecimenBuilder>(specimen, index))) {}

		public void Customize(IFixture fixture)
		{
			Execute(fixture);
		}
	}
}