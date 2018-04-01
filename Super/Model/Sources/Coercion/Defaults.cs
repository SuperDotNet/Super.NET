using Super.ExtensionMethods;
using Super.Model.Sources.Alterations;
using Super.Reflection;

namespace Super.Model.Sources.Coercion
{
	sealed class Defaults<TParameter, TResult>
		: ConditionalInstance<ISource<TParameter, TResult>, ISource<TParameter, TResult>>
	{
		public static Defaults<TParameter, TResult> Default { get; } = new Defaults<TParameter, TResult>();

		Defaults() : base(IsTypeSpecification<ISource<TParameter, TResult>, IAlteration<TParameter>>.Default,
		                  Self<TParameter>.Default.Out(I<TResult>.Default),
		                  Default<TParameter, TResult>.Instance) {}
	}
}