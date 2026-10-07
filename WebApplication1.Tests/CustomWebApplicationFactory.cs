using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WebApplication1.Data;
using WebApplication1.Models;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;

namespace WebApplication1.Tests
{

    public class CustomWebApplicationFactory : WebApplicationFactory<Program>//test version tal app, qed ninheritjaw l app il vera u qed inbiddluha biex
        //tuza in memory databse flok l sql li tuza l app normali biex nitestjaw
    {

        private readonly string _databaseName = Guid.NewGuid().ToString();//guid new guid generates something unique ez 10b718ea-d15f-42b6-91df-bc21f7234824
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }


                services.AddDbContext<AppDbContext>(options =>
                {
                    //options.UseInMemoryDatabase("IntegrationTestDb");
                    options.UseInMemoryDatabase(_databaseName);//database shouldnt be the same for each test, jitgerfxu bvaluri ta tests ohra
                });
            });


            //flok hallejna dawk il qabda lines ghal kull test biex nikkrejaw
            //inbiddlu d database tal program, hekk ghandna class diga lesta u nistu nuzawa ghal kull test


        }
    }
}
