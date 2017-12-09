namespace TeamBuilder.App
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System.Data.SqlClient;
    using AutoMapper;

    using TeamBuilder.Data;
    using TeamBuilder.Service;
    using TeamBuilder.App.Core;
    
    public class Application
    {
        public static void Main(string[] args)
        {
            var context = new TeamBuilderContext();
            ResetDatabase(context);
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            var serviceProvider = ConfigureService();
            var engine = new Engine(serviceProvider);
            engine.Run();
        }

        static IServiceProvider ConfigureService()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<TeamBuilderContext>(opt =>
                opt.UseSqlServer(Configuration.ConnectionString));

            serviceCollection.AddAutoMapper(cfg => cfg.AddProfile<TeamBuilderProfile>());

            serviceCollection.AddTransient<EventService>();
            serviceCollection.AddTransient<InvitationService>();
            serviceCollection.AddTransient<TeamService>();
            serviceCollection.AddTransient<UserService>();

            var serviceProvider = serviceCollection.BuildServiceProvider();
            return serviceProvider;
        }

        private static void ResetDatabase(TeamBuilderContext context, bool shouldDeleteDatabase = false)
        {
            if (shouldDeleteDatabase)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            context.Database.EnsureCreated();

            var disableIntegrityChecksQuery = "EXEC sp_MSforeachtable @command1='ALTER TABLE ? NOCHECK CONSTRAINT ALL'";
            context.Database.ExecuteSqlCommand(disableIntegrityChecksQuery);

            var deleteRowsQuery = "EXEC sp_MSforeachtable @command1='DELETE FROM ?'";
            context.Database.ExecuteSqlCommand(deleteRowsQuery);

            var enableIntegrityChecksQuery = "EXEC sp_MSforeachtable @command1='ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'";
            context.Database.ExecuteSqlCommand(enableIntegrityChecksQuery);

            var reseedQuery = "EXEC sp_MSforeachtable @command1='DBCC CHECKIDENT(''?'', RESEED, 0)'";
            try
            {
                context.Database.ExecuteSqlCommand(reseedQuery);
            }
            catch (SqlException) 
            {
            }
        }
    }
}