using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReusableHttpClient.Constants;
using ReusableHttpClient.Exceptions;
using ReusableHttpClient.Services.Abstractions;

namespace ReusableHttpClient.Services;

public class ReusableHttpClient : IReusableHttpClient
{
    private readonly ILogger<ReusableHttpClient> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private HttpClient _httpClient;

    public ReusableHttpClient(ILogger<ReusableHttpClient> logger,
        IHttpClientFactory httpClientFactory, HttpClient httpClient)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _httpClient = httpClient;
    }

    public Task<TResult?> GetAsync<TResult>(string relativePath, CancellationToken cancellationToken = default)
    {
        return SendRequestAsync<TResult>(HttpMethod.Get, relativePath, null, cancellationToken);
    }

    public async Task<string> PostAsync<TResult>(string relativePath, TResult payload, CancellationToken cancellationToken = default)
    {
        return await SendRequestAndReturnStringAsync(HttpMethod.Post, relativePath, payload, cancellationToken);
    }

    public async Task<string> PostAsync(string relativePath, CancellationToken cancellationToken = default)
    {
        return await SendRequestAndReturnStringAsync<object>(HttpMethod.Post, relativePath, null, cancellationToken);
    }

    public Task<TResponse?> PostAsync<TResult, TResponse>(string relativePath, TResult payload, CancellationToken cancellationToken = default)
    {
        return SendRequestAsync<TResponse>(HttpMethod.Post, relativePath, payload, cancellationToken);
    }

    public async Task<string> PatchAsync<TResult>(string relativePath, TResult payload, CancellationToken cancellationToken = default)
    {
        return await SendRequestAndReturnStringAsync(HttpMethod.Patch, relativePath, payload, cancellationToken);
    }

    public Task<TResponse?> PatchAsync<TResult, TResponse>(string relativePath, TResult payload, CancellationToken cancellationToken = default)
    {
        return SendRequestAsync<TResponse>(HttpMethod.Patch, relativePath, payload, cancellationToken);
    }

    public async Task<string> PutAsync<TResult>(string relativePath, TResult payload, CancellationToken cancellationToken = default)
    {
        return await SendRequestAndReturnStringAsync(HttpMethod.Put, relativePath, payload, cancellationToken);
    }

    public Task<TResponse?> PutAsync<TResult, TResponse>(string relativePath, TResult payload, CancellationToken cancellationToken = default)
    {
        return SendRequestAsync<TResponse>(HttpMethod.Put, relativePath, payload, cancellationToken);
    }

    public async Task<string> DeleteAsync(string relativePath, CancellationToken cancellationToken = default)
    {
        return await SendRequestAndReturnStringAsync<object>(HttpMethod.Delete, relativePath, null, cancellationToken);
    }

    public async Task<string> DeleteAsync<TResult>(string relativePath, TResult payload, CancellationToken cancellationToken = default)
    {
        // Fix: logic was incorrectly using PutAsync in original code
        return await SendRequestAndReturnStringAsync(HttpMethod.Delete, relativePath, payload, cancellationToken);
    }

    public void AddHeaderKeyValue(string name, string value)
    {
        _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(name, value);
    }

    public void SetDefaultHttpClient(string clientName)
    {
        // Don't dispose the injected client as it might be managed by the factory or container
        // _httpClient.Dispose(); 
        _httpClient = _httpClientFactory.CreateClient(clientName);
    }

    public void ClearAuthorizationHeader(string scheme)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(scheme, string.Empty);
    }

    public void SetAuthorizationHeader(string scheme, string value)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(scheme, value);
    }

    private void LogHttpRequestException(HttpRequestException ex)
    {
        _logger.LogError("error occured {statusCode} : {message}", ex.StatusCode, ex.Message);
    }

    private async Task<string> SendRequestAndReturnStringAsync<TRequest>(HttpMethod method, string relativePath, TRequest? payload, CancellationToken cancellationToken)
    {
        var (_, responseBody) = await ExecuteRequestAsync(method, relativePath, payload, cancellationToken);
        return responseBody;
    }

    private async Task<TResponse?> SendRequestAsync<TResponse>(HttpMethod method, string relativePath, object? payload, CancellationToken cancellationToken)
    {
        var (_, responseBody) = await ExecuteRequestAsync(method, relativePath, payload, cancellationToken);
        return string.IsNullOrEmpty(responseBody) ? default : JsonConvert.DeserializeObject<TResponse>(responseBody);
    }

    private async Task<(HttpStatusCode StatusCode, string ResponseBody)> ExecuteRequestAsync(HttpMethod method, string relativePath, object? payload, CancellationToken cancellationToken)
    {
        HttpStatusCode statusCode = HttpStatusCode.OK;
        string responseBody = string.Empty;

        try
        {
            var request = new HttpRequestMessage(method, relativePath);

            if (payload != null)
            {
                string json = JsonConvert.SerializeObject(payload);
                request.Content = new StringContent(json, Encoding.UTF8, MediaTypes.ApplicationJson);
            }

            HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);

            statusCode = response.StatusCode;
            responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            response.EnsureSuccessStatusCode();

            return (statusCode, responseBody);
        }
        catch (HttpRequestException ex)
        {
            LogHttpRequestException(ex);
            throw new SimpleHttpRequestException(statusCode, responseBody);
        }
    }
}