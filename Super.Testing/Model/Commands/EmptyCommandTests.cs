﻿using Super.Model.Commands;
using Xunit;

namespace Super.Testing.Application.Model.Commands
{
	public class EmptyCommandTests
	{
		[Fact]
		public void Coverage() => EmptyCommand<object>.Default.Execute(null);
	}
}