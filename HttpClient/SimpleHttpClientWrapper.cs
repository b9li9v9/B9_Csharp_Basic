using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
namespace SimpleHttpClient;
public class SimpleHttpClientWrapper
{
    private readonly HttpClient _httpClient;

    public SimpleHttpClientWrapper(string baseAddress)
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(baseAddress) };
    }

    public void SetDefaultRequestHeaders(string key, string value)
    {
        _httpClient.DefaultRequestHeaders.Add(key, value);
    }

    public async Task<string> GetAsync(string endpoint)
    {
        var response = await _httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> PostAsync(string endpoint, string jsonContent, Dictionary<string, string> headers = null)
    {
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        // 设置额外的请求头（如果需要）
        if (headers != null)
        {
            foreach (var header in headers)
            {
                _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        var response = await _httpClient.PostAsync(endpoint, content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> PutAsync(string endpoint, string jsonContent, Dictionary<string, string> headers = null)
    {
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        // 设置额外的请求头（如果需要）
        if (headers != null)
        {
            foreach (var header in headers)
            {
                _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        var response = await _httpClient.PutAsync(endpoint, content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> DeleteAsync(string endpoint)
    {
        var response = await _httpClient.DeleteAsync(endpoint);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}