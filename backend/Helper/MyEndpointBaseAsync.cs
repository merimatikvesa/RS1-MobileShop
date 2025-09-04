using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace backend.Helper
{
    public abstract class MyEndpointBaseAsync
    {
        public abstract class WithRequest<TRequest> : EndpointBaseAsync
            .WithRequest<TRequest>
            .WithActionResult
        { }

        public abstract class WithRequest<TRequest, TResponse> : EndpointBaseAsync
            .WithRequest<TRequest>
            .WithActionResult<TResponse>
        { }

        public abstract class WithoutRequest<TResponse> : EndpointBaseAsync
            .WithoutRequest
            .WithActionResult<TResponse>
        { }
    }
}
