using CandidateHubApi.API;
using CandidateHubApi.Context;
using CandidateHubApi.Middleware;
using CandidateHubApi.Repository;
using Microsoft.EntityFrameworkCore;

public class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var cs = builder.Configuration.GetConnectionString("CandidateHubDB");
        builder.Services.AddDbContext<CandidateHubContext>(opts => opts.UseNpgsql(cs));

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<CandidateRepository>();

        builder.Services.AddDistributedMemoryCache();

        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ExceptionMiddleware>();

        app.MapCandidateApi();
        app.UseHttpsRedirection();
        app.Run();
    }
}