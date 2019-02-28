using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspNetNHibernate.API.Entities;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
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
            //var cfg = new Configuration();
            //cfg.DataBaseIntegration(x =>
            //{
            //    x.ConnectionString = Configuration.GetConnectionString("NHibernateFundConnectionString");
            //    x.Driver<SqlClientDriver>();
            //    x.Dialect<MsSql2012Dialect>();
            //    x.LogSqlInConsole = true;
            //    //x.BatchSize = 10;
            //});
            //cfg.AddAssembly(Assembly.GetExecutingAssembly());

            //var sessionFactory = cfg.BuildSessionFactory();

            var sessionFactory = CreateSessionFactory();

            var customer = CreateCustomer(sessionFactory);

            LoadCustomer(sessionFactory, customer.Id);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        private ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
              .Database(
                MsSqlConfiguration.MsSql2012
                  .ConnectionString(Configuration.GetConnectionString("NHibernateFundConnectionString"))
              )
              .Mappings(m =>
                m.FluentMappings.AddFromAssemblyOf<Program>())
              .BuildSessionFactory();
        }

        private Customer CreateCustomer(ISessionFactory sessionFactory)
        {
            Customer customer;
            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    customer = NewCustomer();
                    Console.WriteLine("*** NEW CUSTOMER [BEFORE] ***");
                    Console.WriteLine(customer);
                    session.Save(customer);
                    foreach (var order in customer.Orders)
                    {
                        order.Customer = customer;
                        session.Save(order);
                    }
                    
                    tx.Commit();
                }
            }

            return customer;
        }

        private IEnumerable<Customer> LoadCustomers(ISessionFactory sessionFactory)
        {
            IEnumerable<Customer> customers;
            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    customers = session.Query<Customer>()
                        .Where(x => x.Points > 0)
                        .OrderBy(x => x.Points)
                        .Select(x => x);

                    Console.WriteLine($"***** CUSTOMERS LOADED *****\n");
                    foreach (var c in customers)
                    {
                        Console.WriteLine($"{c}\n");
                    }
                }
            }

            return customers;
        }

        private Customer LoadCustomer(ISessionFactory sessionFactory, Guid id)
        {
            Customer customer;
            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    customer = session.Load<Customer>(id);
                    //Console.WriteLine("*** CUSTOMER RELOADED ***");
                    //Console.WriteLine(customer);

                    Console.WriteLine("*** The orders are of: ***");
                    var customerName = (customer.Orders != null) ? customer.Orders.First().Customer.FirstName : "no orders associated";
                    Console.WriteLine(customerName);
                }
            }

            return customer;
        }

        private Customer NewCustomer()
        {
            return new Customer
            {
                FirstName = "John",
                LastName = "Augusto",
                MemberSince = DateTime.UtcNow,
                Points = 90,
                HasGoldStatus = false,
                CreditRating = CustomerCreditRating.Neutral,
                Address = NewLocation(),
                Orders =
                {
                    new Order(),
                    new Order
                    {
                        Shipped = DateTime.UtcNow,
                        ShipTo = NewLocation()
                    }
                }
            };
        }

        private LocationValueObject NewLocation()
        {
            return new LocationValueObject
            {
                Street = "Rua Dr. Paulo Fróes Machado, 160",
                City = "Nova Iguaçu",
                Province = "RJ",
                Country = "BR"
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
