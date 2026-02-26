//using Lamie.Domain.Repositories;
//using Lamie.Infrastructure.Persistence.Repositories;
//using Lamie.Infrastructure.Persistence;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Lamie.Application.Common.Interfaces;
//using Lamie.Infrastructure.Repositories;

//namespace Lamie.Infrastructure
//{
//    public static class DependencyInjection
//    {
//        public static IServiceCollection AddInfrastructure(
//            this IServiceCollection services,
//            IConfiguration configuration)
//        {
//            services.AddDbContext<AppDbContext>(options =>
//            {
//                options.UseNpgsql(
//                    builder.Configuration.GetConnectionString("Default"),
//                    x => x.MigrationsAssembly("Infrastructure")
//                );

//                options.UseSnakeCaseNamingConvention();
//            });

//            services.AddScoped<ISysUserRepository, SysUserRepository>();
//            services.AddScoped<IProductRepository, ProductRepository>();

//            return services;
//        }
//    }
//}
