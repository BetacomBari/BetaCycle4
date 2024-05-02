
using BetaCycle4.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WebAca5CodeFirst.Logic.Autentication.Basic;

namespace BetaCycle4
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            
            builder.Services.AddEndpointsApiExplorer();

            //AUTHENTICATION
            builder.Services.AddAuthentication()
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", opt => { });
            builder.Services.AddAuthorization(opts =>
            {
                opts.AddPolicy("BasicAuthentication", new AuthorizationPolicyBuilder("BasicAuthentication")
                .RequireAuthenticatedUser().Build());
            });

            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<AdventureWorksLt2019Context>
              (opt => opt.UseSqlServer(
                  builder.Configuration.GetConnectionString("Comics")));
            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy",
                    builder => builder
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed((hosts) => true));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}


