﻿using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PTSoft.BDSManager.Data;
using Volo.Abp.DependencyInjection;

namespace PTSoft.BDSManager.EntityFrameworkCore;

public class EntityFrameworkCoreBDSManagerDbSchemaMigrator
    : IBDSManagerDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreBDSManagerDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the BDSManagerDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<BDSManagerDbContext>()
            .Database
            .MigrateAsync();
    }
}
