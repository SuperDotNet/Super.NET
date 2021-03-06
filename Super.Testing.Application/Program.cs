﻿using Super.Application.Hosting.BenchmarkDotNet;
using Super.Testing.Application.Model.Sequences.Query.Construction;

namespace Super.Testing.Application
{
	public class Program
	{
		static void Main(params string[] arguments)
		{
			Configuration.Default.Get(arguments).To(Run.A<ExitTests.Benchmarks>);
		}
	}
}