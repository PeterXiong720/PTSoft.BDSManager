using PTSoft.BDSManager.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace PTSoft.BDSManager.Permissions;

public class BDSManagerPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(BDSManagerPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(BDSManagerPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<BDSManagerResource>(name);
    }
}
