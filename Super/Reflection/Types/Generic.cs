using System;

namespace Super.Reflection.Types
{
	public class Generic<T> : GenericDefinition<Func<T>>
	{
		public Generic(Type definition) : base(definition, Activator.Default) {}

		sealed class Activator : GenericActivator<Func<T>>
		{
			public static Activator Default { get; } = new Activator();

			Activator() : base(GenericSingleton.Default) {}
		}
	}

	public class Generic<T1, T> : GenericDefinition<Func<T1, T>>
	{
		public Generic(Type definition) : base(definition, Activator.Default) {}

		sealed class Activator : GenericActivator<Func<T1, T>>
		{
			public static Activator Default { get; } = new Activator();

			Activator() : base(typeof(T1)) {}
		}
	}

	public class Generic<T1, T2, T> : GenericDefinition<Func<T1, T2, T>>
	{
		public Generic(Type definition) : base(definition, Activator.Default) {}

		sealed class Activator : GenericActivator<Func<T1, T2, T>>
		{
			public static Activator Default { get; } = new Activator();

			Activator() : base(typeof(T1), typeof(T2)) {}
		}
	}

	public class Generic<T1, T2, T3, T> : GenericDefinition<Func<T1, T2, T3, T>>
	{
		public Generic(Type definition) : base(definition, Activator.Default) {}

		sealed class Activator : GenericActivator<Func<T1, T2, T3, T>>
		{
			public static Activator Default { get; } = new Activator();

			Activator() : base(typeof(T1), typeof(T2), typeof(T3)) {}
		}
	}

	public class Generic<T1, T2, T3, T4, T> : GenericDefinition<Func<T1, T2, T3, T4, T>>
	{
		public Generic(Type definition) : base(definition, Activator.Default) {}

		sealed class Activator : GenericActivator<Func<T1, T2, T3, T4, T>>
		{
			public static Activator Default { get; } = new Activator();

			Activator() : base(typeof(T1), typeof(T2), typeof(T3)) {}
		}
	}
}