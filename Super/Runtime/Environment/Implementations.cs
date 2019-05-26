using System;
using Super.Compose;
using Super.Model.Results;
using Super.Model.Selection;
using Super.Reflection.Types;

namespace Super.Runtime.Environment
{
	static class Implementations
	{
		public static ISelect<Type, object> Activator { get; }
			= Start.A.Selection.Of.System.Type.By.Array()
			       .Select(Start.A.Selection<Type>()
			                    .By.StoredActivation<MakeGenericType>()
			                    .In(StorageTypeDefinition.Default)
			                    .Assume())
			       .Select(Activation.Activator.Default);
	}

	static class Implementations<T>
	{
		public static IResult<IStore<T>> Store { get; } = Start.A.Selection<IMutable<T>>()
		                                                       .By.StoredActivation<Store<T>>()
		                                                       .In(SystemStores<T>.Default);
	}
}