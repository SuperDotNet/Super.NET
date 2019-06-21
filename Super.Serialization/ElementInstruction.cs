using Super.Serialization.Writing.Instructions;

namespace Super.Serialization
{
	class ElementInstruction<T> : IInstruction<T>
	{
		readonly IInstruction    _start, _finish;
		readonly IInstruction<T> _content;

		public ElementInstruction(IInstruction start, IInstruction<T> content, IInstruction finish)
		{
			_start   = start;
			_content = content;
			_finish  = finish;
		}

		public uint Get(Composition<T> parameter)
		{
			var start       = _start.Get(parameter);
			var composition = new Composition<T>(parameter.Output, parameter.Instance, parameter.Index + start);
			var content     = _content.Get(composition);
			var result = start + content +
			                  _finish.Get(new Composition(parameter.Output, composition.Index + content));
			return result;
		}

		public uint Get(T parameter) => _start.Get() + _content.Get(parameter) + _finish.Get();
	}
}