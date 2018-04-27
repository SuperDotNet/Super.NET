using Super.Model.Sources;
using System;
using System.Reflection;

namespace Super.Runtime.Environment
{
	public interface IHosting : ISource<Assembly> {}

	[AttributeUsage(AttributeTargets.Assembly)]
	public abstract class HostingAttribute : Attribute, IHosting
	{
		readonly Assembly _assembly;

		protected HostingAttribute(Assembly assembly) => _assembly = assembly;

		public Assembly Get() => _assembly;
	}

	public sealed class HostingAssembly : Source<Assembly>
	{
		public static HostingAssembly Default { get; } = new HostingAssembly();

		HostingAssembly() : base(PrimaryAssembly.Default
		                                        .Select(x => x.Attribute<HostingAttribute>().Get())
		                                        .Get()) {}
	}
}