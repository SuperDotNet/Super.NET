using System.Reflection;
using Super.Model.Specifications;
using Super.Reflection;

namespace Super.Runtime.Environment
{
	sealed class IsAssemblyDeployed : AnySpecification<Assembly>
	{
		public static IsAssemblyDeployed Default { get; } = new IsAssemblyDeployed();

		IsAssemblyDeployed() : this(I<AssemblyFileExists>.Default) {}

		public IsAssemblyDeployed(I<AssemblyFileExists> infer)
			: base(infer.From(ExecutableRuntimeFile.Default), infer.Activate(DevelopmentRuntimeFile.Default).Inverse()) {}
	}
}