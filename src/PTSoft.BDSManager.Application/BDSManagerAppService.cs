using System;
using System.Collections.Generic;
using System.Text;
using PTSoft.BDSManager.Localization;
using Volo.Abp.Application.Services;

namespace PTSoft.BDSManager;

/* Inherit your application services from this class.
 */
public abstract class BDSManagerAppService : ApplicationService
{
    protected BDSManagerAppService()
    {
        LocalizationResource = typeof(BDSManagerResource);
    }
}
