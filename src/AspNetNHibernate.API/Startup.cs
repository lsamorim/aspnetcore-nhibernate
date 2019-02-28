using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspNetNHibernate.API.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;

namespace AspNetNHibernate.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var cfg = new Configuration();
            cfg.DataBaseIntegration(x =>
            {
                x.ConnectionString = Configuration.GetConnectionString("NHibernateFundConnectionString");
                x.Driver<SqlClientDriver>();
                x.Dialect<MsSql2012Dialect>();
                x.LogSqlInConsole = true;
                x.BatchSize = 10;
            });
            cfg.AddAssembly(Assembly.GetExecutingAssembly());

            var sessionFactory = cfg.BuildSessionFactory();
            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    for (int i = 0; i < 25; i++)
                    {
                        var customer = NewCustomer();
                        session.Save(customer);
                    }

                    tx.Commit();
                }
            }

            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    var customers = session.Query<Customer>()
                        .Where(x => x.Points > 0)
                        .OrderBy(x => x.Points)
                        .Select(x => x);

                    foreach (var c in customers)
                    {
                        Console.WriteLine($"NHibernate: {c.ToString()}");
                    }
                }
            }

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        private Customer NewCustomer()
        {
            return new Customer
            {
                FirstName = "Lucas",
                LastName = "Amorim",
                MemberSince = DateTime.UtcNow,
                Points = 100,
                HasGoldStatus = true,
                CreditRating = CustomerCreditRating.Good,
                Address = new LocationValueObject
                {
                    Street = "Rua Dr. Paulo Fróes Machado, 160",
                    City = "Nova Iguaçu",
                    Province = "RJ",
                    Country = "BR"
                }
            };
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
