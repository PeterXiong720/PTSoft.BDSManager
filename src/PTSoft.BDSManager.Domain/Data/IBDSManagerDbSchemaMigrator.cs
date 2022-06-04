using System.Threading.Tasks;

namespace PTSoft.BDSManager.Data;

public interface IBDSManagerDbSchemaMigrator
{
    Task MigrateAsync();
}
