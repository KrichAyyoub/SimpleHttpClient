namespace ReusableHttpClient.Services.Abstractions;

public interface IReusableHttpClient
{
    Task<TResult?> GetAsync<TResult>(string relativePath, CancellationToken cancellationToken = default);
    Task<string> PostAsync<TResult>(string relativePath, TResult payload, CancellationToken cancellationToken = default);
    Task<string> PostAsync(string relativePath, CancellationToken cancellationToken = default);
    Task<TResponse?> PostAsync<TResult, TResponse>(string relativePath, TResult payload, CancellationToken cancellationToken = default);
    Task<string> PatchAsync<TResult>(string relativePath, TResult payload, CancellationToken cancellationToken = default);
    Task<TResponse?> PatchAsync<TResult, TResponse>(string relativePath, TResult payload, CancellationToken cancellationToken = default);
    Task<string> PutAsync<TResult>(string relativePath, TResult payload, CancellationToken cancellationToken = default);
    Task<TResponse?> PutAsync<TResult, TResponse>(string relativePath, TResult payload, CancellationToken cancellationToken = default);
    Task<string> DeleteAsync(string relativePath, CancellationToken cancellationToken = default);
    Task<string> DeleteAsync<TResult>(string relativePath, TResult payload, CancellationToken cancellationToken = default);
    public void ClearAuthorizationHeader(string scheme);
    public void SetAuthorizationHeader(string scheme, string value);
    public void AddHeaderKeyValue(string name, string value);
    void SetDefaultHttpClient(string clientName);
}