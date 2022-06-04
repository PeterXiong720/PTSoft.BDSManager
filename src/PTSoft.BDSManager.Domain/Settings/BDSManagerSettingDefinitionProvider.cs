using Volo.Abp.Settings;

namespace PTSoft.BDSManager.Settings;

public class BDSManagerSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(BDSManagerSettings.MySetting1));
    }
}
