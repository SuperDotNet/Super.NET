using Super.Compose.Results;
using Super.Runtime;
using Super.Runtime.Activation;
using Super.Runtime.Environment;
using System;
using System.Reflection;

namespace Super.Compose.Extents
{
	public sealed class Extents
	{
		public static Extents Default { get; } = new Extents();

		Extents() {}

		public Extent Of => Extent.Default;
	}

	public sealed class Location<T>
	{
		public static Location<T> Default { get; } = new Location<T>();

		Location() {}

		public T New() => New<T>.Default.Get();

		public T Activate() => Activator<T>.Default.Get();

		public T Locate() => ComponentLocator<T>.Default.Get();
	}

	public sealed class Activation<T>
	{
		public static Activation<T> Instance { get; } = new Activation<T>();

		Activation() {}

		public T New() => Location<T>.Default.New();

		public T Activate() => Location<T>.Default.Activate();

		public T Locate() => Location<T>.Default.Locate();

		public T Singleton() => Singleton<T>.Default.Get();

		public T Default => default;
	}

	public sealed class Extent
	{
		public static Extent Default { get; } = new Extent();

		Extent() {}

		public SystemExtents System => SystemExtents.Default;

		public Extent<object> Any => Extent<object>.Default;

		public Extent<None> None => Extent<None>.Default;

		public Extent<T> Type<T>() => Extent<T>.Default;
	}

	public sealed class SystemExtents
	{
		public static SystemExtents Default { get; } = new SystemExtents();

		SystemExtents() {}

		public Extent<Type> Type => Extent<Type>.Default;

		public Extent<TypeInfo> Metadata => Extent<TypeInfo>.Default;
	}

	public class Extent<T>
	{
		public static Extent<T> Default { get; } = new Extent<T>();

		Extent() {}

		public ExtentSelection<T> Into => ExtentSelection<T>.Default;

		public Activation<T> Activation => Activation<T>.Instance;
	}

	public sealed class ExtentSelection<T>
	{
		public static ExtentSelection<T> Default { get; } = new ExtentSelection<T>();

		ExtentSelection() {}

		public Context<T> Result => Context<T>.Instance;

		public Conditions.Context<T> Condition => Conditions.Context<T>.Instance;

		public Commands.Context<T> Command => Commands.Context<T>.Instance;

		public Selections.Context<T> Selection => Selections.Context<T>.Instance;
	}
}