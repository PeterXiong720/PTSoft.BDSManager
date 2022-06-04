using PTSoft.BDSManager.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace PTSoft.BDSManager;

[DependsOn(
    typeof(BDSManagerEntityFrameworkCoreTestModule)
    )]
public class BDSManagerDomainTestModule : AbpModule
{

}
