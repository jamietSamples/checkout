using Checkout.PaymentGateway.Acquirer.Services;
using Checkout.PaymentGateway.Payment.Api.Authentication;
using Checkout.PaymentGateway.Payment.Api.Swagger;
using Checkout.PaymentGateway.Payment.Application.Services;
using Checkout.PaymentGateway.Payment.Domain.AggregatesModel.PaymentAggregate;
using Checkout.PaymentGateway.Payment.Domain.AggregatesModel.PaymentMethodAggregate;
using Checkout.PaymentGateway.Payment.Domain.Commands;
using Checkout.PaymentGateway.Payment.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace Checkout.PaymentGateway.Payment.Api
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
            services.AddControllers();
            services.AddDbContext<PaymentContext>(options => options.UseInMemoryDatabase("PaymentDatabase"));
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IAcquirerClientService, AcquirerClientService>();
            services.AddMediatR(Assembly.GetAssembly(typeof(CreatePaymentCommand)));
            services.AddAuthentication(ApiKeyAuthenticationOptions.DefaultScheme)
                .AddScheme<ApiKeyAuthenticationOptions,ApiKeyAuthenticationHandler>(ApiKeyAuthenticationOptions.DefaultScheme, auth => { });

            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<AddHeaderFilter>();
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "Checkout.PaymentGateway.Payment.Api",
                    Version = "v1",
                    Description = "A basic example of a payment gateway in which you can submit and retireve payment details. \r\n There is also the option of querying payment methods in advance to check amount and currency against a defined list \r\n The API key is: specialKey123"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Checkout.PaymentGateway.Payment.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
