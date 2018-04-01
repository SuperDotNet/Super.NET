using System.Net.Http;
using Refit;
using Super.Model.Sources;

namespace Super.Services
{
	public sealed class Api<T> : DelegatedSource<HttpClient, T>
	{
		public static Api<T> Default { get; } = new Api<T>();

		Api() : base(RestService.For<T>) {}
	}
}