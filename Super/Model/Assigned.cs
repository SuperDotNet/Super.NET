using System.Runtime.CompilerServices;

namespace Super.Model
{
	public readonly struct Assigned<T> where T : struct
	{
		public Assigned(T instance, bool assigned = true)
		{
			Instance   = instance;
			IsAssigned = assigned;
		}

		public T Instance { get; }

		public bool IsAssigned { get; }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Or(T defaultValue) => IsAssigned ? Instance : defaultValue;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Or(in T defaultValue) => IsAssigned ? Instance : defaultValue;

		public override int GetHashCode() => Instance.GetHashCode();

		public override string ToString() => IsAssigned ? Instance.ToString() : string.Empty;

		public static implicit operator Assigned<T>(T value) => new Assigned<T>(value);

		//public static explicit operator T(Assigned<T> value) => value.Instance;

		public static implicit operator T(Assigned<T> value) => value.Instance;

		bool Equals(Assigned<T> other) => Instance.Equals(other.Instance);

		public override bool Equals(object obj)
			=> !ReferenceEquals(null, obj) && obj is Assigned<T> other && Equals(other);

		public static bool operator ==(Assigned<T> left, Assigned<T> right) => left.Equals(right);

		public static bool operator !=(Assigned<T> left, Assigned<T> right) => !left.Equals(right);
	}

	/*public sealed class Structure<T> : Variable<T> where T : struct {}

	public interface IStructured<TIn, out TOut> where TIn : struct
	{
		TOut Get(Structure<TIn> parameter);
	}

	public interface IStructured<T> : IStructured<T, T> where T : struct {}

	sealed class Structured<T> : IStructured<T> where T : struct
	{
		public static Structured<T> Default { get; } = new Structured<T>();

		Structured() {}

		public T Get(Structure<T> parameter) => parameter.Get();
	}*/

	/*public static class Extensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TOut Get<TIn, TOut>(this IStructured<TIn, TOut> @this, in TIn parameter) where TIn : struct
			=> @this.Get(StructurePool<TIn>.Default.Get(in parameter));
	}*/

	/*public static class Locals
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ILocal<T> For<T>(in T @this) where T : struct
		{
			var parameter = @this;
			return Locals<T>.Default.Get(ref parameter);
		}
	}

	public interface IReference<TIn, out TOut> where TIn : struct
	{
		TOut Get(ref TIn parameter);
	}

	public interface ILocals<T> : IReference<T, Local<T>> where T : struct {}

	sealed class Locals<T> : ILocals<T> where T : struct
	{
		public static Locals<T> Default { get; } = new Locals<T>();

		Locals() : this(LocalStores<T>.Default.Get(128)) {}

		readonly Local<T>[] _structures;

		public Locals(Local<T>[] structures) => _structures = structures;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Local<T> Get(ref T parameter)
		{
			var result = _structures[0];
			result.Execute(in parameter);
			return result;
		}
	}

	sealed class LocalStores<T> : ISelect<uint, Local<T>[]> where T : struct
	{
		public static LocalStores<T> Default { get; } = new LocalStores<T>();

		LocalStores() {}

		public Local<T>[] Get(uint parameter)
		{
			var result = new Local<T>[parameter];
			for (var i = 0u; i < result.Length; i++)
			{
				result[i] = new Local<T>();
			}

			return result;
		}
	}*/
}