﻿using Super.Model.Commands;
using Super.Model.Selection;
using System.Buffers;

namespace Super.Model.Sequences
{
	public interface IStores<T> : ISelect<uint, Store<T>> {}

	public sealed class Stores<T> : Select<uint, Store<T>>, IStores<T>
	{
		public static Stores<T> Default { get; } = new Stores<T>();

		Stores() : base(x => new Store<T>(new T[x])) {}
	}

	public sealed class Allotted<T> : IStores<T>
	{
		public static Allotted<T> Default { get; } = new Allotted<T>();

		Allotted() : this(ArrayPool<T>.Shared) {}

		readonly ArrayPool<T> _pool;

		public Allotted(ArrayPool<T> pool) => _pool = pool;

		public Store<T> Get(uint parameter) => new Store<T>(_pool.Rent((int)parameter), parameter);
	}

	sealed class Return<T> : ICommand<T[]>
	{
		public static Return<T> Default { get; } = new Return<T>();

		Return() : this(ArrayPool<T>.Shared) {}

		readonly ArrayPool<T> _pool;

		public Return(ArrayPool<T> pool) => _pool = pool;

		public void Execute(T[] parameter)
		{
			_pool.Return(parameter);
		}
	}

	sealed class Arrays<T> : ISelect<Store<T>, T[]>
	{
		public static Arrays<T> Default { get; } = new Arrays<T>();

		Arrays() : this(Selection.Default) {}

		readonly uint           _start;
		readonly Assigned<uint> _length;

		public Arrays(Selection selection) : this(selection.Start, selection.Length) {}

		public Arrays(uint start, Assigned<uint> length)
		{
			_start  = start;
			_length = length;
		}

		public T[] Get(Store<T> parameter)
		{
			var size = _length.IsAssigned
				           ? _length.Instance
				           : parameter.Length - _start;
			var result = parameter.Instance.CopyInto(new T[size], _start, size);
			return result;
		}
	}

	
}