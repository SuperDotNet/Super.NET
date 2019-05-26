using System;
using Super.Model.Results;

namespace Super.Runtime.Environment
{
	public class Component<T> : SystemStore<T>
	{
		public Component(Func<T> @default) : this(@default.Start()) {}

		public Component(IResult<T> @default) : base(@default.Unless(ComponentLocator<T>.Default)) {}
	}
}