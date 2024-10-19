using CandidateHubApi.Context;
using CandidateHubApi.Repository;

namespace CandidateHubApi.API
{
    public static class CandidateApi
    {
        public static void MapCandidateApi(this WebApplication app)
        {
            var cand = app.MapGroup("/api/candidate");

            //
            cand.MapGet("/{id}", async (int id, CandidateRepository repo) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest("Invalid ID");
                }

                var result = await repo.GetByIdAsync(id);
                if (result is null) return Results.NotFound();
                return Results.Ok(result);
            });

            cand.MapGet("/", async (CandidateRepository repo, int pageNo = 1, int pageSize = 10) =>
            {
                var results = await repo.GetAllAsync(pageNo, pageSize);
                if (results is null) return Results.NotFound();
                return Results.Ok(results);
            });

            cand.MapPost("/", async (Candidate data, CandidateRepository repo) =>
            {
                await repo.AddAsync(data);
                return Results.Ok();
            });

            cand.MapPut("/", async (Candidate data, CandidateRepository repo) =>
            {
                await repo.UpdateAsync(data);
                return Results.Ok();
            });

            cand.MapDelete("/{id}", async Task<IResult> (int id, CandidateRepository repo) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest("Invalid ID");
                }

                var result = await repo.DeleteAsync(id);
                if (!result) return Results.NotFound();
                return Results.NoContent();
            });
        }
    }
}
