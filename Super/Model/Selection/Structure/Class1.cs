using System;

namespace Super.Model.Selection.Structure
{
	/*public interface IMutable<T> : ISource<T>, IEnhancedCommand<T> where T : struct {}

	public class Variable<T> : IMutable<T> where T : struct
	{
		T _instance;

		public Variable() : this(default) {}

		public Variable(T instance) => _instance = instance;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Get() => _instance;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Execute(in T parameter)
		{
			_instance = parameter;
		}
	}*/

	/*sealed class EphemeralSelf<T> : IEphemeral<T> where T : struct
	{
		public static EphemeralSelf<T> Default { get; } = new EphemeralSelf<T>();

		EphemeralSelf() {}

		public ref readonly T Get(in T parameter) => ref parameter;
	}*/

	/*public sealed class Local<T> : ILocal<T> where T : struct
	{
		T _instance;

		public Local() : this(default) {}

		public Local(T instance) => _instance = instance;

		public ref readonly T Get() => ref _instance;

		public void Execute(in T parameter)
		{
			_instance = parameter;
		}
	}*/

	/*public interface IEphemeralSelect<T> : ISelect<ILocal<T>, ILocal<T>> where T : struct {}*/

	/*public interface IEphemeralSelect<TIn, TOut> : ISelect<ILocal<TIn>, ILocal<TOut>> where TIn : struct
	                                                                                  where TOut : struct {}*/

	/*public interface ILocalCommand<T> where T : struct
	{
		void Execute(ref ref T parameter);
	}*/

	/*public interface ILocalCommand<T> where T : struct
	{
		void Execute(in T parameter);
	}

	public interface ILocal<T> : ILocalCommand<T> where T : struct
	{
		ref readonly T Get();
	}*/

	/*public interface IEphemeral<TIn, TOut> where TIn : struct where TOut : struct
	{
		ref readonly TOut Get(in TIn parameter);
	}

	public interface IEphemeral<T> : IEphemeral<T, T> where T : struct {}*/

	public interface IStructure<TIn, out TOut> where TIn : struct
	{
		TOut Get(in TIn parameter);
	}

	public interface IStructure<T> : IStructure<T, T> where T : struct {}

	sealed class Self<T> : IStructure<T> where T : struct
	{
		public static Self<T> Default { get; } = new Self<T>();

		Self() {}

		public T Get(in T parameter) => parameter;
	}

	class Structure<TIn, TFrom, TTo> : IStructure<TIn, TTo> where TIn : struct where TFrom : struct
	{
		readonly Result<TIn, TFrom> _source;
		readonly Result<TFrom, TTo> _select;

		public Structure(Result<TIn, TFrom> source, Result<TFrom, TTo> select)
		{
			_select = select;
			_source = source;
		}

		public TTo Get(in TIn parameter) => _select(_source(parameter));
	}

	class StructureSelection<TIn, TFrom, TTo> : IStructure<TIn, TTo> where TIn : struct
	{
		readonly Result<TIn, TFrom> _source;
		readonly Func<TFrom, TTo> _select;

		public StructureSelection(Result<TIn, TFrom> source, Func<TFrom, TTo> select)
		{
			_select = select;
			_source = source;
		}

		public TTo Get(in TIn parameter) => _select(_source(parameter));
	}

	class DelegatedStructure<TIn, TOut> : IStructure<TIn, TOut> where TIn : struct
	{
		readonly Result<TIn, TOut> _select;

		public DelegatedStructure(Result<TIn, TOut> select) => _select = select;

		public TOut Get(in TIn parameter) => _select(in parameter);
	}

	class StructureInput<TIn, TFrom, TTo> : ISelect<TIn, TTo> where TFrom : struct
	{
		readonly Func<TIn, TFrom>      _source;
		readonly Result<TFrom, TTo> _select;

		public StructureInput(Func<TIn, TFrom> source, Result<TFrom, TTo> select)
		{
			_select = select;
			_source = source;
		}

		public TTo Get(TIn parameter) => _select(_source(parameter));
	}

	public delegate TOut Result<TIn, out TOut>(in TIn parameter);

	/*public delegate ref readonly TOut Reference<TIn, TOut>(in TIn parameter);*/
}