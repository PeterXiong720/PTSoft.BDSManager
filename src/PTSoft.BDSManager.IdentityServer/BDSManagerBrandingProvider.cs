using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace PTSoft.BDSManager;

[Dependency(ReplaceServices = true)]
public class BDSManagerBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "BDSManager";
}
