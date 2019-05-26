using Super.Runtime.Activation;

namespace Super.Model.Sequences
{
	/*public readonly struct Store<T> : IActivateUsing<T[]>
	{
		public static implicit operator Store<T>(T[] instance) => new Store<T>(instance);

		/*public static implicit operator T[](Store<T> instance) => instance.Instance;#1#

		public Store(T[] instance) : this(instance, Assigned<uint>.Unassigned) {}

		public Store(T[] instance, in Assigned<uint> length)
		{
			Instance = instance;
			Length   = length;
		}

		public T[] Instance { get; }

		public Assigned<uint> Length { get; }
	}*/

	public readonly struct Store<T> : IActivateUsing<T[]>
	{
		public static implicit operator Store<T>(T[] instance) => new Store<T>(instance);

		public Store(T[] instance) : this(instance, (uint)instance.Length, false) {}

		public Store(T[] instance, uint length, bool requested = true)
		{
			Instance  = instance;
			Length    = length;
			Requested = requested;
		}

		public T[] Instance { get; }

		public uint Length { get; }

		public bool Requested { get; }
	}
}