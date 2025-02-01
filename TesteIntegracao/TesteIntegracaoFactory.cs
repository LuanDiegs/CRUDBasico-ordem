using CRUDBasico;
using Infra.Repositorio;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MySql;

namespace TesteIntegracao
{
    public class TesteIntegracaoFactory : WebApplicationFactory<IOrdemAPI>, IAsyncLifetime
    {
        private readonly MySqlContainer _dbContainer;

        public TesteIntegracaoFactory()
        {
            _dbContainer = new MySqlBuilder()
                .WithImage("mysql:8.0")
                .Build();
        }
        
        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();

            using (var scope = Services.CreateScope())
            {
                var service = scope.ServiceProvider;
                var context = service.GetRequiredService<OrdemDbContext>();

                await context.Database.EnsureCreatedAsync();
            }
        }

        public async new Task DisposeAsync()
        {
            await _dbContainer.StopAsync();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var connectionString = _dbContainer.GetConnectionString();
            base.ConfigureWebHost(builder);
            builder.ConfigureTestServices(services =>
            {
                // Removemos o contexto anterioor
                services.RemoveAll(typeof(DbContextOptions<OrdemDbContext>));

                // Colocamos o novo contexto do container
                services.AddDbContext<OrdemDbContext>(op =>
                    op.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0))));
            });

        }
    }
}
