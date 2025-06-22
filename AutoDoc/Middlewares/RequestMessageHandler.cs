using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace AutoDocFront.Middlewares
{
    public class RequestMessageHandler : DelegatingHandler
    {
        private readonly ILogger<RequestMessageHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;

        public RequestMessageHandler(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ILogger<RequestMessageHandler> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
            _logger = logger;
        }



        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;

                if (httpContext == null) return null;

                string? token = await httpContext.GetTokenAsync("access_token");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return await base.SendAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, $"{nameof(RequestMessageHandler)}: {nameof(SendAsync)}");
                throw;
            }

        }
    }
}
