using System.Text.RegularExpressions;
using LLNET.LL;
using LLNET.Logger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Version = System.Version;

namespace PTSoft.BDSManager.Services;

public class PluginService
{
    private readonly Logger _logger;
    public PluginService(Logger logger)
    {
        _logger = logger;
    }
    
    public Plugin GetPlugin(string name)
    {
        return LLAPI.GetPlugin(name, true, true);
    }

    public IEnumerable<string> GetAllPluginNames()
    {
        var plugins = GetAllPlugins();

        return plugins.Select(plugin => plugin.Key).ToList();
    }

    public IDictionary<string, Plugin> GetAllPlugins()
    {
        return LLAPI.GetAllPlugins();
    }

    public async Task CheckVersion(Plugin plugin)
    {
        var data = plugin.Others;
        var git = (from item in data
            where item.Key.ToLower() == "git" || item.Key.ToLower() == "github"
            select item.Value).First();
        if (string.IsNullOrEmpty(git)) return;

        var pluginVersion = new Version(plugin.version.Major, plugin.version.Minor, plugin.version.Revision);

        git = git.Replace("https://", "")
            .Replace("http://", "")
            .Replace("github.com/", "")
            .Replace(".git", "");
        if (git.LastIndexOf('/') == git.Length - 1)
        {
            git = git.Remove(git.Length - 1, 1);
        }
        
        string requestUrl = $"https://api.github.com/repos/{git}/releases";
        
        var (result, targetName, assets) = await Compare(pluginVersion, requestUrl);
        if (!result)
        {
            _logger.warn.WriteLine($"<{plugin.Name}>发现新版本：{targetName}，当前版本{plugin.version}");
            var (name, downloadUrl) = (from item in assets
                where item["state"]?.Value<string>() == "uploaded"
                select (item["name"]?.Value<string>(), item["browser_download_url"]?.Value<string>())).FirstOrDefault();
            if(string.IsNullOrEmpty(downloadUrl))
                return;
            var response = await HttpClientManager.GetStreamAsync(downloadUrl);
                //.Replace("https://github.com", "https://hub.fastgit.xyz"));
            var dirInfo = Directory.CreateDirectory("downloads");
            var savePath = Path.Combine(dirInfo.FullName, name!);
            await using var fileStream = File.Create(savePath);
                //new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
            await response.CopyToAsync(fileStream);
            _logger.info.WriteLine($"新版本<{name}>已下载完毕--> {savePath}");
        }
    }

    private async Task<(bool result, string targetName, IEnumerable<JObject>? assets)> Compare(Version pluginVersion, string requestUrl)
    {
        string responseJson = await HttpClientManager.GetAsync(requestUrl);
        try
        {
            var response = JArray.Parse(responseJson);
            var latest = pluginVersion;
            var latestTargetName = String.Empty; 
            JToken? assets = null;
            var hasUpdate = false;
            foreach (var release in response!)
            {
                var targetName = release["tag_name"]!.Value<string>()!;
                var match = Regex.Match(targetName, @"[0-9]+.[0-9]+.[0-9]+");
                if (!match.Success) continue;

                var version = new System.Version(match.Value);
                if (version.CompareTo(latest) > 0)
                {
                    hasUpdate = true;
                    latest = version;
                    latestTargetName = targetName;
                    assets = release["assets"];
                }
            }
            return (!hasUpdate, latestTargetName, assets?.Values<JObject>())!;
        }
        catch (JsonReaderException)
        {
            return (true, string.Empty, null);
        }
        catch (Exception e)
        {
            _logger.error.WriteLine("Error!");
            Console.Error.WriteLine(e);
            return (true, string.Empty, null);
        }
    }
}