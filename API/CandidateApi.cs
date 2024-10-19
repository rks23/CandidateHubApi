namespace CandidateHubApi.API
{
    public static class CandidateApi
    {
        public static void MapCandidateApi(this WebApplication app)
        {
            var cand = app.MapGroup("/api/candidate");
            cand.MapGet("/{id}", (int id) =>
            {

            });
        }
    }
}
