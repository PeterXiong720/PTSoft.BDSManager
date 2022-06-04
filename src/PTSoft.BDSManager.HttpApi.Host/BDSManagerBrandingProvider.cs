using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace PTSoft.BDSManager;

[Dependency(ReplaceServices = true)]
public class BDSManagerBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "BDSManager";
}
