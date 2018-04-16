using System.Net.Http;
using Refit;
using Super.Model.Selection;

namespace Super.Services
{
	public sealed class Api<T> : Select<HttpClient, T>
	{
		public static Api<T> Default { get; } = new Api<T>();

		Api() : base(RestService.For<T>) {}
	}
}