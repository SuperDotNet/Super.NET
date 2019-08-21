using Refit;
using Super.Model.Selection;
using System.Net.Http;

namespace Super.Application.Services
{
	public sealed class Api<T> : Select<HttpClient, T>
	{
		public static Api<T> Default { get; } = new Api<T>();

		Api() : base(RestService.For<T>) {}
	}
}