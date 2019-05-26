using System;

namespace Super.Model.Sequences.Query
{
	public sealed class Sum32 : Sum32<int>
	{
		public static Sum32 Default { get; } = new Sum32();

		Sum32() : base(i => i) {}
	}

	public class Sum32<T> : Unlimited, IReduce<T, int>
	{
		readonly Func<T, int> _project;

		public Sum32(Func<T, int> project) => _project = project;

		public int Get(Store<T> parameter)
		{
			var to     = parameter.Length;
			var array  = parameter.Instance;
			var result = 0;
			for (var i = 0u; i < to; i++)
			{
				result += _project(array[i]);
			}

			return result;
		}
	}

	public sealed class SumUnsigned32 : SumUnsigned32<uint>
	{
		public static SumUnsigned32 Default { get; } = new SumUnsigned32();

		SumUnsigned32() : base(x => x) {}
	}

	public class SumUnsigned32<T> : Unlimited, IReduce<T, uint>
	{
		readonly Func<T, uint> _project;

		public SumUnsigned32(Func<T, uint> project) => _project = project;

		public uint Get(Store<T> parameter)
		{
			var to     = parameter.Length;
			var array  = parameter.Instance;
			var result = 0u;
			for (var i = 0u; i < to; i++)
			{
				result += _project(array[i]);
			}

			return result;
		}
	}

	public sealed class Sum64 : Sum64<long>
	{
		public static Sum64 Default { get; } = new Sum64();

		Sum64() : base(x => x) {}
	}

	public class Sum64<T> : Unlimited, IReduce<T, long>
	{
		readonly Func<T, long> _project;

		public Sum64(Func<T, long> project) => _project = project;

		public long Get(Store<T> parameter)
		{
			var to     = parameter.Length;
			var array  = parameter.Instance;
			var result = 0L;
			for (var i = 0u; i < to; i++)
			{
				result += _project(array[i]);
			}

			return result;
		}
	}

	public sealed class SumUnsigned64 : SumUnsigned64<ulong>
	{
		public static SumUnsigned64 Default { get; } = new SumUnsigned64();

		SumUnsigned64() : base(x => x) {}
	}

	public class SumUnsigned64<T> : Unlimited, IReduce<T, ulong>
	{
		readonly Func<T, ulong> _project;

		public SumUnsigned64(Func<T, ulong> project) => _project = project;

		public ulong Get(Store<T> parameter)
		{
			var to     = parameter.Length;
			var array  = parameter.Instance;
			var result = 0ul;
			for (var i = 0u; i < to; i++)
			{
				result += _project(array[i]);
			}

			return result;
		}
	}

	public sealed class SumSingle : SumSingle<float>
	{
		public static SumSingle Default { get; } = new SumSingle();

		SumSingle() : base(x => x) {}
	}

	public class SumSingle<T> : Unlimited, IReduce<T, float>
	{
		readonly Func<T, float> _project;

		public SumSingle(Func<T, float> project) => _project = project;

		public float Get(Store<T> parameter)
		{
			var to     = parameter.Length;
			var array  = parameter.Instance;
			var result = 0f;
			for (var i = 0u; i < to; i++)
			{
				result += _project(array[i]);
			}

			return result;
		}
	}

	public sealed class SumDouble : SumDouble<double>
	{
		public static SumDouble Default { get; } = new SumDouble();

		SumDouble() : base(x => x) {}
	}

	public class SumDouble<T> : Unlimited, IReduce<T, double>
	{
		readonly Func<T, double> _project;

		public SumDouble(Func<T, double> project) => _project = project;

		public double Get(Store<T> parameter)
		{
			var to     = parameter.Length;
			var array  = parameter.Instance;
			var result = 0d;
			for (var i = 0u; i < to; i++)
			{
				result += _project(array[i]);
			}

			return result;
		}
	}

	public sealed class SumDecimal : SumDecimal<decimal>
	{
		public static SumDecimal Default { get; } = new SumDecimal();

		SumDecimal() : base(x => x) {}
	}

	public class SumDecimal<T> : Unlimited, IReduce<T, decimal>
	{
		readonly Func<T, decimal> _project;

		public SumDecimal(Func<T, decimal> project) => _project = project;

		public decimal Get(Store<T> parameter)
		{
			var to     = parameter.Length;
			var array  = parameter.Instance;
			var result = 0m;
			for (var i = 0u; i < to; i++)
			{
				result += _project(array[i]);
			}

			return result;
		}
	}
}