using Super.Model.Selection.Alterations;
using Super.Model.Sources;
using Super.Reflection;

namespace Super.Runtime.Execution
{
	sealed class AmbientAlteration<T> : DelegatedAlteration<ISource<T>>
	{
		public static AmbientAlteration<T> Default { get; } = new AmbientAlteration<T>();

		AmbientAlteration() : base(I<Ambient<T>>.Default.From) {}
	}
}