using CandidateHubApi.Context;
using CandidateHubApi.Repository;
using Microsoft.Extensions.Caching.Distributed;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace CandidateHubApi.API
{
    public static class CandidateApi
    {
        public static void MapCandidateApi(this WebApplication app)
        {
            const string grpPrefix = "/api/candidate";
            var cand = app.MapGroup(grpPrefix);

            //
            cand.MapGet("/{id}", async (int id, CandidateRepository repo, IDistributedCache cache) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest("Invalid ID");
                }

                //get from cache
                var cacheKey = $"candidate::{id}";
                var cd = await cache.GetStringAsync(cacheKey);
                if (!string.IsNullOrWhiteSpace(cd))
                {
                    var cachedData = JsonSerializer.Deserialize<Candidate>(cd);
                    return Results.Ok(cachedData);
                }

                //cache miss
                var result = await repo.GetByIdAsync(id);
                if (result is null) return Results.NotFound();
                await AddToCache(cacheKey, result, cache);
                return Results.Ok(result);
            });

            cand.MapGet("/", async (HttpContext context, CandidateRepository repo, IDistributedCache cache,
                int pageNumber = 1, int pageSize = 10) =>
            {

                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1 || pageSize > 50) pageSize = 10;

                var total = await repo.GetTotalCountAsync();
                // Create a metadata object (optional) to return with the paginated data
                var paginationMetadata = new
                {
                    totalCount = total,
                    pageSize,
                    currentPage = pageNumber,
                    totalPages = (int)Math.Ceiling((double)total / pageSize),
                    hasNextPage = pageNumber < (int)Math.Ceiling((double)total / pageSize),
                    hasPreviousPage = pageNumber > 1
                };

                // Return paginated data along with pagination metadata in response headers (optional)
                context.Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

                var results = await repo.GetAllAsync(pageNumber, pageSize);
                if (results is null) return Results.NotFound();
                return Results.Ok(results);
            });

            cand.MapPost("/", async (Candidate data, CandidateRepository repo) =>
            {
                var validationResults = new List<ValidationResult>();
                var validationCtx = new ValidationContext(data);
                if (!Validator.TryValidateObject(data, validationCtx, validationResults, true))
                {
                    return Results.BadRequest(validationResults);
                }

                var candidateByEmail = await repo.GetByFilterAsync(c => c.Email == data.Email);
                if (candidateByEmail is not null && candidateByEmail.Any())
                {
                    validationResults.Add(new ValidationResult($"Candidate with email " +
                        $"{data.Email} already exists!", [nameof(data.Email)]));
                    return Results.BadRequest(validationResults);
                }

                await repo.AddAsync(data);
                return Results.Created($"{grpPrefix}/{data.CandidateId}", data);
            });

            cand.MapPut("/{id}", async (int id, Candidate data,
                CandidateRepository repo, IDistributedCache cache) =>
            {
                if (id <= 0 || id != data.CandidateId)
                {
                    return Results.BadRequest("Invalid ID");
                }
                var cp = repo.GetByIdAsync(id);
                if (cp is null) return Results.NotFound();

                await repo.UpdateAsync(data);
                await RemoveFromCache(id, cache);
                return Results.NoContent();
            });

            cand.MapDelete("/{id}", async Task<IResult> (int id,
                CandidateRepository repo, IDistributedCache cache) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest("Invalid ID");
                }

                var result = await repo.DeleteAsync(id);
                if (!result) return Results.NotFound();
                await RemoveFromCache(id, cache);
                return Results.NoContent();
            });
        }

        private static async Task RemoveFromCache(int id, IDistributedCache cache)
        {
            var cacheKey = $"candidate::{id}";
            await cache.RemoveAsync(cacheKey);
        }

        private static async Task AddToCache<T>(string cacheKey, T data, IDistributedCache cache)
        {
            var json = JsonSerializer.Serialize(data);
            await cache.SetStringAsync(cacheKey, json);
        }
    }
}
