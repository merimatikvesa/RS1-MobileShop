using backend.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Endpoints.Test
{
    [Authorize]
    public class GetProtectedEndpoint
        : MyEndpointBaseAsync.WithoutRequest<string>
    {
        [HttpGet("api/test/protected")]
        public override async Task<ActionResult<string>> HandleAsync(
            CancellationToken cancellationToken = default)
        {
            return Ok("You are authorized!");
        }
    }
}
