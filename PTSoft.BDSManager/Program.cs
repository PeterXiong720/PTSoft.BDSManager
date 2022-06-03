using LLNET.Event;
using LLNET.LL;
using LLNET.Logger;
using PTSoft.BDSManager.Services;
using Version = LLNET.LL.Version;

namespace PTSoft.BDSManager;

public static class Program
{
    public static readonly Logger Logger = new Logger("BDSManager");
    
    private static readonly PluginService PluginService = new PluginService(Logger);

    private static readonly Timer VersionCheckTimer = new Timer(
        CheckPluginsVersion, null, TimeSpan.Zero, TimeSpan.FromHours(2.0d));

    private static void CheckPluginsVersion(object? state)
    {
        var plugins = PluginService.GetAllPlugins();

        Task.Run(() => Parallel.ForEach(plugins, (plugin) =>
        {
            PluginService.CheckVersion(plugin.Value).Wait();
        }));
    }

    public static void Main()
    {
        LLAPI.RegisterPlugin(
            "PTSoft.BDSManager",
            "",
            new Version(1, 0, 1),
            "https://github.com/PeterXiong720/BDSManager.git",
            "AGPLv3",
            "https://github.com/PeterXiong720/BDSManager/"
        );

        RegisterListener();
    }

    private static void RegisterListener()
    {
        ServerStartedEvent.Event += (ev) =>
        {
            Logger.info.WriteLine("Loaded.");
            return true;
        };
    }
}