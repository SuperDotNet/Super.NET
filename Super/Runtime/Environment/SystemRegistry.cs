using System;
using Super.Model.Results;
using Super.Model.Sequences;
using Super.Runtime.Activation;

namespace Super.Runtime.Environment
{
	public class SystemRegistry<T> : Assume<Array<T>>, IRegistry<T>
	{
		readonly Func<IRegistry<T>> _result;

		public SystemRegistry() : this(New<Registry<T>>.Default.Get) {}

		public SystemRegistry(Func<IRegistry<T>> registry) : this(registry.To(SystemStores.New)) {}

		public SystemRegistry(IResult<IRegistry<T>> result)
			: this(result.Get, result.AsDefined().Then().Delegate().Selector()) {}

		public SystemRegistry(Func<IRegistry<T>> result, Func<Func<Array<T>>> get) : base(get)
			=> _result = result;

		public void Execute(Model.Sequences.Store<T> parameter)
		{
			_result().Execute(parameter);
		}

		public void Execute(T parameter)
		{
			_result().Execute(parameter);
		}
	}
}