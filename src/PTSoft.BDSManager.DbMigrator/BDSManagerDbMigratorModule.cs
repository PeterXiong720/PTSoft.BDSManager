using PTSoft.BDSManager.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace PTSoft.BDSManager.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(BDSManagerEntityFrameworkCoreModule),
    typeof(BDSManagerApplicationContractsModule)
    )]
public class BDSManagerDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
    }
}
