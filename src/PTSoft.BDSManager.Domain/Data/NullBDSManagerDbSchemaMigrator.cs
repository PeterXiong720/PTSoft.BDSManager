using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace PTSoft.BDSManager.Data;

/* This is used if database provider does't define
 * IBDSManagerDbSchemaMigrator implementation.
 */
public class NullBDSManagerDbSchemaMigrator : IBDSManagerDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
