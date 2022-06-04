using Volo.Abp.Modularity;

namespace PTSoft.BDSManager;

[DependsOn(
    typeof(BDSManagerApplicationModule),
    typeof(BDSManagerDomainTestModule)
    )]
public class BDSManagerApplicationTestModule : AbpModule
{

}
