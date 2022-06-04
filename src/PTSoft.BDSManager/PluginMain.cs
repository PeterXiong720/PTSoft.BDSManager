using System.Reflection;
using PTSoft.BDSManager;

namespace PluginMain
{
    public class Plugin
    {
        public static void OnPostInit()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                var assemblyName = new AssemblyName(args.Name);
                string path = Path.Combine("plugins/BDSManager", $"{assemblyName.Name!}.dll");
                return File.Exists(path) ? Assembly.LoadFrom(path) : null;
            };
            try
            {
                Program.Main();
            }
            catch (Exception e)
            {
                Program.Logger.debug.WriteLine(e);
            }
        }
    }
}