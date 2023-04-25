using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VotingApplication.Database;
using VotingApplication.Services;

namespace VotingApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string _policyName = "CorsPolicy";
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
            });
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("SMARTERASPDB"));
            });
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IAspirantRepository, AspirantRepositorySQL>();
            builder.Services.AddScoped<ITokenRepository, TokenRepositorySQL>();
            builder.Services.AddScoped<IRolesRepository, RolesRepositorySQL>();
            builder.Services.AddScoped<IVoteCountRepository, VoteCountRepositorySQL>();
            builder.Services.AddScoped<IUsersRepository, UsersRepositorySQL>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: _policyName, builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });

            });

            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                            .AddEntityFrameworkStores<AppDbContext>();

            var app = builder.Build();

            //// Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Voting App V1");
                    options.RoutePrefix = String.Empty;
                });
            }

            // Configure the HTTP request pipeline.

            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
                
            //}


            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseCors(_policyName);
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}