using System.Net;
using System.Text.Json;
using RecipesService.EndpointHelpers;
using System.Net.Http.Headers;


namespace RecipesService;

public class RemoteAuthHandler
{
    private readonly RequestDelegate _next;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public RemoteAuthHandler(RequestDelegate next, IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _next = next;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint?.Metadata.GetMetadata<AuthenticateAttribute>() != null)
        {
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }
            var header = context.Request.Headers["Authorization"].ToString();
            if (!header.StartsWith("Bearer "))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }


            var sessionKey = header.Substring(7); // 6 or 7?
            SessionInfoDto sessionInfo = await CallDecodeSession(sessionKey);

            if (sessionInfo == null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }

            context.Items["SessionInfo"] = sessionInfo;
        }

        await _next(context);
    }

private async Task<SessionInfoDto> CallDecodeSession(string sessionKey)
    {
        var client = _httpClientFactory.CreateClient();
        var authUrl = _configuration["AuthServiceUrl"] + "/api/auth/decode-session";

        var payload = new { SessionKey = sessionKey };
        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Post, authUrl)
        {
            Content = content
        };
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        try
        {
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var result = JsonSerializer.Deserialize<SessionInfoDto>(responseBody, options);

            return result;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}
