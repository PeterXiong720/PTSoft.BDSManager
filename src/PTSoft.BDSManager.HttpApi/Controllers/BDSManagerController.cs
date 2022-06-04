using PTSoft.BDSManager.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace PTSoft.BDSManager.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class BDSManagerController : AbpControllerBase
{
    protected BDSManagerController()
    {
        LocalizationResource = typeof(BDSManagerResource);
    }
}
