using System.Text;

namespace PTSoft.BDSManager;

public static class HttpClientManager
{
    private static HttpClient HttpClient { get; } = Init();

    private static HttpClient Init()
    {
        var httpClient = new HttpClient()
        {
            Timeout = TimeSpan.FromSeconds(10.0d),
        };
        httpClient.DefaultRequestHeaders.Add(
            "user-agent",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/101.0.4951.67 Safari/537.36"
        );
        return httpClient;
    }

    public static async Task<string> GetAsync(string url)
    {
        try
        {
            var response = await HttpClient.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception e)
        {
            Program.Logger.error.WriteLine(e);
            return string.Empty;
        }
    }

    public static async Task<Stream> GetStreamAsync(string url) => await HttpClient.GetStreamAsync(url);

    public static async Task<string> PostAsync(string url, string request)
    {
        var content = new StringContent(request, Encoding.UTF8, "application/json");
        try
        {
            var response = await HttpClient.PostAsync(url, content);
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception e)
        {
            Program.Logger.error.WriteLine(e);
            return string.Empty;
        }
    }
}