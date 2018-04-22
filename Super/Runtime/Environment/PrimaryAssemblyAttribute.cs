using Super.Model.Sources;
using Super.Reflection;
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

		HostingAssembly() : base(Attribute<HostingAttribute>.Default.Select(PrimaryAssembly.Default).Select().Get()) {}
	}
}