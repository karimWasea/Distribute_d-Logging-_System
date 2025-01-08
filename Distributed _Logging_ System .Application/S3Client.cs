using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

public class S3Service
{
    private readonly HttpClient _httpClient;
    private readonly string _accessKey;
    private readonly string _secretKey;
    private readonly string _bucketName;
    private readonly string _endpoint;

    public S3Service(IConfiguration configuration)
    {
        _httpClient = new HttpClient();
        _endpoint = configuration["Storage:S3:Endpoint"]; 
        _accessKey = configuration["Storage:S3:AccessKey"];
        _secretKey = configuration["Storage:S3:SecretKey"];
        _bucketName = configuration["Storage:S3:BucketName"];
    }

    private string GenerateAuthorizationHeader(string method, string resourcePath)
    {
        var date = DateTime.UtcNow.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'");
        var signature = GenerateSignature(method, resourcePath, date);

        return $"AWS {this._accessKey}:{signature}";
    }

    private string GenerateSignature(string method, string resourcePath, string date)
    {
        var stringToSign = $"{method}\n\n\n{date}\n/{_bucketName}/{resourcePath}";
        using var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(_secretKey));
        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign));

        return Convert.ToBase64String(hashBytes);
    }

    public async Task UploadLogAsync(string? service, string? level, string? message)
    {
        var timestamp = DateTime.UtcNow;
        var logContent = $"{timestamp:yyyy-MM-ddTHH:mm:ssZ} - {service} - {level}: {message}";
        var logFileName = $"{service}_{timestamp:yyyyMMddTHHmmss}.log";

        var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"{_endpoint}/{logFileName}");
        requestMessage.Headers.Add("Authorization", GenerateAuthorizationHeader("PUT", logFileName));
        requestMessage.Headers.Add("Date", timestamp.ToString("R"));

        requestMessage.Content = new StringContent(logContent, Encoding.UTF8, "text/plain");

        var response = await _httpClient.SendAsync(requestMessage);
        response.EnsureSuccessStatusCode();
    }

    public async Task<string> DownloadLogAsync(string logFileName)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{_endpoint}/{logFileName}");
        requestMessage.Headers.Add("Authorization", GenerateAuthorizationHeader("GET", logFileName));

        var response = await _httpClient.SendAsync(requestMessage);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}