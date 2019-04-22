using Super.Compose;
using Super.Model.Results;
using Super.Model.Selection;
using Super.Reflection.Types;
using System;

namespace Super.Runtime.Activation
{
	sealed class Activator : Select<Type, object>, IActivator
	{
		public static Activator Default { get; } = new Activator();

		Activator() : base(Start.A.Generic(typeof(ReferenceActivator<>))
		                        .Of.Type<object>()
		                        .As.Result()
		                        .Then()
		                        .Invoke()
		                        .To(x => x.Get().Then())
		                        .Value()
		                        .Get()
		                        .To(Start.A.Selection.Of.System.Type.By.Array().Select)) {}
	}

	public sealed class Activator<T> : DelegatedResult<T>, IActivator<T>
	{
		public static Activator<T> Default { get; } = new Activator<T>();

		Activator() : base(New<T>.Default.Unless(Singleton<T>.Default).Get) {}
	}

	public class FixedActivator<T> : FixedSelection<Type, T>, IActivator<T>
	{
		public FixedActivator(ISelect<Type, T> select) : base(select, Type<T>.Instance) {}
	}
}